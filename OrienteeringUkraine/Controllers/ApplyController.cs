using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OrienteeringUkraine.Data;
using OrienteeringUkraine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrienteeringUkraine.Controllers
{
    [Authorize]
    public class ApplyController : ControllerBase
    {
        public ApplyController(IDataManager dataManager, ICacheManager cacheManager) : base(dataManager, cacheManager) { }
        private void SetSelectLists(int id)
        {
            ViewBag.Groups = new SelectList(dataManager.GetGroupsOnEvent(id), "Id", "Name");
        }
        [HttpGet]
        public IActionResult New(int id)
        {
            if (dataManager.IsApplied(id, User.Identity.Name))
            {
                return RedirectToAction("Edit", new { Id = id });
            }
            var @event = dataManager.GetEventById(id);
            if (@event == null)
            {
                return RedirectToAction("Index", "Home");
            }
            SetSelectLists(id);
            return View(new ApplicationData() { CurrentEvent = @event });
        }
        [HttpPost]
        public IActionResult New(int id, ApplicationData data)
        {

            if (dataManager.IsApplied(id, User.Identity.Name))
            {
                return RedirectToAction("Edit", new { Id = id });
            }
            var @event = dataManager.GetEventById(id);
            if (@event == null)
            {
                return RedirectToAction("Index", "Home");
            }
            if (ModelState.IsValid)
            {
                dataManager.AddNewApplication(id, User.Identity.Name, data.GroupId, data.Chip);
                return RedirectToAction("Applications", "Event", new { Id = id });
            }
            SetSelectLists(id);
            data.CurrentEvent = @event;
            return View(data);
        }
        [HttpGet]
        public IActionResult Edit(int id, string mode)
        {
            SetSelectLists(id);
            if (mode == "organizer")
            {
                var model = dataManager.GetApplicationsById(id);
                if (model == null)
                {
                    return RedirectToAction("Index", "Home");
                }
                ViewBag.ShowAdminModule = (User.IsInRole("admin") || User.IsInRole("moderator") || User.Identity.Name == model.OrganizerLogin);
                if (ViewBag.ShowAdminModule)
                {
                    return View("EditAdmin", model);
                }
            }
            var application = dataManager.GetApplication(id, User.Identity.Name);
            if (application == null)
            {
                return RedirectToAction("New", "Apply", new { Id = id });
            }
            return View("EditUser", application);
        }
        [HttpPost]
        public IActionResult Edit(int id, ApplicationData data, string login = null)
        {
            if (ModelState.IsValid)
            {
                dataManager.UpdateApplication(id, login ?? User.Identity.Name, data.GroupId, data.Chip);
                if (login == null)
                {
                    return RedirectToAction("Applications", "Event", new { Id = id });
                }
                else
                {
                    return RedirectToAction("Edit", "Apply", new { Id = id, Mode = "organizer" });
                }
            }
            SetSelectLists(id);
            return View("EditUser", data);
        }

        public IActionResult Delete(int id, string login = null)
        {
            dataManager.DeleteApplication(id, login ?? User.Identity.Name);
            if (login == null)
            {
                return RedirectToAction("Applications", "Event", new { Id = id });
            }
            else
            {
                return RedirectToAction("Edit", "Apply", new { Id = id, Mode = "organizer" });
            }
        }
    }
}
