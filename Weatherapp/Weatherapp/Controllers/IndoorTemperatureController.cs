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
    public class IndoorTemperatureController : Controller
    {
        private WeatherappContext db = new WeatherappContext();

        // GET: IndoorTemperature
        public ActionResult Index()
        {
            return View(db.IndoorTemperatureModels.ToList());
        }

        // GET: IndoorTemperature/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            IndoorTemperatureModel indoorTemperatureModel = db.IndoorTemperatureModels.Find(id);
            if (indoorTemperatureModel == null)
            {
                return HttpNotFound();
            }
            return View(indoorTemperatureModel);
        }

        // GET: IndoorTemperature/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: IndoorTemperature/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IndoorTemperatureModelId,Temperature,Humidity,DateAndTime,Self")] IndoorTemperatureModel indoorTemperatureModel)
        {
            if (ModelState.IsValid)
            {
                db.IndoorTemperatureModels.Add(indoorTemperatureModel);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(indoorTemperatureModel);
        }

        // GET: IndoorTemperature/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            IndoorTemperatureModel indoorTemperatureModel = db.IndoorTemperatureModels.Find(id);
            if (indoorTemperatureModel == null)
            {
                return HttpNotFound();
            }
            return View(indoorTemperatureModel);
        }

        // POST: IndoorTemperature/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IndoorTemperatureModelId,Temperature,Humidity,DateAndTime,Self")] IndoorTemperatureModel indoorTemperatureModel)
        {
            if (ModelState.IsValid)
            {
                db.Entry(indoorTemperatureModel).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(indoorTemperatureModel);
        }

        // GET: IndoorTemperature/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            IndoorTemperatureModel indoorTemperatureModel = db.IndoorTemperatureModels.Find(id);
            if (indoorTemperatureModel == null)
            {
                return HttpNotFound();
            }
            return View(indoorTemperatureModel);
        }

        // POST: IndoorTemperature/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            IndoorTemperatureModel indoorTemperatureModel = db.IndoorTemperatureModels.Find(id);
            db.IndoorTemperatureModels.Remove(indoorTemperatureModel);
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
