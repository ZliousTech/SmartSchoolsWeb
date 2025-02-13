namespace Common.Base
{
    public class MicrosoftTeamsMeetings
    {
    }
}
namespace MicrosoftTeams
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;

    public partial class MSTeamsMeetingData
    {
        [JsonProperty("@odata.context")]
        public Uri OdataContext { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("creationDateTime")]
        public DateTimeOffset CreationDateTime { get; set; }

        [JsonProperty("startDateTime")]
        public DateTimeOffset StartDateTime { get; set; }

        [JsonProperty("endDateTime")]
        public DateTimeOffset EndDateTime { get; set; }

        [JsonProperty("joinUrl")]
        public Uri JoinUrl { get; set; }

        [JsonProperty("joinWebUrl")]
        public Uri JoinWebUrl { get; set; }

        [JsonProperty("subject")]
        public string Subject { get; set; }

        [JsonProperty("isBroadcast")]
        public bool IsBroadcast { get; set; }

        [JsonProperty("autoAdmittedUsers")]
        public string AutoAdmittedUsers { get; set; }

        [JsonProperty("outerMeetingAutoAdmittedUsers")]
        public object OuterMeetingAutoAdmittedUsers { get; set; }

        [JsonProperty("capabilities")]
        public List<object> Capabilities { get; set; }

        [JsonProperty("videoTeleconferenceId")]
        public object VideoTeleconferenceId { get; set; }

        [JsonProperty("externalId")]
        public object ExternalId { get; set; }

        [JsonProperty("audioConferencing")]
        public object AudioConferencing { get; set; }

        [JsonProperty("meetingInfo")]
        public object MeetingInfo { get; set; }

        [JsonProperty("participants")]
        public Participants Participants { get; set; }

        [JsonProperty("chatInfo")]
        public ChatInfo ChatInfo { get; set; }

        [JsonProperty("joinInformation")]
        public JoinInformation JoinInformation { get; set; }
    }

    public partial class ChatInfo
    {
        [JsonProperty("threadId")]
        public string ThreadId { get; set; }

        [JsonProperty("messageId")]
        //[JsonConverter(typeof(ParseStringConverter))]
        public long MessageId { get; set; }

        [JsonProperty("replyChainMessageId")]
        public object ReplyChainMessageId { get; set; }
    }

    public partial class JoinInformation
    {
        [JsonProperty("content")]
        public string Content { get; set; }

        [JsonProperty("contentType")]
        public string ContentType { get; set; }
    }

    public partial class Participants
    {
        [JsonProperty("organizer")]
        public Organizer Organizer { get; set; }

        [JsonProperty("attendees")]
        public List<object> Attendees { get; set; }
    }

    public partial class Organizer
    {
        [JsonProperty("upn")]
        public string Upn { get; set; }

        [JsonProperty("identity")]
        public Identity Identity { get; set; }
    }

    public partial class Identity
    {
        [JsonProperty("phone")]
        public object Phone { get; set; }

        [JsonProperty("guest")]
        public object Guest { get; set; }

        [JsonProperty("encrypted")]
        public object Encrypted { get; set; }

        [JsonProperty("onPremises")]
        public object OnPremises { get; set; }

        [JsonProperty("azureApplicationInstance")]
        public object AzureApplicationInstance { get; set; }

        [JsonProperty("applicationInstance")]
        public object ApplicationInstance { get; set; }

        [JsonProperty("application")]
        public object Application { get; set; }

        [JsonProperty("device")]
        public object Device { get; set; }

        [JsonProperty("user")]
        public User User { get; set; }
    }

    public partial class User
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("displayName")]
        public object DisplayName { get; set; }

        [JsonProperty("tenantId")]
        public Guid TenantId { get; set; }

        [JsonProperty("identityProvider")]
        public string IdentityProvider { get; set; }
    }
}
namespace MSTeamsEvents
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;

    public partial class EventObject
    {
        [JsonProperty("subject")]
        public string Subject { get; set; }

        [JsonProperty("body")]
        public Body Body { get; set; }

        [JsonProperty("start")]
        public End Start { get; set; }

        [JsonProperty("end")]
        public End End { get; set; }

        [JsonProperty("location")]
        public Location Location { get; set; }

        [JsonProperty("recurrence")]
        public Recurrence Recurrence { get; set; }
    }

    public partial class Body
    {
        [JsonProperty("contentType")]
        public string ContentType { get; set; }

        [JsonProperty("content")]
        public string Content { get; set; }
    }

    public partial class End
    {
        [JsonProperty("dateTime")]
        public string DateTime { get; set; }

        [JsonProperty("timeZone")]
        public string TimeZone { get; set; }
    }

    public partial class Location
    {
        [JsonProperty("displayName")]
        public string DisplayName { get; set; }
    }

    public partial class Recurrence
    {
        [JsonProperty("pattern")]
        public Pattern Pattern { get; set; }

        [JsonProperty("range")]
        public Range Range { get; set; }
    }

    public partial class Pattern
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("interval")]
        public long Interval { get; set; }

        [JsonProperty("daysOfWeek")]
        public List<string> DaysOfWeek { get; set; }

        [JsonProperty("index")]
        public string Index { get; set; }
    }

    public partial class Range
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("startDate")]
        public DateTimeOffset StartDate { get; set; }
    }
}
