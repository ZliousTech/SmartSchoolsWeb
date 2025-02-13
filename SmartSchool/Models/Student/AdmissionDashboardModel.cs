namespace SmartSchool.Models.Student
{
    public class AdmissionDashboardModel
    {

        public int PendingRequests { get; set; }
        public int AcceptedRequests { get; set; }
        public int RejectedRequests { get; set; }
        public int CategoryOne { get; set; }
        public int CategoryTwo { get; set; }
        public int CategoryThree { get; set; }
        public int CategoryFour { get; set; }
        public int CategoryFive { get; set; }
        public int OutsideCategories { get; set; }
    }
}