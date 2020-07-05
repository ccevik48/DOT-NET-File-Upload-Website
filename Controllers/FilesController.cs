using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FileUpload.Models;

namespace FileUpload.Controllers
{
    public class FilesController : Controller
    {
        private fileUploadEntities3 db = new fileUploadEntities3();

        // GET: Files
        public ActionResult Index()
        {
            var f = db.Files.ToList();
            f.Reverse();
            return View(f);
        }

        // GET: Files/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            File file = db.Files.Find(id);
            if (file == null)
            {
                return HttpNotFound();
            }
            return View(file);
        }

        public ActionResult FileDetails(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var file = db.Files.Where(x => x.FileId == id).FirstOrDefault();
            if (file == null)
            {
                return HttpNotFound();
            }
            return View(file);
        }


        /*
        public FileResult Download(int? id)
        {
            var fileToRetrieve = db.Files.Find(id);
            return File(fileToRetrieve.FileContent, "application/octet-stream", fileToRetrieve.FileName + fileToRetrieve.FileExt);
        }*/

        public FileResult Download(string id)
        {
            var fileToRetrieve = db.Files.Where(x => x.FileId == id).FirstOrDefault();
            return File(fileToRetrieve.FileContent, "application/octet-stream", fileToRetrieve.FileName + fileToRetrieve.FileExt);
        }



        public ActionResult MyFiles()
        {
            var tempSess = (int)Session["Id"];
            
            return View(db.Files.Where(x => x.UserId == tempSess));
        }

        public ActionResult Search(string fname, string fext, string fdesc)
        {
            
            return View("Index",db.Files.Where(x => x.FileName.Contains(fname) && x.FileExt.Contains(fext) && x.FileDesc.Contains(fdesc)));
        }

        // GET: Files/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Files/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,UserId,FileName,FileContent,FileId,FileExt,FileDesc,UploadDate")] File file, HttpPostedFileBase uFile)
        {
            if (ModelState.IsValid)
            {
                if(uFile != null)
                {
                    file.UserId = (int) Session["Id"];
                    file.FileContent = new byte[uFile.ContentLength];
                    uFile.InputStream.Read(file.FileContent, 0, uFile.ContentLength);
                    file.FileExt = MimeTypes.MimeTypeMap.GetExtension(uFile.ContentType);
                    string hexString = "";
                    Random rnd = new Random();
                    char[] hexChar = {'1', '2', '3', '4', '5', '6', '7', '8', '9', '0', 'A', 'B', 'C', 'D', 'E', 'F', };
                    for(var i = 0; i< 20; i++)
                    {
                        
                        int j = rnd.Next(0, 16);
                        hexString += hexChar[j];
                    }
                    //var random = new Random();
                    //var hex = String.Format("{0:X20}", random.Next(0x1000000));
                    file.FileId = hexString;
                }
                db.Files.Add(file);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(file);
        }

        // GET: Files/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            File file = db.Files.Find(id);
            if (file == null)
            {
                return HttpNotFound();
            }
            return View(file);
        }

        // POST: Files/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,UserId,FileName,FileContent,FileId,FileExt,FileDesc,UploadDate")] File file)
        {
            if (ModelState.IsValid)
            {
                db.Entry(file).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(file);
        }

        // GET: Files/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            File file = db.Files.Find(id);
            if (file == null)
            {
                return HttpNotFound();
            }
            return View(file);
        }

        // POST: Files/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            File file = db.Files.Find(id);
            db.Files.Remove(file);
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
