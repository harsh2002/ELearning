using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
using ELearning.Model;
using ELearning.Utility;
using Microsoft.Extensions.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;

namespace ELearning.DAL
{
    public class CourseDAL
    {
        Database objDB;
        AppConfiguration appSettings;
        public CourseDAL(IConfiguration config)
        {
            appSettings = new AppConfiguration(config);
            objDB = new SqlDatabase(appSettings.connectionString);
        }

        public CourseViewModel GetCourseDetails()
        {
            objDB = new SqlDatabase(appSettings.connectionString);
            List<CourseModel> lstCourse = new List<CourseModel>();
            List<ModuleModel> lstModule = new List<ModuleModel>();
            List<LessonModel> lstLesson = new List<LessonModel>();
            CourseViewModel objViewModel = new CourseViewModel();
            using (DbCommand objCMD = objDB.GetStoredProcCommand("spGetCourseDetails"))
            {
                try
                {
                    DataSet ds = objDB.ExecuteDataSet(objCMD);

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            lstCourse.Add(new CourseModel
                            {
                                CourseId = Convert.ToInt32(ds.Tables[0].Rows[i]["CourseId"]),
                                Name = ds.Tables[0].Rows[i]["Name"].ToString()
                            });
                        }
                        objViewModel.CourseList = lstCourse;
                    }
                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                        {
                            lstModule.Add(new ModuleModel
                            {
                                ModuleId = Convert.ToInt32(ds.Tables[1].Rows[i]["ModuleId"]),
                                Name = ds.Tables[1].Rows[i]["Name"].ToString()
                            });
                        }
                        objViewModel.ModuleList = lstModule;
                    }
                    if (ds.Tables[2].Rows.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[2].Rows.Count; i++)
                        {
                            lstLesson.Add(new LessonModel
                            {
                                LessonId = Convert.ToInt32(ds.Tables[2].Rows[i]["LessonId"]),
                                Name = ds.Tables[2].Rows[i]["Name"].ToString()
                            });
                        }
                        objViewModel.LessonList = lstLesson;
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return objViewModel;
        }

        public List<UserModel> GetAdminUser()
        {
            List<UserModel> userList = new List<UserModel>();

            objDB = new SqlDatabase(appSettings.connectionString);
            using (DbCommand objCMD = objDB.GetStoredProcCommand("spGetAdminUser"))
            {
                try
                {
                    DataSet ds = objDB.ExecuteDataSet(objCMD);

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            userList.Add(new UserModel
                            {
                                Name = Convert.ToString(ds.Tables[0].Rows[i]["Name"]),
                                Email = Convert.ToString(ds.Tables[0].Rows[i]["Email"]),
                                Password = Convert.ToString(ds.Tables[0].Rows[i]["Password"])
                            });
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
            return userList;
        }

        public AdminConfigurationViewModel AdminConfigurationList()
        {
            AdminConfigurationViewModel objModel = new AdminConfigurationViewModel();
            List<CourseModel> CourseList = new List<CourseModel>();
            List<ModuleModel> ModuleList = new List<ModuleModel>();
            List<LessonModel> LessonList = new List<LessonModel>();
            List<InstituteModel> InstituteList = new List<InstituteModel>();

            objDB = new SqlDatabase(appSettings.connectionString);
            using (DbCommand objCMD = objDB.GetStoredProcCommand("spAdminConfigurationList"))
            {
                try
                {
                    DataSet ds = objDB.ExecuteDataSet(objCMD);

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            CourseList.Add(new CourseModel
                            {
                                CourseId = Convert.ToInt32(ds.Tables[0].Rows[i]["CourseId"]),
                                Name = Convert.ToString(ds.Tables[0].Rows[i]["Name"])
                            });
                        }
                    }

                    objModel.CourseList = CourseList;

                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                        {
                            ModuleList.Add(new ModuleModel
                            {
                                ModuleId = Convert.ToInt32(ds.Tables[1].Rows[i]["ModuleId"]),
                                Name = Convert.ToString(ds.Tables[1].Rows[i]["Name"])
                            });
                        }
                    }

                    objModel.ModuleList = ModuleList;

                    if (ds.Tables[2].Rows.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[2].Rows.Count; i++)
                        {
                            LessonList.Add(new LessonModel
                            {
                                LessonId = Convert.ToInt32(ds.Tables[2].Rows[i]["LessonId"]),
                                Name = Convert.ToString(ds.Tables[2].Rows[i]["Name"])
                            });
                        }
                    }

                    objModel.LessonList = LessonList;

                    if (ds.Tables[3].Rows.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[3].Rows.Count; i++)
                        {
                            InstituteList.Add(new InstituteModel
                            {
                                InstituteId = Convert.ToInt32(ds.Tables[3].Rows[i]["InstituteId"]),
                                Name = Convert.ToString(ds.Tables[3].Rows[i]["Name"])
                            });
                        }
                    }

                    objModel.InstituteList = InstituteList;

                }
                catch (Exception)
                {
                    throw;
                }
            }
            return objModel;
        }

        public List<TestModel> GetTestList()
        {
            objDB = new SqlDatabase(appSettings.connectionString);
            List<TestModel> lstTest = new List<TestModel>();
            using (DbCommand objCMD = objDB.GetStoredProcCommand("spGetTestList"))
            {
                try
                {
                    DataSet ds = objDB.ExecuteDataSet(objCMD);

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            lstTest.Add(new TestModel
                            {
                                TestId = Convert.ToInt32(ds.Tables[0].Rows[i]["TestId"]),
                                Image = Convert.ToString(ds.Tables[0].Rows[i]["TestImage"]) != string.Empty ? (byte[])ds.Tables[0].Rows[i]["TestImage"] : null,
                                Question = Convert.ToString(ds.Tables[0].Rows[i]["Question"]),
                                Answer = Convert.ToString(ds.Tables[0].Rows[i]["TestAnswer"]),
                                Module = new ModuleModel { Name = ds.Tables[0].Rows[i]["Name"].ToString() }
                            });
                        }
                    }

                }
                catch (Exception)
                {
                    throw;
                }
            }
            return lstTest;
        }

        public bool SaveTest(TestModel objModel)
        {
            bool result = false;
            using (DbCommand objCMD = objDB.GetStoredProcCommand("spSaveTest"))
            {
                try
                {
                    objDB.AddInParameter(objCMD, "@ModuleId", DbType.Int32, objModel.Module.ModuleId);
                    objDB.AddInParameter(objCMD, "@Image", DbType.Binary, objModel.Image);
                    objDB.AddInParameter(objCMD, "@Question", DbType.String, objModel.Question);
                    objDB.AddInParameter(objCMD, "@Answer", DbType.String, objModel.Answer);
                    objDB.AddInParameter(objCMD, "@OptionA", DbType.String, objModel.OptionA);
                    objDB.AddInParameter(objCMD, "@OptionB", DbType.String, objModel.OptionB);
                    objDB.AddInParameter(objCMD, "@OptionC", DbType.String, objModel.OptionC);
                    objDB.AddInParameter(objCMD, "@OptionD", DbType.String, objModel.OptionD);
                    objDB.AddOutParameter(objCMD, "@Status", DbType.Int16, 0);
                    objDB.ExecuteNonQuery(objCMD);
                    result = Convert.ToBoolean(objDB.GetParameterValue(objCMD, "@Status"));
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return result;
        }

        public bool SaveLesson(string lname, int moduleid, string filename, string videoname, string videolength)
        {
            bool result = false;
            using (DbCommand objCMD = objDB.GetStoredProcCommand("spSaveLesson"))
            {
                try
                {
                    objDB.AddInParameter(objCMD, "@LessonName", DbType.String, lname);
                    objDB.AddInParameter(objCMD, "@FilePath", DbType.String, filename);
                    objDB.AddInParameter(objCMD, "@ModuleId", DbType.Int32, moduleid);
                    objDB.AddInParameter(objCMD, "@VideoName", DbType.String, videoname);
                    objDB.AddInParameter(objCMD, "@VideoLength", DbType.String, videolength);
                    objDB.AddOutParameter(objCMD, "@Status", DbType.Int16, 0);
                    objDB.ExecuteNonQuery(objCMD);
                    result = Convert.ToBoolean(objDB.GetParameterValue(objCMD, "@Status"));
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return result;
        }

        public bool SaveModule(string mname, int courseid, string filename)
        {
            bool result = false;
            using (DbCommand objCMD = objDB.GetStoredProcCommand("spSaveModule"))
            {
                try
                {
                    objDB.AddInParameter(objCMD, "@ModuleName", DbType.String, mname);
                    objDB.AddInParameter(objCMD, "@CourseId", DbType.Int32, courseid);
                    objDB.AddInParameter(objCMD, "@ModuleFile", DbType.String, filename);
                    objDB.AddOutParameter(objCMD, "@Status", DbType.Int16, 0);
                    objDB.ExecuteNonQuery(objCMD);
                    result = Convert.ToBoolean(objDB.GetParameterValue(objCMD, "@Status"));
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return result;
        }

        public bool SaveCourse(string cname, string imagepath)
        {
            bool result = false;
            using (DbCommand objCMD = objDB.GetStoredProcCommand("spSaveCourse"))
            {
                try
                {
                    objDB.AddInParameter(objCMD, "@CourseName", DbType.String, cname);
                    objDB.AddInParameter(objCMD, "@ImagePath", DbType.String, imagepath);
                    objDB.AddOutParameter(objCMD, "@Status", DbType.Int16, 0);
                    objDB.ExecuteNonQuery(objCMD);
                    result = Convert.ToBoolean(objDB.GetParameterValue(objCMD, "@Status"));
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return result;
        }

        public bool SaveInstitute(InstituteModel objInsti)
        {
            bool result = false;
            using (DbCommand objCMD = objDB.GetStoredProcCommand("spSaveInstitute"))
            {
                try
                {
                    objDB.AddInParameter(objCMD, "@EmailId", DbType.String, objInsti.User.Email);
                    objDB.AddInParameter(objCMD, "@InstituteName", DbType.String, objInsti.Name);
                    objDB.AddInParameter(objCMD, "@UserName", DbType.String, objInsti.User.Name);
                    objDB.AddInParameter(objCMD, "@Password", DbType.String, objInsti.User.Password);
                    objDB.AddInParameter(objCMD, "@TotalStudentCode", DbType.Int16, objInsti.ActivationNumber);
                    objDB.AddInParameter(objCMD, "@Year", DbType.Int16, objInsti.Subcription.Year);
                    objDB.AddInParameter(objCMD, "@Month", DbType.Int16, objInsti.Subcription.Month);
                    objDB.AddOutParameter(objCMD, "@Status", DbType.Int16, 0);
                    objDB.ExecuteNonQuery(objCMD);
                    result = Convert.ToBoolean(objDB.GetParameterValue(objCMD, "@Status"));
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return result;
        }

        public CourseInstituteViewModel GetCourseInstituteDetails()
        {
            objDB = new SqlDatabase(appSettings.connectionString);
            List<CourseModel> lstCourse = new List<CourseModel>();
            List<InstituteModel> lstInstitute = new List<InstituteModel>();
            CourseInstituteViewModel objViewModel = new CourseInstituteViewModel();
            using (DbCommand objCMD = objDB.GetStoredProcCommand("spGetCourseIstitute"))
            {
                try
                {
                    DataSet ds = objDB.ExecuteDataSet(objCMD);

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            lstCourse.Add(new CourseModel
                            {
                                CourseId = Convert.ToInt32(ds.Tables[0].Rows[i]["CourseId"]),
                                Name = ds.Tables[0].Rows[i]["Name"].ToString()
                            });
                        }
                        objViewModel.CourseList = lstCourse;
                    }
                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                        {
                            lstInstitute.Add(new InstituteModel
                            {
                                InstituteId = Convert.ToInt32(ds.Tables[1].Rows[i]["InstituteId"]),
                                Name = ds.Tables[1].Rows[i]["Name"].ToString()
                            });
                        }
                        objViewModel.InstituteList = lstInstitute;
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return objViewModel;
        }

        public bool MapCourse(int courseid, int instituteid)
        {
            bool result = false;
            using (DbCommand objCMD = objDB.GetStoredProcCommand("spMapCourseInstitute"))
            {
                try
                {
                    objDB.AddInParameter(objCMD, "@CourseId", DbType.Int32, courseid);
                    objDB.AddInParameter(objCMD, "@InstituteId", DbType.Int32, instituteid);
                    objDB.AddOutParameter(objCMD, "@Status", DbType.Int16, 0);
                    objDB.ExecuteNonQuery(objCMD);
                    result = Convert.ToBoolean(objDB.GetParameterValue(objCMD, "@Status"));
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return result;
        }

    }
}
