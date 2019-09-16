using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RecordStore.Models;

namespace RecordStore.Models.ViewModels
{
    public class ProductsListViewModel
    {
        public IEnumerable<Product> Products { get; set; }
        public PageInfo PageInfo { get; set; }
        public string CurrentGenre { get; set; }
    }
}
