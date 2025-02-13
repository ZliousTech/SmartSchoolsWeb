using Business.Base;
using Common;
using Common.Base;
using Common.Helpers;
using DataAccess;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Objects;
using Objects.DTO;
using SmartSchool.Models.transportation;
using SmartSchool.Models.TransportMAPs;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.Mvc;

namespace SmartSchool.Controllers
{
    [Authorize]
    public class transportationController : BaseController
    {
        BusinessComponentsFactory factory = new BusinessComponentsFactory();

        // GET: Buses
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult BusInfo()
        {

            return View();
        }

        public ActionResult TourInfo()
        {

            return View();
        }

        public ActionResult ListBus()
        {
            SmartSchoolsEntities entities = new SmartSchoolsEntities();
            int __SchoolID = Convert.ToInt32(GetCookie((int)clsenumration.UserData.SchoolID));
            string lang = "";
            if (ViewBag.CurrentLanguage == Languges.English)
            {
                lang = "en";
            }
            else
            {
                lang = "ar";
            }

            var query = (from Bus in entities.BusInfoes
                         where Bus.SchoolID == __SchoolID
                         select new transportationDTO
                         {
                             BusID = Bus.BusID,
                             BusNo = Bus.BusNo,
                             PlateNumber = Bus.PlateNumber,
                             Driver = lang.Contains("en") ?
                             entities.Staffs.Where(c => c.StaffID == Bus.DriverID).Select(c => c.StaffEnglishName).FirstOrDefault() :
                             entities.Staffs.Where(c => c.StaffID == Bus.DriverID).Select(c => c.StaffArabicName).FirstOrDefault(),
                             Attendant = lang.Contains("en") ?
                             entities.Staffs.Where(c => c.StaffID == Bus.AttendantID).Select(c => c.StaffEnglishName).FirstOrDefault() :
                             entities.Staffs.Where(c => c.StaffID == Bus.AttendantID).Select(c => c.StaffArabicName).FirstOrDefault(),
                             SecondAttendant = lang.Contains("en") ?
                             entities.Staffs.Where(c => c.StaffID == Bus.SecondAttendantID).Select(c => c.StaffEnglishName).FirstOrDefault() :
                             entities.Staffs.Where(c => c.StaffID == Bus.SecondAttendantID).Select(c => c.StaffArabicName).FirstOrDefault(),
                             NumberOfseats = Bus.NumberofSeats.Value,
                             ParkingArea = Bus.ParkingArea,
                             BusLicenseDate = Bus.BusLicenseDate.Value
                         }).OrderByDescending(c => c.BusNo).ToList();

            return View(query);
        }

        public ActionResult AddBus()
        {
            TransportatioModel model = new TransportatioModel();
            PrepareAddBusModel(model);
            return View(model);
        }

        [HttpPost]
        public JsonResult AddBus(TransportatioModel model)
        {
            try
            {
                string lang = "";
                if (ViewBag.CurrentLanguage == Languges.English)
                {
                    lang = "en";
                }
                else
                {
                    lang = "ar";
                }

                var BusInfoManager = factory.CreateBusInfosManager();
                int _SchoolID = Convert.ToInt32(GetCookie((int)clsenumration.UserData.SchoolID));

                /* Check BusNumber if exist */
                var BusInfo = BusInfoManager.Find(c => c.BusNo.ToString().Trim() == model.BusNo.ToString().Trim() && c.SchoolID == _SchoolID).FirstOrDefault();
                if (BusInfo != null && BusInfo.BusNo.Count() > 0)
                {
                    return Json(new { Success = false, Message = 
                        lang.Contains("en") 
                        ? 
                        "Something went wrong, there is another bus with the same number" 
                        :
                        "حدث خطأ ما، هناك حافلة أخرى بنفس الرقم" }, JsonRequestBehavior.AllowGet);
                }
                /* ------------------------------------------------------------------------------------------------------------- */

                //Check if BusNo is integer value
                if (!SystemBase.RegIsMatch(model.BusNo, "NUMBER"))
                {
                    return Json(new { Success = false, Message = 
                        lang.Contains("en") 
                        ? "Something went wrong, bus number must be an integer"
                        :
                        "حدث خطأ ما، رقم الحافلة يجب أن يكون عددًا صحيحًا" }, JsonRequestBehavior.AllowGet);
                }

                BusInfo Bus = new BusInfo
                {
                    BusNo = model.BusNo,
                    SchoolID = _SchoolID,
                    DriverID = model.DriverID,
                    NumberofSeats = model.NumberOfseats,
                    BusLicenseDate = Convert.ToDateTime(model.BusLicenseDate),
                    Ownership = -1,
                    PurchasePrice = 0,
                    RentValue = 0,
                    BusOwner = string.Empty,
                    ParkingArea = model.ParkingArea,
                    StandbyDay1 = -1,
                    StandbyDay2 = -1,
                    StandbyDay3 = -1,
                    StandbyDay4 = -1,
                    StandbyDay5 = -1,
                    PicnicAllowed = -1,
                    AttendantID = model.AttendantID,
                    SecondAttendantID = model.SecondAttendantID,
                    BusCurrentStatus = 0,
                    PlateNumber = model.PlateNumber

                };
                BusInfoManager.Add(Bus);

                return Json(new { Success = true, Message = 
                    lang.Contains("en")
                ?
                "The save process was successful"
                :
                "تمت عملية الحفظ بنجاح" }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(new { Success = false, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult EditBus(int BusID)
        {
            int _SchoolID = Convert.ToInt32(GetCookie((int)clsenumration.UserData.SchoolID));
            TransportatioModel model = new TransportatioModel();
            PrepareAddBusModel(model);
            var BusManger = factory.CreateBusInfosManager();
            var BusInfo = BusManger.Find(c => c.BusID == BusID && c.SchoolID == _SchoolID).FirstOrDefault();
            model.BusID = BusID;
            model.BusNo = BusInfo.BusNo;
            model.PlateNumber = BusInfo.PlateNumber;
            model.DriverID = BusInfo.DriverID;
            model.AttendantID = BusInfo.AttendantID;
            model.SecondAttendantID = model.SecondAttendantID;
            model.ParkingArea = BusInfo.ParkingArea;
            model.NumberOfseats = BusInfo.NumberofSeats.Value;
            model.BusLicenseDate = BusInfo.BusLicenseDate.Value.ToString("dd-MM-yyyy");
            return View(model);
        }

        [HttpPost]
        public JsonResult EditBus(TransportatioModel model)
        {
            try
            {
                string lang = "";
                if (ViewBag.CurrentLanguage == Languges.English)
                {
                    lang = "en";
                }
                else
                {
                    lang = "ar";
                }


                var BusInfoManager = factory.CreateBusInfosManager();
                var BusManger = factory.CreateBusInfosManager();
                int _SchoolID = Convert.ToInt32(GetCookie((int)clsenumration.UserData.SchoolID));

                //var BusInfoObj = BusManger.Find(c => c.SchoolID == _SchoolID && c.BusID == model.BusID).FirstOrDefault();
                var BusInfoObj = BusManger.Find(c => c.BusID == model.BusID).FirstOrDefault();

                //Check if Bus Number is exist.
                var BusInfo = BusInfoManager.Find(c => c.BusNo.ToString().Trim() == model.BusNo.ToString().Trim() && c.SchoolID == _SchoolID).FirstOrDefault();

                if (BusInfo != null)
                {
                    if (BusInfoObj.BusID != BusInfo.BusID)
                    {
                        if (BusInfo != null && BusInfo.BusNo.Count() > 0)
                        {
                            return Json(new
                            {
                                Success = false,
                                Message =
                                lang.Contains("en")
                                ?
                                "Something went wrong, there is another bus with the same number"
                                :
                                "حدث خطأ ما، هناك حافلة أخرى بنفس الرقم"
                            }, JsonRequestBehavior.AllowGet);
                        }
                    }
                }

                //Check if BusNo is integer value
                if (!SystemBase.RegIsMatch(model.BusNo.ToString(), "NUMBER"))
                {
                    return Json(new
                    {
                        Success = false,
                        Message =
                        lang.Contains("en")
                        ? "Something went wrong, bus number must be an integer"
                        :
                        "حدث خطأ ما، رقم الحافلة يجب أن يكون عددًا صحيحًا"
                    }, JsonRequestBehavior.AllowGet);
                }

                if (BusInfoObj != null)
                {
                    BusInfoObj.BusNo = model.BusNo;
                    BusInfoObj.PlateNumber = model.PlateNumber;
                    BusInfoObj.DriverID = model.DriverID;
                    BusInfoObj.AttendantID = model.AttendantID;
                    BusInfoObj.SecondAttendantID = model.SecondAttendantID;
                    BusInfoObj.NumberofSeats = model.NumberOfseats;
                    BusInfoObj.BusLicenseDate = Convert.ToDateTime(model.BusLicenseDate);
                    BusInfoObj.ParkingArea = model.ParkingArea;
                    BusManger.Update(BusInfoObj);

                    return Json(new
                    {
                        Success = true,
                        Message =
                        lang.Contains("en")
                    ?
                    "The save process was successful"
                    :
                    "تمت عملية الحفظ بنجاح" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new
                    {
                        Success = false,
                        Message =
                        lang.Contains("en")
                    ?
                    "Something is wrong, there is no bus information"
                    :
                    "هناك خطأ ما، لا يوجد معلومات متعلقة بالحافلة" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { Success = false, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult ListBusTours()
        {
            SmartSchoolsEntities entities = new SmartSchoolsEntities();
            var ScheduledBusTripsManager = factory.CreateScheduledBusTripsManager();
            int _SchoolID = Convert.ToInt32(GetCookie((int)clsenumration.UserData.SchoolID));
            string lang = "";
            if (ViewBag.CurrentLanguage == Languges.English)
            {
                lang = "en";
            }
            else
            {
                lang = "ar";
            }
            var query = (from Tour in entities.BusTours
                         where Tour.SchoolID == _SchoolID
                         select new transportationDTO
                         {
                             TourID = Tour.TourID,
                             BusNo = Tour.BusNo,
                             TourName = lang.Contains("en") ? Tour.TourEnglishName : Tour.TourArabicName,
                             //Attendant = lang.Contains("en") ?
                             //entities.Staffs.Where(c => c.StaffID == Tour.TourAttendant).Select(c => c.StaffEnglishName).FirstOrDefault() :
                             //entities.Staffs.Where(c => c.StaffID == Tour.TourAttendant).Select(c => c.StaffArabicName).FirstOrDefault(),
                             //SecondAttendant = lang.Contains("en") ?
                             //entities.Staffs.Where(c => c.StaffID == Tour.SecondAttendant).Select(c => c.StaffEnglishName).FirstOrDefault() :
                             //entities.Staffs.Where(c => c.StaffID == Tour.SecondAttendant).Select(c => c.StaffArabicName).FirstOrDefault(),
                             MaxNoofStudents = Tour.MaxNoofStudents.Value,
                             NumberofStudents = Tour.NumberofStudents.Value,
                             TourDirection = Tour.TourDirection.Value,
                             //TourAreaName = lang.Contains("en") ?
                             //entities.Areas.Where(c => c.AreaID == Tour.TourArea.Value).Select(c => c.AreaNameinEnglish).FirstOrDefault() :
                             //entities.Areas.Where(c => c.AreaID == Tour.TourArea.Value).Select(c => c.AreaNameinArabic).FirstOrDefault(),
                             StratingTime = Tour.StratingTime.Value
                         }).OrderByDescending(c => c.BusNo).ToList();

            foreach (var item in query)
            {
                item.NumberofStudents = ScheduledBusTripsManager.Find(x => x.TripID == item.TourID).ToList().Count();
            }

            return View(query);
        }

        public ActionResult AddBusTour()
        {
            TransportatioModel model = new TransportatioModel();
            PrepareAddTourModel(model);
            return View(model);
        }

        [HttpPost]
        public JsonResult AddBusTour(TransportatioModel model)
        {
            try
            {
                var BusToursManager = factory.CreateBusToursManager();
                int _SchoolID = Convert.ToInt32(GetCookie((int)clsenumration.UserData.SchoolID));

                if (model.TourDirection == 2) //Already stopped
                {
                    // if TourDirection twoway
                    //one for Arrival tour and second for go round Tour 

                    BusTour TourA = new BusTour
                    {
                        SchoolID = _SchoolID,
                        BusNo = model.BusNo,
                        TourArabicName = model.TourNameAr,
                        TourEnglishName = model.TourNameEn,
                        TourArea = model.AreaID,
                        MaxNoofStudents = model.MaxNoofStudents,
                        NumberofStudents = model.NumberofStudents,
                        TourAttendant = model.AttendantID,
                        EstimatedTourTime = 0,
                        EstimatedFuelConsumption = 0,
                        TourDirection = 0, // Onway(Go) tour
                        StratingTime = Convert.ToDateTime(model.StratingTime),
                        StraightDistanceFromSchool = 0,
                        BestDistanceFromSchool = 0,
                        SecondAttendant = model.SecondAttendantID

                    };
                    BusToursManager.Add(TourA);

                    BusTour TourG = new BusTour
                    {
                        SchoolID = _SchoolID,
                        BusNo = model.BusNo,
                        TourArabicName = model.TourNameAr,
                        TourEnglishName = model.TourNameEn,
                        TourArea = model.AreaID,
                        MaxNoofStudents = model.MaxNoofStudents,
                        NumberofStudents = model.NumberofStudents,
                        TourAttendant = model.AttendantID,
                        EstimatedTourTime = 0,
                        EstimatedFuelConsumption = 0,
                        TourDirection = 1,// Onway(return) tour
                        StratingTime = Convert.ToDateTime(model.StratingTime),
                        StraightDistanceFromSchool = 0,
                        BestDistanceFromSchool = 0,
                        SecondAttendant = model.SecondAttendantID

                    };
                    BusToursManager.Add(TourG);

                }
                else
                {
                    BusTour Tour = new BusTour
                    {
                        SchoolID = _SchoolID,
                        BusNo = model.BusNo,
                        TourArabicName = model.TourNameAr,
                        TourEnglishName = model.TourNameEn,
                        TourArea = model.AreaID,
                        MaxNoofStudents = model.MaxNoofStudents,
                        NumberofStudents = model.NumberofStudents,
                        TourAttendant = model.AttendantID,
                        EstimatedTourTime = 0,
                        EstimatedFuelConsumption = 0,
                        TourDirection = model.TourDirection,
                        StratingTime = Convert.ToDateTime(model.StratingTime),
                        StraightDistanceFromSchool = 0,
                        BestDistanceFromSchool = 0,
                        SecondAttendant = model.SecondAttendantID

                    };
                    BusToursManager.Add(Tour);
                }


                return Json(new { Success = true }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(new { Success = false, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult EditTour(int TourID)
        {
            TransportatioModel model = new TransportatioModel();
            SmartSchoolsEntities entities = new SmartSchoolsEntities();
            var ScheduledBusTripsManager = factory.CreateScheduledBusTripsManager();
            var BusToursManger = factory.CreateBusToursManager();
            int _SchoolID = Convert.ToInt32(GetCookie((int)clsenumration.UserData.SchoolID));
            PrepareAddTourModel(model);

            var ScheduledBusTrips = ScheduledBusTripsManager.Find(c => c.TripID == TourID).ToList();
            var BusTourInfo = BusToursManger.Find(c => c.TourID == TourID).FirstOrDefault();
            model.TourID = TourID;
            model.TourNameAr = BusTourInfo.TourArabicName;
            model.TourNameEn = BusTourInfo.TourEnglishName;
            model.AreaID = BusTourInfo.TourArea.Value;
            model.BusNo = BusTourInfo.BusNo;
            model.MaxNoofStudents = BusTourInfo.MaxNoofStudents.Value;
            //model.NumberofStudents = BusTourInfo.NumberofStudents.Value;
            model.NumberofStudents = ScheduledBusTrips.Count();
            model.TourDirection = BusTourInfo.TourDirection.Value;
            model.StratingTime = BusTourInfo.StratingTime.Value.ToString("HH:mm");

            string lang = "";
            if (ViewBag.CurrentLanguage == Languges.English)
            {
                lang = "en";
            }
            else
            {
                lang = "ar";
            }
            var BusInfoes = entities.BusInfoes.Where(c => c.SchoolID == _SchoolID && c.BusNo == BusTourInfo.BusNo).FirstOrDefault();

            model.Attendant = lang.Contains("en") ?
                             entities.Staffs.Where(c => c.StaffID == BusInfoes.AttendantID).Select(c => c.StaffEnglishName).FirstOrDefault() :
                             entities.Staffs.Where(c => c.StaffID == BusInfoes.AttendantID).Select(c => c.StaffArabicName).FirstOrDefault();

            model.SecondAttendant = lang.Contains("en") ?
                     entities.Staffs.Where(c => c.StaffID == BusInfoes.SecondAttendantID).Select(c => c.StaffEnglishName).FirstOrDefault() :
                     entities.Staffs.Where(c => c.StaffID == BusInfoes.SecondAttendantID).Select(c => c.StaffArabicName).FirstOrDefault();
            return View(model);
        }

        [HttpPost]
        public JsonResult EditTour(TransportatioModel model)
        {
            try
            {
                var BusToursManger = factory.CreateBusToursManager();
                int _SchoolID = Convert.ToInt32(GetCookie((int)clsenumration.UserData.SchoolID));

                var BusTourInfo = BusToursManger.Find(c => c.TourID == model.TourID && c.SchoolID == _SchoolID).FirstOrDefault();
                BusTourInfo.TourArabicName = model.TourNameAr;
                BusTourInfo.TourEnglishName = model.TourNameEn;
                BusTourInfo.TourArea = model.AreaID;
                BusTourInfo.BusNo = model.BusNo;
                BusTourInfo.MaxNoofStudents = model.MaxNoofStudents;
                BusTourInfo.NumberofStudents = model.NumberofStudents;
                BusTourInfo.TourDirection = model.TourDirection;
                BusTourInfo.StratingTime = Convert.ToDateTime(model.StratingTime);

                BusToursManger.Update(BusTourInfo);


                return Json(new { Success = true }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(new { Success = false, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult AddStudentToTour()
        {
            TransportatioModel Model = new TransportatioModel();
            SmartSchoolsEntities entities = new SmartSchoolsEntities();
            string lang = "";
            if (ViewBag.CurrentLanguage == Languges.English)
            {
                lang = "en";
            }
            else
            {
                lang = "ar";
            }
            int _SchoolID = Convert.ToInt32(GetCookie((int)clsenumration.UserData.SchoolID));

            var AllStudent = (from SS in entities.Students
                              join TT in entities.StudentSchoolDetails on SS.StudentID equals TT.StudentID
                              where TT.SchoolID == _SchoolID && SS.TransportDirectionID != 3
                              select new StudentDto
                              {
                                  StudentID = SS.StudentID,
                                  StudentName = lang.Contains("en") ? SS.StudentEnglishName : SS.StudentArabicName,
                                  NationalNumber = SS.NationalNumber,
                                  AddressID = SS.AddressID.Value,
                                  TransportationDirection = lang.Contains("en")
                                                            ?
                                                            SS.TransportDirectionID == 0 ? "(Go)" : SS.TransportDirectionID == 1 ? "(Return)" : "(Two Way)"
                                                            :
                                                            SS.TransportDirectionID == 0 ? "(ذهاب)" : SS.TransportDirectionID == 1 ? "(اياب)" : "(ذهاب واياب)",
                              }).OrderBy(c => c.StudentName).ToList();

            /* Exclude the scheduled students "without determine in which tour direction" */
            //var AllStudent = (from SS in entities.Students
            //                  join TT in entities.StudentSchoolDetails on SS.StudentID equals TT.StudentID
            //                  where (TT.SchoolID == _SchoolID && SS.TransportDirectionID != 3)
            //                  && (!entities.ScheduledBusTrips.Any(sb => sb.PassengerID == SS.StudentID))
            //                  select new StudentDto
            //                  {
            //                      StudentID = SS.StudentID,
            //                      StudentName = lang.Contains("en") ? SS.StudentEnglishName : SS.StudentArabicName,
            //                      AddressID = SS.AddressID.Value,
            //                  }).OrderByDescending(c => c.StudentID).ToList();

            //var x = AllStudent.Count();

            var AllTourList = (from T in entities.BusTours
                               join b in entities.BusInfoes on T.BusNo equals b.BusNo
                               where T.SchoolID == _SchoolID && b.SchoolID == _SchoolID
                               select new LookupDTO
                               {
                                   ID = T.TourID,
                                   Description = "Tour:" + T.TourEnglishName.ToString() + "  " + "Direction:" + (T.TourDirection == 0 ? " Going" : "Coming") + "  " + "Bus No:" + b.BusNo,
                                   DescriptionAR = "الجولة:" + " " + T.TourArabicName.ToString() + " " + "اتجاه الجوله:" + "  " + (T.TourDirection == 0 ? "ذهاب" : "إياب") + " " + "رقم الحافلة:" + "  " + b.BusNo,

                               }).OrderByDescending(c => c.ID).ToList();

            //int xxx = AllStudent.Count;
            foreach (var item in AllStudent)
            {
                //string yy = item.StudentID;
                ////item.TransportCategoryTypeID = GetTransportCategoryType(item.AddressID);
                item.TransportCategoryTypeID = 1;
            }

            Model.StudentList = AllStudent;
            Model.TourList = AllTourList;

            return View(Model);
        }

        public JsonResult GetStudentNumberInTour(int TourID)
        {
            SmartSchoolsEntities entities = new SmartSchoolsEntities();
            int _SchoolID = Convert.ToInt32(GetCookie((int)clsenumration.UserData.SchoolID));
            var BusTours = entities.BusTours.Where(c => c.TourID == TourID && c.SchoolID == _SchoolID).FirstOrDefault();

            //return Json(new { Success = true, NumberofStudents = BusTours.MaxNoofStudents }, JsonRequestBehavior.AllowGet);
            return Json(new { Success = true, NumberofStudents = BusTours.MaxNoofStudents }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ISstudentParticipationInstour(string StudentID, int TourID)
        {
            SmartSchoolsEntities entities = new SmartSchoolsEntities();
            int _SchoolID = Convert.ToInt32(GetCookie((int)clsenumration.UserData.SchoolID));
            bool StudentInTour = false;
            var TourDirection = entities.BusTours.Where(c => c.TourID == TourID && c.SchoolID == _SchoolID).FirstOrDefault();

            var info = (from Student in entities.Students
                        where Student.StudentID == StudentID
                        select new StudentDto
                        {
                            OnewayGo = Student.TransportDirectionID.Value == 0 ? true : false,
                            OnewayRetrun = Student.TransportDirectionID.Value == 1 ? true : false,
                            TwoWay = Student.TransportDirectionID.Value == 2 ? true : false,

                        }).FirstOrDefault();

            if (info.OnewayGo && TourDirection.TourDirection == 0)
            {
                StudentInTour = true;
            }
            else if (info.OnewayRetrun && TourDirection.TourDirection == 1)
            {
                StudentInTour = true;
            }
            else if (info.TwoWay && (TourDirection.TourDirection + 1) == 2)
            {
                StudentInTour = true;
            }

            return Json(new { Success = true, InTour = StudentInTour }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult isStudentScheduledInSameDirection(string StudentID, int TourID)
        {
            SmartSchoolsEntities entities = new SmartSchoolsEntities();
            int _SchoolID = Convert.ToInt32(GetCookie((int)clsenumration.UserData.SchoolID));
            bool StudentScheduledInSameDirection = false;
            var TourDirection = entities.BusTours.Where(c => c.TourID == TourID && c.SchoolID == _SchoolID).FirstOrDefault();
            //var BusScheduleInfo = entities.ScheduledBusTrips.Where(x => x.SchoolID == _SchoolID 
            //                                                         && x.PassengerID == StudentID 
            //                                                         && x.TripDate == GetLastSchoolScheduleDate(_SchoolID) 
            //                                                         && (x.Direction == TourDirection.TourDirection || x.Direction == 2)).FirstOrDefault();

            string LastSchoolScheduleDate = GetLastSchoolScheduleDate(_SchoolID);
            var BusScheduleInfo = entities.ScheduledBusTrips.Where(x => x.SchoolID == _SchoolID
                                                                     && x.PassengerID == StudentID
                                                                     && (x.TripDate == LastSchoolScheduleDate)
                                                                     && (x.Direction == TourDirection.TourDirection || x.Direction == 2)).FirstOrDefault();
            if (BusScheduleInfo != null)
            {
                if (TourDirection.TourDirection == BusScheduleInfo.Direction)
                {
                    StudentScheduledInSameDirection = true;
                }
            }

            return Json(new { Success = true, isScheduled = StudentScheduledInSameDirection }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetBusTripInfo(string StudentID, int TourID)
        {
            string lang = "";
            if (ViewBag.CurrentLanguage == Languges.English)
            {
                lang = "en";
            }
            else
            {
                lang = "ar";
            }

            string TripInfo = "";

            SmartSchoolsEntities entities = new SmartSchoolsEntities();
            int _SchoolID = Convert.ToInt32(GetCookie((int)clsenumration.UserData.SchoolID));
            var TourDirection = entities.BusTours.Where(c => c.TourID == TourID && c.SchoolID == _SchoolID).FirstOrDefault();

            string LastSchoolScheduleDate = GetLastSchoolScheduleDate(_SchoolID);
            var BusScheduleInfo = entities.ScheduledBusTrips.Where(x => x.SchoolID == _SchoolID
                                                                     && x.PassengerID == StudentID
                                                                     && (x.TripDate == LastSchoolScheduleDate)
                                                                     && (x.Direction == TourDirection.TourDirection || x.Direction == 2)).FirstOrDefault();
            if (BusScheduleInfo != null)
            {
                if (TourDirection.TourDirection == BusScheduleInfo.Direction)
                {
                    int TripID = int.Parse(BusScheduleInfo.TripID.ToString());
                    var TripInfoDB = entities.BusTours.Where(c => c.TourID == TripID).FirstOrDefault();
                    if (TripInfoDB != null) 
                    {
                        string TransportationDirection = 
                            lang.Contains("en")
                            ?
                            TripInfoDB.TourDirection == 0 ? "(Go)" : TripInfoDB.TourDirection == 1 ? "(Return)" : "(Two Way)"
                            :
                            TripInfoDB.TourDirection == 0 ? "(ذهاب)" : TripInfoDB.TourDirection == 1 ? "(اياب)" : "(ذهاب واياب)";

                        TripInfo =
                            String.Concat(
                                "Tour name: ",
                                lang.Contains("en") ? TripInfoDB.TourEnglishName : TripInfoDB.TourArabicName, 
                                ", Direction: ",
                                TransportationDirection, 
                                ", Bus Number: ", 
                                TripInfoDB.BusNo
                                );
                    }
                }
            }

            return Json(new { Success = true, TripInfo = TripInfo }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult getCurStdsInTour(int TourID)
        {
            SmartSchoolsEntities entities = new SmartSchoolsEntities();
            int _SchoolID = Convert.ToInt32(GetCookie((int)clsenumration.UserData.SchoolID));
            int CurStdsInTour = 0;
            string LastSchoolScheduleDate = GetLastSchoolScheduleDate(_SchoolID);
            var BusScheduleInfo = entities.ScheduledBusTrips.Where(x => x.SchoolID == _SchoolID
                                                                     && x.TripID == TourID
                                                                     && x.TripDate == LastSchoolScheduleDate);
            if (BusScheduleInfo != null)
            {
                CurStdsInTour = BusScheduleInfo.Count();
            }

            return Json(new { Success = true, curStdsInTour = CurStdsInTour }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult deleteStudentFromTour()
        {
            TransportatioModel Model = new TransportatioModel();
            SmartSchoolsEntities entities = new SmartSchoolsEntities();
            string lang = ViewBag.CurrentLanguage == Languges.English ? "en" : "ar";

            int _SchoolID = Convert.ToInt32(GetCookie((int)clsenumration.UserData.SchoolID));
            var AllTourList = (from T in entities.BusTours
                               join b in entities.BusInfoes on T.BusNo equals b.BusNo
                               where T.SchoolID == _SchoolID && b.SchoolID == _SchoolID
                               select new LookupDTO
                               {
                                   ID = T.TourID,
                                   Description = "Tour:" + T.TourEnglishName.ToString() + "  " + "Direction:" + (T.TourDirection == 0 ? " Going" : "Coming") + "  " + "Bus No:" + b.BusNo,
                                   DescriptionAR = "الجولة:" + " " + T.TourArabicName.ToString() + " " + "اتجاه الجوله:" + "  " + (T.TourDirection == 0 ? "ذهاب" : "إياب") + " " + "رقم الحافلة:" + "  " + b.BusNo,

                               }).OrderByDescending(c => c.ID).ToList();
            Model.TourList = AllTourList;
            return View(Model);
        }

        public JsonResult GetStudentInTour(int TourID)
        {
            TransportatioModel Model = new TransportatioModel();
            SmartSchoolsEntities entities = new SmartSchoolsEntities();
            //string lang = "";
            //if (ViewBag.CurrentLanguage == Languges.English)
            //{
            //    lang = "en";
            //}
            //else
            //{
            //    lang = "ar";
            //}

            int _SchoolID = Convert.ToInt32(GetCookie((int)clsenumration.UserData.SchoolID));
            string LastTripDate = GetLastSchoolScheduleDate(_SchoolID);

            var ScheduledStdsInTour = (from T in entities.ScheduledBusTrips
                                       where T.SchoolID == _SchoolID && T.TripID == TourID && T.TripDate == LastTripDate
                                       select new ScheduledBusTrips
                                       {
                                           TourID = TourID,
                                           StudentLat = T.PassengerLatitude.ToString(),
                                           StudentLng = T.PassengerLongitude.ToString(),
                                           SchoolID = _SchoolID,
                                           LastDate = LastTripDate,
                                           StudentID = T.PassengerID,
                                           StudentName = T.PassengerName
                                       }).OrderByDescending(c => c.StudentName).ToList();
            if (ScheduledStdsInTour != null)
            {
                Model.ScheduledStudents = ScheduledStdsInTour;

                //foreach (var s in Model.ScheduledStudents)
                //{
                //    string tourID = s.TourID.ToString();
                //    string stdId = s.StudentID;
                //    string stdName = s.StudentName;
                //}
            }
            return this.Json(JsonConvert.SerializeObject(Model.ScheduledStudents), JsonRequestBehavior.AllowGet);
        }

        public JsonResult ProffStdDeletedFromTour(int SchID, string StdID, string LstDte, int TourID)
        {
            bool isDel = false;
            int _SchoolID = SchID;
            string _StudentID = StdID;
            string _TripDate = LstDte;
            int _TourID = TourID;

            SmartSchoolsEntities db = new SmartSchoolsEntities();
            var scheduledTrips = (
                from SBT in db.ScheduledBusTrips
                where SBT.SchoolID == _SchoolID && SBT.PassengerID == _StudentID && SBT.TripDate == _TripDate && SBT.TripID == TourID
                select SBT).FirstOrDefault();

            if (scheduledTrips != null)
            {
                try
                {
                    string xx = scheduledTrips.PassengerID;

                    db.ScheduledBusTrips.Remove(scheduledTrips);
                    db.SaveChanges();
                    isDel = true;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }

            }

            return Json(new { Success = true, isDeleted = isDel }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SubmitBusTours(List<Students> model, int TourID)
        {
            try
            {
                int _SchoolID = Convert.ToInt32(GetCookie((int)clsenumration.UserData.SchoolID));
                var BusToursManager = factory.CreateBusToursManager();
                var ScheduledBusTripsManger = factory.CreateScheduledBusTripsManager();
                var studentAddress = factory.CreateStudentAdresssManager();
                var BusInfoManager = factory.CreateBusInfosManager();
                var StaffManager = factory.CreateStaffsManager();
                var studentManger = factory.CreateStudentsManager();
                var StaffContactDetails = factory.CreateStaffContactDetailsManager();
                var SchoolBranchManager = factory.CreateSchoolBranchsManager();
                var TourInfo = BusToursManager.Find(c => c.TourID == TourID).FirstOrDefault();
                var Businfo = BusInfoManager.Find(c => c.SchoolID == _SchoolID && c.BusNo == TourInfo.BusNo).FirstOrDefault();
                var StaffInfo = StaffManager.Find(c => c.StaffID == Businfo.DriverID).FirstOrDefault();
                var SchoolAddress = SchoolBranchManager.Find(a => a.SchoolID == _SchoolID).FirstOrDefault();
                var _schoolyear = getCurrentAcademicYear();
                string lang = "";
                if (ViewBag.CurrentLanguage == Languges.English)
                {
                    lang = "en";
                }
                else
                {
                    lang = "ar";
                }
                if (model != null)
                {
                    string _TripDate = GetLastSchoolScheduleDate(_SchoolID);
                    foreach (var s in model)
                    {
                        var stduent = studentManger.Find(c => c.StudentID == s.StudentID).FirstOrDefault();
                        var Address = studentAddress.Find(c => c.AddressID == stduent.AddressID).FirstOrDefault();
                        ScheduledBusTrip t = new ScheduledBusTrip
                        {
                            SchoolID = _SchoolID,
                            SchoolYear = _schoolyear,
                            MobileNumber = SchoolAddress.SchoolContactNumber,
                            SchoolLongitude = SchoolAddress.Longitude.Value,
                            SchoolLatitude = SchoolAddress.Latitude.Value,
                            Direction = TourInfo.TourDirection,
                            TripDate = _TripDate,
                            TripTime = TourInfo.StratingTime.Value.ToString("HH:mm"),
                            BusNumber = TourInfo.BusNo,
                            DriverID = Businfo.DriverID,
                            DriverName = lang.Contains("ar") ? StaffInfo.StaffArabicName : StaffInfo.StaffEnglishName,
                            AttendantID = Businfo.AttendantID,
                            PassengerID = s.StudentID,
                            PassengerType = 0,
                            PassengerName = lang.Contains("ar") ? stduent.StudentArabicName : stduent.StudentEnglishName,
                            PassengerAgeFactor = 0,
                            PassengerLongitude = Address.Longitude,
                            PassengerLatitude = Address.Latitude,
                            PassengerOnBoard = 0,
                            PassengeroffBoard = 0,
                            AttendantLatitude = StaffContactDetails.Find(c => c.StaffID == Businfo.AttendantID).Select(c => c.Latitude).FirstOrDefault(),
                            AttendantLongitude = StaffContactDetails.Find(c => c.StaffID == Businfo.AttendantID).Select(c => c.Longitude).FirstOrDefault(),
                            AttendantName = lang.Contains("ar") ? StaffManager.Find(c => c.StaffID == Businfo.AttendantID).Select(c => c.StaffArabicName).FirstOrDefault() :
                            StaffManager.Find(c => c.StaffID == Businfo.AttendantID).Select(c => c.StaffEnglishName).FirstOrDefault(),
                            DriverMobileNo = StaffContactDetails.Find(c => c.StaffID == Businfo.DriverID).Select(c => c.MobileNo).FirstOrDefault(),
                            AttendantMobileNo = StaffContactDetails.Find(c => c.StaffID == Businfo.AttendantID).Select(c => c.MobileNo).FirstOrDefault(),
                            TripID = TourID,
                            TripEnded = 0,
                            AttendanceStatus = 0,
                            PickupOrder = 0,
                            SecondAttendantID = Businfo.SecondAttendantID,
                            SecondAttendantLongitude = StaffContactDetails.Find(c => c.StaffID == Businfo.SecondAttendantID).Select(c => c.Longitude).FirstOrDefault(),
                            SecondAttendantLatitude = StaffContactDetails.Find(c => c.StaffID == Businfo.SecondAttendantID).Select(c => c.Latitude).FirstOrDefault(),
                            SecondAttendantName = lang.Contains("ar") ? StaffManager.Find(c => c.StaffID == Businfo.SecondAttendantID).Select(c => c.StaffArabicName).FirstOrDefault() :
                            StaffManager.Find(c => c.StaffID == Businfo.SecondAttendantID).Select(c => c.StaffEnglishName).FirstOrDefault(),
                            SecondAttendantMobileNo = StaffContactDetails.Find(c => c.StaffID == Businfo.SecondAttendantID).Select(c => c.MobileNo).FirstOrDefault()
                        };
                        ScheduledBusTripsManger.Add(t);
                    }
                }
                else
                {
                    return Json(new { Success = false }, JsonRequestBehavior.AllowGet);

                }

                return Json(new { Success = true }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(new { Success = false, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult ScheduledBusTrips()
        {
            string lang = "";
            if (ViewBag.CurrentLanguage == Languges.English)
            {
                lang = "en";
            }
            else
            {
                lang = "ar";
            }

            int SchoolID = Convert.ToInt32(GetCookie((int)clsenumration.UserData.SchoolID));
            SmartSchoolsEntities entities = new SmartSchoolsEntities();
            var query = (from T in entities.BusTours
                             //join A in entities.Areas on T.TourArea equals A.AreaID
                         where T.SchoolID == SchoolID
                         select new TourDto
                         {
                             BusNo = T.BusNo,
                             TourID = T.TourID,
                             TourName = lang.Contains("ar") ? T.TourArabicName : T.TourEnglishName,
                             //AreaName = lang.Contains("ar") ? A.AreaNameinArabic : A.AreaNameinEnglish,
                             MaxNumberOfStudents = (int)T.MaxNoofStudents,
                             StudentsinTourCount = entities.ScheduledBusTrips.Count(z => z.SchoolID == SchoolID && z.TripID == T.TourID),
                             TourDirection = T.TourDirection.Value
                         }).OrderByDescending(c => c.TourID).ToList();

            return View(query);
        }

        public JsonResult GetTourMAPInfo(string BusNo, int TourID)
        {
            TourDto tourWayPointsInfo = new TourDto();
            SmartSchoolsEntities db = new SmartSchoolsEntities();
            int _SchoolID = Convert.ToInt32(GetCookie((int)clsenumration.UserData.SchoolID));
            string LastTripDate = GetLastSchoolScheduleDate(_SchoolID);

            var tourRoutInfo = (
                    from STB in db.ScheduledBusTrips
                    where STB.SchoolID == _SchoolID && STB.TripID == TourID && STB.TripDate == LastTripDate && STB.BusNumber == BusNo
                    select new TourWayPoint
                    {
                        SchoolLat = (double)STB.SchoolLatitude,
                        SchoolLng = (double)STB.SchoolLongitude,
                        AttendantLat = (double)STB.AttendantLatitude,
                        AttendantLng = (double)STB.AttendantLongitude,
                        PassengerLat = (double)STB.PassengerLatitude,
                        PassengerLng = (double)STB.PassengerLongitude
                    }
                ).ToList();

            if (tourRoutInfo != null)
            {
                tourWayPointsInfo.TourWayPoints = tourRoutInfo;
            }

            return this.Json(JsonConvert.SerializeObject(tourWayPointsInfo.TourWayPoints), JsonRequestBehavior.AllowGet);
        }

        public ActionResult StudentByBusNoModal(string BusNo, int TourID)
        {
            SmartSchoolsEntities entities = new SmartSchoolsEntities();
            string lang = ViewBag.CurrentLanguage == Languges.English ? "en" : "ar";

            int _SchoolID = Convert.ToInt32(GetCookie((int)clsenumration.UserData.SchoolID));
            string LastTripDate = GetLastSchoolScheduleDate(_SchoolID);
            //var _SchoolYear = getCurrentAcademicYear();
            var ScheduledBusTrips = (from S in entities.ScheduledBusTrips
                                     where S.BusNumber == BusNo
                                     where S.SchoolID == _SchoolID && S.TripDate == LastTripDate && S.TripID == TourID
                                     select new ScheduledBus
                                     {
                                         PassengerID = S.PassengerID,
                                         PassengerName = S.PassengerName
                                     }).ToList();

            //foreach (var item in ScheduledBusTrips)
            //{
            //    var student = entities.Students.Where(c => c.StudentID == item.PassengerID).FirstOrDefault();
            //    if (student !=null)
            //    {
            //       item.TransportCategoryType = GetTransportCategoryType(student.AddressID.Value);
            //        item.TransportCategoryName = lang.Contains("ar") ? entities.TransportCategoryTypes.Where(c => c.TransportCategoryTypeID == item.TransportCategoryType).Select(c => c.TransportCategoryArabicName).FirstOrDefault() :
            //              entities.TransportCategoryTypes.Where(c => c.TransportCategoryTypeID == item.TransportCategoryType).Select(c => c.TransportCategoryEnglishName).FirstOrDefault();

            //    }
            //}
            return View(ScheduledBusTrips);
        }

        public ActionResult TourInformationModal(string BusNo, int TourID)
        {
            SmartSchoolsEntities entities = new SmartSchoolsEntities();
            string lang = ViewBag.CurrentLanguage == Languges.English ? "en" : "ar";

            int _SchoolID = Convert.ToInt32(GetCookie((int)clsenumration.UserData.SchoolID));
            string LastTripDate = GetLastSchoolScheduleDate(_SchoolID);
            //var _SchoolYear = getCurrentAcademicYear();
            var ScheduledBusTrips = (from S in entities.ScheduledBusTrips
                                     where S.BusNumber == BusNo
                                     where S.SchoolID == _SchoolID && S.TripDate == LastTripDate && S.TripID == TourID
                                     select new ScheduledBus
                                     {
                                         PassengerID = S.PassengerID,
                                         PassengerName = S.PassengerName,
                                         StudentLat = (double)S.PassengerLatitude,
                                         StudentLng = (double)S.PassengerLongitude
                                     }).ToList();

            return View(ScheduledBusTrips);
        }

        public int GetTransportCategoryType(int AddressID)
        {
            TransportMAPs model = new TransportMAPs();
            var StudentAddressManager = factory.CreateStudentAdresssManager();
            var SchoolBranchManager = factory.CreateSchoolBranchsManager();
            var TransportCategories = factory.CreateTransportCategorysManager();
            var CategoryTypesManager = factory.CreateTransportCategoryTypesManager();
            int SchoolID = Convert.ToInt32(GetCookie((int)clsenumration.UserData.SchoolID));
            var StudentAddress = StudentAddressManager.Find(a => a.AddressID == AddressID).FirstOrDefault();
            var SchoolAddress = SchoolBranchManager.Find(a => a.SchoolID == SchoolID).FirstOrDefault();

            if (StudentAddress != null)
            {
                model.StudentLatitude = StudentAddress.Latitude.Value;
                model.StudentLongitude = StudentAddress.Longitude.Value;
            }
            else
            {
                model.StudentLatitude = 0;
                model.StudentLongitude = 0;
            }

            if (SchoolAddress != null)
            {
                model.SChoolLatitude = SchoolAddress.Latitude.Value;
                model.SchoolLongitude = SchoolAddress.Longitude.Value;
            }
            else
            {
                model.SChoolLatitude = 0;
                model.SchoolLongitude = 0;
            }

            //model.SChoolLatitude = SchoolAddress.Latitude.Value;
            //model.SchoolLongitude = SchoolAddress.Longitude.Value;
            //model.StudentLatitude = StudentAddress.Latitude.Value;
            //model.StudentLongitude = StudentAddress.Longitude.Value;
            /************/

            var CategoriesDistances = TransportCategories.Find(a => a.SchoolID == SchoolID).ToList();
            string Origins = model.SChoolLatitude.ToString() + ',' + model.SchoolLongitude.ToString();
            string Distination = model.StudentLatitude.ToString() + ',' + model.StudentLongitude.ToString();
            int TransportCategoryTypeID = 5;

            double DistanceM = 0;
            double DistanceKM = 0;
            try
            {

                //travelMode = Driving, Walking, Bicycling, Transit.
                //string jsonData = DistanceMatrixRequest("31.9604899962494,35.848434140599", "31.9769664,35.8585633", "Driving", "<%=ConfigurationManager.AppSettings["GoogleAPIKey"] %>");
                string jsonData = DistanceMatrixRequest(Origins, Distination, "Driving", ConfigurationManager.AppSettings["GoogleAPIKey"]);
                JObject o = JObject.Parse(jsonData);

                if ((string)o.SelectToken("rows[0].elements[0].status") == "OK")
                {
                    DistanceM = (double)o.SelectToken("rows[0].elements[0].distance.value");
                }
                else
                {
                    DistanceM = 0;
                }

                DistanceKM = DistanceM / 1000;
                DistanceKM = Math.Round(DistanceKM, 1);
                if (CategoriesDistances.Count > 0)
                {
                    foreach (var item in CategoriesDistances)
                    {
                        if (DistanceM <= item.DistanceInMeters)
                        {
                            var Category = CategoryTypesManager.Find(a => a.TransportCategoryTypeID == item.TransportCategoryTypeID).FirstOrDefault();
                            TransportCategoryTypeID = CurrentLanguage == Languges.English ? Category.TransportCategoryTypeID : Category.TransportCategoryTypeID;
                            model.Category = Convert.ToString(TransportCategoryTypeID);
                            break;
                        }

                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(model.Category))
                    {
                        TransportCategoryTypeID = 5; //"Category Five" : "الفئة الخامسة";

                    }
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
            return TransportCategoryTypeID;
        }

        public JsonResult BusInfoDetails(string BusNo)
        {
            SmartSchoolsEntities entities = new SmartSchoolsEntities();
            int _SchoolID = Convert.ToInt32(GetCookie((int)clsenumration.UserData.SchoolID));
            string lang = "";
            if (ViewBag.CurrentLanguage == Languges.English)
            {
                lang = "en";
            }
            else
            {
                lang = "ar";
            }
            var query = (from Bus in entities.BusInfoes
                         where Bus.SchoolID == _SchoolID && Bus.BusNo == BusNo
                         select new transportationDTO
                         {
                             Attendant = lang.Contains("en") ?
                             entities.Staffs.Where(c => c.StaffID == Bus.AttendantID).Select(c => c.StaffEnglishName).FirstOrDefault() :
                             entities.Staffs.Where(c => c.StaffID == Bus.AttendantID).Select(c => c.StaffArabicName).FirstOrDefault(),
                             AttendantID = Bus.AttendantID,
                             SecondAttendantID = Bus.SecondAttendantID,
                             SecondAttendant = lang.Contains("en") ?
                             entities.Staffs.Where(c => c.StaffID == Bus.SecondAttendantID).Select(c => c.StaffEnglishName).FirstOrDefault() :
                             entities.Staffs.Where(c => c.StaffID == Bus.SecondAttendantID).Select(c => c.StaffArabicName).FirstOrDefault(),
                             NumberOfseats = Bus.NumberofSeats.Value,
                         }).FirstOrDefault();

            return Json(new { Success = true, data = query }, JsonRequestBehavior.AllowGet);

        }

        #region Methods 
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

        private void PrepareAddBusModel(TransportatioModel model)
        {
            string lang = "";
            if (ViewBag.CurrentLanguage == Languges.English)
            {
                lang = "en";
            }
            else
            {
                lang = "ar";
            }
            int SchoolID = Convert.ToInt32(GetCookie((int)clsenumration.UserData.SchoolID));
            //string schoolyear = getCurrentAcademicYear();
            var StaffManager = factory.CreateStaffsManager();
            var SchoolBranchsManager = factory.CreateSchoolBranchsManager();
            var SchoolBranch = SchoolBranchsManager.GetAll();
            var SchoolBranchList = (from School in SchoolBranch
                                    select new LookupDTO
                                    {
                                        Description = School.SchoolEnglishName.ToString(),
                                        DescriptionAR = School.SchoolArabicName.ToString(),
                                        ID = (School.SchoolID)
                                    }).ToList();
            model.schoolList = SchoolBranchList;
            model.schoolID = SchoolID;
            var AllDrivers = StaffManager.GetDrivers(SchoolID, lang);
            var DriversList = (from D in AllDrivers
                               select new LookupDTO
                               {
                                   Description = D.EmployeeName.ToString(),
                                   DescriptionAR = D.EmployeeName.ToString(),
                                   id = (D.StaffID)
                               }).ToList();
            model.DriverList = DriversList;
            var AllEscort = StaffManager.GetEscort(SchoolID, lang);

            var EscortList = (from Escort in AllEscort
                              select new LookupDTO
                              {
                                  Description = Escort.EmployeeName.ToString(),
                                  DescriptionAR = Escort.EmployeeName.ToString(),
                                  id = Escort.StaffID
                              }).ToList();

            model.EscortList = EscortList;

        }

        private void PrepareAddTourModel(TransportatioModel model)
        {
            string lang = ViewBag.CurrentLanguage == Languges.English ? "en" : "ar";

            int SchoolID = Convert.ToInt32(GetCookie((int)clsenumration.UserData.SchoolID));
            var BusManger = factory.CreateBusInfosManager();
            var AllBus = BusManger.Find(x => x.SchoolID == SchoolID).ToList();
            //var AllBus = BusManger.GetAll();
            var BusList = (from Bus in AllBus
                           select new LookupDTO
                           {
                               Description = "Bus No:" + Bus.BusNo.ToString() + "  " + "Plate Number:" + Bus.PlateNumber,
                               DescriptionAR = "رقم الحافلة:" + " " + Bus.BusNo.ToString() + " " + "الرقم الجانبي:" + "  " + Bus.PlateNumber,
                               id = Bus.BusNo
                           }).ToList();

            model.BusList = BusList;

            //var AreaManger = factory.CreateAreasManager();
            //var AllAreas = AreaManger.GetAll();
            //var AreasList = (from Area in AllAreas
            //                 select new LookupDTO
            //                 {
            //                     Description = Area.AreaNameinEnglish.ToString(),
            //                     DescriptionAR = Area.AreaNameinArabic.ToString(),
            //                     ID = Area.AreaID
            //                 }).ToList();

            //model.TourAreaList = AreasList;
            //model.StratingTime = DateTime.Now.ToString("HH:mm");

        }

        #endregion 

        private string GetLastSchoolScheduleDate(int SchoolID)
        {
            string LastDate = DateTime.Now.ToString();
            DataTable LastSchoolScheduleDateInfo =
                SystemBase.GetDataTble("SELECT TOP 1 TripPassengerID, TripDate " +
                                       "FROM ScheduledBusTrips " +
                                       "WHERE SchoolID = '" + SchoolID + "' " +
                                       "ORDER BY TripPassengerID DESC");
            if (LastSchoolScheduleDateInfo != null)
            {
                if (LastSchoolScheduleDateInfo.Rows.Count > 0)
                {
                    LastDate = LastSchoolScheduleDateInfo.Rows[0]["TripDate"].ToString();
                }
            }

            LastDate = Convert.ToDateTime(LastDate).ToString("dd/MM/yyyy");
            return LastDate;
        }
    }
}