using CRUDApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace CRUDApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private IConfiguration _configuration;

        public UsersController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private UserModel AuthenticateUser(UserModel user)
        {
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            UserModel _user = null;
            if (user.username == "admin" && user.password == "12345")
            {
                _user = new UserModel {username = "Himanshi Jain" };
            }

#pragma warning disable CS8603 // Possible null reference return.
            return _user != null?_user:null;
        }

        private string GenerateToken(UserModel user)
        {
#pragma warning disable CS8604 // Possible null reference argument.
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWTSettings:Key"]));
            var credentials = new SigningCredentials(securityKey,SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_configuration["JWTSettings:Issuer"], _configuration["JWTSettings:Audience"],null,
                expires:DateTime.Now.AddMinutes(1),
                signingCredentials:credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Login(UserModel user)
        {
            IActionResult response = Unauthorized();
            var _user = AuthenticateUser(user);

            if (_user == null)
            {
                var token = GenerateToken(_user);
                response = Ok(new {token = token});
            }
            return response;
        }
    }
}
