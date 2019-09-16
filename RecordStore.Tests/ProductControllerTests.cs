using System;
using Xunit;
using System.Collections.Generic;
using Moq;
using RecordStore.Controllers;
using RecordStore.Models;
using System.Linq;
using RecordStore.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

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
            ProductsListViewModel result = controller.List(null, 2).ViewData.Model as ProductsListViewModel;

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
            ProductController controller = new ProductController(mock.Object) { PageSize = 3 };

            //Act
            ProductsListViewModel result = controller.List(null, 2).ViewData.Model as ProductsListViewModel;

            //Assert
            PageInfo pageinfo = result.PageInfo;
            Assert.Equal(2, pageinfo.CurrentPage);
            Assert.Equal(3, pageinfo.ItemsPerPage);
            Assert.Equal(5, pageinfo.TotalItems);
            Assert.Equal(2, pageinfo.TotalPages);
        }
        [Fact]
        public void Can_Filter_Products()
        {
            //Arrange
            // - create mock repository
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns((new Product[]
            {
                new Product {ProductId = 1, Name = "P1", Genre = "Cat1"},
                new Product {ProductId = 2, Name = "P2", Genre = "Cat2"},
                new Product {ProductId = 3, Name = "P3", Genre = "Cat1"},
                new Product {ProductId = 4, Name = "P4", Genre = "Cat2"},
                new Product {ProductId = 5, Name = "P5", Genre = "Cat3"}
            }).AsQueryable<Product>());

            //Arrange - create a controller and make the page size 3 items
            ProductController controller = new ProductController(mock.Object);
            controller.PageSize = 3;

            //Action
            Product[] result = (controller.List("Cat2", 1).ViewData.Model as ProductsListViewModel)
                .Products.ToArray();

            //Assert
            Assert.Equal(2, result.Length);
            Assert.True(result[0].Name == "P2" && result[0].Genre == "Cat2");
            Assert.True(result[1].Name == "P4" && result[1].Genre == "Cat2");
        }
        [Fact]
        public void Generate_Genre_Specific_Product_Count()
        {
            // Arrange
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns((new Product[] {
                new Product {ProductId = 1, Name = "P1", Genre = "Cat1"},
                new Product {ProductId = 2, Name = "P2", Genre = "Cat2"},
                new Product {ProductId = 3, Name = "P3", Genre = "Cat1"},
                new Product {ProductId = 4, Name = "P4", Genre = "Cat2"},
                new Product {ProductId = 5, Name = "P5", Genre = "Cat3"}
            }).AsQueryable<Product>());

            ProductController target = new ProductController(mock.Object);
            target.PageSize = 3;
            Func<ViewResult, ProductsListViewModel> GetModel = result =>
                 result?.ViewData?.Model as ProductsListViewModel;

            // Action
            int? res1 = GetModel(target.List("Cat1"))?.PageInfo.TotalItems;
            int? res2 = GetModel(target.List("Cat2"))?.PageInfo.TotalItems;
            int? res3 = GetModel(target.List("Cat3"))?.PageInfo.TotalItems;
            int? resAll = GetModel(target.List(null))?.PageInfo.TotalItems;

            // Assert
            Assert.Equal(2, res1);
            Assert.Equal(2, res2);
            Assert.Equal(1, res3);
            Assert.Equal(5, resAll);
        }
    }
}
