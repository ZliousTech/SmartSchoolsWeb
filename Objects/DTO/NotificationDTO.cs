using System;

namespace Objects.DTO
{
    public class NotificationDTO
    {
        public int NotifcationID { get; set; }
        public string ArabicDescription { get; set; }
        public string EnglishDescription { get; set; }
        public string CreatedByID { get; set; }
        public string ToID { get; set; }
        public Nullable<int> NotificationTypeID { get; set; }
        public string Link { get; set; }
        public Nullable<int> ToGroupID { get; set; }
        public Nullable<System.DateTime> CreationDate { get; set; }
        public Nullable<bool> Visited { get; set; }
        public Nullable<int> SchoolID { get; set; }
        public bool IsActive { get; set; }
    }
}
