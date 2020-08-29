using Course_Management.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Course_Management.ViewModels
{
    public class BatchReportViewModel
    {
        public string BatchName { get; set; }
        public string CourseTitle { get; set; }
        public string Trainer { get; set; }
        public string StartDate { get; set; }
        public int TraineeId { get; set; }
        public string UserName { get; set; }
        public virtual ICollection<Enrollment> Enrollments { get; set; }

        public string Name { get; set; }

        public string FatherName { get; set; }

        public string Email { get; set; }

        public Gender Gender { get; set; }

        public string Phone { get; set; }

        public string Address { get; set; }

        public string NID { get; set; }

        public DateTime BirthDate { get; set; }
        public string PhotoPath { get; set; }
    }
}