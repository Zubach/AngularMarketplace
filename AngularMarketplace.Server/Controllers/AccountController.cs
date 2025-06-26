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

        

    }
}
