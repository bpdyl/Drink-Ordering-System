using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DrinkOrdering.Models;
using DrinkOrdering.Interfaces;

namespace DrinkOrdering.Services
{
    public class DrinkService : IDrinkService
    {
        private readonly ApplicationDbContext _dbContext;
        public DrinkService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<Drink> Drinks => _dbContext.Drinks.Include(c => c.Category);

        public IEnumerable<Drink> PreferredDrinks => _dbContext.Drinks.Where(p => p.IsPreferredDrink).Include(c => c.Category);

        public Drink GetDrinkById(int drinkId) => _dbContext.Drinks.FirstOrDefault(p => p.DrinkId == drinkId);

        public void AddDrink(Drink drink)
        {
            _dbContext.Drinks.Add(drink);
            _dbContext.SaveChanges();
        }

        public void DeleteDrink(int drinkId)
        {
            Drink D = _dbContext.Drinks.FirstOrDefault(p => p.DrinkId == drinkId);
            if (D != null)
                _dbContext.Drinks.Remove(D);
            _dbContext.SaveChanges();
        }

        public void UpdateDrink(Drink drink)
        {
            var dbDrink = _dbContext.Drinks.FirstOrDefault(p => p.DrinkId == drink.DrinkId);
            if(dbDrink != null)
            {
                dbDrink.Name = drink.Name;
                dbDrink.Price = drink.Price;
                dbDrink.LongDescription = drink.LongDescription;
                dbDrink.Category = drink.Category;
                dbDrink.ImageUrl = drink.ImageUrl;
                dbDrink.InStock = drink.InStock;
                dbDrink.IsPreferredDrink = drink.IsPreferredDrink;
                dbDrink.ImageThumbnailUrl = drink.ImageThumbnailUrl;
                _dbContext.Drinks.Update(dbDrink);
                _dbContext.SaveChanges();
            }
        }
    }
}
