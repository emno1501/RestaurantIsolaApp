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
    public class MenuItemsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MenuItemsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: MenuItems
        [HttpGet("/meny")]
        public async Task<IActionResult> Index(int? id)
        {
            var restaurant = _context.Restaurant.Single(r => r.City == "Örebro");
            ViewData["restaurantInfo"] = restaurant;

            var menu = from allMenus in _context.MenuItem.Include(m => m.Menu)
                         select allMenus;
            menu = menu.Where(m => m.Menu.UsingMenu == true);

            var getMenuFile = _context.Menu.Single(m => m.UsingMenu == true);
            ViewData["MenuFile"] = getMenuFile.MenuFileId;

            if (id != null)
            {
                var menufile = _context.MenuFile.Single(f => f.Id == id);
                string fileName = menufile.FileName;
                byte[] fileBytes = menufile.Content;

                return File(fileBytes, "application/pdf", fileName);
            }

            return View(await menu.ToListAsync());
        }

        // GET: MenuItems
        [Authorize]
        [HttpGet("/admin/menyratter")]
        public async Task<IActionResult> AdminIndex(int? id)
        {
            var dishes = from items in _context.MenuItem.Include(m => m.Menu)
                      select items;
            if (id != null)
            {
                dishes = dishes.Where(dish => dish.MenuId == id);
            }

            return View(await dishes.AsNoTracking().ToListAsync());
        }

        // GET: MenuItems/Create
        [Authorize]
        [HttpGet("/admin/ny-menyratt")]
        public IActionResult Create()
        {
            List<SelectListItem> dishtypes = new List<SelectListItem>();
            dishtypes.Add(new SelectListItem { Value= "Förrätt", Text= "Förrätt" });
            dishtypes.Add(new SelectListItem { Value = "Varmrätt", Text = "Varmrätt" });
            dishtypes.Add(new SelectListItem { Value = "Efterrätt", Text = "Efterrätt" });

            ViewData["Dishtypes"] = dishtypes;
            ViewData["MenuId"] = new SelectList(_context.Menu, "Id", "MenuName");
            return View();
        }

        // POST: MenuItems/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost("/admin/ny-menyratt")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,DishName,DishType,DishDescription,Price,MenuId")] MenuItem menuItem)
        {
            if (ModelState.IsValid)
            {
                _context.Add(menuItem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(AdminIndex));
            }
            List<SelectListItem> dishtypes = new List<SelectListItem>();
            dishtypes.Add(new SelectListItem { Value = "Förrätt", Text = "Förrätt" });
            dishtypes.Add(new SelectListItem { Value = "Varmrätt", Text = "Varmrätt" });
            dishtypes.Add(new SelectListItem { Value = "Efterrätt", Text = "Efterrätt" });

            ViewData["Dishtypes"] = dishtypes;
            ViewData["MenuId"] = new SelectList(_context.Menu, "Id", "MenuName", menuItem.MenuId);
            return View(menuItem);
        }

        // GET: MenuItems/Edit/5
        [Authorize]
        [HttpGet("/admin/edit-menyratt")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var menuItem = await _context.MenuItem.FindAsync(id);
            if (menuItem == null)
            {
                return NotFound();
            }
            List<SelectListItem> dishtypes = new List<SelectListItem>();
            dishtypes.Add(new SelectListItem { Value = "Förrätt", Text = "Förrätt" });
            dishtypes.Add(new SelectListItem { Value = "Varmrätt", Text = "Varmrätt" });
            dishtypes.Add(new SelectListItem { Value = "Efterrätt", Text = "Efterrätt" });

            ViewData["Dishtypes"] = dishtypes;
            ViewData["MenuId"] = new SelectList(_context.Menu, "Id", "MenuName", menuItem.MenuId);
            return View(menuItem);
        }

        // POST: MenuItems/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost("/admin/edit-menyratt")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,DishName,DishType,DishDescription,Price,MenuId")] MenuItem menuItem)
        {
            if (id != menuItem.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(menuItem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MenuItemExists(menuItem.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(AdminIndex));
            }
            List<SelectListItem> dishtypes = new List<SelectListItem>();
            dishtypes.Add(new SelectListItem { Value = "Förrätt", Text = "Förrätt" });
            dishtypes.Add(new SelectListItem { Value = "Varmrätt", Text = "Varmrätt" });
            dishtypes.Add(new SelectListItem { Value = "Efterrätt", Text = "Efterrätt" });

            ViewData["Dishtypes"] = dishtypes;
            ViewData["MenuId"] = new SelectList(_context.Menu, "Id", "MenuName", menuItem.MenuId);
            return View(menuItem);
        }

        // GET: MenuItems/Delete/5
        [Authorize]
        [HttpGet("/admin/ta-bort-ratt")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var menuItem = await _context.MenuItem
                .Include(m => m.Menu)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (menuItem == null)
            {
                return NotFound();
            }

            return View(menuItem);
        }

        // POST: MenuItems/Delete/5
        [HttpPost("/admin/ta-bort-ratt"), ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var menuItem = await _context.MenuItem.FindAsync(id);
            _context.MenuItem.Remove(menuItem);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(AdminIndex));
        }

        private bool MenuItemExists(int id)
        {
            return _context.MenuItem.Any(e => e.Id == id);
        }
    }
}
