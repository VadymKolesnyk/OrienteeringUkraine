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

    public class EventControllerTests
    {
        EventController controller;
        [TestInitialize]
        public void Initial()
        {

            var httpContext = new Mock<HttpContext>();
            var claims = new List<Claim>
            {
                new Claim("Login", "testlogin"),
                new Claim("Role", "organizer"),
                new Claim("FullName", "test user")
            };
            ClaimsIdentity id = new ClaimsIdentity(claims, "Token", "Login", "Role");
            ClaimsPrincipal user = new ClaimsPrincipal(id);
            httpContext.Setup(h => h.User).Returns(user);

            var context = new ControllerContext(new ActionContext(httpContext.Object, new RouteData(), new ControllerActionDescriptor()));
            var db = new Mock<IDataManager>();
            db.Setup(dm => dm.GetApplicationsById(4)).Returns(new OrienteeringUkraine.Models.EventApplicationsModel());
            db.Setup(ev => ev.GetEventById(3)).Returns(new OrienteeringUkraine.Data.EventData());
            controller = new EventController(db.Object)
            {
                ControllerContext = context
            };
        }

        [TestMethod]
      
        public void ApplicationReceivesApplicationById4ReturnsViewModel()
        {
            for (int i = 0; i < 5; i++)
            {
                var result = controller.Applications(i);


                if (result is RedirectToActionResult redir)
                {
                    Assert.AreEqual("Index", redir.ActionName);
                    Assert.AreEqual("Home", redir.ControllerName);
                }
                else if (result is ViewResult view)
                {
                    Assert.IsNotNull(view.Model);
                }
            }
        }
        
        [TestMethod]
        public void OrganizerHasRightToEditRedirectToApplications()
        {
            var result = controller.Edit(3);
            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
            if(result is RedirectToActionResult redir)
            {
                Assert.AreEqual("Applications", redir.ActionName);
            }
            else if (result is ViewResult view)
            {
                Assert.IsNotNull(view.Model);
            }
        }

    }
}
