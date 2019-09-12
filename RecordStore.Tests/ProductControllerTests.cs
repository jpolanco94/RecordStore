using System;
using Xunit;
using System.Collections.Generic;
using Moq;
using RecordStore.Controllers;
using RecordStore.Models;
using System.Linq;
using RecordStore.Models.ViewModels;

namespace RecordStore.Tests
{
    public class ProductControllerTests
    {
        [Fact]
        public void Can_Paginate()
        {
            //Arragnge
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns((new Product[]
            {
                new Product {ProductId = 1, Name = "P1"},
                new Product {ProductId = 2, Name = "P2"},
                new Product {ProductId = 3, Name = "P3"},
                new Product {ProductId = 4, Name = "P4"},
                new Product {ProductId = 5, Name = "P5"}
            }).AsQueryable<Product>());
            ProductController controller = new ProductController(mock.Object);
            controller.PageSize = 3;

            //Act
            ProductsListViewModel result = controller.List(2).ViewData.Model as ProductsListViewModel;

            //Assert
            Product[] prodArray = result.Products.ToArray();
            Assert.True(prodArray.Length == 2);
            Assert.Equal("P4", prodArray[0].Name);
            Assert.Equal("P5", prodArray[1].Name);
        }
        [Fact]
        public void Can_Send_Pagination_View_Model()
        {
            //Arrange
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns((new Product[]
            {
                new Product {ProductId = 1, Name = "P1"},
                new Product {ProductId = 2, Name = "P2"},
                new Product {ProductId = 3, Name = "P3"},
                new Product {ProductId = 4, Name = "P4"},
                new Product {ProductId = 5, Name = "P5"},
            }).AsQueryable<Product>());

            //Arrange
            ProductController controller = new ProductController(mock.Object) { PageSize = 3};

            //Act
            ProductsListViewModel result = controller.List(2).ViewData.Model as ProductsListViewModel;

            //Assert
            PageInfo pageinfo = result.PageInfo;
            Assert.Equal(2, pageinfo.CurrentPage);
            Assert.Equal(3, pageinfo.ItemsPerPage);
            Assert.Equal(5, pageinfo.TotalItems);
            Assert.Equal(2, pageinfo.TotalPages);
        }
    }
}
