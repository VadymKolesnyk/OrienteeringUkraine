using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OrienteeringUkraine.Data;
using OrienteeringUkraine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace OrienteeringUkraine.Controllers
{
    public class AccountController : ControllerBase
    {
        public AccountController(IDataManager dataManager) : base(dataManager) { }
        
        [HttpGet]
        public IActionResult Register()
        {
            SetSelectLists();
            return View();
        }

        private void SetSelectLists()
        {
            ViewBag.Regions = new SelectList(dataManager.GetAllRegions(), "Id", "Name");
            ViewBag.Clubs = new SelectList(dataManager.GetAllClubs(), "Id", "Name");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(AccountRegisterData data)
        {
            SetSelectLists();
            if (ModelState.IsValid)
            {
                var user = await dataManager.GetUserAsync(data.Login);
                if (user != null)
                {
                    await dataManager.AddNewUserAsync(data); // добавляем пользователя в бд
                    await Authenticate(user); // аутентификация

                    return RedirectToAction("Index", "Home");
                }
                else
                    ModelState.AddModelError("Login", "Такой логин уже существует");
            }
            return View(data);
        }

        private async Task Authenticate(AccountUser user)
        {
            // создаем один claim
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.Login),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role)
            };
            // создаем объект ClaimsIdentity
            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            // установка аутентификационных куки
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(AccountLoginData data, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var user = await dataManager.GetUserAsync(data.Login, data.Password);
                if (user != null)
                {
                    await Authenticate(user); // аутентификация

                    if (Url.IsLocalUrl(returnUrl))
                        return Redirect(returnUrl);
                    else
                        return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError("", "Некорректные логин и(или) пароль");
            }
            return View(data);
        }
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }
        [Authorize]
        public IActionResult Profile()
        {
            return View();
        }
        [Authorize]
        public IActionResult Edit()
        {
            return View();
        }
    }
}
