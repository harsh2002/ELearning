using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ELearning.BAL;
using ELearning.helper;
using ELearning.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace ELearning.Controllers
{
    [AppAuthorization("InstituteAdmin")]
    public class InstituteController : Controller
    {
        InstituteBAL bal;
        public InstituteController(IConfiguration config)
        {
            bal = new InstituteBAL(config);
        }

        public IActionResult Index()
        {
            return View(bal.GetDashboard(CookieClaims.Instance.UserId(HttpContext.User)));
        }

        public IActionResult ActivationCode()
        {
            return View(bal.GetActivationCode(CookieClaims.Instance.UserId(HttpContext.User)));
        }

        public IActionResult StudentRecords()
        {
            return View(bal.GetStudentRecords(CookieClaims.Instance.UserId(HttpContext.User)));
        }

        public IActionResult UserProfile()
        {
            return View(bal.GetUserProfile(CookieClaims.Instance.UserId(HttpContext.User)));
        }

        [HttpPost]
        public IActionResult UserProfile(IFormCollection fc)
        {
            UserModel objModel = new UserModel();
            objModel.UserId = CookieClaims.Instance.UserId(HttpContext.User);
            if (!string.IsNullOrEmpty(fc["email"]))
                objModel.Email = fc["email"].ToString();

            if (!string.IsNullOrEmpty(fc["name"]))
                objModel.Name = fc["name"].ToString();
            if (objModel.Name != null && objModel.Email != null)
            {
                int result = bal.UpdateUser(objModel);
                ViewBag.usuccess = "Successfully updated profile.";
            }
            else
            {
                ViewBag.ufailure = "Can not update profle, please try again.";
            } 

            return View(bal.GetUserProfile(CookieClaims.Instance.UserId(HttpContext.User)));
        }

        [HttpPost]
        public IActionResult ChangePassword(IFormCollection fc)
        {
            UserModel objModel = new UserModel();
            
            objModel = bal.GetUserProfile(CookieClaims.Instance.UserId(HttpContext.User));
            objModel.UserId = CookieClaims.Instance.UserId(HttpContext.User);

            if (objModel.Password.DecryptString() == fc["opassword"])
            {
                if (!string.IsNullOrEmpty(fc["npassword"]))
                {
                    objModel.Password = fc["npassword"].ToString().EncryptString();
                }
                int result = bal.UpdateUser(objModel);
                if (result == 1)
                {
                    ViewBag.success = "Successfully changed password.";
                }
                else
                {
                    ViewBag.failure = "Can not update password, please try again.";
                }
            }
            else
            {
                ViewBag.failure = "Old password is not matched";
            }
            return View("UserProfile", bal.GetUserProfile(CookieClaims.Instance.UserId(HttpContext.User)));
        }
    }
}
