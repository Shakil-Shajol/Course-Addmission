using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Course_Management.ViewModels
{
    public class RegistrationVM
    {
        [Required, StringLength(50, MinimumLength = 6)]
        public string Username { get; set; }
        [Required, StringLength(50), Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Required, StringLength(50), Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Required, StringLength(100), EmailAddress]
        public string Email { get; set; }
        [Required, StringLength(100, MinimumLength = 6), DataType(DataType.Password)]
        public string Password { get; set; }
        [Required, StringLength(100, MinimumLength = 6), DataType(DataType.Password), Compare("Password")]
        public string ConfirmPassword { get; set; }
    }
}