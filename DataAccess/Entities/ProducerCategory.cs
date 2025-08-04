using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entities
{

    // many to many relationship
    
    public class ProducerCategory
    {

        public int ProducerId { get; set; }

        public int CategoryId { get; set; }

        

    }
}
