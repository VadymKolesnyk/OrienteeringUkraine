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
using Microsoft.AspNetCore.Mvc.Controllers;
using OrienteeringUkraine.Data;

namespace UnitTests
{
    [TestClass]
    public class AccountControllerUnitTests
    {
        AccountController control;
       [TestInitialize]
        public void Initial()
        {

            var httpContext = new Mock<HttpContext>();
            var claims = new List<Claim>
            {
                new Claim("Login", "testlogin"),
                new Claim("Role", "admin"),
                new Claim("FullName", "test user")
            };
            ClaimsIdentity id = new ClaimsIdentity(claims, "Token", "Login", "Role");
            ClaimsPrincipal user = new ClaimsPrincipal(id);
            httpContext.Setup(h => h.User).Returns(user);
            
            var provider = new Mock<IServiceProvider>();
            
           
            var context = new ControllerContext(new ActionContext(httpContext.Object, new RouteData(), new ControllerActionDescriptor()));
            var db = new Mock<IDataManager>();
            var cache = new Mock<ICacheManager>();
            control = new AccountController(db.Object, cache.Object);
            control.ControllerContext = context;
      
        }

        [TestMethod]
        public void RegisterReturnsViewResultTypeSuccess()
        {
            var result = control.Register();
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

      

        [TestMethod]
        public void LoginReturnsViewResult()
        {
            var result =  control.Login();
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public async Task LoginReturnsNotNullAndIActionResultType()
        {
            var result = await control.Login(new OrienteeringUkraine.Data.AccountLoginData(),null);
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IActionResult));
        }


        [TestMethod]
        public async Task ProfileMethodShouldRedirectToIndex()
        {
            var res = (RedirectToActionResult)(await control.Profile(null));
            Assert.AreEqual("Index", res.ActionName);
            Assert.AreEqual("Home", res.ControllerName);
        }


        [TestMethod]
        public async Task EditMethodReturnsNotNullORRedirectToLogin()
        {
            var actual = await control.Edit();
            if (actual is ViewResult view)
            {
                Assert.IsNotNull(view.Model);
            }
            else if (actual is RedirectToActionResult redirect)
            {
                Assert.AreEqual("Login", redirect.ActionName);
                Assert.AreEqual("Account", redirect.ControllerName);
            }
        }
 
    }
}
