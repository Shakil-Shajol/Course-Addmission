using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Course_Management.Models
{
    public class Batch
    {
        public int BatchId { get; set; }
        [Required, StringLength(100), Display(Name = "Batch Name")]
        public string BatchName { get; set; }
        [Required,ForeignKey("Course")]
        public int CourseId { get; set; }
        [Required,ForeignKey("Trainer")]
        public int TrainerId { get; set; }
        [Required,Column(TypeName ="date")]
        public DateTime StartDate { get; set; }
        [Column(TypeName ="date")]
        public DateTime? EndDate { get; set; }
        [Required, Display(Name = "Trainee Limit")]
        public int TraineeLimit { get; set; }
        public string Status { get; set; }
        public virtual Course Course { get; set; }
        public virtual Trainer Trainer { get; set; }
        public virtual ICollection<Enrollment> Enrollments { get; set; }
    }
}