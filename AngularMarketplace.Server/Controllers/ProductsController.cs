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

        [HttpGet]
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
        [HttpGet("{id}")]
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

        [HttpGet("category/{id}")]
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

        [HttpGet("seller")]
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
        [HttpPost]
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
                        Category =  await _context.ProductCategories.FirstOrDefaultAsync(x => x.Mask == dto.CategoryMask),
                        Producer = await _context.Producers.FirstOrDefaultAsync(x => x.ID == dto.ProducerId),
                        SellerID = user.FindFirst(x => x.Type == "UserID")?.Value,
                        AvailabilityStatus = ProductAvailabilityStatus.Pending,
                        VisibilityStatus = ProductVisibilityStatus.Processing,
                        Images = new List<ProductImage>()
                    };
                    if (dto.Imgs != null && dto.Imgs.Count <= 6)
                    {
                        for (int i = 0; i < dto.Imgs.Count; i++)
                        {
                            var file = dto.Imgs[i];
                            string fileName = product.Mask + $"_{i + 1}.{_uploadImageService.GetFileExtension(file.FileName)}";
                            await _uploadImageService.UploadImageAsync(file, Path.Combine(_env.ContentRootPath, "Images", "Products"), fileName);
                            product.Images.Add(new ProductImage { Filename = fileName });

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
            return Results.StatusCode(401);
        }

        [HttpGet("moderation")]
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

            
            return new ModerationProductDTO()
            {
                Title = product.Title,
                Description = product.Description,
                Mask = product.Mask,
                CategoryID = product.CategoryID ?? 0,
                Price = product.Price,
                Status = product.VisibilityStatus,
                Imgs = product.Images?.Select(x=> x.Filename)?.ToArray() ?? []
            };
        }

        private ProductDTO ToProductDTO(Product product)
        {
            return new ProductDTO()
            {
                ID = product.ID,
                Description = product.Description,
                Title = product.Title,
                img1 = product.Images?.ElementAtOrDefault(0)?.Filename ?? "",
                img2 = product.Images?.ElementAtOrDefault(1)?.Filename ?? "",
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
