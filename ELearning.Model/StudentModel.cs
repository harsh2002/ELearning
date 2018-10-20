using System;
using System.Collections.Generic;
using System.Text;

namespace ELearning.Model
{
    public class StudentModel
    {
        public int CourseId { get; set; }
        public int ModuleId { get; set; }
        public string CourseName { get; set; }
        public string ImagePath { get; set; }
        public byte[] Image{ get; set; }
        public string ModuleName { get; set; }
        public string LessonName { get; set; }
        public int LessonId { get; set; }
        public string VideoName { get; set; }
        public string FilePath { get; set; }
        public bool IsCompleted { get; set; }
        public int MarkSecure { get; set; }
        public int MarkTotal { get; set; }
        public int LessonCompleted { get; set; }
        

    }

    public class StudentViewModel
    {
        public ModuleModel Module{get;set;}
        public int TotalLesson { get; set; }
        public int TestCompleted { get; set; }
        public int LessonCompleted { get; set; }
        public int Attempt { get; set; }

    }
}
