﻿using Solution.Data;
using Solution.Domain.Entities;
using Solution.Service;
using Solution.Web.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;


namespace Solution.Web.Controllers
{
    public class ClaimController : Controller
    {
       
        IClaimService ClaimsService;
        IUserService userService;
        public ClaimController()
        {
            ClaimsService = new ClaimService();
            userService = new UserService();
        }
        // GET: Claim
        public ActionResult Index()
        {

            PidevContext db = new PidevContext();
            return View(db.Claims.ToList());
        }

        //POST : Claim
        [HttpPost]
        public ActionResult Index( int id ,ClaimModel cm)
        {
            List<Claim> surveyslist = new List<Claim>();
            foreach (var survey in surveyslist)
            {
                if (surveyslist.Count() == 3)
                {
                    Claim c = ClaimsService.Get(s => s.Name == survey.Name && s.Description == survey.Description && s.ClaimType == survey.ClaimType && s.status == survey.status && s.Parent.prenom == survey.Parent.prenom && s.ComplaintId == survey.ComplaintId);
                    c.Parent.Ban = 1;
                    ClaimsService.Update(c);
                    ClaimsService.Commit();  
                }               
            }
            return RedirectToAction("Index");
        }
        // GET: Claim/Details/5
        public ActionResult Details(int id)
        {
           /* var userId = (int)Session["idu"];
            String Phone2 = userService.GetById(userId).login;
            String mail = userService.GetById(userId).email;
            ViewBag.home = mail;
            ViewBag.phone = Phone2;*/
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Claim c;
            c = ClaimsService.GetById((int)id);
            if (c == null)
            {
                return HttpNotFound();
            }
                ClaimModel cm = new ClaimModel()
            {
                ComplaintId = c.ComplaintId,
                
                Name = c.Name,
                Description = c.Description,
                ClaimDate = c.ClaimDate,
                ParentId = c.ParentId,
                ClaimType = c.ClaimType,
                status = c.status
            };
            return View(cm);
        }

        // GET: Claim/Create
        public ActionResult Create()
        {

            var userId = (int)Session["idu"];
            String Phone2 = userService.GetById(userId).login;
            String mail = userService.GetById(userId).email;
            ViewBag.home = mail;
            ViewBag.phone = Phone2;
            return View();
        }

        // POST: Claim/Create
        [HttpPost]
        public ActionResult Create(ClaimModel claimM,string name)
        {

            DateTime today = DateTime.Now;
            Claim claims = new Claim()
            {
                Name = claimM.Name,
                Description = claimM.Description,
                ClaimDate = today,
                ParentId = (int)Session["idu"],
                ClaimType = claimM.ClaimType,
                status = "In_progress"
            };
            
            ClaimsService.Add(claims);
            ClaimsService.Commit();
            bool result2 = false;
            result2 = ClaimsService.SendEmail("raslensafsaf@gmail.com", "You've got a new claim to treat", "<p><h1>Mister Admin you've got a new claim to treat: </h1><br /><b> *** Claim Type : </b></p>" + claims.ClaimType + "<p><b> *** Claim status Status : </b></p>" + claims.status + "<p><b> *** Claim description : </b></p>" + claims.Description + "<p><b> *** Claim Name : </b></p>" + claims.Name + "<p><b> *** Claim Date : </b></p>" + claims.ClaimDate + "<p><h4><a href='https://localhost:44326/Login/Login/Claim/index'>Please Check your claims Here</a></h4></p>");
            return RedirectToAction("IndexFront");
        }

        // GET: Claim/Edit/5
        public ActionResult Edit(int id)
        {
            
            var userId = (int)Session["idu"];
            String Phone2 = userService.GetById(userId).login;
            String mail = userService.GetById(userId).email;
            ViewBag.home = mail;
            ViewBag.phone = Phone2;
            if (id == 0)
            { return new HttpStatusCodeResult(HttpStatusCode.BadRequest); }
              else
            {
            Claim e = ClaimsService.GetById(id);
            ClaimModel em = new ClaimModel();
            
            em.Name = e.Name;
            em.Description = e.Description;
            em.ClaimDate = e.ClaimDate;
            em.ClaimType = e.ClaimType;
            em.status = e.status;
            return View(em);
        }
        }
        

        // POST: Claim/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, ClaimModel cm)
        {
            DateTime today = DateTime.Now;
            try
            {
                Claim c = ClaimsService.GetById(id);
                c.Name = cm.Name;
                c.Description = cm.Description;
                c.ClaimDate = today;
                c.ClaimType = cm.ClaimType;
                c.status = cm.status;
                ClaimsService.Update(c);
                ClaimsService.Commit();

                return RedirectToAction("IndexFront");
            }
            catch
            {
                return View(cm);
            }
        }
        
         
        // GET: Claim/Delete/5
        public ActionResult Delete(int id)
        {

           /* var userId = (int)Session["idu"];
            String Phone2 = userService.GetById(userId).login;
            String mail = userService.GetById(userId).email;
            ViewBag.home = mail;
            ViewBag.phone = Phone2;*/
            return View();
        }

        // POST: Claim/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            Claim C = ClaimsService.GetById((int)id);
            if (C.status == "Resolved")
            {
                ClaimsService.Delete(C);
                ClaimsService.Commit();
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.DEL = "You should update the reclamation First";
            }
            return View();
        }
        // GET: ClaimFront
        public ActionResult IndexFront(string searchString , string sortOrder)
        {
            var userId = (int)Session["idu"];
            String login = userService.GetById(userId).login;
            String mail = userService.GetById(userId).email;
            ViewBag.home = mail;
            ViewBag.phone = login;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";
            PidevContext db = new PidevContext();
            var claims = from s in db.Claims
                           select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                claims = claims.Where(s => s.Name.ToUpper().Contains(searchString.ToUpper()));
            }
            switch (sortOrder)
            {
                case "Name_desc":
                    claims = claims.OrderByDescending(s => s.Name);
                    break;
                case "Date":
                    claims = claims.OrderBy(s => s.ClaimDate);
                    break;
                case "Date_desc":
                    claims = claims.OrderByDescending(s => s.ClaimDate);
                    break;
                default:
                    claims = claims.OrderBy(s => s.Name);
                    break;
            }          
            return View(claims.ToList());            
        }
        // GET: Claim/DeleteFront/5
        public ActionResult DeleteFront(int id)
        {

            var userId = (int)Session["idu"];
            String login = userService.GetById(userId).login;
            String mail = userService.GetById(userId).email;
            ViewBag.home = mail;
            ViewBag.phone = login;
            return View();
        }
        // POST: Claim/DeleteFront/5
        [HttpPost]
        public ActionResult DeleteFront(int id, FormCollection collection)
        {
                Claim C = ClaimsService.GetById((int)id);
                ClaimsService.Delete(C);
                ClaimsService.Commit();
                return RedirectToAction("IndexFront");
           
        }
        // GET: Claim/DetailsFront/5
        public ActionResult DetailsFront(int id)
        {
             var userId = (int)Session["idu"];
             String login = userService.GetById(userId).login;
             String mail = userService.GetById(userId).email;
             ViewBag.home = mail;
             ViewBag.phone = login;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Claim c;
            c = ClaimsService.GetById((int)id);
            if (c == null)
            {
                return HttpNotFound();
            }
            ClaimModel cm = new ClaimModel()
            {
                ComplaintId = c.ComplaintId,
                
                Name = c.Name,
                Description = c.Description,
                ClaimDate = c.ClaimDate,
                ParentId = (int)Session["idu"],
                ClaimType = c.ClaimType,
                status = c.status
            };
            return View(cm);
        }
        // GET: Claim/EditBack/5
        public ActionResult EditBack(int id)
        {
                return View();
            
        }
        // POST: Claim/EditBack/5
        [HttpPost]
        public ActionResult EditBack(int id, Claim c)
        {          
                c = ClaimsService.GetById(id);
                c.status = "Resolved";
                ClaimsService.Update(c);
                ClaimsService.Commit();
                return RedirectToAction("Index");
   
        }
    }
}
