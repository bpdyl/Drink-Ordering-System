using DrinkOrdering.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DrinkOrdering.Interfaces
{
    public interface IOrderService
    {
        void CreateOrder(Order order);
    }
}
