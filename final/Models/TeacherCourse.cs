using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace final.Models
{
    public class TeacherCourse
    {
        public int Id { get; set; }
        public int CourseId { get; set; }
        public Course Course { get; set; }
        public int TeacherId { get; set; }
        public Teacher Teacher { get; set; }
        public int ResearchAssistantId { get; set; }
        public IList<Plan> Plans { get; set; }
    }
}
