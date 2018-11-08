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
    public class CardCollectionsController : Controller
    {
        private HoardSorterEntities db = new HoardSorterEntities();

        // GET: CardCollections
        public ActionResult Index()
        {
            var cardCollection = db.CardCollection.Include(c => c.CardDetails).Include(c => c.Collections);
            return View(cardCollection.ToList());
        }

        public ActionResult MyTrades()
        {
            var cardCollection = db.CardCollection.Include(c => c.CardDetails).Include(c => c.Collections);
            return View(cardCollection.ToList());
        }

        // GET: CardCollections/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CardCollection cardCollection = db.CardCollection.Find(id);
            if (cardCollection == null)
            {
                return HttpNotFound();
            }
            return View(cardCollection);
        }

        // GET: CardCollections/Create
        public ActionResult Create()
        {
            ViewBag.CardID = new SelectList(db.CardDetails, "CardID", "CardName");
            ViewBag.collectorID = new SelectList(db.Collections, "collectorID", "UserID");
            return View();
        }

        // POST: CardCollections/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CardCollectionID,CardID,ToTrade,Wanted,OwnedQty,TradeQty,WantQty,collectorID")] CardCollection cardCollection)
        {
            if (ModelState.IsValid)
            {
                db.CardCollection.Add(cardCollection);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CardID = new SelectList(db.CardDetails, "CardID", "CardName", cardCollection.CardID);
            ViewBag.collectorID = new SelectList(db.Collections, "collectorID", "UserID", cardCollection.collectorID);
            return View(cardCollection);
        }

        // GET: CardCollections/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CardCollection cardCollection = db.CardCollection.Find(id);
            if (cardCollection == null)
            {
                return HttpNotFound();
            }
            ViewBag.CardID = new SelectList(db.CardDetails, "CardID", "CardName", cardCollection.CardID);
            ViewBag.collectorID = new SelectList(db.Collections, "collectorID", "UserID", cardCollection.collectorID);
            return View(cardCollection);
        }

        // POST: CardCollections/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CardCollectionID,CardID,ToTrade,Wanted,OwnedQty,TradeQty,WantQty,collectorID")] CardCollection cardCollection)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cardCollection).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CardID = new SelectList(db.CardDetails, "CardID", "CardName", cardCollection.CardID);
            ViewBag.collectorID = new SelectList(db.Collections, "collectorID", "UserID", cardCollection.collectorID);
            return View(cardCollection);
        }

        // GET: CardCollections/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CardCollection cardCollection = db.CardCollection.Find(id);
            if (cardCollection == null)
            {
                return HttpNotFound();
            }
            return View(cardCollection);
        }

        // POST: CardCollections/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CardCollection cardCollection = db.CardCollection.Find(id);
            db.CardCollection.Remove(cardCollection);
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
