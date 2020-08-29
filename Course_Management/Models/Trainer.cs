using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Course_Management.Models
{
    public class Trainer : Person
    {
        public int TrainerId { get; set; }
        public virtual ICollection<Skill> Skills { get; set; }
        public virtual ICollection<Batch> Batches { get; set; }

    }
}