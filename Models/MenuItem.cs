using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantIsolaApp.Models
{
    public class MenuItem
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Fyll i namn på rätt")]
        public string DishName { get; set; }
        public string DishType { get; set; }
        [Required(ErrorMessage = "Fyll i beskrivning av rätt")]
        public string DishDescription { get; set; }
        [Required(ErrorMessage = "Fyll i pris på rätt")]
        public double? Price { get; set; }
        public int MenuId { get; set; }
        public Menu Menu { get; set; }
        public MenuItem()
        {

        }
    }
}
