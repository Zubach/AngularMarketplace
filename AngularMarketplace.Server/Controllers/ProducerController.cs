using AngularMarketplace.Server.DTOs.Producer;
using DataAccess.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AngularMarketplace.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProducerController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ILogger<ProducerController> _logger;
        public ProducerController(AppDbContext context,ILogger<ProducerController> logger)
        {
            this._context = context;
            this._logger = logger;
        }

        [HttpPost("producers")]
        [Authorize(Roles = "Moderator,Admin")]
        public async Task<IResult> AddProducer([FromBody]CreateProducerDTO dto)
        {
            var user = HttpContext.User;
            if(user?.Identity?.IsAuthenticated ?? false)
            {
                try
                {
                    Producer producer = new Producer
                    {
                        Name = dto.Name,
                        CreatorId = user.FindFirst(x=> x.Type == "UserID")?.Value ?? "",
                        DateAdded = DateTime.UtcNow
                    };

                    //producer.Categories = dto.Categories.Select(
                    //     x => new ProductCategory
                    //     {
                    //         ID = x
                    //     }
                    // ).ToList();
                    producer.Categories = _context.ProductCategories.Where(x => dto.Categories.Contains(x.ID)).ToList();
                    _context.Producers.Add(producer);

                   
                    
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
        [HttpGet("producers")]
        public async Task<IResult> GetProducers()
        {
            try
            {
                var producers = _context.Producers.Select(ToProducerDTO);
               
                return Results.Json(producers); 
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Results.StatusCode(500);
            }
        }

        [HttpGet("get_producers/category/{id}")]
        public async Task<IResult> GetCategoryProducers(int id)
        {
            try
            {
                var producers = _context.Producers.Where(x => 
                    x.Categories != null 
                    ? 
                    x.Categories.Any(y => y.ID == id)
                    :
                    false
                    // imagine a world where u can write it like this x.Categories?.Any(y => y.ID == id) ?? false )))
                ).Select(ToProducerDTO);
                return Results.Json(producers);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Results.StatusCode(500);
            }
        }

        private ProducerDTO ToProducerDTO(Producer model)
        {
            return new ProducerDTO
            {
                ID = model.ID,
                Name = model.Name,
            };
        }

    }
}
