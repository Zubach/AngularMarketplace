using AngularMarketplace.Server.DTOs.User;
using DataAccess.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AngularMarketplace.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AccountController(AppDbContext context, UserManager<User> userManager, SignInManager<User> signInManager)
        {
            this._context = context;
            this._userManager = userManager;
            this._signInManager = signInManager;
        }

        [HttpPost("register")]
        public async Task<JsonResult> Register([FromBody] UserRegistrationDTO userDTO)
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
                        return new JsonResult(Results.Ok(response));
                    }
                    return new JsonResult(Results.BadRequest(response));

                }
                catch (Exception ex) {
                    // to log
                }
            }
            return new JsonResult(Results.BadRequest(ModelState));
        }
    }
}

