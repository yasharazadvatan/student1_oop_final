using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using final.Models;
using Microsoft.AspNetCore.Http;
using final.VıewModels;

namespace final.Controllers
{
    public class TeacherCoursesController : Controller
    {
        private readonly MyDbContext _context;

        public TeacherCoursesController(MyDbContext context)
        {
            _context = context;
        }

        // GET: TeacherCourses
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("Admin") == "true")
            {
                ViewBag.Admin = "true";
                var tca = from a in _context.TeacherCourses.ToList()
                          join b in _context.Courses.ToList() on a.CourseId equals b.Id
                          join c in _context.Teachers.ToList() on a.TeacherId equals c.Id
                          join d in _context.Students.ToList() on a.ResearchAssistantId equals d.Id
                          select new TeacherCourseAssistant
                          {
                              Assistant = d,
                              Teacher = c,
                              Course = b,
                              TeacherCourse = a
                          };
                return View(tca);
            }
            ViewBag.Admin = "false";
            var tca1 = from a in _context.TeacherCourses.ToList()
                       join b in _context.Courses.ToList() on a.CourseId equals b.Id
                       join c in _context.Teachers.ToList() on a.TeacherId equals c.Id
                       join d in _context.Students.ToList() on a.ResearchAssistantId equals d.Id
                       where c.Id == HttpContext.Session.GetInt32("UserId")
                       select new TeacherCourseAssistant
                       {
                           Assistant = d,
                           Teacher = c,
                           Course = b,
                           TeacherCourse = a
                       };
            return View(tca1);
        }


        // GET: TeacherCourses/Create
        public IActionResult GetStudents(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            if (HttpContext.Session.GetString("Admin") == "true")
            {
                ViewBag.Admin = "true";
            }
            else
            {
                ViewBag.Admin = "false";
            }

            var x = _context.Plans.Include(x => x.Student).Include(a => a.TeacherCourse).Where(x => x.TeacherCourseId == id);
            return View(x);
        }

        // GET: TeacherCourses/Create
        public IActionResult Create()
        {
            ViewData["CourseId"] = new SelectList(_context.Courses, "Id", "Title");
            ViewData["TeacherId"] = new SelectList(_context.Teachers, "Id", "Mail");
            ViewData["ResearchAssistantId"] = new SelectList(_context.Students.Where(x => x.isAssistant == true), "Id", "Mail");
            return View();
        }

        // POST: TeacherCourses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CourseId,TeacherId,ResearchAssistantId")] TeacherCourse teacherCourse)
        {
            if (ModelState.IsValid)
            {
                _context.Add(teacherCourse);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CourseId"] = new SelectList(_context.Courses, "Id", "Title");
            ViewData["TeacherId"] = new SelectList(_context.Teachers, "Id", "Mail");
            ViewData["ResearchAssistantId"] = new SelectList(_context.Students.Where(x => x.isAssistant == true), "Id", "Mail");
            return View(teacherCourse);
        }

        // GET: TeacherCourses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teacherCourse = await _context.TeacherCourses.FindAsync(id);
            if (teacherCourse == null)
            {
                return NotFound();
            }
            ViewData["CourseId"] = new SelectList(_context.Courses, "Id", "Title");
            ViewData["TeacherId"] = new SelectList(_context.Teachers, "Id", "Mail");
            ViewData["ResearchAssistantId"] = new SelectList(_context.Students.Where(x => x.isAssistant == true), "Id", "Mail");
            return View(teacherCourse);
        }

        // POST: TeacherCourses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CourseId,TeacherId,ResearchAssistantId")] TeacherCourse teacherCourse)
        {
            if (id != teacherCourse.CourseId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(teacherCourse);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TeacherCourseExists(teacherCourse.CourseId))
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
            ViewData["CourseId"] = new SelectList(_context.Courses, "Id", "Title");
            ViewData["TeacherId"] = new SelectList(_context.Teachers, "Id", "Mail");
            ViewData["ResearchAssistantId"] = new SelectList(_context.Students.Where(x => x.isAssistant == true), "Id", "Mail");
            return View(teacherCourse);
        }

        // GET: TeacherCourses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teacherCourse = await _context.TeacherCourses
                .Include(t => t.Course)
                .Include(t => t.Teacher)
                .FirstOrDefaultAsync(m => m.CourseId == id);
            if (teacherCourse == null)
            {
                return NotFound();
            }

            var assistant = from a in _context.TeacherCourses.ToList()
                            join b in _context.Students.ToList() on a.ResearchAssistantId equals b.Id
                            where a.CourseId == id
                            select b;
            ViewData["ResearchAssistantId"] = assistant.FirstOrDefault().Mail;

            return View(teacherCourse);
        }

        // POST: TeacherCourses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var teacherCourse = await _context.TeacherCourses.FindAsync(id);
            _context.TeacherCourses.Remove(teacherCourse);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TeacherCourseExists(int id)
        {
            return _context.TeacherCourses.Any(e => e.CourseId == id);
        }
    }
}
