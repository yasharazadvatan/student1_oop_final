﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace final.Models
{
    public class Course
    {
        public int Id { get; set; }
        public string Title { get; set; }

        // 1 yuksek lisant, 2 Doktora, 3 Yuksek Lisans and Doctora
        public int CourseType { get; set; }

        public virtual ICollection<TeacherCourse> TeacherCourses { get; set; }
    }
}
