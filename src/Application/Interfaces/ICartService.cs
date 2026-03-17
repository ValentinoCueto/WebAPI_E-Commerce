using Application.Models;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface ICartService
    {
        CartToResponse? CreateCart(int userId);
        CartToResponse? GetCartById(int cartId);
        float AddToCart(int cartId, int productId, int quantity);
        CartToResponse? RemoveFromCart(int cartId, int productId);
        CartToResponse? UpdateCart(int cartId, int productId, int newQuantity);





    }
}
