using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OrienteeringUkraine.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
            var model = dataManager.GetApplicationsById(id);
            if (model == null)
            {
                return RedirectToAction("Index", "Home");
            }
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
        public IActionResult New(EventData data)
        {
            SetSelectLists();
            if (ModelState.IsValid)
            {
                data.OrganizerLogin = User.Identity.Name;
                int id = dataManager.AddNewEvent(data);
                return RedirectToAction("Applications", new { Id = id });
            }
            return View(data);
        }
        [Authorize(Roles = "admin, moderator, organizer")]
        [HttpGet]
        public IActionResult Edit(int id)
        {
            SetSelectLists();
            var @event = dataManager.GetEventById(id);
            if (User.IsInRole("organizer") && User.Identity.Name != @event?.OrganizerLogin)
            {
                return RedirectToAction("Applications", new { Id = id });
            }
            if (@event == null)
            {
                return RedirectToAction("Index","Home");
            }
            return View(@event);
        }
        [Authorize(Roles = "admin, moderator, organizer")]
        [HttpPost]
        public IActionResult Edit(int id, EventData data)
        {
            SetSelectLists();
            if (ModelState.IsValid)
            {
                dataManager.UpdateEvent(id, data);
                return RedirectToAction("Applications", new { Id = id });
            }
            ModelState.AddModelError("", "Недопустимые изменения");
            return View(data);
        }
        [Authorize(Roles = "admin, moderator, organizer")]
        public IActionResult Export(int id)
        {
            return RedirectToAction("Applications", new { Id = id });
        }
    }
}
