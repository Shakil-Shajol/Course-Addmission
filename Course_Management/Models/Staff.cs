using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Course_Management.Models
{
    public class Staff : Person
    {
        public int StaffId { get; set; }
        public Designation Designation { get; set; }
    }

    public enum Designation
    {

    }
}