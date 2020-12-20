using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OrienteeringUkraine;
using OrienteeringUkraine.Controllers;
using OrienteeringUkraine.Data;
using System.Web;
using Moq;
using System.Collections.Generic;
using OrienteeringUkraine.Models;

namespace UnitTests
{
    [TestClass]
    public class HomeTests
    {
        private HomeIndexData data = new HomeIndexData();

        [TestMethod]
        public void HomeModelIsNotNull()
        {
            var mock = new Mock<IDataManager>();
            var cache = new Mock<ICacheManager>();
            HomeController homeController = new HomeController(mock.Object, cache.Object);
            var result = homeController.Index(data) as ViewResult;
            Assert.IsNotNull(result.Model);
        }

        [TestMethod]
        public void DbReturnsNullTest() // positive test
        {
            var mock = new Mock<IDataManager>();
            var cache = new Mock<ICacheManager>();
            HomeController controller = new HomeController(mock.Object, cache.Object);
            var result = controller.Index(data);
            Assert.IsNotNull(result);
        }
    }
}