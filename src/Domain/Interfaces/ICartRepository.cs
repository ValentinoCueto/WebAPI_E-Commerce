using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface ICartRepository : IBaseRepository<Cart>
    {
        Cart? GetCartWithDetails(int cartId);
        CartDetail? GetDetailByProduct(int cartId, int productId);
        Cart? GetCartById(int id);
        Cart? GetCartByUserId(int userId);


    }
}
