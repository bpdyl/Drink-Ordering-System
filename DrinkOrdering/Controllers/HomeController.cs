using DrinkOrdering.Interfaces;
using DrinkOrdering.Models;
using DrinkOrdering.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace DrinkOrdering.Controllers
{
    public class HomeController : Controller
    {
        private readonly IDrinkService _drinkService;
        public HomeController(IDrinkService drinkService)
        {
            _drinkService = drinkService;
        }

        public ViewResult Index()
        {
            var homeViewModel = new HomeViewModel
            {
                PreferredDrinks = _drinkService.PreferredDrinks
            };
            return View(homeViewModel);
        }
    }
}
