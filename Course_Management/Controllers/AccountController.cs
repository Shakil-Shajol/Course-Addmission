using Course_Management.Authentication;
using Course_Management.IdentityModels;
using Course_Management.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Course_Management.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Login(string ReturnUrl = "")
        {
            if (User.Identity.IsAuthenticated)
            {
                return LogOut();
            }
            ViewBag.ReturnUrl = ReturnUrl;
            return View();
        }


        [HttpPost]
        public ActionResult Login(LoginVM loginView, string ReturnUrl = "")
        {
            if (ModelState.IsValid)
            {
                if (Membership.ValidateUser(loginView.UserName, loginView.Password))
                {
                    var user = (CustomMembershipUser)Membership.GetUser(loginView.UserName, false);
                    if (user != null)
                    {
                        CustomSerializeModel userModel = new CustomSerializeModel
                        {
                            UserId = user.UserId,
                            FirstName = user.FirstName,
                            LastName = user.LastName,
                            RoleName = user.Roles.Select(r => r.RoleName).ToList()
                        };

                        string userData = JsonConvert.SerializeObject(userModel);
                        FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(1, loginView.UserName, DateTime.Now, DateTime.Now.AddMinutes(15), false, userData);
                        string enTicket = FormsAuthentication.Encrypt(authTicket);
                        HttpCookie cookie = new HttpCookie("trCookies", enTicket);
                        Response.Cookies.Add(cookie);

                    }
                    if (Url.IsLocalUrl(ReturnUrl))
                    {
                        return Redirect(ReturnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
            }
            ModelState.AddModelError("", "Something Wrong : Username or Password invalid!");
            return View(loginView);
        }

        public ActionResult Registration()
        {
            return View();
        }


        [HttpPost]
        public ActionResult Registration(RegistrationVM rvm)
        {
            bool regStatus = false;
            string messageRegistration = string.Empty;
            if (ModelState.IsValid)
            {
                string email = Membership.GetUserNameByEmail(rvm.Email);
                var userName = Membership.GetUser(rvm.Username, false);
                Guid vcode = Guid.NewGuid();
                Random random = new Random();
                int activationOtp = random.Next(100000, 999999);
                if (userName != null)
                {
                    ModelState.AddModelError("", "Sorry! Username already Exists! Try different one.");
                    return View(rvm);
                }
                if (!string.IsNullOrEmpty(email))
                {
                    ModelState.AddModelError("", "Sorry! Email already Exists");
                    return View(rvm);
                }

                using (AuthenticationDb db = new AuthenticationDb())
                {
                    var user = new User
                    {
                        Username = rvm.Username,
                        FirstName = rvm.FirstName,
                        LastName = rvm.LastName,
                        Email = rvm.Email,
                        Password = rvm.Password,
                        ActivationCode = vcode,
                        ActivationOtp = activationOtp.ToString()

                    };

                    db.Users.Add(user);
                    db.SaveChanges();
                    var ro = db.Roles.FirstOrDefault(x => x.RoleName == "User");
                    db.Users.Include("Roles").FirstOrDefault(x => x.UserId == user.UserId).Roles.Add(ro);
                    db.SaveChanges();

                }
                //VerificationEmail(rvm.Email, vcode.ToString(), activationOtp.ToString());
                /*if you uncomment avobe line then please add a valid gmail address & password to line 148 & 151 respectively*/
                messageRegistration = "Your account has been created successfully.";
                regStatus = true;
            }
            else
            {
                messageRegistration = "Something Wrong!";
            }
            ViewBag.Message = messageRegistration;
            ViewBag.Status = regStatus;
            ViewBag.username = rvm.Username;
            return View(rvm);
        }


        [NonAction]
        private void VerificationEmail(string email, string activationCode, string activationOtp)
        {
            var url = string.Format("/Account/ActivationAccount/{0}", activationCode);
            var link = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, url);

            var fromEmail = new MailAddress("A gmail address", "VerifyAccount"); // need to replace "A gmail address" to a valid google mail address.
            var toEmail = new MailAddress(email);

            var fromEmailPassword = "password"; //Need to replace the "password" to actual password of the mail account
            string subject = "[Account Activation]";

            string body = "<br/> Please click on the following link in order to activate your account" + "<br/><a href='" + link + "'> Click to active your account</a><br /> Activation Code : <b>" + activationOtp + "</b>";
            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromEmail.Address, fromEmailPassword)

            };

            using (var message = new MailMessage(fromEmail, toEmail)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true

            })
                smtp.Send(message);
        }




        [HttpGet]
        public ActionResult ActivationAccount(string id)
        {
            bool statusAccount = false;
            using (AuthenticationDb dbContext = new AuthenticationDb())
            {
                var userAccount = dbContext.Users.Where(u => u.ActivationCode.ToString().Equals(id)).FirstOrDefault();

                if (userAccount != null)
                {
                    userAccount.IsActive = true;
                    dbContext.SaveChanges();
                    statusAccount = true;
                }
                else
                {
                    ViewBag.Message = "Something Wrong !!";
                    return RedirectToAction("Registration", "Account");
                }

            }
            ViewBag.Status = statusAccount;
            return View();
        }



        [HttpPost]
        public ActionResult ActiveUserAccount(string otpCode, string username)
        {
            bool statusAccount = false;
            using (AuthenticationDb dbContext = new AuthenticationDb())
            {
                var userAccount = dbContext.Users.Where(u => u.Username.ToString().Equals(username)).FirstOrDefault();

                if (userAccount != null)
                {
                    if (userAccount.ActivationOtp == otpCode)
                    {
                        userAccount.IsActive = true;
                        dbContext.SaveChanges();
                        statusAccount = true;
                    }
                    else
                    {
                        ViewBag.Message = "Something Wrong !!";
                        return RedirectToAction("Registration", "Account");
                    }
                }
                else
                {
                    ViewBag.Message = "Something Wrong !!";
                    return RedirectToAction("Registration", "Account");
                }

            }
            ViewBag.Status = statusAccount;
            return View();
        }



        public ActionResult LogOut()
        {
            HttpCookie cookie = new HttpCookie("trCookies", "");
            cookie.Expires = DateTime.Now.AddYears(-1);
            Response.Cookies.Add(cookie);
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Account", null);

        }

        [CustomAuthorize(Roles = "Admin,Co-Ordinator")]
        public ActionResult ActiveAccount()
        {
            using (AuthenticationDb db = new AuthenticationDb())
            {
                var users = db.Users.Where(x => x.IsActive == false).ToList();
                return View(users);
            }
        }

        [HttpPost]
        public ActionResult ActiveAccount(string Username)
        {
            using (AuthenticationDb db = new AuthenticationDb())
            {
                var user = db.Users.FirstOrDefault(x => x.Username == Username);
                if (user != null)
                {
                    user.IsActive = true;
                    db.Entry(user).State = EntityState.Modified;
                    db.SaveChanges();
                    var usersn = db.Users.Where(x => x.IsActive == false).ToList();
                    return View(usersn);
                }
                var users = db.Users.Where(x => x.IsActive == false).ToList();
                return View(users);
            }
        }


        [CustomAuthorize(Roles = "Admin")]
        public ActionResult DeactiveAccount()
        {
            using (AuthenticationDb db = new AuthenticationDb())
            {
                var users = db.Roles.Where(x => x.RoleName == "Admin").SelectMany(x => x.Users).ToList();
                var acUser = db.Users.Where(x => x.IsActive == true).ToList();
                foreach (var item in users)
                {
                    acUser.Remove(item);
                }
                return View(acUser);
            }
        }


        [HttpPost]
        public ActionResult DeactiveAccount(string username)
        {
            using (AuthenticationDb db = new AuthenticationDb())
            {
                var user = db.Users.FirstOrDefault(x => x.Username == username);
                if (user != null)
                {
                    user.IsActive = false;
                    db.Entry(user).State = EntityState.Modified;
                    db.SaveChanges();
                    

                }
                var users = db.Roles.Where(x => x.RoleName == "Admin").SelectMany(x => x.Users).ToList();
                var acUser = db.Users.Where(x => x.IsActive == true).ToList();
                foreach (var item in users)
                {
                    acUser.Remove(item);
                }
                return View(acUser);
            }
        }
    }
}
