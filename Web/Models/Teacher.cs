using Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CBAdmin.Models
{
    public class Teacher
    {
        [Key]
        public string Id { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string FirstMidName { get; set; }

        public DateTime DateOfBirth { get; set; }

        [Display(Name = "Full Name")]
        public string FullName
        {
            get
            {
                return LastName + ", " + FirstMidName;
            }
        }

        public GenderType Gender { get; set; }

        [DisplayName("beschäftigt seit")]
        public DateTime Employment_Commence { get; set; }

        [DisplayName("Besoldungsgrad")]
        public SalaryType Salary { get; set; }


    }
}
