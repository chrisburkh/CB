using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Photo.Api.Models
{
    public class PhotoModel
    {
        public long bytes { get; set; }

        public string Id { get; set; }

        public string Name { get; set; }

        public string ParentId { get; set; }

        public IFormFile Avatar { get; set; }

    }
}
