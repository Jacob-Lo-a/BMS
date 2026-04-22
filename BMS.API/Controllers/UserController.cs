using BMS.API.DTOs;
using BMS.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Renci.SshNet.Messages;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace BMS.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IBookService _bookService;
        private readonly ILogger _securityLogger;
       
        public UserController(IUserService userService, IBookService bookService, ILoggerFactory loggerFactory) 
        { 
            _userService = userService;
            _bookService = bookService;
            _securityLogger = loggerFactory.CreateLogger("SecurityLogger");
          
        }

        [HttpPost("login")]
        public IActionResult Login(LoginDto dto)
        {
            var user = _userService.ValidateUser(dto.Username, dto.Password);

            if (user == null)
            {
                _securityLogger.LogWarning("Login failed for user: {Username}", dto.Username);
                return Unauthorized(new {Success = false, Message = "帳號或密碼錯誤"});
            }
            else
            {
                dto.Success = true;
                dto.Message = "登入成功";
            }




            var claims = new[]
            {
                new Claim(ClaimTypes.Name, dto.Username),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes("e3b0c44298fc1c149afbf4c8996fb92427ae41e4649b934ca495991b7852b855"));

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
            );

            
            return Ok(new
            {
                success = dto.Success,
                message  = dto.Message,
                token = new JwtSecurityTokenHandler().WriteToken(token)
            });
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            return Ok("Deleted");
        }
        
    }
}
