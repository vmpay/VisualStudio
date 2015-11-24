﻿using Template.Components.Security;
using Template.Services;
using System.Web.Mvc;

namespace Template.Controllers
{
    [AllowUnauthorized]
    public class HomeController : ServicedController<IAccountService>
    {
        public HomeController(IAccountService service)
            : base(service)
        {
        }

        [HttpGet]
        public ActionResult Index()
        {
            if (!Service.IsActive(CurrentAccountId))
                return RedirectIfAuthorized("Logout", "Auth");

            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Error()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult NotFound()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Unauthorized()
        {
            if (!Service.IsActive(CurrentAccountId))
                return RedirectIfAuthorized("Logout", "Auth");

            return View();
        }
    }
}
