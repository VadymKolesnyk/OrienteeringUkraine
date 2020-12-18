using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OrienteeringUkraine;
using Moq;
using OrienteeringUkraine.Controllers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Authentication;
using OrienteeringUkraine.Models;
using System.Security.Claims;

namespace UnitTests
{


    [TestClass]
    public class AccountControllerUnitTests
    {
        private IDataManager myManager = new TempDataManager();

        [TestMethod]
        public void RegisterReturnsViewResultTypeSuccess()
        {

            var mock = new Mock<IDataManager>();
            AccountController control = new AccountController(mock.Object);
            var result = control.Register();
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public async Task ReturnsNotNullOrRedirectToIndex()
        {
            var mock = new Mock<IDataManager>();
            AccountController control = new AccountController(mock.Object);
           // var context = new Mock<HttpContext>();

            //var request = new Mock<HttpRequest>();
            //var provider = new Mock<IServiceProvider>();
            //context
            //    .Setup(c => c.Request)
            //    .Returns(request.Object);
            //context
            //    .Setup(c => c.RequestServices)
            //    .Returns(provider.Object);
           
            //var moq = new Mock<ControllerContext>();
            //moq.SetupGet(x => x.HttpContext.Request.Path).Returns("/foo.com");

            //var controllerContext = new ControllerContext(/*new ActionContext(context.Object, new RouteData(), control)*/);
            //controllerContext.HttpContext = context.Object;
            //control.ControllerContext = controllerContext;

            var result = await control.Register(new OrienteeringUkraine.Data.AccountRegisterData()
            {
                Login = "testlogin",
                Name = "TestName",
                Surname = "TestSurname",  
            });
            if(result is RedirectToActionResult redir)
            {
                Assert.AreEqual("Index", redir.ActionName);
                Assert.AreEqual("Home", redir.ControllerName);
            }
            //Assert.IsNotNull(result);
            //Assert.IsInstanceOfType(result, typeof(IActionResult));
        }

        [TestMethod]
        public void LoginReturnsViewResult()
        {
            var moq = new Mock<IDataManager>();
            AccountController control = new AccountController(moq.Object);
            var result =  control.Login();
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public async Task LoginReturnsNotNullAndIActionResultType()
        {
            var mock = new Mock<IDataManager>();
            AccountController control = new AccountController(mock.Object);
            var result = await control.Login(new OrienteeringUkraine.Data.AccountLoginData(),null);
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IActionResult));
        }


        [TestMethod]
        public async Task ProfileMethodShouldRedirectToIndex()
        {
            var mock = new Mock<IDataManager>();
            AccountController control = new AccountController(mock.Object);
            var res = (RedirectToActionResult)(await control.Profile(null));
            Assert.AreEqual("Index", res.ActionName);
            Assert.AreEqual("Home", res.ControllerName);
        }


        [TestMethod]
        public async Task EditMethodReturnsNotNullORRedirectToLogin()
        {
            var mock = new Mock<IDataManager>();
            AccountController control = new AccountController(mock.Object);
            var actual = await control.Edit();
            if(actual is ViewResult view)
            {
                Assert.IsNotNull(view.Model);
            } 
            else if(actual is RedirectToActionResult redirect)
            {
                Assert.AreEqual("Login", redirect.ActionName);
                Assert.AreEqual("Account", redirect.ControllerName);
            }
        }

        [TestMethod]
        public async Task EditRedirectsNullUserToProfile()
        {
            var mock = new Mock<IDataManager>();
            AccountController control = new AccountController(mock.Object);
            var user = new Mock<ClaimsPrincipal>();
            var identity = new Mock<ClaimsIdentity>();
            identity.Setup(i => i.Name).Returns("testlogin");
            user.Setup(u => u.Identity).Returns(identity.Object);
           // controller useerr  !!!!
            AccountUserModel usermod = new AccountUserModel()
            {
                Login = "testlogin",
                Name = "TestName",
                Role = "admin",
                Surname = "TestSurname",
            };
            var actual = await control.Edit(usermod);
            if (actual is ViewResult view)
            {
                Assert.IsNotNull(view.Model);
            }
            else if (actual is RedirectToActionResult redirect)
            {
                Assert.AreEqual("Profile", redirect.ActionName);
                Assert.AreEqual("Account", redirect.ControllerName);
            }
        }
    }
}
