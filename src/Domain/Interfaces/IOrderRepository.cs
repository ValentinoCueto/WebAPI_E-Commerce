using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IOrderRepository : IBaseRepository<Order>
    {
        Order? GetOrderById(int id);
        List<Order> GetAllOrders();
        List<Order> GetOrdersByUserId(int userId);
    }
}
