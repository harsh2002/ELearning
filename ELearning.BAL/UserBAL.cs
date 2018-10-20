using ELearning.DAL;
using ELearning.Model;
using Microsoft.Extensions.Configuration;

namespace ELearning.BAL
{
    public class UserBAL:IUserBAL
    {
        UserDAL dal;
        public UserBAL(IConfiguration config)
        {
            dal = new UserDAL(config);
        }
        public UserModel UserAuthentication(UserModel objUser)
        {
            return dal.UserAuthentication(objUser);
        }

        public UserModel RegisterStudent(UserModel objUser, out int result)
        {
            return dal.RegisterStudent(objUser, out result);
        }

        public UserModel Forgotpassword(string email, out bool result)
        {
            return dal.Forgotpassword(email, out result);
        }
    }
}
