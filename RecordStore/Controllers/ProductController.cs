using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RecordStore.Models;
using RecordStore.Models.ViewModels;

namespace RecordStore.Controllers
{
    public class ProductController : Controller
    {
        private IProductRepository repository;
        public int PageSize = 4; // Displays how many items per page.
        public ProductController(IProductRepository repo)
        {
            repository = repo;
        }
        public ViewResult List(int productPage = 1) =>
            View(new ProductsListViewModel
            {
                Products = repository.Products
                    .OrderBy(p => p.ProductId)
                    .Skip((productPage - 1) * PageSize)
                    .Take(PageSize),
                PageInfo = new PageInfo
                {
                    CurrentPage = productPage,
                    ItemsPerPage = PageSize,
                    TotalItems = repository.Products.Count()
                }
            });
    }
}