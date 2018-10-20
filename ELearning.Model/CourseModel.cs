using System;
using System.Collections.Generic;
using System.Text;

namespace ELearning.Model
{
    public class CourseModel
    {
        public int CourseId { get; set; }
        public string Name { get; set; }
    }
    public class ModuleModel
    {
        public int ModuleId { get; set; }
        public string Name { get; set; }
        public string ModuleFile { get; set; }
    }
    public class LessonModel
    {
        public int LessonId { get; set; }
        public string Name { get; set; }
        public int Total { get; set; }
        public int Completed { get; set; }
    }

    public class CourseViewModel
    {
        public List<CourseModel> CourseList { get; set; }
        public List<ModuleModel> ModuleList { get; set; }
        public List<LessonModel> LessonList { get; set; }
    }

    public class CourseInstituteViewModel
    {
        public List<CourseModel> CourseList { get; set; }
        public List<InstituteModel> InstituteList { get; set; }
    }

    public class AdminConfigurationViewModel
    {
        public List<CourseModel> CourseList { get; set; }
        public List<ModuleModel> ModuleList { get; set; }
        public List<LessonModel> LessonList { get; set; }
        public List<InstituteModel> InstituteList { get; set; }
    }
}
