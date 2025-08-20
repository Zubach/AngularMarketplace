using DataAccess.Entities;

namespace DTO.Models.Product
{
    public class ModerationProductDTO
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public float Price { get; set; }

        public int CategoryID { get; set; }
        public string Mask { get; set; }

        public string[] Imgs { get; set; }

        public ProductVisibilityStatus Status { get; set; }
    }
}
