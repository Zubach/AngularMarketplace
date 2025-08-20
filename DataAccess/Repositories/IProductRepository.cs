using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public interface IProductRepository:IRepository<Product>
    {
        public Task<IEnumerable<Product>> GetVisibleProductsAsync();
        public Task<IEnumerable<Product>> GetProductsOnModeration();
        public Task<IEnumerable<Product>> GetSellerProducts(string seller_id);
    }
}
