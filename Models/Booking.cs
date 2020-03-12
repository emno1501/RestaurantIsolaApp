using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantIsolaApp.Models
{
    public class Booking
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Fyll i fullständigt namn")]
        public string FullName { get; set; }
        [EmailAddress]
        [Required(ErrorMessage = "Fyll i e-post")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Fyll i telefonnummer")]
        public string PhoneNr { get; set; }
        [Required(ErrorMessage = "Fyll i antal personer")]
        public int NumbOfPersons { get; set; }
        [Required(ErrorMessage = "Fyll i datum för bokningen")]
        [DataType(DataType.Date)]
        public DateTime BookedDate { get; set; }
        [Required(ErrorMessage = "Fyll i tid för bokningen")]

        [DataType(DataType.Time)]
        public DateTime BookedTime { get; set; }
        public int RestaurantId { get; set; }
        public Restaurant Restaurant { get; set; }
        public Booking()
        {

        }
    }
}
