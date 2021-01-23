using GadgetStore.Data;
using GadgetStore.Models;
using GadgetStore.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace GadgetStore.Controllers
{
    [Authorize(Roles = "admin")]
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public UsersController(ApplicationDbContext context, UserManager<User> userManager)
        {
            _userManager = userManager;
            _context = context;
        }

        public IActionResult Index() => View(_userManager.Users.Where(u => u.UserName != "unknow").ToList());

        public ActionResult Create()
        {
            ViewData["Roles"] = new SelectList(new List<string>{ "admin", "seller", "customer" });
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = new User
                {
                    UserName = model.UserName,
                    FirstName = model.FirstName,
                    SecondName = model.SecondName
                };

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, model.Role);

                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }

            ViewData["Roles"] = new SelectList(new List<string> { "admin", "seller", "customer" });
            return View(model);
        }

        public async Task<IActionResult> Edit(string id)
        {
            EditUserViewModel model;
            User user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            var role = await _userManager.GetRolesAsync(user);

            model = new EditUserViewModel
            {
                Id = user.Id,
                UserName = user.UserName,
                FirstName = user.FirstName,
                SecondName = user.SecondName,
                Role = role.First()
            };

            ViewData["Roles"] = new SelectList(new List<string> { "admin", "seller", "customer" }, model.Role);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await _userManager.FindByIdAsync(model.Id);

                if (user == null)
                {
                    return NotFound();
                }

                user.FirstName = model.FirstName;
                user.SecondName = model.SecondName;
                user.UserName = model.UserName;

                var updateResult = await _userManager.UpdateAsync(user);

                if (updateResult.Succeeded)
                {
                    if (model.Password != null)
                    {
                            await _userManager.RemovePasswordAsync(user);
                            await _userManager.AddPasswordAsync(user, model.Password);
                    }

                    await _userManager.AddToRoleAsync(user, model.Role);

                    return RedirectToAction(nameof(Index)); 
                }
                else
                {
                    foreach (var error in updateResult.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }

            ViewData["Roles"] = new SelectList(new List<string> { "admin", "seller", "customer" });
            return View(model);
        }

        public async Task<ActionResult> Delete(string id)
        {
            User user = await _userManager.FindByIdAsync(id);
            User unknow = await _userManager.FindByNameAsync("unknow");

            if (user == null)
            {
                return NotFound();
            }

            var orders = await _context.Orders.Where(o => o.CustomerId == user.Id).ToListAsync();

            foreach (var item in orders)
            {
                item.CustomerId = unknow.Id;
            }

            _context.Orders.UpdateRange(orders);
            await _context.SaveChangesAsync();

            orders = await _context.Orders.Where(o => o.SellerId == user.Id).ToListAsync();
            
            foreach (var item in orders)
            {
                item.CustomerId = unknow.Id;
            }

            _context.Orders.UpdateRange(orders);
            await _context.SaveChangesAsync();

            IdentityResult result = await _userManager.DeleteAsync(user);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

    }
}
