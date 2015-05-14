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
    public class WindController : Controller
    {
        private WeatherappContext db = new WeatherappContext();

        // GET: Wind
        public ActionResult Index()
        {
            return View(db.WindModels.ToList());
        }

        // GET: Wind/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WindModel windModel = db.WindModels.Find(id);
            if (windModel == null)
            {
                return HttpNotFound();
            }
            return View(windModel);
        }

        // GET: Wind/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Wind/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "WindModelId,LatestWind,Bearing,Gust,AverageWind,AvgDirection,WindRun,DateAndTime,Self")] WindModel windModel)
        {
            if (ModelState.IsValid)
            {
                db.WindModels.Add(windModel);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(windModel);
        }

        // GET: Wind/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WindModel windModel = db.WindModels.Find(id);
            if (windModel == null)
            {
                return HttpNotFound();
            }
            return View(windModel);
        }

        // POST: Wind/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "WindModelId,LatestWind,Bearing,Gust,AverageWind,AvgDirection,WindRun,DateAndTime,Self")] WindModel windModel)
        {
            if (ModelState.IsValid)
            {
                db.Entry(windModel).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(windModel);
        }

        // GET: Wind/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WindModel windModel = db.WindModels.Find(id);
            if (windModel == null)
            {
                return HttpNotFound();
            }
            return View(windModel);
        }

        // POST: Wind/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            WindModel windModel = db.WindModels.Find(id);
            db.WindModels.Remove(windModel);
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
