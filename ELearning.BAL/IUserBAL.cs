using System;
using ELearning.Model;

namespace ELearning.BAL
{
    public interface IUserBAL
    {
        UserModel UserAuthentication(UserModel objuser);
        UserModel RegisterStudent(UserModel objModel, out int result);
        UserModel Forgotpassword(string email, out bool result);
    }
}
