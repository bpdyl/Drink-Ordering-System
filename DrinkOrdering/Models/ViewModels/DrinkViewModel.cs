using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DrinkOrdering.Models.ViewModels
{
    public class DrinkViewModel
    {
        public int DrinkId { get; set; }
        public string Name { get; set; }
        public string ShortDescription { get; set; }
        public int Price { get; set; }
        public string ImageThumbnailUrl { get; set; }
    }
}
