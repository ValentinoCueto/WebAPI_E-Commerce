using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models
{
    public class CartToResponse
    {
        public int Id { get; set; }
        public float TotalPrice { get; set; }
        public UserToResponse? User { get; set; }
        public List<CartDetail>? Details { get; set; }
    }
}
