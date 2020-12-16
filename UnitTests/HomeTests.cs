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
        private IDataManager manager = new TempDataManager();
        private HomeIndexData data = new HomeIndexData();

        [TestMethod]
        public void HomeModelIsNotNull()
        {
            HomeController homeController = new HomeController(manager);
            var result = homeController.Index(data) as ViewResult;
            Assert.IsNotNull(result.Model);
        }

        [TestMethod]
        public void DbReturnsNullTest() // positive test
        {
            var mock = new Mock<IDataManager>();
            HomeController controller = new HomeController(mock.Object);
            var result = controller.Index(data);
            Assert.IsNotNull(result);
        }
    }
}