using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RecordStore.Controllers;
using RecordStore.Models;
using Xunit;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace RecordStore.Tests
{
    public class AdminControllerTest
    {
        [Fact]
        public void Index_Contains_All_Products()
        {
            //Arrange - create the mock Repository
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]
            {
                new Product {ProductId = 1, Name = "p1"},
                new Product {ProductId = 2, Name = "p2"},
                new Product {ProductId = 3, Name = "p3"}
            }.AsQueryable<Product>());

            //Arrange - create a controller
            AdminController target = new AdminController(mock.Object);

            //Action
            Product[] result = GetViewModel<IEnumerable<Product>>(target.Index())?.ToArray();

            //Assert
            Assert.Equal(3, result.Length);
            Assert.Equal("p1", result[0].Name);
            Assert.Equal("p2", result[1].Name);
            Assert.Equal("p3", result[2].Name);
        }
        private T GetViewModel<T>(IActionResult result) where T : class 
        {
            return (result as ViewResult)?.ViewData.Model as T;
        }
        [Fact]
        public void Can_Edit_Product()
        {
            //Arrange - create the mock repository
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]
            {
                new Product { ProductId = 1, Name = "p2"},
                new Product { ProductId = 2, Name = "p2"},
                new Product { ProductId = 3, Name = "p3"}
            }.AsQueryable<Product>());

            //Arrange - create the controller
            AdminController target = new AdminController(mock.Object);

            //Act
            Product result = GetViewModel<Product>(target.Edit(4));

            //Assert
            Assert.Null(result);
        }
        public void Can_Save_Valid_Changes()
        {
            //Arrange - create mock repository
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            //Arrange - creat mock temp data
            Mock<ITempDataDictionary> tempData = new Mock<ITempDataDictionary>();
            //Arrange - create the controller
            AdminController target = new AdminController(mock.Object)
            {
                TempData = tempData.Object
            };
            //Arrange - create a product
            Product product = new Product { Name = "Test" };

            //Act - try to save the product
            IActionResult result = target.Edit(product);

            //Assert - check that the repository was called
            mock.Verify(m => m.SaveProduct(product));
            //Assert - check the result type is redirection
            Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", (result as RedirectToActionResult).ActionName);
        }
        [Fact]
        public void Cannot_Save_Invalid_Changes()
        {
            //Arrange - create mock repository
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            //Arrange - crate the controller
            AdminController target = new AdminController(mock.Object);
            //Arrange - create a product
            Product product = new Product { Name = "Test" };
            //Arrange - add an error to the model state
            target.ModelState.AddModelError("error", "error");

            //Act - try to save the product
            IActionResult result = target.Edit(product);

            //Assert - check that the repository was not called
            mock.Verify(m => m.SaveProduct(It.IsAny<Product>()), Times.Never());
            //Assert - check the method result type
            Assert.IsType<ViewResult>(result);
        }
        [Fact]
        public void Can_Delete_Valid_Products()
        {
            //Arrange - create a product
            Product prod = new Product { ProductId = 2, Name = "test" };

            //Arrange - create the mock repository
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]
                {
                    new Product { ProductId = 1, Name = "P1"},
                    prod,
                    new Product {ProductId = 3, Name = "P3"},
                }.AsQueryable<Product>());

            //Arrange - create the controller
            AdminController target = new AdminController(mock.Object);

            //Act = delete the product
            target.Delete(prod.ProductId);

            //Assert - ensure that the repository delete method was
            //called with the correct Product
            mock.Verify(m => m.DeleteProduct(prod.ProductId));
        }
    }
}
