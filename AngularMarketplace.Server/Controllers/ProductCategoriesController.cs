using AngularMarketplace.Server.DTOs;
using DataAccess.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AngularMarketplace.Server.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductCategoriesController : ControllerBase
    {
        private readonly AppDbContext _context;
        
        public ProductCategoriesController(AppDbContext context) {
            _context = context; 
        }

        [HttpGet("get_main")]
        public JsonResult GetMainCategories()
        {
            try
            {
                ICollection<ProductCategory> productCategories = _context.ProductCategories.Where(x => x.IsSubCategory == false).Include(e=>e.SubCategoriesList).ToList();
                if (productCategories?.Count > 0) {
                    IEnumerable<ProductCategoryDTO> categoryDTOs = productCategories.Select(c => ToProductCategoryDTO(c));
                    return new JsonResult(categoryDTOs);
                }

            }
            catch(Exception ex)
            {
                // to log
                
            }
            return new JsonResult("Error");
        }
        [HttpGet("get_subcategories/{id}")]
        public JsonResult GetSubCategories(int id)
        {
            try
            {
                ICollection<ProductCategory> productCategories = _context.ProductCategories.Where(x => x.ParentID == id).Include(x => x.SubCategoriesList).ToList();
                if(productCategories?.Count > 0){
                    IEnumerable<ProductCategoryDTO> categoryDTOs = productCategories.Select(c => ToProductCategoryDTO(c));
                    return new JsonResult(categoryDTOs);
                }
            }
            catch (Exception ex) {
                //to log
            }
            return new JsonResult("Error");
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
