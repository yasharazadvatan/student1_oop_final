using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace final.Models
{
    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Family { get; set; }
        public string StudentNumber { get; set; }
        public string Address { get; set; }
        public string Tel { get; set; }
        public string Mail { get; set; }
        public string Password { get; set; }
        public int ConselorId { get; set; }
        public ICollection<Plan> Plans { get; set; }
    }
}
