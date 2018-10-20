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
    public class InstituteDAL
    {
        Database objDB;
        AppConfiguration appSettings;
        public InstituteDAL(IConfiguration config)
        {
            appSettings = new AppConfiguration(config);
            objDB = new SqlDatabase(appSettings.connectionString);
        }

        public List<InstituteModel> GetActivationCode(int userid)
        {
            List<InstituteModel> lstModel = new List<InstituteModel>();

            objDB = new SqlDatabase(appSettings.connectionString);

            using (DbCommand objCMD = objDB.GetStoredProcCommand("spGetActivationCode"))
            {
                try
                {
                    objDB.AddInParameter(objCMD, "@UserId", DbType.Int32, userid);
                    DataSet ds = objDB.ExecuteDataSet(objCMD);

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            InstituteModel objModel = new InstituteModel();
                            if (ds.Tables[0].Rows[i]["ActivationCode"] != null)
                                objModel.ActivationNumber = ds.Tables[0].Rows[i]["ActivationCode"].ToString();

                            if (ds.Tables[0].Rows[i]["IsActived"] != null)
                                objModel.IsActivated = Convert.ToBoolean(ds.Tables[0].Rows[i]["IsActived"]);

                            if (ds.Tables[0].Rows[i]["DateCreated"] != null)
                                objModel.DateCreated = Convert.ToDateTime(ds.Tables[0].Rows[i]["DateCreated"]);

                            if (ds.Tables[0].Rows[i]["DateExpiry"] != null)
                                objModel.DateExpired = Convert.ToDateTime(ds.Tables[0].Rows[i]["DateExpiry"]);

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

        public InstituteViewModel GetStudentRecords(int userid)
        {
            InstituteViewModel objViewModel = new InstituteViewModel();
            List<InstituteModel> lstModel = new List<InstituteModel>();
            InstituteDashboard objModel = new InstituteDashboard();
            objDB = new SqlDatabase(appSettings.connectionString);

            using (DbCommand objCMD = objDB.GetStoredProcCommand("spInstituteAdminDashboard"))
            {
                try
                {
                    objDB.AddInParameter(objCMD, "@UserId", DbType.Int32, userid);
                    DataSet ds = objDB.ExecuteDataSet(objCMD);

                    if (ds.Tables[0].Rows.Count > 0)
                    {

                        if (ds.Tables[0].Rows[0]["ActivatedCode"] != null)
                            objModel.ActivatedCode = Convert.ToInt32(ds.Tables[0].Rows[0]["ActivatedCode"]);

                        if (ds.Tables[0].Rows[0]["NonActivatedCode"] != null)
                            objModel.NonActivatedCode = Convert.ToInt32(ds.Tables[0].Rows[0]["NonActivatedCode"]);

                        if (ds.Tables[0].Rows[0]["TotalStudent"] != null)
                            objModel.TotalStudent = Convert.ToInt32(ds.Tables[0].Rows[0]["TotalStudent"]);

                        if (ds.Tables[0].Rows[0]["TotalCourses"] != null)
                            objModel.TotalCourses = Convert.ToInt32(ds.Tables[0].Rows[0]["TotalCourses"]);
                    }

                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                        {
                            InstituteModel objIModel = new InstituteModel();
                            if (ds.Tables[1].Rows[i]["Name"] != null)
                                objIModel.User = new UserModel
                                {
                                    UserId = Convert.ToInt32(ds.Tables[1].Rows[i]["UserId"]),
                                    Name = ds.Tables[1].Rows[i]["Name"].ToString()
                                };

                            if (ds.Tables[1].Rows[i]["MarkTotal"] != null)
                                objIModel.MarkTotal = Convert.ToInt32(ds.Tables[1].Rows[i]["MarkTotal"]);

                            if (ds.Tables[1].Rows[i]["MarkSecure"] != null)
                                objIModel.MarkSecure = Convert.ToInt32(ds.Tables[1].Rows[i]["MarkSecure"]);

                            if (ds.Tables[1].Rows[i]["AVGTotal"] != null)
                                objIModel.MarkAvg = Convert.ToInt32(ds.Tables[1].Rows[i]["AVGTotal"]);

                            if (ds.Tables[1].Rows[i]["Lesson"] != null && ds.Tables[1].Rows[i]["LessonCompleted"] != null)
                            {
                                objIModel.Lesson = new LessonModel
                                {
                                    Total = Convert.ToInt32(ds.Tables[1].Rows[i]["Lesson"]),
                                    Completed = Convert.ToInt32(ds.Tables[1].Rows[i]["LessonCompleted"])
                                };
                            }

                            if (ds.Tables[1].Rows[i]["ModuleName"] != null)
                                objIModel.Module = new ModuleModel { Name = ds.Tables[1].Rows[i]["ModuleName"].ToString() };

                            if (ds.Tables[1].Rows[i]["Attempt"] != null)
                                objIModel.Attempt = Convert.ToInt32(ds.Tables[1].Rows[i]["Attempt"]);

                            lstModel.Add(objIModel);
                        }
                    }

                    objViewModel.InstituteList = lstModel;
                    objViewModel.institute = objModel;
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return objViewModel;
        }

        public InstituteViewModel GetDashboard(int userid)
        {
            InstituteViewModel objViewModel = new InstituteViewModel();
            List<InstituteModel> lstModel = new List<InstituteModel>();
            InstituteDashboard objModel = new InstituteDashboard();
            objDB = new SqlDatabase(appSettings.connectionString);

            using (DbCommand objCMD = objDB.GetStoredProcCommand("spInstituteAdminDashboard_Main"))
            {
                try
                {
                    objDB.AddInParameter(objCMD, "@UserId", DbType.Int32, userid);
                    DataSet ds = objDB.ExecuteDataSet(objCMD);

                    if (ds.Tables[0].Rows.Count > 0)
                    {

                        if (ds.Tables[0].Rows[0]["ActivatedCode"] != null)
                            objModel.ActivatedCode = Convert.ToInt32(ds.Tables[0].Rows[0]["ActivatedCode"]);

                        if (ds.Tables[0].Rows[0]["NonActivatedCode"] != null)
                            objModel.NonActivatedCode = Convert.ToInt32(ds.Tables[0].Rows[0]["NonActivatedCode"]);

                        if (ds.Tables[0].Rows[0]["TotalStudent"] != null)
                            objModel.TotalStudent = Convert.ToInt32(ds.Tables[0].Rows[0]["TotalStudent"]);

                        if (ds.Tables[0].Rows[0]["TotalCourses"] != null)
                            objModel.TotalCourses = Convert.ToInt32(ds.Tables[0].Rows[0]["TotalCourses"]);
                        if (ds.Tables[0].Rows[0]["TestAttended"] != null)
                            objModel.TotalTestAttended = Convert.ToInt32(ds.Tables[0].Rows[0]["TestAttended"]);
                        if (ds.Tables[0].Rows[0]["TotalLesson"] != null)
                            objModel.TotalLesson = Convert.ToInt32(ds.Tables[0].Rows[0]["TotalLesson"]);
                    }

                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                        {
                            InstituteModel objIModel = new InstituteModel();
                            if (ds.Tables[1].Rows[i]["Name"] != null)
                                objIModel.User = new UserModel
                                {
                                    UserId = Convert.ToInt32(ds.Tables[1].Rows[i]["UserId"]),
                                    Name = ds.Tables[1].Rows[i]["Name"].ToString()
                                };

                            if (ds.Tables[1].Rows[i]["MarkTotal"] != null)
                                objIModel.MarkTotal = Convert.ToInt32(ds.Tables[1].Rows[i]["MarkTotal"]);

                            if (ds.Tables[1].Rows[i]["MarkSecure"] != null)
                                objIModel.MarkSecure = Convert.ToInt32(ds.Tables[1].Rows[i]["MarkSecure"]);

                            if (ds.Tables[1].Rows[i]["Lesson"] != null && ds.Tables[1].Rows[i]["LessonCompleted"] != null)
                            {
                                objIModel.Lesson = new LessonModel
                                {
                                    Total = Convert.ToInt32(ds.Tables[1].Rows[i]["Lesson"]),
                                    Completed = Convert.ToInt32(ds.Tables[1].Rows[i]["LessonCompleted"])
                                };
                            }

                            //if (ds.Tables[1].Rows[i]["ModuleName"] != null)
                            //    objIModel.Module = new ModuleModel { Name = ds.Tables[1].Rows[i]["ModuleName"].ToString() };
                            lstModel.Add(objIModel);
                        }
                    }

                    objViewModel.InstituteList = lstModel;
                    objViewModel.institute = objModel;
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return objViewModel;
        }
    }
}
