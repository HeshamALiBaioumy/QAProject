using QA.Entities.Business_Entities;
using QA.Entities.Session_Entities;
using QA.Entities.View_Entities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OracleClient;
using System.Data.SqlClient;

namespace QA.Models
{
    public class Mdl_Dashboard
    {
        public Vw_Dashboard SupervisorEngDashboard(int userID)
        {
            Vw_Dashboard viewResult = new Vw_Dashboard();

            //try
            //{
            //    using (SqlConnection dbConnection = new SqlConnection(
            //        ConfigurationManager.ConnectionStrings["dbConnectionString"].ConnectionString))
            //    using (SqlCommand dbCommand = dbConnection.CreateCommand())
            //    {
            //        dbCommand.CommandText = "SP_Dashboard_SupervisorEng";
            //        dbCommand.CommandType = CommandType.StoredProcedure;
            //        dbCommand.Parameters.Add("I_UserID", SqlDbType.NVarChar).Value = userID;

            //        dbCommand.Parameters.Add("O_Projects_Total", SqlDbType.Int).Direction = ParameterDirection.Output;
            //        dbCommand.Parameters.Add("O_Projects_New_Total", SqlDbType.Int).Direction = ParameterDirection.Output;

            //        dbCommand.Parameters.Add("O_Complaints_Total", SqlDbType.Int).Direction = ParameterDirection.Output;
            //        dbCommand.Parameters.Add("O_Complaints_New_Total", SqlDbType.Int).Direction = ParameterDirection.Output;
            //        dbCommand.Parameters.Add("O_Complaints_Closed_Total", SqlDbType.Int).Direction = ParameterDirection.Output;

            //        dbCommand.Parameters.Add("O_CR_Total", SqlDbType.Int).Direction = ParameterDirection.Output;
            //        dbCommand.Parameters.Add("O_CR_New", SqlDbType.Int).Direction = ParameterDirection.Output;
            //        dbCommand.Parameters.Add("O_CR_Pending", SqlDbType.Int).Direction = ParameterDirection.Output;
            //        dbCommand.Parameters.Add("O_CR_New_Pending", SqlDbType.Int).Direction = ParameterDirection.Output;
            //        dbCommand.Parameters.Add("O_CR_Recieved_Pending", SqlDbType.Int).Direction = ParameterDirection.Output;
            //        dbCommand.Parameters.Add("O_CR_New_Recieved_Pending", SqlDbType.Int).Direction = ParameterDirection.Output;
            //        dbCommand.Parameters.Add("O_CR_Accepted", SqlDbType.Int).Direction = ParameterDirection.Output;
            //        dbCommand.Parameters.Add("O_CR_New_Accepted", SqlDbType.Int).Direction = ParameterDirection.Output;
            //        dbCommand.Parameters.Add("O_CR_Rejected", SqlDbType.Int).Direction = ParameterDirection.Output;
            //        dbCommand.Parameters.Add("O_CR_New_Rejected", SqlDbType.Int).Direction = ParameterDirection.Output;

            //        dbCommand.Parameters.Add("O_CL_Total_Count", SqlDbType.Int).Direction = ParameterDirection.Output;
            //        dbCommand.Parameters.Add("O_CL_New", SqlDbType.Int).Direction = ParameterDirection.Output;
            //        dbCommand.Parameters.Add("O_CL_Total_Done_Maker", SqlDbType.Int).Direction = ParameterDirection.Output;
            //        dbCommand.Parameters.Add("O_CL_Total_Pending_Maker", SqlDbType.Int).Direction = ParameterDirection.Output;
            //        dbCommand.Parameters.Add("O_CL_Total_New_Pending_Maker", SqlDbType.Int).Direction = ParameterDirection.Output;
            //        dbCommand.Parameters.Add("O_CL_Total_Done_Checker", SqlDbType.Int).Direction = ParameterDirection.Output;
            //        dbCommand.Parameters.Add("O_CL_Total_Pending_Checker", SqlDbType.Int).Direction = ParameterDirection.Output;
            //        dbCommand.Parameters.Add("O_CL_Total_New_Pending_Checker", SqlDbType.Int).Direction = ParameterDirection.Output;
            //        dbCommand.Parameters.Add("O_CL_Total_Accepted", SqlDbType.Int).Direction = ParameterDirection.Output;
            //        dbCommand.Parameters.Add("O_CL_Total_Rejected", SqlDbType.Int).Direction = ParameterDirection.Output;
            //        dbCommand.Parameters.Add("O_CL_Total_Closed", SqlDbType.Int).Direction = ParameterDirection.Output;

            //        dbCommand.Parameters.Add("O_RCV_Total_Count", SqlDbType.Int).Direction = ParameterDirection.Output;
            //        dbCommand.Parameters.Add("O_RCV_Total_New", SqlDbType.Int).Direction = ParameterDirection.Output;
            //        dbCommand.Parameters.Add("O_RCV_Total_Match", SqlDbType.Int).Direction = ParameterDirection.Output;
            //        dbCommand.Parameters.Add("O_RCV_Total_MissMatch", SqlDbType.Int).Direction = ParameterDirection.Output;
            //        dbCommand.Parameters.Add("O_RCV_Total_Pending", SqlDbType.Int).Direction = ParameterDirection.Output;
            //        dbCommand.Parameters.Add("O_RCV_Total_New_Pending", SqlDbType.Int).Direction = ParameterDirection.Output;
                    
            //        dbCommand.Parameters.Add("O_RCVMM_Total_Count", SqlDbType.Int).Direction = ParameterDirection.Output;
            //        dbCommand.Parameters.Add("O_RCVMM_Total_New", SqlDbType.Int).Direction = ParameterDirection.Output;
            //        dbCommand.Parameters.Add("O_RCVMM_Total_Pending", SqlDbType.Int).Direction = ParameterDirection.Output;
            //        dbCommand.Parameters.Add("O_RCVMM_Total_New_Pending", SqlDbType.Int).Direction = ParameterDirection.Output;
            //        dbCommand.Parameters.Add("O_RCVMM_Total_Fixed", SqlDbType.Int).Direction = ParameterDirection.Output;
            //        dbCommand.Parameters.Add("O_RCVMM_Total_Closed", SqlDbType.Int).Direction = ParameterDirection.Output;

            //        dbCommand.Parameters.Add("searchResultCursor", SqlDbType.Int).Direction = ParameterDirection.Output;

            //        dbConnection.Open();
            //        SqlDataReader reader = dbCommand.ExecuteReader(CommandBehavior.CloseConnection);

            //        viewResult.dashboard = new Ent_Dashboard()
            //        {
            //            Projects_Total = int.Parse(dbCommand.Parameters["O_Projects_Total"].Value.ToString()),
            //            Projects_New = int.Parse(dbCommand.Parameters["O_Projects_New_Total"].Value.ToString()),

            //            compliants_Total = int.Parse(dbCommand.Parameters["O_Complaints_Total"].Value.ToString()),
            //            compliants_New = int.Parse(dbCommand.Parameters["O_Complaints_New_Total"].Value.ToString()),
            //            compliants_Closed = int.Parse(dbCommand.Parameters["O_Complaints_Closed_Total"].Value.ToString()),

            //            CR_Total = int.Parse(dbCommand.Parameters["O_CR_Total"].Value.ToString()),
            //            CR_New = int.Parse(dbCommand.Parameters["O_CR_New"].Value.ToString()),
            //            CR_Pending_Total = int.Parse(dbCommand.Parameters["O_CR_Pending"].Value.ToString()),
            //            CR_Pending_New = int.Parse(dbCommand.Parameters["O_CR_New_Pending"].Value.ToString()),
            //            CR_Recieved_Pending_Total = int.Parse(dbCommand.Parameters["O_CR_Recieved_Pending"].Value.ToString()),
            //            CR_Recieved_Pending_New = int.Parse(dbCommand.Parameters["O_CR_New_Recieved_Pending"].Value.ToString()),
            //            CR_Accepted_Total = int.Parse(dbCommand.Parameters["O_CR_Accepted"].Value.ToString()),
            //            CR_Accepted_New = int.Parse(dbCommand.Parameters["O_CR_New_Accepted"].Value.ToString()),
            //            CR_Rejected_Total = int.Parse(dbCommand.Parameters["O_CR_Rejected"].Value.ToString()),
            //            CR_Rejected_New = int.Parse(dbCommand.Parameters["O_CR_New_Rejected"].Value.ToString()),

            //            CL_Total = int.Parse(dbCommand.Parameters["O_CL_Total_Count"].Value.ToString()),
            //            CL_New = int.Parse(dbCommand.Parameters["O_CL_New"].Value.ToString()),
            //            CL_Done_Maker = int.Parse(dbCommand.Parameters["O_CL_Total_Done_Maker"].Value.ToString()),
            //            CL_Pending_Maker = int.Parse(dbCommand.Parameters["O_CL_Total_Pending_Maker"].Value.ToString()),
            //            CL_Pending_New_Maker = int.Parse(dbCommand.Parameters["O_CL_Total_New_Pending_Maker"].Value.ToString()),
            //            CL_Done_Cheker = int.Parse(dbCommand.Parameters["O_CL_Total_Done_Checker"].Value.ToString()),
            //            CL_Pending_Cheker = int.Parse(dbCommand.Parameters["O_CL_Total_Pending_Checker"].Value.ToString()),
            //            CL_Pending_New_Cheker = int.Parse(dbCommand.Parameters["O_CL_Total_New_Pending_Checker"].Value.ToString()),
            //            CL_Accepted_Total = int.Parse(dbCommand.Parameters["O_CL_Total_Accepted"].Value.ToString()),
            //            CL_Rejected_Total = int.Parse(dbCommand.Parameters["O_CL_Total_Rejected"].Value.ToString()),
            //            CL_Closed_Total = int.Parse(dbCommand.Parameters["O_CL_Total_Closed"].Value.ToString()),

            //            RCV_Total = int.Parse(dbCommand.Parameters["O_RCV_Total_Count"].Value.ToString()),
            //            RCV_New = int.Parse(dbCommand.Parameters["O_RCV_Total_New"].Value.ToString()),
            //            RCV_Match = int.Parse(dbCommand.Parameters["O_RCV_Total_Match"].Value.ToString()),
            //            RCV_MissMatch = int.Parse(dbCommand.Parameters["O_RCV_Total_MissMatch"].Value.ToString()),
            //            RCV_Pending = int.Parse(dbCommand.Parameters["O_RCV_Total_Pending"].Value.ToString()),
            //            RCV_New_Pending = int.Parse(dbCommand.Parameters["O_RCV_Total_New_Pending"].Value.ToString()),

            //            MMC_Total = int.Parse(dbCommand.Parameters["O_RCVMM_Total_Count"].Value.ToString()),
            //            MMC_New = int.Parse(dbCommand.Parameters["O_RCVMM_Total_New"].Value.ToString()),
            //            MMC_Pending = int.Parse(dbCommand.Parameters["O_RCVMM_Total_Pending"].Value.ToString()),
            //            MMC_New_Pending = int.Parse(dbCommand.Parameters["O_RCVMM_Total_New_Pending"].Value.ToString()),
            //            MMC_Fixed = int.Parse(dbCommand.Parameters["O_RCVMM_Total_Fixed"].Value.ToString()),
            //            MMC_Closed = int.Parse(dbCommand.Parameters["O_RCVMM_Total_Closed"].Value.ToString())
            //        };

            //        viewResult.dashboard.lstOverAllDashboard = new List<List<string>>();

            //        while (reader.Read())
            //        {
            //            viewResult.dashboard.lstOverAllDashboard.Add(new List<string>()
            //            {
            //                reader.GetString(reader.GetOrdinal("STATUSTYPE")),
            //                reader.GetString(reader.GetOrdinal("S1")),
            //                reader.GetString(reader.GetOrdinal("S2")),
            //                reader.GetString(reader.GetOrdinal("S3")),
            //                reader.GetString(reader.GetOrdinal("S4")),
            //                reader.GetString(reader.GetOrdinal("S5")),
            //                reader.GetString(reader.GetOrdinal("S6")),
            //                reader.GetString(reader.GetOrdinal("S7")),
            //                reader.GetString(reader.GetOrdinal("S8")),
            //                reader.GetString(reader.GetOrdinal("S9"))
            //            });
            //        }

            //        int saturdayIndex = viewResult.dashboard.lstOverAllDashboard[0].IndexOf(
            //            ConfigurationManager.AppSettings["Dashboard_Saturday_Name"].ToString());
            //        foreach (List<string> lst in viewResult.dashboard.lstOverAllDashboard)
            //        {
            //            lst.RemoveRange(saturdayIndex, 2);
            //        }

            //        dbCommand.Dispose();
            //        dbConnection.Close();

            //        viewResult.response = new ResponseMessage()
            //        {
            //            responseStatus = true
            //        };
            //    }
            //}
            //catch (Exception ex)
            //{
            //    viewResult.response = new ResponseMessage()
            //    {
            //        responseStatus = false,
            //        errorMessage = ex.Message,
            //        endUserMessage = Localization.Global.UnhandledErrorOccured
            //    };
            //}

            return viewResult;
        }

        public Vw_Dashboard ConsultantEngDashboard(int userID)
        {
            Vw_Dashboard viewResult = new Vw_Dashboard();

            try
            {
                using (SqlConnection dbConnection = new SqlConnection(
                    ConfigurationManager.ConnectionStrings["dbConnectionString"].ConnectionString))
                using (SqlCommand dbCommand = dbConnection.CreateCommand())
                {
                    dbCommand.CommandText = "SP_Dashboard_ConsultantEng";
                    dbCommand.CommandType = CommandType.StoredProcedure;
                    dbCommand.Parameters.Add("I_UserID", SqlDbType.NVarChar).Value = userID;

                    dbCommand.Parameters.Add("O_Projects_Total", SqlDbType.Int).Direction = ParameterDirection.Output;
                    dbCommand.Parameters.Add("O_Projects_New_Total", SqlDbType.Int).Direction = ParameterDirection.Output;

                    dbCommand.Parameters.Add("O_Complaints_Total", SqlDbType.Int).Direction = ParameterDirection.Output;
                    dbCommand.Parameters.Add("O_Complaints_New_Total", SqlDbType.Int).Direction = ParameterDirection.Output;
                    dbCommand.Parameters.Add("O_Complaints_Closed_Total", SqlDbType.Int).Direction = ParameterDirection.Output;

                    dbCommand.Parameters.Add("O_CR_Total", SqlDbType.Int).Direction = ParameterDirection.Output;
                    dbCommand.Parameters.Add("O_CR_New", SqlDbType.Int).Direction = ParameterDirection.Output;
                    dbCommand.Parameters.Add("O_CR_Pending", SqlDbType.Int).Direction = ParameterDirection.Output;
                    dbCommand.Parameters.Add("O_CR_New_Pending", SqlDbType.Int).Direction = ParameterDirection.Output;
                    dbCommand.Parameters.Add("O_CR_Recieved_Pending", SqlDbType.Int).Direction = ParameterDirection.Output;
                    dbCommand.Parameters.Add("O_CR_New_Recieved_Pending", SqlDbType.Int).Direction = ParameterDirection.Output;
                    dbCommand.Parameters.Add("O_CR_Accepted", SqlDbType.Int).Direction = ParameterDirection.Output;
                    dbCommand.Parameters.Add("O_CR_New_Accepted", SqlDbType.Int).Direction = ParameterDirection.Output;
                    dbCommand.Parameters.Add("O_CR_Rejected", SqlDbType.Int).Direction = ParameterDirection.Output;
                    dbCommand.Parameters.Add("O_CR_New_Rejected", SqlDbType.Int).Direction = ParameterDirection.Output;

                    dbCommand.Parameters.Add("O_CL_Total_Count", SqlDbType.Int).Direction = ParameterDirection.Output;
                    dbCommand.Parameters.Add("O_CL_New", SqlDbType.Int).Direction = ParameterDirection.Output;
                    dbCommand.Parameters.Add("O_CL_Total_Done_Maker", SqlDbType.Int).Direction = ParameterDirection.Output;
                    dbCommand.Parameters.Add("O_CL_Total_Pending_Maker", SqlDbType.Int).Direction = ParameterDirection.Output;
                    dbCommand.Parameters.Add("O_CL_Total_New_Pending_Maker", SqlDbType.Int).Direction = ParameterDirection.Output;
                    dbCommand.Parameters.Add("O_CL_Total_Done_Checker", SqlDbType.Int).Direction = ParameterDirection.Output;
                    dbCommand.Parameters.Add("O_CL_Total_Pending_Checker", SqlDbType.Int).Direction = ParameterDirection.Output;
                    dbCommand.Parameters.Add("O_CL_Total_New_Pending_Checker", SqlDbType.Int).Direction = ParameterDirection.Output;
                    dbCommand.Parameters.Add("O_CL_Total_Accepted", SqlDbType.Int).Direction = ParameterDirection.Output;
                    dbCommand.Parameters.Add("O_CL_Total_Rejected", SqlDbType.Int).Direction = ParameterDirection.Output;
                    dbCommand.Parameters.Add("O_CL_Total_Closed", SqlDbType.Int).Direction = ParameterDirection.Output;

                    dbCommand.Parameters.Add("searchResultCursor", SqlDbType.Int).Direction = ParameterDirection.Output;

                    dbConnection.Open();
                    SqlDataReader reader = dbCommand.ExecuteReader(CommandBehavior.CloseConnection);

                    viewResult.dashboard = new Ent_Dashboard()
                    {
                        Projects_Total = int.Parse(dbCommand.Parameters["O_Projects_Total"].Value.ToString()),
                        Projects_New = int.Parse(dbCommand.Parameters["O_Projects_New_Total"].Value.ToString()),

                        compliants_Total = int.Parse(dbCommand.Parameters["O_Complaints_Total"].Value.ToString()),
                        compliants_New = int.Parse(dbCommand.Parameters["O_Complaints_New_Total"].Value.ToString()),
                        compliants_Closed = int.Parse(dbCommand.Parameters["O_Complaints_Closed_Total"].Value.ToString()),

                        CR_Total = int.Parse(dbCommand.Parameters["O_CR_Total"].Value.ToString()),
                        CR_New = int.Parse(dbCommand.Parameters["O_CR_New"].Value.ToString()),
                        CR_Pending_Total = int.Parse(dbCommand.Parameters["O_CR_Pending"].Value.ToString()),
                        CR_Pending_New = int.Parse(dbCommand.Parameters["O_CR_New_Pending"].Value.ToString()),
                        CR_Recieved_Pending_Total = int.Parse(dbCommand.Parameters["O_CR_Recieved_Pending"].Value.ToString()),
                        CR_Recieved_Pending_New = int.Parse(dbCommand.Parameters["O_CR_New_Recieved_Pending"].Value.ToString()),
                        CR_Accepted_Total = int.Parse(dbCommand.Parameters["O_CR_Accepted"].Value.ToString()),
                        CR_Accepted_New = int.Parse(dbCommand.Parameters["O_CR_New_Accepted"].Value.ToString()),
                        CR_Rejected_Total = int.Parse(dbCommand.Parameters["O_CR_Rejected"].Value.ToString()),
                        CR_Rejected_New = int.Parse(dbCommand.Parameters["O_CR_New_Rejected"].Value.ToString()),

                        CL_Total = int.Parse(dbCommand.Parameters["O_CL_Total_Count"].Value.ToString()),
                        CL_New = int.Parse(dbCommand.Parameters["O_CL_New"].Value.ToString()),
                        CL_Done_Maker = int.Parse(dbCommand.Parameters["O_CL_Total_Done_Maker"].Value.ToString()),
                        CL_Pending_Maker = int.Parse(dbCommand.Parameters["O_CL_Total_Pending_Maker"].Value.ToString()),
                        CL_Pending_New_Maker = int.Parse(dbCommand.Parameters["O_CL_Total_New_Pending_Maker"].Value.ToString()),
                        CL_Done_Cheker = int.Parse(dbCommand.Parameters["O_CL_Total_Done_Checker"].Value.ToString()),
                        CL_Pending_Cheker = int.Parse(dbCommand.Parameters["O_CL_Total_Pending_Checker"].Value.ToString()),
                        CL_Pending_New_Cheker = int.Parse(dbCommand.Parameters["O_CL_Total_New_Pending_Checker"].Value.ToString()),
                        CL_Accepted_Total = int.Parse(dbCommand.Parameters["O_CL_Total_Accepted"].Value.ToString()),
                        CL_Rejected_Total = int.Parse(dbCommand.Parameters["O_CL_Total_Rejected"].Value.ToString()),
                        CL_Closed_Total = int.Parse(dbCommand.Parameters["O_CL_Total_Closed"].Value.ToString()),
                    };

                    viewResult.dashboard.lstOverAllDashboard = new List<List<string>>();

                    while (reader.Read())
                    {
                        viewResult.dashboard.lstOverAllDashboard.Add(new List<string>()
                        {
                            reader.GetString(reader.GetOrdinal("STATUSTYPE")),
                            reader.GetString(reader.GetOrdinal("S1")),
                            reader.GetString(reader.GetOrdinal("S2")),
                            reader.GetString(reader.GetOrdinal("S3")),
                            reader.GetString(reader.GetOrdinal("S4")),
                            reader.GetString(reader.GetOrdinal("S5")),
                            reader.GetString(reader.GetOrdinal("S6")),
                            reader.GetString(reader.GetOrdinal("S7")),
                            reader.GetString(reader.GetOrdinal("S8")),
                            reader.GetString(reader.GetOrdinal("S9"))
                        });
                    }

                    int saturdayIndex = viewResult.dashboard.lstOverAllDashboard[0].IndexOf(
                        ConfigurationManager.AppSettings["Dashboard_Saturday_Name"].ToString());
                    foreach (List<string> lst in viewResult.dashboard.lstOverAllDashboard)
                    {
                        lst.RemoveRange(saturdayIndex, 2);
                    }

                    dbCommand.Dispose();
                    dbConnection.Close();

                    viewResult.response = new ResponseMessage()
                    {
                        responseStatus = true
                    };
                }
            }
            catch (Exception ex)
            {
                viewResult.response = new ResponseMessage()
                {
                    responseStatus = false,
                    errorMessage = ex.Message,
                    endUserMessage = Localization.Global.UnhandledErrorOccured
                };
            }

            return viewResult;
        }

        public Vw_Dashboard AuthLabDashboard(int userID)
        {
            Vw_Dashboard viewResult = new Vw_Dashboard();

            try
            {
                using (SqlConnection dbConnection = new SqlConnection(
                    ConfigurationManager.ConnectionStrings["dbConnectionString"].ConnectionString))
                using (SqlCommand dbCommand = dbConnection.CreateCommand())
                {
                    dbCommand.CommandText = "SP_Dashboard_AuthLab";
                    dbCommand.CommandType = CommandType.StoredProcedure;
                    dbCommand.Parameters.Add("I_UserID", SqlDbType.NVarChar).Value = userID;

                    dbCommand.Parameters.Add("O_Projects_Total", SqlDbType.Int).Direction = ParameterDirection.Output;
                    dbCommand.Parameters.Add("O_Projects_New_Total", SqlDbType.Int).Direction = ParameterDirection.Output;

                    dbCommand.Parameters.Add("O_Complaints_Total", SqlDbType.Int).Direction = ParameterDirection.Output;
                    dbCommand.Parameters.Add("O_Complaints_New_Total", SqlDbType.Int).Direction = ParameterDirection.Output;
                    dbCommand.Parameters.Add("O_Complaints_Closed_Total", SqlDbType.Int).Direction = ParameterDirection.Output;

                    dbCommand.Parameters.Add("O_CR_Total", SqlDbType.Int).Direction = ParameterDirection.Output;
                    dbCommand.Parameters.Add("O_CR_New", SqlDbType.Int).Direction = ParameterDirection.Output;
                    dbCommand.Parameters.Add("O_CR_Pending", SqlDbType.Int).Direction = ParameterDirection.Output;
                    dbCommand.Parameters.Add("O_CR_New_Pending", SqlDbType.Int).Direction = ParameterDirection.Output;
                    dbCommand.Parameters.Add("O_CR_Recieved_Pending", SqlDbType.Int).Direction = ParameterDirection.Output;
                    dbCommand.Parameters.Add("O_CR_New_Recieved_Pending", SqlDbType.Int).Direction = ParameterDirection.Output;
                    dbCommand.Parameters.Add("O_CR_Accepted", SqlDbType.Int).Direction = ParameterDirection.Output;
                    dbCommand.Parameters.Add("O_CR_New_Accepted", SqlDbType.Int).Direction = ParameterDirection.Output;
                    dbCommand.Parameters.Add("O_CR_Rejected", SqlDbType.Int).Direction = ParameterDirection.Output;
                    dbCommand.Parameters.Add("O_CR_New_Rejected", SqlDbType.Int).Direction = ParameterDirection.Output;

                    dbCommand.Parameters.Add("O_CL_Total_Count", SqlDbType.Int).Direction = ParameterDirection.Output;
                    dbCommand.Parameters.Add("O_CL_New", SqlDbType.Int).Direction = ParameterDirection.Output;
                    dbCommand.Parameters.Add("O_CL_Total_Done_Maker", SqlDbType.Int).Direction = ParameterDirection.Output;
                    dbCommand.Parameters.Add("O_CL_Total_Pending_Maker", SqlDbType.Int).Direction = ParameterDirection.Output;
                    dbCommand.Parameters.Add("O_CL_Total_New_Pending_Maker", SqlDbType.Int).Direction = ParameterDirection.Output;
                    dbCommand.Parameters.Add("O_CL_Total_Done_Checker", SqlDbType.Int).Direction = ParameterDirection.Output;
                    dbCommand.Parameters.Add("O_CL_Total_Pending_Checker", SqlDbType.Int).Direction = ParameterDirection.Output;
                    dbCommand.Parameters.Add("O_CL_Total_New_Pending_Checker", SqlDbType.Int).Direction = ParameterDirection.Output;
                    dbCommand.Parameters.Add("O_CL_Total_Accepted", SqlDbType.Int).Direction = ParameterDirection.Output;
                    dbCommand.Parameters.Add("O_CL_Total_Rejected", SqlDbType.Int).Direction = ParameterDirection.Output;
                    dbCommand.Parameters.Add("O_CL_Total_Closed", SqlDbType.Int).Direction = ParameterDirection.Output;

                    dbCommand.Parameters.Add("searchResultCursor", SqlDbType.Int).Direction = ParameterDirection.Output;

                    dbConnection.Open();
                    SqlDataReader reader = dbCommand.ExecuteReader(CommandBehavior.CloseConnection);

                    viewResult.dashboard = new Ent_Dashboard()
                    {
                        Projects_Total = int.Parse(dbCommand.Parameters["O_Projects_Total"].Value.ToString()),
                        Projects_New = int.Parse(dbCommand.Parameters["O_Projects_New_Total"].Value.ToString()),

                        compliants_Total = int.Parse(dbCommand.Parameters["O_Complaints_Total"].Value.ToString()),
                        compliants_New = int.Parse(dbCommand.Parameters["O_Complaints_New_Total"].Value.ToString()),
                        compliants_Closed = int.Parse(dbCommand.Parameters["O_Complaints_Closed_Total"].Value.ToString()),

                        CR_Total = int.Parse(dbCommand.Parameters["O_CR_Total"].Value.ToString()),
                        CR_New = int.Parse(dbCommand.Parameters["O_CR_New"].Value.ToString()),
                        CR_Pending_Total = int.Parse(dbCommand.Parameters["O_CR_Pending"].Value.ToString()),
                        CR_Pending_New = int.Parse(dbCommand.Parameters["O_CR_New_Pending"].Value.ToString()),
                        CR_Recieved_Pending_Total = int.Parse(dbCommand.Parameters["O_CR_Recieved_Pending"].Value.ToString()),
                        CR_Recieved_Pending_New = int.Parse(dbCommand.Parameters["O_CR_New_Recieved_Pending"].Value.ToString()),
                        CR_Accepted_Total = int.Parse(dbCommand.Parameters["O_CR_Accepted"].Value.ToString()),
                        CR_Accepted_New = int.Parse(dbCommand.Parameters["O_CR_New_Accepted"].Value.ToString()),
                        CR_Rejected_Total = int.Parse(dbCommand.Parameters["O_CR_Rejected"].Value.ToString()),
                        CR_Rejected_New = int.Parse(dbCommand.Parameters["O_CR_New_Rejected"].Value.ToString()),

                        CL_Total = int.Parse(dbCommand.Parameters["O_CL_Total_Count"].Value.ToString()),
                        CL_New = int.Parse(dbCommand.Parameters["O_CL_New"].Value.ToString()),
                        CL_Done_Maker = int.Parse(dbCommand.Parameters["O_CL_Total_Done_Maker"].Value.ToString()),
                        CL_Pending_Maker = int.Parse(dbCommand.Parameters["O_CL_Total_Pending_Maker"].Value.ToString()),
                        CL_Pending_New_Maker = int.Parse(dbCommand.Parameters["O_CL_Total_New_Pending_Maker"].Value.ToString()),
                        CL_Done_Cheker = int.Parse(dbCommand.Parameters["O_CL_Total_Done_Checker"].Value.ToString()),
                        CL_Pending_Cheker = int.Parse(dbCommand.Parameters["O_CL_Total_Pending_Checker"].Value.ToString()),
                        CL_Pending_New_Cheker = int.Parse(dbCommand.Parameters["O_CL_Total_New_Pending_Checker"].Value.ToString()),
                        CL_Accepted_Total = int.Parse(dbCommand.Parameters["O_CL_Total_Accepted"].Value.ToString()),
                        CL_Rejected_Total = int.Parse(dbCommand.Parameters["O_CL_Total_Rejected"].Value.ToString()),
                        CL_Closed_Total = int.Parse(dbCommand.Parameters["O_CL_Total_Closed"].Value.ToString()),
                    };

                    viewResult.dashboard.lstOverAllDashboard = new List<List<string>>();

                    while (reader.Read())
                    {
                        viewResult.dashboard.lstOverAllDashboard.Add(new List<string>()
                        {
                            reader.GetString(reader.GetOrdinal("STATUSTYPE")),
                            reader.GetString(reader.GetOrdinal("S1")),
                            reader.GetString(reader.GetOrdinal("S2")),
                            reader.GetString(reader.GetOrdinal("S3")),
                            reader.GetString(reader.GetOrdinal("S4")),
                            reader.GetString(reader.GetOrdinal("S5")),
                            reader.GetString(reader.GetOrdinal("S6")),
                            reader.GetString(reader.GetOrdinal("S7")),
                            reader.GetString(reader.GetOrdinal("S8")),
                            reader.GetString(reader.GetOrdinal("S9"))
                        });
                    }

                    int saturdayIndex = viewResult.dashboard.lstOverAllDashboard[0].IndexOf(
                        ConfigurationManager.AppSettings["Dashboard_Saturday_Name"].ToString());
                    foreach (List<string> lst in viewResult.dashboard.lstOverAllDashboard)
                    {
                        lst.RemoveRange(saturdayIndex, 2);
                    }

                    dbCommand.Dispose();
                    dbConnection.Close();

                    viewResult.response = new ResponseMessage()
                    {
                        responseStatus = true
                    };
                }
            }
            catch (Exception ex)
            {
                viewResult.response = new ResponseMessage()
                {
                    responseStatus = false,
                    errorMessage = ex.Message,
                    endUserMessage = Localization.Global.UnhandledErrorOccured
                };
            }

            return viewResult;
        }

        public Vw_Dashboard TechnicianDashboard(int userID)
        {
            Vw_Dashboard viewResult = new Vw_Dashboard();

            try
            {
                using (SqlConnection dbConnection = new SqlConnection(
                    ConfigurationManager.ConnectionStrings["dbConnectionString"].ConnectionString))
                using (SqlCommand dbCommand = dbConnection.CreateCommand())
                {
                    dbCommand.CommandText = "SP_Dashboard_Technician";
                    dbCommand.CommandType = CommandType.StoredProcedure;
                    dbCommand.Parameters.Add("I_UserID", SqlDbType.NVarChar).Value = userID;

                    dbCommand.Parameters.Add("O_Projects_Total", SqlDbType.Int).Direction = ParameterDirection.Output;
                    dbCommand.Parameters.Add("O_Projects_New_Total", SqlDbType.Int).Direction = ParameterDirection.Output;

                    dbCommand.Parameters.Add("O_Complaints_Total", SqlDbType.Int).Direction = ParameterDirection.Output;
                    dbCommand.Parameters.Add("O_Complaints_New_Total", SqlDbType.Int).Direction = ParameterDirection.Output;
                    dbCommand.Parameters.Add("O_Complaints_Closed_Total", SqlDbType.Int).Direction = ParameterDirection.Output;

                    dbCommand.Parameters.Add("O_CR_Total", SqlDbType.Int).Direction = ParameterDirection.Output;
                    dbCommand.Parameters.Add("O_CR_New", SqlDbType.Int).Direction = ParameterDirection.Output;
                    dbCommand.Parameters.Add("O_CR_Pending", SqlDbType.Int).Direction = ParameterDirection.Output;
                    dbCommand.Parameters.Add("O_CR_New_Pending", SqlDbType.Int).Direction = ParameterDirection.Output;
                    dbCommand.Parameters.Add("O_CR_Recieved_Pending", SqlDbType.Int).Direction = ParameterDirection.Output;
                    dbCommand.Parameters.Add("O_CR_New_Recieved_Pending", SqlDbType.Int).Direction = ParameterDirection.Output;
                    dbCommand.Parameters.Add("O_CR_Accepted", SqlDbType.Int).Direction = ParameterDirection.Output;
                    dbCommand.Parameters.Add("O_CR_New_Accepted", SqlDbType.Int).Direction = ParameterDirection.Output;
                    dbCommand.Parameters.Add("O_CR_Rejected", SqlDbType.Int).Direction = ParameterDirection.Output;
                    dbCommand.Parameters.Add("O_CR_New_Rejected", SqlDbType.Int).Direction = ParameterDirection.Output;

                    dbCommand.Parameters.Add("O_CL_Total_Count", SqlDbType.Int).Direction = ParameterDirection.Output;
                    dbCommand.Parameters.Add("O_CL_New", SqlDbType.Int).Direction = ParameterDirection.Output;
                    dbCommand.Parameters.Add("O_CL_Total_Done_Maker", SqlDbType.Int).Direction = ParameterDirection.Output;
                    dbCommand.Parameters.Add("O_CL_Total_Pending_Maker", SqlDbType.Int).Direction = ParameterDirection.Output;
                    dbCommand.Parameters.Add("O_CL_Total_New_Pending_Maker", SqlDbType.Int).Direction = ParameterDirection.Output;
                    dbCommand.Parameters.Add("O_CL_Total_Done_Checker", SqlDbType.Int).Direction = ParameterDirection.Output;
                    dbCommand.Parameters.Add("O_CL_Total_Pending_Checker", SqlDbType.Int).Direction = ParameterDirection.Output;
                    dbCommand.Parameters.Add("O_CL_Total_New_Pending_Checker", SqlDbType.Int).Direction = ParameterDirection.Output;
                    dbCommand.Parameters.Add("O_CL_Total_Accepted", SqlDbType.Int).Direction = ParameterDirection.Output;
                    dbCommand.Parameters.Add("O_CL_Total_Rejected", SqlDbType.Int).Direction = ParameterDirection.Output;
                    dbCommand.Parameters.Add("O_CL_Total_Closed", SqlDbType.Int).Direction = ParameterDirection.Output;

                    dbCommand.Parameters.Add("O_RCV_Total_Count", SqlDbType.Int).Direction = ParameterDirection.Output;
                    dbCommand.Parameters.Add("O_RCV_Total_New", SqlDbType.Int).Direction = ParameterDirection.Output;
                    dbCommand.Parameters.Add("O_RCV_Total_Match", SqlDbType.Int).Direction = ParameterDirection.Output;
                    dbCommand.Parameters.Add("O_RCV_Total_MissMatch", SqlDbType.Int).Direction = ParameterDirection.Output;
                    dbCommand.Parameters.Add("O_RCV_Total_Pending", SqlDbType.Int).Direction = ParameterDirection.Output;
                    dbCommand.Parameters.Add("O_RCV_Total_New_Pending", SqlDbType.Int).Direction = ParameterDirection.Output;

                    dbCommand.Parameters.Add("O_RCVMM_Total_Count", SqlDbType.Int).Direction = ParameterDirection.Output;
                    dbCommand.Parameters.Add("O_RCVMM_Total_New", SqlDbType.Int).Direction = ParameterDirection.Output;
                    dbCommand.Parameters.Add("O_RCVMM_Total_Pending", SqlDbType.Int).Direction = ParameterDirection.Output;
                    dbCommand.Parameters.Add("O_RCVMM_Total_New_Pending", SqlDbType.Int).Direction = ParameterDirection.Output;
                    dbCommand.Parameters.Add("O_RCVMM_Total_Fixed", SqlDbType.Int).Direction = ParameterDirection.Output;
                    dbCommand.Parameters.Add("O_RCVMM_Total_Closed", SqlDbType.Int).Direction = ParameterDirection.Output;

                    dbCommand.Parameters.Add("searchResultCursor", SqlDbType.Int).Direction = ParameterDirection.Output;

                    dbConnection.Open();
                    SqlDataReader reader = dbCommand.ExecuteReader(CommandBehavior.CloseConnection);

                    viewResult.dashboard = new Ent_Dashboard()
                    {
                        Projects_Total = int.Parse(dbCommand.Parameters["O_Projects_Total"].Value.ToString()),
                        Projects_New = int.Parse(dbCommand.Parameters["O_Projects_New_Total"].Value.ToString()),

                        compliants_Total = int.Parse(dbCommand.Parameters["O_Complaints_Total"].Value.ToString()),
                        compliants_New = int.Parse(dbCommand.Parameters["O_Complaints_New_Total"].Value.ToString()),
                        compliants_Closed = int.Parse(dbCommand.Parameters["O_Complaints_Closed_Total"].Value.ToString()),

                        CR_Total = int.Parse(dbCommand.Parameters["O_CR_Total"].Value.ToString()),
                        CR_New = int.Parse(dbCommand.Parameters["O_CR_New"].Value.ToString()),
                        CR_Pending_Total = int.Parse(dbCommand.Parameters["O_CR_Pending"].Value.ToString()),
                        CR_Pending_New = int.Parse(dbCommand.Parameters["O_CR_New_Pending"].Value.ToString()),
                        CR_Recieved_Pending_Total = int.Parse(dbCommand.Parameters["O_CR_Recieved_Pending"].Value.ToString()),
                        CR_Recieved_Pending_New = int.Parse(dbCommand.Parameters["O_CR_New_Recieved_Pending"].Value.ToString()),
                        CR_Accepted_Total = int.Parse(dbCommand.Parameters["O_CR_Accepted"].Value.ToString()),
                        CR_Accepted_New = int.Parse(dbCommand.Parameters["O_CR_New_Accepted"].Value.ToString()),
                        CR_Rejected_Total = int.Parse(dbCommand.Parameters["O_CR_Rejected"].Value.ToString()),
                        CR_Rejected_New = int.Parse(dbCommand.Parameters["O_CR_New_Rejected"].Value.ToString()),

                        CL_Total = int.Parse(dbCommand.Parameters["O_CL_Total_Count"].Value.ToString()),
                        CL_New = int.Parse(dbCommand.Parameters["O_CL_New"].Value.ToString()),
                        CL_Done_Maker = int.Parse(dbCommand.Parameters["O_CL_Total_Done_Maker"].Value.ToString()),
                        CL_Pending_Maker = int.Parse(dbCommand.Parameters["O_CL_Total_Pending_Maker"].Value.ToString()),
                        CL_Pending_New_Maker = int.Parse(dbCommand.Parameters["O_CL_Total_New_Pending_Maker"].Value.ToString()),
                        CL_Done_Cheker = int.Parse(dbCommand.Parameters["O_CL_Total_Done_Checker"].Value.ToString()),
                        CL_Pending_Cheker = int.Parse(dbCommand.Parameters["O_CL_Total_Pending_Checker"].Value.ToString()),
                        CL_Pending_New_Cheker = int.Parse(dbCommand.Parameters["O_CL_Total_New_Pending_Checker"].Value.ToString()),
                        CL_Accepted_Total = int.Parse(dbCommand.Parameters["O_CL_Total_Accepted"].Value.ToString()),
                        CL_Rejected_Total = int.Parse(dbCommand.Parameters["O_CL_Total_Rejected"].Value.ToString()),
                        CL_Closed_Total = int.Parse(dbCommand.Parameters["O_CL_Total_Closed"].Value.ToString()),

                        RCV_Total = int.Parse(dbCommand.Parameters["O_RCV_Total_Count"].Value.ToString()),
                        RCV_New = int.Parse(dbCommand.Parameters["O_RCV_Total_New"].Value.ToString()),
                        RCV_Match = int.Parse(dbCommand.Parameters["O_RCV_Total_Match"].Value.ToString()),
                        RCV_MissMatch = int.Parse(dbCommand.Parameters["O_RCV_Total_MissMatch"].Value.ToString()),
                        RCV_Pending = int.Parse(dbCommand.Parameters["O_RCV_Total_Pending"].Value.ToString()),
                        RCV_New_Pending = int.Parse(dbCommand.Parameters["O_RCV_Total_New_Pending"].Value.ToString()),

                        MMC_Total = int.Parse(dbCommand.Parameters["O_RCVMM_Total_Count"].Value.ToString()),
                        MMC_New = int.Parse(dbCommand.Parameters["O_RCVMM_Total_New"].Value.ToString()),
                        MMC_Pending = int.Parse(dbCommand.Parameters["O_RCVMM_Total_Pending"].Value.ToString()),
                        MMC_New_Pending = int.Parse(dbCommand.Parameters["O_RCVMM_Total_New_Pending"].Value.ToString()),
                        MMC_Fixed = int.Parse(dbCommand.Parameters["O_RCVMM_Total_Fixed"].Value.ToString()),
                        MMC_Closed = int.Parse(dbCommand.Parameters["O_RCVMM_Total_Closed"].Value.ToString())
                    };

                    viewResult.dashboard.lstOverAllDashboard = new List<List<string>>();

                    while (reader.Read())
                    {
                        viewResult.dashboard.lstOverAllDashboard.Add(new List<string>()
                        {
                            reader.GetString(reader.GetOrdinal("STATUSTYPE")),
                            reader.GetString(reader.GetOrdinal("S1")),
                            reader.GetString(reader.GetOrdinal("S2")),
                            reader.GetString(reader.GetOrdinal("S3")),
                            reader.GetString(reader.GetOrdinal("S4")),
                            reader.GetString(reader.GetOrdinal("S5")),
                            reader.GetString(reader.GetOrdinal("S6")),
                            reader.GetString(reader.GetOrdinal("S7")),
                            reader.GetString(reader.GetOrdinal("S8")),
                            reader.GetString(reader.GetOrdinal("S9"))
                        });
                    }

                    int saturdayIndex = viewResult.dashboard.lstOverAllDashboard[0].IndexOf(
                        ConfigurationManager.AppSettings["Dashboard_Saturday_Name"].ToString());
                    foreach (List<string> lst in viewResult.dashboard.lstOverAllDashboard)
                    {
                        lst.RemoveRange(saturdayIndex, 2);
                    }

                    dbCommand.Dispose();
                    dbConnection.Close();

                    viewResult.response = new ResponseMessage()
                    {
                        responseStatus = true
                    };
                }
            }
            catch (Exception ex)
            {
                viewResult.response = new ResponseMessage()
                {
                    responseStatus = false,
                    errorMessage = ex.Message,
                    endUserMessage = Localization.Global.UnhandledErrorOccured
                };
            }

            return viewResult;
        }

        public Vw_Dashboard CEODashboard(int userID)
        {
            Vw_Dashboard viewResult = new Vw_Dashboard();

            try
            {
                using (SqlConnection dbConnection = new SqlConnection(
                    ConfigurationManager.ConnectionStrings["dbConnectionString"].ConnectionString))
                using (SqlCommand dbCommand = dbConnection.CreateCommand())
                {
                    dbCommand.CommandText = "SP_Dashboard_CEO";
                    dbCommand.CommandType = CommandType.StoredProcedure;
                    dbCommand.Parameters.Add("I_UserID", SqlDbType.NVarChar).Value = userID;

                    dbCommand.Parameters.Add("O_Projects_Total", SqlDbType.Int).Direction = ParameterDirection.Output;
                    dbCommand.Parameters.Add("O_Projects_New_Total", SqlDbType.Int).Direction = ParameterDirection.Output;

                    dbCommand.Parameters.Add("O_Complaints_Total", SqlDbType.Int).Direction = ParameterDirection.Output;
                    dbCommand.Parameters.Add("O_Complaints_New_Total", SqlDbType.Int).Direction = ParameterDirection.Output;
                    dbCommand.Parameters.Add("O_Complaints_Closed_Total", SqlDbType.Int).Direction = ParameterDirection.Output;

                    dbCommand.Parameters.Add("O_CR_Total", SqlDbType.Int).Direction = ParameterDirection.Output;
                    dbCommand.Parameters.Add("O_CR_New", SqlDbType.Int).Direction = ParameterDirection.Output;
                    dbCommand.Parameters.Add("O_CR_Pending", SqlDbType.Int).Direction = ParameterDirection.Output;
                    dbCommand.Parameters.Add("O_CR_New_Pending", SqlDbType.Int).Direction = ParameterDirection.Output;
                    dbCommand.Parameters.Add("O_CR_Recieved_Pending", SqlDbType.Int).Direction = ParameterDirection.Output;
                    dbCommand.Parameters.Add("O_CR_New_Recieved_Pending", SqlDbType.Int).Direction = ParameterDirection.Output;
                    dbCommand.Parameters.Add("O_CR_Accepted", SqlDbType.Int).Direction = ParameterDirection.Output;
                    dbCommand.Parameters.Add("O_CR_New_Accepted", SqlDbType.Int).Direction = ParameterDirection.Output;
                    dbCommand.Parameters.Add("O_CR_Rejected", SqlDbType.Int).Direction = ParameterDirection.Output;
                    dbCommand.Parameters.Add("O_CR_New_Rejected", SqlDbType.Int).Direction = ParameterDirection.Output;

                    dbCommand.Parameters.Add("O_CL_Total_Count", SqlDbType.Int).Direction = ParameterDirection.Output;
                    dbCommand.Parameters.Add("O_CL_New", SqlDbType.Int).Direction = ParameterDirection.Output;
                    dbCommand.Parameters.Add("O_CL_Total_Done_Maker", SqlDbType.Int).Direction = ParameterDirection.Output;
                    dbCommand.Parameters.Add("O_CL_Total_Pending_Maker", SqlDbType.Int).Direction = ParameterDirection.Output;
                    dbCommand.Parameters.Add("O_CL_Total_New_Pending_Maker", SqlDbType.Int).Direction = ParameterDirection.Output;
                    dbCommand.Parameters.Add("O_CL_Total_Done_Checker", SqlDbType.Int).Direction = ParameterDirection.Output;
                    dbCommand.Parameters.Add("O_CL_Total_Pending_Checker", SqlDbType.Int).Direction = ParameterDirection.Output;
                    dbCommand.Parameters.Add("O_CL_Total_New_Pending_Checker", SqlDbType.Int).Direction = ParameterDirection.Output;
                    dbCommand.Parameters.Add("O_CL_Total_Accepted", SqlDbType.Int).Direction = ParameterDirection.Output;
                    dbCommand.Parameters.Add("O_CL_Total_Rejected", SqlDbType.Int).Direction = ParameterDirection.Output;
                    dbCommand.Parameters.Add("O_CL_Total_Closed", SqlDbType.Int).Direction = ParameterDirection.Output;

                    dbCommand.Parameters.Add("O_RCV_Total_Count", SqlDbType.Int).Direction = ParameterDirection.Output;
                    dbCommand.Parameters.Add("O_RCV_Total_New", SqlDbType.Int).Direction = ParameterDirection.Output;
                    dbCommand.Parameters.Add("O_RCV_Total_Match", SqlDbType.Int).Direction = ParameterDirection.Output;
                    dbCommand.Parameters.Add("O_RCV_Total_MissMatch", SqlDbType.Int).Direction = ParameterDirection.Output;
                    dbCommand.Parameters.Add("O_RCV_Total_Pending", SqlDbType.Int).Direction = ParameterDirection.Output;
                    dbCommand.Parameters.Add("O_RCV_Total_New_Pending", SqlDbType.Int).Direction = ParameterDirection.Output;

                    dbCommand.Parameters.Add("O_RCVMM_Total_Count", SqlDbType.Int).Direction = ParameterDirection.Output;
                    dbCommand.Parameters.Add("O_RCVMM_Total_New", SqlDbType.Int).Direction = ParameterDirection.Output;
                    dbCommand.Parameters.Add("O_RCVMM_Total_Pending", SqlDbType.Int).Direction = ParameterDirection.Output;
                    dbCommand.Parameters.Add("O_RCVMM_Total_New_Pending", SqlDbType.Int).Direction = ParameterDirection.Output;
                    dbCommand.Parameters.Add("O_RCVMM_Total_Fixed", SqlDbType.Int).Direction = ParameterDirection.Output;
                    dbCommand.Parameters.Add("O_RCVMM_Total_Closed", SqlDbType.Int).Direction = ParameterDirection.Output;

                    dbCommand.Parameters.Add("searchResultCursor", SqlDbType.Int).Direction = ParameterDirection.Output;

                    dbConnection.Open();
                    SqlDataReader reader = dbCommand.ExecuteReader(CommandBehavior.CloseConnection);

                    viewResult.dashboard = new Ent_Dashboard()
                    {
                        Projects_Total = int.Parse(dbCommand.Parameters["O_Projects_Total"].Value.ToString()),
                        Projects_New = int.Parse(dbCommand.Parameters["O_Projects_New_Total"].Value.ToString()),

                        compliants_Total = int.Parse(dbCommand.Parameters["O_Complaints_Total"].Value.ToString()),
                        compliants_New = int.Parse(dbCommand.Parameters["O_Complaints_New_Total"].Value.ToString()),
                        compliants_Closed = int.Parse(dbCommand.Parameters["O_Complaints_Closed_Total"].Value.ToString()),

                        CR_Total = int.Parse(dbCommand.Parameters["O_CR_Total"].Value.ToString()),
                        CR_New = int.Parse(dbCommand.Parameters["O_CR_New"].Value.ToString()),
                        CR_Pending_Total = int.Parse(dbCommand.Parameters["O_CR_Pending"].Value.ToString()),
                        CR_Pending_New = int.Parse(dbCommand.Parameters["O_CR_New_Pending"].Value.ToString()),
                        CR_Recieved_Pending_Total = int.Parse(dbCommand.Parameters["O_CR_Recieved_Pending"].Value.ToString()),
                        CR_Recieved_Pending_New = int.Parse(dbCommand.Parameters["O_CR_New_Recieved_Pending"].Value.ToString()),
                        CR_Accepted_Total = int.Parse(dbCommand.Parameters["O_CR_Accepted"].Value.ToString()),
                        CR_Accepted_New = int.Parse(dbCommand.Parameters["O_CR_New_Accepted"].Value.ToString()),
                        CR_Rejected_Total = int.Parse(dbCommand.Parameters["O_CR_Rejected"].Value.ToString()),
                        CR_Rejected_New = int.Parse(dbCommand.Parameters["O_CR_New_Rejected"].Value.ToString()),

                        CL_Total = int.Parse(dbCommand.Parameters["O_CL_Total_Count"].Value.ToString()),
                        CL_New = int.Parse(dbCommand.Parameters["O_CL_New"].Value.ToString()),
                        CL_Done_Maker = int.Parse(dbCommand.Parameters["O_CL_Total_Done_Maker"].Value.ToString()),
                        CL_Pending_Maker = int.Parse(dbCommand.Parameters["O_CL_Total_Pending_Maker"].Value.ToString()),
                        CL_Pending_New_Maker = int.Parse(dbCommand.Parameters["O_CL_Total_New_Pending_Maker"].Value.ToString()),
                        CL_Done_Cheker = int.Parse(dbCommand.Parameters["O_CL_Total_Done_Checker"].Value.ToString()),
                        CL_Pending_Cheker = int.Parse(dbCommand.Parameters["O_CL_Total_Pending_Checker"].Value.ToString()),
                        CL_Pending_New_Cheker = int.Parse(dbCommand.Parameters["O_CL_Total_New_Pending_Checker"].Value.ToString()),
                        CL_Accepted_Total = int.Parse(dbCommand.Parameters["O_CL_Total_Accepted"].Value.ToString()),
                        CL_Rejected_Total = int.Parse(dbCommand.Parameters["O_CL_Total_Rejected"].Value.ToString()),
                        CL_Closed_Total = int.Parse(dbCommand.Parameters["O_CL_Total_Closed"].Value.ToString()),

                        RCV_Total = int.Parse(dbCommand.Parameters["O_RCV_Total_Count"].Value.ToString()),
                        RCV_New = int.Parse(dbCommand.Parameters["O_RCV_Total_New"].Value.ToString()),
                        RCV_Match = int.Parse(dbCommand.Parameters["O_RCV_Total_Match"].Value.ToString()),
                        RCV_MissMatch = int.Parse(dbCommand.Parameters["O_RCV_Total_MissMatch"].Value.ToString()),
                        RCV_Pending = int.Parse(dbCommand.Parameters["O_RCV_Total_Pending"].Value.ToString()),
                        RCV_New_Pending = int.Parse(dbCommand.Parameters["O_RCV_Total_New_Pending"].Value.ToString()),

                        MMC_Total = int.Parse(dbCommand.Parameters["O_RCVMM_Total_Count"].Value.ToString()),
                        MMC_New = int.Parse(dbCommand.Parameters["O_RCVMM_Total_New"].Value.ToString()),
                        MMC_Pending = int.Parse(dbCommand.Parameters["O_RCVMM_Total_Pending"].Value.ToString()),
                        MMC_New_Pending = int.Parse(dbCommand.Parameters["O_RCVMM_Total_New_Pending"].Value.ToString()),
                        MMC_Fixed = int.Parse(dbCommand.Parameters["O_RCVMM_Total_Fixed"].Value.ToString()),
                        MMC_Closed = int.Parse(dbCommand.Parameters["O_RCVMM_Total_Closed"].Value.ToString())
                    };

                    viewResult.dashboard.lstOverAllDashboard = new List<List<string>>();

                    while (reader.Read())
                    {
                        viewResult.dashboard.lstOverAllDashboard.Add(new List<string>()
                        {
                            reader.GetString(reader.GetOrdinal("STATUSTYPE")),
                            reader.GetString(reader.GetOrdinal("S1")),
                            reader.GetString(reader.GetOrdinal("S2")),
                            reader.GetString(reader.GetOrdinal("S3")),
                            reader.GetString(reader.GetOrdinal("S4")),
                            reader.GetString(reader.GetOrdinal("S5")),
                            reader.GetString(reader.GetOrdinal("S6")),
                            reader.GetString(reader.GetOrdinal("S7")),
                            reader.GetString(reader.GetOrdinal("S8")),
                            reader.GetString(reader.GetOrdinal("S9"))
                        });
                    }

                    int saturdayIndex = viewResult.dashboard.lstOverAllDashboard[0].IndexOf(
                        ConfigurationManager.AppSettings["Dashboard_Saturday_Name"].ToString());
                    foreach (List<string> lst in viewResult.dashboard.lstOverAllDashboard)
                    {
                        lst.RemoveRange(saturdayIndex, 2);
                    }

                    dbCommand.Dispose();
                    dbConnection.Close();

                    viewResult.response = new ResponseMessage()
                    {
                        responseStatus = true
                    };
                }
            }
            catch (Exception ex)
            {
                viewResult.response = new ResponseMessage()
                {
                    responseStatus = false,
                    errorMessage = ex.Message,
                    endUserMessage = Localization.Global.UnhandledErrorOccured
                };
            }

            return viewResult;
        }

        public Vw_Dashboard QualityEngDashboard(int userID)
        {
            Vw_Dashboard viewResult = new Vw_Dashboard();

            try
            {
                using (SqlConnection dbConnection = new SqlConnection(
                    ConfigurationManager.ConnectionStrings["dbConnectionString"].ConnectionString))
                using (SqlCommand dbCommand = dbConnection.CreateCommand())
                {
                    dbCommand.CommandText = "SP_Dashboard_QualityEng";
                    dbCommand.CommandType = CommandType.StoredProcedure;
                    dbCommand.Parameters.Add("I_UserID", SqlDbType.NVarChar).Value = userID;

                    dbCommand.Parameters.Add("O_Projects_Total", SqlDbType.Int).Direction = ParameterDirection.Output;
                    dbCommand.Parameters.Add("O_Projects_New_Total", SqlDbType.Int).Direction = ParameterDirection.Output;

                    dbCommand.Parameters.Add("O_RCV_Total_Count", SqlDbType.Int).Direction = ParameterDirection.Output;
                    dbCommand.Parameters.Add("O_RCV_Total_New", SqlDbType.Int).Direction = ParameterDirection.Output;
                    dbCommand.Parameters.Add("O_RCV_Total_Match", SqlDbType.Int).Direction = ParameterDirection.Output;
                    dbCommand.Parameters.Add("O_RCV_Total_MissMatch", SqlDbType.Int).Direction = ParameterDirection.Output;
                    dbCommand.Parameters.Add("O_RCV_Total_Pending", SqlDbType.Int).Direction = ParameterDirection.Output;
                    dbCommand.Parameters.Add("O_RCV_Total_New_Pending", SqlDbType.Int).Direction = ParameterDirection.Output;

                    dbCommand.Parameters.Add("O_RCVMM_Total_Count", SqlDbType.Int).Direction = ParameterDirection.Output;
                    dbCommand.Parameters.Add("O_RCVMM_Total_New", SqlDbType.Int).Direction = ParameterDirection.Output;
                    dbCommand.Parameters.Add("O_RCVMM_Total_Pending", SqlDbType.Int).Direction = ParameterDirection.Output;
                    dbCommand.Parameters.Add("O_RCVMM_Total_New_Pending", SqlDbType.Int).Direction = ParameterDirection.Output;
                    dbCommand.Parameters.Add("O_RCVMM_Total_Fixed", SqlDbType.Int).Direction = ParameterDirection.Output;
                    dbCommand.Parameters.Add("O_RCVMM_Total_Closed", SqlDbType.Int).Direction = ParameterDirection.Output;

                    dbConnection.Open();
                    SqlDataReader reader = dbCommand.ExecuteReader(CommandBehavior.CloseConnection);

                    viewResult.dashboard = new Ent_Dashboard()
                    {
                        Projects_Total = int.Parse(dbCommand.Parameters["O_Projects_Total"].Value.ToString()),
                        Projects_New = int.Parse(dbCommand.Parameters["O_Projects_New_Total"].Value.ToString()),

                        RCV_Total = int.Parse(dbCommand.Parameters["O_RCV_Total_Count"].Value.ToString()),
                        RCV_New = int.Parse(dbCommand.Parameters["O_RCV_Total_New"].Value.ToString()),
                        RCV_Match = int.Parse(dbCommand.Parameters["O_RCV_Total_Match"].Value.ToString()),
                        RCV_MissMatch = int.Parse(dbCommand.Parameters["O_RCV_Total_MissMatch"].Value.ToString()),
                        RCV_Pending = int.Parse(dbCommand.Parameters["O_RCV_Total_Pending"].Value.ToString()),
                        RCV_New_Pending = int.Parse(dbCommand.Parameters["O_RCV_Total_New_Pending"].Value.ToString()),

                        MMC_Total = int.Parse(dbCommand.Parameters["O_RCVMM_Total_Count"].Value.ToString()),
                        MMC_New = int.Parse(dbCommand.Parameters["O_RCVMM_Total_New"].Value.ToString()),
                        MMC_Pending = int.Parse(dbCommand.Parameters["O_RCVMM_Total_Pending"].Value.ToString()),
                        MMC_New_Pending = int.Parse(dbCommand.Parameters["O_RCVMM_Total_New_Pending"].Value.ToString()),
                        MMC_Fixed = int.Parse(dbCommand.Parameters["O_RCVMM_Total_Fixed"].Value.ToString()),
                        MMC_Closed = int.Parse(dbCommand.Parameters["O_RCVMM_Total_Closed"].Value.ToString())
                    };

                    viewResult.dashboard.lstOverAllDashboard = new List<List<string>>();

                    dbCommand.Dispose();
                    dbConnection.Close();

                    viewResult.response = new ResponseMessage()
                    {
                        responseStatus = true
                    };
                }
            }
            catch (Exception ex)
            {
                viewResult.response = new ResponseMessage()
                {
                    responseStatus = false,
                    errorMessage = ex.Message,
                    endUserMessage = Localization.Global.UnhandledErrorOccured
                };
            }

            return viewResult;
        }
    }
}