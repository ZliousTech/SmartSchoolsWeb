using Business.Base;
using Common.Helpers;
using Newtonsoft.Json.Linq;
using SmartSchool.Models.GoogleMapsAirDistanceAPI;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.Mvc;

namespace SmartSchool.Controllers
{
    [Authorize]
    public class GoogleMapsAirDistanceAPIController : BaseController
    {
        BusinessComponentsFactory factory = new BusinessComponentsFactory();

        // GET: GoogleMapsAirDistanceAPI
        public ActionResult Index()
        {
            GoogleMapsAirDistanceAPI model = new GoogleMapsAirDistanceAPI();

            model.SChoolLatitude = 31.9564058;
            model.SchoolLongitude = 35.8453127;
            model.Distanceinmeters = 1000;
            model.Distanceinkilometers = model.Distanceinmeters / 1000;
            return View(model);
        }
    }
}