using AngularMarketplace.Server.DTOs;
using DataAccess.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AngularMarketplace.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly AppDbContext _context;
        
        public ProductsController(AppDbContext context)
        {
            this._context = context;
        }

        [HttpGet("get_products")]
        public JsonResult GetProducts()
        {
            try
            {
                IEnumerable<ProductDTO> products = _context.Products.ToList().Select(p =>
                    new ProductDTO
                    {
                        ID = p.ID,
                        Description = p.Description,
                        Title = p.Title,
                        img1 = p.img1,
                        img2 = p.img2,
                        Price = p.Price
                    }
                );
                return new JsonResult(products);
            }
            catch(Exception ex)
            {
                return new JsonResult(ex.Message);
                // Log
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
                    ProductDTO result = new ProductDTO
                    {
                        ID= product.ID,
                        Description = product.Description,
                        Title = product.Title,
                        img1 = product.img1,
                        img2 = product.img2,
                        Price = product.Price
                    };
                    return new JsonResult(result);
                }
                return new JsonResult(null);

            }
            catch (Exception ex) { 
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

                            new ProductDTO
                            {
                                ID = p.ID,
                                Description = p.Description,
                                Title = p.Title,
                                img1 = p.img1,
                                img2 = p.img2,
                                Price = p.Price
                            }
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
                return new JsonResult(ex.Message);
            }
        }

        [HttpGet("get_category/{id}")]
        public JsonResult GetCategory(int id)
        {
            return new JsonResult(_context.ProductCategories.Where(x => x.ID == id).Include(x => x.Parent).Single().Parent);
        }
    }
}
