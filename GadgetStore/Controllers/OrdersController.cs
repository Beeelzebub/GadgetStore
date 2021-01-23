using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GadgetStore.Data;
using GadgetStore.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace GadgetStore.Controllers
{
    public class OrdersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public OrdersController(ApplicationDbContext context, UserManager<User> userManager)
        {
            _userManager = userManager;
            _context = context;
        }

        [Authorize(Roles = "seller")]
        public async Task<IActionResult> SendOrder(int id)
        {
            var order = await _context.Orders.Where(o => o.Id == id).FirstOrDefaultAsync();
            order.OrderStatusId = 2;
            order.SellerId = _userManager.GetUserId(User);
            order.EndDate = DateTime.Now;
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();

            return RedirectToAction("ActiveOrders");
        }

        [Authorize(Roles = "seller")]
        public async Task<IActionResult> ActiveOrders()
        {
            var orders = await _context.Orders
                .Include(o => o.OrderStatus)
                .Include(o => o.Address)
                .Include(o => o.Customer)
                .Where(o => o.OrderStatusId == 1)
                .ToListAsync();

            return View(orders);
        }

        [Authorize]
        public async Task<IActionResult> OrderArrived(int id)
        {
            var order = await _context.Orders.Where(o => o.Id == id).FirstOrDefaultAsync();
            order.OrderStatusId = 3;
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();

            return RedirectToAction("MyOrders");
        }

        [Authorize(Roles = "seller")]
        public async Task<IActionResult> History()
        {
            var orders = await _context.Orders
                .Include(o => o.OrderStatus)
                .Include(o => o.Address)
                .Include(o => o.Customer)
                .Include(o => o.Seller)
                .Where(o => o.OrderStatusId != 1)
                .ToListAsync();

            return View(orders);
        }


        [Authorize]
        public async Task<IActionResult> MyOrders()
        {
            var orders = await _context.Orders.Include(o => o.OrderStatus).Where(o => o.CustomerId == _userManager.GetUserId(User)).ToListAsync();

            return View(orders);
        }
    }
}
