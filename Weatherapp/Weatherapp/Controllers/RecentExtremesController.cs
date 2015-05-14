using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Weatherapp.Models;

namespace Weatherapp.Controllers
{
    public class RecentExtremesController : Controller
    {
        private WeatherappContext db = new WeatherappContext();

        // GET: RecentExtremes
        public ActionResult Index()
        {
            return View(db.RecentExtremesModels.ToList());
        }

        // GET: RecentExtremes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RecentExtremesModel recentExtremesModel = db.RecentExtremesModels.Find(id);
            if (recentExtremesModel == null)
            {
                return HttpNotFound();
            }
            return View(recentExtremesModel);
        }

        // GET: RecentExtremes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: RecentExtremes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "RecentExtremesModelId,HighWind,HighGust,Bearing,MinTemp,MaxTemp,MinPressure,MaxPressure,RainRate,DateAndTime,Self")] RecentExtremesModel recentExtremesModel)
        {
            if (ModelState.IsValid)
            {
                db.RecentExtremesModels.Add(recentExtremesModel);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(recentExtremesModel);
        }

        // GET: RecentExtremes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RecentExtremesModel recentExtremesModel = db.RecentExtremesModels.Find(id);
            if (recentExtremesModel == null)
            {
                return HttpNotFound();
            }
            return View(recentExtremesModel);
        }

        // POST: RecentExtremes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "RecentExtremesModelId,HighWind,HighGust,Bearing,MinTemp,MaxTemp,MinPressure,MaxPressure,RainRate,DateAndTime,Self")] RecentExtremesModel recentExtremesModel)
        {
            if (ModelState.IsValid)
            {
                db.Entry(recentExtremesModel).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(recentExtremesModel);
        }

        // GET: RecentExtremes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RecentExtremesModel recentExtremesModel = db.RecentExtremesModels.Find(id);
            if (recentExtremesModel == null)
            {
                return HttpNotFound();
            }
            return View(recentExtremesModel);
        }

        // POST: RecentExtremes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            RecentExtremesModel recentExtremesModel = db.RecentExtremesModels.Find(id);
            db.RecentExtremesModels.Remove(recentExtremesModel);
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
