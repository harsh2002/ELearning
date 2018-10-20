using System;
using System.Collections.Generic;
using System.Text;
using ELearning.DAL;
using ELearning.Model;
using Microsoft.Extensions.Configuration;

namespace ELearning.BAL
{
    public class InstituteBAL:IInstituteBAL
    {
        InstituteDAL dal;
        public InstituteBAL(IConfiguration config)
        {
            dal = new InstituteDAL(config);
        }
        public InstituteViewModel GetDashboard(int userid)
        {
            return dal.GetDashboard(userid);
        }

        public List<InstituteModel> GetActivationCode(int userid)
        {
            return dal.GetActivationCode(userid);
        }

        public InstituteViewModel GetStudentRecords(int userid)
        {
            return dal.GetStudentRecords(userid);
        }

        public UserModel GetUserProfile(int userid)
        {
            return dal.GetUserProfile(userid);
        }

        public int UpdateUser(UserModel objModel)
        {
            return dal.UpdateUser(objModel);
        }
    }
}
