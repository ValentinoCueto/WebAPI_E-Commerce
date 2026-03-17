using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Interfaces;
using Application.Models;
using Domain.Entities;
using Domain.Enums;
using Domain.Exceptions;
using Domain.Interfaces;

namespace Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public List<Product> GetAll()
        {
            return _productRepository.GetAllActiveProducts();
        }

        public Product? GetById(int id)
        {
            var product = _productRepository.GetById(id);
            if (product == null || !product.IsActive)
            {
                throw new NotFoundException("El producto no fue encontrado");
            }
            return product;
        }

        public Product Create(ProductToCreate productToCreate)
        {
            var existingProductName = _productRepository.GetByName(productToCreate.Name);
            if (existingProductName is not null)
            {
                throw new BadRequestException("Ya existe un producto con ese nombre");
            }
            var product = new Product()
            {
                Name = productToCreate.Name,
                Price = productToCreate.Price,
                Stock = productToCreate.Stock,
                Description = productToCreate.Description,
            };


            return _productRepository.Create(product);
            
        }

        public void Delete(int id)
        {
            var product = _productRepository.GetById(id);
            if (product == null)
            {
                throw new NotFoundException("El producto no fue encontrado");
            }
            product.IsActive = false;

            _productRepository.Update(product);
        }

        public void Update(ProductToUpdate productToUpdate, int id)
        {
            var product = _productRepository.GetById(id);
            if (product == null || !product.IsActive)
            {
                throw new NotFoundException("El producto no fue encontrado");
            }
            product.Price = productToUpdate.Price;
            product.Stock = productToUpdate.Stock;
            _productRepository.Update(product);
        }
        public Product? GetByName(string name)
        {
            var product =  _productRepository.GetByName(name);
            if (product == null)
            {
                throw new NotFoundException("El producto no fue encontrado");
            }
            return product;
        }
    }
}
