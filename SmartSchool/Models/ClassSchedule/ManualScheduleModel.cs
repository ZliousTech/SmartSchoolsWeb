using Common.Helpers;
using Objects.DTO;
using SmartSchool.Models.System;
using System.Collections.Generic;

namespace SmartSchool.Models.ClassSchedule
{
    public class ManualScheduleModel : SystemSettingsModel
    {
        public string TeacherID { get; set; }
        public int SchoolClassID { get; set; }
        public int SchoolID { get; set; }
        public List<LookupDTO> Classes { get; set; }
        public List<LookupDTO> Teachers { get; set; }

        public int SectionID { get; set; }

        public List<ManualScheduleDTO> ManualScheduleDTO { get; set; }
        public string ClassName { get; set; }
        public int ClassID { get; set; }
        public string SectionName { get; set; }


        //TimeTable
        public List<LookupDTO> WeekDaysList { get; set; }
        public List<LookupDTO> SessionsPerDayWithStartTimeList { get; set; }
        public List<LookupDTO> SessionNameList { get; set; }
        public List<ClassTimeTableDTO> TeacherTimeTableList { get; set; }

    }
}