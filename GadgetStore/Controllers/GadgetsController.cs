using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GadgetStore.Data;
using GadgetStore.Models;
using GadgetStore.ViewModels;
using System.IO;
using Microsoft.AspNetCore.Authorization;

namespace GadgetStore.Controllers
{
    public class GadgetsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public GadgetsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var manufacturers = await _context.Manufacturers.ToListAsync();
            manufacturers.Add(new Manufacturer { Id = 0, Name = "Любой" });
            manufacturers.Reverse();

            var gadgetType = await _context.gadgetTypes.ToListAsync();
            gadgetType.Add(new GadgetType { Id = 0, Name = "Любой" });
            gadgetType.Reverse();

            var diagonals = await _context.Diagonals.ToListAsync();
            diagonals.Add(new Diagonal { Id = 0, Value = "Любая" });
            diagonals.Reverse();

            var screenResolutions = await _context.ScreenResolutions.ToListAsync();
            screenResolutions.Add(new ScreenResolution { Id = 0, Value = "Любое" });
            screenResolutions.Reverse();

            var colors = await _context.Colors.ToListAsync();
            colors.Add(new Color { Id = 0, Name = "Любой" });
            colors.Reverse();

            var CPUs = await _context.CPUs.ToListAsync();
            CPUs.Add(new CPU { Id = 0, Name = "Любой" });
            CPUs.Reverse();

            var Memory = await _context.Gadgets.Select(g => g.Memory).Distinct().ToListAsync();
            Memory.Add("Любая");
            Memory.Reverse();

            var RAM = await _context.Gadgets.Select(g => g.RAM).Distinct().ToListAsync();
            RAM.Add("Любая");
            RAM.Reverse();

            ViewData["Manufacturers"] = new SelectList(manufacturers, "Id", "Name", 0);
            ViewData["GadgetTypes"] = new SelectList(gadgetType, "Id", "Name", 0);
            ViewData["Diagonals"] = new SelectList(diagonals, "Id", "Value", 0);
            ViewData["ScreenResolutions"] = new SelectList(screenResolutions, "Id", "Value", 0);
            ViewData["Colors"] = new SelectList(colors, "Id", "Name", 0);
            ViewData["CPUs"] = new SelectList(CPUs, "Id", "Name", 0);
            ViewData["Memory"] = new SelectList(Memory, 0);
            ViewData["RAM"] = new SelectList(RAM, 0);


            var applicationDbContext = _context.Gadgets
                .Include(g => g.Picture)
                .Include(g => g.CPU)
                .Include(g => g.Color)
                .Include(g => g.Diagonal)
                .Include(g => g.GadgetType)
                .Include(g => g.Manufacturer)
                .Include(g => g.ScreenResolution);
            
            return View(await applicationDbContext.ToListAsync());
        }

        public async Task<IActionResult> Create()
        {
            ViewData["GadgetTypeId"] = new SelectList(_context.gadgetTypes, "Id", "Name");
            ViewData["Manufacturers"] = await _context.Manufacturers.Select(m => m.Name).ToListAsync();
            ViewData["Diagonals"] = await _context.Diagonals.Select(d => d.Value).ToListAsync();
            ViewData["ScreenResolutions"] = await _context.ScreenResolutions.Select(s => s.Value).ToListAsync();
            ViewData["Colors"] = await _context.Colors.Select(c => c.Name).ToListAsync();
            ViewData["CPUs"] = await _context.CPUs.Select(c => c.Name).ToListAsync();
            ViewData["Year"] = await _context.Gadgets.Select(g => g.Year).Distinct().ToArrayAsync();
            ViewData["Memory"] = await _context.Gadgets.Select(g => g.Memory).Distinct().ToArrayAsync();
            ViewData["RAM"] = await _context.Gadgets.Select(g => g.RAM).Distinct().ToArrayAsync();

            return View();
        }

        [HttpPost]
        [Authorize(Roles = "seller")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(GadgetViewModel model)
        {
            if (ModelState.IsValid)
            {
                byte[] imageData = null;

                if (model.Image != null)
                {
                    // считываем переданный файл в массив байтов
                    using (var binaryReader = new BinaryReader(model.Image.OpenReadStream()))
                    {
                        imageData = binaryReader.ReadBytes((int)model.Image.Length);
                    }
                }

                Picture picture = new Picture { Image = imageData };

                CPU cpu = await _context.CPUs
                    .Where(c => c.Name.ToLower() == model.CPU.ToLower())
                    .FirstOrDefaultAsync();
                if (cpu == null)
                {
                    cpu = new CPU
                    {
                        Name = model.CPU
                    };

                    await _context.CPUs.AddAsync(cpu);
                }

                Diagonal diagonal = await _context.Diagonals
                    .Where(d => d.Value.ToLower() == model.Diagonal.ToLower())
                    .FirstOrDefaultAsync();
                if (diagonal == null)
                {
                    diagonal = new Diagonal
                    {
                        Value = model.Diagonal
                    };

                    await _context.Diagonals.AddAsync(diagonal);
                }

                ScreenResolution screenResolution = await _context.ScreenResolutions
                    .Where(s => s.Value.ToLower() == model.ScreenResolution.ToLower())
                    .FirstOrDefaultAsync();
                if (screenResolution == null)
                {
                    screenResolution = new ScreenResolution
                    {
                        Value = model.ScreenResolution
                    };

                    await _context.ScreenResolutions.AddAsync(screenResolution);
                }

                Color color = await _context.Colors
                    .Where(c => c.Name.ToLower() == model.Color.ToLower())
                    .FirstOrDefaultAsync();
                if (color == null)
                {
                    color = new Color
                    {
                        Name = model.Color
                    };

                    await _context.Colors.AddAsync(color);
                }

                Manufacturer manufacturer = await _context.Manufacturers
                    .Where(m => m.Name.ToLower() == model.Manufacturer.ToLower())
                    .FirstOrDefaultAsync();
                if (manufacturer == null)
                {
                    manufacturer = new Manufacturer
                    {
                        Name = model.Manufacturer
                    };

                    await _context.Manufacturers.AddAsync(manufacturer);
                }


                await _context.SaveChangesAsync();

                Gadget gadget = new Gadget
                {
                    Picture = picture,
                    CPUId = cpu.Id,
                    DiagonalId = diagonal.Id,
                    ScreenResolutionId = screenResolution.Id,
                    ColorId = color.Id,
                    ManufacturerId = manufacturer.Id,
                    Year = model.Year,
                    Count = model.Count,
                    Memory = model.Memory,
                    RAM = model.RAM,
                    GadgetTypeId = model.GadgetTypeId,
                    Name = model.Name,
                    Price = model.Price
                };

                _context.Add(gadget);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Management));
            }

            ViewData["GadgetTypeId"] = new SelectList(_context.gadgetTypes, "Id", "Name");
            ViewData["Manufacturers"] = await _context.Manufacturers.Select(m => m.Name).ToListAsync();
            ViewData["Diagonals"] = await _context.Diagonals.Select(d => d.Value).ToListAsync();
            ViewData["ScreenResolutions"] = await _context.ScreenResolutions.Select(s => s.Value).ToListAsync();
            ViewData["Colors"] = await _context.Colors.Select(c => c.Name).ToListAsync();
            ViewData["CPUs"] = await _context.CPUs.Select(c => c.Name).ToListAsync();
            ViewData["Year"] = await _context.Gadgets.Select(g => g.Year).Distinct().ToArrayAsync();
            ViewData["Memory"] = await _context.Gadgets.Select(g => g.Memory).Distinct().ToArrayAsync();
            ViewData["RAM"] = await _context.Gadgets.Select(g => g.RAM).Distinct().ToArrayAsync();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Search(string searchName)
        {
            if (searchName == null)
            {
                searchName = "";
            }
            var gadgets = await _context.Gadgets
                .Include(g => g.Picture)
                .Include(g => g.CPU)
                .Include(g => g.Color)
                .Include(g => g.Diagonal)
                .Include(g => g.GadgetType)
                .Include(g => g.Manufacturer)
                .Include(g => g.ScreenResolution)
                .Where(g => g.Name.Contains(searchName))
                .ToListAsync();

            return PartialView("Assortment", gadgets);
        }


        [Authorize(Roles = "seller")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gadget = await _context.Gadgets
                .Include(g => g.Picture)
                .Include(g => g.CPU)
                .Include(g => g.Color)
                .Include(g => g.Diagonal)
                .Include(g => g.GadgetType)
                .Include(g => g.Manufacturer)
                .Include(g => g.ScreenResolution)
                .Where(g => g.Id == id)
                .FirstOrDefaultAsync();

            if (gadget == null)
            {
                return NotFound();
            }

            GadgetViewModel model = new GadgetViewModel
            {
                Name = gadget.Name,
                Year = gadget.Year,
                Price = gadget.Price,
                Count = gadget.Count,
                Memory = gadget.Memory,
                RAM = gadget.RAM,
                Diagonal = gadget.Diagonal.Value,
                ScreenResolution = gadget.ScreenResolution.Value,
                Color = gadget.Color.Name,
                CPU = gadget.CPU.Name,
                Manufacturer = gadget.Manufacturer.Name
            };

            ViewData["GadgetTypeId"] = new SelectList(_context.gadgetTypes, "Id", "Name", gadget.GadgetType);
            ViewData["Manufacturers"] = await _context.Manufacturers.Select(m => m.Name).ToListAsync();
            ViewData["Diagonals"] = await _context.Diagonals.Select(d => d.Value).ToListAsync();
            ViewData["ScreenResolutions"] = await _context.ScreenResolutions.Select(s => s.Value).ToListAsync();
            ViewData["Colors"] = await _context.Colors.Select(c => c.Name).ToListAsync();
            ViewData["CPUs"] = await _context.CPUs.Select(c => c.Name).ToListAsync();
            ViewData["Year"] = await _context.Gadgets.Select(g => g.Year).Distinct().ToArrayAsync();
            ViewData["Memory"] = await _context.Gadgets.Select(g => g.Memory).Distinct().ToArrayAsync();
            ViewData["RAM"] = await _context.Gadgets.Select(g => g.RAM).Distinct().ToArrayAsync();
            ViewData["Id"] = gadget.Id;

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Filter(
            [Bind("Year", "Memory", "RAM", "GadgetType", "Manufacturer", "Diagonal", "ScreenResolution", "Color", "CPU")] FilterViewModel model)
        {
            var gadgets = await _context.Gadgets
                .Include(g => g.Picture)
                .Include(g => g.CPU)
                .Include(g => g.Color)
                .Include(g => g.Diagonal)
                .Include(g => g.GadgetType)
                .Include(g => g.Manufacturer)
                .Include(g => g.ScreenResolution)
                .ToListAsync();

            if (model.GadgetType != 0)
            {
                gadgets = gadgets.Where(g => g.GadgetTypeId == model.GadgetType).ToList();
            }
            if (model.Memory != "Любая")
            {
                gadgets = gadgets.Where(g => g.Memory.Contains(model.Memory)).ToList();
            }
            if (model.RAM != "Любая")
            {
                gadgets = gadgets.Where(g => g.RAM.Contains(model.RAM)).ToList();
            }
            if (model.Manufacturer != 0)
            {
                gadgets = gadgets.Where(g => g.ManufacturerId == model.Manufacturer).ToList();
            }
            if (model.Diagonal != 0)
            {
                gadgets = gadgets.Where(g => g.DiagonalId == model.Diagonal).ToList();
            }
            if (model.ScreenResolution != 0)
            {
                gadgets = gadgets.Where(g => g.ScreenResolutionId == model.ScreenResolution).ToList();
            }
            if (model.Color != 0)
            {
                gadgets = gadgets.Where(g => g.ColorId == model.Color).ToList();
            }
            if (model.CPU != 0)
            {
                gadgets = gadgets.Where(g => g.CPUId == model.CPU).ToList();
            }

            return PartialView("Assortment", gadgets);
        }

        [HttpPost]
        [Authorize(Roles = "seller")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? Id, GadgetViewModel model)
        {
            if (Id == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                byte[] imageData = null;
                Picture picture = null;
                
                if (model.Image != null)
                {
                    using (var binaryReader = new BinaryReader(model.Image.OpenReadStream()))
                    {
                        imageData = binaryReader.ReadBytes((int)model.Image.Length);
                    }

                    picture = new Picture { Image = imageData };
                    _context.Pictures.Add(picture);
                }

                CPU cpu = await _context.CPUs
                    .Where(c => c.Name.ToLower() == model.CPU.ToLower())
                    .FirstOrDefaultAsync();
                if (cpu == null)
                {
                    cpu = new CPU
                    {
                        Name = model.CPU
                    };

                    await _context.CPUs.AddAsync(cpu);
                }

                Diagonal diagonal = await _context.Diagonals
                    .Where(d => d.Value.ToLower() == model.Diagonal.ToLower())
                    .FirstOrDefaultAsync();
                if (diagonal == null)
                {
                    diagonal = new Diagonal
                    {
                        Value = model.Diagonal
                    };

                    await _context.Diagonals.AddAsync(diagonal);
                }

                ScreenResolution screenResolution = await _context.ScreenResolutions
                    .Where(s => s.Value.ToLower() == model.ScreenResolution.ToLower())
                    .FirstOrDefaultAsync();
                if (screenResolution == null)
                {
                    screenResolution = new ScreenResolution
                    {
                        Value = model.ScreenResolution
                    };

                    await _context.ScreenResolutions.AddAsync(screenResolution);
                }

                Color color = await _context.Colors
                    .Where(c => c.Name.ToLower() == model.Color.ToLower())
                    .FirstOrDefaultAsync();
                if (color == null)
                {
                    color = new Color
                    {
                        Name = model.Color
                    };

                    await _context.Colors.AddAsync(color);
                }

                Manufacturer manufacturer = await _context.Manufacturers
                    .Where(m => m.Name.ToLower() == model.Manufacturer.ToLower())
                    .FirstOrDefaultAsync();
                if (manufacturer == null)
                {
                    manufacturer = new Manufacturer
                    {
                        Name = model.Manufacturer
                    };

                    await _context.Manufacturers.AddAsync(manufacturer);
                }

                await _context.SaveChangesAsync();

                Gadget gadget = await _context.Gadgets.FindAsync(Id);

                if (gadget == null)
                {
                    return NotFound();
                }

                if (picture != null)
                {
                    gadget.PictureId = picture.Id;
                }
                gadget.CPUId = cpu.Id;
                gadget.DiagonalId = diagonal.Id;
                gadget.ScreenResolutionId = screenResolution.Id;
                gadget.ColorId = color.Id;
                gadget.ManufacturerId = manufacturer.Id;
                gadget.Year = model.Year;
                gadget.Count = model.Count;
                gadget.Memory = model.Memory;
                gadget.RAM = model.RAM;
                gadget.GadgetTypeId = model.GadgetTypeId;
                gadget.Name = model.Name;
                gadget.Price = model.Price;

                _context.Update(gadget);
                await _context.SaveChangesAsync();


                return RedirectToAction(nameof(Management));
            }

            ViewData["GadgetTypeId"] = new SelectList(_context.gadgetTypes, "Id", "Name", model.GadgetTypeId);
            ViewData["Manufacturers"] = await _context.Manufacturers.Select(m => m.Name).ToListAsync();
            ViewData["Diagonals"] = await _context.Diagonals.Select(d => d.Value).ToListAsync();
            ViewData["ScreenResolutions"] = await _context.ScreenResolutions.Select(s => s.Value).ToListAsync();
            ViewData["Colors"] = await _context.Colors.Select(c => c.Name).ToListAsync();
            ViewData["CPUs"] = await _context.CPUs.Select(c => c.Name).ToListAsync();
            ViewData["Year"] = await _context.Gadgets.Select(g => g.Year).Distinct().ToArrayAsync();
            ViewData["Memory"] = await _context.Gadgets.Select(g => g.Memory).Distinct().ToArrayAsync();
            ViewData["RAM"] = await _context.Gadgets.Select(g => g.RAM).Distinct().ToArrayAsync();
            ViewData["Id"] = Id;

            return View(model);
        }


        [Authorize(Roles = "seller")]
        public async Task<IActionResult> Management()
        {
            var gadgets = await _context.Gadgets
                .Include(g => g.Picture)
                .Include(g => g.CPU)
                .Include(g => g.Color)
                .Include(g => g.Diagonal)
                .Include(g => g.GadgetType)
                .Include(g => g.Manufacturer)
                .Include(g => g.ScreenResolution)
                .ToListAsync();

            return View(gadgets);
        }


        [Authorize(Roles = "seller")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gadget = await _context.Gadgets.FirstOrDefaultAsync(m => m.Id == id);

            if (gadget == null)
            {
                return NotFound();
            }

            var picture = await _context.Pictures.Where(p => p.Id == gadget.PictureId).FirstOrDefaultAsync();

            _context.Gadgets.Remove(gadget);

            if (picture != null)
            {
                _context.Pictures.Remove(picture);
            }

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Management));
        }

    }
}
