using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DrinkOrdering.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DrinkOrdering.Components
{
    public class CategoryMenu : ViewComponent
    {
        private readonly ICategoryService _categoryService;
        public CategoryMenu(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public IViewComponentResult Invoke()
        {
            var categories = _categoryService.Categories.OrderBy(p => p.CategoryName);
            return View(categories);
        }
    }
}
