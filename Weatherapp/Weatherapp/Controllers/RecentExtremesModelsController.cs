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
    public class RecentExtremesModelsController : ApiController
    {
        private WeatherappContext db = new WeatherappContext();

        // GET: api/RecentExtremesModels
        public IQueryable<RecentExtremesModel> GetRecentExtremesModels()
        {
            return db.RecentExtremesModels;
        }

        // GET: api/RecentExtremesModels/5
        [ResponseType(typeof(RecentExtremesModel))]
        public IHttpActionResult GetRecentExtremesModel(int id)
        {
            RecentExtremesModel recentExtremesModel = db.RecentExtremesModels.Find(id);
            if (recentExtremesModel == null)
            {
                return NotFound();
            }

            return Ok(recentExtremesModel);
        }

        // PUT: api/RecentExtremesModels/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutRecentExtremesModel(int id, RecentExtremesModel recentExtremesModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != recentExtremesModel.RecentExtremesModelId)
            {
                return BadRequest();
            }

            db.Entry(recentExtremesModel).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RecentExtremesModelExists(id))
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

        // POST: api/RecentExtremesModels
        [ResponseType(typeof(RecentExtremesModel))]
        public IHttpActionResult PostRecentExtremesModel(RecentExtremesModel recentExtremesModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.RecentExtremesModels.Add(recentExtremesModel);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = recentExtremesModel.RecentExtremesModelId }, recentExtremesModel);
        }

        // DELETE: api/RecentExtremesModels/5
        [ResponseType(typeof(RecentExtremesModel))]
        public IHttpActionResult DeleteRecentExtremesModel(int id)
        {
            RecentExtremesModel recentExtremesModel = db.RecentExtremesModels.Find(id);
            if (recentExtremesModel == null)
            {
                return NotFound();
            }

            db.RecentExtremesModels.Remove(recentExtremesModel);
            db.SaveChanges();

            return Ok(recentExtremesModel);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool RecentExtremesModelExists(int id)
        {
            return db.RecentExtremesModels.Count(e => e.RecentExtremesModelId == id) > 0;
        }
    }
}