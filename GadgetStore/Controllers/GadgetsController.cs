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

namespace GadgetStore.Controllers
{
    public class GadgetsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public GadgetsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Gadgets
        public async Task<IActionResult> Index()
        {
            FilterViewModel filterViewModel = new FilterViewModel
            {
                GadgetTypes = await _context.gadgetTypes.ToListAsync(),
                Diagonals = await _context.Diagonals.ToListAsync(),
                ScreenResolutions = await _context.ScreenResolutions.ToListAsync(),
                Colors = await _context.Colors.ToListAsync(),
                CPUs = await _context.CPUs.ToListAsync(),
                Manufacturers = await _context.Manufacturers.ToListAsync(),
                Year = await _context.Gadgets.Select(g => g.Year).Distinct().ToArrayAsync(),
                Memory = await _context.Gadgets.Select(g => g.Memory).Distinct().ToArrayAsync(),
                RAM = await _context.Gadgets.Select(g => g.RAM).Distinct().ToArrayAsync()
            };
            
            ViewData["Manufacturers"] = new SelectList(await _context.Manufacturers.ToListAsync(), "Id", "Name");
            ViewData["GadgetTypes"] = new SelectList(await _context.gadgetTypes.ToListAsync(), "Id", "Name");
            ViewData["Diagonals"] = new SelectList(await _context.Diagonals.ToListAsync(), "Id", "Value");
            ViewData["ScreenResolutions"] = new SelectList(await _context.ScreenResolutions.ToListAsync(), "Id", "Value");
            ViewData["Colors"] = new SelectList(await _context.Colors.ToListAsync(), "Id", "Name");
            ViewData["CPUs"] = new SelectList(await _context.CPUs.ToListAsync(), "Id", "Name");
            ViewData["Year"] = new SelectList(await _context.Gadgets.Select(g => g.Year).Distinct().ToArrayAsync());
            ViewData["Memory"] = new SelectList(await _context.Gadgets.Select(g => g.Memory).Distinct().ToArrayAsync());
            ViewData["RAM"] = new SelectList(await _context.Gadgets.Select(g => g.RAM).Distinct().ToArrayAsync());


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

        // GET: Gadgets/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gadget = await _context.Gadgets
                .Include(g => g.CPU)
                .Include(g => g.Color)
                .Include(g => g.Diagonal)
                .Include(g => g.GadgetType)
                .Include(g => g.Manufacturer)
                .Include(g => g.ScreenResolution)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (gadget == null)
            {
                return NotFound();
            }

            return View(gadget);
        }

        // GET: Gadgets/Create
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

        // POST: Gadgets/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
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
                _context.Pictures.Add(picture);

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
                    PictureId = picture.Id,
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
                return RedirectToAction(nameof(Index));
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

        // GET: Gadgets/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gadget = await _context.Gadgets.FindAsync(id);
            if (gadget == null)
            {
                return NotFound();
            }
            ViewData["CPUId"] = new SelectList(_context.CPUs, "Id", "Id", gadget.CPUId);
            ViewData["Colorid"] = new SelectList(_context.Colors, "Id", "Id", gadget.ColorId);
            ViewData["DiagonalId"] = new SelectList(_context.Diagonals, "Id", "Id", gadget.DiagonalId);
            ViewData["GadgetTypeId"] = new SelectList(_context.gadgetTypes, "Id", "Id", gadget.GadgetTypeId);
            ViewData["ManufacturerId"] = new SelectList(_context.Manufacturers, "Id", "Id", gadget.ManufacturerId);
            ViewData["ScreenResolutionId"] = new SelectList(_context.ScreenResolutions, "Id", "Id", gadget.ScreenResolutionId);
            return View(gadget);
        }

        // POST: Gadgets/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Year,Count,Memory,RAM,GadgetTypeId,DiagonalId,ScreenResolutionId,Colorid,CPUId,ManufacturerId")] Gadget gadget)
        {
            if (id != gadget.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(gadget);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GadgetExists(gadget.Id))
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
            ViewData["CPUId"] = new SelectList(_context.CPUs, "Id", "Id", gadget.CPUId);
            ViewData["Colorid"] = new SelectList(_context.Colors, "Id", "Id", gadget.ColorId);
            ViewData["DiagonalId"] = new SelectList(_context.Diagonals, "Id", "Id", gadget.DiagonalId);
            ViewData["GadgetTypeId"] = new SelectList(_context.gadgetTypes, "Id", "Id", gadget.GadgetTypeId);
            ViewData["ManufacturerId"] = new SelectList(_context.Manufacturers, "Id", "Id", gadget.ManufacturerId);
            ViewData["ScreenResolutionId"] = new SelectList(_context.ScreenResolutions, "Id", "Id", gadget.ScreenResolutionId);
            return View(gadget);
        }

        // GET: Gadgets/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gadget = await _context.Gadgets
                .Include(g => g.CPU)
                .Include(g => g.Color)
                .Include(g => g.Diagonal)
                .Include(g => g.GadgetType)
                .Include(g => g.Manufacturer)
                .Include(g => g.ScreenResolution)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (gadget == null)
            {
                return NotFound();
            }

            return View(gadget);
        }

        // POST: Gadgets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var gadget = await _context.Gadgets.FindAsync(id);
            _context.Gadgets.Remove(gadget);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GadgetExists(int id)
        {
            return _context.Gadgets.Any(e => e.Id == id);
        }
    }
}
