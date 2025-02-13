using Business.Base;
using Common.Helpers;
using Objects;
using RestSharp;
using System;
using System.Collections.Generic;

namespace Common.Base
{
    public class TeacherBaseController : BaseController
    {
        public MSTeams_Sessions CreateMSTeamsMeeting(string meetingTitle, string AccessToken, List<MeetingAttendees> attendees, DateTime fromDate, DateTime toDate, out string msg, int sectionId = 0, bool AddToCalender = false)
        {
            string StaffID = GetCookie((int)clsenumration.UserData.StaffID);
            int SchoolID = Convert.ToInt32(GetCookie((int)clsenumration.UserData.SchoolID));

            BusinessComponentsFactory factory = new BusinessComponentsFactory();

            var obj = new MSTeams
            {
                startDateTime = fromDate.ToUniversalTime().ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'"),//  "2019-07-12T14:30:34.2444915-07:00",
                endDateTime = toDate.ToUniversalTime().ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'"),//"2019-07-12T15:00:34.2464912-07:00",
                subject = meetingTitle,
            };

            var request = new RestRequest("onlineMeetings", Method.POST);
            request.AddHeader("Authorization", "Bearer " + AccessToken);
            request.AddJsonBody(obj);

            RestSharp.RestClient client = new RestClient("https://graph.microsoft.com/v1.0/me/");
            var data = client.Post<MicrosoftTeams.MSTeamsMeetingData>(request);
            var meeting = data.Data;

            if (data.StatusCode == System.Net.HttpStatusCode.Created && meeting != null)
            {
                var sessionObj = new MSTeams_Sessions();

                sessionObj.CreationDate = DateTime.Now;
                sessionObj.StartTime = fromDate;
                sessionObj.EndTime = toDate;
                sessionObj.SectionID = sectionId;
                sessionObj.StaffID = StaffID;
                sessionObj.MSTeamsAccountID = 1;
                sessionObj.JoinURL = meeting.JoinUrl.ToString();
                sessionObj.JoinWebURL = meeting.JoinWebUrl.ToString();
                sessionObj.MeetingTitle = meetingTitle;
                sessionObj.IsDeleted = false;
                sessionObj.StatusID = 1;


                var TeamsManager = factory.CreateMSTeams_SessionssManager();
                var atenddeessManager = factory.CreateMSTeams_SessionAttendessManager();
                var notificationssManager = factory.CreateNotificationsWebsManager();
                var onlineMeetingssManager = factory.CreateVirtualMeetingsManager();

                TeamsManager.Add(sessionObj);

                var virtualMeeting = new VirtualMeeting()
                {
                    CreationDate = DateTime.Now,
                    StaffID = StaffID,
                    IsDeleted = false,
                    EndDate = toDate,
                    MeetingTitle = meetingTitle,
                    StatusID = true,
                    StartDate = fromDate,
                    MeetingID = sessionObj.MSTeamSessionID,

                };
                onlineMeetingssManager.Add(virtualMeeting);


                foreach (var item in attendees)
                {
                    var objAttendees = new MSTeams_SessionAttendes()
                    {
                        IsDeleted = false,
                        CreationDate = DateTime.Now,
                        JoinURL = meeting.JoinUrl.ToString(),
                        MSTeamSessionID = sessionObj.MSTeamSessionID,
                        StatusID = 0,
                        AttendeID = item.ID
                    };
                    atenddeessManager.Add(objAttendees);
                    NotificationsWeb notification = new NotificationsWeb()
                    {
                        ArabicDescription = meetingTitle,
                        CreatedByID = StaffID,
                        CreationDate = DateTime.Now,
                        EnglishDescription = meetingTitle,
                        Link = meeting.JoinUrl.ToString(),
                        //NotificationTypeID = (int)clsenumration.NotificationTypes.OnlineMeeting,
                        NotificationTypeID = (int)clsenumration.NotificationsTypes.Meeting,
                        ToGroupID = (int)item.GroupID,
                        ToID = item.ID,
                        Visited = false,
                        SchoolID = SchoolID,
                        ModuleID = sessionObj.MSTeamSessionID
                    };
                    notificationssManager.Add(notification);



                }
                msg = data.Content;


                return sessionObj;

            }
            else
            {

                msg = data.Content;
                return null;
            }
        }

        public class MeetingAttendees
        {

            public string ID { get; set; }
            public clsenumration.MeetingUserGroups GroupID { get; set; }
        }
    }
}
