using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Course_Management.Models
{
    public class Trainee : Person
    {
        public int TraineeId { get; set; }
        public string UserName { get; set; }
        public virtual ICollection<Enrollment> Enrollments { get; set; }
    }
}