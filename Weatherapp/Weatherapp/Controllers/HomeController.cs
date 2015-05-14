using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Weatherapp.Models;
using Weatherapp.Filters;

namespace Weatherapp.Controllers
{
    public class HomeController : Controller
    {
        private WeatherappContext db = new WeatherappContext();

        // GET: Home
        public ActionResult Index()
        {
            return View(db.WeatherModels.ToList());
        }

        // GET: Home/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WeatherModel weatherModel = db.WeatherModels.Find(id);
            if (weatherModel == null)
            {
                return HttpNotFound();
            }
            return View(weatherModel);
        }

        // GET: Home/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Home/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "WeatherModelId,DateAndTime,OutdoorTemperatureModel,IndoorTemperatureModel,WindModel,BarometerModel,RainfallModel,RecentExtremesModel,Self")] WeatherModel weatherModel)
        {
            if (ModelState.IsValid)
            {
                db.WeatherModels.Add(weatherModel);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(weatherModel);
        }

        // GET: Home/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WeatherModel weatherModel = db.WeatherModels.Find(id);
            if (weatherModel == null)
            {
                return HttpNotFound();
            }
            return View(weatherModel);
        }

        // POST: Home/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "WeatherModelId,DateAndTime,OutdoorTemperatureModel,IndoorTemperatureModel,WindModel,BarometerModel,RainfallModel,RecentExtremesModel,Self")] WeatherModel weatherModel)
        {
            if (ModelState.IsValid)
            {
                db.Entry(weatherModel).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(weatherModel);
        }

        // GET: Home/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WeatherModel weatherModel = db.WeatherModels.Find(id);
            if (weatherModel == null)
            {
                return HttpNotFound();
            }
            return View(weatherModel);
        }

        // POST: Home/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            WeatherModel weatherModel = db.WeatherModels.Find(id);
            db.WeatherModels.Remove(weatherModel);
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
