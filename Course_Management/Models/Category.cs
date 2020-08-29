using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Course_Management.Models
{
    public class Category
    {
        public int CategoryId { get; set; }
        [Required, StringLength(50), Display(Name = "Category")]
        public string CategoryTitle { get; set; }
        public virtual ICollection<Course> Courses { get; set; }
    }
}