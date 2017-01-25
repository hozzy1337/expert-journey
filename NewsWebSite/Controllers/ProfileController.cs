using Microsoft.AspNet.Identity;
using NewsWebSite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NewsWebSite.Controllers
{
    public class ProfileController : Controller
    {
        readonly UserManager<User> manager;

        ProfileController(UserManager<User> manager)
        {
            this.manager = manager;
        }


        public ActionResult Register()
        {
            return Content("");
        }
        public ActionResult Login()
        {
            return Content("");
        }
    }
}