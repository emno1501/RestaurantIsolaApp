using RestaurantIsolaApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantIsolaApp.Models
{
    public class Menu
    {
        public int Id { get; set; }
        public string MenuName { get; set; }
        public bool UsingMenu { get; set; }
        // Lägga till property för pdf-fil med full meny
        public virtual ICollection<MenuItem> MenuItems { get; set; }
        public int MenuFileId { get; set; }
        public MenuFile MenuFile { get; set; }
        public Menu()
        {

        }
    }
}
