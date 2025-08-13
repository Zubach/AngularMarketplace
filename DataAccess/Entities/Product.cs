using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entities
{
    public enum ProductAvailabilityStatus
    {
        InStock,
        OutOfStock,
        Pending,
        Discontinued

    }
    public enum ProductVisibilityStatus
    {
        Approved,
        Processing,
        NotApproved
    }

    [Table("tblProducts")]
    public class Product
    {
        [Key]
        public int ID { get; set; }

        [Column("title",TypeName = "nvarhcar(50)"),MaxLength(50)]
        public string Title { get; set; }
        [Column("description",TypeName = "nvarchar(300)"),MaxLength(300)]
        public string? Description { get; set; }

        [Column("url_title", TypeName = "nvarchar(50)"), MaxLength(50)]
        public string Url_Title { get; set; }
        [Column("mask", TypeName = "nvarchar(9)"), MaxLength(9)]
        public string Mask { get; set; }

        [Column("price")]
        public float Price { get; set; }


        
        public ICollection<ProductImage> Images{ get; set; }

        [Column("availability_status")]
        public ProductAvailabilityStatus AvailabilityStatus { get; set; }
        [Column("visibility_status")]
        public ProductVisibilityStatus VisibilityStatus { get; set; }

        public int? CategoryID { get; set; }
        public ProductCategory? Category { get; set; }

        public string? SellerID { get; set; }
        public User? Seller { get; set; }

        public int? ProducerID { get; set; }
        public Producer? Producer { get; set; }
    }
}
