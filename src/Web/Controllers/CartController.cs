using Application.Interfaces;
using Domain.Entities;
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
    [Authorize]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;
        public CartController (ICartService cartService)
        {
            _cartService = cartService;
        }
        private bool IsClient()
        {
            var userRole = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

            if (userRole == RolEnum.Client.ToString())
            {
                return true;
            }
            return false;
        }

        private int GetAuthenticatedUserId()
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            return userIdClaim != null ? int.Parse(userIdClaim.Value) : -1;
            
        }

        [HttpPost("[Action]")]
        public IActionResult CreateCart()
        {
            try
            {
                if (!IsClient())
                {
                    return Forbid();
                }

                var authenticatedUserId = GetAuthenticatedUserId();
                
                return Ok(_cartService.CreateCart(authenticatedUserId));
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }catch(BadRequestException ex)
            {
                return BadRequest(ex.Message);
            }
            
        }

        [HttpGet("[Action]/{id}")]
        public IActionResult GetCartById([FromRoute] int id)
        {
            try
            {
                if (!IsClient())
                {
                    return Forbid();
                }
                var authenticatedUserId = GetAuthenticatedUserId();
                var cart = _cartService.GetCartById(id);
                if (cart.User.Id != authenticatedUserId)
                {
                    return Forbid();
                }

                return Ok(cart);
            }
            catch(NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            
        }

        [HttpPost("[Action]/{cartId}/{productId}/{quantity}")]
        public IActionResult AddToCart([FromRoute] int cartId, [FromRoute] int productId, [FromRoute] int quantity)
        {
            try
            {
                var cart = _cartService.GetCartById(cartId);
                if (!IsClient())
                {
                    return Forbid();
                }

                var authenticatedUserId = GetAuthenticatedUserId();
                if (cart.User.Id != authenticatedUserId)
                {
                    return BadRequest();
                }
                return Ok(_cartService.AddToCart(cartId, productId, quantity));
            }
            catch(NotFoundException ex)
            {
                return NotFound(ex.Message);
            }catch(BadRequestException ex)
            {
                return BadRequest(ex.Message);
            }
            
        }

        [HttpDelete("[Action]/{cartId}/{productId}")]
        public IActionResult RemoveFromCart([FromRoute] int cartId, [FromRoute] int productId)
        {
            try
            {
                var cart = _cartService.GetCartById(cartId);
                if (!IsClient())
                {
                    return Forbid();
                }

                var authenticatedUserId = GetAuthenticatedUserId();
                if (cart.User.Id != authenticatedUserId)
                {
                    return Forbid();
                }
                return Ok(_cartService.RemoveFromCart(cartId, productId));
            }
            catch(NotFoundException ex)
            {
                return NotFound(ex.Message);
            }catch(BadRequestException ex)
            {
                return BadRequest(ex.Message);
            }
            
        }

        [HttpPut("[Action]/{cartId}/{productId}/{newQuantity}")]
        public IActionResult UpdateCart([FromRoute] int cartId, [FromRoute] int productId, [FromRoute] int newQuantity)
        {
            try
            {
                var cart = _cartService.GetCartById(cartId);
                if (!IsClient())
                {
                    return Forbid();
                }

                var authenticatedUserId = GetAuthenticatedUserId();
                if (cart.User.Id != authenticatedUserId)
                {
                    return Forbid();
                }
                return Ok(_cartService.UpdateCart(cartId, productId, newQuantity));
            }
            catch(NotFoundException ex)
            {
                return NotFound(ex.Message);
            }catch(BadRequestException ex)
            {
                return BadRequest(ex.Message);
            }
            
        }



    }
}
