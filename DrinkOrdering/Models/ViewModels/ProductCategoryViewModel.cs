using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DrinkOrdering.Models;

namespace DrinkOrdering.Models.ViewModels
{
    public class ProductCategoryViewModel
    {
        public IEnumerable<Category> CategoryList { get; set; }

        public Drink Drinks { get; set; }

        public string StatusMessage { get; set; }
    }
}
