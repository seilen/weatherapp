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
    public class OutdoorTemperatureController : Controller
    {
        private WeatherappContext db = new WeatherappContext();

        // GET: OutdoorTemperature
        public ActionResult Index()
        {
            return View(db.OutdoorTemperatureModels.ToList());
        }

        // GET: OutdoorTemperature/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OutdoorTemperatureModel outdoorTemperatureModel = db.OutdoorTemperatureModels.Find(id);
            if (outdoorTemperatureModel == null)
            {
                return HttpNotFound();
            }
            return View(outdoorTemperatureModel);
        }

        // GET: OutdoorTemperature/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: OutdoorTemperature/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "OutdoorTemperatureModelId,Temperature,TemperatureTrend,AvgTemperature,WindChill,HeatIndex,DewPoint,RelHumidity,ApparentTemperature,DateAndTime,Self")] OutdoorTemperatureModel outdoorTemperatureModel)
        {
            if (ModelState.IsValid)
            {
                db.OutdoorTemperatureModels.Add(outdoorTemperatureModel);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(outdoorTemperatureModel);
        }

        // GET: OutdoorTemperature/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OutdoorTemperatureModel outdoorTemperatureModel = db.OutdoorTemperatureModels.Find(id);
            if (outdoorTemperatureModel == null)
            {
                return HttpNotFound();
            }
            return View(outdoorTemperatureModel);
        }

        // POST: OutdoorTemperature/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "OutdoorTemperatureModelId,Temperature,TemperatureTrend,AvgTemperature,WindChill,HeatIndex,DewPoint,RelHumidity,ApparentTemperature,DateAndTime,Self")] OutdoorTemperatureModel outdoorTemperatureModel)
        {
            if (ModelState.IsValid)
            {
                db.Entry(outdoorTemperatureModel).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(outdoorTemperatureModel);
        }

        // GET: OutdoorTemperature/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OutdoorTemperatureModel outdoorTemperatureModel = db.OutdoorTemperatureModels.Find(id);
            if (outdoorTemperatureModel == null)
            {
                return HttpNotFound();
            }
            return View(outdoorTemperatureModel);
        }

        // POST: OutdoorTemperature/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            OutdoorTemperatureModel outdoorTemperatureModel = db.OutdoorTemperatureModels.Find(id);
            db.OutdoorTemperatureModels.Remove(outdoorTemperatureModel);
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
