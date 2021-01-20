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

        public async Task<IActionResult> OrderArrived(int id)
        {
            var order = await _context.Orders.Where(o => o.Id == id).FirstOrDefaultAsync();
            order.OrderStatusId = 3;
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();

            return RedirectToAction("MyOrders");
        }

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

        public async Task<IActionResult> MyOrders()
        {
            var orders = await _context.Orders.Include(o => o.OrderStatus).Where(o => o.CustomerId == _userManager.GetUserId(User)).ToListAsync();

            return View(orders);
        }

        // GET: Orders
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Orders.Include(o => o.Address).Include(o => o.Customer).Include(o => o.OrderStatus).Include(o => o.Seller);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Orders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o.Address)
                .Include(o => o.Customer)
                .Include(o => o.OrderStatus)
                .Include(o => o.Seller)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // GET: Orders/Create
        public IActionResult Create()
        {
            ViewData["AddressId"] = new SelectList(_context.Addresses, "Id", "Id");
            ViewData["CustomerId"] = new SelectList(_context.Users, "Id", "Id");
            ViewData["OrderStatusId"] = new SelectList(_context.OrderStatuses, "Id", "Name");
            ViewData["SellerId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,OrderStatusId,AddressId,SellerId,CustomerId,StartDate,EndDate,Cart")] Order order)
        {
            if (ModelState.IsValid)
            {
                _context.Add(order);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AddressId"] = new SelectList(_context.Addresses, "Id", "Id", order.AddressId);
            ViewData["CustomerId"] = new SelectList(_context.Users, "Id", "Id", order.CustomerId);
            ViewData["OrderStatusId"] = new SelectList(_context.OrderStatuses, "Id", "Name", order.OrderStatusId);
            ViewData["SellerId"] = new SelectList(_context.Users, "Id", "Id", order.SellerId);
            return View(order);
        }

        // GET: Orders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            ViewData["AddressId"] = new SelectList(_context.Addresses, "Id", "Id", order.AddressId);
            ViewData["CustomerId"] = new SelectList(_context.Users, "Id", "Id", order.CustomerId);
            ViewData["OrderStatusId"] = new SelectList(_context.OrderStatuses, "Id", "Name", order.OrderStatusId);
            ViewData["SellerId"] = new SelectList(_context.Users, "Id", "Id", order.SellerId);
            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,OrderStatusId,AddressId,SellerId,CustomerId,StartDate,EndDate,Cart")] Order order)
        {
            if (id != order.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(order);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["AddressId"] = new SelectList(_context.Addresses, "Id", "Id", order.AddressId);
            ViewData["CustomerId"] = new SelectList(_context.Users, "Id", "Id", order.CustomerId);
            ViewData["OrderStatusId"] = new SelectList(_context.OrderStatuses, "Id", "Name", order.OrderStatusId);
            ViewData["SellerId"] = new SelectList(_context.Users, "Id", "Id", order.SellerId);
            return View(order);
        }

        // GET: Orders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o.Address)
                .Include(o => o.Customer)
                .Include(o => o.OrderStatus)
                .Include(o => o.Seller)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.Id == id);
        }
    }
}
