using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OrienteeringUkraine;
using Moq;
using OrienteeringUkraine.Controllers;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Routing;

namespace UnitTests
{
    [TestClass]
    public class ManageControllerTests
    {
        ManageController controller;

        [TestInitialize]
        public void Initializate()
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
            var cache = new Mock<ICacheManager>();
            controller = new ManageController(db.Object, cache.Object)
            {
                ControllerContext = context
            };
        }


        [TestMethod]
        public void EmptyGetUsersShouldReturnNotNullView()
        {
            var result = controller.Users();
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            if (result is ViewResult view) Assert.IsNotNull(view.Model);
        }

        [TestMethod]
        public void UpdateUserShoudReturnToUsers()
        {
            var result = controller.Edit(new OrienteeringUkraine.Data.ManageEditData());
            if (result is RedirectToActionResult redir) Assert.AreEqual("Users", redir.ActionName);
        }

        [TestMethod]
        public void DeleteUserReturnsToUsers()
        {
            var result = controller.Delete("testlogin");
            if (result is RedirectToActionResult redir) Assert.AreEqual("Users", redir.ActionName);
        }

    }
}
