
//using System.IdentityModel.Tokens.Jwt;
using AppointmentBooking.AppLayer.Interfaces;
using AppointmentBooking.Domains.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
//using Microsoft.IdentityModel.Tokens;

namespace AppointmentBooking.Infrastructuree
{
    public class JwtTokenGenerator : IJwtTokenGenerator
    {
       // private readonly string _key = "b23a7254-7689-4842-86bd-aa8ef9c68c96";
        private readonly IConfiguration _config;

        public JwtTokenGenerator(IConfiguration config)
        {
            _config = config;
        }
        public string GenerateToken(User user)
        {

            var key = _config["Jwt:Key"];
            var issuer = _config["Jwt:Issuer"];
            var audience = _config["Jwt:Audience"];
            var expireHours = int.Parse(_config["Jwt:ExpireHours"]);

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var creds = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
             issuer,
             audience,
             claims,
             expires: DateTime.Now.AddMinutes(expireHours),
             signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
 
        }
    }

}

