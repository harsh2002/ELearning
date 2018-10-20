using System;
using System.IO;
using System.Threading.Tasks;
using ELearning.BAL;
using ELearning.helper;
using ELearning.Model;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace ELearning.Controllers
{
    [AppAuthorization("Student")]
    public class StudentController : Controller
    {
        StudentBAL bal;
        private IHostingEnvironment _env;
        public StudentController(IConfiguration config, IHostingEnvironment env)
        {
            _env = env;
            bal = new StudentBAL(config);
        }
        public IActionResult Index()
        {
            ViewBag.VirtualPath = _env.WebRootPath;
            return View(bal.GetDashboardMain(CookieClaims.Instance.UserId(HttpContext.User)));
        }

        public IActionResult Training(string mid)
        {
            if (!string.IsNullOrEmpty(mid))
            {
                var lst = bal.GetTraningModule(CookieClaims.Instance.UserId(HttpContext.User),Convert.ToInt32(mid));

                return View(lst);
            }

            return View();
        }

        public IActionResult Modules(string id)
        {
            return View(bal.GetUserModules(CookieClaims.Instance.UserId(HttpContext.User), id));
        }

        public IActionResult Instruction(string id)
        {
            ViewBag.Id = id;
            return View();
        }

        public IActionResult UserTest(string id)
        {
            int result;
            ViewBag.Id = id;
            var lstTest = bal.GetUserTest(CookieClaims.Instance.UserId(HttpContext.User), id, out result);
            //if (result == 10)
            //{
            //    return View(lstTest);
            //}
            return View(lstTest);
        }

        [HttpPost]
        public JsonResult UpdateLessonStatus(string Lessonid)
        {
            bool result = bal.UpdateLessonStatus(CookieClaims.Instance.UserId(HttpContext.User), Lessonid);

            return Json(result);
        }

        [HttpPost]
        public JsonResult SubmitTest(string[] Answer, string[] Testid, int ModuleId, int Total)
        {
            bool result = bal.SaveUserTest(CookieClaims.Instance.UserId(HttpContext.User), Answer, Testid, ModuleId, Total);
            return Json(result);
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

        [HttpGet]
        public async Task<FileStreamResult> GetVideo(string name)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "Documents", "Videos", name);

            byte[] file = System.IO.File.ReadAllBytes(path);
            var stream = new MemoryStream(file);
            return new FileStreamResult(stream, "video/mp4");
        }

        [HttpGet]
        public async Task<FileContentResult> FileDownload(string name)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "Documents", "ModuleFiles", Path.GetFileName(name));

            byte[] fileBytes = System.IO.File.ReadAllBytes(path);

            return File(fileBytes, "application/vnd.ms-excel", name);
        }

    }

}

