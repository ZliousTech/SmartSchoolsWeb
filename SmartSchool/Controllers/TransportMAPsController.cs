using Business.Base;
using Common;
using Common.Helpers;
using DataAccess;
using Newtonsoft.Json.Linq;
using Objects;
using SmartSchool.Models.TransportMAPs;
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
    public class TransportMAPsController : BaseController
    {
        BusinessComponentsFactory factory = new BusinessComponentsFactory();
        SmartSchoolsEntities context = new SmartSchoolsEntities();

        // GET: TransportMAPs
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GoogleMapsAirDistanceAPI(int SchoolID)
        {

            TransportMAPs model = new TransportMAPs();
            var schoolManager = factory.CreateSchoolBranchsManager();
            var schoolResult = schoolManager.Find(a => a.SchoolID == SchoolID).FirstOrDefault();
            model.TransportCategoryTypes = (from TranCatTypes in context.TransportCategoryTypes select TranCatTypes).ToList();
            var TransportRes = (from TransportCategory in context.TransportCategories where TransportCategory.SchoolID == SchoolID && TransportCategory.TransportTypeID == 2 select TransportCategory).ToList();
            model.SchoolID = SchoolID;
            model.SChoolLatitude = schoolResult.Latitude.Value;
            model.SchoolLongitude = schoolResult.Longitude.Value;
            if (TransportRes.Count > 0)
            {
                foreach (var item in TransportRes)
                {
                    if (item.TransportCategoryTypeID == 1)
                    {
                        if (item.DistanceInMeters == 0)
                        {

                            model.Distanceinmeters1 = 1000;
                            model.Distanceinkilometers1 = model.Distanceinmeters1 / 1000;

                        }
                        else
                        {
                            model.Distanceinmeters1 = item.DistanceInMeters.Value;
                            model.Distanceinkilometers1 = model.Distanceinmeters1 / 1000;

                        }
                    }
                    else if (item.TransportCategoryTypeID == 2)
                    {
                        if (item.DistanceInMeters == 0)
                        {

                            model.Distanceinmeters2 = 2000;
                            model.Distanceinkilometers2 = model.Distanceinmeters2 / 1000;

                        }
                        else
                        {
                            model.Distanceinmeters2 = item.DistanceInMeters.Value;
                            model.Distanceinkilometers2 = model.Distanceinmeters2 / 1000;

                        }
                    }
                    else if (item.TransportCategoryTypeID == 3)
                    {
                        if (item.DistanceInMeters == 0)
                        {

                            model.Distanceinmeters3 = 3000;
                            model.Distanceinkilometers3 = model.Distanceinmeters3 / 1000;

                        }
                        else
                        {
                            model.Distanceinmeters3 = item.DistanceInMeters.Value;
                            model.Distanceinkilometers3 = model.Distanceinmeters3 / 1000;

                        }
                    }
                    else if (item.TransportCategoryTypeID == 4)
                    {
                        if (item.DistanceInMeters == 0)
                        {

                            model.Distanceinmeters4 = 4000;
                            model.Distanceinkilometers4 = model.Distanceinmeters4 / 1000;

                        }
                        else
                        {
                            model.Distanceinmeters4 = item.DistanceInMeters.Value;
                            model.Distanceinkilometers4 = model.Distanceinmeters4 / 1000;

                        }
                    }
                    else if (item.TransportCategoryTypeID == 5)
                    {
                        if (item.DistanceInMeters == 0)
                        {

                            model.Distanceinmeters5 = 5000;
                            model.Distanceinkilometers5 = model.Distanceinmeters5 / 1000;

                        }
                        else
                        {
                            model.Distanceinmeters5 = item.DistanceInMeters.Value;
                            model.Distanceinkilometers5 = model.Distanceinmeters5 / 1000;


                        }
                    }
                }
            }

            else
            {
                model.Distanceinmeters1 = 1000;
                model.Distanceinkilometers1 = model.Distanceinmeters1 / 1000;
                model.Distanceinmeters2 = 2000;
                model.Distanceinkilometers2 = model.Distanceinmeters2 / 1000;
                model.Distanceinmeters3 = 3000;
                model.Distanceinkilometers3 = model.Distanceinmeters3 / 1000;
                model.Distanceinmeters4 = 4000;
                model.Distanceinkilometers4 = model.Distanceinmeters4 / 1000;
                model.Distanceinmeters5 = 5000;
                model.Distanceinkilometers5 = model.Distanceinmeters5 / 1000;
            }
            return View(model);
        }
        [HttpPost]
        public JsonResult AddUpdateTransportCategories(TransportMAPs model)
        {
            var TransportCategoriesManager = factory.CreateTransportCategorysManager();
            var TransportCategoryTypes = (from TranCatTypes in context.TransportCategoryTypes select TranCatTypes).ToList();
            var schoolManager = factory.CreateSchoolBranchsManager();
            var schoolresult = schoolManager.Find(a => a.SchoolID == model.SchoolID).FirstOrDefault();

            double Distanceinmeters = 0;
            double GoCost = 0;
            double ReturnCost = 0;
            double TwoWayCost = 0;
            foreach (var item in TransportCategoryTypes)
            {
                var TransportRes = (from TransportCategory in context.TransportCategories where TransportCategory.SchoolID == model.SchoolID && TransportCategory.TransportCategoryTypeID == item.TransportCategoryTypeID && TransportCategory.TransportTypeID == 2 select TransportCategory).FirstOrDefault();

                if (item.TransportCategoryTypeID == 1)
                {
                    Distanceinmeters = model.Distanceinmeters1;
                    GoCost = model.GoCost_1;
                    ReturnCost = model.ReturnCost_1;
                    TwoWayCost = model.TwoWay_1;
                }
                else if (item.TransportCategoryTypeID == 2)
                {
                    Distanceinmeters = model.Distanceinmeters2;
                    GoCost = model.GoCost_2;
                    ReturnCost = model.ReturnCost_2;
                    TwoWayCost = model.TwoWay_2;
                }
                else if (item.TransportCategoryTypeID == 3)
                {
                    Distanceinmeters = model.Distanceinmeters2;
                    GoCost = model.GoCost_3;
                    ReturnCost = model.ReturnCost_3;
                    TwoWayCost = model.TwoWay_3;
                }
                else if (item.TransportCategoryTypeID == 4)
                {
                    Distanceinmeters = model.Distanceinmeters4;
                    GoCost = model.GoCost_4;
                    ReturnCost = model.ReturnCost_4;
                    TwoWayCost = model.TwoWay_4;
                }
                else if (item.TransportCategoryTypeID == 5)
                {
                    Distanceinmeters = model.Distanceinmeters5;
                    GoCost = model.GoCost_5;
                    ReturnCost = model.ReturnCost_5;
                    TwoWayCost = model.TwoWay_5;
                }
                if (TransportRes != null)
                {
                    TransportRes.DistanceInMeters = Distanceinmeters;
                    TransportRes.TransportCategoryCostGo = GoCost;
                    TransportRes.TransportCategoryCostReturn = ReturnCost;
                    TransportRes.TransportCategoryCostTwoWay = TwoWayCost;
                    TransportCategoriesManager.Update(TransportRes);
                }
                else
                {
                    TransportCategory obj = new TransportCategory();
                    obj.DistanceInMeters = Distanceinmeters;
                    obj.TransportCategoryCostGo = GoCost;
                    obj.TransportCategoryCostReturn = ReturnCost;
                    obj.TransportCategoryCostTwoWay = TwoWayCost;
                    obj.SchoolID = model.SchoolID;
                    obj.TransportCategoryTypeID = item.TransportCategoryTypeID;
                    obj.TransportTypeID = 2;
                    TransportCategoriesManager.Add(obj);
                }
            }
            schoolresult.IsRegistered = true;
            schoolManager.Update(schoolresult);
            return Json(new { Success = true }, JsonRequestBehavior.AllowGet);

        }
        public ActionResult GoogleMapsDistanceAPI()
        {
            TransportMAPs model = new TransportMAPs();

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
                //string jsonData = DistanceMatrixRequest("31.9604899962494,35.848434140599", "31.9769664,35.8585633", "Driving", "<%=ConfigurationManager.AppSettings["GoogleAPIKey"] %>");
                //string jsonData = DistanceMatrixRequest(Origins, Distination, "Driving", "<%=ConfigurationManager.AppSettings["GoogleAPIKey"] %>");
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

        public PartialViewResult StudentSchoolMapAPI(int StudentID)  //only for the new students
        {
            TransportMAPs model = new TransportMAPs();
            var ExternalStudentAddressManager = factory.CreateExternalStudentAdresssManager();
            var ExternalStudentManager = factory.CreateExternalStudentsManager();
            var SchoolBranchManager = factory.CreateSchoolBranchsManager();
            var TransportCategories = factory.CreateTransportCategorysManager();
            var CategoryTypesManager = factory.CreateTransportCategoryTypesManager();
            int SchoolID = Convert.ToInt32(GetCookie((int)clsenumration.UserData.SchoolID));
            int GuardianID = ExternalStudentManager.Find(a => a.StudentID == StudentID).Select(a => a.GuardianID.Value).FirstOrDefault();
            var StudentAddress = ExternalStudentAddressManager.Find(a => a.GuardianID == GuardianID).FirstOrDefault();
            var SchoolAddress = SchoolBranchManager.Find(a => a.SchoolID == SchoolID).FirstOrDefault();
            model.SChoolLatitude = SchoolAddress.Latitude.Value;
            model.SchoolLongitude = SchoolAddress.Longitude.Value;
            model.StudentLatitude = StudentAddress.Latitude.Value;
            model.StudentLongitude = StudentAddress.Longitude.Value;
            var CategoriesDistances = TransportCategories.Find(a => a.SchoolID == SchoolID).ToList();
            string Origins = model.SChoolLatitude.ToString() + ',' + model.SchoolLongitude.ToString();
            string Distination = model.StudentLatitude.ToString() + ',' + model.StudentLongitude.ToString();
            var ExternalStudentResult = ExternalStudentManager.Find(a => a.StudentID == StudentID).FirstOrDefault();
            if (ExternalStudentResult.TransportDirectionID == 0)
            {
                model.SubscribeTransportation = CurrentLanguage == Languges.English ? "Goes Only(one way)" : "ذهاب فقط";
            }
            else if (ExternalStudentResult.TransportDirectionID == 1)
            {
                model.SubscribeTransportation = CurrentLanguage == Languges.English ? "Comes Only(one way)" : "اياب فقط";

            }
            else if (ExternalStudentResult.TransportDirectionID == 2)
            {
                model.SubscribeTransportation = CurrentLanguage == Languges.English ? "Round-trip(two way)" : "اتجاهين";
            }
            else if (ExternalStudentResult.TransportDirectionID == 3)
            {
                model.SubscribeTransportation = CurrentLanguage == Languges.English ? "He does not wish to subcripe in transportation" : "لا يرغب الاشتراك في المواصلات";
            }
            double DistanceM;
            double DistanceKM;
            try
            {

                //travelMode = Driving, Walking, Bicycling, Transit.
                //string jsonData = DistanceMatrixRequest("31.9604899962494,35.848434140599", "31.9769664,35.8585633", "Driving", "<%=ConfigurationManager.AppSettings["GoogleAPIKey"] %>");
                //string jsonData = DistanceMatrixRequest(Origins, Distination, "Driving", "<%=ConfigurationManager.AppSettings["GoogleAPIKey"] %>");
                string jsonData = DistanceMatrixRequest(Origins, Distination, "Driving", ConfigurationManager.AppSettings["GoogleAPIKey"]);
                JObject o = JObject.Parse(jsonData);

                DistanceM = (double)o.SelectToken("rows[0].elements[0].distance.value");
                DistanceKM = DistanceM / 1000;
                DistanceKM = Math.Round(DistanceKM, 1);
                if (CategoriesDistances.Count > 0)
                {
                    foreach (var item in CategoriesDistances)
                    {
                        if (DistanceM <= item.DistanceInMeters)
                        {
                            var Category = CategoryTypesManager.Find(a => a.TransportCategoryTypeID == item.TransportCategoryTypeID).FirstOrDefault();
                            model.Category = CurrentLanguage == Languges.English ? Category.TransportCategoryEnglishName : Category.TransportCategoryArabicName;
                            break;
                        }

                    }
                }
                if (string.IsNullOrEmpty(model.Category))
                {
                    model.Category = CurrentLanguage == Languges.English ? "Category Five" : "الفئة الخامسة";
                }
            }
            catch
            {
                DistanceM = 0;
                DistanceKM = 0;
            }

            //model.TextFromJSON = json1 ?? "";
            model.DistanceinM = DistanceM.ToString();
            model.DistanceinKM = DistanceKM.ToString();
            return PartialView("_StudentSchoolMapAPI", model);
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