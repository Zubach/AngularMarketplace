using AngularMarketplace.Server.DTOs.User;
using DataAccess.Entities;
using Microsoft.AspNetCore.Authorization;
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
        private readonly ILogger<AccountController> _logger;

        public AccountController(AppDbContext context, UserManager<User> userManager, SignInManager<User> signInManager, IConfiguration configuration,ILogger<AccountController> logger)
        {
            this._context = context;
            this._userManager = userManager;
            this._signInManager = signInManager;
            this._configuration = configuration;
            this._logger = logger;
        }

        [AllowAnonymous]
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
                    if(userDTO.Role == "Buyer" || userDTO.Role == "Seller")
                    {
                        await _userManager.AddToRoleAsync(_user, userDTO.Role);
                    }
                    else
                    {
                        await _userManager.AddToRoleAsync(_user, "Buyer");
                    }
                    if (response.Succeeded)
                    {
                        return Results.Ok(response);
                    }
                    return Results.BadRequest(response);

                }
                catch (Exception ex) {
                    _logger.LogError(ex, ex.Message);
                    return Results.BadRequest(new { custom_message = "Something went wrong.Please try again later" });
                }
            }
            return Results.BadRequest(new { custom_message = "Data is invalid." });
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IResult> Login([FromBody] UserLoginDTO userLoginDTO)
        {
            var user = await _userManager.FindByEmailAsync(userLoginDTO.Email);
            if (user != null && await _userManager.CheckPasswordAsync(user,userLoginDTO.Password))
            {
                try
                {
                    var roles = await _userManager.GetRolesAsync(user);
                    var signInKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["AppSettings:JWTSecret"]!));
                    SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
                    {
                        Subject = new ClaimsIdentity(new Claim[]
                        {
                        new Claim("UserID",user.Id),
                        new Claim(ClaimTypes.Role, roles.First())
                        }),
                        Expires = DateTime.UtcNow.AddDays(15),
                        SigningCredentials = new SigningCredentials(signInKey, SecurityAlgorithms.HmacSha256Signature)
                    };
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var securityToken = tokenHandler.CreateToken(tokenDescriptor);
                    var token = tokenHandler.WriteToken(securityToken);
                    return Results.Ok(new { token });
                }
                catch(Exception ex)
                {
                    _logger.LogError(ex, ex.Message);
                    return Results.BadRequest(new { custom_message = "Something went wrong.Please try again later" });
                }
            }
            return Results.BadRequest(new {custom_message = "Username or password is incorrect." });
        }
    }
    
}

