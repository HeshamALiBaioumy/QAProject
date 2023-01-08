using QA.Entities.Business_Entities;
using QA.Entities.Session_Entities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OracleClient;
using System.Data.SqlClient;
using System.Linq;
namespace QA.Models
{
    public class Mdl_Complaint
    {
        public QualityDbEntities ctx = new QualityDbEntities();
        public ResponseMessage insert_updateComplaint(Ent_Complaint complaint, bool isUpdateForm)
        {
            ResponseMessage response = new ResponseMessage();

            try
            {
                var entity = new COMPLAINT();
                var attach = new ATTACHMENT();
                if (complaint.complaintID > 0)
                {
                    entity = ctx.COMPLAINTS.FirstOrDefault(d => d.COMPLAINTS_ID == complaint.complaintID);
                    attach = ctx.ATTACHMENTS.FirstOrDefault(d => d.ATTACHMENT_ID == entity.ATTACHMENT_ID.Value);
                }
                entity.MAKER = complaint.makerID;
                entity.MAKER_DAT_TIM = DateTime.Now;
                entity.DESCRIPTION = complaint.description;
                entity.CR_ID = complaint.CRID;

                entity.COMMENTS = complaint.comments;
                entity.NOTIFICATION_LIST = complaint.NotificationList ?? "";
                entity.COMPLAINT_STATUS = complaint.complaintStatus;

                attach.ATTACHEMENT_NAME = complaint.attachmentName;
                attach.ATTACHEMENT_PATH = complaint.attachmentPath;
                if (complaint.complaintID <= 0)
                {
                    ctx.ATTACHMENTS.Add(attach);
                    ctx.SaveChanges();

                    entity.ATTACHMENT_ID = attach.ATTACHMENT_ID;
                    ctx.COMPLAINTS.Add(entity);
                }


                ctx.SaveChanges();



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

        public List<Ent_Complaint> searchComplaint(int searchProjectID, int searchCRID, string searchComments
            , string searchDescription, int searchcomplaintStatus)
        {
            List<Ent_Complaint> searchResult = new List<Ent_Complaint>();

            try
            {
                var query =
                            from comp in ctx.COMPLAINTS
                            join cr in ctx.CRs on comp.CR_ID equals cr.PROJECT_ID
                            join proj in ctx.PROJECTS on cr.PROJECT_ID equals proj.PROJECTS_ID

                            select new Ent_Complaint
                            {
                                complaintID = comp.COMPLAINTS_ID,
                                projectID = proj.PROJECTS_ID,
                                projectName = proj.NAME,
                                CRID = cr.CR_ID,
                                regisetDate = cr.REGISTER_DATE.HasValue? cr.REGISTER_DATE.Value:default(DateTime),
                                comments = comp.COMMENTS,
                                description = comp.DESCRIPTION,
                                NotificationList = comp.NOTIFICATION_LIST,
                                complaintStatus = comp.COMPLAINT_STATUS==null?0: comp.COMPLAINT_STATUS.Value,

                            };

                if (searchProjectID >0)
                {
                    query = query.Where(s => s.projectID == searchProjectID);
                }

                if (searchCRID > 0)
                {
                    query = query.Where(s => s.CRID == searchCRID);
                }

                if (searchComments !=null)
                {
                    query = query.Where(s => s.comments.Contains(searchComments));
                }

                if (searchDescription != null)
                {
                    query = query.Where(s => s.description.Contains(searchDescription));
                }

                if (searchcomplaintStatus >0)
                {
                    query = query.Where(s => s.complaintStatus == searchcomplaintStatus);
                }

                searchResult = query.ToList();
                searchResult.ForEach(y =>
                {
                    y.CRName = y.regisetDate.ToString("dd/MM/yyyy");
                });
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
            //        dbCommand.CommandText = "SP_COMPLAINTS_Search";
            //        dbCommand.CommandType = CommandType.StoredProcedure;
            //        dbCommand.Parameters.Add("Inpt_PROJECT_ID", SqlDbType.Int).Value = searchProjectID;
            //        dbCommand.Parameters.Add("Inpt_CR_ID", SqlDbType.Int).Value = searchCRID;
            //        dbCommand.Parameters.Add("Inpt_SearchCOMMENTS", SqlDbType.NVarChar).Value = searchComments ?? "";
            //        dbCommand.Parameters.Add("Inpt_SearchDescription", SqlDbType.NVarChar).Value = searchDescription ?? "";
            //        dbCommand.Parameters.Add("Inpt_complaint_Status", SqlDbType.Int).Value = searchcomplaintStatus;
            //        dbCommand.Parameters.Add("searchResultCursor", SqlDbType.Int).Direction = ParameterDirection.Output;

            //        dbConnection.Open();
            //        SqlDataReader reader = dbCommand.ExecuteReader();
            //        while (reader.Read())
            //        {
            //            searchResult.Add(new Ent_Complaint()
            //            {
            //                complaintID = reader.GetInt32(reader.GetOrdinal("COMPLAINTS_ID")),
            //                projectID = reader.GetInt32(reader.GetOrdinal("PROJECTS_ID")),
            //                projectName = reader.GetString(reader.GetOrdinal("PROJECT_Name")),
            //                CRID = reader.GetInt32(reader.GetOrdinal("CR_ID")),
            //                CRName = reader.GetString(reader.GetOrdinal("CR_Name")),
            //                comments = reader.GetString(reader.GetOrdinal("COMMENTS")),
            //                description = reader.GetString(reader.GetOrdinal("DESCRIPTION")),
            //                NotificationList = (reader.IsDBNull(reader.GetOrdinal("NOTIFICATION_LIST"))) ? string.Empty
            //                : reader.GetString(reader.GetOrdinal("NOTIFICATION_LIST")),
            //                complaintStatus = reader.GetInt32(reader.GetOrdinal("complaint_Status"))
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

            //   return searchResult;
        }

        public Ent_Complaint viewComplaint(int ComplaintID)
        {
            Ent_Complaint viewResult = new Ent_Complaint();

            try
            {
                var query =
                           from comp in ctx.COMPLAINTS
                           join cr in ctx.CRs on comp.CR_ID equals cr.PROJECT_ID
                           join proj in ctx.PROJECTS on cr.PROJECT_ID equals proj.PROJECTS_ID
                           where comp.COMPLAINTS_ID == ComplaintID

                           select new Ent_Complaint()
                           {
                               complaintID = comp.COMPLAINTS_ID,
                               projectID = proj.PROJECTS_ID,
                               projectName = proj.NAME,
                               CRID = cr.CR_ID,
                               CRName = cr.REGISTER_DATE != null ? cr.REGISTER_DATE.Value.ToString("dd/MM/yyyy") : "",
                               comments = comp.COMMENTS,
                               description = comp.DESCRIPTION,
                               NotificationList = comp.NOTIFICATION_LIST,
                               complaintStatus = comp.COMPLAINT_STATUS == null ? 0 : comp.COMPLAINT_STATUS.Value,
                           };

                viewResult = query.FirstOrDefault();
            }
            catch (Exception)
            {

                throw;
            }
            //try
            //{
            //    using (SqlConnection dbConnection = new SqlConnection(
            //        ConfigurationManager.ConnectionStrings["dbConnectionString"].ConnectionString))
            //    using (SqlCommand dbCommand = dbConnection.CreateCommand())
            //    {
            //        dbCommand.CommandText = "SP_COMPLAINTS_VIEW";
            //        dbCommand.CommandType = CommandType.StoredProcedure;
            //        dbCommand.Parameters.Add("input_id", SqlDbType.NVarChar).Value = ComplaintID;
            //        dbCommand.Parameters.Add("viewResultCursor", SqlDbType.Int).Direction = ParameterDirection.Output;

            //        dbConnection.Open();
            //        SqlDataReader reader = dbCommand.ExecuteReader();
            //        if (reader.Read())
            //        {
            //            viewResult = new Ent_Complaint()
            //            {
            //                complaintID = reader.GetInt32(reader.GetOrdinal("COMPLAINTS_ID")),
            //                projectID = reader.GetInt32(reader.GetOrdinal("PROJECTS_ID")),
            //                projectName = reader.GetString(reader.GetOrdinal("PROJECT_Name")),
            //                CRID = reader.GetInt32(reader.GetOrdinal("CR_ID")),
            //                CRName = reader.GetString(reader.GetOrdinal("CR_Name")),
            //                comments = reader.GetString(reader.GetOrdinal("COMMENTS")),
            //                description = reader.GetString(reader.GetOrdinal("DESCRIPTION")),
            //                NotificationList = (reader.IsDBNull(reader.GetOrdinal("NOTIFICATION_LIST"))) ? string.Empty
            //                    : reader.GetString(reader.GetOrdinal("NOTIFICATION_LIST")),
            //                complaintStatus = reader.GetInt32(reader.GetOrdinal("complaint_Status")),
            //                attachmentName = (reader.IsDBNull(reader.GetOrdinal("ATTACHEMENT_NAME"))) ? ""
            //                    : reader.GetString(reader.GetOrdinal("ATTACHEMENT_NAME")),
            //                attachmentPath = (reader.IsDBNull(reader.GetOrdinal("ATTACHEMENT_PATH"))) ? ""
            //                    : reader.GetString(reader.GetOrdinal("ATTACHEMENT_PATH"))
            //            };
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

            return viewResult;
        }
    }
}