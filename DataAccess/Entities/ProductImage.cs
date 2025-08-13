using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entities
{
    public class ProductImage
    {
        public int ID { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }

        public string Filename { get; set; }
    }
}

