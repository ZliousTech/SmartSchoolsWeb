using Common.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SmartSchool.Controllers
{
    public class MapController : BaseController
    {
        // GET: Map
        public ActionResult Index()
        {
            return View();
        }
    }
}