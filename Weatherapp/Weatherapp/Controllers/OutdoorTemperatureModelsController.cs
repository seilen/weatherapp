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
    public class OutdoorTemperatureModelsController : ApiController
    {
        private WeatherappContext db = new WeatherappContext();

        // GET: api/OutdoorTemperatureModels
        public IQueryable<OutdoorTemperatureModel> GetOutdoorTemperatureModels()
        {
            return db.OutdoorTemperatureModels;
        }

        // GET: api/OutdoorTemperatureModels/5
        [ResponseType(typeof(OutdoorTemperatureModel))]
        public IHttpActionResult GetOutdoorTemperatureModel(int id)
        {
            OutdoorTemperatureModel outdoorTemperatureModel = db.OutdoorTemperatureModels.Find(id);
            if (outdoorTemperatureModel == null)
            {
                return NotFound();
            }

            return Ok(outdoorTemperatureModel);
        }

        // PUT: api/OutdoorTemperatureModels/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutOutdoorTemperatureModel(int id, OutdoorTemperatureModel outdoorTemperatureModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != outdoorTemperatureModel.OutdoorTemperatureModelId)
            {
                return BadRequest();
            }

            db.Entry(outdoorTemperatureModel).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OutdoorTemperatureModelExists(id))
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

        // POST: api/OutdoorTemperatureModels
        [ResponseType(typeof(OutdoorTemperatureModel))]
        public IHttpActionResult PostOutdoorTemperatureModel(OutdoorTemperatureModel outdoorTemperatureModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.OutdoorTemperatureModels.Add(outdoorTemperatureModel);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = outdoorTemperatureModel.OutdoorTemperatureModelId }, outdoorTemperatureModel);
        }

        // DELETE: api/OutdoorTemperatureModels/5
        [ResponseType(typeof(OutdoorTemperatureModel))]
        public IHttpActionResult DeleteOutdoorTemperatureModel(int id)
        {
            OutdoorTemperatureModel outdoorTemperatureModel = db.OutdoorTemperatureModels.Find(id);
            if (outdoorTemperatureModel == null)
            {
                return NotFound();
            }

            db.OutdoorTemperatureModels.Remove(outdoorTemperatureModel);
            db.SaveChanges();

            return Ok(outdoorTemperatureModel);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool OutdoorTemperatureModelExists(int id)
        {
            return db.OutdoorTemperatureModels.Count(e => e.OutdoorTemperatureModelId == id) > 0;
        }
    }
}