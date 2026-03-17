using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models
{
    public class ProductToCreate
    {
        public string? Name { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "El precio debe ser al menos 1.")]
        public float Price { get; set; }
       [Range(1, int.MaxValue, ErrorMessage = "El stock debe ser al menos 1.")]
        public int Stock { get; set; }
        public string? Description { get; set; }

    }
}
