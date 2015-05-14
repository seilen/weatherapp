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
    public class RainfallModelsController : ApiController
    {
        private WeatherappContext db = new WeatherappContext();

        // GET: api/RainfallModels
        public IQueryable<RainfallModel> GetRainfallModels()
        {
            return db.RainfallModels;
        }

        // GET: api/RainfallModels/5
        [ResponseType(typeof(RainfallModel))]
        public IHttpActionResult GetRainfallModel(int id)
        {
            RainfallModel rainfallModel = db.RainfallModels.Find(id);
            if (rainfallModel == null)
            {
                return NotFound();
            }

            return Ok(rainfallModel);
        }

        // PUT: api/RainfallModels/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutRainfallModel(int id, RainfallModel rainfallModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != rainfallModel.RainfallModelId)
            {
                return BadRequest();
            }

            db.Entry(rainfallModel).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RainfallModelExists(id))
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

        // POST: api/RainfallModels
        [ResponseType(typeof(RainfallModel))]
        public IHttpActionResult PostRainfallModel(RainfallModel rainfallModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.RainfallModels.Add(rainfallModel);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = rainfallModel.RainfallModelId }, rainfallModel);
        }

        // DELETE: api/RainfallModels/5
        [ResponseType(typeof(RainfallModel))]
        public IHttpActionResult DeleteRainfallModel(int id)
        {
            RainfallModel rainfallModel = db.RainfallModels.Find(id);
            if (rainfallModel == null)
            {
                return NotFound();
            }

            db.RainfallModels.Remove(rainfallModel);
            db.SaveChanges();

            return Ok(rainfallModel);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool RainfallModelExists(int id)
        {
            return db.RainfallModels.Count(e => e.RainfallModelId == id) > 0;
        }
    }
}