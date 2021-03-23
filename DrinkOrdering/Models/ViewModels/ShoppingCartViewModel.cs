using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DrinkOrdering.Models;

namespace DrinkOrdering.Models.ViewModels
{
    public class ShoppingCartViewModel
    {
        public ShoppingCart ShoppingCart { get; set; }
        public int ShoppingCartTotal { get; set; }
    }
}
