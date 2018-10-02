using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CBAdmin.Models;
using CBAdmin.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CBAdmin.Controllers
{
    public class TeacherController : Controller
    {
        private IApiService<Teacher> _api;

        public TeacherController(IApiService<Teacher> apiService)
        {
            _api = apiService;
            _api.SetBaseUrl(typeof(Teacher).Name.ToLower());
        }
        // GET: Teacher
        public async Task<IActionResult> Index()
        {
            return View(await _api.GetAll());
        }

        // GET: Teacher/Details/5
        public async Task<ActionResult> Details(string id)
        {
            return View(await _api.Get(id));
        }

        // GET: Teacher/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Student/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Teacher teacher)
        {
            try
            {
                // TODO: we have to set it this way so that ravendb creates a nice uuid for us. Remove to a better place.
                if (teacher.Id == null)
                {
                    teacher.Id = string.Empty;
                }
                await _api.Create(teacher);

                return RedirectToAction("Index");
            }
            catch
            {
                return View(teacher);
            }
        }

        // GET: Teacher/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            return View(await _api.Get(id));

        }

        // POST: Teacher/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Teacher teacher)
        {
            try
            {
                await _api.Write(teacher);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Teacher/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            return View(await _api.Get(id));
        }

        // POST: Teacher/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(Teacher teacher)
        {

            await _api.Delete(teacher.Id);

            return RedirectToAction("Index");

        }
    }
}