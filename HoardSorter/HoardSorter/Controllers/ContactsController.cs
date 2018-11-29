using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using HoardSorter.Models;

namespace HoardSorter.Controllers
{
    public class ContactsController : Controller
    {
        private HoardSorterEntities db = new HoardSorterEntities();

        // GET: Contacts
        public ActionResult Index()
        {
            String UserID = db.AspNetUsers.Where(x => x.Email == System.Web.HttpContext.Current.User.Identity.Name).FirstOrDefault().Id;
            var contacts = db.Contacts.Where(y => (y.myID.ToString()).Equals(UserID)).OrderBy(y => y.AspNetUsers1.Email);
            //var contacts = db.Contacts.Include(c => c.AspNetUsers).Include(c => c.AspNetUsers1);
            return View(contacts.ToList());
        }

        // GET: Contacts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contacts contacts = db.Contacts.Find(id);
            if (contacts == null)
            {
                return HttpNotFound();
            }
            return View(contacts);
        }

        // GET: Contacts/Create
        public ActionResult Create()
        {
            ViewBag.myID = new SelectList(db.AspNetUsers, "Id", "Email");
            ViewBag.yourID = new SelectList(db.AspNetUsers.OrderBy(s => s.Email), "Id", "Email");
            return View();
        }

        // POST: Contacts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ContactID,myID,yourID")] Contacts contacts)
        {
            String UserID = db.AspNetUsers.Where(x => x.Email == System.Web.HttpContext.Current.User.Identity.Name).FirstOrDefault().Id;
            contacts.myID = UserID;

            if (ModelState.IsValid)
            {
                db.Contacts.Add(contacts);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.myID = new SelectList(db.AspNetUsers, "Id", "Email", contacts.myID);
            ViewBag.yourID = new SelectList(db.AspNetUsers, "Id", "Email", contacts.yourID);
            return View(contacts);
        }

        // GET: Contacts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contacts contacts = db.Contacts.Find(id);
            if (contacts == null)
            {
                return HttpNotFound();
            }
            ViewBag.myID = new SelectList(db.AspNetUsers, "Id", "Email", contacts.myID);
            ViewBag.yourID = new SelectList(db.AspNetUsers, "Id", "Email", contacts.yourID);
            return View(contacts);
        }

        // POST: Contacts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ContactID,myID,yourID")] Contacts contacts)
        {
            if (ModelState.IsValid)
            {
                db.Entry(contacts).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.myID = new SelectList(db.AspNetUsers, "Id", "Email", contacts.myID);
            ViewBag.yourID = new SelectList(db.AspNetUsers, "Id", "Email", contacts.yourID);
            return View(contacts);
        }

        // GET: Contacts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contacts contacts = db.Contacts.Find(id);
            if (contacts == null)
            {
                return HttpNotFound();
            }
            return View(contacts);
        }

        // POST: Contacts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Contacts contacts = db.Contacts.Find(id);
            db.Contacts.Remove(contacts);
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
