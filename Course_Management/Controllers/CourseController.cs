using Course_Management.Authentication;
using Course_Management.DataAccessLayer;
using Course_Management.Models;
using Course_Management.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Course_Management.Controllers
{
    public class CourseController : Controller
    {
        TrainingDB db = new TrainingDB();
        // GET: Course
        public ActionResult Index(int? id,int page=1)
        {
            
            IEnumerable<Course> course = db.Courses.Include("Category").Include("Batches").ToList();
            int perPage = 4;
            if (id==null)
            {
                double count = db.Courses.Count();
                int totalPages = (int)Math.Ceiling(count / perPage);
                ViewBag.TotalPages = totalPages;
                ViewBag.CurrentPage = page;
                course = db.Courses.Include("Category").Include("Batches").OrderBy(x => x.CourseId).Skip(perPage * (page - 1)).Take(perPage).ToList();
            }
            else
            {
                double count = db.Courses.Where(x => x.CategoryId == id).Count();
                int totalPages = (int)Math.Ceiling(count / perPage);
                ViewBag.TotalPages = totalPages;
                ViewBag.CategoryID = id;
                ViewBag.CurrentPage = page;
                course = db.Courses.Include("Category").Include("Batches").Where(x=>x.CategoryId==id).OrderBy(x => x.CourseId).Skip(perPage * (page - 1)).Take(perPage).ToList();
            }
            return View(course);
        }

        public ActionResult Details(int id)
        {
            Course c = db.Courses.Include("Category").FirstOrDefault(x => x.CourseId == id);
            return View(c);
        }

        [CustomAuthorize(Roles = ("Admin,Co-Ordinator"))]

        public ActionResult Create()
        {
            ViewBag.categories = new SelectList(db.Categories, "CategoryId", "CategoryTitle");
            return View();
        }

        [HttpPost]
        public ActionResult Create(CourseViewModel cvm)
        {

            if (ModelState.IsValid)
            {
                if (cvm.Photo!=null)
                {
                    string filename = Guid.NewGuid().ToString() + Path.GetExtension(cvm.Photo.FileName);
                    string filepath = Path.Combine("~/Images", "CourseImages", filename);
                    cvm.Photo.SaveAs(Server.MapPath(filepath));
                    Course course = new Course
                    {
                        CourseTitle = cvm.CourseTitle,
                        Overview = cvm.Overview,
                        CategoryId = cvm.CategoryId,
                        Duration = cvm.Duration,
                        Quizes = cvm.Quizes,
                        Assesments = cvm.Assesments,
                        Cost = cvm.Cost,
                        Thumbnail = filepath
                    };
                    db.Courses.Add(course);
                    db.SaveChanges();
                    return RedirectToAction("Index");

                }
            }
            return View(cvm);
        }
        public ActionResult Delete(int id)
        {
            Course c = db.Courses.FirstOrDefault(x => x.CourseId == id);
            db.Courses.Remove(c);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}