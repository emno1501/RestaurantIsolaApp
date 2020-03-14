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
using SendGrid;
using SendGrid.Helpers.Mail;

namespace RestaurantIsolaApp.Controllers
{
    public class BookingController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BookingController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Booking
        [Authorize]
        [HttpGet("/admin/bokningar")]
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Booking.Include(b => b.Restaurant);
            foreach (var bookings in applicationDbContext)
            {
                if (bookings.BookedDate.Date < DateTime.Now.Date)
                {
                    var booking = await _context.Booking.FindAsync(bookings.Id);
                    _context.Booking.Remove(booking);
                    await _context.SaveChangesAsync();
                }
            }
            var currentBookings = _context.Booking.Include(b => b.Restaurant).OrderBy(b => b.BookedDate).ThenBy(b => b.BookedTime);
            return View(await currentBookings.ToListAsync());
        }

        // GET: Booking/Create
        [HttpGet("/boka")]
        public IActionResult Create()
        {
            var restaurant = _context.Restaurant.Single(r => r.City == "Örebro");
            ViewData["restaurantInfo"] = restaurant;

            ViewData["RestaurantId"] = new SelectList(_context.Restaurant, "Id", "City");
            return View();
        }

        // POST: Booking/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost("/boka")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FullName,Email,PhoneNr,NumbOfPersons,BookedDate,BookedTime,RestaurantId")] Booking booking)
        {
            if (ModelState.IsValid)
            {
                if (booking.BookedDate.DayOfWeek != 0)
                {
                    _context.Add(booking);
                    await _context.SaveChangesAsync();

                    // Bekräftelse-email
                    var apiKey = System.Environment.GetEnvironmentVariable("SENDGRID_APIKEY");
                    var client = new SendGridClient(apiKey);

                    var restaurantinfo = _context.Restaurant.Single(e => e.Id == booking.RestaurantId);
                    var fromEmail = restaurantinfo.Email;
                    var fromName = "Isola Restaurang & Bar " + restaurantinfo.City;
                    var bookingmail = new SendGridMessage();
                    bookingmail.SetFrom(new EmailAddress(fromEmail, fromName));

                    var guestName = booking.FullName;
                    var guestEmail = booking.Email;
                    bookingmail.AddTo(new EmailAddress(guestEmail, guestName));

                    bookingmail.SetSubject("Bekräftelse bordsbokning");

                    bookingmail.AddContent(MimeType.Html, "<p>Hej " + guestName +
                        "!<br />Du har bokat bord på Isola Restaurang & Bar " + restaurantinfo.City + " " + booking.BookedDate.ToString("yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture) +
                        " klockan " + booking.BookedTime.ToString("HH:mm", System.Globalization.CultureInfo.InvariantCulture) + ", för " + booking.NumbOfPersons + " personer.<br />Om du vill ändra eller avboka ditt bord kontakta oss på " + fromEmail +
                        " eller " + restaurantinfo.PhoneNr + ".<br />Välkommen!<br /><br />Isola Restaurang & Bar<p>");
                    bookingmail.AddContent(MimeType.Text, "Hej " + guestName +
                        "!\nDu har bokat bord på Isola Restaurang & Bar " + restaurantinfo.City + " " + booking.BookedDate.ToString("yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture) +
                        " klockan " + booking.BookedTime.ToString("HH:mm", System.Globalization.CultureInfo.InvariantCulture) + ", för " + booking.NumbOfPersons + " personer.\nOm du vill ändra eller avboka ditt bord kontakta oss på " + fromEmail +
                        " eller " + restaurantinfo.PhoneNr + ".\nVälkommen!\n\nIsola Restaurang & Bar");

                    await client.SendEmailAsync(bookingmail);
                    

                    return RedirectToAction(nameof(Confirmation));
                }
                else
                    ViewData["noValidDay"] = "Söndagar är ej bokningsbara";
            }
            var restaurant = _context.Restaurant.Single(r => r.City == "Örebro");
            ViewData["restaurantInfo"] = restaurant;

            ViewData["RestaurantId"] = new SelectList(_context.Restaurant, "Id", "City", booking.RestaurantId);
            return View(booking);
        }

        //GET: Booking/Confirmation
        [HttpGet("/boka/bekraftelse")]
        public IActionResult Confirmation()
        {
            var restaurant = _context.Restaurant.Single(r => r.City == "Örebro");
            ViewData["restaurantInfo"] = restaurant;
            return View();
        }

        // GET: Booking/Create
        [Authorize]
        [HttpGet("/admin/boka")]
        public IActionResult AdminCreate()
        {
            ViewData["RestaurantId"] = new SelectList(_context.Restaurant, "Id", "City");
            return View();
        }

        // POST: Booking/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost("/admin/boka")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AdminCreate([Bind("Id,FullName,Email,PhoneNr,NumbOfPersons,BookedDate,BookedTime,RestaurantId")] Booking booking)
        {
            if (ModelState.IsValid)
            {
                if (booking.BookedDate.DayOfWeek != 0)
                {
                    _context.Add(booking);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                else
                    ViewData["noValidDay"] = "Söndagar är ej bokningsbara";
            }
            ViewData["RestaurantId"] = new SelectList(_context.Restaurant, "Id", "City", booking.RestaurantId);
            return View(booking);
        }

        // GET: Booking/Edit/5
        [Authorize]
        [HttpGet("/admin/edit-bokning")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var booking = await _context.Booking.FindAsync(id);
            if (booking == null)
            {
                return NotFound();
            }
            ViewData["RestaurantId"] = new SelectList(_context.Restaurant, "Id", "City", booking.RestaurantId);
            return View(booking);
        }

        // POST: Booking/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost("/admin/edit-bokning")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FullName,Email,PhoneNr,NumbOfPersons,BookedDate,BookedTime,RestaurantId")] Booking booking)
        {
            if (id != booking.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (booking.BookedDate.DayOfWeek != 0)
                {
                    try
                    {
                        _context.Update(booking);
                        await _context.SaveChangesAsync();

                        // Bekräftelse-email
                        var apiKey = System.Environment.GetEnvironmentVariable("SENDGRID_APIKEY");
                        var client = new SendGridClient(apiKey);

                        var restaurantinfo = _context.Restaurant.Single(e => e.Id == booking.RestaurantId);
                        var fromEmail = restaurantinfo.Email;
                        var fromName = "Isola Restaurang & Bar " + restaurantinfo.City;
                        var bookingmail = new SendGridMessage();
                        bookingmail.SetFrom(new EmailAddress(fromEmail, fromName));

                        var guestName = booking.FullName;
                        var guestEmail = booking.Email;
                        bookingmail.AddTo(new EmailAddress(guestEmail, guestName));

                        bookingmail.SetSubject("Bekräftelse bordsbokning");

                        bookingmail.AddContent(MimeType.Html, "<p>Hej " + guestName +
                            "!<br />Din bokning på Isola Restaurang & Bar " + restaurantinfo.City + " har ändrats. <br />Ny bokning: " + booking.BookedDate.ToString("yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture) +
                            " klockan " + booking.BookedTime.ToString("HH:mm", System.Globalization.CultureInfo.InvariantCulture) + ", för " + booking.NumbOfPersons + " personer.<br />Om du vill ändra eller avboka ditt bord kontakta oss på " + fromEmail +
                            " eller " + restaurantinfo.PhoneNr + ".<br />Välkommen!<br /><br />Isola Restaurang & Bar<p>");
                        bookingmail.AddContent(MimeType.Text, "Hej " + guestName +
                            "!\nDin bokning på Isola Restaurang & Bar " + restaurantinfo.City + " har ändrats.\nNy bokning: " + booking.BookedDate.ToString("yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture) +
                            " klockan " + booking.BookedTime.ToString("HH:mm", System.Globalization.CultureInfo.InvariantCulture) + ", för " + booking.NumbOfPersons + " personer.\nOm du vill ändra eller avboka ditt bord kontakta oss på " + fromEmail +
                            " eller " + restaurantinfo.PhoneNr + ".\nVälkommen!\n\nIsola Restaurang & Bar");

                        await client.SendEmailAsync(bookingmail);
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!BookingExists(booking.Id))
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
                else
                    ViewData["noValidDay"] = "Söndagar är ej bokningsbara";
            }
            ViewData["RestaurantId"] = new SelectList(_context.Restaurant, "Id", "Adress", booking.RestaurantId);
            return View(booking);
        }

        // GET: Booking/Delete/5
        [Authorize]
        [HttpGet("/admin/ta-bort-bokning")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var booking = await _context.Booking
                .Include(b => b.Restaurant)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (booking == null)
            {
                return NotFound();
            }

            return View(booking);
        }

        // POST: Booking/Delete/5
        [HttpPost("/admin/ta-bort-bokning"), ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var booking = await _context.Booking.FindAsync(id);
            _context.Booking.Remove(booking);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookingExists(int id)
        {
            return _context.Booking.Any(e => e.Id == id);
        }
    }
}
