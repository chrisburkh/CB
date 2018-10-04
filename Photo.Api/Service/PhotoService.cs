using Microsoft.AspNetCore.Http;
using Photo.Api.Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Photo.Api.Service
{
    public class PhotoService : IPhotoService
    {
        private IPhotoRepository _repo;

        public PhotoService(IPhotoRepository photoRepository)
        {
            _repo = photoRepository;
        }

        public Stream GetFile(string parentId)
        {
            Stream s = _repo.GetFile(parentId);

            return s;


        }

        public async Task<string> WriteFile(string id, IFormFile formFile)
        {
            return await _repo.WriteFile(id, formFile);
        }
    }
}
