using AngularMarketplace.Server.Binders;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace AngularMarketplace.Server.DTOs.Category
{
    [ModelBinder(BinderType = typeof(CreateCategoryDTOBinder))]
    public class CreateCategoryDTO
    {
        public string Title { get; set; }
        public string Url_Title { get; set; }
        public bool IsSubCategory { get; set; }

        public string Mask { get; set; }
        public IFormFile? Img { get; set; }

        public ProductCategoryDTO? Parent { get; set; }

        public string? Svg { get; set; }


        public CreateCategoryDTO()
        {
            Title = "";
            Url_Title = "";
            Mask = "";
        }

        public CreateCategoryDTO(string mask)
        {
            Mask = mask;
            Title = "";
            Url_Title = "";
        }
    }
}
