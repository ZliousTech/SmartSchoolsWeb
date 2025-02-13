using Business.Base;
using Common.Helpers;
using Newtonsoft.Json.Linq;
using SmartSchool.Models.GoogleMapsDistanceAPI;
using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.Mvc;

namespace SmartSchool.Controllers
{
    [Authorize]
    public class GoogleMapsDistanceAPIController : BaseController
    {
        BusinessComponentsFactory factory = new BusinessComponentsFactory();
        
        // GET: GoogleMapsDistanceAPI
        public ActionResult Index()
        {
            GoogleMapsDistanceAPI model = new GoogleMapsDistanceAPI();

            model.SChoolLatitude = 31.9604899962494;
            model.SchoolLongitude = 35.848434140599;
            model.StudentLatitude = 31.9769664;
            model.StudentLongitude = 35.8585633;

            string Origins = model.SChoolLatitude.ToString() + ',' + model.SchoolLongitude.ToString();
            string Distination = model.StudentLatitude.ToString() + ',' + model.StudentLongitude.ToString();

            double DistanceM;
            double DistanceKM;

            try
            {
                //travelMode = Driving, Walking, Bicycling, Transit.
                //string jsonData = DistanceMatrixRequest("31.9604899962494,35.848434140599", "31.9769664,35.8585633", "Driving", "AIzaSyAC4eYLfDTVH4rIJsY4ZnwZpNBnWugR4wg");
                string jsonData = DistanceMatrixRequest(Origins, Distination, "Driving", ConfigurationManager.AppSettings["GoogleAPIKey"]);
                JObject o = JObject.Parse(jsonData);

                DistanceM = (double)o.SelectToken("rows[0].elements[0].distance.value");
                DistanceKM = DistanceM / 1000;
                DistanceKM = Math.Round(DistanceKM, 1);
            }
            catch
            {
                DistanceM = 0;
                DistanceKM = 0;
            }

            //model.TextFromJSON = json1 ?? "";
            model.DistanceinM = DistanceM.ToString();
            model.DistanceinKM = DistanceKM.ToString();
            return View(model);
        }

        public string DistanceMatrixRequest(string source, string Destination, string travelMode, string keyString)
        {
            try
            {
                string urlRequest = "";
                urlRequest = @"https://maps.googleapis.com/maps/api/distancematrix/json?origins=" + source + "&destinations=" + Destination + "&mode=" + travelMode + "&key=" + keyString + "&sensor=false";

                WebRequest request = WebRequest.Create(urlRequest);
                request.Method = "POST";
                string postData = "This is a test that posts this string to a Web server.";
                byte[] byteArray = Encoding.UTF8.GetBytes(postData);

                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = byteArray.Length;

                Stream dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();

                WebResponse response = request.GetResponse();
                dataStream = response.GetResponseStream();

                StreamReader reader = new StreamReader(dataStream);
                string resp = reader.ReadToEnd();

                reader.Close();
                dataStream.Close();
                response.Close();

                return resp;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}