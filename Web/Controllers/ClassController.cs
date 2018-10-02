using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CBAdmin.Models;
using CBAdmin.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CBAdmin.Controllers
{
    public class ClassController : Controller
    {
        private IService<Class> _service;

        public ClassController(IService<Class> classService)
        {
            _service = classService;
        }
        // GET: Class
        public async Task<IActionResult> Index()
        {

            var list = (await _service.GetEntityListAsynch());

            var session = _service.GetSession();

            var clazzes = session.Query<Class>().Include(r => r.TeacherID).Include(r => r.CourseID).ToList<Class>();

            foreach (Class clazz in clazzes)
            {
                clazz.Teacher = session.Load<Teacher>(clazz.TeacherID);
                clazz.Course = session.Load<Course>(clazz.CourseID);
            }

            clazzes = clazzes.OrderBy(x => x.Course.Abbreviation).ToList();

            return View(clazzes);

        }

        // GET: Class/Details/5
        public ActionResult Details(string id)
        {
            using (var session = _service.GetSession())
            {

                var clazz = session.Include("TeacherID").Include("CourseID").Load<Class>(id);

                clazz.Teacher = session.Load<Teacher>(clazz.TeacherID);
                clazz.Course = session.Load<Course>(clazz.CourseID);

                return View(clazz);
            }
        }

        // GET: Class/Create
        public ActionResult Create()
        {

            var session = _service.GetSession();

            var listTeacher = session.Query<Teacher>().ToList();

            ViewData["TeacherID"] = new SelectList(listTeacher, "Id", "FullName");

            var listCourse = session.Query<Course>().ToList();

            ViewData["CourseID"] = new SelectList(listCourse, "Id", "Subject");
            return View();
        }

        // POST: Class/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Class claz)
        {
            try
            {
                // TODO: we have to set it this way so that ravendb creates a nice uuid for us. Remove to a better place.
                if (claz.Id == null)
                {
                    claz.Id = string.Empty;
                }


                _service.WriteEntity(claz);

                return RedirectToAction("Index");
            }
            catch
            {
                return View(claz);
            }
        }

        // GET: Class/Edit/5
        public ActionResult Edit(string id)
        {
            var clazz = _service.GetEntity(id);

            var session = _service.GetSession();

            var listTeacher = session.Query<Teacher>().ToList();
            var listCourse = session.Query<Course>().ToList();

            ViewData["TeacherID"] = new SelectList(listTeacher, "Id", "FullName", clazz.TeacherID);
            ViewData["CourseID"] = new SelectList(listCourse, "Id", "Subject", clazz.CourseID);

            clazz.SelectedStudents = new List<string>();
            foreach (Student s in clazz.Students)
            {
                clazz.SelectedStudents.Add(new Student().Id = s.Id);
            }

            List<Student> listStudent = session.Query<Student>().ToList();

            clazz.Students = listStudent;

            return View(clazz);

        }

        // POST: Class/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Class clazz)
        {
            try
            {
                clazz.Students = new List<Student>();
                var session = _service.GetSession();

                foreach (String id in clazz.SelectedStudents)
                {
                    var student = session.Load<Student>(id);
                    clazz.Students.Add(student);
                }


                _service.WriteEntity(clazz);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        private void PopulateDepartmentsDropDownList(object selectedDepartment = null)
        {

            var session = _service.GetSession();

            var list = session.Query<Teacher>().ToList();

            ViewBag.Teacher = new SelectList(list, "Id", "FullName", selectedDepartment);
        }

        // GET: Class/Delete/5
        public ActionResult Delete(string id)
        {
            var student = _service.GetEntity(id);

            return View();
        }

        // POST: Class/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Class claz)
        {


            _service.DeleteEntity(claz.Id);

            return RedirectToAction("Index");

        }
    }
}