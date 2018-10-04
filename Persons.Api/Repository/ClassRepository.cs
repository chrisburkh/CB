using MastertData.Models;
using Microsoft.Extensions.Configuration;
using Persons.Api.Models;
using Raven.Client.Documents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Persons.Api.Repository
{
    public class ClassRepository : IClassRepository
    {
        private string _RavendbUrl;

        private DocumentStore _store;

        private readonly string RavenDefault = "http://localhost:8080";

        public ClassRepository(IConfiguration config)
        {
            _RavendbUrl = Environment.GetEnvironmentVariable("CBADMIN_ravendb_url");

            _store = CreateStore();
        }

        private DocumentStore CreateStore()
        {

            if (string.IsNullOrEmpty(_RavendbUrl))
            {
                _RavendbUrl = RavenDefault;
            }

            //WaitForDatabaseStart();

            DocumentStore store = new DocumentStore()
            {
                Urls = new[] { _RavendbUrl },
                Database = "DockerDB"
            };

            store.Initialize();

            return store;
        }

        public IEnumerable<Class> GetAll()
        {
            var session = _store.OpenSession();

            var list = session.Query<Class>().ToList();



            var clazzes = session.Query<Class>().Include(r => r.TeacherID).Include(r => r.CourseID).ToList<Class>();

            foreach (Class clazz in clazzes)
            {
                clazz.Teacher = session.Load<Teacher>(clazz.TeacherID);
                clazz.Course = session.Load<Course>(clazz.CourseID);
            }

            clazzes = clazzes.OrderBy(x => x.Course.Abbreviation).ToList();

            return clazzes;
        }

        public async Task<Class> Get(string Id)
        {
            var session = _store.OpenAsyncSession();

            var clazz = await session.Include("TeacherID").Include("CourseID").LoadAsync<Class>(Id);

            clazz.Teacher = await session.LoadAsync<Teacher>(clazz.TeacherID);
            clazz.Course = await session.LoadAsync<Course>(clazz.CourseID);

            List<Student> listWithAllDetails = new List<Student>();
            foreach (Student s in clazz.Students)
            {
                Student loaded = await session.LoadAsync<Student>(s.Id);
                listWithAllDetails.Add(loaded);
            }

            clazz.Students = listWithAllDetails;

            return await session.LoadAsync<Class>(Id);
        }

        public async Task<Class> Write(Class model)
        {
            var session = _store.OpenAsyncSession();

            await session.StoreAsync(model);

            await session.SaveChangesAsync();



            return model;
        }

        public async Task<string> Delete(string id)
        {
            var session = _store.OpenAsyncSession();

            var x = await session.LoadAsync<Class>(id);
            session.Delete(x);
            await session.SaveChangesAsync();

            return id;
        }
    }
}
