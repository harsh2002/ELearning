using System;
using System.Collections.Generic;
using System.Text;
using ELearning.DAL;
using ELearning.Model;
using Microsoft.Extensions.Configuration;

namespace ELearning.BAL
{
    public class StudentBAL:IStudentBAL
    {
        StudentDAL dal;
        public StudentBAL(IConfiguration config)
        {
            dal = new StudentDAL(config);
        }
        public List<StudentModel> GetDashboard(int userid)
        {
            return dal.GetDashboard(userid);
        }

        public List<StudentModel> GetDashboardMain(int userid)
        {
            return dal.GetDashboardMain(userid);
        }

        public List<TestModel> GetUserTest(int userid, string moduleid,out int result)
        {
            return dal.GetUserTest(userid,moduleid,out result);
        }

        public List<StudentViewModel> GetUserModules(int userid,string courseid)
        {
            return dal.GetUserModules(userid, courseid);
        }

        public bool UpdateLessonStatus(int userid, string lessonid)
        {
            return dal.UpdateLessonStatus(userid, lessonid);
        }

        public bool SaveUserTest(int userid, string[] answer, string[] testid,int moduleid,int total)
        {
            return dal.SaveUserTest(userid, answer,testid,moduleid, total);
        }

        public UserModel GetUserProfile(int userid)
        {
            return dal.GetUserProfile(userid);
        }

        public int UpdateUser(UserModel objModel)
        {
            return dal.UpdateUser(objModel);
        }

        public List<StudentModel> GetTraningModule(int userid,int moduleid)
        {
            return dal.GetTraningModule(userid, moduleid);
        }
    }
}