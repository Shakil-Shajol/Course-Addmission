using Course_Management.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Course_Management.ViewModels
{
    public class TrainerViewModel:PersonViewModel
    {
        public int TrainerId { get; set; }
        public ICollection<Skill> Skills { get; set; }
        public ICollection<int> SkillIds { get; set; }
    }
}