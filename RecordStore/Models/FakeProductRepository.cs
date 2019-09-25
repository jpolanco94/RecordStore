using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecordStore.Models
{
    public class FakeProductRepository //: IProductRepository
    {
        public IQueryable<Product> Products => new List<Product>
        {
            new Product { Name = "Appetite For Destruction", Price = 12},
            new Product { Name = "Animals", Price = 15},
            new Product { Name = "Twin Fantasy", Price = 25}
        }.AsQueryable<Product>();
    }
}
