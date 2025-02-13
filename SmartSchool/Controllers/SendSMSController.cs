using Business.Base;
using Common;
using Common.Helpers;
using DataAccess;
using Objects;
using SmartSchool.Models.SMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace SmartSchool.Controllers
{
    public class SendSMSController : BaseController
    {
        BusinessComponentsFactory factory = new BusinessComponentsFactory();

        // GET: SendSMS
        public ActionResult Index()
        {
            int SchoolID = Convert.ToInt32(GetCookie((int)clsenumration.UserData.SchoolID));
            SmsViewModel model = new SmsViewModel();
            var GuardiansManager = factory.CreateGuardiansManager();
            var Guardians = GuardiansManager.GetAll().ToList();

            var ClassesManger = factory.CreateSchoolClasssManager();
            var Classes = ClassesManger.Find(c => c.SchoolID == SchoolID).ToList();
            model.SchoolClassList = (from c in Classes
                                     select new LookupDTO
                                     {
                                         Description = c.SchoolClassEnglishName,
                                         DescriptionAR = c.SchoolClassArabicName,
                                         ID = c.SchoolClassID
                                     }).ToList();

            var MailTemplateManager = factory.CreateMailTemplatesManager();
            var MailTemplates = MailTemplateManager.GetAll().ToList();

            string lang = "";
            if (ViewBag.CurrentLanguage == Languges.English)
            {
                lang = "en";
            }
            else
            {
                lang = "ar";
            }
            if (MailTemplates.Count == 0)
            {
                var Template = new MailTemplate()
                {
                    TemplateNameEn = "Send Password",
                    TemplateNameAr = "إرسال كلمة المرور",
                    Template = lang == "en" ? "Dear,&#8810StudentName&#8811 your  <br>  Username:&#8810Username&#8811 <br> Password:&#8810Password&#8811 <br>" : "عزيزي,&#8810StudentName&#8811 <br> اسم المستخدم:&#8810Username&#8811 <br> كلمه المرور:&#8810Password&#8811 <br>"


                };
                MailTemplateManager.Add(Template);
            }

            model.MailTemplateList = (from c in MailTemplates
                                      select new LookupDTO
                                      {
                                          Description = c.TemplateNameEn.ToString(),
                                          DescriptionAR = c.TemplateNameAr.ToString(),
                                          ID = (c.TemplateID)
                                      }).ToList();
            return View(model);
        }


        public ActionResult Getrecipient(int ClassID, int SectionID)
        {
            var Recipient = new List<Recipient>();

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

                Recipient = (from S in entities.Students
                             join G in entities.Guardians
                             on S.GuardianID equals G.GuardianID
                             join D in entities.StudentSchoolDetails
                             on S.StudentID equals D.StudentID
                             where D.SchoolID == SchoolID && (ClassID != 0 ? D.ClassID == ClassID : 1 == 1)
                             && (SectionID != 0 ? D.SectionID == SectionID : 1 == 1)
                             select new Recipient
                             {
                                 GuardianID = G.GuardianID,
                                 GuardianName = lang == "ar" ? G.GuardianArabicName : G.GuardianEnglishName,
                                 MobileNumber = G.MobileNumber
                             }).Distinct().ToList();

            }
            return View(Recipient);
        }


        [HttpGet]
        public JsonResult GetTemplate(int TemplateID)
        {

            try
            {
                var MailTemplateManager = factory.CreateMailTemplatesManager();
                var MailTemplates = MailTemplateManager.Find(c => c.TemplateID == TemplateID).FirstOrDefault();


                return Json(new { Success = true, MailTemplates }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(new { Success = false, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }



        //[HttpPost]
        //public JsonResult SendSmsToParent(string Body, List<string> MobileNumbers, string TemplateName)
        //{
        //    try
        //    {

        //        if (TemplateName == "Send Password" || TemplateName == "إرسال كلمة المرور")
        //        {
        //            var StudentsManager = factory.CreateStudentsManager();
        //            var GuardiansManager = factory.CreateGuardiansManager();
        //            var Student_LoginsManager = factory.CreateStudent_LoginsManager();
        //            string lang = "";
        //            if (ViewBag.CurrentLanguage == Languges.English)
        //            {
        //                lang = "en";
        //            }
        //            else
        //            {
        //                lang = "ar";
        //            }
        //            foreach (var num in MobileNumbers)
        //            {
        //                var GuardianID = GuardiansManager.Find(c => c.MobileNumber == num).Select(c=>c.GuardianID).FirstOrDefault();
        //                var Student = StudentsManager.Find(c => c.GuardianID == GuardianID).ToList();

        //                foreach (var s in Student)
        //                {
        //                    var StudentCredentials = Student_LoginsManager.Find(c => c.StudentID == s.StudentID).FirstOrDefault();
        //                    var StudentName = lang == "en" ? s.StudentEnglishName : s.StudentArabicName;
        //                    string _body = Body.Replace("≪StudentName≫", StudentName).Replace("≪Username≫", StudentCredentials.Username).Replace("≪Password≫", Decrypt(StudentCredentials.Password));
        //                    var res = SendSMS(_body, num);
        //                    if (res.IsSuccessStatusCode)
        //                    {
        //                        return Json(new { Success = true }, JsonRequestBehavior.AllowGet);

        //                    }
        //                    else
        //                    {
        //                        return Json(new { Success = false }, JsonRequestBehavior.AllowGet);

        //                    }
        //                }


        //            }
        //        }
        //        else
        //        {
        //            //max arabic message 62 bulk and 70  chartaer by one msg, max english message 160 chartaer by one msg  bulk 152
        //            var response = SendBulkSMS(Body, MobileNumbers);

        //            if (response.IsSuccessStatusCode)
        //            {
        //                return Json(new { Success = true }, JsonRequestBehavior.AllowGet);

        //            }
        //            else
        //            {
        //                return Json(new { Success = false }, JsonRequestBehavior.AllowGet);

        //            }
        //        }


        //        return Json(new { Success = false }, JsonRequestBehavior.AllowGet);

        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { Success = false }, JsonRequestBehavior.AllowGet);
        //    }
        //}

        [HttpPost]
        public JsonResult SendSmsToParent(string Body, List<string> MobileNumbers, string TemplateName)
        {
            try
            {
                var StudentsManager = factory.CreateStudentsManager();
                var GuardiansManager = factory.CreateGuardiansManager();
                var Student_LoginsManager = factory.CreateStudent_LoginsManager();
                string lang = ViewBag.CurrentLanguage == Languges.English ? "en" : "ar";

                if (TemplateName == "Send Password" || TemplateName == "إرسال كلمة المرور")
                {
                    foreach (var num in MobileNumbers)
                    {
                        var GuardianID = GuardiansManager.Find(c => c.MobileNumber == num).Select(c => c.GuardianID).FirstOrDefault();
                        var Student = StudentsManager.Find(c => c.GuardianID == GuardianID).ToList();

                        foreach (var s in Student)
                        {
                            var StudentCredentials = Student_LoginsManager.Find(c => c.StudentID == s.StudentID).FirstOrDefault();
                            var StudentName = lang == "en" ? s.StudentEnglishName : s.StudentArabicName;
                            if (StudentCredentials != null)
                            {
                                string _body = Body.Replace("≪StudentName≫", StudentName).Replace("≪Username≫", StudentCredentials.Username).Replace("≪Password≫", Decrypt(StudentCredentials.Password));

                                var res = SendSMS(_body, num, lang[0], num.StartsWith("20") ? "20" : "962"); // lang[0] : takeing the first latter from the current bag language to pass it in case of Egypt number.

                                if (!res.IsSuccessStatusCode)
                                    return Json(new { Success = false, Message = "Some thing went Wrong" }, JsonRequestBehavior.AllowGet);
                            }
                            else
                            {
                                //return Json(new { Success = false, Message = StudentName + " don't have login account" }, JsonRequestBehavior.AllowGet);
                            }
                        }
                    }
                    return Json(new { Success = true }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    //max arabic message 62 bulk and 70  chartaer by one msg, max english message 160 chartaer by one msg  bulk 152
                    var response = SendBulkSMS(Body, MobileNumbers);

                    if (response.IsSuccessStatusCode)
                        return Json(new { Success = true }, JsonRequestBehavior.AllowGet);
                    else if (response.StatusCode == HttpStatusCode.Gone)
                    {
                        foreach (var num in MobileNumbers)
                        {
                            var res = SendSMS(Body, num, lang[0], num.StartsWith("20") ? "20" : "962"); // lang[0] : takeing the first latter from the current bag language to pass it in case of Egypt number.

                            if (!res.IsSuccessStatusCode)
                                return Json(new { Success = false, Message = "Some thing went Wrong" }, JsonRequestBehavior.AllowGet);
                        }
                        return Json(new { Success = true }, JsonRequestBehavior.AllowGet);
                    }
                    else
                        return Json(new { Success = false, Message = "Some thing went Wrong" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { Success = false, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public JsonResult AddMailTemplate(string TemplateNameAr, string TemplateNameEn, string Template)
        {
            try
            {
                var MailTemplate = factory.CreateMailTemplatesManager();

                MailTemplate obj = new MailTemplate
                {
                    TemplateNameAr = TemplateNameAr,
                    TemplateNameEn = TemplateNameEn,
                    Template = Template

                };
                MailTemplate.Add(obj);

                return Json(new { Success = true, data = obj }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(new { Success = false, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}