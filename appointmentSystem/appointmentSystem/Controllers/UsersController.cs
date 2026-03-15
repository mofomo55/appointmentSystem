
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

         [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _UserServ.GetUsers();

            return Ok(users);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOneUserById(int id)
        {
            var user = await _UserServ.GetOneUser(id);

            if (user == null)
                return NotFound();

            return Ok(user);
        }

        [HttpPost]
        public async Task<IActionResult> AddUser([FromBody] User user)
        {
            var newUser = await _UserServ.AddUser(user);

            if (newUser == null)
                return BadRequest();

            return Ok(newUser);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] User user)
        {
            var updatedUser = await _UserServ.UpdateUser(id, user);

            if (updatedUser == null)
                return NotFound();

            return Ok(updatedUser);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var deleted = await _UserServ.DeleteUser(id);

            if (!deleted)
                return NotFound();

            return Ok("User deleted successfully");
        }
    }
}
