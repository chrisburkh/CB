using Events;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Persons.Api.Models;
using Raven.Client.Documents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Persons.Api.Repository
{
    public class PersonsRepository<T> : IPersonsRepository<T>
    {
        private string _RavendbUrl;

        private DocumentStore _store;
        private readonly IBus _bus;

        private readonly string RavenDefault = "http://localhost:8080";

        public PersonsRepository(IConfiguration config, IBus bus)
        {
            _RavendbUrl = Environment.GetEnvironmentVariable("CBADMIN_ravendb_url");

            _store = CreateStore();
            _bus = bus;
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

        public async Task<IEnumerable<T>> GetAll()
        {
            var session = _store.OpenAsyncSession();

            var students = session.Query<T>().ToListAsync();

            return await students;
        }

        public async Task<T> Get(string Id)
        {
            var session = _store.OpenAsyncSession();

            return await session.LoadAsync<T>(Id);
        }

        public async Task<T> Write(T model)
        {
            var session = _store.OpenAsyncSession();

            await session.StoreAsync(model);

            await session.SaveChangesAsync();



            return model;
        }

        public async Task<string> Delete(string id)
        {
            var session = _store.OpenAsyncSession();

            var x = await session.LoadAsync<T>(id);
            session.Delete(x);
            await session.SaveChangesAsync();

            // per Message mögliche Bilder löschen

            await _bus.Publish<PhotoDeleteEvent>(new { id });

            return id;
        }
    }
}
