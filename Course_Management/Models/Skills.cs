using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Course_Management.Models
{
    public class Skill
    {
        public int SkillId { get; set; }
        [Required, StringLength(50), Display(Name = "Skill")]
        public string SkillTitle { get; set; }
        public virtual ICollection<Trainer> Trainers { get; set; }
    }
}