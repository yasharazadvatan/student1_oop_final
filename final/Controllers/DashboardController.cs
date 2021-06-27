using final.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace final.Controllers
{
    public class DashboardController : Controller
    {
        private readonly MyDbContext _context;

        public DashboardController(MyDbContext context)
        {
            _context = context;
        }

        // GET: Dashboard
        public ActionResult Index()
        {
            string adminVal = HttpContext.Session.GetString("Admin");
            switch (adminVal)
            {
                case "true":
                    return View(nameof(AdminIndex));
                case "false":
                    string userType = HttpContext.Session.GetString("UserType");
                    if (userType == "teacher")
                    {
                        return View(nameof(TeacherIndex));
                    }
                    else
                    {
                        return View(nameof(StudentIndex));
                    }
                default:
                    break;
            }

            return View();
        }

        public ActionResult AdminIndex()
        {
            return View();
        }

        public ActionResult TeacherIndex()
        {
            return View();
        }

        public ActionResult StudentIndex()
        {
            return View();
        }

        // GET: Dashboard/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Dashboard/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Dashboard/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Dashboard/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Dashboard/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Dashboard/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Dashboard/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
