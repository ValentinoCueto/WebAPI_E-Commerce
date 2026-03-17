using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Enums;

namespace Application.Models
{
    public class UserToCreate
    {
        [Required]
        public string? Name { get; set; }

        [EmailAddress]
        public string? Email { get; set; }

        [Required]
        public string? Password { get; set; }


    }
}
