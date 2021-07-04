using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using final.Models;
using Microsoft.AspNetCore.Http;

namespace final.Controllers
{
    public class PlansController : Controller
    {
        private readonly MyDbContext _context;

        public PlansController(MyDbContext context)
        {
            _context = context;
        }

        // GET: Plans
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("Admin") == "true")
            {
                ViewBag.UserType = "admin";
                var myDbContext = from a in _context.Plans.ToList()
                                  join b in _context.Students on a.StudentId equals b.Id
                                  join c in _context.TeacherCourses on a.TeacherCourseId equals c.Id
                                  join d in _context.Teachers on c.TeacherId equals d.Id
                                  join e in _context.Courses on c.CourseId equals e.Id
                                  select new Plan
                                  {
                                      Student = b,
                                      TeacherCourse = c,
                                      Id = a.Id
                                  };
                return View(myDbContext);
            }
            else
            {
                if (HttpContext.Session.GetString("UserType") == "teacher")
                {
                    ViewBag.UserType = "teacher";
                    var myDbContext = from a in _context.Plans.ToList()
                                      join b in _context.Students on a.StudentId equals b.Id
                                      join c in _context.TeacherCourses on a.TeacherCourseId equals c.Id
                                      join d in _context.Teachers on c.TeacherId equals d.Id
                                      join e in _context.Courses on c.CourseId equals e.Id
                                      where d.Id == HttpContext.Session.GetInt32("UserId")
                                      select new Plan
                                      {
                                          Student = b,
                                          TeacherCourse = c,
                                          Id = a.Id
                                      };
                    return View(myDbContext);
                }
                else
                {
                    ViewBag.UserType = "student";
                    var myDbContext = from a in _context.Plans.ToList()
                                      join b in _context.Students on a.StudentId equals b.Id
                                      join c in _context.TeacherCourses on a.TeacherCourseId equals c.Id
                                      join d in _context.Teachers on c.TeacherId equals d.Id
                                      join e in _context.Courses on c.CourseId equals e.Id
                                      where b.Id == HttpContext.Session.GetInt32("UserId")
                                      select new Plan
                                      {
                                          Student = b,
                                          TeacherCourse = c,
                                          Id = a.Id
                                      };
                    return View(myDbContext);
                }
            }

        }

        // GET: Plans/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var plan = await _context.Plans
                .Include(p => p.Student)
                .Include(p => p.TeacherCourse)
                .ThenInclude(a => a.Teacher)
                .Include(p => p.TeacherCourse)
                .ThenInclude(b => b.Course)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (plan == null)
            {
                return NotFound();
            }

            return View(plan);
        }

        // GET: Plans/Create
        public IActionResult Create()
        {
            ViewData["StudentId"] = new SelectList(_context.Students, "Id", "Mail");
            var teacherCourse = from a in _context.TeacherCourses.ToList()
                                join b in _context.Teachers.ToList() on a.TeacherId equals b.Id
                                join c in _context.Courses.ToList() on a.CourseId equals c.Id
                                select new
                                {
                                    Id = a.Id,
                                    Merged = b.Mail + " - " + c.Title
                                };
            ViewData["TeacherCourseId"] = new SelectList(teacherCourse, "Id", "Merged");
            return View();
        }

        // POST: Plans/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,StudentId,TeacherCourseId,Not,isPassed")] Plan plan)
        {
            if (ModelState.IsValid)
            {
                _context.Add(plan);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["StudentId"] = new SelectList(_context.Students, "Id", "Mail", plan.StudentId);
            var teacherCourse = from a in _context.TeacherCourses.ToList()
                                join b in _context.Teachers.ToList() on a.TeacherId equals b.Id
                                join c in _context.Courses.ToList() on a.CourseId equals c.Id
                                select new
                                {
                                    Id = a.Id,
                                    Merged = b.Mail + " - " + c.Title
                                };
            ViewData["TeacherCourseId"] = new SelectList(teacherCourse, "Id", "Merged", plan.TeacherCourseId);
            return View(plan);
        }

        // GET: Plans/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var plan = await _context.Plans.FindAsync(id);
            if (plan == null)
            {
                return NotFound();
            }
            ViewData["StudentId"] = new SelectList(_context.Students, "Id", "Mail", plan.StudentId);
            var teacherCourse = from a in _context.TeacherCourses.ToList()
                                join b in _context.Teachers.ToList() on a.TeacherId equals b.Id
                                join c in _context.Courses.ToList() on a.CourseId equals c.Id
                                select new
                                {
                                    Id = a.Id,
                                    Merged = b.Mail + " - " + c.Title
                                };
            ViewData["TeacherCourseId"] = new SelectList(teacherCourse, "Id", "Merged", plan.TeacherCourseId);
            return View(plan);
        }

        // POST: Plans/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,StudentId,TeacherCourseId,Not,isPassed")] Plan plan)
        {
            if (id != plan.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(plan);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PlanExists(plan.Id))
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
            ViewData["StudentId"] = new SelectList(_context.Students, "Id", "Mail", plan.StudentId);
            var teacherCourse = from a in _context.TeacherCourses.ToList()
                                join b in _context.Teachers.ToList() on a.TeacherId equals b.Id
                                join c in _context.Courses.ToList() on a.CourseId equals c.Id
                                select new
                                {
                                    Id = a.Id,
                                    Merged = b.Mail + " - " + c.Title
                                };
            ViewData["TeacherCourseId"] = new SelectList(teacherCourse, "Id", "Merged", plan.TeacherCourseId);
            return View(plan);
        }

        // GET: Plans/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var plan = await _context.Plans
                .Include(p => p.Student)
                .Include(p => p.TeacherCourse)
                .ThenInclude(a => a.Teacher)
                .Include(p => p.TeacherCourse)
                .ThenInclude(b => b.Course)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (plan == null)
            {
                return NotFound();
            }

            return View(plan);
        }

        // POST: Plans/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var plan = await _context.Plans.FindAsync(id);
            _context.Plans.Remove(plan);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PlanExists(int id)
        {
            return _context.Plans.Any(e => e.Id == id);
        }
    }
}
