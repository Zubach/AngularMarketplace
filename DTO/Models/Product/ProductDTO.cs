namespace DTO.Models.Product
{
    // preview product
    public class ProductDTO
    {
        public int ID { get; set; }
        
        public string Title { get; set; }
        
        public string Description { get; set; }
        public string Url_Title { get; set; }
        public string Mask { get; set; }

        public string img1 { get; set; } // images[0]
        public string img2 { get; set; } // images[1]

        public float Price { get; set; } 


    }
}
