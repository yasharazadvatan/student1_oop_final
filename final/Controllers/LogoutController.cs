using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace final.Controllers
{
    public class LogoutController : Controller
    {
        // GET: LogoutController
        public ActionResult logout()
        {
            HttpContext.Session.Remove("Admin");
            HttpContext.Session.Remove("UserType");
            HttpContext.Session.Remove("UserId");
            HttpContext.Session.Remove("UserMail");
            HttpContext.Session.Clear();

            return RedirectToAction("Index", "Home");
        }
    }
}
