using QA.Entities.Business_Entities;
using QA.Entities.Session_Entities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OracleClient;
using System.Data.SqlClient;
using System.Linq;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
namespace QA.Models
{
    public class Mdl_RCV
    {
        public QualityDbEntities ctx = new QualityDbEntities();
        public List<Ent_CR> searchRandom(int userID)
        {
            List<Ent_CR> searchResult = new List<Ent_CR>();

            try
            {
                var rcvCR = ctx.RCVs.Where(s => s.CR_ID.HasValue).Select(s => s.CR_ID).ToList();
                var query = from CWF in ctx.CR_WORKFOW
                            join C in ctx.CRs on CWF.CR_ID equals C.CR_ID
                            join prj in ctx.PROJECTS on C.PROJECT_ID equals prj.PROJECTS_ID
                            join PI in ctx.PROJECT_ITEMS on C.PROJECT_ITEM_ID equals PI.PROJECT_ITEMS_ID
                            join UP_CONTRACTOR in ctx.USERS_PROFILE on CWF.CONTRACTOR_ID equals UP_CONTRACTOR.PROFILE_ID
                            join CT in ctx.CR_TYPES on C.CR_TYPE_ID equals CT.CR_TYPE_ID
                            join CG in ctx.CR_TYPE_GROUPS on CT.CRTG_ID equals CG.CR_TYPE_GROUPS_ID
                            join CMG in ctx.CR_TYPES_MAIN_CATEGORIES on CG.CRTMC_ID equals CMG.CR_TYPE_MC_ID
                            where (CWF.CR_CURRENT_STATUS.HasValue && (CWF.CR_CURRENT_STATUS.Value == 1 || CWF.CR_CURRENT_STATUS.Value == 2))
                                  && prj.QA_TECHNICIAN_ID == userID && CWF.SUPERVISORENG_ID == userID &&
                                  !rcvCR.Contains(CWF.CR_ID)

                            select new Ent_CR
                            {
                                CRID = CWF.CR_ID,
                                projectName = prj.PROJECTS_ID + "-" + prj.NAME,
                                projectItemID = PI.PROJECT_ITEMS_ID,
                                projectItemName = PI.NAME,
                                contractorID = CWF.CONTRACTOR_ID.HasValue ? CWF.CONTRACTOR_ID.Value : 0,
                                contractorName = UP_CONTRACTOR.NAME,
                                CRTypeMCID = CMG.CR_TYPE_MC_ID,
                                CRTypeMCName = CMG.NAME,
                                CRTypeGroupID = CG.CR_TYPE_GROUPS_ID,
                                CRTypeGroupName = CG.NAME,
                                CRTypeID = C.CR_TYPE_ID.HasValue ? C.CR_TYPE_ID.Value : 0,
                                CRTypeName = CT.NAME,
                                registrationDate = C.REGISTER_DATE.HasValue ? C.REGISTER_DATE.Value : default(DateTime),
                                isLabRequired = CWF.IS_REQUIRE_SAMPLE.HasValue ? CWF.IS_REQUIRE_SAMPLE.Value : false,
                                allowedforRCV = prj.QA_TECHNICIAN_ID == userID ? true : false
                            };




                return searchResult;
            }
            catch (Exception ex)
            {

                throw ex;
            }

            //try
            //{
            //    using (SqlConnection dbConnection = new SqlConnection(
            //        ConfigurationManager.ConnectionStrings["dbConnectionString"].ConnectionString))
            //    using (SqlCommand dbCommand = dbConnection.CreateCommand())
            //    {
            //        dbCommand.CommandText = "SP_RCV_RandomSearch";
            //        dbCommand.CommandType = CommandType.StoredProcedure;
            //        dbCommand.Parameters.Add("I_User_ID", SqlDbType.Int).Value = userID;
            //        dbCommand.Parameters.Add("searchResultCursor", SqlDbType.Int).Direction = ParameterDirection.Output;

            //        dbConnection.Open();
            //        SqlDataReader reader = dbCommand.ExecuteReader();
            //        while (reader.Read())
            //        {
            //            searchResult.Add(new Ent_CR()
            //            {
            //                CRID = reader.GetInt32(reader.GetOrdinal("CR_ID")),
            //                projectID = reader.GetInt32(reader.GetOrdinal("PROJECT_ID")),
            //                projectName = reader.GetString(reader.GetOrdinal("PROJECT_Name")),
            //                projectItemID = reader.GetInt32(reader.GetOrdinal("PROJECT_ITEM_ID")),
            //                projectItemName = reader.GetString(reader.GetOrdinal("PROJECT_ITEM_Name")),
            //                contractorID = reader.GetInt32(reader.GetOrdinal("CONTRACTOR_ID")),
            //                contractorName = reader.GetString(reader.GetOrdinal("CONTRACTOR_Name")),
            //                CRTypeMCID = reader.GetInt32(reader.GetOrdinal("CR_TYPE_MC_ID")),
            //                CRTypeMCName = reader.GetString(reader.GetOrdinal("CR_TYPE_MC_Name")),
            //                CRTypeGroupID = reader.GetInt32(reader.GetOrdinal("CR_TYPE_GROUPS_ID")),
            //                CRTypeGroupName = reader.GetString(reader.GetOrdinal("CR_TYPE_GROUPS_Name")),
            //                CRTypeID = reader.GetInt32(reader.GetOrdinal("CR_TYPE_ID")),
            //                CRTypeName = reader.GetString(reader.GetOrdinal("CR_TYPE_Name")),
            //                registrationDate = reader.GetDateTime(reader.GetOrdinal("REGISTER_DATE")),
            //                isLabRequired = (reader.GetString(reader.GetOrdinal("IS_REQUIRE_SAMPLE")) == "Y") ? true : false,
            //                allowedforRCV = (reader.GetString(reader.GetOrdinal("Allowed_To_Assign")) == "Y") ? true : false
            //            });
            //        }
            //        reader.Close();
            //        dbCommand.Dispose();
            //        dbConnection.Close();
            //    }
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}

            //return searchResult;
        }

        public ResponseMessage AssignCR(int CRID, int userID)
        {
            ResponseMessage result = new ResponseMessage();

            try
            {
                var currentDate = DateTime.Now.Date;
                var query = from CWF in ctx.CR_WORKFOW
                            join C in ctx.CRs on CWF.CR_ID equals C.CR_ID
                            join PRJ in ctx.PROJECTS on C.PROJECT_ID equals PRJ.PROJECTS_ID
                            where (CWF.CR_CURRENT_STATUS >= 0 && CWF.CR_CURRENT_STATUS <= 6) &&
                            C.REGISTER_DATE == currentDate && C.CR_ID == CRID && PRJ.QA_TECHNICIAN_ID == userID
                            select new RCV
                            {
                                PROJECT_ID = PRJ.PROJECTS_ID,
                                IS_SAMPLE_REQUIRED = CWF.IS_REQUIRE_SAMPLE,
                                CR_ID = CWF.CR_ID,
                            };

                var check = query.FirstOrDefault();

                if (check != null)
                {
                    check.ASSIGN_DATE = DateTime.Now;
                    check.ASSIGN_USER_ID = userID;
                    check.STATUS = 0;
                    ctx.RCVs.Add(check);
                    ctx.SaveChanges();
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
            //try
            //{
            //    using (SqlConnection dbConnection = new SqlConnection(
            //        ConfigurationManager.ConnectionStrings["dbConnectionString"].ConnectionString))
            //    using (SqlCommand dbCommand = dbConnection.CreateCommand())
            //    {
            //        dbCommand.CommandText = "SP_RCV_Assign";
            //        dbCommand.CommandType = CommandType.StoredProcedure;
            //        dbCommand.Parameters.Add("I_CR_ID", SqlDbType.Int).Value = CRID;
            //        dbCommand.Parameters.Add("I_User_ID", SqlDbType.Int).Value = userID;

            //        dbConnection.Open();
            //        dbCommand.ExecuteNonQuery();
            //        dbCommand.Dispose();
            //        dbConnection.Close();
            //    }

            //    result.responseStatus = true;
            //    result.errorMessage = null;
            //    result.endUserMessage = Localization.RCV.CR_Assigned_RCV_Success;
            //    result.comments = null;
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}

            //return result;
        }

        public List<Ent_RCV> searchPending(int userID)
        {
            List<Ent_RCV> searchResult = new List<Ent_RCV>();


            try
            {
                var query = from RCV in ctx.RCVs
                            join C in ctx.CRs on RCV.CR_ID equals C.CR_ID
                            join PRJ in ctx.PROJECTS on C.PROJECT_ID equals PRJ.PROJECTS_ID
                            join PRF in ctx.USERS_PROFILE on RCV.ASSIGN_USER_ID equals PRF.PROFILE_ID
                            join UsType in ctx.USER_PROFILE_TYPE on PRF.USER_TYPE_ID equals UsType.TYPE_ID
                            where RCV.ASSIGN_USER_ID == (UsType.TYPE_CODE == "QT" ? userID : RCV.ASSIGN_USER_ID) &&
                           (RCV.STATUS == 0 || RCV.STATUS == 2 || RCV.STATUS == 4)
                            orderby RCV.ASSIGN_DATE

                            select new Ent_RCV
                            {
                                RCVID = RCV.ID,
                                projectID = PRJ.PROJECTS_ID,
                                projectName = PRJ.NAME,
                                CRID = C.CR_ID,
                                CRRegistrationDate = C.REGISTER_DATE.HasValue ? C.REGISTER_DATE.Value : default(DateTime),
                                RCVAssignDate = RCV.ASSIGN_DATE.HasValue ? RCV.ASSIGN_DATE.Value : default(DateTime),
                                assignUserID = RCV.ASSIGN_USER_ID.HasValue ? RCV.ASSIGN_USER_ID.Value : 0,
                                assignUserName = PRF.NAME,
                                allowedforAction = RCV.ASSIGN_USER_ID == userID ? true : false
                            };

                query.ToList();
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return searchResult;
            //try
            //{
            //    using (SqlConnection dbConnection = new SqlConnection(
            //        ConfigurationManager.ConnectionStrings["dbConnectionString"].ConnectionString))
            //    using (SqlCommand dbCommand = dbConnection.CreateCommand())
            //    {
            //        dbCommand.CommandText = "SP_RCV_SearchPending";
            //        dbCommand.CommandType = CommandType.StoredProcedure;
            //        dbCommand.Parameters.Add("I_User_ID", SqlDbType.Int).Value = userID;
            //        dbCommand.Parameters.Add("searchResultCursor", SqlDbType.Int).Direction = ParameterDirection.Output;

            //        dbConnection.Open();
            //        SqlDataReader reader = dbCommand.ExecuteReader();
            //        while (reader.Read())
            //        {
            //            searchResult.Add(new Ent_RCV()
            //            {
            //                RCVID = reader.GetInt32(reader.GetOrdinal("ID")),
            //                projectID = reader.GetInt32(reader.GetOrdinal("PROJECT_ID")),
            //                projectName = reader.GetString(reader.GetOrdinal("PROJECT_Name")),
            //                CRID = reader.GetInt32(reader.GetOrdinal("CR_ID")),
            //                CRRegistrationDate = reader.GetDateTime(reader.GetOrdinal("CR_REGISTER_DATE")),
            //                RCVAssignDate = reader.GetDateTime(reader.GetOrdinal("ASSIGN_DATE")),
            //                assignUserID = reader.GetInt32(reader.GetOrdinal("ASSIGN_USER_ID")),
            //                assignUserName = reader.GetString(reader.GetOrdinal("ASSIGN_USER_Name")),
            //                allowedforAction = (reader.GetString(reader.GetOrdinal("Allowed_For_Action")) == "Y") ? true : false
            //            });
            //        }
            //        reader.Close();
            //        dbCommand.Dispose();
            //        dbConnection.Close();
            //    }
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}

            //return searchResult;
        }

        public Ent_RCV viewPendingRCV(int RCVID, int userID)
        {
            Ent_RCV viewResult = null;

            try
            {
                var query = from RCV in ctx.RCVs
                            join C in ctx.CRs on RCV.CR_ID equals C.CR_ID
                            join PRJ in ctx.PROJECTS on C.PROJECT_ID equals PRJ.PROJECTS_ID

                            where RCV.ID == RCVID && RCV.ASSIGN_USER_ID == userID &&
                            (RCV.STATUS == 0 || RCV.STATUS == 2 || RCV.STATUS == 4)
                            select new Ent_RCV
                            {
                                RCVID = RCV.ID,
                                comments = RCV.COMMENTS,
                                isLabRequired = RCV.IS_SAMPLE_REQUIRED.HasValue ? RCV.IS_SAMPLE_REQUIRED.Value : false,
                                RCVAssignDate = RCV.ASSIGN_DATE.HasValue ? RCV.ASSIGN_DATE.Value : default(DateTime),
                                projectID = PRJ.PROJECTS_ID,
                                projectName = PRJ.PROJECTS_ID+ "-" + PRJ.NAME,
                                CRID = C.CR_ID,
                                CRRegistrationDate = C.REGISTER_DATE.HasValue ? C.REGISTER_DATE.Value : default(DateTime),
                            };

                viewResult = query.FirstOrDefault();
                if (viewResult != null)
                {
                    var currentAttach = ctx.ATTACHMENTS.Where(s => s.PARENT_ID == viewResult.RCVID && s.TYPE == "RCV").ToList();
                    if (currentAttach.Any())
                    {
                        viewResult.lstAttachmentNames = currentAttach.Select(s => s.ATTACHEMENT_NAME).ToList();
                        viewResult.lstAttachmentPaths = currentAttach.Select(s => s.ATTACHEMENT_PATH).ToList();
                    }

                };




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
            //try
            //{
            //    using (SqlConnection dbConnection = new SqlConnection(
            //        ConfigurationManager.ConnectionStrings["dbConnectionString"].ConnectionString))
            //    using (SqlCommand dbCommand = dbConnection.CreateCommand())
            //    {
            //        dbCommand.CommandText = "SP_RCV_ViewPending";
            //        dbCommand.CommandType = CommandType.StoredProcedure;
            //        dbCommand.Parameters.Add("I_RCV_ID", SqlDbType.Int).Value = RCVID;
            //        dbCommand.Parameters.Add("I_User_ID", SqlDbType.Int).Value = userID;
            //        dbCommand.Parameters.Add("searchResultCursor", SqlDbType.Int).Direction = ParameterDirection.Output;
            //        dbCommand.Parameters.Add("viewAttachmentsCursor", SqlDbType.Int).Direction = ParameterDirection.Output;

            //        dbConnection.Open();
            //        SqlDataReader reader = dbCommand.ExecuteReader(CommandBehavior.CloseConnection);
            //        if (reader.Read())
            //        {
            //            viewResult = new Ent_RCV()
            //            {
            //                RCVID = reader.GetInt32(reader.GetOrdinal("ID")),
            //                projectID = reader.GetInt32(reader.GetOrdinal("PROJECT_ID")),
            //                projectName = reader.GetString(reader.GetOrdinal("PROJECT_Name")),
            //                CRID = reader.GetInt32(reader.GetOrdinal("CR_ID")),
            //                CRRegistrationDate = reader.GetDateTime(reader.GetOrdinal("CR_REGISTER_DATE")),
            //                RCVAssignDate = reader.GetDateTime(reader.GetOrdinal("ASSIGN_DATE")),
            //                comments = (reader.IsDBNull(reader.GetOrdinal("COMMENTS")) ? ""
            //                    : reader.GetString(reader.GetOrdinal("COMMENTS"))),
            //                isLabRequired = (reader.GetString(reader.GetOrdinal("IS_SAMPLE_REQUIRED")) == "Y") ? true : false
            //            };
            //        }

            //        viewResult.lstAttachmentNames = new List<string>();
            //        viewResult.lstAttachmentPaths = new List<string>();
            //        reader.NextResult();
            //        while (reader.Read())
            //        {
            //            viewResult.lstAttachmentNames.Add(reader.GetString(reader.GetOrdinal("ATTACHEMENT_NAME")));
            //            viewResult.lstAttachmentPaths.Add(reader.GetString(reader.GetOrdinal("ATTACHEMENT_PATH")));
            //        }

            //        reader.Close();
            //        dbCommand.Dispose();
            //        dbConnection.Close();
            //    }
            //}
            //catch (Exception ex)
            //{
            //    string x = ex.Message.Substring(0, 9);
            //    if (ex.Message.Substring(0, 9).Equals("ORA-24338"))
            //    {
            //        return null;
            //    }
            //    else
            //    {
            //        throw ex;
            //    }
            //}

            //return viewResult;
        }

        public ResponseMessage feedbackRCV(int RCVID, int userID, int action, string comments
            , List<string> lstAttachmentNames, List<string> lstAttachmentPaths)
        {
            ResponseMessage response = new ResponseMessage();

            try
            {
                var rcv = ctx.RCVs.FirstOrDefault(s => s.ID == RCVID && s.ASSIGN_USER_ID == userID &&
                                                 (s.STATUS == 0 || s.STATUS == 2 || s.STATUS == 4));
                if (rcv != null)
                {
                    rcv.COMMENTS = comments;
                    if (action == 1)
                    {
                        rcv.CLOSURE_DATE = DateTime.Now;
                    }
                    rcv.STATUS = action;
                    ctx.SaveChanges();
                    prepareAttachmentFileCommand(RCVID, userID, lstAttachmentNames, lstAttachmentPaths);
                }
                response.responseStatus = true;
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return response;
            //try
            //{
            //    using (SqlConnection dbConnection = new SqlConnection(
            //        ConfigurationManager.ConnectionStrings["dbConnectionString"].ConnectionString))
            //    using (SqlCommand dbCommand = dbConnection.CreateCommand())
            //    {
            //        dbCommand.CommandText = "SP_RCV_Feedback";
            //        dbCommand.CommandType = CommandType.StoredProcedure;
            //        dbCommand.Parameters.Add("I_RCV_ID", SqlDbType.Int).Value = RCVID;
            //        dbCommand.Parameters.Add("I_User_ID", SqlDbType.Int).Value = userID;
            //        dbCommand.Parameters.Add("I_Action", SqlDbType.Int).Value = action;
            //        dbCommand.Parameters.Add("I_Comments", SqlDbType.NVarChar).Value = comments ?? "";
            //        dbCommand.Parameters.Add("I_Attachments_Type", SqlDbType.NVarChar).Value =
            //            prepareAttachmentFileCommand(RCVID, userID, lstAttachmentNames, lstAttachmentPaths);

            //        dbConnection.Open();
            //        dbCommand.ExecuteNonQuery();
            //        dbCommand.Dispose();
            //        dbConnection.Close();
            //    }

            //    response.responseStatus = true;
            //}
            //catch (Exception ex)
            //{
            //    response.responseStatus = false;
            //    response.errorMessage = ex.Message;
            //    response.comments = ex.StackTrace;
            //    response.endUserMessage = Localization.ErrorMessages.ErrorWhileConnectingDBpleaseConsultAdmin;
            //}

            //return response;
        }

        private string prepareAttachmentFileCommand(int RCVID, int userID, List<string> lstAttachmentNames
            , List<string> lstAttachmentPaths)
        {
            try
            {

                for (int i = 0; i < lstAttachmentNames.Count; i++)
                {
                    var attach = new ATTACHMENT
                    {
                        TYPE = "RCV",
                        PARENT_ID = RCVID,
                        PARENT_SUB_ID = -1,
                        ATTACHEMENT_NAME = lstAttachmentNames[i],
                        ATTACHEMENT_PATH = lstAttachmentPaths[i],
                        MAKERID = userID
                    };
                    ctx.ATTACHMENTS.Add(attach);
                    ctx.SaveChanges();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return "";
            //try
            //{
            //    string txtCommand = "";

            //    if (lstAttachmentNames.Count > 0)
            //    {
            //        txtCommand = "INSERT ALL ";
            //        for (int i = 0; i < lstAttachmentNames.Count; i++)
            //        {
            //            txtCommand += " Into ATTACHMENTS Values (Fn_Get_Attachment_seq, 'RCV', '" + RCVID
            //                + "', -1, '" + lstAttachmentNames[i] + "', '" + lstAttachmentPaths[i]
            //                + "', null, null, null, '" + userID + "') ";
            //        }
            //    }
            //    txtCommand += " SELECT 1 FROM dual ";

            //    return txtCommand;
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}
        }

        public List<Ent_RCV> searchRCV(int searchProjectID, int searchCRID, int searchProfileID
            , int searchStatus, string searchIsLabRequired, int userID)
        {
            List<Ent_RCV> searchResult = new List<Ent_RCV>();
            try
            {
                var query = from RCV in ctx.RCVs
                            join C in ctx.CRs on RCV.CR_ID equals C.CR_ID
                            join PRJ in ctx.PROJECTS on C.PROJECT_ID equals PRJ.PROJECTS_ID
                            join PRF in ctx.USERS_PROFILE on RCV.ASSIGN_USER_ID equals PRF.PROFILE_ID
                            join statusLV in ctx.RCV_STATUS_LOV on RCV.STATUS equals statusLV.ID

                            select new Ent_RCV
                            {
                                RCVID = RCV.ID,
                                projectID = PRJ.PROJECTS_ID,
                                projectName = PRJ.PROJECTS_ID +"-"+ PRJ.NAME,
                                CRID = C.CR_ID,
                                CRRegistrationDate = C.REGISTER_DATE.HasValue? C.REGISTER_DATE.Value:default(DateTime),
                                RCVAssignDate = RCV.ASSIGN_DATE.HasValue? RCV.ASSIGN_DATE.Value:default(DateTime),
                                assignUserID = PRF.PROFILE_ID,
                                assignUserName = PRF.NAME,
                                isLabRequired = RCV.IS_SAMPLE_REQUIRED.HasValue ? RCV.IS_SAMPLE_REQUIRED.Value:false,
                                status = statusLV.ID,
                                statusName = statusLV.STATUS_ARA,
                                comments = RCV.COMMENTS,
                                RCVClosureDate = RCV.CLOSURE_DATE.HasValue? RCV.CLOSURE_DATE.Value: default(DateTime),
                                allowedforAction = (RCV.ASSIGN_USER_ID == userID &&( RCV.STATUS==0 || RCV.STATUS == 2 || RCV.STATUS == 4))?true:false
                            };

                if (searchProjectID>0)
                {
                    query = query.Where(s => s.projectID == searchProjectID);
                }

                if (searchCRID > 0)
                {
                    query = query.Where(s => s.CRID == searchCRID);
                }

                if (searchProfileID >0)
                {
                    query = query.Where(s => s.assignUserID == searchProfileID);
                }

                if (searchStatus > 0)
                {
                    query = query.Where(s => s.status == searchStatus);
                }

                //if (!string.IsNullOrEmpty(searchIsLabRequired))
                //{
                //    var conv = bool.Parse(searchIsLabRequired);
                //    query = query.Where(s => s.status == searchStatus);
                //}
                searchResult = query.ToList();
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return searchResult;
            //try
            //{
            //    using (SqlConnection dbConnection = new SqlConnection(
            //        ConfigurationManager.ConnectionStrings["dbConnectionString"].ConnectionString))
            //    using (SqlCommand dbCommand = dbConnection.CreateCommand())
            //    {
            //        dbCommand.CommandText = "SP_RCV_Search";
            //        dbCommand.CommandType = CommandType.StoredProcedure;
            //        dbCommand.Parameters.Add("I_Project_ID", SqlDbType.Int).Value = searchProjectID;
            //        dbCommand.Parameters.Add("I_CR_ID", SqlDbType.Int).Value = searchCRID;
            //        dbCommand.Parameters.Add("I_SearchProfile_ID", SqlDbType.Int).Value = searchProfileID;
            //        dbCommand.Parameters.Add("I_Status_ID", SqlDbType.Int).Value = searchStatus;
            //        dbCommand.Parameters.Add("I_Is_LabRequired", SqlDbType.Char).Value = searchIsLabRequired;
            //        dbCommand.Parameters.Add("I_User_ID", SqlDbType.Int).Value = userID;
            //        dbCommand.Parameters.Add("searchResultCursor", SqlDbType.Int).Direction = ParameterDirection.Output;

            //        dbConnection.Open();
            //        SqlDataReader reader = dbCommand.ExecuteReader();
            //        while (reader.Read())
            //        {
            //            searchResult.Add(new Ent_RCV()
            //            {
            //                RCVID = reader.GetInt32(reader.GetOrdinal("ID")),
            //                projectID = reader.GetInt32(reader.GetOrdinal("PROJECT_ID")),
            //                projectName = reader.GetString(reader.GetOrdinal("PROJECT_Name")),
            //                CRID = reader.GetInt32(reader.GetOrdinal("CR_ID")),
            //                CRRegistrationDate = reader.GetDateTime(reader.GetOrdinal("CR_REGISTER_DATE")),
            //                RCVAssignDate = reader.GetDateTime(reader.GetOrdinal("ASSIGN_DATE")),
            //                assignUserID = reader.GetInt32(reader.GetOrdinal("ASSIGN_USER_ID")),
            //                assignUserName = reader.GetString(reader.GetOrdinal("ASSIGN_USER_Name")),
            //                isLabRequired = (reader.GetString(reader.GetOrdinal("IS_SAMPLE_REQUIRED")) == "Y") ? true : false,
            //                status = reader.GetInt32(reader.GetOrdinal("STATUS")),
            //                statusName = reader.GetString(reader.GetOrdinal("STATUS_Name")),
            //                comments = (reader.IsDBNull(reader.GetOrdinal("COMMENTS")) ? ""
            //                    : reader.GetString(reader.GetOrdinal("COMMENTS"))),
            //                RCVClosureDate = (reader.IsDBNull(reader.GetOrdinal("CLOSURE_DATE")) ? default(DateTime)
            //                    : reader.GetDateTime(reader.GetOrdinal("CLOSURE_DATE"))),
            //                allowedforEdit = (reader.GetString(reader.GetOrdinal("Allowed_For_Edit")) == "Y") ? true : false
            //            });
            //        }
            //        reader.Close();
            //        dbCommand.Dispose();
            //        dbConnection.Close();
            //    }
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}

            //return searchResult;
        }

        public Ent_RCV viewEditRCV(int RCVID, int userID)
        {
            Ent_RCV viewResult = new Ent_RCV();

            try
            {
                var query = from RCV in ctx.RCVs
                            join C in ctx.CRs on RCV.CR_ID equals C.CR_ID
                            join PRJ in ctx.PROJECTS on C.PROJECT_ID equals PRJ.PROJECTS_ID
                            join PRF in ctx.USERS_PROFILE on RCV.ASSIGN_USER_ID equals PRF.PROFILE_ID
                            where RCV.ID == RCVID && RCV.ASSIGN_USER_ID == userID &&
                            (RCV.STATUS == 0 || RCV.STATUS == 2 || RCV.STATUS == 4)

                            select new Ent_RCV
                            {
                                RCVID = RCV.ID,
                                projectID = PRJ.PROJECTS_ID,
                                projectName = PRJ.PROJECTS_ID + "-" + PRJ.NAME,
                                CRID = C.CR_ID,
                                CRRegistrationDate = C.REGISTER_DATE.HasValue ? C.REGISTER_DATE.Value : default(DateTime),
                                RCVAssignDate = RCV.ASSIGN_DATE.HasValue ? RCV.ASSIGN_DATE.Value : default(DateTime),
                                isLabRequired = RCV.IS_SAMPLE_REQUIRED.HasValue ? RCV.IS_SAMPLE_REQUIRED.Value : false,
                                comments = RCV.COMMENTS
                            };
                viewResult = query.FirstOrDefault();

                if (viewResult !=null)
                {
                    var attachLst = ctx.ATTACHMENTS.Where(s => s.TYPE == "RCV" && s.PARENT_ID == viewResult.RCVID).ToList();
                    if (attachLst.Any())
                    {
                        viewResult.lstAttachmentNames = attachLst.Select(s => s.ATTACHEMENT_NAME).ToList();
                        viewResult.lstAttachmentPaths = attachLst.Select(s => s.ATTACHEMENT_PATH).ToList();
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return viewResult;
            //try
            //{
            //    using (SqlConnection dbConnection = new SqlConnection(
            //        ConfigurationManager.ConnectionStrings["dbConnectionString"].ConnectionString))
            //    using (SqlCommand dbCommand = dbConnection.CreateCommand())
            //    {
            //        dbCommand.CommandText = "SP_RCV_ViewEdit";
            //        dbCommand.CommandType = CommandType.StoredProcedure;
            //        dbCommand.Parameters.Add("I_RCV_ID", SqlDbType.Int).Value = RCVID;
            //        dbCommand.Parameters.Add("I_User_ID", SqlDbType.Int).Value = userID;
            //        dbCommand.Parameters.Add("searchResultCursor", SqlDbType.Int).Direction = ParameterDirection.Output;
            //        dbCommand.Parameters.Add("viewAttachmentsCursor", SqlDbType.Int).Direction = ParameterDirection.Output;

            //        dbConnection.Open();
            //        SqlDataReader reader = dbCommand.ExecuteReader(CommandBehavior.CloseConnection);
            //        if (reader.Read())
            //        {
            //            viewResult = new Ent_RCV()
            //            {
            //                RCVID = reader.GetInt32(reader.GetOrdinal("ID")),
            //                projectID = reader.GetInt32(reader.GetOrdinal("PROJECT_ID")),
            //                projectName = reader.GetString(reader.GetOrdinal("PROJECT_Name")),
            //                CRID = reader.GetInt32(reader.GetOrdinal("CR_ID")),
            //                CRRegistrationDate = reader.GetDateTime(reader.GetOrdinal("CR_REGISTER_DATE")),
            //                RCVAssignDate = reader.GetDateTime(reader.GetOrdinal("ASSIGN_DATE")),
            //                isLabRequired = (reader.GetString(reader.GetOrdinal("IS_SAMPLE_REQUIRED")) == "Y") ? true : false,
            //                comments = (reader.IsDBNull(reader.GetOrdinal("COMMENTS")) ? ""
            //                    : reader.GetString(reader.GetOrdinal("COMMENTS")))
            //            };
            //        }

            //        viewResult.lstAttachmentNames = new List<string>();
            //        viewResult.lstAttachmentPaths = new List<string>();
            //        reader.NextResult();
            //        while (reader.Read())
            //        {
            //            viewResult.lstAttachmentNames.Add(reader.GetString(reader.GetOrdinal("ATTACHEMENT_NAME")));
            //            viewResult.lstAttachmentPaths.Add(reader.GetString(reader.GetOrdinal("ATTACHEMENT_PATH")));
            //        }

            //        reader.Close();
            //        dbCommand.Dispose();
            //        dbConnection.Close();
            //    }
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}

            //return viewResult;
        }

        public Ent_RCV viewRCV(int RCVID)
        {
            Ent_RCV viewResult = new Ent_RCV();

            try
            {
                var query = from RCV in ctx.RCVs
                            join C in ctx.CRs on RCV.CR_ID equals C.CR_ID
                            join PRJ in ctx.PROJECTS on C.PROJECT_ID equals PRJ.PROJECTS_ID
                            join PRF in ctx.USERS_PROFILE on RCV.ASSIGN_USER_ID equals PRF.PROFILE_ID
                            join statusLV in ctx.RCV_STATUS_LOV on RCV.STATUS equals statusLV.ID
                            where RCV.ID == RCVID

                            select new Ent_RCV
                            {
                                RCVID = RCV.ID,
                                projectID = PRJ.PROJECTS_ID,
                                projectName = PRJ.PROJECTS_ID + "-" + PRJ.NAME,
                                CRID = C.CR_ID,
                                CRRegistrationDate = C.REGISTER_DATE.HasValue ? C.REGISTER_DATE.Value : default(DateTime),
                                RCVAssignDate = RCV.ASSIGN_DATE.HasValue ? RCV.ASSIGN_DATE.Value : default(DateTime),
                                assignUserID = PRF.PROFILE_ID,
                                assignUserName = PRF.NAME,
                                isLabRequired = RCV.IS_SAMPLE_REQUIRED.HasValue ? RCV.IS_SAMPLE_REQUIRED.Value : false,
                                status = statusLV.ID,
                                statusName = statusLV.STATUS_ARA,
                                comments = RCV.COMMENTS,
                                RCVClosureDate = RCV.CLOSURE_DATE.HasValue ? RCV.CLOSURE_DATE.Value : default(DateTime),
                            };

                viewResult = query.FirstOrDefault();
                if (viewResult !=null)
                {
                    var attachLst = ctx.ATTACHMENTS.Where(s => s.TYPE == "RCV" && s.PARENT_ID == viewResult.RCVID).ToList();
                    if (attachLst.Any())
                    {
                        viewResult.lstAttachmentNames = attachLst.Select(s => s.ATTACHEMENT_NAME).ToList();
                        viewResult.lstAttachmentPaths = attachLst.Select(s => s.ATTACHEMENT_PATH).ToList();
                    }
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
            return viewResult;
            //try
            //{
            //    using (SqlConnection dbConnection = new SqlConnection(
            //        ConfigurationManager.ConnectionStrings["dbConnectionString"].ConnectionString))
            //    using (SqlCommand dbCommand = dbConnection.CreateCommand())
            //    {
            //        dbCommand.CommandText = "SP_RCV_View";
            //        dbCommand.CommandType = CommandType.StoredProcedure;
            //        dbCommand.Parameters.Add("I_RCV_ID", SqlDbType.Int).Value = RCVID;
            //        dbCommand.Parameters.Add("searchResultCursor", SqlDbType.Int).Direction = ParameterDirection.Output;
            //        dbCommand.Parameters.Add("viewAttachmentsCursor", SqlDbType.Int).Direction = ParameterDirection.Output;

            //        dbConnection.Open();
            //        SqlDataReader reader = dbCommand.ExecuteReader(CommandBehavior.CloseConnection);
            //        if (reader.Read())
            //        {
            //            viewResult = new Ent_RCV()
            //            {
            //                RCVID = reader.GetInt32(reader.GetOrdinal("ID")),
            //                projectID = reader.GetInt32(reader.GetOrdinal("PROJECT_ID")),
            //                projectName = reader.GetString(reader.GetOrdinal("PROJECT_Name")),
            //                CRID = reader.GetInt32(reader.GetOrdinal("CR_ID")),
            //                CRRegistrationDate = reader.GetDateTime(reader.GetOrdinal("CR_REGISTER_DATE")),
            //                RCVAssignDate = reader.GetDateTime(reader.GetOrdinal("ASSIGN_DATE")),
            //                assignUserID = reader.GetInt32(reader.GetOrdinal("ASSIGN_USER_ID")),
            //                assignUserName = reader.GetString(reader.GetOrdinal("ASSIGN_USER_Name")),
            //                isLabRequired = (reader.GetString(reader.GetOrdinal("IS_SAMPLE_REQUIRED")) == "Y") ? true : false,
            //                status = reader.GetInt32(reader.GetOrdinal("STATUS")),
            //                statusName = reader.GetString(reader.GetOrdinal("STATUS_Name")),
            //                comments = (reader.IsDBNull(reader.GetOrdinal("COMMENTS")) ? ""
            //                    : reader.GetString(reader.GetOrdinal("COMMENTS"))),
            //                RCVClosureDate = (reader.IsDBNull(reader.GetOrdinal("CLOSURE_DATE")) ? default(DateTime)
            //                    : reader.GetDateTime(reader.GetOrdinal("CLOSURE_DATE")))
            //            };
            //        }

            //        viewResult.lstAttachmentNames = new List<string>();
            //        viewResult.lstAttachmentPaths = new List<string>();
            //        reader.NextResult();
            //        while (reader.Read())
            //        {
            //            viewResult.lstAttachmentNames.Add(reader.GetString(reader.GetOrdinal("ATTACHEMENT_NAME")));
            //            viewResult.lstAttachmentPaths.Add(reader.GetString(reader.GetOrdinal("ATTACHEMENT_PATH")));
            //        }

            //        reader.Close();
            //        dbCommand.Dispose();
            //        dbConnection.Close();
            //    }
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}

            //  return viewResult;
        }
    }
}