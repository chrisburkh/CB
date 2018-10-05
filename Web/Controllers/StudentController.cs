using Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CBAdmin.Models;
using CBAdmin.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Raven.Client.Documents.Linq;
using System.IO;
using static System.Net.Mime.MediaTypeNames;

namespace CBAdmin.Controllers
{
    public class StudentController : Controller
    {

        private IApiService<Student> _api;

        public StudentController(IApiService<Student> apiService)
        {
            _api = apiService;
            _api.SetBaseUrl(typeof(Student).Name.ToLower());
        }
        // GET: Student
        public async Task<IActionResult> Index(string SearchString)
        {
            return View(await _api.GetAll(SearchString));
        }

        // GET: Student/Details/5
        public async Task<ActionResult> Details(string id)
        {

            var student = await _api.Get(id);

            return View(student);
        }

        public async Task<ActionResult> RenderImage(string id)
        {
            byte[] photoBack = await _api.DownloadImage(id);

            return File(photoBack, "image/png");
        }

        // GET: Student/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Student/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Student student)
        {
            try
            {
                // TODO: we have to set it this way so that ravendb creates a nice uuid for us. Remove to a better place.
                if (student.Id == null)
                {
                    student.Id = string.Empty;
                }
                await _api.Create(student);

                return RedirectToAction("Index");
            }
            catch
            {
                return View(student);
            }
        }

        // GET: Student/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            return View(await _api.Get(id));
        }


        // POST: Student/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Student student)
        {


            try
            {
                _api.Write(student);
                _api.UploadImage(student.Id, student.Avatar);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Student/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            return View(await _api.Get(id));
        }

        // POST: Student/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(Student student)
        {
            await _api.Delete(student.Id);

            return RedirectToAction("Index");
        }



    }
}