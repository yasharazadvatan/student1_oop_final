using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace final.Models
{
    public class Teacher
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Family { get; set; }
        public string TeacherNumber { get; set; }
        public string Address { get; set; }
        public string Tel { get; set; }
        public string Mail { get; set; }
        public string Password { get; set; }
        public string Prefix { get; set; }
        public bool isAdmin { get; set; }
        public IList<TeacherCourse> TeacherCourses { get; set; }
    }
}
