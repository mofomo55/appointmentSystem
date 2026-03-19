
using AppointmentBooking.AppLayer.DTO;
using AppointmentBooking.AppLayer.Services;
using AppointmentBooking.Domains.Entities;
using AppointmentBooking.Persistencee.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<IActionResult> GetUsers()
        {
            var users = await _UserServ.GetUsers();

            return Ok(users);
        }
        [HttpGet("GetUser/{id}")]
        public async Task<IActionResult> GetOneUserById(int id)
        {
            var user = await _UserServ.GetOneUser(id);

            if (user == null)
                return NotFound();

            return Ok(user);
        }

        [HttpPost("SignUp")]
        public async Task<IActionResult> AddUser([FromBody] CreateNewUserDTO user)
        {
            var newUser = await _UserServ.AddUser(user);

            if (newUser == null)
                return BadRequest();

            return Ok(newUser);
        }
        [HttpPost("Login")]
        public async Task<IActionResult> LoginTheUser([FromBody] LoginDTO user)
        {
            var newUser = await _UserServ.LoginAsync(user);

            if (newUser == null)
                return BadRequest();

            return Ok(newUser);
        }

        [HttpPut("UpdateUser/{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] User user)
        {
            var updatedUser = await _UserServ.UpdateUser(id, user);

            if (updatedUser == null)
                return NotFound();

            return Ok(updatedUser);
        }

        [HttpDelete("DeleteUser{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var deleted = await _UserServ.DeleteUser(id);

            if (!deleted)
                return NotFound();

            return Ok("User deleted successfully");
        }
    }
}
