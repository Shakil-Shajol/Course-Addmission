using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Course_Management.Models
{
    public class Enrollment
    {
        [Key,Required,ForeignKey("Batch")]
        [Column(Order =1)]
        public int BatchId { get; set; }
        [Column(Order =2)]
        [Key,Required,ForeignKey("Trainee")]
        public int TraineeId { get; set; }
        public virtual Batch Batch { get; set; }
        public virtual Trainee Trainee { get; set; }
    }
}