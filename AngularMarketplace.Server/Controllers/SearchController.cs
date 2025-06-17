using AngularMarketplace.Server.DTOs;
using DataAccess.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AngularMarketplace.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        private readonly AppDbContext _context;

        public SearchController(AppDbContext context)
        {
            this._context = context;
        }

        [HttpGet("searchby_urltitle_mask/{url_title};{mask}")]
        public JsonResult SearchByUrlTitleAndMask(string url_title,string mask)
        {
            if (mask != "" && url_title != "")
            {
                try
                {
                    if (mask[0] == 'c')
                    {
                        ICollection<Product> products = _context.ProductCategories.Where(x => x.Mask == mask && x.Url_Title == url_title).Include(c => c.ProductsList).Single().ProductsList;
                        if (products != null) {
                            IEnumerable<ProductDTO> result = products.Select(p =>

                                ToProductDTO(p)
                            );
                            return new JsonResult(result);
                        }
                    }
                    else if (mask[0] == 'p')
                    {
                        ICollection<Product> product = _context.Products.Where(p => p.Mask == mask && p.Url_Title == url_title).ToList();
                        if (product != null) { 
                            IEnumerable<ProductDTO> result = product.Select(p =>

                                ToProductDTO(p)
                                );
                            return new JsonResult(result);
                        }
                       
                    }
                    return new JsonResult(null);
                }
                catch (Exception ex)
                {
                    return new JsonResult($"{ex.Message}");
                }
            }
            return new JsonResult(null);
        }



        private ProductDTO ToProductDTO(Product product)
        {
            return new ProductDTO()
            { 
                ID = product.ID,
                Description =  product.Description,
                Title = product.Title,
                img1 = product.img1,
                img2 = product.img2,
                Price = product.Price,
                Mask = product.Mask,
                UrlTitle = product.Url_Title
            };
        }
    }
}
