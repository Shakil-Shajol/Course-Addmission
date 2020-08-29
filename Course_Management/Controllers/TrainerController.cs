using Course_Management.Authentication;
using Course_Management.DataAccessLayer;
using Course_Management.Models;
using Course_Management.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Course_Management.Controllers
{
    public class TrainerController : Controller
    {
        private TrainingDB db = new TrainingDB();
        // GET: Trainer
        public ActionResult Index()
        {
            return View(db.Trainers.Include("Skills").ToList());
        }

        [CustomAuthorize(Roles = ("Admin,Co-Ordinator"))]

        public ActionResult Create()
        {
            try
            {
                ViewBag.skills = new SelectList(db.Skills, "SkillId", "SkillTitle");
                return View();
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Error", new { errormsg = ex.Message });
            }
        }
        [HttpPost]
        public ActionResult Create(TrainerViewModel tvm)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (tvm.Photo != null)
                    {
                        string filename = Guid.NewGuid().ToString() + Path.GetExtension(tvm.Photo.FileName);
                        string filepath = Path.Combine("~/Images", "Trainer", filename);
                        tvm.Photo.SaveAs(Server.MapPath(filepath));
                        Trainer trainer = new Trainer
                        {
                            Name = tvm.Name,
                            FatherName = tvm.FatherName,
                            Email = tvm.Email,
                            Gender = tvm.Gender,
                            BirthDate = tvm.BirthDate,
                            NID = tvm.NID,
                            Phone = tvm.Phone,
                            Address = tvm.Address,
                            PhotoPath = filepath
                        };
                        db.Trainers.Add(trainer);
                        db.SaveChanges();
                        int id = trainer.TrainerId;
                        foreach (var skillid in tvm.SkillIds)
                        {
                            Skill skill = db.Skills.FirstOrDefault(x => x.SkillId == skillid);
                            db.Trainers.Include("Skills").FirstOrDefault(x => x.TrainerId == id).Skills.Add(skill);
                        }
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                }
                return View(tvm);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Error", new { errormsg = ex.Message });
            }
        }


        [CustomAuthorize(Roles = ("Admin,Co-Ordinator"))]


        public ActionResult Edit(int id)
        {
            try
            {
                
                Trainer trainer = db.Trainers.Include("Skills").FirstOrDefault(x => x.TrainerId == id);
                List<int> existingSkills = new List<int>();

                foreach (var item in trainer.Skills)
                {
                    existingSkills.Add(item.SkillId);
                }
                ViewBag.skills = new MultiSelectList(db.Skills, "SkillId", "SkillTitle",existingSkills.ToArray());
                TrainerViewModel tvm = new TrainerViewModel
                {
                    TrainerId = trainer.TrainerId,
                    Name = trainer.Name,
                    FatherName = trainer.FatherName,
                    Email = trainer.Email,
                    Gender = trainer.Gender,
                    BirthDate = trainer.BirthDate,
                    NID = trainer.NID,
                    Phone = trainer.Phone,
                    Address = trainer.Address,
                    PhotoPath = trainer.PhotoPath,
                    Skills = trainer.Skills
                };
                return View(tvm);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Error", new { errormsg = ex.Message });
            }
        }
        [HttpPost]
        public ActionResult Edit(TrainerViewModel tvm)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string filepath = tvm.PhotoPath;
                    if (tvm.Photo != null)
                    {
                        string filename = Guid.NewGuid().ToString() + Path.GetExtension(tvm.Photo.FileName);
                        filepath = Path.Combine("~/Images", "Trainer", filename);
                        tvm.Photo.SaveAs(Server.MapPath(filepath));
                        Trainer trainer = new Trainer
                        {
                            TrainerId = tvm.TrainerId,
                            Name = tvm.Name,
                            FatherName = tvm.FatherName,
                            Email = tvm.Email,
                            Gender = tvm.Gender,
                            BirthDate = tvm.BirthDate,
                            NID = tvm.NID,
                            Phone = tvm.Phone,
                            Address = tvm.Address,
                            PhotoPath = filepath
                        };
                        db.Entry(trainer).State = EntityState.Modified;
                        db.SaveChanges();
                        if (tvm.SkillIds == null)
                        {
                            return RedirectToAction("Index");
                        }
                        int id = trainer.TrainerId;
                        Trainer currentTrainer = db.Trainers.Include("Skills").FirstOrDefault(x => x.TrainerId == id);
                        List<Skill> existingSkills = new List<Skill>();

                        foreach (var item in currentTrainer.Skills)
                        {
                            existingSkills.Add(item);
                        }
                        foreach (var item in existingSkills)
                        {
                            currentTrainer.Skills.Remove(item);
                        }
                        foreach (var skillid in tvm.SkillIds)
                        {
                            Skill skill = db.Skills.FirstOrDefault(x => x.SkillId == skillid);
                            currentTrainer.Skills.Add(skill);
                        }
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        Trainer trainer = new Trainer
                        {
                            TrainerId = tvm.TrainerId,
                            Name = tvm.Name,
                            FatherName = tvm.FatherName,
                            Email = tvm.Email,
                            Gender = tvm.Gender,
                            BirthDate = tvm.BirthDate,
                            NID = tvm.NID,
                            Phone = tvm.Phone,
                            Address = tvm.Address,
                            PhotoPath = filepath
                        };
                        db.Entry(trainer).State = EntityState.Modified;
                        db.SaveChanges();
                        if (tvm.SkillIds == null)
                        {
                            return RedirectToAction("Index");
                        }
                        int id = trainer.TrainerId;
                        Trainer currentTrainer = db.Trainers.Include("Skills").FirstOrDefault(x => x.TrainerId == id);
                        List<Skill> existingSkills = new List<Skill>();

                        foreach (var item in currentTrainer.Skills)
                        {
                            existingSkills.Add(item);
                        }
                        foreach (var item in existingSkills)
                        {
                            currentTrainer.Skills.Remove(item);
                        }
                        foreach (var skillid in tvm.SkillIds)
                        {
                            Skill skill = db.Skills.FirstOrDefault(x => x.SkillId == skillid);
                            currentTrainer.Skills.Add(skill);
                        }
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                }
                return View(tvm);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Error", new { errormsg = ex.Message });
            }
        }
        [CustomAuthorize(Roles = ("Admin,Co-Ordinator"))]

        public ActionResult Delete(int? id)
        {
            try
            {
                if (id == null)
                {
                    throw new Exception(HttpStatusCode.BadRequest.ToString());
                }
                Trainer trainer = db.Trainers.Find(id);
                if (trainer == null)
                {
                    throw new Exception(HttpNotFound().StatusCode.ToString());
                }
                db.Trainers.Remove(trainer);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Error", new { errormsg = ex.Message });
            }
        }
    }
}