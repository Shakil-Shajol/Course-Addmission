using Course_Management.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Course_Management.ViewModels
{
    public class PersonViewModel
    {
        [Required, StringLength(50), Display(Name = "Name")]
        public string Name { get; set; }
        [Required, StringLength(50), Display(Name = "Father Name")]
        public string FatherName { get; set; }
        [Required, StringLength(50), Display(Name = "Email"), EmailAddress]
        public string Email { get; set; }
        [Required]
        public Gender Gender { get; set; }
        [Required, StringLength(20), Display(Name = "Phone")]
        public string Phone { get; set; }
        [Required, StringLength(50), Display(Name = "Address"),DataType(DataType.MultilineText)]
        public string Address { get; set; }
        [Required, StringLength(50), Display(Name = "NID")]
        public string NID { get; set; }
        [Required, Column(TypeName = "date")]
        public DateTime BirthDate { get; set; }
        public string PhotoPath { get; set; }
        public HttpPostedFileBase Photo { get; set; }
    }
}