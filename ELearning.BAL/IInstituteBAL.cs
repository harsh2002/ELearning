using System;
using System.Collections.Generic;
using System.Text;
using ELearning.Model;

namespace ELearning.BAL
{
    public interface IInstituteBAL
    {
        InstituteViewModel GetDashboard(int userid);
        List<InstituteModel> GetActivationCode(int userid);
        InstituteViewModel GetStudentRecords(int userid);
        UserModel GetUserProfile(int userid);
        int UpdateUser(UserModel objModel);
    }
}
