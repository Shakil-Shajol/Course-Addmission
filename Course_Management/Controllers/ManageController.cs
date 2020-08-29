using Course_Management.Authentication;
using Course_Management.IdentityModels;
using Course_Management.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Course_Management.Controllers
{
    [CustomAuthorize(Roles ="Admin",Users ="Shakil")]
    public class ManageController : Controller
    {
        AuthenticationDb db = new AuthenticationDb();
        // GET: Manage
        public ActionResult Index()
        {

            return View(db.Users.Include("Roles").ToList());

        }

        public ActionResult CreateRole()
        {
            return View();
        }

        

        public ActionResult AssignRole()
        {
            using (AuthenticationDb db = new AuthenticationDb())
            {
                ViewBag.UserList = db.Users.ToList();
                ViewBag.RoleList = db.Roles.ToList();

            }
            return View();
        }
        [HttpPost]
        public ActionResult AssignRole(UserRolesVM uvm)
        {
            string msg = string.Empty;
            if (ModelState.IsValid)
            {
                using (AuthenticationDb db = new AuthenticationDb())
                {

                    var us = db.Users.Include("Roles").FirstOrDefault(x => x.UserId == uvm.UserId);
                    var ro = db.Roles.FirstOrDefault(x => x.RoleId == uvm.RoleId);


                    db.Users.Include("Roles").FirstOrDefault(x => x.UserId == uvm.UserId).Roles.Add(ro);

                    db.SaveChanges();
                    ViewBag.UserList = db.Users.ToList();
                    ViewBag.RoleList = db.Roles.ToList();
                }
            }
            return View();
        }



        [HttpPost]
        public ActionResult CreateRole(Role role)
        {
            if (ModelState.IsValid)
            {

                string statusMsg = string.Empty;
                using (AuthenticationDb db = new AuthenticationDb())
                {

                    var ro = db.Roles.FirstOrDefault(x => x.RoleName == role.RoleName);
                    if (ro == null)
                    {
                        var newRole = new Role
                        {
                            RoleName = role.RoleName

                        };

                        db.Roles.Add(newRole);
                        db.SaveChanges();
                        statusMsg = "Role Created Successfully!";
                    }
                    else
                    {
                        statusMsg = "Role is already exiests.";
                    }

                }
                ViewBag.msg = statusMsg;
            }
            return View();
        }
    }
}