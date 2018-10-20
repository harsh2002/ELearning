using System;
using System.Collections.Generic;
using System.Text;

namespace ELearning.Model
{
    public class InstituteModel
    {
        public int InstituteId { get; set; }
        public string Name { get; set; }
        public string ActivationNumber { get; set; }
        public UserModel User { get; set; }
        public SubcriptionModel Subcription { get; set; }
        public bool IsActivated { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateExpired { get; set; }
        public ModuleModel Module { get; set; }
        public int MarkTotal { get; set; }
        public int MarkSecure { get; set; }
        public int MarkAvg { get; set; }
        public LessonModel Lesson { get; set; }
        public int Attempt { get; set; }
    }

    public class InstituteViewModel
    {
        public InstituteDashboard institute { get; set; }
        public List<InstituteModel> InstituteList { get; set; }
    }

    public class SubcriptionModel
    {
        public int Year { get; set; }
        public int Month { get; set; }

    }

    public class InstituteDashboard
    {
        public int ActivatedCode { get; set; }
        public int NonActivatedCode { get; set; }
        public int TotalStudent { get; set; }
        public int TotalCourses { get; set; }
        public int TotalTestAttended { get; set; }
        public int TotalLesson { get; set; }
    }

}
