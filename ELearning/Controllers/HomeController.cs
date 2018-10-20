using System;
using System.Diagnostics;
using System.IO;
using System.Security.Claims;
using ELearning.BAL;
using ELearning.helper;
using ELearning.Model;
using ELearning.Models;
using ELearning.Utility;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace ELearning.Controllers
{
    public class HomeController : Controller
    {
        UserBAL bal;
        IConfiguration _config;
        public HomeController(IConfiguration config)
        {
            bal = new UserBAL(config);
            _config = config;
        }

        public IActionResult Index()
        {
            //var sss = HttpContext.Session.GetObject<UserModel>("SuperAdmin");

            string ssss=Guid.NewGuid().ToString() + Path.GetExtension("ads.mp4");

            string ss = "0dh5tL5IKPNe72j4pEoT6fjcvFxEYMdnp+/MiuP9Ozo=";//"!nd!@!ndore74";
            string sss = ss.DecryptString();
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public IActionResult Login(IFormCollection fc)
        {
            string password = fc["password"];
            UserModel objUser = new UserModel()
            {
                Email = fc["email"],
                Password = fc["password"]
            };

            objUser = bal.UserAuthentication(objUser);

            if (objUser.IsAuthenticated && objUser.Password.DecryptString() == password)
            {
                var claims = new[]
                {
                    new Claim("name", objUser.Name),
                    new Claim("email", objUser.Email),
                    new Claim("role", objUser.RoleId.ToString()),
                    new Claim("userid",objUser.UserId.ToString())
                };

                var identity = new ClaimsIdentity(claims, "FiverSecurityScheme");
                HttpContext.SignInAsync("FiverSecurityScheme", new ClaimsPrincipal(identity));

                if (Convert.ToInt16(RoleType.SuperAdmin) == objUser.RoleId)
                {
                    return RedirectToAction("Index", "SuperAdmin");
                }
                if (Convert.ToInt16(RoleType.InstituteAdmin) == objUser.RoleId)
                {
                    return RedirectToAction("Index", "Institute");
                }
                if (Convert.ToInt16(RoleType.Student) == objUser.RoleId)
                {
                    return RedirectToAction("Index", "Student");
                }
            }
            else
            {
                ViewBag.error = "Email or password is wrong";
                return View("Index", "Home");
            }

            return View("Index", "Home");
        }

        [HttpPost]
        public IActionResult Register(IFormCollection fc)
        {
            bool isValid = true;

            UserModel objUser = new UserModel();

            if (string.IsNullOrEmpty(fc["rname"])) isValid = false;
            else
                objUser.Name = fc["rname"];
            if (string.IsNullOrEmpty(fc["remail"])) isValid = false;
            else
                objUser.Email = fc["remail"];
            if (string.IsNullOrEmpty(fc["cemail"])) isValid = false;
            if (string.IsNullOrEmpty(fc["rpassword"])) isValid = false;
            else
                objUser.Password = fc["rpassword"].ToString().EncryptString();
            if (string.IsNullOrEmpty(fc["cpassword"])) isValid = false;
            if (string.IsNullOrEmpty(fc["activationcode"]))
                isValid = false;
            else
                objUser.ActivationCode = fc["activationcode"];

            if (isValid)
            {
                objUser = bal.RegisterStudent(objUser, out int result);

                if (objUser.IsAuthenticated)
                {
                    var claims = new[]
                    {
                    new Claim("name", objUser.Name),
                    new Claim("email", objUser.Email),
                    new Claim("role", objUser.RoleId.ToString()),
                    new Claim("userid",objUser.UserId.ToString())
                };

                    var identity = new ClaimsIdentity(claims, "FiverSecurityScheme");
                    HttpContext.SignInAsync("FiverSecurityScheme", new ClaimsPrincipal(identity));

                    return RedirectToAction("Index", "Student");
                }
                else
                {
                    if (result == 1) ViewBag.rerror = "Activation code is wrong,Please verify.";
                    if (result == 2) ViewBag.rerror = "Email address is already registered.";
                    if (result == 3) ViewBag.rerror = "Technical issue, Please reach out to admin.";
                }
            }
            else
            {
                ViewBag.rerror = "You are not authorized.";
            }

            return View("Index", "Home");
        }

        [HttpPost]
        public JsonResult Forgotpassword(string Email)
        {
            bool result;
            UserModel objModel = bal.Forgotpassword(Email, out result);
            objModel.Email = Email;
            objModel.Password = !string.IsNullOrEmpty(objModel.Password) ? objModel.Password.DecryptString() : "";
            if (result)
            {
                new EmailHelper(_config).SendEmail(objModel);
            }

            return Json(result);
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        [HttpPost]
        public JsonResult Contact(string Name,string Email,string Phone,string Message)
        {
            ContactModel objModel = new ContactModel();
            objModel.Email = Email;
            objModel.Name = Name;
            objModel.Phone = Phone;
            objModel.Message = Message;

            new EmailHelper(_config).SendContactEmail(objModel);
            return Json(true);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
