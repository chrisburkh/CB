using CBAdmin.Models;
using CBAdmin.Service;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace CBAdmin.Controllers
{
    public class HomeController : Controller

    {
        private IDatabaseService _databaseService;

        private IApiService<SystemInformation> _apiService;

        public HomeController(IDatabaseService databaseService, IApiService<SystemInformation> apiService)

        {
            _databaseService = databaseService;
            _apiService = apiService;
        }


        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var info = await _apiService.GetSystemInformation();

            SystemInformation s = new SystemInformation();

            s.Message = info;

            return View(s);

        }

        [HttpPost]
        public async Task<IActionResult> ResetDataBase()
        {
            await _databaseService.ResetDatabase();

            Console.WriteLine("finished");

            //throw new Exception("test");

            return RedirectToAction("Index");
        }



        public IActionResult About()

        {

            ViewData["Message"] = "Your application description page.";



            return View();

        }



        public IActionResult Contact()

        {

            ViewData["Message"] = "Your contact page.";



            return View();

        }





        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]

        public IActionResult Error()

        {

            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });

        }

    }
}
