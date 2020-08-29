using Course_Management.Authentication;
using Course_Management.DataAccessLayer;
using Course_Management.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Course_Management.Controllers
{
    
    public class CategoriesController : Controller
    {
        private TrainingDB db = new TrainingDB();
        // GET: Categories
        [CustomAuthorize(Roles =("Admin,Co-Ordinator"))]
        public ActionResult Index()
        {
            return View(db.Categories.ToList());
        }
        [CustomAuthorize(Roles = ("Admin,Co-Ordinator"))]

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CategoryId,CategoryTitle")] Category category)
        {
            if (ModelState.IsValid)
            {
                db.Categories.Add(category);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(category);
        }

        [CustomAuthorize(Roles = ("Admin,Co-Ordinator"))]

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CategoryId,CategoryTitle")] Category category)
        {
            if (ModelState.IsValid)
            {
                db.Entry(category).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(category);
        }

        [CustomAuthorize(Roles = ("Admin,Co-Ordinator"))]

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            db.Categories.Remove(category);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
       
        public PartialViewResult CategoryNav()
        {
            ViewBag.CourseCount = db.Courses.Count();
            return PartialView("_CatNav", db.Categories.ToList());
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}