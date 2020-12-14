﻿using Microsoft.AspNetCore.Authorization;
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
        public ApplyController(IDataManager dataManager) : base(dataManager) { }
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
                dataManager.AppNewApplication(id, User.Identity.Name, data.GroupId, data.Chip);
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
                if ((User.IsInRole("admin") || User.IsInRole("moderator") /*|| User.Identity.Name == data.OrganizerLogin*/))
                {
                    return View("EditAdmin");
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
        public IActionResult Edit(int id, ApplicationData data)
        {
            if (ModelState.IsValid)
            {
                dataManager.UpdateApplication(id, User.Identity.Name, data.GroupId, data.Chip);
                return RedirectToAction("Applications", "Event", new { Id = id });
            }
            SetSelectLists(id);
            return View("EditUser", data);
        }
        [HttpPost]
        public IActionResult Edit(int id, ApplyEditData data)
        {
            if (ModelState.IsValid)
            {
                //dataManager.UpdateApplication(id, User.Identity.Name, data.GroupId, data.Chip);
                return RedirectToAction("Applications", "Event", new { Id = id });
            }
            SetSelectLists(id);
            return View("EditUser", data);
        }

        public IActionResult Delete(int id)
        {
            dataManager.DeleteApplication(id, User.Identity.Name);
            return RedirectToAction("Applications", "Event", new { Id = id });
        }
    }
}
