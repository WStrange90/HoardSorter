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
    public class TradesController : Controller
    {
        private HoardSorterContext db = new HoardSorterContext();

        // GET: Trades
        public ActionResult Index()
        {
            return View(db.Trades.ToList());
        }

        // GET: Trades/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Trades trades = db.Trades.Find(id);
            if (trades == null)
            {
                return HttpNotFound();
            }
            return View(trades);
        }

        // GET: Trades/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Trades/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Name")] Trades trades)
        {
            if (ModelState.IsValid)
            {
                db.Trades.Add(trades);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(trades);
        }

        // GET: Trades/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Trades trades = db.Trades.Find(id);
            if (trades == null)
            {
                return HttpNotFound();
            }
            return View(trades);
        }

        // POST: Trades/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Name")] Trades trades)
        {
            if (ModelState.IsValid)
            {
                db.Entry(trades).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(trades);
        }

        // GET: Trades/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Trades trades = db.Trades.Find(id);
            if (trades == null)
            {
                return HttpNotFound();
            }
            return View(trades);
        }

        // POST: Trades/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Trades trades = db.Trades.Find(id);
            db.Trades.Remove(trades);
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
