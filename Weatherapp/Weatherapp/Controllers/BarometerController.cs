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
    public class BarometerController : Controller
    {
        private WeatherappContext db = new WeatherappContext();

        // GET: Barometer
        public ActionResult Index()
        {
            return View(db.BarometerModels.ToList());
        }

        // GET: Barometer/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BarometerModel barometerModel = db.BarometerModels.Find(id);
            if (barometerModel == null)
            {
                return HttpNotFound();
            }
            return View(barometerModel);
        }

        // GET: Barometer/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Barometer/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "BarometerModelId,Pressure,PressureTrend,DateAndTime,Self")] BarometerModel barometerModel)
        {
            if (ModelState.IsValid)
            {
                db.BarometerModels.Add(barometerModel);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(barometerModel);
        }

        // GET: Barometer/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BarometerModel barometerModel = db.BarometerModels.Find(id);
            if (barometerModel == null)
            {
                return HttpNotFound();
            }
            return View(barometerModel);
        }

        // POST: Barometer/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "BarometerModelId,Pressure,PressureTrend,DateAndTime,Self")] BarometerModel barometerModel)
        {
            if (ModelState.IsValid)
            {
                db.Entry(barometerModel).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(barometerModel);
        }

        // GET: Barometer/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BarometerModel barometerModel = db.BarometerModels.Find(id);
            if (barometerModel == null)
            {
                return HttpNotFound();
            }
            return View(barometerModel);
        }

        // POST: Barometer/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            BarometerModel barometerModel = db.BarometerModels.Find(id);
            db.BarometerModels.Remove(barometerModel);
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
