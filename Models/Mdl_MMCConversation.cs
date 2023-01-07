using QA.Entities.Business_Entities;
using QA.Entities.Session_Entities;
using QA.Entities.View_Entities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OracleClient;
using System.Data.SqlClient;
using static QA.Entities.Business_Entities.Ent_MMC_Conversation;

namespace QA.Models
{
    public class Mdl_MMCConversation
    {
        public Vw_MMC_Conversation viewMMCConversation(int MMCID, int userID)
        {
            Vw_MMC_Conversation viewResult = null;

            try
            {
                using (SqlConnection dbConnection = new SqlConnection(
                    ConfigurationManager.ConnectionStrings["dbConnectionString"].ConnectionString))
                using (SqlCommand dbCommand = dbConnection.CreateCommand())
                {
                    dbCommand.CommandText = "SP_MMC_Conversation_View";
                    dbCommand.CommandType = CommandType.StoredProcedure;
                    dbCommand.Parameters.Add("I_MMC_ID", SqlDbType.Int).Value = MMCID;
                    dbCommand.Parameters.Add("I_User_ID", SqlDbType.Int).Value = userID;
                    dbCommand.Parameters.Add("O_Technician_Name", SqlDbType.NVarChar, 255).Direction = ParameterDirection.Output;
                    dbCommand.Parameters.Add("O_SuperEng_Name", SqlDbType.NVarChar, 255).Direction = ParameterDirection.Output;
                    dbCommand.Parameters.Add("O_QualityEng_Name", SqlDbType.NVarChar, 255).Direction = ParameterDirection.Output;
                    dbCommand.Parameters.Add("O_ContractorAssistant_Name", SqlDbType.NVarChar, 255).Direction = ParameterDirection.Output;
                    dbCommand.Parameters.Add("O_Allowed_For_Reply", SqlDbType.Int).Direction = ParameterDirection.Output;
                    dbCommand.Parameters.Add("O_Allowed_For_Closure", SqlDbType.Int).Direction = ParameterDirection.Output;
                    dbCommand.Parameters.Add("O_Allowed_For_Escalation", SqlDbType.Int).Direction = ParameterDirection.Output;
                    dbCommand.Parameters.Add("repliesCursor", SqlDbType.Int).Direction = ParameterDirection.Output;
                    dbCommand.Parameters.Add("attachmentsCursor", SqlDbType.Int).Direction = ParameterDirection.Output;

                    dbConnection.Open();
                    SqlDataReader reader = dbCommand.ExecuteReader(CommandBehavior.CloseConnection);
                    viewResult = new Vw_MMC_Conversation()
                    {
                        conversation = new Ent_MMC_Conversation()
                        {
                            MMCID = MMCID,
                            technicianName = dbCommand.Parameters["O_Technician_Name"].Value.ToString(),
                            supervisorEngName = dbCommand.Parameters["O_SuperEng_Name"].Value.ToString(),
                            QualityAssuranceEngName = dbCommand.Parameters["O_QualityEng_Name"].Value.ToString(),
                            contractorAssName = dbCommand.Parameters["O_ContractorAssistant_Name"].Value.ToString(),
                            allowedforReply = (dbCommand.Parameters["O_Allowed_For_Reply"].Value.ToString() == "1")
                                    ? true : false,
                            allowedforClosure = (dbCommand.Parameters["O_Allowed_For_Closure"].Value.ToString() == "1")
                                    ? true : false,
                            allowedforEscalation = (dbCommand.Parameters["O_Allowed_For_Escalation"].Value.ToString() == "1")
                                    ? true : false,
                            repliesHistory = new List<Ent_MMC_Reply>()
                        }
                    };

                    while (reader.Read())
                    {
                        viewResult.conversation.repliesHistory.Add(new Ent_MMC_Reply()
                        {
                            ID = reader.GetInt32(reader.GetOrdinal("ID")),
                            replyUserName = reader.GetString(reader.GetOrdinal("REPLY_USER_Name")),
                            replyDate = reader.GetDateTime(reader.GetOrdinal("REPLY_DATETIME")),
                            replyAction = reader.GetString(reader.GetOrdinal("REPLY_ACTION")),
                            replyMessage = (reader.IsDBNull(reader.GetOrdinal("REPLY_MESSAGE")) ? ""
                                : reader.GetString(reader.GetOrdinal("REPLY_MESSAGE"))),
                            lstAttachmentNames = new List<string>(),
                            lstAttachmentPaths = new List<string>()
                        });
                    }

                    reader.NextResult();
                    List<tmpRepliesAttachments> attachments = new List<tmpRepliesAttachments>();
                    while (reader.Read())
                    {
                        attachments.Add(new tmpRepliesAttachments()
                        {
                            replyID = reader.GetInt32(reader.GetOrdinal("reply_ID")),
                            attachmentName = reader.GetString(reader.GetOrdinal("ATTACHEMENT_NAME")),
                            attachmentPath = reader.GetString(reader.GetOrdinal("ATTACHEMENT_PATH"))
                        });
                    }

                    foreach (tmpRepliesAttachments attach in attachments)
                    {
                        foreach (Ent_MMC_Reply reply in viewResult.conversation.repliesHistory)
                        {
                            if (attach.replyID == reply.ID)
                            {
                                reply.lstAttachmentNames.Add(attach.attachmentName);
                                reply.lstAttachmentPaths.Add(attach.attachmentPath);
                            }
                        }
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

        public ResponseMessage MMC_Reply(int MMCID, int replyUserID, feedbackActions replyAction
            , string replyMessage, List<string> lstAttachmentNames, List<string> lstAttachmentPaths)
        {
            ResponseMessage response = new ResponseMessage();

            try
            {
                using (SqlConnection dbConnection = new SqlConnection(
                    ConfigurationManager.ConnectionStrings["dbConnectionString"].ConnectionString))
                using (SqlCommand dbCommand = dbConnection.CreateCommand())
                {
                    dbCommand.CommandText = "SP_MMC_Conversation_AddReply";
                    dbCommand.CommandType = CommandType.StoredProcedure;
                    dbCommand.Parameters.Add("I_MMC_ID", SqlDbType.Int).Value = MMCID;
                    dbCommand.Parameters.Add("I_User_ID", SqlDbType.Int).Value = replyUserID;
                    dbCommand.Parameters.Add("I_REPLY_ACTION", SqlDbType.NVarChar).Value = replyAction.ToString();
                    dbCommand.Parameters.Add("I_REPLY_MESSAGE", SqlDbType.NVarChar).Value = replyMessage ?? "";
                    dbCommand.Parameters.Add("I_Attachments_Type", SqlDbType.NVarChar).Value =
                        prepareAttachmentFileCommand(MMCID, replyUserID, lstAttachmentNames, lstAttachmentPaths);

                    dbConnection.Open();
                    dbCommand.ExecuteNonQuery();
                    dbCommand.Dispose();
                    dbConnection.Close();
                }

                response.responseStatus = true;
            }
            catch (OracleException ex)
            {
                response.responseStatus = false;
                response.errorMessage = ex.Message;
                if (ex.Message.Substring(0, 9).Equals("ORA-24338"))
                {
                    response.errorMessage = Localization.ErrorMessages.DatabaseConnectionError;
                }
                response.comments = ex.StackTrace;
                response.endUserMessage = Localization.MMC_Conversation.AddReply_Case_NotAvailable_Reply;

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

        private string prepareAttachmentFileCommand(int MMCID, int replyUserID, List<string> lstAttachmentNames
            , List<string> lstAttachmentPaths)
        {
            try
            {
                string txtCommand = "";

                if (lstAttachmentNames != null && lstAttachmentNames.Count > 0)
                {
                    txtCommand = "INSERT ALL ";
                    for (int i = 0; i < lstAttachmentNames.Count; i++)
                    {
                        txtCommand += " Into ATTACHMENTS Values (Fn_Get_Attachment_seq, 'MMC_Conversation', '" + MMCID
                            + "', SEQ_MMC_REPLIES_ID.CurrVal, '" + lstAttachmentNames[i] + "', '" + lstAttachmentPaths[i]
                            + "', null, null, null, '" + replyUserID + "') ";
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
    }

    public struct tmpRepliesAttachments
    {
        public int replyID { get; set; }

        public string attachmentPath { get; set; }

        public string attachmentName { get; set; }
    }
}