using CBAdmin.Data;
using CBAdmin.Models;
using Microsoft.Extensions.Configuration;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CBAdmin.Service
{
    public class Service<T> : IService<T>
    {
        private IConfiguration _config; // this gets the settings from JSON config file
        private IDocumentStore _db;

        public Service(IConfiguration config)
        {
            _config = config;

            _db = DocumentStoreHolder.Store;
        }

        public void DeleteEntity(String id)
        {
            var session = _db.OpenSession();
            var x = session.Load<T>(id);
            session.Delete<T>(x);

            session.SaveChanges();
        }

        public T GetEntity(String id)
        {
            var session = _db.OpenSession();

            var entity = session.Load<T>(id);

            return entity;
        }


        public async Task<IList<T>> GetEntityListAsynch()
        {
            var session = _db.OpenAsyncSession();

            var students = session.Query<T>().ToListAsync();

            return await students;
        }

        public void WriteEntity(T entity)
        {

            var session = _db.OpenSession();

            session.Store(entity);
            session.SaveChanges();
        }

        public IDocumentSession GetSession()
        {
            return _db.OpenSession();


        }
    }
}
