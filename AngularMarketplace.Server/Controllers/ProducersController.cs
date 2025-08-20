using AngularMarketplace.Server.DTOs.Producer;
using DataAccess.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace AngularMarketplace.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProducersController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ILogger<ProducersController> _logger;
        public ProducersController(AppDbContext context,ILogger<ProducersController> logger)
        {
            this._context = context;
            this._logger = logger;
        }

        [HttpPost]
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
        [HttpGet]
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

        [HttpGet("category/{id}")]
        [AllowAnonymous]
        public async Task<IResult> GetCategoryProducers(int id)
        {
            try
            {
                var producers = _context.ProductCategories.Where(c => c.ID == id).Include(p => p.Producers).FirstOrDefault()?.Producers?.Select(ToProducerDTO);
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
