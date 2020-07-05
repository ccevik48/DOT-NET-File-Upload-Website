using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using FileUpload.Models;

namespace FileUpload.Controllers
{
    public class UsersController : Controller
    {
        private fileUploadEntities2 db = new fileUploadEntities2();

        // GET: Users
        public ActionResult Index()
        {
            return View(db.Users.ToList());
        }


        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(FileUpload.Models.User user)
        {
            var userDetails = db.Users.Where(x => x.Username == user.Username && x.Password == user.Password).FirstOrDefault();
            if (userDetails == null)
            {
                //user.loginError = "Wrong User Name or Password";
                return View();
            }
            else
            {
                Session["Id"] = userDetails.Id;
                Session["UserName"] = userDetails.Username;
                return RedirectToAction("Profile", "Users");
            }
        }
        public ActionResult LogOut()
        {
            int userID = (int)Session["Id"];
            Session.Abandon();
            return RedirectToAction("Index", "Home");
        }
        public ActionResult Profile()
        {
            return View();
        }




        // GET: Users/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: Users/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Username,Email,Password")] User user)
        {
            if (ModelState.IsValid)
            {
                
                var uNameTaken = db.Users.Any(x => x.Username == user.Username || x.Email == user.Email);
                if (uNameTaken)
                {
                    ViewBag.err = "Username/email already taken!";
                    return View();
                }
                
                db.Users.Add(user);
                db.SaveChanges();
                return RedirectToAction("Login");
            }

            return View();
        }

        // GET: Users/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Username,Email,Password")] User user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(user);
        }

        // GET: Users/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            User user = db.Users.Find(id);
            db.Users.Remove(user);
            db.SaveChanges();
            return RedirectToAction("Index");
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
