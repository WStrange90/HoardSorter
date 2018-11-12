﻿using System;
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
    public class DeckCardsController : Controller
    {
        private HoardSorterEntities db = new HoardSorterEntities();

        // GET: DeckCards
        public ActionResult Index()
        {
            var deckCards = db.DeckCards.Include(d => d.CardDetails).Include(d => d.Deck);
            return View(deckCards.ToList());
        }

        // GET: DeckCards/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DeckCards deckCards = db.DeckCards.Find(id);
            if (deckCards == null)
            {
                return HttpNotFound();
            }
            return View(deckCards);
        }

        // GET: DeckCards/Create
        public ActionResult Create()
        {
            ViewBag.CardID = new SelectList(db.CardDetails, "CardID", "CardName");
            ViewBag.DeckID = new SelectList(db.Deck, "DeckID", "UserID");
            return View();
        }

        // POST: DeckCards/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "DeckCardID,DeckID,CardID,CardQty")] DeckCards deckCards)
        {
            if (ModelState.IsValid)
            {
                db.DeckCards.Add(deckCards);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CardID = new SelectList(db.CardDetails, "CardID", "CardName", deckCards.CardID);
            ViewBag.DeckID = new SelectList(db.Deck, "DeckID", "UserID", deckCards.DeckID);
            return View(deckCards);
        }

        // GET: DeckCards/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DeckCards deckCards = db.DeckCards.Find(id);
            if (deckCards == null)
            {
                return HttpNotFound();
            }
            ViewBag.CardID = new SelectList(db.CardDetails, "CardID", "CardName", deckCards.CardID);
            ViewBag.DeckID = new SelectList(db.Deck, "DeckID", "UserID", deckCards.DeckID);
            return View(deckCards);
        }

        // POST: DeckCards/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "DeckCardID,DeckID,CardID,CardQty")] DeckCards deckCards)
        {
            if (ModelState.IsValid)
            {
                db.Entry(deckCards).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CardID = new SelectList(db.CardDetails, "CardID", "CardName", deckCards.CardID);
            ViewBag.DeckID = new SelectList(db.Deck, "DeckID", "UserID", deckCards.DeckID);
            return View(deckCards);
        }

        // GET: DeckCards/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DeckCards deckCards = db.DeckCards.Find(id);
            if (deckCards == null)
            {
                return HttpNotFound();
            }
            return View(deckCards);
        }

        // POST: DeckCards/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DeckCards deckCards = db.DeckCards.Find(id);
            db.DeckCards.Remove(deckCards);
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
