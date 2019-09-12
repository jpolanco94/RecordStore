using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecordStore.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Artist { get; set; }
        public decimal Price { get; set; }
        public string Genre { get; set; }
    }
}
