using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OrienteeringUkraine;
using Moq;
using OrienteeringUkraine.Controllers;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Routing;

namespace UnitTests
{
    [TestClass]
    public class ApplyControllerTests
    {
        ApplyController controller;

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

            var context = new ControllerContext(new ActionContext(httpContext.Object, new RouteData(), new ControllerActionDescriptor()));
            var db = new Mock<IDataManager>();
            controller = new ApplyController(db.Object)
            {
                ControllerContext = context
            };
        }
       

        [TestMethod]
        public void EmptyEventRedirectsToHomePage()
        {
            var actual = controller.New(0);
            if(actual is RedirectToActionResult redir)
            {
                Assert.AreEqual("Index", redir.ActionName);
                Assert.AreEqual("Home", redir.ControllerName);
            }
        }


        [TestMethod]
        public void Method2()
        {

        }
    }
}

