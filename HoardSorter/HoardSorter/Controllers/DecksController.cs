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
    public class DecksController : Controller
    {
        private HoardSorterEntities db = new HoardSorterEntities();

        // GET: Decks
        public ActionResult Index()
        {
            String UserID = db.AspNetUsers.Where(x => x.Email == System.Web.HttpContext.Current.User.Identity.Name).FirstOrDefault().Id;
            var deck = db.Deck.Where(y => (y.UserID.ToString()).Equals(UserID));
            return View(deck.ToList());
        }

        // GET: Decks/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Deck deck = db.Deck.Find(id);
            if (deck == null)
            {
                return HttpNotFound();
            }
            return View(deck);
        }

        // GET: Decks/Create
        public ActionResult Create()
        {
            
            ViewBag.DeckTypeID = new SelectList(db.DeckType, "DeckTypeID", "DeckType1");
            return View();
        }

        // POST: Decks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "DeckID,UserID,DeckTypeID,DeckName")] Deck deck)
        {
            deck.UserID = db.AspNetUsers.Where(x => x.Email == System.Web.HttpContext.Current.User.Identity.Name).FirstOrDefault().Id;
            if (ModelState.IsValid)
            {
                db.Deck.Add(deck);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.UserID = new SelectList(db.AspNetUsers, "Id", "Email", deck.UserID);
            ViewBag.DeckTypeID = new SelectList(db.DeckType, "DeckTypeID", "DeckType1", deck.DeckTypeID);
            return View(deck);
        }

        // GET: Decks/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Deck deck = db.Deck.Find(id);
            if (deck == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserID = new SelectList(db.AspNetUsers, "Id", "Email", deck.UserID);
            ViewBag.DeckTypeID = new SelectList(db.DeckType, "DeckTypeID", "DeckType1", deck.DeckTypeID);
            return View(deck);
        }

        // POST: Decks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "DeckID,UserID,DeckTypeID,DeckName")] Deck deck)
        {
            deck.UserID = db.AspNetUsers.Where(x => x.Email == System.Web.HttpContext.Current.User.Identity.Name).FirstOrDefault().Id;
            if (ModelState.IsValid)
            {
                db.Entry(deck).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UserID = new SelectList(db.AspNetUsers, "Id", "Email", deck.UserID);
            ViewBag.DeckTypeID = new SelectList(db.DeckType, "DeckTypeID", "DeckType1", deck.DeckTypeID);
            return View(deck);
        }

        // GET: Decks/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Deck deck = db.Deck.Find(id);
            if (deck == null)
            {
                return HttpNotFound();
            }
            return View(deck);
        }

        // POST: Decks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Deck deck = db.Deck.Find(id);

            var cards = db.DeckCards.Where(y => y.DeckID == deck.DeckID);
            foreach (DeckCards card in cards)
            {
                db.DeckCards.Remove(card);
            }

            db.Deck.Remove(deck);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        // GET: DeckCards/DeckContents
        public ActionResult DeckContents(int DeckID, string message)
        {
            ViewBag.Message = message;
            Deck deck = new Deck();
            deck = db.Deck.Single(x => x.DeckID == DeckID);
            ViewBag.DeckName = deck.DeckName;
            ViewBag.CardID = new SelectList(db.CardDetails, "CardID", "CardName");
            ViewBag.DeckID = DeckID;
            return View();
        }

        // POST: Deckcards/Deckcontents
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeckContents([Bind(Include = "DeckCardID,DeckID,CardID,CardQty")] DeckCards deckCards)
        {
            int deckid = deckCards.DeckID;
            deckCards.CardQty = 1;
            List<DeckCards> deck = db.DeckCards.Where(x => x.DeckID == deckCards.DeckID).ToList();
            bool duplicateCard = false;
            foreach(var item in deck)
            {
                if (item.CardID == deckCards.CardID)
                {
                    duplicateCard = true;
                }
            }
            
            if (ModelState.IsValid && !duplicateCard)
            {
                db.DeckCards.Add(deckCards);
                db.SaveChanges();
                
                return RedirectToAction("DeckContents", new {DeckID = deckid, message = "Card added successfully!"});
            }
            ViewBag.Error = "Error: That card is already in the deck!";
            ViewBag.CardID = new SelectList(db.CardDetails, "CardID", "CardName", deckCards.CardID);
            ViewBag.DeckID = deckCards.DeckID;
            return View(deckCards);
            /*
             if (id == null)
             {
                 return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
             }
             Deck deck = db.Deck.Find(id);
             if (deck == null)
             {
                 return HttpNotFound();
             }
             ViewBag.UserID = new SelectList(db.AspNetUsers, "Id", "Email", deck.UserID);
             ViewBag.DeckTypeID = new SelectList(db.DeckType, "DeckTypeID", "DeckType1", deck.DeckTypeID);
             return View(deck);
             */
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
