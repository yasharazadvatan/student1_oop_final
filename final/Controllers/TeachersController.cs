using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using final.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace final.Controllers
{
    public class TeachersController : Controller
    {
        private readonly MyDbContext _context;

        public TeachersController(MyDbContext context)
        {
            _context = context;
        }

        // GET: Teachers
        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetString("Admin") == "true")
            {
                ViewBag.Admin = "true";
                return View(await _context.Teachers.ToListAsync());
            }
            ViewBag.Admin = "false";
            return RedirectToAction("Details", "Teachers", new RouteValueDictionary(new { id = HttpContext.Session.GetInt32("UserId") }));
        }

        // GET: Teachers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teacher = await _context.Teachers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (teacher == null)
            {
                return NotFound();
            }

            if (teacher.isAdmin)
            {
                ViewBag.Admin = "true";
            }
            else
            {
                ViewBag.Admin = "false";
            }

            return View(teacher);
        }

        // GET: Teachers/Create
        public IActionResult Create()
        {
            if (HttpContext.Session.GetString("Admin") == "false")
            {
                return NotFound();
            }

            return View();
        }

        // POST: Teachers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Family,TeacherNumber,Address,Tel,Mail,Password,Prefix,isAdmin")] Teacher teacher)
        {
            if (HttpContext.Session.GetString("Admin") == "false")
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _context.Add(teacher);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(teacher);
        }

        // GET: Teachers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teacher = await _context.Teachers.FindAsync(id);
            if (teacher == null)
            {
                return NotFound();
            }

            if (teacher.isAdmin)
            {
                ViewBag.Admin = "true";
            }
            else
            {
                ViewBag.Admin = "false";
            }

            return View(teacher);
        }

        // POST: Teachers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Family,TeacherNumber,Address,Tel,Mail,Password,Prefix,isAdmin")] Teacher teacher)
        {
            if (id != teacher.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (HttpContext.Session.GetString("Admin") == "false")
                    {
                        teacher.isAdmin = false;
                    }
                    _context.Update(teacher);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TeacherExists(teacher.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            if (teacher.isAdmin)
            {
                ViewBag.Admin = "true";
            }
            else
            {
                ViewBag.Admin = "false";
            }

            return View(teacher);
        }

        // GET: Teachers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teacher = await _context.Teachers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (teacher == null)
            {
                return NotFound();
            }

            if (teacher.isAdmin)
            {
                ViewBag.Admin = "true";
            }
            else
            {
                ViewBag.Admin = "false";
            }

            return View(teacher);
        }

        // POST: Teachers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var teacher = await _context.Teachers.FindAsync(id);
            _context.Teachers.Remove(teacher);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TeacherExists(int id)
        {
            return _context.Teachers.Any(e => e.Id == id);
        }
    }
}
