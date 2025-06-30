using AngularMarketplace.Server.DTOs.User;
using DataAccess.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AngularMarketplace.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IConfiguration _configuration;

        public AccountController(AppDbContext context, UserManager<User> userManager, SignInManager<User> signInManager, IConfiguration configuration)
        {
            this._context = context;
            this._userManager = userManager;
            this._signInManager = signInManager;
            this._configuration = configuration;
        }

        [HttpPost("register")]
        public async Task<IResult> Register([FromBody] UserRegistrationDTO userDTO)
        {
            if (userDTO != null)
            {
                try
                {
                    User _user = new User()
                    {
                        UserName = userDTO.FullName,
                        Email = userDTO.Email
                    };
                    var response = await _userManager.CreateAsync(_user, userDTO.Password);
                    if (response.Succeeded)
                    {
                        return Results.Ok(response);
                    }
                    return Results.BadRequest(response);

                }
                catch (Exception ex) {
                    // to log
                }
            }
            return Results.BadRequest(new { custom_message = "Data is invalid." });
        }
        [HttpPost("login")]
        public async Task<IResult> Login([FromBody] UserLoginDTO userLoginDTO)
        {
            var user = await _userManager.FindByEmailAsync(userLoginDTO.Email);
            if (user != null && await _userManager.CheckPasswordAsync(user,userLoginDTO.Password))
            {
                var signInKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["AppSettings:JWTSecret"]!));
                SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                        new Claim("UserID",user.Id)
                    }),
                    Expires = DateTime.UtcNow.AddDays(15),
                    SigningCredentials = new SigningCredentials(signInKey,SecurityAlgorithms.HmacSha256Signature)
                };
                var tokenHandler = new JwtSecurityTokenHandler();
                var securityToken = tokenHandler.CreateToken(tokenDescriptor);
                var token = tokenHandler.WriteToken(securityToken);
                return Results.Ok(new {token});
            }
            return Results.BadRequest(new {custom_message = "Username or password is incorrect." });
        }
    }
    
}

