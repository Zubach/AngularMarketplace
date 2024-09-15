using DataAccess.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
                IEnumerable<Product> products = _context.Products.ToList();
                return new JsonResult(products);
            }
            catch(Exception ex)
            {
                return new JsonResult(ex.Message);
                // Log
            }
            
        }
    }
}
