using System;
using System.IO;
using ELearning.BAL;
using ELearning.helper;
using ELearning.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace ELearning.Controllers
{
    [AppAuthorization("SuperAdmin")]
    public class SuperAdminController : Controller
    {
        CourseBAL bal;
        public SuperAdminController(IConfiguration config)
        {
            bal = new CourseBAL(config);
        }
        #region Content Management
        public IActionResult Index()
        {
            AdminConfigurationViewModel objModel = bal.AdminConfigurationList();
            return View(objModel);
        }

        public IActionResult Course()
        {

            return View(bal.GetCourseDetails());
        }

        [HttpPost]
        public IActionResult Course(IFormCollection fc, IFormFile file)
        {
            if (string.IsNullOrEmpty(fc["cname"]) || string.IsNullOrEmpty(file.FileName))
            {
                ViewBag.error = "There is an issue saving data";
                return View("Index", bal.GetCourseDetails());
            }

            bool result = bal.SaveCourse(fc["cname"], file.FileName);
            if (result)
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "ServerImage", file.FileName);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    file.CopyTo(stream);
                }
                ViewBag.success = "Successfully Save";
            }
            else
            {
                ViewBag.failure = "There is an issue saving data";
            }
            return View("Course", bal.GetCourseDetails());
        }

        public IActionResult Module()
        {
            return View(bal.GetCourseDetails());
        }

        [HttpPost]
        public IActionResult Module(IFormCollection fc, IFormFile file)
        {
            if (string.IsNullOrEmpty(fc["mname"]) || string.IsNullOrEmpty(fc["courseid"]))
            {
                ViewBag.error = "There is an issue saving data";
                return View("Index", bal.GetCourseDetails());
            }
            string filename = string.Empty;
            if (file != null && file.Length > 0)
            {
                filename = file.FileName;
            }

            bool result = bal.SaveModule(fc["mname"], Convert.ToInt32(fc["courseid"]), filename);
            if (result)
            {
                if (file != null && file.Length > 0)
                {
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "Documents", "ModuleFiles", file.FileName);

                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                }
                ViewBag.success = "Successfully Save";
            }
            else
            {
                ViewBag.faulure = "There is an issue saving data";
            }
            return View(bal.GetCourseDetails());
        }

        public IActionResult Lesson()
        {
            return View(bal.GetCourseDetails());
        }

        [HttpPost]
        public IActionResult Lesson(IFormCollection fc, IFormFile file)
        {
            if (string.IsNullOrEmpty(fc["lname"]) || string.IsNullOrEmpty(fc["moduleid"])
                || string.IsNullOrEmpty(fc["vname"]) || string.IsNullOrEmpty(fc["vlength"])
                || string.IsNullOrEmpty(file.FileName))
            {
                ViewBag.error = "There is an issue saving data";
                return View("Index", bal.GetCourseDetails());
            }

            string filename = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);

            bool result = bal.SaveLesson(fc["lname"], Convert.ToInt32(fc["moduleid"]), filename, fc["vname"], fc["vlength"]);
            if (result)
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(), "Documents", "Videos", filename);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    file.CopyTo(stream);
                }
                ViewBag.success = "Successfully Save";
            }
            else
            {
                ViewBag.faulure = "There is an issue saving data";
            }
            return View(bal.GetCourseDetails());
        }

        public IActionResult Test()
        {
            return View(bal.GetCourseDetails());
        }

        [HttpPost]
        public IActionResult Test(IFormCollection fc, IFormFile file)
        {
            if (string.IsNullOrEmpty(fc["moduleid"])
                || string.IsNullOrEmpty(fc["answer"]))
            {
                ViewBag.error = "There is an issue saving data";
                return View("Test", bal.GetCourseDetails());
            }

            TestModel objModel = new TestModel();
            objModel.OptionA = Convert.ToString(fc["optiona"]);
            objModel.OptionB = Convert.ToString(fc["optionb"]);
            objModel.OptionC = Convert.ToString(fc["optionc"]);
            objModel.OptionD = Convert.ToString(fc["optiond"]);
            objModel.Answer = Convert.ToString(fc["answer"]);
            objModel.Module = new ModuleModel { ModuleId = Convert.ToInt32(fc["moduleid"]) };
            if (file != null && file.Length > 0)
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    file.OpenReadStream().CopyTo(ms);

                    objModel.Image = ms.ToArray();
                }
            }
            objModel.Question = Convert.ToString(fc["question"]);
            bool result = bal.SaveTest(objModel);

            if (result)
            {
                ViewBag.success = "Successfully save";
            }
            else
            {
                ViewBag.error = "There is an issue saving data";
            }


            return View("Test", bal.GetCourseDetails());
        }

        public IActionResult TestList()
        {

            return View(bal.GetTestList());
        }

        #endregion

        #region Corporate

        public IActionResult Institute()
        {
            return View(bal.GetCourseInstituteDetails());
        }

        [HttpPost]
        public IActionResult Institute(IFormCollection fc)
        {
            InstituteModel objInsti = new InstituteModel();
            Random random = new Random();
            var password = random.Next(0, 1000000);

            if (!string.IsNullOrEmpty(fc["iname"])) objInsti.Name = fc["iname"];

            if (!string.IsNullOrEmpty(fc["year"]) && !string.IsNullOrEmpty(fc["month"]))
                objInsti.Subcription = new SubcriptionModel
                {
                    Year = Convert.ToInt16(fc["year"]),
                    Month = Convert.ToInt16(fc["month"])
                };

            if (!string.IsNullOrEmpty(fc["nname"])) objInsti.ActivationNumber = fc["nname"];

            if (!string.IsNullOrEmpty(fc["uname"]) && !string.IsNullOrEmpty(fc["ename"]))
                objInsti.User = new UserModel
                {
                    Name = fc["uname"],
                    Email = fc["ename"],
                    Password = password.ToString().EncryptString()
                };

            bool result = bal.SaveInstitute(objInsti);
            if (result)
            {
                ViewBag.success = "Successfully Saved ";
            }
            else
            {
                ViewBag.failure = "Error occured";
            }
            return View("Institute", bal.GetCourseInstituteDetails());
        }

        [HttpPost]
        public IActionResult MapCourse(IFormCollection fc)
        {
            bool result = bal.MapCourse(Convert.ToInt32(fc["courseid"]), Convert.ToInt32(fc["instituteid"]));
            if (result)
            {
                ViewBag.success = "Successfully mapped";
            }
            else
            {
                ViewBag.failure = "Error occured";
            }
            return View("Institute", bal.GetCourseDetails());
        }

        #endregion

        public IActionResult AdminUser()
        {
            return View(bal.GetAdminUser());
        }
    }
}