using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Photo.Api.Models;
using Raven.Client.Documents;
using Raven.Client.Documents.Operations.Attachments;

namespace Photo.Api.Repository
{
    public class PhotoRepository : IPhotoRepository
    {

        private string _RavendbUrl;

        private DocumentStore _store;

        private readonly string RavenDefault = "http://localhost:8080";

        public PhotoRepository(IConfiguration config)
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

        public async Task<string> WriteFile(string id, IFormFile formFile)
        {
            using (var session = _store.OpenSession())
            {

                var model = new PhotoModel
                {
                    ParentId = id,
                    bytes = formFile.Length,
                    Id = string.Empty,
                    Name = formFile.FileName
                };

                List<PhotoModel> list = session.Query<PhotoModel>().Where(x => x.ParentId == id).ToList();

                if (list.Count == 1)
                {
                    // existiert schon also überschreiben

                    var existing = session.Load<PhotoModel>(list[0].Id);
                    session.Delete(existing);
                    session.SaveChanges();
                }

                session.Store(model);
                session.SaveChanges();



                Stream s = formFile.OpenReadStream();

                session.Advanced.Attachments.Store(model, formFile.FileName, s, "image/jpeg");

                session.SaveChanges();

                return model.ParentId;
            }
        }

        public Stream GetFile(string parentId)
        {
            var session = _store.OpenSession();

            List<PhotoModel> list = session.Query<PhotoModel>().Where(x => x.ParentId == parentId).ToList();

            if (list.Count == 1)
            {
                PhotoModel m = list[0];

                AttachmentResult r = session.Advanced.Attachments.Get(m, m.Name);
                return r.Stream;
            }
            else
            {
                return null;
            }
        }
    }
}
