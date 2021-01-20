using GadgetStore.Data;
using GadgetStore.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using GadgetStore.ViewModels;

namespace GadgetStore.Controllers
{
    public class MyCartController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        public MyCartController(ApplicationDbContext context, UserManager<User> userManager)
        {
            _userManager = userManager;
            _context = context;
        }

        public IActionResult Delivery()
        {
            Address address = HttpContext.Session.Get<Address>("address");

            if (address != null)
            {
                DelivaryViewModel model = new DelivaryViewModel
                {
                    Hous = address.Hous,
                    Apartment = address.Apartment,
                    City = address.City,
                    PhoneNumber = address.PhoneNumber,
                    Porch = address.Porch,
                    Street = address.Street
                };

                return View(model);
            }

            return View();
        }

        [HttpPost]
        public IActionResult Delivery(DelivaryViewModel model)
        {
            if (ModelState.IsValid)
            {
                Address address = new Address
                {
                    Hous = model.Hous,
                    Apartment = model.Apartment,
                    City = model.City,
                    PhoneNumber = model.PhoneNumber,
                    Porch = model.Porch,
                    Street = model.Street
                };

                HttpContext.Session.Set<Address>("address", address);

                if (model.PayType == 1)
                {
                    return View("CreditCardPay");
                }
                else if (model.PayType == 2)
                {
                    return View("YandexMoneyPay");
                }
                else
                {
                    return NotFound();
                }
            }

            return View(model);
        }

        public async Task<IActionResult> Pay()
        {
            Cart cart = HttpContext.Session.Get<Cart>("cart");
            foreach(CartLine item in cart.Lines())
            {
                Gadget gadget = _context.Gadgets.Where(g => g.Id == item.Gadget.Id).FirstOrDefault();
                if (gadget != null)
                {
                    gadget.Count -= item.Count;
                    _context.Gadgets.Update(gadget);
                }

                await _context.SaveChangesAsync();
            }

            Address address = HttpContext.Session.Get<Address>("address");

            _context.Addresses.Add(address);
            await _context.SaveChangesAsync();
            var user = await _userManager.GetUserAsync(User);
            Order order = new Order
            {
                AddressId = address.Id,
                OrderStatusId = 1,
                Customer = user,
                StartDate = DateTime.Now,
                Cart = HttpContext.Session.GetSessionString("cart")
            };

            _context.Orders.Add(order);

            await _context.SaveChangesAsync();

            HttpContext.Session.Set<Cart>("cart", null);

            return View("PayInfo", order.Id);
        }

        public IActionResult Index()
        {
            Cart cart = HttpContext.Session.Get<Cart>("cart");
            return View(cart);
        }

        public IActionResult Add(int Id)
        {
            Cart cart = HttpContext.Session.Get<Cart>("cart") ?? new Cart();
            
            Gadget gadget = _context.Gadgets.Where(g => g.Id == Id).FirstOrDefault();

            if (gadget != null)
            {
                cart.AddItem(gadget);
                HttpContext.Session.Set<Cart>("cart", cart);
                Cart cart2 = HttpContext.Session.Get<Cart>("cart") ?? new Cart();

                return PartialView(true);
            }

            return PartialView(false);
        }

        public IActionResult Remove(int Id)
        {
            Cart cart = HttpContext.Session.Get<Cart>("cart") ?? new Cart();
            
            Gadget gadget = _context.Gadgets.Where(g => g.Id == Id).FirstOrDefault();

            if (gadget != null)
            {
                cart.RemoveItem(gadget);
                //доделать
                HttpContext.Session.Set<Cart>("cart", cart);
            }
            
            return RedirectToAction(nameof(Index));
        }
    }
}
