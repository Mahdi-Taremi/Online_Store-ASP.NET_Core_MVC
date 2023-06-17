using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Writers;
using Online_Store_ASP.NET_Core_MVC.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Online_Store_ASP.NET_Core_MVC.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;

        public AuthController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;  
            _configuration = configuration;
        }

        // Send User Role to DbContextProject
        [HttpPost]
        [Route("Set-roles")]
        public async Task<IActionResult> SetRoles()
        {
            bool isUSERRole = await _roleManager.RoleExistsAsync(UsersRoles.USER);
            bool isADMINRole = await _roleManager.RoleExistsAsync(UsersRoles.ADMIN);
            bool isOWNERRole = await _roleManager.RoleExistsAsync(UsersRoles.OWNER);
            if(isUSERRole && isADMINRole && isOWNERRole)
            {
                return Ok("Set Role, It has already been Done");
            }
            else
            {
            await _roleManager.CreateAsync(new IdentityRole(UsersRoles.USER));
            await _roleManager.CreateAsync(new IdentityRole(UsersRoles.ADMIN));
            await _roleManager.CreateAsync(new IdentityRole(UsersRoles.OWNER));
                return Ok("Successfull, Set Role");
            }
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            var isExistUser = await _userManager.FindByNameAsync(registerDto.UserName);
            if (isExistUser != null) {
            return BadRequest("Username already exists " + ": " + isExistUser.ToString());
            }
            IdentityUser newUser = new IdentityUser()
            {
                Email = registerDto.Email,
                UserName = registerDto.UserName,
                SecurityStamp = Guid.NewGuid().ToString(),
            };
            var CreateUser = await _userManager.CreateAsync(newUser,registerDto.Password);

            if (!CreateUser.Succeeded)
            {
                var Error = "Failed : ";
                foreach (var error in CreateUser.Errors)
                {
                    Error += "#" + error.Description;
                }
                return BadRequest(Error);
            }
            await _userManager.AddToRoleAsync(newUser, UsersRoles.USER);
            return Ok("Successfull, User Created");
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var user = await _userManager.FindByNameAsync(loginDto.UserName);
            if (user is null) {
                return Unauthorized("Invalid Creden");
            }
            var isPasswordCorrect = await _userManager.CheckPasswordAsync(user, loginDto.Password);
            if(!isPasswordCorrect)
            {
                return Unauthorized("Invalid Password");
            }
            var userRoles = await _userManager.GetRolesAsync(user);

            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim("JWTId", Guid.NewGuid().ToString()),
            };

            foreach (var userRole in userRoles) { 
                authClaims.Add(new Claim(ClaimTypes.Role,userRole));
            }
            var token = GenerateNewJsonWebToken(authClaims);
            return Ok(token);

        }
        private string GenerateNewJsonWebToken(List<Claim> claims)
        {
            var authSecret = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Token"]));
            var tokenObject = new JwtSecurityToken(
                issuer: _configuration["Jwt:ValidateIssuer"],
                audience: _configuration["Jwt:ValidateAudience"],
                expires: DateTime.Now.AddMinutes(30),
                claims: claims,
                signingCredentials: new SigningCredentials(authSecret, SecurityAlgorithms.HmacSha256)
                );
            string token = new JwtSecurityTokenHandler().WriteToken(tokenObject);   
            return token;   
        }


        [HttpPost]
        [Route("set-role-admin")]
        public async Task<IActionResult> SetRoleAdmin([FromBody] UpdateRoleDto updateRoleDto)
        {
            var user = await _userManager.FindByNameAsync(updateRoleDto.UserName);
            if (user is null)
            {
                return BadRequest("Invalid UserName ");
            }
            await _userManager.AddToRoleAsync(user, UsersRoles.ADMIN);
            return Ok("Successful");    
        }
          
    }
}
