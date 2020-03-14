using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RestaurantIsolaApp.Models;
using RestaurantIsolaApp.Data;

namespace RestaurantIsolaApp.Controllers
{
    public class MenusController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MenusController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Menus
        [Authorize]
        [HttpGet("/admin/menyer")]
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Menu.Include(b => b.MenuFile).OrderBy(m => m.MenuName);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Menus/Create
        [Authorize]
        [HttpGet("/admin/ny-meny")]
        public IActionResult Create()
        {
            ViewData["MenuFile"] = new SelectList(_context.MenuFile, "Id", "FileName");

            return View();
        }

        // POST: Menus/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost("/admin/ny-meny")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,MenuName,UsingMenu,MenuFileId")] Menu menu)
        {
            if (ModelState.IsValid)
            {
                _context.Add(menu);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MenuFile"] = new SelectList(_context.MenuFile, "Id", "FileName", menu.MenuFileId);
            return View(menu);
        }

        // GET: Menus/Edit/5
        [Authorize]
        [HttpGet("/admin/edit-meny")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var menu = await _context.Menu.FindAsync(id);
            if (menu == null)
            {
                return NotFound();
            }
            ViewData["MenuFile"] = new SelectList(_context.MenuFile, "Id", "FileName", menu.MenuFileId);
            return View(menu);
        }

        // POST: Menus/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost("/admin/edit-meny")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,MenuName,UsingMenu, MenuFileId")] Menu menu)
        {
            if (id != menu.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(menu);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MenuExists(menu.Id))
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
            ViewData["MenuFile"] = new SelectList(_context.MenuFile, "Id", "FileName", menu.MenuFileId);
            return View(menu);
        }

        // GET: Menus/Delete/5
        [Authorize]
        [HttpGet("/admin/ta-bort-meny")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var menu = await _context.Menu
                .Include(m => m.MenuFile)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (menu == null)
            {
                return NotFound();
            }

            return View(menu);
        }

        // POST: Menus/Delete/5
        [HttpPost("/admin/ta-bort-meny"), ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var menu = await _context.Menu.FindAsync(id);
            _context.Menu.Remove(menu);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MenuExists(int id)
        {
            return _context.Menu.Any(e => e.Id == id);
        }
    }
}
