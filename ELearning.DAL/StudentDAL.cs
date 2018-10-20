using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Text;
using ELearning.Model;
using ELearning.Utility;
using Microsoft.Extensions.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;

namespace ELearning.DAL
{
    public class StudentDAL
    {
        Database objDB;
        AppConfiguration appSettings;
        public StudentDAL(IConfiguration config)
        {
            appSettings = new AppConfiguration(config);
            objDB = new SqlDatabase(appSettings.connectionString);
        }

        public List<TestModel> GetUserTest(int userid, string moduleid, out int result)
        {
            objDB = new SqlDatabase(appSettings.connectionString);
            List<TestModel> lstTest = new List<TestModel>();
            using (DbCommand objCMD = objDB.GetStoredProcCommand("spGetUserTestList"))
            {
                try
                {
                    objDB.AddInParameter(objCMD, "@ModuleId", DbType.Int32, Convert.ToInt32(moduleid));
                    objDB.AddInParameter(objCMD, "@UserId", DbType.Int32, userid);
                    objDB.AddOutParameter(objCMD, "@Status", DbType.Int16, 0);
                    DataSet ds = objDB.ExecuteDataSet(objCMD);
                    result = Convert.ToInt16(objDB.GetParameterValue(objCMD, "@Status"));
                    if (result == 10)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                            {
                                lstTest.Add(new TestModel
                                {
                                    TestId = Convert.ToInt32(ds.Tables[0].Rows[i]["TestId"]),
                                    Question = Convert.ToString(ds.Tables[0].Rows[i]["Question"]),
                                    Image = Convert.ToString(ds.Tables[0].Rows[i]["TestImage"]) != string.Empty ? (byte[])ds.Tables[0].Rows[i]["TestImage"] : null,
                                    Module = new ModuleModel { Name = ds.Tables[0].Rows[i]["Name"].ToString() },
                                    OptionA = Convert.ToString(ds.Tables[0].Rows[i]["OptionA"]),
                                    OptionB = Convert.ToString(ds.Tables[0].Rows[i]["OptionB"]),
                                    OptionC = Convert.ToString(ds.Tables[0].Rows[i]["OptionC"]),
                                    OptionD = Convert.ToString(ds.Tables[0].Rows[i]["OptionD"]),
                                });
                            }
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

        public bool SaveUserTest(int userid, string[] answer, string[] testid, int moduleid, int total)
        {
            bool result = false;
            objDB = new SqlDatabase(appSettings.connectionString);
            using (DbCommand objCMD = objDB.GetStoredProcCommand("spSaveUserTest"))
            {
                try
                {
                    DataTable dt = new DataTable();
                    dt.Columns.Add(new DataColumn("answer", typeof(string)));
                    dt.Columns.Add(new DataColumn("testid", typeof(Int32)));

                    for (int i = 0; i < testid.Length - 1; i++)
                    {
                        DataRow r = dt.NewRow();
                        r["answer"] = answer[i];
                        r["testid"] = Convert.ToInt32(testid[i]);
                        dt.Rows.Add(r);
                    }

                    objDB.AddInParameter(objCMD, "@UserId", DbType.Int32, userid);

                    SqlParameter p = new SqlParameter("TestAnswer", dt);
                    p.SqlDbType = SqlDbType.Structured;
                    objCMD.Parameters.Add(p);

                    //objDB.AddInParameter(objCMD, "@TestAnswer", DbType.Structured, dt);
                    objDB.AddInParameter(objCMD, "@ModuleId", DbType.Int32, moduleid);
                    objDB.AddInParameter(objCMD, "@Total", DbType.Int32, total);
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

        public bool UpdateLessonStatus(int userid, string lessonid)
        {
            bool result = false;
            objDB = new SqlDatabase(appSettings.connectionString);
            using (DbCommand objCMD = objDB.GetStoredProcCommand("spUpdateLessonStatus"))
            {
                try
                {
                    objDB.AddInParameter(objCMD, "@UserId", DbType.Int32, userid);
                    objDB.AddInParameter(objCMD, "@LessonId", DbType.Int32, Convert.ToInt32(lessonid));
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

        public List<StudentViewModel> GetUserModules(int userid, string courseid)
        {
            List<StudentViewModel> lstModel = new List<StudentViewModel>();
            objDB = new SqlDatabase(appSettings.connectionString);

            using (DbCommand objCMD = objDB.GetStoredProcCommand("spGetUserModules"))
            {
                try
                {
                    objDB.AddInParameter(objCMD, "@UserId", DbType.Int32, userid);
                    objDB.AddInParameter(objCMD, "@CourseId", DbType.Int32, Convert.ToInt32(courseid));
                    DataSet ds = objDB.ExecuteDataSet(objCMD);

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            StudentViewModel objModel = new StudentViewModel();
                            if (ds.Tables[0].Rows[i]["ModuleId"] != null &&
                                ds.Tables[0].Rows[i]["Name"] != null
                                )
                            {
                                objModel.Module = new ModuleModel
                                {
                                    ModuleId = Convert.ToInt32((ds.Tables[0].Rows[i]["ModuleId"])),
                                    Name = Convert.ToString(ds.Tables[0].Rows[i]["Name"]),
                                    ModuleFile = Convert.ToString(ds.Tables[0].Rows[i]["ModuleFile"])
                                };
                            }

                            if (ds.Tables[0].Rows[i]["TotalLesson"] != null)
                                objModel.TotalLesson = Convert.ToInt32(ds.Tables[0].Rows[i]["TotalLesson"]);

                            if (ds.Tables[0].Rows[i]["TestCompleted"] != null)
                                objModel.TestCompleted = Convert.ToInt32(ds.Tables[0].Rows[i]["TestCompleted"]);

                            if (ds.Tables[0].Rows[i]["LessonCompleted"] != null)
                                objModel.LessonCompleted = Convert.ToInt32(ds.Tables[0].Rows[i]["LessonCompleted"]);

                            if (ds.Tables[0].Rows[i]["Attempt"] != null)
                                objModel.Attempt = Convert.ToInt32(ds.Tables[0].Rows[i]["Attempt"]);

                            lstModel.Add(objModel);
                        }
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return lstModel;
        }

        public List<StudentModel> GetTraningModule(int userid,int moduleid)
        {
            List<StudentModel> lstModel = new List<StudentModel>();
            objDB = new SqlDatabase(appSettings.connectionString);

            using (DbCommand objCMD = objDB.GetStoredProcCommand("spGetStudentTraining"))
            {
                try
                {
                    objDB.AddInParameter(objCMD, "@UserId", DbType.Int32, userid);
                    objDB.AddInParameter(objCMD, "@ModuleId", DbType.Int32, moduleid);
                    DataSet ds = objDB.ExecuteDataSet(objCMD);

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            StudentModel objModel = new StudentModel();
                            if (ds.Tables[0].Rows[i]["CourseId"] != null)
                                objModel.CourseId = Convert.ToInt32(ds.Tables[0].Rows[i]["CourseId"]);

                            if (ds.Tables[0].Rows[i]["ModuleId"] != null)
                                objModel.ModuleId = Convert.ToInt32(ds.Tables[0].Rows[i]["ModuleId"]);

                            if (ds.Tables[0].Rows[i]["CourseName"] != null)
                                objModel.CourseName = ds.Tables[0].Rows[i]["CourseName"].ToString();

                            if (ds.Tables[0].Rows[i]["ImagePath"] != null)
                                objModel.ImagePath = ds.Tables[0].Rows[i]["ImagePath"].ToString();

                            if (ds.Tables[0].Rows[i]["ModuleName"] != null)
                                objModel.ModuleName = ds.Tables[0].Rows[i]["ModuleName"].ToString();

                            if (ds.Tables[0].Rows[i]["LessonName"] != null)
                                objModel.LessonName = ds.Tables[0].Rows[i]["LessonName"].ToString();

                            if (ds.Tables[0].Rows[i]["LessonId"] != null)
                                objModel.LessonId = Convert.ToInt32(ds.Tables[0].Rows[i]["LessonId"]);

                            if (ds.Tables[0].Rows[i]["VideoName"] != null)
                                objModel.VideoName = ds.Tables[0].Rows[i]["VideoName"].ToString();

                            if (ds.Tables[0].Rows[i]["FilePath"] != null)
                                objModel.FilePath = ds.Tables[0].Rows[i]["FilePath"].ToString();

                            if (ds.Tables[0].Rows[i]["IsCompleted"] != null)
                                objModel.IsCompleted = Convert.ToBoolean(ds.Tables[0].Rows[i]["IsCompleted"]);

                            if (ds.Tables[0].Rows[i]["MarkSecure"] != null)
                                objModel.MarkSecure = Convert.ToInt32(ds.Tables[0].Rows[i]["MarkSecure"]);

                            if (ds.Tables[0].Rows[i]["MarkTotal"] != null)
                                objModel.MarkTotal = Convert.ToInt32(ds.Tables[0].Rows[i]["MarkTotal"]);

                            lstModel.Add(objModel);
                        }
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return lstModel;
        }



        public List<StudentModel> GetDashboard(int userid)
        {
            List<StudentModel> lstModel = new List<StudentModel>();
            objDB = new SqlDatabase(appSettings.connectionString);

            using (DbCommand objCMD = objDB.GetStoredProcCommand("spGetStudentDashboard"))
            {
                try
                {
                    objDB.AddInParameter(objCMD, "@UserId", DbType.Int32, userid);
                    DataSet ds = objDB.ExecuteDataSet(objCMD);

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            StudentModel objModel = new StudentModel();
                            if (ds.Tables[0].Rows[i]["CourseId"] != null)
                                objModel.CourseId = Convert.ToInt32(ds.Tables[0].Rows[i]["CourseId"]);

                            if (ds.Tables[0].Rows[i]["ModuleId"] != null)
                                objModel.ModuleId = Convert.ToInt32(ds.Tables[0].Rows[i]["ModuleId"]);

                            if (ds.Tables[0].Rows[i]["CourseName"] != null)
                                objModel.CourseName = ds.Tables[0].Rows[i]["CourseName"].ToString();

                            if (ds.Tables[0].Rows[i]["ImagePath"] != null)
                                objModel.ImagePath = ds.Tables[0].Rows[i]["ImagePath"].ToString();

                            if (ds.Tables[0].Rows[i]["ModuleName"] != null)
                                objModel.ModuleName = ds.Tables[0].Rows[i]["ModuleName"].ToString();

                            if (ds.Tables[0].Rows[i]["LessonName"] != null)
                                objModel.LessonName = ds.Tables[0].Rows[i]["LessonName"].ToString();

                            if (ds.Tables[0].Rows[i]["LessonId"] != null)
                                objModel.LessonId = Convert.ToInt32(ds.Tables[0].Rows[i]["LessonId"]);

                            if (ds.Tables[0].Rows[i]["VideoName"] != null)
                                objModel.VideoName = ds.Tables[0].Rows[i]["VideoName"].ToString();

                            if (ds.Tables[0].Rows[i]["FilePath"] != null)
                                objModel.FilePath = ds.Tables[0].Rows[i]["FilePath"].ToString();

                            if (ds.Tables[0].Rows[i]["IsCompleted"] != null)
                                objModel.IsCompleted = Convert.ToBoolean(ds.Tables[0].Rows[i]["IsCompleted"]);

                            if (ds.Tables[0].Rows[i]["MarkSecure"] != null)
                                objModel.MarkSecure = Convert.ToInt32(ds.Tables[0].Rows[i]["MarkSecure"]);

                            if (ds.Tables[0].Rows[i]["MarkTotal"] != null)
                                objModel.MarkTotal = Convert.ToInt32(ds.Tables[0].Rows[i]["MarkTotal"]);

                            lstModel.Add(objModel);
                        }
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return lstModel;
        }

        public List<StudentModel> GetDashboardMain(int userid)
        {
            List<StudentModel> lstModel = new List<StudentModel>();
            objDB = new SqlDatabase(appSettings.connectionString);

            using (DbCommand objCMD = objDB.GetStoredProcCommand("spGetStudentDashboard_Main"))
            {
                try
                {
                    objDB.AddInParameter(objCMD, "@UserId", DbType.Int32, userid);
                    DataSet ds = objDB.ExecuteDataSet(objCMD);

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            StudentModel objModel = new StudentModel();
                            if (ds.Tables[0].Rows[i]["CourseId"] != null)
                                objModel.CourseId = Convert.ToInt32(ds.Tables[0].Rows[i]["CourseId"]);

                            if (ds.Tables[0].Rows[i]["ModuleId"] != null)
                                objModel.ModuleId = Convert.ToInt32(ds.Tables[0].Rows[i]["ModuleId"]);

                            if (ds.Tables[0].Rows[i]["CourseName"] != null)
                                objModel.CourseName = ds.Tables[0].Rows[i]["CourseName"].ToString();

                            if (ds.Tables[0].Rows[i]["ImagePath"] != null)
                                objModel.ImagePath = ds.Tables[0].Rows[i]["ImagePath"].ToString();

                            if (ds.Tables[0].Rows[i]["ModuleName"] != null)
                                objModel.ModuleName = ds.Tables[0].Rows[i]["ModuleName"].ToString();

                            if (ds.Tables[0].Rows[i]["LessonName"] != null)
                                objModel.LessonName = ds.Tables[0].Rows[i]["LessonName"].ToString();

                            if (ds.Tables[0].Rows[i]["LessonId"] != null)
                                objModel.LessonId = Convert.ToInt32(ds.Tables[0].Rows[i]["LessonId"]);

                            //if (ds.Tables[0].Rows[i]["VideoName"] != null)
                            //    objModel.VideoName = ds.Tables[0].Rows[i]["VideoName"].ToString();

                            //if (ds.Tables[0].Rows[i]["FilePath"] != null)
                            //    objModel.FilePath = ds.Tables[0].Rows[i]["FilePath"].ToString();

                            if (ds.Tables[0].Rows[i]["LessonCompleted"] != null)
                                objModel.LessonCompleted = Convert.ToInt32(ds.Tables[0].Rows[i]["LessonCompleted"]);

                            if (ds.Tables[0].Rows[i]["MarkSecure"] != null)
                                objModel.MarkSecure = Convert.ToInt32(ds.Tables[0].Rows[i]["MarkSecure"]);

                            if (ds.Tables[0].Rows[i]["MarkTotal"] != null)
                                objModel.MarkTotal = Convert.ToInt32(ds.Tables[0].Rows[i]["MarkTotal"]);

                            lstModel.Add(objModel);
                        }
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return lstModel;
        }

        public int UpdateUser(UserModel objModel)
        {
            int result = 0;
            using (DbCommand objCMD = objDB.GetStoredProcCommand("spUpdateUserProfile"))
            {
                try
                {
                    objDB.AddInParameter(objCMD, "@UserId", DbType.Int32, objModel.UserId);
                    objDB.AddInParameter(objCMD, "@Password", DbType.String, objModel.Password);
                    objDB.AddInParameter(objCMD, "@Email", DbType.String, objModel.Email);
                    objDB.AddInParameter(objCMD, "@Name", DbType.String, objModel.Name);
                    objDB.AddOutParameter(objCMD, "@Status", DbType.Int16, 0);
                    objDB.ExecuteNonQuery(objCMD);
                    result = Convert.ToInt32(objDB.GetParameterValue(objCMD, "@Status"));
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return result;
        }

        public UserModel GetUserProfile(int userid)
        {
            UserModel objModel = new UserModel();
            objDB = new SqlDatabase(appSettings.connectionString);

            using (DbCommand objCMD = objDB.GetStoredProcCommand("spGetUserProfile"))
            {
                try
                {
                    objDB.AddInParameter(objCMD, "@UserId", DbType.Int32, userid);
                    DataSet ds = objDB.ExecuteDataSet(objCMD);

                    if (ds.Tables[0].Rows.Count > 0)
                    {

                        if (ds.Tables[0].Rows[0]["Name"] != null)
                            objModel.Name = ds.Tables[0].Rows[0]["Name"].ToString();

                        if (ds.Tables[0].Rows[0]["Email"] != null)
                            objModel.Email = ds.Tables[0].Rows[0]["Email"].ToString();
                        if (ds.Tables[0].Rows[0]["Password"] != null)
                            objModel.Password = ds.Tables[0].Rows[0]["Password"].ToString();
                    }
                }
                catch (Exception ex)
                {

                }
            }
            return objModel;
        }
    }
}
