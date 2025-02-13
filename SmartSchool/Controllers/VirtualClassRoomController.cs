using Business.Base;
using Common;
using Common.Base;
using Common.Helpers;
using DataAccess;
using SmartSchool.Models.VirtualClassRoom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace SmartSchool.Controllers
{
    public class VirtualClassRoomController : TeacherBaseController
    {
        SmartSchoolsEntities context = new SmartSchoolsEntities();
        BusinessComponentsFactory factory = new BusinessComponentsFactory();

        // GET: VirtualClassRoom
        public ActionResult Index()
        {
            var MSTeamsAccountManager = factory.CreateMSTeams_AccountssManager();
            int SchoolID = Convert.ToInt32(GetCookie((int)clsenumration.UserData.SchoolID));

            var account = MSTeamsAccountManager.Find(a => a.IsDeleted == false).FirstOrDefault();
            VirtualClassRoom model = new VirtualClassRoom();
            model.Classes = GetSchoolClassesBySchoolID(SchoolID);
            model.MsClientID = account.ClientID;
            model.MsTenantID = account.TenantID;
            return View(model);

        }

        public ActionResult Getrecipient(int ClassID, int SectionID)
        {
            var Recipient = new List<RecipientStudent>();
            int SchoolID = Convert.ToInt32(GetCookie((int)clsenumration.UserData.SchoolID));

            using (SmartSchoolsEntities entities = new SmartSchoolsEntities())
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

                Recipient = (from a in context.Students
                             join b in context.StudentSchoolDetails on a.StudentID equals b.StudentID
                             where b.ClassID == ClassID
                             && b.SectionID == SectionID && b.SchoolID == SchoolID
                             select new RecipientStudent()
                             {
                                 StudentID = a.StudentID,
                                 StudentName = lang == "ar" ? a.StudentArabicName : a.StudentEnglishName
                             }).Distinct().ToList();

            }
            return View(Recipient);
        }

        [HttpPost]
        public JsonResult GetSections(int SchoolClassID)
        {
            var Sections = GetSectionsBySchoolClassID(SchoolClassID);
            var Subjects = GetSubjectsBySchoolClassID(SchoolClassID);

            return Json(new { Sections = Sections, Subjects = Subjects }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult CreateTeamsMeeting(List<MeetingAttendees> Attendees, VirtualClassRoom virtualMeeting, bool smschecked)
        {

            var startdate = DateTime.Now;
            var enddate = DateTime.Now.AddHours(1);
            string errorMessage = "";
            var AccessToken = virtualMeeting.hfAccessToken;
            var meeting = CreateMSTeamsMeeting(virtualMeeting.MeetingTitle, AccessToken, Attendees, startdate, enddate, out errorMessage, 0);

            if (meeting != null)
            {
                virtualMeeting.hfSessionID = meeting.MSTeamSessionID.ToString();
                virtualMeeting.MeetingJoinURL = meeting.JoinURL.ToString();
                string lessonTitle = virtualMeeting.MeetingTitle;
                var Date = DateTime.Now;
                BusinessComponentsFactory _factory = new BusinessComponentsFactory();
                List<string> StudentsLoginName = new List<string>();
                try
                {


                    BusinessComponentsFactory factory = new BusinessComponentsFactory();
                    var studentmanager = factory.CreateStudentsManager();
                    var Guardianmanager = factory.CreateGuardiansManager();

                    //Message = string.Format("Dear Student ({0}) you are invited to ({1} ) virtual class, you can access it from this link ( {2} )", "natour", virtualMeeting.MeetingTitle, meeting.JoinURL.ToString());
                    //SendSMS(Message, "0796351592");
                    //if (smschecked)
                    //{
                    //    foreach (var item in Attendees)
                    //    {
                    //        string studentname = "";
                    //        string GurdianMobileNumber = "";
                    //        int GuardianID = 0;
                    //        studentname = studentmanager.Find(a => a.StudentID == item.ID).Select(a => a.StudentArabicName).FirstOrDefault();
                    //        GuardianID = studentmanager.Find(a => a.StudentID == item.ID).Select(a => a.GuardianID.Value).FirstOrDefault();
                    //        GurdianMobileNumber = Guardianmanager.Find(a => a.GuardianID == GuardianID).Select(a => a.MobileNumber).FirstOrDefault();
                    //        Message = string.Format("Dear Student ({0}) you are invited to ({1} ) virtual class, you can access it from this link ( {2} )", studentname, virtualMeeting.MeetingTitle, meeting.JoinURL.ToString());
                    //        SendSMS(Message, GurdianMobileNumber);

                    //    }
                    //}

                    return Json(new { ErrorMsg = "", JoinURL = meeting.JoinURL }, JsonRequestBehavior.AllowGet);

                }
                catch (Exception)
                {
                    return Json(new { ErrorMsg = "Something wrong !" }, JsonRequestBehavior.AllowGet);

                }
            }
            else
            {
                //Error
                return Json(new { ErrorMsg = "Something wrong !" }, JsonRequestBehavior.AllowGet);



            }
        }
    }
}







