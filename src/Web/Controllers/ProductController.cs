using Application.Interfaces;
using Application.Models;
using Domain.Enums;
using Domain.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        private bool IsSeller()
        {
            var userRole = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

            if (userRole == RolEnum.Seller.ToString())
            {
                return true;
            }
            return false;
        }


        [HttpGet("[Action]")]
        public IActionResult GetAll()
        {
            return Ok(_productService.GetAll());
        }

        [HttpGet("[Action]/{id}")]
        public IActionResult GetById([FromRoute] int id)
        {
            try
            {
                return Ok(_productService.GetById(id));
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            
        }

        [Authorize]
        [HttpPost("[Action]")]
        public IActionResult Create([FromBody]ProductToCreate productToCreate)
        {
            try
            {
                if (!IsSeller())
                {
                    return Forbid();
                }
                return Ok(_productService.Create(productToCreate));
            }
            catch (BadRequestException ex)
            {
                return BadRequest(ex.Message);
            }
            
        }

        [Authorize]
        [HttpDelete("[Action]/{id}")]
        public IActionResult Delete([FromRoute] int id)
        {
            try
            {
                if (!IsSeller())
                {
                    return Forbid();
                }
                _productService.Delete(id);
                return Ok();
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            
        }

        [Authorize]
        [HttpPut("[Action]/{id}")]
        public IActionResult Update([FromBody]ProductToUpdate productToUpdate, [FromRoute] int id)
        {
            try
            {
                if (!IsSeller())
                {
                    return Forbid();
                }

                _productService.Update(productToUpdate, id);
                return Ok();
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            
        }

        [HttpGet("[Action]/{name}")]
        public IActionResult GetByName([FromRoute] string name)
        {
            try
            {
                return Ok(_productService.GetByName(name));
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            
        }
    }


}
