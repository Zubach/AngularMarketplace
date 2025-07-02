using AngularMarketplace.Server.DTOs.User;
using DataAccess.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AngularMarketplace.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WishlistController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly UserManager<User> _userManager;
        public WishlistController(AppDbContext context, UserManager<User> userManager)
        {
            this._context = context;
            this._userManager = userManager;
        }

        [HttpPost]
        public async Task<IResult> CreateWishlist(ClaimsPrincipal claims,string wishlist_name)
        {
            string userId = claims.Claims.First(x => x.Type == "UserID").Value;
            var user = await _userManager.FindByIdAsync(userId);
            if(user != null)
            {
                try
                {
                    _context.Wishlists.Add(new Wishlist
                    {
                        Name = wishlist_name,
                        User = user
                    });
                    await _context.SaveChangesAsync();
                    return Results.Created();
                }
                catch (Exception ex) {
                    return Results.StatusCode(503);
                }

            }
            return Results.BadRequest("Unknown user.");
        }

        [HttpGet("get_user_wishlists")]
        public async Task<IResult> GetUserWishlists()
        {
            var user = HttpContext.User;
            if (user.Identity.IsAuthenticated)
            {
                try
                {
                    var userId = user?.FindFirst(x => x.Type == "UserID").Value ?? "";
                    if (userId != "" && userId != null)
                    {
                        var wishlistList = _context.Wishlists.Where(x => x.UserId == userId).ToList().Select(x => ToUserWishlistDTO(x));
                        return Results.Ok(wishlistList);
                    }

                }
                catch (Exception ex)
                {
                    return Results.StatusCode(503);
                }
            }
            //string userId = claims.Claims.First(x => x.Type == "UserID").Value;
            //var user = await _userManager.FindByIdAsync(userId);
            //if (user != null)
            //{
            //    try
            //    {
            //        var wishlistList = _context.Wishlists.Where(x => x.UserId == userId).ToList().Select(x=> ToUserWishlistDTO(x));
            //        return Results.Ok(wishlistList);

                //    }
                //    catch (Exception ex)
                //    {
                //        return Results.StatusCode(503);
                //    }

                //}
            return Results.BadRequest("Unknown user");
        }

        private UserWishlistDTO ToUserWishlistDTO(Wishlist wishlists)
        {
            return new UserWishlistDTO { Name = wishlists.Name };
        }
    }
}
