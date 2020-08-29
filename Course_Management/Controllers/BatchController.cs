using Course_Management.Authentication;
using Course_Management.DataAccessLayer;
using Course_Management.Models;
using Course_Management.ViewModels;
using CrystalDecisions.CrystalReports.Engine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Course_Management.Controllers
{
    public class BatchController : Controller
    {
        TrainingDB db = new TrainingDB();
        // GET: Batch
        public ActionResult Index()
        {
            var batches = db.Batches.Include("Enrollments").ToList();
            return View(batches);
        }
        
        [CustomAuthorize(Roles ="Admin,Co-Ordinator")]
        public ActionResult Details(int id)
        {
            var batch = db.Batches.Include("Enrollments").FirstOrDefault(x => x.BatchId == id);
            return View(batch);
        }
        [CustomAuthorize(Roles ="Admin,Co-Ordinator")]
        public ActionResult Create()
        {
            ViewBag.trainers =new SelectList(db.Trainers, "TrainerId", "Name");
            ViewBag.courses = new SelectList(db.Courses, "CourseId", "CourseTitle");

            return View();
        }


        [HttpPost]
        public ActionResult Create(Batch batch)
        {
            if (ModelState.IsValid)
            {
                db.Batches.Add(batch);
                db.SaveChanges();
                return RedirectToAction("Details", new { id = batch.BatchId });
            }
            ViewBag.trainers = new SelectList(db.Trainers, "TrainerId", "Name");
            ViewBag.courses = new SelectList(db.Courses, "CourseId", "CourseTitle");
            return View(batch);
        }

        public ActionResult EnBatch()
        {
            string username = User.Identity.Name;
            var trainee = db.Trainees.FirstOrDefault(x => x.UserName == username);
            if (trainee == null)
            {
                return RedirectToAction("Create", "Trainee",new { returnurl = "~/Batch/EnBatch" });
            }
            var enrollments = db.Enrollments.Where(x => x.TraineeId == trainee.TraineeId).ToList();
            List<Batch> batches = new List<Batch>();
            foreach (var item in enrollments)
            {
                Batch batch = db.Batches.Include("Enrollments").FirstOrDefault(x => x.BatchId == item.BatchId);
                batches.Add(batch);
            }
            return View(batches);
        }


        public ActionResult ExportReport(int bId)
        {
            Batch batch = db.Batches.Include("Trainer").FirstOrDefault(x => x.BatchId == bId);
            var trainees = db.Enrollments.Include("Trainee").Where(x => x.BatchId == bId).ToList();
           List<BatchReportViewModel> reportData = new List<BatchReportViewModel>();
            foreach (var item in trainees)
            {
                var trainee = db.Trainees.FirstOrDefault(x => x.TraineeId == item.TraineeId);
                BatchReportViewModel brvm = new BatchReportViewModel
                {
                    TraineeId = trainee.TraineeId,
                    Name = trainee.Name,
                    FatherName = trainee.FatherName,
                    Email = trainee.Email,
                    Phone = trainee.Phone,
                    Address = trainee.Address,
                    BatchName = batch.BatchName,
                    StartDate = batch.StartDate.ToString("MMM dd, yyyy"),
                    Trainer = batch.Trainer.Name

                };

                reportData.Add(brvm);
            }
            ReportDocument rd = new ReportDocument();
            rd.Load(Path.Combine(Server.MapPath("~/Reports"), "CrystalReport.rpt"));
            rd.SetDataSource(reportData);
            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();
            try
            {
                string outputfilename = "Batch_Details_" + batch.BatchName + "_" + DateTime.Now.ToShortDateString() + ".pdf";
                Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stream.Seek(0, SeekOrigin.Begin);
                return File(stream, "application/pdf", outputfilename);
            }
            catch (Exception ex)
            {

                return RedirectToAction("Index", "Error", routeValues: new { errormsg = ex.Message });
            }
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