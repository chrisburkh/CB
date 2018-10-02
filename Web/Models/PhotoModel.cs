using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Models
{
    public class PhotoModel
    {
        string Id { get; set; }

        public IFormFile Avatar { get; set; }

        public PhotoModel(string id, IFormFile file)
        {
            Id = id;
            Avatar = file;
        }

    }
}
