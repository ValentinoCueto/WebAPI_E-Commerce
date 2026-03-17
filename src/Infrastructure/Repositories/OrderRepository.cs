using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class OrderRepository : BaseRepository<Order>, IOrderRepository
    {
        public OrderRepository(ApplicationContext context) : base (context) 
        {

        }

        public Order? GetOrderById(int id)
        {
            return _context.Orders?.Include(c => c.Client).Include(c => c.Details).ThenInclude(d => d.Product).FirstOrDefault(c => c.Id == id);
        }

        public List<Order> GetAllOrders()
        {
            return _context.Orders.Include(o => o.Client).Include(o => o.Details).ThenInclude(d => d.Product).ToList();

        }

        public List<Order> GetOrdersByUserId(int userId)
        {
            return _context.Orders.Include(o => o.Details).ThenInclude(d => d.Product).Include(o => o.Client).Where(o => o.Client.Id == userId).ToList();
        }


    }
}
