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
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly ICartService _cartService;

        public OrderController(IOrderService orderService, ICartService cartService) 
        {
            _orderService = orderService;
            _cartService = cartService;
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

        [HttpGet("[Action]")]
        public IActionResult GetAllOrders()
        {
            if (!IsSeller())
            {
                return Forbid();
            }
            return Ok(_orderService.GetAllOrders());
        }

        [HttpGet("[Action]")]
        public IActionResult GetHistorialByUserId()
        {
            try
            {
                var authenticatedUserId = GetAuthenticatedUserId();
                if (!IsClient())
                {
                    return Forbid();
                }
                return Ok(_orderService.GetOrdersByUserId(authenticatedUserId));
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            
        }

        [HttpGet("[Action]/{orderId}")]
        public IActionResult GetById([FromRoute] int orderId)
        {
            try
            {
                if (!IsSeller())
                {
                    return Forbid();
                }
                return Ok(_orderService.GetOrderById(orderId));
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
           
        }

        [HttpPost("[Action]/{cartId}")]
        public IActionResult CreateOrder([FromRoute] int cartId)
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
                return Ok(_orderService.CreateOrderFromCart(cartId));
            }catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }catch (BadRequestException ex)
            {
                return BadRequest(ex.Message);
            }
            
        }

        [HttpDelete("[Action]/{orderId}")]
        public IActionResult DeleteOrder([FromRoute] int orderId) 
        {
            try
            {
                var order = _orderService.GetOrderById(orderId);
                if (!IsSeller())
                {
                   return Forbid();
                }
                _orderService.DeleteOrderFromCart(orderId);
                return Ok();
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            
        }
    }
}
