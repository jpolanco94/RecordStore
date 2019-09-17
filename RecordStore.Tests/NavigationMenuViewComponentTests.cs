using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Moq;
using RecordStore.Components;
using RecordStore.Models;
using Xunit;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Routing;

namespace RecordStore.Tests
{
    public class NavigationMenuViewComponentTests
    {
        [Fact]
        public void Can_Select_Genres()
        {
            //Arrange
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns((new Product[] {
                new Product { ProductId = 1, Name = "P1", Genre = "Apples" },
                new Product { ProductId = 2, Name = "P2", Genre = "Apples" },
                new Product { ProductId = 3, Name = "P3", Genre = "Plums" },
                new Product { ProductId = 4, Name = "P4", Genre = "Oranges" },
                }).AsQueryable<Product>());

            NavigationMenuViewComponent target = new NavigationMenuViewComponent(mock.Object);

            //Action - Get the set of genres
            string[] results = ((IEnumerable<string>)(target.Invoke()
                as ViewViewComponentResult).ViewData.Model).ToArray();

            //Assert
            Assert.True(Enumerable.SequenceEqual(new string[] {"Apples",
                "Oranges", "Plums" }, results));
        }
        [Fact]
        public void Indicates_Selected_Genre()
        {
            //Arrange
            string genreToSelect = "Apples";
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns((new Product[]
            {
                new Product {ProductId = 1, Name = "P1", Genre = "Apples"},
                new Product {ProductId = 4, Name = "P2", Genre = "Oranges"},
            }).AsQueryable<Product>());
            NavigationMenuViewComponent target =
                new NavigationMenuViewComponent(mock.Object);
            target.ViewComponentContext = new ViewComponentContext
            {
                ViewContext = new ViewContext
                {
                    RouteData = new RouteData()
                }
            };
            target.RouteData.Values["genre"] = genreToSelect;

            //Action
            string result = (string)(target.Invoke() as ViewViewComponentResult).
                ViewData["SelectedGenre"];

            //Assert
            Assert.Equal(genreToSelect, result);
        }
    }
}
