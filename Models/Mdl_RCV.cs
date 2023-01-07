using QA.Entities.Business_Entities;
using QA.Entities.Session_Entities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OracleClient;
using System.Data.SqlClient;

namespace QA.Models
{
    public class Mdl_RCV
    {
        public List<Ent_CR> searchRandom(int userID)
        {
            List<Ent_CR> searchResult = new List<Ent_CR>();

            try
            {
                using (SqlConnection dbConnection = new SqlConnection(
                    ConfigurationManager.ConnectionStrings["dbConnectionString"].ConnectionString))
                using (SqlCommand dbCommand = dbConnection.CreateCommand())
                {
                    dbCommand.CommandText = "SP_RCV_RandomSearch";
                    dbCommand.CommandType = CommandType.StoredProcedure;
                    dbCommand.Parameters.Add("I_User_ID", SqlDbType.Int).Value = userID;
                    dbCommand.Parameters.Add("searchResultCursor", SqlDbType.Int).Direction = ParameterDirection.Output;

                    dbConnection.Open();
                    SqlDataReader reader = dbCommand.ExecuteReader();
                    while (reader.Read())
                    {
                        searchResult.Add(new Ent_CR()
                        {
                            CRID = reader.GetInt32(reader.GetOrdinal("CR_ID")),
                            projectID = reader.GetInt32(reader.GetOrdinal("PROJECT_ID")),
                            projectName = reader.GetString(reader.GetOrdinal("PROJECT_Name")),
                            projectItemID = reader.GetInt32(reader.GetOrdinal("PROJECT_ITEM_ID")),
                            projectItemName = reader.GetString(reader.GetOrdinal("PROJECT_ITEM_Name")),
                            contractorID = reader.GetInt32(reader.GetOrdinal("CONTRACTOR_ID")),
                            contractorName = reader.GetString(reader.GetOrdinal("CONTRACTOR_Name")),
                            CRTypeMCID = reader.GetInt32(reader.GetOrdinal("CR_TYPE_MC_ID")),
                            CRTypeMCName = reader.GetString(reader.GetOrdinal("CR_TYPE_MC_Name")),
                            CRTypeGroupID = reader.GetInt32(reader.GetOrdinal("CR_TYPE_GROUPS_ID")),
                            CRTypeGroupName = reader.GetString(reader.GetOrdinal("CR_TYPE_GROUPS_Name")),
                            CRTypeID = reader.GetInt32(reader.GetOrdinal("CR_TYPE_ID")),
                            CRTypeName = reader.GetString(reader.GetOrdinal("CR_TYPE_Name")),
                            registrationDate = reader.GetDateTime(reader.GetOrdinal("REGISTER_DATE")),
                            isLabRequired = (reader.GetString(reader.GetOrdinal("IS_REQUIRE_SAMPLE")) == "Y") ? true : false,
                            allowedforRCV = (reader.GetString(reader.GetOrdinal("Allowed_To_Assign")) == "Y") ? true : false
                        });
                    }
                    reader.Close();
                    dbCommand.Dispose();
                    dbConnection.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return searchResult;
        }

        public ResponseMessage AssignCR(int CRID, int userID)
        {
            ResponseMessage result = new ResponseMessage();

            try
            {
                using (SqlConnection dbConnection = new SqlConnection(
                    ConfigurationManager.ConnectionStrings["dbConnectionString"].ConnectionString))
                using (SqlCommand dbCommand = dbConnection.CreateCommand())
                {
                    dbCommand.CommandText = "SP_RCV_Assign";
                    dbCommand.CommandType = CommandType.StoredProcedure;
                    dbCommand.Parameters.Add("I_CR_ID", SqlDbType.Int).Value = CRID;
                    dbCommand.Parameters.Add("I_User_ID", SqlDbType.Int).Value = userID;

                    dbConnection.Open();
                    dbCommand.ExecuteNonQuery();
                    dbCommand.Dispose();
                    dbConnection.Close();
                }

                result.responseStatus = true;
                result.errorMessage = null;
                result.endUserMessage = Localization.RCV.CR_Assigned_RCV_Success;
                result.comments = null;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }

        public List<Ent_RCV> searchPending(int userID)
        {
            List<Ent_RCV> searchResult = new List<Ent_RCV>();

            try
            {
                using (SqlConnection dbConnection = new SqlConnection(
                    ConfigurationManager.ConnectionStrings["dbConnectionString"].ConnectionString))
                using (SqlCommand dbCommand = dbConnection.CreateCommand())
                {
                    dbCommand.CommandText = "SP_RCV_SearchPending";
                    dbCommand.CommandType = CommandType.StoredProcedure;
                    dbCommand.Parameters.Add("I_User_ID", SqlDbType.Int).Value = userID;
                    dbCommand.Parameters.Add("searchResultCursor", SqlDbType.Int).Direction = ParameterDirection.Output;

                    dbConnection.Open();
                    SqlDataReader reader = dbCommand.ExecuteReader();
                    while (reader.Read())
                    {
                        searchResult.Add(new Ent_RCV()
                        {
                            RCVID = reader.GetInt32(reader.GetOrdinal("ID")),
                            projectID = reader.GetInt32(reader.GetOrdinal("PROJECT_ID")),
                            projectName = reader.GetString(reader.GetOrdinal("PROJECT_Name")),
                            CRID = reader.GetInt32(reader.GetOrdinal("CR_ID")),
                            CRRegistrationDate = reader.GetDateTime(reader.GetOrdinal("CR_REGISTER_DATE")),
                            RCVAssignDate = reader.GetDateTime(reader.GetOrdinal("ASSIGN_DATE")),
                            assignUserID = reader.GetInt32(reader.GetOrdinal("ASSIGN_USER_ID")),
                            assignUserName = reader.GetString(reader.GetOrdinal("ASSIGN_USER_Name")),
                            allowedforAction = (reader.GetString(reader.GetOrdinal("Allowed_For_Action")) == "Y") ? true : false
                        });
                    }
                    reader.Close();
                    dbCommand.Dispose();
                    dbConnection.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return searchResult;
        }

        public Ent_RCV viewPendingRCV(int RCVID, int userID)
        {
            Ent_RCV viewResult = null;

            try
            {
                using (SqlConnection dbConnection = new SqlConnection(
                    ConfigurationManager.ConnectionStrings["dbConnectionString"].ConnectionString))
                using (SqlCommand dbCommand = dbConnection.CreateCommand())
                {
                    dbCommand.CommandText = "SP_RCV_ViewPending";
                    dbCommand.CommandType = CommandType.StoredProcedure;
                    dbCommand.Parameters.Add("I_RCV_ID", SqlDbType.Int).Value = RCVID;
                    dbCommand.Parameters.Add("I_User_ID", SqlDbType.Int).Value = userID;
                    dbCommand.Parameters.Add("searchResultCursor", SqlDbType.Int).Direction = ParameterDirection.Output;
                    dbCommand.Parameters.Add("viewAttachmentsCursor", SqlDbType.Int).Direction = ParameterDirection.Output;

                    dbConnection.Open();
                    SqlDataReader reader = dbCommand.ExecuteReader(CommandBehavior.CloseConnection);
                    if (reader.Read())
                    {
                        viewResult = new Ent_RCV()
                        {
                            RCVID = reader.GetInt32(reader.GetOrdinal("ID")),
                            projectID = reader.GetInt32(reader.GetOrdinal("PROJECT_ID")),
                            projectName = reader.GetString(reader.GetOrdinal("PROJECT_Name")),
                            CRID = reader.GetInt32(reader.GetOrdinal("CR_ID")),
                            CRRegistrationDate = reader.GetDateTime(reader.GetOrdinal("CR_REGISTER_DATE")),
                            RCVAssignDate = reader.GetDateTime(reader.GetOrdinal("ASSIGN_DATE")),
                            comments = (reader.IsDBNull(reader.GetOrdinal("COMMENTS")) ? ""
                                : reader.GetString(reader.GetOrdinal("COMMENTS"))),
                            isLabRequired = (reader.GetString(reader.GetOrdinal("IS_SAMPLE_REQUIRED")) == "Y") ? true : false
                        };
                    }

                    viewResult.lstAttachmentNames = new List<string>();
                    viewResult.lstAttachmentPaths = new List<string>();
                    reader.NextResult();
                    while (reader.Read())
                    {
                        viewResult.lstAttachmentNames.Add(reader.GetString(reader.GetOrdinal("ATTACHEMENT_NAME")));
                        viewResult.lstAttachmentPaths.Add(reader.GetString(reader.GetOrdinal("ATTACHEMENT_PATH")));
                    }

                    reader.Close();
                    dbCommand.Dispose();
                    dbConnection.Close();
                }
            }
            catch (Exception ex)
            {
                string x = ex.Message.Substring(0, 9);
                if (ex.Message.Substring(0, 9).Equals("ORA-24338"))
                {
                    return null;
                }
                else
                {
                    throw ex;
                }
            }

            return viewResult;
        }

        public ResponseMessage feedbackRCV(int RCVID, int userID, int action, string comments
            , List<string> lstAttachmentNames, List<string> lstAttachmentPaths)
        {
            ResponseMessage response = new ResponseMessage();

            try
            {
                using (SqlConnection dbConnection = new SqlConnection(
                    ConfigurationManager.ConnectionStrings["dbConnectionString"].ConnectionString))
                using (SqlCommand dbCommand = dbConnection.CreateCommand())
                {
                    dbCommand.CommandText = "SP_RCV_Feedback";
                    dbCommand.CommandType = CommandType.StoredProcedure;
                    dbCommand.Parameters.Add("I_RCV_ID", SqlDbType.Int).Value = RCVID;
                    dbCommand.Parameters.Add("I_User_ID", SqlDbType.Int).Value = userID;
                    dbCommand.Parameters.Add("I_Action", SqlDbType.Int).Value = action;
                    dbCommand.Parameters.Add("I_Comments", SqlDbType.NVarChar).Value = comments ?? "";
                    dbCommand.Parameters.Add("I_Attachments_Type", SqlDbType.NVarChar).Value =
                        prepareAttachmentFileCommand(RCVID, userID, lstAttachmentNames, lstAttachmentPaths);

                    dbConnection.Open();
                    dbCommand.ExecuteNonQuery();
                    dbCommand.Dispose();
                    dbConnection.Close();
                }

                response.responseStatus = true;
            }
            catch (Exception ex)
            {
                response.responseStatus = false;
                response.errorMessage = ex.Message;
                response.comments = ex.StackTrace;
                response.endUserMessage = Localization.ErrorMessages.ErrorWhileConnectingDBpleaseConsultAdmin;
            }

            return response;
        }

        private string prepareAttachmentFileCommand(int RCVID, int userID, List<string> lstAttachmentNames
            , List<string> lstAttachmentPaths)
        {
            try
            {
                string txtCommand = "";

                if(lstAttachmentNames.Count > 0)
                {
                    txtCommand = "INSERT ALL ";
                    for (int i = 0; i < lstAttachmentNames.Count; i++)
                    {
                        txtCommand += " Into ATTACHMENTS Values (Fn_Get_Attachment_seq, 'RCV', '" + RCVID
                            + "', -1, '" + lstAttachmentNames[i] + "', '" + lstAttachmentPaths[i]
                            + "', null, null, null, '" + userID + "') ";
                    }
                }
                txtCommand += " SELECT 1 FROM dual ";

                return txtCommand;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<Ent_RCV> searchRCV(int searchProjectID, int searchCRID, int searchProfileID
            , int searchStatus, string searchIsLabRequired, int userID)
        {
            List<Ent_RCV> searchResult = new List<Ent_RCV>();

            try
            {
                using (SqlConnection dbConnection = new SqlConnection(
                    ConfigurationManager.ConnectionStrings["dbConnectionString"].ConnectionString))
                using (SqlCommand dbCommand = dbConnection.CreateCommand())
                {
                    dbCommand.CommandText = "SP_RCV_Search";
                    dbCommand.CommandType = CommandType.StoredProcedure;
                    dbCommand.Parameters.Add("I_Project_ID", SqlDbType.Int).Value = searchProjectID;
                    dbCommand.Parameters.Add("I_CR_ID", SqlDbType.Int).Value = searchCRID;
                    dbCommand.Parameters.Add("I_SearchProfile_ID", SqlDbType.Int).Value = searchProfileID;
                    dbCommand.Parameters.Add("I_Status_ID", SqlDbType.Int).Value = searchStatus;
                    dbCommand.Parameters.Add("I_Is_LabRequired", SqlDbType.Char).Value = searchIsLabRequired;
                    dbCommand.Parameters.Add("I_User_ID", SqlDbType.Int).Value = userID;
                    dbCommand.Parameters.Add("searchResultCursor", SqlDbType.Int).Direction = ParameterDirection.Output;

                    dbConnection.Open();
                    SqlDataReader reader = dbCommand.ExecuteReader();
                    while (reader.Read())
                    {
                        searchResult.Add(new Ent_RCV()
                        {
                            RCVID = reader.GetInt32(reader.GetOrdinal("ID")),
                            projectID = reader.GetInt32(reader.GetOrdinal("PROJECT_ID")),
                            projectName = reader.GetString(reader.GetOrdinal("PROJECT_Name")),
                            CRID = reader.GetInt32(reader.GetOrdinal("CR_ID")),
                            CRRegistrationDate = reader.GetDateTime(reader.GetOrdinal("CR_REGISTER_DATE")),
                            RCVAssignDate = reader.GetDateTime(reader.GetOrdinal("ASSIGN_DATE")),
                            assignUserID = reader.GetInt32(reader.GetOrdinal("ASSIGN_USER_ID")),
                            assignUserName = reader.GetString(reader.GetOrdinal("ASSIGN_USER_Name")),
                            isLabRequired = (reader.GetString(reader.GetOrdinal("IS_SAMPLE_REQUIRED")) == "Y") ? true : false,
                            status = reader.GetInt32(reader.GetOrdinal("STATUS")),
                            statusName = reader.GetString(reader.GetOrdinal("STATUS_Name")),
                            comments = (reader.IsDBNull(reader.GetOrdinal("COMMENTS")) ? ""
                                : reader.GetString(reader.GetOrdinal("COMMENTS"))),
                            RCVClosureDate = (reader.IsDBNull(reader.GetOrdinal("CLOSURE_DATE")) ? default(DateTime)
                                : reader.GetDateTime(reader.GetOrdinal("CLOSURE_DATE"))),
                            allowedforEdit = (reader.GetString(reader.GetOrdinal("Allowed_For_Edit")) == "Y") ? true : false
                        });
                    }
                    reader.Close();
                    dbCommand.Dispose();
                    dbConnection.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return searchResult;
        }

        public Ent_RCV viewEditRCV(int RCVID, int userID)
        {
            Ent_RCV viewResult = new Ent_RCV();

            try
            {
                using (SqlConnection dbConnection = new SqlConnection(
                    ConfigurationManager.ConnectionStrings["dbConnectionString"].ConnectionString))
                using (SqlCommand dbCommand = dbConnection.CreateCommand())
                {
                    dbCommand.CommandText = "SP_RCV_ViewEdit";
                    dbCommand.CommandType = CommandType.StoredProcedure;
                    dbCommand.Parameters.Add("I_RCV_ID", SqlDbType.Int).Value = RCVID;
                    dbCommand.Parameters.Add("I_User_ID", SqlDbType.Int).Value = userID;
                    dbCommand.Parameters.Add("searchResultCursor", SqlDbType.Int).Direction = ParameterDirection.Output;
                    dbCommand.Parameters.Add("viewAttachmentsCursor", SqlDbType.Int).Direction = ParameterDirection.Output;

                    dbConnection.Open();
                    SqlDataReader reader = dbCommand.ExecuteReader(CommandBehavior.CloseConnection);
                    if (reader.Read())
                    {
                        viewResult = new Ent_RCV()
                        {
                            RCVID = reader.GetInt32(reader.GetOrdinal("ID")),
                            projectID = reader.GetInt32(reader.GetOrdinal("PROJECT_ID")),
                            projectName = reader.GetString(reader.GetOrdinal("PROJECT_Name")),
                            CRID = reader.GetInt32(reader.GetOrdinal("CR_ID")),
                            CRRegistrationDate = reader.GetDateTime(reader.GetOrdinal("CR_REGISTER_DATE")),
                            RCVAssignDate = reader.GetDateTime(reader.GetOrdinal("ASSIGN_DATE")),
                            isLabRequired = (reader.GetString(reader.GetOrdinal("IS_SAMPLE_REQUIRED")) == "Y") ? true : false,
                            comments = (reader.IsDBNull(reader.GetOrdinal("COMMENTS")) ? ""
                                : reader.GetString(reader.GetOrdinal("COMMENTS")))
                        };
                    }

                    viewResult.lstAttachmentNames = new List<string>();
                    viewResult.lstAttachmentPaths = new List<string>();
                    reader.NextResult();
                    while (reader.Read())
                    {
                        viewResult.lstAttachmentNames.Add(reader.GetString(reader.GetOrdinal("ATTACHEMENT_NAME")));
                        viewResult.lstAttachmentPaths.Add(reader.GetString(reader.GetOrdinal("ATTACHEMENT_PATH")));
                    }

                    reader.Close();
                    dbCommand.Dispose();
                    dbConnection.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return viewResult;
        }

        public Ent_RCV viewRCV(int RCVID)
        {
            Ent_RCV viewResult = new Ent_RCV();

            try
            {
                using (SqlConnection dbConnection = new SqlConnection(
                    ConfigurationManager.ConnectionStrings["dbConnectionString"].ConnectionString))
                using (SqlCommand dbCommand = dbConnection.CreateCommand())
                {
                    dbCommand.CommandText = "SP_RCV_View";
                    dbCommand.CommandType = CommandType.StoredProcedure;
                    dbCommand.Parameters.Add("I_RCV_ID", SqlDbType.Int).Value = RCVID;
                    dbCommand.Parameters.Add("searchResultCursor", SqlDbType.Int).Direction = ParameterDirection.Output;
                    dbCommand.Parameters.Add("viewAttachmentsCursor", SqlDbType.Int).Direction = ParameterDirection.Output;

                    dbConnection.Open();
                    SqlDataReader reader = dbCommand.ExecuteReader(CommandBehavior.CloseConnection);
                    if (reader.Read())
                    {
                        viewResult = new Ent_RCV()
                        {
                            RCVID = reader.GetInt32(reader.GetOrdinal("ID")),
                            projectID = reader.GetInt32(reader.GetOrdinal("PROJECT_ID")),
                            projectName = reader.GetString(reader.GetOrdinal("PROJECT_Name")),
                            CRID = reader.GetInt32(reader.GetOrdinal("CR_ID")),
                            CRRegistrationDate = reader.GetDateTime(reader.GetOrdinal("CR_REGISTER_DATE")),
                            RCVAssignDate = reader.GetDateTime(reader.GetOrdinal("ASSIGN_DATE")),
                            assignUserID = reader.GetInt32(reader.GetOrdinal("ASSIGN_USER_ID")),
                            assignUserName = reader.GetString(reader.GetOrdinal("ASSIGN_USER_Name")),
                            isLabRequired = (reader.GetString(reader.GetOrdinal("IS_SAMPLE_REQUIRED")) == "Y") ? true : false,
                            status = reader.GetInt32(reader.GetOrdinal("STATUS")),
                            statusName = reader.GetString(reader.GetOrdinal("STATUS_Name")),
                            comments = (reader.IsDBNull(reader.GetOrdinal("COMMENTS")) ? ""
                                : reader.GetString(reader.GetOrdinal("COMMENTS"))),
                            RCVClosureDate = (reader.IsDBNull(reader.GetOrdinal("CLOSURE_DATE")) ? default(DateTime)
                                : reader.GetDateTime(reader.GetOrdinal("CLOSURE_DATE")))
                        };
                    }

                    viewResult.lstAttachmentNames = new List<string>();
                    viewResult.lstAttachmentPaths = new List<string>();
                    reader.NextResult();
                    while (reader.Read())
                    {
                        viewResult.lstAttachmentNames.Add(reader.GetString(reader.GetOrdinal("ATTACHEMENT_NAME")));
                        viewResult.lstAttachmentPaths.Add(reader.GetString(reader.GetOrdinal("ATTACHEMENT_PATH")));
                    }

                    reader.Close();
                    dbCommand.Dispose();
                    dbConnection.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return viewResult;
        }
    }
}