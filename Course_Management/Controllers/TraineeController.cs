using Course_Management.Authentication;
using Course_Management.DataAccessLayer;
using Course_Management.IdentityModels;
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
    [Authorize]
    public class TraineeController : Controller
    {
        private TrainingDB db = new TrainingDB();
        private AuthenticationDb authDb = new AuthenticationDb();
       [CustomAuthorize(Roles = ("Admin,Co-Ordinator"))]
        // GET: Trainee
        public ActionResult Index()
        {
            return View();
        }

        // GET: Trainee/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }
        [Authorize]
        // GET: Trainee/Create
        public ActionResult Create(string returnurl="")
        {
            string username = User.Identity.Name;
                var user = (from us in authDb.Users
                            where string.Compare(us.Username, username, StringComparison.OrdinalIgnoreCase) == 0
                            select us).FirstOrDefault();
                if (user == null)
                {
                    return null;
                }
                var selectedUser = new CustomMembershipUser(user);
            if (selectedUser==null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                TraineeViewModel trainee = new TraineeViewModel
                {
                    Name = selectedUser.FirstName + " " + selectedUser.LastName,
                    Email = selectedUser.Email,
                    UserName = selectedUser.UserName
                };
                ViewBag.ReturnUrl = returnurl;
                return View(trainee);
            }
        }

        // POST: Trainee/Create
        [HttpPost]
        public ActionResult Create(TraineeViewModel tvm,string returnurl="")
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (tvm.Photo != null)
                    {
                        string filename = Guid.NewGuid().ToString() + Path.GetExtension(tvm.Photo.FileName);
                        string filepath = Path.Combine("Images", "Trainee", filename);
                        string file = Path.Combine(Server.MapPath("~/"+ filepath));
                        tvm.Photo.SaveAs(file);
                        Trainee trainer = new Trainee
                        {
                            Name = tvm.Name,
                            FatherName = tvm.FatherName,
                            Email = tvm.Email,
                            Gender = tvm.Gender,
                            BirthDate = tvm.BirthDate,
                            NID = tvm.NID,
                            Phone = tvm.Phone,
                            Address = tvm.Address,
                            UserName=tvm.UserName,
                            PhotoPath = filepath
                        };
                        db.Trainees.Add(trainer);
                        db.SaveChanges();
                        if (Url.IsLocalUrl(returnurl))
                        {
                            return Redirect(returnurl);
                        }
                        else
                        {
                            return RedirectToAction("Index", "Course");
                        }
                        
                    }
                }
                throw new Exception();
            }
            catch
            {
                return View();
            }
        }

        // GET: Trainee/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Trainee/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Trainee/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Trainee/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
