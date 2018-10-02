using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Photo.Api.Repository
{
    public interface IPhotoRepository
    {
        string WriteFile(string id, IFormFile formFile);

        Stream GetFile(string parentId);
    }
}
