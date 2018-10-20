using System;
using System.Collections.Generic;
using System.Text;
using ELearning.Model;

namespace ELearning.BAL
{
    public interface ICourseBAL
    {
        CourseViewModel GetCourseDetails();
        bool SaveCourse(string cname, string imagepath);
        bool SaveModule(string mname, int courseid, string filename);
        bool SaveLesson(string lname, int moduleid, string filename, string videoname, string videolength);
        bool SaveInstitute(InstituteModel objInsti);
        CourseInstituteViewModel GetCourseInstituteDetails();
        bool SaveTest(TestModel objModel);
        List<TestModel> GetTestList();
        AdminConfigurationViewModel AdminConfigurationList();
        List<UserModel> GetAdminUser();
    }
}
