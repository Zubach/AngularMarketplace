using AngularMarketplace.Server.DTOs;
using AngularMarketplace.Server.DTOs.Category;
using AngularMarketplace.Server.Services.Intefaces;
using Azure.Core.Pipeline;
using DataAccess.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Linq;
using System.Reflection;
using System.Text.Json.Serialization;

namespace AngularMarketplace.Server.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductCategoriesController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ILogger<ProductCategoriesController> _logger;
        private readonly IUploadImageService _imageService;
        private readonly IWebHostEnvironment _env;

        public ProductCategoriesController(AppDbContext context, ILogger<ProductCategoriesController> logger,IUploadImageService uploadImageService, IWebHostEnvironment env) {
            _context = context;
            this._logger = logger;
            this._imageService = uploadImageService;
            this._env = env;
        }

        [HttpGet("get_main")]
        public JsonResult GetMainCategories()
        {
            try
            {
                ICollection<ProductCategory> productCategories = _context.ProductCategories.Where(x => x.IsSubCategory == false).Include(e => e.SubCategoriesList).Include(p => p.Parent).ToList();

                if (productCategories?.Count > 0)
                {
                    foreach (var p in productCategories)
                    {

                        if (p.SubCategoriesList != null)
                            foreach (var s in p.SubCategoriesList)
                            {
                                ICollection<ProductCategory> subSubCategories = _context.ProductCategories.Where(x => x.ParentID == s.ID).Include(x => x.SubCategoriesList).Include(p => p.Parent).ToList();
                                if (subSubCategories?.Count > 0)
                                {

                                    s.SubCategoriesList = subSubCategories;
                                }
                            }
                    }
                    IEnumerable<ProductCategoryDTO> categoryDTOs = productCategories.Select(c => ToProductCategoryDTO(c));
                    return new JsonResult(categoryDTOs);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);

            }
            return new JsonResult("Error");
        }
        [HttpGet("get_subcategories/{id}")]
        public JsonResult GetSubCategories(int id)
        {
            try
            {
                ICollection<ProductCategory> productCategories = _context.ProductCategories.Where(x => x.ParentID == id).Include(x => x.SubCategoriesList).ToList();
                if (productCategories?.Count > 0) {
                    IEnumerable<ProductCategoryDTO> categoryDTOs = productCategories.Select(c => ToProductCategoryDTO(c));
                    return new JsonResult(categoryDTOs);
                }
            }
            catch (Exception ex) {
                _logger.LogError(ex, ex.Message);
            }
            return new JsonResult("Error");
        }

        [HttpGet("get_available_categories")]
        [Authorize(Roles = "Moderator,Admin")]
        public async Task<IResult> GetAvailableCategories()
        {
            var user = HttpContext.User;
            if(user?.Identity?.IsAuthenticated ?? false)
            {
                try
                {
                    var categories = _context.ProductCategories
                        .Where(x => x.IsSubCategory
                        &&
                        x.SubCategoriesList == null
                        ||
                        x.SubCategoriesList.Count == 0)
                        .Include(x => x.SubCategoriesList)
                        .Select(p => ToProductCategoryDTO(p,false));

                    return Results.Json(categories);
                }
                catch(Exception ex)
                {
                    _logger.LogError(ex, ex.Message);
                    return Results.StatusCode(500);

                }
            }
            return Results.BadRequest("Uknown user");
        }

        [Consumes("multipart/form-data")]
        [HttpPost("create_category")]
        [Authorize(Roles = "Admin")]
        public async Task<IResult> CreateCategory([FromForm] CreateCategoryDTO dto)
        {
            
            var user = HttpContext.User;
            if(user?.Identity?.IsAuthenticated ?? false)
            {
                try
                {
                    if(_context.ProductCategories.Any(x=> x.Mask == dto.Mask))
                        return Results.BadRequest("Ooops.. Category with this mask already exists.");
                  
                    var category = new ProductCategory
                    {
                        Title = dto.Title,
                        Url_Title = dto.Url_Title,
                        Mask = dto.Mask,
                        IsSubCategory = dto.IsSubCategory,

                    };
                    bool isUsingSvg = false;

                    if (dto.Img != null)
                    {
                        var fileExtension = _imageService.GetFileExtension(dto.Img);
                        category.img = dto.Mask + "." + fileExtension;
                    }
                    else if (!dto.IsSubCategory && dto.Svg != null && dto.Svg != String.Empty)
                    {
                        category.img = dto.Svg;
                        isUsingSvg = true;
                    }


                    if (dto.Parent != null)
                    {
                        category.ParentID = _context.ProductCategories.First(x => x.Mask == dto.Parent.Mask).ID;
                    }
                    await _context.ProductCategories.AddAsync(category);
                    if (category.img != null && category.img != String.Empty && !isUsingSvg)
                    {
                        await _imageService.UploadImageAsync(dto.Img, Path.Combine(_env.ContentRootPath, "Images", "Categories"), category.img);
                    }
                    await _context.SaveChangesAsync();
                    return Results.Created();
                }
                catch(Exception ex)
                {
                    _logger.LogError(ex, ex.Message);
                    return Results.StatusCode(500);
                }
            }
            return Results.BadRequest("Unknown user.");
        }
        
        private ProductCategoryDTO? ToProductCategoryDTO(ProductCategory productCategory, bool ignoreSubCategoriesList = false)
        {
            if (productCategory != null)
            {
                var dto = new ProductCategoryDTO()
                {
                    Id = productCategory.ID,
                    IsSubCategory = productCategory.IsSubCategory,
                    Mask = productCategory.Mask,
                    Title = productCategory.Title,
                    Url_Title = productCategory.Url_Title,
                    Img = productCategory.img,
                    SubCategoriesList = ignoreSubCategoriesList ? null : productCategory.SubCategoriesList?.Select(x => ToProductCategoryDTO(x)),
                    Parent = ToProductCategoryDTO(productCategory?.Parent, true)
                };
                return dto;
            }
            return null;
        }

        [HttpGet("generate_category_dto")]
        [Authorize(Roles = "Admin")]
        public async Task<IResult> GenerateMask()
        {

            var user = HttpContext.User;
            if (user?.Identity?.IsAuthenticated ?? false)
            {
                try
                {
                    var userId = user.FindFirst(x => x.Type == "UserID").Value ?? "";
                    if (userId != "")
                    {
                        var seed = Guid.Parse(userId).GetHashCode();
                        var dto = new CreateCategoryDTO(await GenerateMaskAsync(seed));
                        return Results.Ok(dto);
                    }
                    return Results.StatusCode(500);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, ex.Message);
                    return Results.StatusCode(500);
                }
            }
            return Results.BadRequest("Unknown user.");
        }

        private async Task<string> GenerateMaskAsync(int seed)
        {
            // from 6 to 9 digits

            string mask = await Task.Run(() => { return GenerateMask(seed); });
            return mask;


        }

        private string GenerateMask(int seed)
        {
            // from 6 to 9 digits
            Random rand = new Random(seed);
            for (; true;)
            {
                var mask = rand.Next(100000, 999999999).ToString();
                if (!_context.ProductCategories.Where(x => x.Mask == mask).Any())
                {
                    return mask;
                }
            }
        }

    }
}
