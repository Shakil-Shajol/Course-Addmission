using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Course_Management.Controllers
{
    public class ErrorController : Controller
    {
        // GET: Error
        public ActionResult Index(string errormsg="")
        {
            ViewBag.msg = errormsg;
            return View();
        }

        public ActionResult NotFound()
        {
            return View();
        }
    }
}