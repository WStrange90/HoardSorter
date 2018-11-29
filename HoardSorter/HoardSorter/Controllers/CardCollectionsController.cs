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
        public int test;
        // GET: CardCollections
        public ActionResult Index(string sortOrder)
        {
            String id = (db.AspNetUsers.Where(x => x.Email == System.Web.HttpContext.Current.User.Identity.Name).FirstOrDefault().Id);
            var collectorID = db.Collections.Where(y => y.UserID == id).FirstOrDefault().collectorID;

            var cardCollection = db.CardCollection.Where(c => c.collectorID == collectorID);
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.CostSortParm = sortOrder == "cost_asc" ? "cost_desc" : "cost_asc";
            ViewBag.QtySortParm = sortOrder == "qty_asc" ? "qty_desc" : "qty_asc";
            ViewBag.NameLabel = String.IsNullOrEmpty(sortOrder) ? "Name Z-A" : "Name A-Z";
            ViewBag.CostLabel = sortOrder == "cost_asc" ? "Cost v" : "Cost ^";
            ViewBag.QuantityLabel = sortOrder == "qty_asc" ? "Quantity v" : "Quantity ^";
            switch (sortOrder)
            {
                case "name_desc":
                    cardCollection = cardCollection.OrderByDescending(s => s.CardDetails.CardName);
                    break;
                case "cost_desc":
                    cardCollection = cardCollection.OrderByDescending(s => s.CardDetails.ConvertedManaCost);
                    break;
                case "cost_asc":
                    cardCollection = cardCollection.OrderBy(s => s.CardDetails.ConvertedManaCost);
                    break;
                case "qty_desc":
                    cardCollection = cardCollection.OrderByDescending(s => s.OwnedQty);
                    break;
                case "qty_asc":
                    cardCollection = cardCollection.OrderBy(s => s.OwnedQty);
                    break;
                default:
                    cardCollection = cardCollection.OrderBy(s => s.CardDetails.CardName);
                    break;
            }


            return View(cardCollection.ToList());
        }

        public ActionResult CreateTrade()
        {
            String id = (db.AspNetUsers.Where(x => x.Email == System.Web.HttpContext.Current.User.Identity.Name).FirstOrDefault().Id);
            var collectorID = db.Collections.Where(y => y.UserID == id).FirstOrDefault().collectorID;

            var cardCollection = db.CardCollection.Where(c => c.collectorID == collectorID);
            cardCollection = cardCollection.OrderBy(s => s.CardDetails.CardName);
            return View(cardCollection.ToList());
        }


        public ActionResult MyTrades()
        {
            String id = (db.AspNetUsers.Where(x => x.Email == System.Web.HttpContext.Current.User.Identity.Name).FirstOrDefault().Id);
            var collectorID = db.Collections.Where(y => y.UserID == id).FirstOrDefault().collectorID;

            var cardCollection = db.CardCollection.Where(c => c.collectorID == collectorID);
            cardCollection = cardCollection.OrderBy(s => s.CardDetails.CardName);
            return View(cardCollection.ToList());
        }

        public ActionResult FriendView(String id)
        {
            var collectorID = db.Collections.Where(y => y.UserID == id.ToString()).FirstOrDefault().collectorID;

            String email = (db.AspNetUsers.Where(x => x.Id == id).FirstOrDefault().Email);
            ViewBag.Friend = email;

            var cardCollection = db.CardCollection.Where(c => c.collectorID == collectorID);
            return View(cardCollection.ToList());
        }

        public ActionResult Search()
        {
            ViewBag.CardID = new SelectList(db.CardDetails.OrderBy(s => s.CardName), "CardID", "CardName");
            ViewBag.collectorID = new SelectList(db.Collections, "collectorID", "UserID");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Search(int CardID)
        {
            return RedirectToAction("SearchResults", new { id = CardID });

        }

        public ActionResult SearchResults(int id)
        {
            var cardCollection = db.CardCollection.Where(c => c.CardID == id).OrderBy(y => y.Collections.AspNetUsers.Email);

            return View(cardCollection);
        }


        public ActionResult MyWants()
        {
            String id = (db.AspNetUsers.Where(x => x.Email == System.Web.HttpContext.Current.User.Identity.Name).FirstOrDefault().Id);
            var collectorID = db.Collections.Where(y => y.UserID == id).FirstOrDefault().collectorID;

            var cardCollection = db.CardCollection.Where(c => c.collectorID == collectorID);
            cardCollection = cardCollection.OrderBy(s => s.CardDetails.CardName);
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
            ViewBag.CardID = new SelectList(db.CardDetails.OrderBy(s => s.CardName), "CardID", "CardName");
            ViewBag.collectorID = new SelectList(db.Collections, "collectorID", "UserID");
            return View();
        }

        public ActionResult CreateWant()
        {
            ViewBag.CardID = new SelectList(db.CardDetails.OrderBy(s => s.CardName), "CardID", "CardName");
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
                String id = (db.AspNetUsers.Where(x => x.Email == System.Web.HttpContext.Current.User.Identity.Name).FirstOrDefault().Id);
                cardCollection.collectorID = db.Collections.Where(y => y.UserID == id).FirstOrDefault().collectorID;
                db.CardCollection.Add(cardCollection);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CardID = new SelectList(db.CardDetails, "CardID", "CardName", cardCollection.CardID);
            ViewBag.collectorID = new SelectList(db.Collections, "collectorID", "UserID", cardCollection.collectorID);
            return View(cardCollection);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateWant([Bind(Include = "CardCollectionID,CardID,ToTrade,Wanted,OwnedQty,TradeQty,WantQty,collectorID")] CardCollection cardCollection)
        {

            if (ModelState.IsValid)
            {
                String id = (db.AspNetUsers.Where(x => x.Email == System.Web.HttpContext.Current.User.Identity.Name).FirstOrDefault().Id);
                cardCollection.collectorID = db.Collections.Where(y => y.UserID == id).FirstOrDefault().collectorID;
                db.CardCollection.Add(cardCollection);
                db.SaveChanges();
                return RedirectToAction("MyWants");
            }
            ViewBag.Error = "Error: That card is already in the deck!";


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

        public ActionResult EditTrades(int? id)
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

        public ActionResult EditWants(int? id)
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

        public ActionResult CompleteTrade(int? id)
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

        public ActionResult CancelTrade(int? id)
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

        public ActionResult DeleteWant(int? id)
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditTrades([Bind(Include = "CardCollectionID,CardID,ToTrade,Wanted,OwnedQty,TradeQty,WantQty,collectorID")] CardCollection cardCollection)
        {
            String id = (db.AspNetUsers.Where(x => x.Email == System.Web.HttpContext.Current.User.Identity.Name).FirstOrDefault().Id);
            cardCollection.collectorID = db.Collections.Where(y => y.UserID == id).FirstOrDefault().collectorID;

            if (ModelState.IsValid)
            {
                db.Entry(cardCollection).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("MyTrades");
            }
            ViewBag.CardID = new SelectList(db.CardDetails, "CardID", "CardName", cardCollection.CardID);
            ViewBag.collectorID = new SelectList(db.Collections, "collectorID", "UserID", cardCollection.collectorID);
            return View(cardCollection);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditWants([Bind(Include = "CardCollectionID,CardID,ToTrade,Wanted,OwnedQty,TradeQty,WantQty,collectorID")] CardCollection cardCollection)
        {
            String id = (db.AspNetUsers.Where(x => x.Email == System.Web.HttpContext.Current.User.Identity.Name).FirstOrDefault().Id);
            cardCollection.collectorID = db.Collections.Where(y => y.UserID == id).FirstOrDefault().collectorID;

            if (ModelState.IsValid)
            {
                db.Entry(cardCollection).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("MyWants");
            }
            ViewBag.CardID = new SelectList(db.CardDetails, "CardID", "CardName", cardCollection.CardID);
            ViewBag.collectorID = new SelectList(db.Collections, "collectorID", "UserID", cardCollection.collectorID);
            return View(cardCollection);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CompleteTrade([Bind(Include = "CardCollectionID,CardID,ToTrade,Wanted,OwnedQty,TradeQty,WantQty,collectorID")] CardCollection cardCollection)
        {
            String id = (db.AspNetUsers.Where(x => x.Email == System.Web.HttpContext.Current.User.Identity.Name).FirstOrDefault().Id);
            cardCollection.collectorID = db.Collections.Where(y => y.UserID == id).FirstOrDefault().collectorID;


            if (ModelState.IsValid)
            {
                db.Entry(cardCollection).State = EntityState.Modified;
                cardCollection.OwnedQty = cardCollection.OwnedQty - cardCollection.TradeQty;
                cardCollection.TradeQty = 0;
                cardCollection.ToTrade = false;
       
                db.SaveChanges();
                return RedirectToAction("MyTrades");
            }
            
            ViewBag.CardID = new SelectList(db.CardDetails, "CardID", "CardName", cardCollection.CardID);
            ViewBag.collectorID = new SelectList(db.Collections, "collectorID", "UserID", cardCollection.collectorID);
            return View(cardCollection);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CancelTrade([Bind(Include = "CardCollectionID,CardID,ToTrade,Wanted,OwnedQty,TradeQty,WantQty,collectorID")] CardCollection cardCollection)
        {
            String id = (db.AspNetUsers.Where(x => x.Email == System.Web.HttpContext.Current.User.Identity.Name).FirstOrDefault().Id);
            cardCollection.collectorID = db.Collections.Where(y => y.UserID == id).FirstOrDefault().collectorID;


            if (ModelState.IsValid)
            {
                db.Entry(cardCollection).State = EntityState.Modified;
                cardCollection.TradeQty = 0;
                cardCollection.ToTrade = false;

                db.SaveChanges();
                return RedirectToAction("MyTrades");
            }

            ViewBag.CardID = new SelectList(db.CardDetails, "CardID", "CardName", cardCollection.CardID);
            ViewBag.collectorID = new SelectList(db.Collections, "collectorID", "UserID", cardCollection.collectorID);
            return View(cardCollection);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteWant([Bind(Include = "CardCollectionID,CardID,ToTrade,Wanted,OwnedQty,TradeQty,WantQty,collectorID")] CardCollection cardCollection)
        {
            String id = (db.AspNetUsers.Where(x => x.Email == System.Web.HttpContext.Current.User.Identity.Name).FirstOrDefault().Id);
            cardCollection.collectorID = db.Collections.Where(y => y.UserID == id).FirstOrDefault().collectorID;


            if (ModelState.IsValid)
            {
                db.Entry(cardCollection).State = EntityState.Modified;
                cardCollection.WantQty = 0;
                cardCollection.Wanted = false;

                db.SaveChanges();
                return RedirectToAction("MyWants");
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
