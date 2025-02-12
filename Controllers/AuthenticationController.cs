using Clean_Life_API.DTO;
using Clean_Life_API.Models;
using Clean_Life_API.Repo.repointerface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Clean_Life_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private string _secretKey = "YourSuperSecretKey1234567890123456";  // طول 32 حرفًا (256 بت)
        private readonly IUserRepository _userRepository;

        public AuthenticationController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        // تسجيل مستخدم جديد
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO registerDto)
        {
            if (await _userRepository.UserExistsAsync(registerDto.Email, registerDto.Username))
            {
                return BadRequest(new { message = "User already exists" });
            }

            var user = new user
            {
                Username = registerDto.Username,
                Email = registerDto.Email
            };

            var createdUser = await _userRepository.RegisterUserAsync(user, registerDto.Password);
            var token = GenerateJwtToken(createdUser);

            return Ok(new { Token = token });
        }

        // تسجيل الدخول
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDto)
        {
            var user = await _userRepository.AuthenticateAsync(loginDto.Username, loginDto.Password);
            if (user == null)
            {
                return Unauthorized(new { message = "Invalid username or password" });
            }

            var token = GenerateJwtToken(user);
            return Ok(new { Token = token });
        }

        // استرجاع كلمة المرور
        [HttpPost("forget-password")]
        public async Task<IActionResult> ForgetPassword([FromBody] ForgetPasswordDTO forgetPasswordDto)
        {
            var result = await _userRepository.UpdatePasswordAsync(forgetPasswordDto.Email, forgetPasswordDto.NewPassword);
            if (!result)
            {
                return NotFound(new { message = "User not found" });
            }
            return Ok(new { message = "Password updated successfully" });
        }
       
        private string GenerateJwtToken(user user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("YourSuperSecretKey1234567890123456")); // تأكد من أن المفتاح السري طوله 32 حرفًا
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[] {
        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        new Claim(ClaimTypes.Name, user.Username),
        new Claim(ClaimTypes.Email, user.Email)
    };

            var token = new JwtSecurityToken(
                issuer: "your-issuer",  
                audience: "your-audience",  
                claims: claims,
                expires: DateTime.Now.AddDays(1), // صلاحية التوكن لمدة يوم واحد
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}

