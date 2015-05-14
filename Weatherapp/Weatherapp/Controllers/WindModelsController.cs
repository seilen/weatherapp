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
    public class WindModelsController : ApiController
    {
        private WeatherappContext db = new WeatherappContext();

        // GET: api/WindModels
        public IQueryable<WindModel> GetWindModels()
        {
            return db.WindModels;
        }

        // GET: api/WindModels/5
        [ResponseType(typeof(WindModel))]
        public IHttpActionResult GetWindModel(int id)
        {
            WindModel windModel = db.WindModels.Find(id);
            if (windModel == null)
            {
                return NotFound();
            }

            return Ok(windModel);
        }

        // PUT: api/WindModels/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutWindModel(int id, WindModel windModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != windModel.WindModelId)
            {
                return BadRequest();
            }

            db.Entry(windModel).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!WindModelExists(id))
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

        // POST: api/WindModels
        [ResponseType(typeof(WindModel))]
        public IHttpActionResult PostWindModel(WindModel windModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.WindModels.Add(windModel);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = windModel.WindModelId }, windModel);
        }

        // DELETE: api/WindModels/5
        [ResponseType(typeof(WindModel))]
        public IHttpActionResult DeleteWindModel(int id)
        {
            WindModel windModel = db.WindModels.Find(id);
            if (windModel == null)
            {
                return NotFound();
            }

            db.WindModels.Remove(windModel);
            db.SaveChanges();

            return Ok(windModel);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool WindModelExists(int id)
        {
            return db.WindModels.Count(e => e.WindModelId == id) > 0;
        }
    }
}