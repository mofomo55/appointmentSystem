
using AppointmentBooking.AppLayer.DTO;
using AppointmentBooking.AppLayer.Services;
using AppointmentBooking.Domains.Entities;
using AppointmentBooking.Persistencee.Repositories;
using appointmentSystem.Middleware;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Umbraco.Core.Models.Membership;

namespace appointmentSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserService _UserServ;

        public UsersController(UserService UserServ)
        {
            _UserServ = UserServ;
        }

        [HttpGet("GetAllUsers")]
        [Authorize(Roles = "admin")]
        [ServiceFilter(typeof(EmailVerifiedFilter))]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _UserServ.GetUsers();

            return Ok(users);
        }

        [ServiceFilter(typeof(EmailVerifiedFilter))]
        [HttpGet("GetUser/{id}")]
        [Authorize]
        public async Task<IActionResult> GetOneUserById(Guid id)
        {
            var user = await _UserServ.GetOneUser(id);

            if (user == null)
                return NotFound();

            return Ok(user);
        }

        [HttpPost("SignUp")]
        public async Task<IActionResult> AddUser([FromBody] CreateNewUserDTO user)
        {
            try
            {
                var newUser = await _UserServ.AddUser(user);

                return Ok(new
                {
                    
                    NewUser = newUser
                });
            }
            catch (Exception ex)
            { 
                return BadRequest(new { message = ex.Message });
            }

        }
        [HttpPost("Login")]
        public async Task<IActionResult> LoginTheUser([FromBody] LoginDTO user)
        {
            try {
                var token = await _UserServ.LoginAsync(user);

                return Ok(new
                {
                    Token = token
                });
            }catch (Exception ex)
            {
                if(ex.Message == "User not found")
                {
                    return BadRequest(new { message = ex.Message });
                }
                else
                {
                    return Unauthorized(new { message = ex.Message });
                }
                    
            }

        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] string refreshToken)
        {
            try
            {
                var result = await _UserServ.RefreshTokenAsync(refreshToken);
                return Ok(result);
            }
            catch (Exception ex)
            {
                    return Unauthorized(new { message = ex.Message });
            }
        }

//        [HttpPut("UpdateUser/{id}")]
//        [Authorize]
//        public async Task<IActionResult> UpdateUser(int id, [FromBody] User user)
//        {
//            var updatedUser = await _UserServ.UpdateUser(id, user);

//            if (updatedUser == null)
//                return NotFound();

//0            return Ok(updatedUser);
//        }

        [HttpDelete("DeleteUser{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            var deleted = await _UserServ.DeleteUser(id);

            if (!deleted)
                return NotFound();

            return Ok("User deleted successfully");
        }


        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmationEmail(string token, string email)
        {
            try
            {
                var result = await _UserServ.ConfirmEmail(token, email);

                return Ok("User Confirmation is Successful!");
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
