using AngularMarketplace.Server.DTOs;
using AngularMarketplace.Server.DTOs.Category;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;

namespace AngularMarketplace.Server.Binders
{
    public class CreateCategoryDTOBinder : IModelBinder
    {


        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null)
            {
                throw new ArgumentNullException(nameof(bindingContext));
            }

            var request = bindingContext.HttpContext.Request ?? throw new ArgumentNullException(nameof(bindingContext));

            
            CreateCategoryDTO dto = new CreateCategoryDTO();
            

            try
            {
                if(request.Form.Files.Count > 0)
                {
                    dto.Img = request.Form.Files[0];
                }
                StringValues json;
                if(request.Form.TryGetValue("parent",out json))
                {
                    dto.Parent = JsonConvert.DeserializeObject<ProductCategoryDTO>(json.ToString());
                }
                StringValues value;
                foreach (var prop in typeof(CreateCategoryDTO).GetProperties())
                {
                    var propName = FirstCharToLowerCase(prop.Name);
                    if(propName != "img" && propName != "parent")
                    {
                        if (request.Form.TryGetValue(propName, out value))
                        {
                            if(propName == "isSubCategory")
                                prop.SetValue(dto, Convert.ToBoolean(value.ToString()));
                            else
                                prop.SetValue(dto, value.ToString());
                        }
                    }
                }
                
                
                bindingContext.Result = ModelBindingResult.Success(dto);
            }
            catch (Exception ex) {
                bindingContext.Result = ModelBindingResult.Failed();
                
            }

            return Task.CompletedTask;
        }

        private string? FirstCharToLowerCase(string? str)
        {
            if (!string.IsNullOrEmpty(str) && char.IsUpper(str[0]))
                return str.Length == 1 ? char.ToLower(str[0]).ToString() : char.ToLower(str[0]) + str[1..];

            return str;
        }
    }
}
