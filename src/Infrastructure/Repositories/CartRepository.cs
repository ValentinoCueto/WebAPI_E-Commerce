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
    public class CartRepository : BaseRepository<Cart>, ICartRepository
    {
        public CartRepository(ApplicationContext context) : base(context)
        {

        }

        public Cart? GetCartWithDetails(int cartId)
        {
            return _context.Carts?.Include(c => c.User).Include(c => c.Details).ThenInclude(d => d.Product).FirstOrDefault(c => c.Id == cartId);
        }
        public CartDetail? GetDetailByProduct(int cartId, int productId)
        {
            var cart = GetCartWithDetails(cartId);
            return cart?.Details?.FirstOrDefault(d => d.Product?.Id == productId);
        }

        public Cart? GetCartById(int id)
        {
            return _context.Carts?.Include(c => c.User).Include(c => c.Details).ThenInclude(d => d.Product).FirstOrDefault(c => c.Id == id); 
        }

        public Cart? GetCartByUserId(int userId)
        {
            return _context.Carts.FirstOrDefault(c => c.User.Id == userId);
        }


    }


}
