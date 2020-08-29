using Course_Management.Authentication;
using Course_Management.DataAccessLayer;
using Course_Management.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Course_Management.Controllers
{
    [CustomAuthorize(Roles ="User")]
    public class EnrollController : Controller
    {
        TrainingDB db = new TrainingDB();

        // GET: Enroll
        public ActionResult Index()
        {
            return View();
        }
        [Authorize]
        public ActionResult Create(int bId)
        {
            try
            {
                var trainee = db.Trainees.FirstOrDefault(x => x.UserName == User.Identity.Name);
                if (trainee == null)
                {
                    return RedirectToAction("Create", "Trainee", new { returnurl = "~/Enroll/Create?bId=" + bId });
                }
                var batch = db.Batches.FirstOrDefault(x => x.BatchId == bId);
                var chk = db.Enrollments.FirstOrDefault(x => x.BatchId == bId && x.TraineeId == trainee.TraineeId);
                if (chk != null)
                {
                    return RedirectToAction("AlreadyAssigned");
                }
                db.Enrollments.Add(new Enrollment { BatchId = bId, TraineeId = trainee.TraineeId });
                db.SaveChanges();
                return RedirectToAction("EnBatch", "Batch");
            }
            catch (Exception ex)
            {

                return RedirectToAction("Index", "Error", new { errormsg = ex.Message });
            }
        }

        public ActionResult AlreadyAssigned()
        {
            return View();
        }
    }
}