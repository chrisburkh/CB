using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CBAdmin.Models
{
    public class Course
    {
        [Key]
        public string Id { get; set; }
        [Required]
        public string Subject { get; set; }


        public string Abbreviation { get; set; }

    }
}
