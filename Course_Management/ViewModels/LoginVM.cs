using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Course_Management.ViewModels
{
    public class LoginVM
    {
        [Required, StringLength(50, MinimumLength = 6),Display(Name = "Username")]
        public string UserName { get; set; }
        [Required, StringLength(100, MinimumLength = 6), DataType(DataType.Password)]
        public string Password { get; set; }
    }
}