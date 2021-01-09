using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using ViewModels;

namespace Presentation.Controllers
{
    public class AccountController : Infrastructure.BaseControllerWithUnitOfWork
    {

        //private DatabaseContext db = new DatabaseContext();
        public ActionResult Login(string ReturnUrl = "")
        {
            ViewBag.Message = "";
            ViewBag.ReturnUrl = ReturnUrl;
            LoginViewModel loginViewModel = new LoginViewModel();
            return View(loginViewModel);

        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]

        public ActionResult Login(LoginViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                User oUser = UnitOfWork.UserRepository
                    .Get(a => a.CellNum == model.Username && a.Password == model.Password).FirstOrDefault();


                if (oUser != null)
                {
                    Role role =UnitOfWork.RoleRepository.GetById(oUser.RoleId);

                    var ident = new ClaimsIdentity(
                      new[] { 
              // adding following 2 claim just for supporting default antiforgery provider
              new Claim(ClaimTypes.NameIdentifier, oUser.CellNum),
              new Claim("http://schemas.microsoft.com/accesscontrolservice/2010/07/claims/identityprovider", "ASP.NET Identity", "http://www.w3.org/2001/XMLSchema#string"),

              new Claim(ClaimTypes.Name,oUser.CellNum),

              // optionally you could add roles if any
              new Claim(ClaimTypes.Role, role.Name),

                      },
                      DefaultAuthenticationTypes.ApplicationCookie);

                    HttpContext.GetOwinContext().Authentication.SignIn(
                       new AuthenticationProperties { IsPersistent = true }, ident);
                    return RedirectToLocal(returnUrl, role.Name); // auth succeed 
                }
                else
                {
                    // invalid username or password
                    TempData["WrongPass"] = "نام کاربری و یا کلمه عبور وارد شده صحیح نمی باشد.";
                }
            }
            // If we got this far, something failed, redisplay form
            LoginPageViewModel login = new LoginPageViewModel();
            login.login = model;
            LoginViewModel loginViewModel = new LoginViewModel();
            return View(loginViewModel);

        }

        private ActionResult RedirectToLocal(string returnUrl, string role)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            if (role.ToLower() == "factory")
                return RedirectToAction("List", "ProductRequest");

            if (role.ToLower() == "branch")
                return RedirectToAction("List", "Orders");

            return RedirectToAction("Index", "Products");
        }
        public ActionResult LogOff()
        {
            if (User.Identity.IsAuthenticated)
            {
                HttpContext.GetOwinContext().Authentication.SignOut();
            }
            return Redirect("/account/login");
        }


    }
}