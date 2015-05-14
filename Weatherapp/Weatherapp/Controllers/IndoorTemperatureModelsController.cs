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

namespace Weatherapp.Controllers
{
    public class IndoorTemperatureModelsController : ApiController
    {
        private WeatherappContext db = new WeatherappContext();

        // GET: api/IndoorTemperatureModels
        public IQueryable<IndoorTemperatureModel> GetIndoorTemperatureModels()
        {
            return db.IndoorTemperatureModels;
        }

        // GET: api/IndoorTemperatureModels/5
        [ResponseType(typeof(IndoorTemperatureModel))]
        public IHttpActionResult GetIndoorTemperatureModel(int id)
        {
            IndoorTemperatureModel indoorTemperatureModel = db.IndoorTemperatureModels.Find(id);
            if (indoorTemperatureModel == null)
            {
                return NotFound();
            }

            return Ok(indoorTemperatureModel);
        }

        // PUT: api/IndoorTemperatureModels/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutIndoorTemperatureModel(int id, IndoorTemperatureModel indoorTemperatureModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != indoorTemperatureModel.IndoorTemperatureModelId)
            {
                return BadRequest();
            }

            db.Entry(indoorTemperatureModel).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!IndoorTemperatureModelExists(id))
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

        // POST: api/IndoorTemperatureModels
        [ResponseType(typeof(IndoorTemperatureModel))]
        public IHttpActionResult PostIndoorTemperatureModel(IndoorTemperatureModel indoorTemperatureModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.IndoorTemperatureModels.Add(indoorTemperatureModel);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = indoorTemperatureModel.IndoorTemperatureModelId }, indoorTemperatureModel);
        }

        // DELETE: api/IndoorTemperatureModels/5
        [ResponseType(typeof(IndoorTemperatureModel))]
        public IHttpActionResult DeleteIndoorTemperatureModel(int id)
        {
            IndoorTemperatureModel indoorTemperatureModel = db.IndoorTemperatureModels.Find(id);
            if (indoorTemperatureModel == null)
            {
                return NotFound();
            }

            db.IndoorTemperatureModels.Remove(indoorTemperatureModel);
            db.SaveChanges();

            return Ok(indoorTemperatureModel);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool IndoorTemperatureModelExists(int id)
        {
            return db.IndoorTemperatureModels.Count(e => e.IndoorTemperatureModelId == id) > 0;
        }
    }
}