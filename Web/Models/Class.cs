using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CBAdmin.Models
{
    public class Class
    {
        [Key]
        public string Id { get; set; }

        [Required]
        public string Name { get; set; }


        public string CourseID { get; set; }

        public Course Course { get; set; }

        public string TeacherID { get; set; }

        public Teacher Teacher { get; set; }

        public ICollection<Student> Students { get; set; }

        public List<String> SelectedStudents { get; set; }

        public Class()
        {
            Students = new List<Student>();
            SelectedStudents = new List<string>();
        }
    }
}
