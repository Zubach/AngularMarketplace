namespace DTO.Models
{
    public class ProductCategoryDTO
    {
        public int Id { get; set; }
        public bool IsSubCategory { get; set; }

        public string Title { get; set; }

        public string Url_Title { get; set; }

        public string Mask { get; set; }
        public string Img { get; set; }

        public IEnumerable<ProductCategoryDTO>? SubCategoriesList { get; set; }

        public ProductCategoryDTO? Parent { get; set; }

        public ProductCategoryDTO()
        {
            this.Mask = "";
            this.Parent = null;
            this.Title = "";
            this.Url_Title = "";
            this.Img = "";
            this.SubCategoriesList = null;
        }

    }
}
