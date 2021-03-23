using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DrinkOrdering.Models;
namespace DrinkOrdering.Interfaces
{
    public interface IDrinkService
    {
        IEnumerable<Drink> Drinks { get; }
        IEnumerable<Drink> PreferredDrinks { get; }
        Drink GetDrinkById(int drinkId);
        void AddDrink(Drink drink);
        void UpdateDrink(Drink drink);
        void DeleteDrink(int drinkId);

    }
}
