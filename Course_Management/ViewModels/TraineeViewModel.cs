using Course_Management.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Course_Management.ViewModels
{
    public class TraineeViewModel:PersonViewModel
    {
        public int TraineeId { get; set; }
        public string UserName { get; set; }
    }
}