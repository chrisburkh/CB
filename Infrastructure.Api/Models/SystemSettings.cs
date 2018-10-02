using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Api.Models
{
    public class SystemSetting
    {

        public string Id { get; set; }

        public bool ResetDatabase { get; set; }

        public DateTime CreateTime { get; set; }

        public SystemSetting(string app)
        {
            Id = app;
        }
    }
}
