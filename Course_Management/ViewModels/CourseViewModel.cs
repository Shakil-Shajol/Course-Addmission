using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Course_Management.ViewModels
{
    public class CourseViewModel
    {
        public int CourseId { get; set; }
        [Required, StringLength(50), Display(Name = "Course Title")]
        public string CourseTitle { get; set; }
        [Required, StringLength(2000), Display(Name = "Overview"),DataType(DataType.MultilineText)]
        public string Overview { get; set; }
        [Required, ForeignKey("Category"), Display(Name = "Category")]
        public int CategoryId { get; set; }
        [Required]
        public int Duration { get; set; }
        [Required]
        public int Quizes { get; set; }
        [Required]
        public int Assesments { get; set; }
        [Required, Column(TypeName = "money")]
        public decimal Cost { get; set; }
        public string Thumbnail { get; set; }
        [Display(Name = "Thumbnail")]
        public HttpPostedFileBase Photo { get; set; }
    }
}