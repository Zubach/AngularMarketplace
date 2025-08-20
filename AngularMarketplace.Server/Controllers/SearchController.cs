using AngularMarketplace.Server.DTOs;
using AngularMarketplace.Server.DTOs.Product;
using DataAccess.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AngularMarketplace.Server.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ILogger<SearchController> _logger;

        public SearchController(AppDbContext context,ILogger<SearchController> logger)
        {
            this._context = context;
            this._logger = logger;
        }

        [HttpGet("searchby_urltitle_mask/{url_title};{mask}")]
        public IResult SearchByUrlTitleAndMask(string url_title,string mask)
        {
            if (mask != "" && url_title != "")
            {
                try
                {
                    if (mask[0] == 'c')
                    {
                        // remove c from mask
                        mask = mask.Substring(1);
                        ProductCategory category = _context.ProductCategories.Where(x => x.Mask == mask && x.Url_Title == url_title).Include(c=> c.SubCategoriesList).SingleOrDefault();
                        if (category != null)
                        {
                            if(category.SubCategoriesList?.Count > 0)
                            {
                                IEnumerable<ProductCategoryDTO> categoryDTOs = category.SubCategoriesList.Select(c => ToProductCategoryDTO(c));
                                return Results.Json(new
                                {
                                    Type = "CategoriesList",
                                    Data = categoryDTOs
                                });
                            }
                            else
                            {
                                category.ProductsList = _context.Products.Where(x => x.CategoryID == category.ID).Include(i => i.Images).ToList();
                            }

                            if (category.ProductsList != null)
                            {
                                IEnumerable<ProductDTO> productDTOs = category.ProductsList.Select(p =>

                                    ToProductDTO(p)
                                );
                                return Results.Json(new
                                {
                                    Type = "ProductsList",
                                    Data = productDTOs
                                });
                            }
                        }
                    }
                    else if (mask[0] == 'p')
                    {
                        ICollection<Product> product = _context.Products.Where(p => p.Mask == mask && p.Url_Title == url_title).ToList();
                        if (product != null) { 
                            IEnumerable<ProductDTO> result = product.Select(p =>

                                ToProductDTO(p)
                                );
                            return Results.Json(new
                            {
                                Type = "ProductDetails",
                                Data = result
                            });
                        }
                       
                    }
                    return Results.BadRequest();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, ex.Message);
                    return Results.StatusCode(501);
                }
            }
            return Results.BadRequest();
        }



        private ProductDTO ToProductDTO(Product product)
        {
            return new ProductDTO()
            { 
                ID = product.ID,
                Description =  product.Description,
                Title = product.Title,
                img1 = product.Images?.ElementAtOrDefault(0)?.Filename ?? "",
                img2 = product.Images?.ElementAtOrDefault(1)?.Filename ?? "",
                Price = product.Price,
                Mask = product.Mask,
                Url_Title = product.Url_Title
                
            };
        }
        private ProductCategoryDTO ToProductCategoryDTO(ProductCategory productCategory)
        {
            return new ProductCategoryDTO()
            {
                IsSubCategory = productCategory.IsSubCategory,
                Mask = productCategory.Mask,
                Title = productCategory.Title,
                Url_Title = productCategory.Url_Title,
                Img = productCategory.img
            };
        }
    }
}
