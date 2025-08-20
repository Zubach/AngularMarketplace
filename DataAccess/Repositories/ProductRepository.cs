using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DataAccess.Repositories
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
       
        public ProductRepository(AppDbContext context):base(context)
        {
        }
        


        public async Task<IEnumerable<Product>> GetVisibleProductsAsync()
        {
                return await _dbSet.Where(p => p.VisibilityStatus == ProductVisibilityStatus.Approved).ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetProductsOnModeration()
        {
                return await _dbSet
                    .Where(p => p.VisibilityStatus >= ProductVisibilityStatus.Processing)
                    .OrderBy(p => p.VisibilityStatus)
                    .ToListAsync();
            
        }

        public async Task<IEnumerable<Product>> GetSellerProducts(string seller_id)
        {
                return await _dbSet.Where(p => p.SellerID == seller_id)
                    .OrderBy(p => p.VisibilityStatus)
                    .ThenBy(p => p.AvailabilityStatus)
                    .ThenBy(p => p.Title)
                    .ToListAsync();
        }

    }
}
