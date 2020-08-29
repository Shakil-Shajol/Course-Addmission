using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Course_Management.Authentication
{
    public class CustomSerializeModel
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<string> RoleName { get; set; }
    }
}