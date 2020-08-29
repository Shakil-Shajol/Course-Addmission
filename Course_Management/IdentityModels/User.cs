using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Course_Management.IdentityModels
{
    public class User
    {
        public int UserId { get; set; }
        [Required, StringLength(50, MinimumLength = 6),Index(IsUnique =true)]
        public string Username { get; set; }
        [Required, StringLength(50), Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Required, StringLength(50), Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Required, StringLength(100), EmailAddress]
        public string Email { get; set; }
        [Required, StringLength(100, MinimumLength = 6), DataType(DataType.Password)]
        public string Password { get; set; }
        public bool IsActive { get; set; }
        public Guid ActivationCode { get; set; }
        public string ActivationOtp { get; set; }
        public virtual ICollection<Role> Roles { get; set; }
    }
}