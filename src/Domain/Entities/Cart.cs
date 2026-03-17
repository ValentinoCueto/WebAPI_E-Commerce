using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Enums;

namespace Domain.Entities
{
    public class Cart
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; } 
        public List<CartDetail>? Details { get; set; } = new List<CartDetail>();
        public float TotalPrice { get; set; }
        public User? User { get; private set; }


        public void SetUser(User user)
        {
            if(user.Rol != RolEnum.Client) 
            {
                throw new Exception("el vendedor no puede tener acceso a un carrito");
            }
            User = user;
        }
    }

    
}
