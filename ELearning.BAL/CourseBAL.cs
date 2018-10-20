using System;
using System.Collections.Generic;
using System.Text;
using ELearning.DAL;
using ELearning.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;

namespace ELearning.BAL
{
    public class CourseBAL: ICourseBAL
    {
        CourseDAL dal;
        public CourseBAL(IConfiguration config)
        {
            dal = new CourseDAL(config);
        }
        public CourseViewModel GetCourseDetails()
        {
            return dal.GetCourseDetails();
        }

        public bool SaveCourse(string cname, string imagepath)
        {
            return dal.SaveCourse(cname, imagepath);
        }

        public AdminConfigurationViewModel AdminConfigurationList()
        {
            return dal.AdminConfigurationList();
        }

        public bool SaveModule(string mname, int courseid,string filename)
        {
            return dal.SaveModule(mname, courseid, filename);
        }

        public bool SaveLesson(string lname, int moduleid, string filename, string videoname, string videolength)
        {
            return dal.SaveLesson(lname, moduleid, filename, videoname, videolength);
        }

        public bool SaveInstitute(InstituteModel objInsti)
        {
            return dal.SaveInstitute(objInsti);
        }

        public bool MapCourse(int courseId,int instituteid)
        {
            return dal.MapCourse(courseId,instituteid);
        }

        public CourseInstituteViewModel GetCourseInstituteDetails()
        {
            return dal.GetCourseInstituteDetails();
        }

        public bool SaveTest(TestModel objModel)
        {
            return dal.SaveTest(objModel);
        }

        public List<TestModel> GetTestList()
        {
            return dal.GetTestList();
        }

        public List<UserModel> GetAdminUser()
        {
            return dal.GetAdminUser();
        }
    }
}
