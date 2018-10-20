using System;
using System.Data;
using System.Data.Common;
using ELearning.Model;
using ELearning.Utility;
using Microsoft.Extensions.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;

namespace ELearning.DAL
{
    public class UserDAL
    {
        Database objDB;
        AppConfiguration appSettings;
        public UserDAL(IConfiguration config)
        {
            appSettings = new AppConfiguration(config);
            objDB = new SqlDatabase(appSettings.connectionString);
        }

        public UserModel RegisterStudent(UserModel objUser, out int result)
        {
            result = 0;
            using (DbCommand objCMD = objDB.GetStoredProcCommand("spRegisterStudent"))
            {
                objDB.AddInParameter(objCMD, "@Email", DbType.String, objUser.Email);
                objDB.AddInParameter(objCMD, "@Password", DbType.String, objUser.Password);
                objDB.AddInParameter(objCMD, "@ActivationCode", DbType.String, objUser.ActivationCode);
                objDB.AddInParameter(objCMD, "@Name", DbType.String, objUser.Name);
                objDB.AddOutParameter(objCMD, "@Status", DbType.Int16, 0);

                try
                {
                    DataSet ds = objDB.ExecuteDataSet(objCMD);

                    result = Convert.ToInt16(objDB.GetParameterValue(objCMD, "@Status"));

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        if (ds.Tables[0].Rows[0]["Name"] != null)
                        {
                            objUser.Name = ds.Tables[0].Rows[0]["Name"].ToString();
                        }
                        if (ds.Tables[0].Rows[0]["RoleId"] != null)
                        {
                            objUser.RoleId = Convert.ToInt16(ds.Tables[0].Rows[0]["RoleId"]);
                        }
                        if (ds.Tables[0].Rows[0]["Password"] != null)
                        {
                            objUser.Password = ds.Tables[0].Rows[0]["Password"].ToString();
                        }
                        if (ds.Tables[0].Rows[0]["UserId"] != null)
                        {
                            objUser.UserId = Convert.ToInt32(ds.Tables[0].Rows[0]["UserId"]);
                        }
                        objUser.IsAuthenticated = true;
                    }
                }
                catch (Exception)
                {
                    objUser.IsAuthenticated = false;
                    throw;
                }
            }
            return objUser;
        }

        public UserModel Forgotpassword(string email, out bool result)
        {
            UserModel objModel = new UserModel();
            result = false;
            using (DbCommand objCMD = objDB.GetStoredProcCommand("spForgotPassword"))
            {
                objDB.AddInParameter(objCMD, "@Email", DbType.String, email);

                try
                {
                    DataSet ds = objDB.ExecuteDataSet(objCMD);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        objModel.Password = ds.Tables[0].Rows[0]["Password"].ToString();
                        objModel.Name = ds.Tables[0].Rows[0]["Name"].ToString();
                        result = true;
                    }

                }
                catch (Exception)
                {
                    throw;
                }
            }
            return objModel;
        }

        public UserModel UserAuthentication(UserModel objUser)
        {
            using (DbCommand objCMD = objDB.GetStoredProcCommand("spUserAuth"))
            {
                objDB.AddInParameter(objCMD, "@Email", DbType.String, objUser.Email);
                objDB.AddInParameter(objCMD, "@Password", DbType.String, objUser.Password);
                try
                {
                    DataSet ds = objDB.ExecuteDataSet(objCMD);

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        if (ds.Tables[0].Rows[0]["Name"] != null)
                        {
                            objUser.Name = ds.Tables[0].Rows[0]["Name"].ToString();
                        }
                        if (ds.Tables[0].Rows[0]["RoleId"] != null)
                        {
                            objUser.RoleId = Convert.ToInt16(ds.Tables[0].Rows[0]["RoleId"]);
                        }
                        if (ds.Tables[0].Rows[0]["Password"] != null)
                        {
                            objUser.Password = ds.Tables[0].Rows[0]["Password"].ToString();
                        }
                        if (ds.Tables[0].Rows[0]["UserId"] != null)
                        {
                            objUser.UserId = Convert.ToInt32(ds.Tables[0].Rows[0]["UserId"]);
                        }
                        objUser.IsAuthenticated = true;
                    }
                }
                catch (Exception)
                {
                    objUser.IsAuthenticated = false;
                    throw;
                }
            }
            return objUser;
        }
    }
}
