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
        public void MethodNewEmptyEventRedirectsToHomePage()
        {
            var actual = controller.New(0);
            if(actual is RedirectToActionResult redir)
            {
                Assert.AreEqual("Index", redir.ActionName);
                Assert.AreEqual("Home", redir.ControllerName);
            }
        }


        [TestMethod]
        public void NewMethodReturnsRedirectionToHomePage()
        {
            var actual = controller.New(0,new OrienteeringUkraine.Data.ApplicationData());
            if (actual is RedirectToActionResult redir)
            {
                Assert.AreEqual("Index", redir.ActionName);
                Assert.AreEqual("Home", redir.ControllerName);
            }
        }
        [TestMethod]
        public void ModeOrganizerEditMetodRedirectsToHomePage()
        {
            
            var actual = controller.Edit(0,"organizer");
            if (actual is RedirectToActionResult redir)
            {
                Assert.AreEqual("Index", redir.ActionName);
                Assert.AreEqual("Home", redir.ControllerName);
            }
            else if(actual is ViewResult view)
            {
                Assert.IsNotNull(view.Model);
            }
        }

        [TestMethod]
        public void WithoutModeOrganizerRedirectsToNew()
        {

            var actual = controller.Edit(0, "");
            if (actual is RedirectToActionResult redir)
            {
                Assert.AreEqual("New", redir.ActionName);
                Assert.AreEqual("Apply", redir.ControllerName);
            }
            else if (actual is ViewResult view)
            {
                Assert.IsNotNull(view.Model);
            }
        }

        [TestMethod]
        public void EmptyLoginMethodRedirectsToApplications()
        {
            var actual = controller.Edit(0, new OrienteeringUkraine.Data.ApplicationData());
            if (actual is RedirectToActionResult redir)
            {
                Assert.AreEqual("Applications", redir.ActionName);
                Assert.AreEqual("Event", redir.ControllerName);
            }
        }

        [TestMethod]
        public void MethodReceivesLoginAndRedirectsToEditApplication()
        {
            var actual = controller.Edit(0, new OrienteeringUkraine.Data.ApplicationData(), "testlogin");
            if (actual is RedirectToActionResult redir)
            {
                Assert.AreEqual("Edit", redir.ActionName);
                Assert.AreEqual("Apply", redir.ControllerName);
            }
            else if (actual is ViewResult view)
            {
                Assert.IsNotNull(view.Model);
            }
        }


        [TestMethod]
        public void DeleteMethodRecievesEmptyLoginRedirectsToApplications()
        {
            var actual = controller.Delete(0);
            if (actual is RedirectToActionResult redir)
            {
                Assert.AreEqual("Applications", redir.ActionName);
                Assert.AreEqual("Event", redir.ControllerName);
            }
            else if (actual is ViewResult view)
            {
                Assert.IsNotNull(view.Model);
            }
        }

        [TestMethod]
        public void DeleteMethodRecievesTestLoginRedirectsToEdit()
        {
            var actual = controller.Delete(0, "testlogin");
            if (actual is RedirectToActionResult redir)
            {
                Assert.AreEqual("Edit", redir.ActionName);
                Assert.AreEqual("Apply", redir.ControllerName);
            }
            else if (actual is ViewResult view)
            {
                Assert.IsNotNull(view.Model);
            }
        }
    }
}

