using System;

namespace ELearning.Model
{
    public class UserModel
    {
        public int UserId { set; get; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int RoleId { get; set; }
        public bool IsAuthenticated { get; set; }
        public string ActivationCode { get; set; }
    }
}
