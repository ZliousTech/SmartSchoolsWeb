namespace SmartSchool.Models.Settings
{
    public class ClassRooms
    {
        public int ClassRoomID { get; set; }
        public int CLassRoomNumber { get; set; }
        public int SchoolID { get; set; }
        public int Building { get; set; }
        public int Floor { get; set; }
        public int NumberofChairs { get; set; }
        public int Available { get; set; }
        public string RoomArabicName { get; set; }
        public string RoomEnglishName { get; set; }
        public string BuildingArabicName { get; set; }
        public string BuildingEnglishName { get; set; }
    }
}