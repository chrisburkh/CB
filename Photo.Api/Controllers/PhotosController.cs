using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Photo.Api.Models;
using Photo.Api.Service;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Photo.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PhotosController : ControllerBase
    {
        private IPhotoService _service;
        private readonly IHostingEnvironment _hostingEnvironment;


        public PhotosController(IPhotoService photoService, IHostingEnvironment hostingEnvironment)
        {
            _service = photoService;

            _hostingEnvironment = hostingEnvironment;

        }


        // GET api/photos/5
        [HttpGet("{id}")]
        public ActionResult<IActionResult> Get(string id)
        {

            var input = _service.GetFile(id);

            using (MemoryStream ms = new MemoryStream())
            {
                if (input != null)
                {
                    return File(input, "application/octet-stream");
                }
                else
                {
                    return NotFound();
                }
            }

        }


        [HttpPost("{id}")]
        public async Task<IActionResult> Post(string id)
        {
            if (Request.HasFormContentType)
            {
                var form = Request.Form;
                foreach (var formFile in form.Files)
                {
                    Console.WriteLine(formFile.FileName);

                    await _service.WriteFile(id, formFile);
                    return Ok(formFile.FileName);
                }
            }
            return BadRequest();

        }
    }
}
