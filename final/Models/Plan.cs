using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace final.Models
{
    public class Plan
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public Student Student { get; set; }
        public int TeacherCourseId { get; set; }
        public TeacherCourse TeacherCourse { get; set; }
        public double Not { get; set; }
        public bool isPassed { get; set; }
    }
}
