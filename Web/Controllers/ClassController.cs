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

        private IApiService<Class> _api;

        public ClassController(IApiService<Class> apiService)
        {

            _api = apiService;
            _api.SetBaseUrl(typeof(Class).Name.ToLower());
        }
        // GET: Class
        public async Task<IActionResult> Index()
        {
            return View(await _api.GetAllClasses());

        }

        // GET: Class/Details/5
        public async Task<ActionResult> Details(string id)
        {

            var clazz = await _api.Get(id);

            return View(clazz);

        }

        // GET: Class/Create
        public async Task<ActionResult> Create()
        {
            var listTeacher = await _api.GetAllTeacher();

            ViewData["TeacherID"] = new SelectList(listTeacher, "Id", "FullName");

            var listCourse = await _api.GetAllCourse();

            ViewData["CourseID"] = new SelectList(listCourse, "Id", "Subject");
            return View();
        }

        // POST: Class/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Class claz)
        {
            try
            {
                // TODO: we have to set it this way so that ravendb creates a nice uuid for us. Remove to a better place.
                if (claz.Id == null)
                {
                    claz.Id = string.Empty;
                }


                await _api.Write(claz);

                return RedirectToAction("Index");
            }
            catch
            {
                return View(claz);
            }
        }

        // GET: Class/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            var clazz = await _api.Get(id);



            var listTeacher = await _api.GetAllTeacher();
            var listCourse = await _api.GetAllCourse();

            ViewData["TeacherID"] = new SelectList(listTeacher, "Id", "FullName", clazz.TeacherID);
            ViewData["CourseID"] = new SelectList(listCourse, "Id", "Subject", clazz.CourseID);

            clazz.SelectedStudents = new List<string>();
            foreach (Student s in clazz.Students)
            {
                clazz.SelectedStudents.Add(new Student().Id = s.Id);
            }

            List<Student> listStudent = _api.GetAllStudent().Result.ToList();

            clazz.Students = listStudent;

            return View(clazz);

        }

        // POST: Class/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Class clazz)
        {
            try
            {
                clazz.Students = new List<Student>();

                foreach (String id in clazz.SelectedStudents)
                {

                    Student s = new Student();
                    s.Id = id;
                    clazz.Students.Add(s);
                };



                await _api.Write(clazz);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }



        // GET: Class/Delete/5
        public ActionResult Delete(string id)
        {
            var student = _api.Get(id);

            return View();
        }

        // POST: Class/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(Class claz)
        {
            await _api.Delete(claz.Id);

            return RedirectToAction("Index");
        }
    }
}