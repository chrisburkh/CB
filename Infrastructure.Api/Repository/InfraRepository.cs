using Microsoft.Extensions.Configuration;
using Raven.Client.Documents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Api.Models;
using System.Net;

namespace Infrastructure.Api.Repository
{
    public class InfraRepository : IInfraRepository
    {
        private string _RavendbUrl;

        private DocumentStore _store;

        private readonly string RavenDefault = "http://localhost:8080";

        public InfraRepository(IConfiguration config)
        {
            _RavendbUrl = Environment.GetEnvironmentVariable("CBADMIN_ravendb_url");

            _store = CreateStore();
        }

        private DocumentStore CreateStore()
        {
            Console.WriteLine("Connecting to Ravendb at: " + _RavendbUrl);
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

        public async Task<string> GetInformation()
        {
            var session = _store.OpenAsyncSession();

            var settings = new SystemSetting("CBAdmin");

            var fromDB = await session.LoadAsync<SystemSetting>(settings.Id);

            IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName()); // `Dns.Resolve()` method is deprecated.
            IPAddress ipAddress = ipHostInfo.AddressList[0];

            if (fromDB == null)
            {
                return "no settings found";
            }
            else
            {
                return "created at: " + fromDB.CreateTime + " with the server " + ipAddress;
            }
        }
    }
}
