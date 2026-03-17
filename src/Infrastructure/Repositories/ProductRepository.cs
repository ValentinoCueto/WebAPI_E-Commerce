using Domain.Entities;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class ProductRepository : BaseRepository<Product>, IProductRepository
    {
        public ProductRepository(ApplicationContext context) : base(context)
        {

        }

        public List<Product> GetAllActiveProducts()
        {
            return _context.Products.Where(p => p.IsActive).ToList();
        }

        public Product? GetByName(string name)
        {
            string comparedName = name.ToLower().Replace(" ", "").Trim();

            return _context.Products.FirstOrDefault(p => p.IsActive && p.Name.ToLower().Replace(" ", "").Trim() == comparedName);
        }
    }
}
