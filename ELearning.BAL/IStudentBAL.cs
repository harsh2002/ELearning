using System;
using System.Collections.Generic;
using System.Text;
using ELearning.Model;

namespace ELearning.BAL
{
    public interface IStudentBAL
    {
        List<StudentModel> GetDashboard(int userid);
        List<TestModel> GetUserTest(int userid, string moduleid,out int result);
        List<StudentViewModel> GetUserModules(int userid, string courseid);
        bool UpdateLessonStatus(int userid, string lessonid);
        bool SaveUserTest(int userid, string[] answer, string[] testid, int moduleid, int total);
        List<StudentModel> GetDashboardMain(int userid);
        UserModel GetUserProfile(int userid);
        int UpdateUser(UserModel objModel);
        List<StudentModel> GetTraningModule(int userid, int moduleid);
    }
}
