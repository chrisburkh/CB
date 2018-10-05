using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Persons.Api.Models
{
    public class Student
    {

        [Key]
        public string Id { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string FirstMidName { get; set; }

        public DateTime DateOfBirth { get; set; }

    }
}
