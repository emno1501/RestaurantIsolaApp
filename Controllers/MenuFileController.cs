using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RestaurantIsolaApp.Data;
using RestaurantIsolaApp.Models;

namespace RestaurantIsolaApp.Controllers
{
    public class MenuFileController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MenuFileController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: MenuFile
        [Authorize]
        [HttpGet("/admin/menyfiler")]
        public async Task<IActionResult> Index(int? id)
        {
            if (id != null)
            {
                var menufile = _context.MenuFile.Single(f => f.Id == id);
                string fileName = menufile.FileName;
                byte[] fileBytes = menufile.Content;

                return File(fileBytes, "application/pdf", fileName);
            }

            return View(await _context.MenuFile.ToListAsync());
        }

        // GET: MenuFile/Create
        [Authorize]
        [HttpGet("/admin/meny-ladda-upp")]
        public IActionResult Create()
        {
            
            return View();
        }

        // POST: MenuFile/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost("/admin/meny-ladda-upp")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upload()
        {
            foreach (var file in Request.Form.Files)
            {
                MenuFile menufile = new MenuFile();
                menufile.FileName = file.FileName;

                MemoryStream ms = new MemoryStream();
                file.CopyTo(ms);
                menufile.Content = ms.ToArray();

                ms.Close();
                ms.Dispose();

                _context.MenuFile.Add(menufile);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));

        }

        // GET: MenuFile/Delete/5
        [Authorize]
        [HttpGet("/admin/menyfil-ta-bort")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var menuFile = await _context.MenuFile
                .FirstOrDefaultAsync(m => m.Id == id);
            if (menuFile == null)
            {
                return NotFound();
            }

            return View(menuFile);
        }

        // POST: MenuFile/Delete/5
        [Authorize]
        [HttpPost("/admin/menyfil-ta-bort"), ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var menuFile = await _context.MenuFile.FindAsync(id);
            _context.MenuFile.Remove(menuFile);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MenuFileExists(int id)
        {
            return _context.MenuFile.Any(e => e.Id == id);
        }
    }
}
