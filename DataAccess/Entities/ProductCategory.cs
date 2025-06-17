using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entities
{
    [Table("tblProductCategories")]
    public class ProductCategory
    {
        [Key]
        public int ID { get; set; }

        [Column("title",TypeName = "nvarchar(50)"),MaxLength(50)]
        public string Title { get; set; }

        [Column("url_title", TypeName = "nvarchar(50)"), MaxLength(50)]
        public string Url_Title { get; set; }
        [Column("mask", TypeName = "nvarchar(9)"), MaxLength(9)]
        public string Mask { get; set; }


        public bool IsSubCategory { get; set; }
        
        

        public ICollection<Product> ProductsList { get; set; }

        public int? ParentID { get; set; }
        public ProductCategory? Parent { get; set; }


    }
}
