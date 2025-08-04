using AngularMarketplace.Server.DTOs;
using AngularMarketplace.Server.DTOs.Category;
using AngularMarketplace.Server.DTOs.Product;
using AngularMarketplace.Server.Services.Intefaces;
using DataAccess.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Numerics;
using System.Reflection;

namespace AngularMarketplace.Server.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ILogger<ProductsController> _logger;
        private readonly IUploadImageService _uploadImageService;
        private readonly IWebHostEnvironment _env;

        public ProductsController(AppDbContext context,ILogger<ProductsController> logger, IUploadImageService uploadImageService,IWebHostEnvironment environment)
        {
            this._context = context;
            this._logger = logger;
            this._uploadImageService = uploadImageService;
            this._env = environment;
        }

        [HttpGet("get_products")]
        public JsonResult GetProducts()
        {
            try
            {
                IEnumerable<ProductDTO> products = _context.Products.ToList().Select(p =>
                   ToProductDTO(p)
                );
                return new JsonResult(products);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return new JsonResult(ex.Message);
            }
            
        }
        [HttpGet("get_product_details/{id}")]
        public JsonResult GetProduct(int id)
        {
            try
            {
                Product? product = _context.Products.SingleOrDefault(x => x.ID == id);
                if(product != null)
                {
                    ProductDTO result = ToProductDTO(product);
                    return new JsonResult(result);
                }
                return new JsonResult(null);

            }
            catch (Exception ex) {
                _logger.LogError(ex, ex.Message);
                return new JsonResult(ex.Message);
            }
        }

        [HttpGet("get_products_by_category/{id}")]
        public JsonResult GetProductsByCategory(int id)
        {
            try
            {
                if (_context.ProductCategories.Count(x => x.ID == id) > 0) {
                    ICollection<Product> products = _context.ProductCategories.Where(x => x.ID == id).Include(x=> x.ProductsList).Single().ProductsList;
                    if (products != null)
                    {
                        IEnumerable<ProductDTO> result = products.Select(p =>

                           ToProductDTO(p)
                        );
                        return new JsonResult(result);
                    }
                    return new JsonResult("something went wrong");
                }
                else
                {
                    return new JsonResult("Category dont found");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return new JsonResult(ex.Message);
            }
        }

        [HttpGet("get_seller_products")]
        [Authorize(Roles = "Seller")]
        public async Task<IResult> GetSellerProducts()
        {
            var user = HttpContext.User;
            if (user?.Identity?.IsAuthenticated ?? false)
            {
                try
                {


                    var userId = user.FindFirst(x => x.Type == "UserID").Value ?? "";
                    if (userId != "" && userId != null)
                    {
                        var products = _context.Products.Where(x => x.SellerID == userId).ToList();
                        var productsDTOs = products.Select(p => ToProductDTO(p));

                        return Results.Ok(productsDTOs);
                    }
                }
                catch (Exception ex) {
                    _logger.LogError(ex, ex.Message);
                    return Results.StatusCode(500);
                }
            }
            return Results.BadRequest("Unknowk user.");
        }

        

        [Consumes("multipart/form-data")]
        [HttpPost("create_product")]
        [Authorize(Roles ="Seller")]
        public async Task<IResult> CreateProduct([FromForm]CreateProductDTO dto)
        {
            var user = HttpContext.User;
            if(user?.Identity?.IsAuthenticated ?? false)
            {
                try
                {
                    if(Request.Form.Files?.Count > 0)
                    {
                        dto.Imgs = Request.Form.Files;
                    }
                    if(!(dto.Imgs != null && dto.Imgs.Count <= 6))
                        return Results.BadRequest(new { Message = "Limit for product images is 6 files." });

                    var userId = user.FindFirst(x => x.Type == "UserID")?.Value ?? "";
                    int seed = Guid.Parse(userId).GetHashCode();
                    Product product = new Product
                    {
                        Title = dto.Title,
                        Description = dto.Description,
                        Mask = await GenerateMaskAsync(seed),
                        Url_Title = "",
                        Price = dto.Price,
                        CategoryID = _context.ProductCategories.FirstOrDefaultAsync(x => x.Mask == dto.CategoryMask).Id,
                        SellerID = user.FindFirst(x => x.Type == "UserID")?.Value,
                        AvailabilityStatus = ProductAvailabilityStatus.Pending,
                        VisibilityStatus = ProductVisibilityStatus.Processing
                    };
                    if (dto.Imgs != null && dto.Imgs.Count <= 6)
                    {
                        var props = typeof(Product).GetProperties();
                        for (int i = 0; i < dto.Imgs.Count;i++)
                        {
                            var file = dto.Imgs[i];
                            string fileName = product.Mask + $"_{i+1}.{_uploadImageService.GetFileExtension(file.FileName)}";
                            await _uploadImageService.UploadImageAsync(file, Path.Combine(_env.ContentRootPath, "Images", "Products"), fileName);
                            var p = props.FirstOrDefault(p => p.Name == "img" + (i + 1).ToString());
                            if (p != null) { 
                                p.SetValue(product, fileName);
                            }
                            
                        }
                    }
                    _context.Products.Add(product);
                    await _context.SaveChangesAsync();
                    return Results.Created();
                }
                catch (Exception ex) {
                    _logger.LogError(ex, ex.Message);
                    return Results.StatusCode(500);
                }
            }
            return Results.BadRequest("Unknown user");
        }

        [HttpGet("get_moderation_products")]
        [Authorize(Roles = "Admin,Moderator")]
        public async Task<IResult> GetModerationProducts()
        {
            var user = HttpContext.User;
            if(user?.Identity?.IsAuthenticated ?? false)
            {
                try
                {
                    
                    return Results.Json( _context.Products
                        .Where(p => 
                            p.VisibilityStatus == ProductVisibilityStatus.Processing 
                            || 
                            p.VisibilityStatus == ProductVisibilityStatus.NotApproved)
                        .Select(ToModerationProductDTO));
                }
                catch(Exception ex)
                {
                    _logger.LogError(ex, ex.Message);
                    return Results.StatusCode(500);
                }
            }
            return Results.BadRequest("Unknown user");
        }

        private ModerationProductDTO ToModerationProductDTO(Product product)
        {

            List<string> imgs = new List<string>();
            if(product.img1 != null  && product.img1 != String.Empty)
            {
                var props = typeof(Product).GetProperties().Where(p => p.Name.Contains("img"));
                foreach(var p in props)
                {
                    string value = p.GetValue(product) as string;
                    if(value !=null && value != String.Empty)
                    {
                        imgs.Add(value);
                    }
                }
            }
            return new ModerationProductDTO()
            {
                Title = product.Title,
                Description = product.Description,
                Mask = product.Mask,
                CategoryID = product.CategoryID ?? 0,
                Price = product.Price,
                Status = product.VisibilityStatus,
                Imgs = imgs.ToArray()
            };
        }

        private ProductDTO ToProductDTO(Product product)
        {
            return new ProductDTO()
            {
                ID = product.ID,
                Description = product.Description,
                Title = product.Title,
                img1 = product.img1,
                img2 = product.img2,
                Price = product.Price,
                Mask = product.Mask,
                Url_Title = product.Url_Title
            };
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
                if (!_context.Products.Where(x => x.Mask == mask).Any())
                {
                    return mask;
                }
            }
        }
        private string GenerateUrlTitle()
        {
            return "";
        }
    }
}
