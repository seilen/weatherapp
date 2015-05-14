using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Weatherapp.Models;
using Weatherapp.Filters;

namespace Weatherapp.Controllers
{
    public class WeatherController : ApiController
    {
        private WeatherappContext db = new WeatherappContext();

        // GET: api/Weather
        public IQueryable<WeatherModel> GetWeatherModels()
        {
            return db.WeatherModels;
        }

        // GET: api/Weather/5
        [ResponseType(typeof(WeatherModel))]
        public IHttpActionResult GetWeatherModel(int id)
        {
            WeatherModel weatherModel = db.WeatherModels.Find(id);
            if (weatherModel == null)
            {
                return NotFound();
            }

            return Ok(weatherModel);
        }

        // PUT: api/Weather/5
        [ValidateHttpAntiForgeryToken]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutWeatherModel(int id, WeatherModel weatherModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != weatherModel.WeatherModelId)
            {
                return BadRequest();
            }

            db.Entry(weatherModel).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!WeatherModelExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Weather
        [ValidateHttpAntiForgeryToken]
        [ResponseType(typeof(WeatherModel))]
        public IHttpActionResult PostWeatherModel(WeatherModel weatherModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.WeatherModels.Add(weatherModel);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = weatherModel.WeatherModelId }, weatherModel);
        }

        // DELETE: api/Weather/5
        [ValidateHttpAntiForgeryToken]
        [ResponseType(typeof(WeatherModel))]
        public IHttpActionResult DeleteWeatherModel(int id)
        {
            WeatherModel weatherModel = db.WeatherModels.Find(id);
            if (weatherModel == null)
            {
                return NotFound();
            }

            db.WeatherModels.Remove(weatherModel);
            db.SaveChanges();

            return Ok(weatherModel);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool WeatherModelExists(int id)
        {
            return db.WeatherModels.Count(e => e.WeatherModelId == id) > 0;
        }
    }
}