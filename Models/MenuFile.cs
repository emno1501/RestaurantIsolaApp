using RestaurantIsolaApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantIsolaApp.Models
{
    public class MenuFile
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public byte[] Content { get; set; }
        public virtual ICollection<Menu> Menus { get; set; }
        public MenuFile()
        {

        }
    }
}
