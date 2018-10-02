using Raven.Client.Documents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace CBAdmin.Data
{
    public class DocumentStoreHolder


    {
        private static string raven_url = "http://localhost:8080";

        private static Lazy<DocumentStore> store = new Lazy<DocumentStore>(CreateStore);

        public static DocumentStore Store => store.Value;

        private static DocumentStore CreateStore()
        {

            string ravenEnv = Environment.GetEnvironmentVariable("CBADMIN_ravendb_url");

            Console.WriteLine("Connecting to Ravendb at: " + ravenEnv);

            if (!string.IsNullOrEmpty(ravenEnv))
            {
                raven_url = ravenEnv;
            }

            WaitForDatabaseStart();

            DocumentStore store = new DocumentStore()
            {
                Urls = new[] { raven_url },
                Database = "DockerDB"
            };

            store.Initialize();

            return store;
        }

        private static void WaitForDatabaseStart()
        {
            int counter = 10;
            Console.WriteLine("Waiting for Ravendb to start on: " + raven_url);
            while (counter != 0)
            {
                Console.Write(".");
                System.Threading.Thread.Sleep(1000);


                counter--;
            }
        }
    }
}
