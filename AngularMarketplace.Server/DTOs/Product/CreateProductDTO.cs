using AngularMarketplace.Server.Binders;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;

namespace AngularMarketplace.Server.DTOs.Product
{
    public class CreateProductDTO
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public float Price { get; set; }
        
        public string CategoryMask { get; set; }

        public IFormFileCollection? Imgs { get; set; }

        public CreateProductDTO()
        {
            Title = "";
            Description = "";
         
            CategoryMask = "";
            Price = 0;
        }
        

    }
}
