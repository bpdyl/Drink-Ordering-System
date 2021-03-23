using DrinkOrdering.Interfaces;
using DrinkOrdering.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DrinkOrdering.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ApplicationDbContext _dbContext;
        public CategoryService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;    
        }
        public IEnumerable<Category> Categories => _dbContext.Categories;
        public void AddCategory(Category category)
        {
            _dbContext.Categories.Add(category);
            _dbContext.SaveChanges();
        }
        public void DeleteCategory(int categoryId)
        {
            Category C = _dbContext.Categories.FirstOrDefault(x => x.CategoryId == categoryId);
            if (C != null)
                _dbContext.Categories.Remove(C);
            _dbContext.SaveChanges();
        }

        public void UpdateCategory(Category category)
        {
            var dbCategory = _dbContext.Categories.FirstOrDefault(x => x.CategoryId == category.CategoryId);
            if (dbCategory != null)
            {
                dbCategory.CategoryName = category.CategoryName;
                dbCategory.Description = category.Description;
                _dbContext.Categories.Update(dbCategory);
                _dbContext.SaveChanges();
            }
        }
    }
}
