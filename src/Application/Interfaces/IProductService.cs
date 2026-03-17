using Application.Models;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IProductService
    {
        List<Product> GetAll();
        Product? GetById(int id);
        Product? Create(ProductToCreate productToCreate);
        void Delete(int id);
        void Update(ProductToUpdate productToUpdate, int id);
        Product? GetByName(string name);
    }
}
