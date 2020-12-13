using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OrienteeringUkraine.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrienteeringUkraine.Controllers
{
    public class EventController : ControllerBase
    {
        public EventController(IDataManager dataManager) : base(dataManager) { }
        private void SetSelectLists()
        {
            ViewBag.Regions = new SelectList(dataManager.GetAllRegions(), "Id", "Name");
        }
        public IActionResult Applications(int id)
        {
            if (!dataManager.IsExistsEvent(id))
            {
                return RedirectToAction("Index", "Home");
            }
            var model = dataManager.GetApplicationsById(id);
            ViewBag.ShowAdminModule = (User.IsInRole("admin") || User.IsInRole("moderator") || User.Identity.Name == model.OrganizerLogin);
            return View(model);
        }
        [Authorize(Roles = "admin, moderator, organizer")]
        [HttpGet]
        public IActionResult New()
        {
            SetSelectLists();
            return View();
        }
        [Authorize(Roles = "admin, moderator, organizer")]
        [HttpPost]
        public IActionResult New(EventNewData data)
        {
            SetSelectLists();
            if (ModelState.IsValid)
            {

            }
            return View(data);
        }
        public IActionResult Edit()
        {
            return View();
        }
        public IActionResult Export()
        {
            return RedirectToAction("Index", "Home");
        }
    }
}
