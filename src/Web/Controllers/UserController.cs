
using Application.Interfaces;
using Application.Models;
using Application.Services;
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
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
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

        [Authorize]
        [HttpGet("[Action]")]
        public IActionResult GetAll()
        {
            if (!IsSeller())
            {
                return Forbid();
            }
            return Ok(_userService.GetAll());
        }

        [Authorize]
        [HttpGet("[Action]/{id}")]
        public IActionResult GetById([FromRoute] int id)
        {
            try
            {
                if (!IsSeller())
                {
                    return Forbid();
                }
                return Ok(_userService.GetById(id));
            }catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            
        }

        [Authorize]
        [HttpGet("[Action]/{name}")]
        public IActionResult GetByName([FromRoute] string name)
        {
            try
            {
                if (!IsSeller())
                {
                    return Forbid();
                }
                return Ok(_userService.GetByName(name));
            }
            catch(NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            
        }

        [HttpPost("[Action]")]
        public IActionResult Create([FromBody] UserToCreate userToCreate)
        {
            try
            {
                _userService.Create(userToCreate);
                return Ok(userToCreate.Name);
            }
            catch(BadRequestException ex)
            {
                return BadRequest(ex.Message);
            }
            
        }

        [Authorize]
        [HttpDelete("[Action]")]
        public IActionResult Delete()
        {
            try
            {
                if (!IsClient())
                {
                    return Forbid();
                }
                var userAuthenticated = GetAuthenticatedUserId();
                _userService.DeactivateUser(userAuthenticated);
                return Ok();
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
           
        }

        [Authorize]
        [HttpPut("[Action]")]
        public IActionResult Update([FromBody] UserToUpdate userToUpdate)
        {
            try
            {
                if (!IsClient())
                {
                    return Forbid();
                }

                var userAuthenticated = GetAuthenticatedUserId();
                _userService.Update(userToUpdate, userAuthenticated);
                return Ok();
                
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
