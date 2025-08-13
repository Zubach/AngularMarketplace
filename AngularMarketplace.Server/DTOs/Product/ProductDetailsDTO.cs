namespace AngularMarketplace.Server.DTOs.Product
{
    public class ProductDetailsDTO
    {
        public int ID { get; set; }
        public string Title { get; set; }

        public string Description { get; set; }
        public string Url_Title { get; set; }
        public string Mask { get; set; }
        public float Price { get; set; }

        public string[] Images { get; set; }
    }
}
