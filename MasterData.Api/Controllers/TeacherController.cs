using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Persons.Api.Models;
using Persons.Api.Repository;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Persons.Api.Controllers
{
    [Route("api/[controller]")]
    public class TeacherController : Controller
    {
        private IPersonsRepository<Teacher> _rep;

        public TeacherController(IPersonsRepository<Teacher> studentRepository)
        {
            _rep = studentRepository;
        }

        // GET: api/<controller>
        [HttpGet]
        public async Task<IEnumerable<Teacher>> Get()
        {
            return await _rep.GetAll();
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        public async Task<Teacher> Get(string id)
        {
            return await _rep.Get(id);
        }

        // POST api/<controller>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]Teacher model)
        {
            model = await _rep.Write(model);

            return Ok(model.Id);
        }

        // PUT api/<controller>/5
        [HttpPut]
        public async void Put([FromBody]Teacher model)
        {
            model = await _rep.Write(model);

        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public async void Delete(string id)
        {
            await _rep.Delete(id);

        }
    }
}
