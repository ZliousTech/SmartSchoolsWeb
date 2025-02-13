using SmartSchool.Models.Settings;
using System.Collections.Generic;

namespace SmartSchool.Models.Grades
{
    public class PrepareGrades : PrepareExam
    {
        public List<Grades> Grades { get; set; }
    }
}