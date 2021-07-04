using final.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace final.Controllers
{
    public class HomeController : Controller
    {
        private readonly MyDbContext _context;

        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, MyDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> TeacherLogin(string mail, string password)
        {
            var t = await _context.Teachers.FirstOrDefaultAsync(m => m.Mail == mail && m.Password == password);

            if (t == null)
            {
                return NotFound();
            }

            if (t.isAdmin)
            {
                HttpContext.Session.SetString("Admin", "true");
                HttpContext.Session.SetString("UserType", "admin");
            }
            else
            {
                HttpContext.Session.SetString("Admin", "false");
                HttpContext.Session.SetString("UserType", "teacher");
            }

            HttpContext.Session.SetInt32("UserId", t.Id);
            HttpContext.Session.SetString("UserMail", t.Mail);

            return RedirectToAction("Index", "Dashboard");
        }

        [HttpPost]
        public async Task<IActionResult> StudentLogin(string mail, string password)
        {
            var s = await _context.Students.FirstOrDefaultAsync(m => m.Mail == mail && m.Password == password);

            if (s == null)
            {
                return NotFound();
            }

            HttpContext.Session.SetString("Admin", "false");
            HttpContext.Session.SetString("UserType", "student");
            HttpContext.Session.SetInt32("UserId", s.Id);
            HttpContext.Session.SetString("UserMail", s.Mail);

            return RedirectToAction("Index", "Dashboard");
        }
    }
}
