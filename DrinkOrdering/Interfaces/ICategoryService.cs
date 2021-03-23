using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DrinkOrdering.Models;
namespace DrinkOrdering.Interfaces
{
    public interface ICategoryService
    {
        IEnumerable<Category> Categories { get; }
    }
}
