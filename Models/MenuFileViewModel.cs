using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantIsolaApp.Models
{
    public class MenuFileViewModel
    {
        public IFormFile FileUpload { get; set; }
    }
}
