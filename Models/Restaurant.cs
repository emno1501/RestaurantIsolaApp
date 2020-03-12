using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantIsolaApp.Models
{
    public class Restaurant
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Fyll i stad")]
        public string City { get; set; }
        [Required(ErrorMessage = "Fyll i adress")]
        public string Adress { get; set; }
        [Required(ErrorMessage = "Fyll i postnummer")]
        public int PostalCode { get; set; }
        [Required(ErrorMessage = "Fyll i öppettider")]
        public string OpenHours { get; set; }
        [Required(ErrorMessage = "Fyll i telefonnummer")]
        public string PhoneNr { get; set; }
        [Required(ErrorMessage = "Fyll i e-pstadress")]
        public string Email { get; set; }
        public virtual ICollection<Booking> Bookings { get; set; }
        public Restaurant()
        {

        }
    }
}
