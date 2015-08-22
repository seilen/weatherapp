using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Weatherapp.Controllers
{
    public class FrontPageController : Controller
    {
        // GET: FrontPage
        public ActionResult Index()
        {
            //return View(db.WeatherModels.ToList());
            return Redirect(Url.Content("~/PWdashboard-master/index.htm"));
        }

    }
}