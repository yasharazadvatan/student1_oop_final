using final.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace final.VıewModels
{
    public class TeacherCourseAssistant
    {
        public TeacherCourse TeacherCourse { get; set; }
        public Teacher Teacher { get; set; }
        public Course Course { get; set; }
        public Student Assistant { get; set; }
    }
}
