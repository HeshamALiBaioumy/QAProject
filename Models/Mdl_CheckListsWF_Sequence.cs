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
    public class Mdl_CheckListsWF_Sequence
    {
        public QualityDbEntities ctx = new QualityDbEntities();
        public ResponseMessage insert_updateSequence(Ent_CheckListFlow_Sequence sequence, bool isUpdateForm)
        {
            ResponseMessage response = new ResponseMessage();

            try
            {
                var entity = new CHECKLIST_FLOW_SEQUENCE();
                if (sequence.ID > 0)
                    entity = ctx.CHECKLIST_FLOW_SEQUENCE.FirstOrDefault(d => d.SEQUENCE_ID == sequence.ID);



                entity.SEQUENCE_NAME = sequence.name;
                entity.MAKER = sequence.makerID;
                entity.MAKER_DAT_TIM = DateTime.Now;
                entity.DESCREPTION = sequence.description;
                entity.TECHNICIANID = sequence.technicianID;
                entity.TECHNICIAN_MAXDAYS = sequence.technician_maxDays;
                entity.SUPERVISORENGID = sequence.supervisorEngID;
                entity.SUPERENG_MAXDAYS = sequence.superEng_maxDays;
                entity.QALABID = sequence.qALabID;
                entity.QALAB_MAXDAYS = sequence.qALab_maxDays;
                entity.REPRESENTITIVESUPERID = sequence.representitiveSuperID;
                entity.REPSUPER_MAXDAYS = sequence.repSuper_maxDays;
                entity.IS_ACTIVE = sequence.isActive;

                if (sequence.ID <= 0)
                    ctx.CHECKLIST_FLOW_SEQUENCE.Add(entity);

                ctx.SaveChanges();
            }
            catch (Exception ex)
            {
                response.responseStatus = false;
                response.errorMessage = ex.Message;
                response.comments = ex.StackTrace;
                response.endUserMessage = Localization.ErrorMessages.ErrorWhileConnectingDBpleaseConsultAdmin;
            }

            return response;
            //try
            //{
            //    using (SqlConnection dbConnection = new SqlConnection(
            //        ConfigurationManager.ConnectionStrings["dbConnectionString"].ConnectionString))
            //    using (SqlCommand dbCommand = dbConnection.CreateCommand())
            //    {
            //        dbCommand.CommandText = "SP_CHECKLIST_FLOW_SEQUENCE";
            //        dbCommand.CommandType = CommandType.StoredProcedure;
            //        dbCommand.Parameters.Add("INPUT_ID", SqlDbType.Int).Value = sequence.ID;
            //        dbCommand.Parameters.Add("I_SEQUENCE_NAME", SqlDbType.VarChar).Value = sequence.name;
            //        dbCommand.Parameters.Add("I_DESCREPTION", SqlDbType.NVarChar).Value = sequence.description;
            //        dbCommand.Parameters.Add("I_TECHNICIANID", SqlDbType.Int).Value = sequence.technicianID;
            //        dbCommand.Parameters.Add("I_TECHNICIAN_MAXDAYS", SqlDbType.Int).Value = sequence.technician_maxDays;
            //        dbCommand.Parameters.Add("I_SUPERVISORENGID", SqlDbType.Int).Value = sequence.supervisorEngID;
            //        dbCommand.Parameters.Add("I_SUPERENG_MAXDAYS", SqlDbType.Int).Value = sequence.superEng_maxDays;
            //        dbCommand.Parameters.Add("I_QALABID", SqlDbType.Int).Value = sequence.qALabID;
            //        dbCommand.Parameters.Add("I_QALAB_MAXDAYS", SqlDbType.Int).Value = sequence.qALab_maxDays;
            //        dbCommand.Parameters.Add("I_REPRESENTITIVESUPERID", SqlDbType.Int).Value = sequence.representitiveSuperID;
            //        dbCommand.Parameters.Add("I_REPSUPER_MAXDAYS", SqlDbType.Int).Value = sequence.repSuper_maxDays;
            //        dbCommand.Parameters.Add("I_IS_ACTIVE", SqlDbType.Char).Value = (sequence.isActive) ? 'Y' : 'N';
            //        dbCommand.Parameters.Add("I_MAKER", SqlDbType.NVarChar).Value = sequence.makerID;
            //        dbCommand.Parameters.Add("ACTION", SqlDbType.NVarChar).Value = (isUpdateForm) ? "UPDATE" : "NEW";

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

        public List<Ent_CheckListFlow_Sequence> searchSequences(string searchName, string searchDescription
            , int searchIsActive)
        {
            List<Ent_CheckListFlow_Sequence> searchResult = new List<Ent_CheckListFlow_Sequence>();

            try
            {
                searchResult = ctx.CHECKLIST_FLOW_SEQUENCE.Where(s => s.SEQUENCE_NAME.Contains(searchName)
                                    || s.DESCREPTION.Contains(searchDescription) || s.IS_ACTIVE == (searchIsActive == 1 ? true : false))
                    .Select(mod => new Ent_CheckListFlow_Sequence
                    {
                        ID = mod.SEQUENCE_ID,
                        name = mod.SEQUENCE_NAME,
                        description = mod.DESCREPTION,
                        isActive = mod.IS_ACTIVE,
                        technicianID = mod.TECHNICIANID.HasValue ? mod.TECHNICIANID.Value : 0,
                        technician_maxDays = mod.TECHNICIAN_MAXDAYS.HasValue ? mod.TECHNICIAN_MAXDAYS.Value : 0,
                        technicianName = mod.TECHNICIANID.HasValue ? ctx.USERS_PROFILE.FirstOrDefault(s => s.PROFILE_ID == mod.TECHNICIANID.Value).NAME : "",
                        supervisorEngID = mod.SUPERVISORENGID.HasValue ? mod.SUPERVISORENGID.Value : 0,
                        superEng_maxDays = mod.SUPERENG_MAXDAYS.HasValue ? mod.SUPERENG_MAXDAYS.Value : 0,
                        supervisorEngName = mod.SUPERVISORENGID.HasValue ? ctx.USERS_PROFILE.FirstOrDefault(s => s.PROFILE_ID == mod.SUPERVISORENGID.Value).NAME : "",
                        qALabID = mod.QALABID.HasValue ? mod.QALABID.Value : 0,
                        qALab_maxDays = mod.QALAB_MAXDAYS.HasValue ? mod.QALAB_MAXDAYS.Value : 0,
                        qALabName = mod.QALABID.HasValue ? ctx.USERS_PROFILE.FirstOrDefault(s => s.PROFILE_ID == mod.QALABID.Value).NAME : "",
                        representitiveSuperID = mod.REPRESENTITIVESUPERID.HasValue ? mod.REPRESENTITIVESUPERID.Value : 0,
                        repSuper_maxDays = mod.REPSUPER_MAXDAYS.HasValue ? mod.REPSUPER_MAXDAYS.Value : 0,
                        representitiveSuperName = mod.REPRESENTITIVESUPERID.HasValue ? ctx.USERS_PROFILE.FirstOrDefault(s => s.PROFILE_ID == mod.REPRESENTITIVESUPERID.Value).NAME : "",
                    }).ToList();
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
            //        dbCommand.CommandText = "SP_CLFS_SEARCH";
            //        dbCommand.CommandType = CommandType.StoredProcedure;
            //        dbCommand.Parameters.Add("I_SearchName", SqlDbType.NVarChar).Value = searchName ?? "";
            //        dbCommand.Parameters.Add("I_SearchDescription", SqlDbType.NVarChar).Value = searchDescription ?? "";
            //        dbCommand.Parameters.Add("I_IS_ACTIVE", SqlDbType.NVarChar).Value = (searchIsActive == 0) ? 'Y' : (searchIsActive == 1) ? 'N' : 'E';
            //        dbCommand.Parameters.Add("searchResultCursor", SqlDbType.Int).Direction = ParameterDirection.Output;

            //        dbConnection.Open();
            //        SqlDataReader reader = dbCommand.ExecuteReader();
            //        while (reader.Read())
            //        {
            //            searchResult.Add(new Ent_CheckListFlow_Sequence()
            //            {
            //                ID = reader.GetInt32(reader.GetOrdinal("SEQUENCE_ID")),
            //                name = reader.GetString(reader.GetOrdinal("SEQUENCE_NAME")),
            //                description = reader.GetString(reader.GetOrdinal("DESCREPTION")),
            //                isActive = (reader.GetString(reader.GetOrdinal("IS_ACTIVE")) == "Y") ? true : false,
            //                technicianID = reader.GetInt32(reader.GetOrdinal("TECHNICIANID")),
            //                technicianName = reader.IsDBNull(reader.GetOrdinal("TECHNICIANName"))
            //                    ? "" : reader.GetString(reader.GetOrdinal("TECHNICIANName")),
            //                technician_maxDays = reader.GetInt32(reader.GetOrdinal("TECHNICIAN_MAXDAYS")),
            //                supervisorEngID = reader.GetInt32(reader.GetOrdinal("SUPERVISORENGID")),
            //                supervisorEngName = reader.IsDBNull(reader.GetOrdinal("SUPERVISORENGName"))
            //                    ? "" : reader.GetString(reader.GetOrdinal("SUPERVISORENGName")),
            //                superEng_maxDays = reader.GetInt32(reader.GetOrdinal("SUPERENG_MAXDAYS")),
            //                qALabID = reader.GetInt32(reader.GetOrdinal("QALABID")),
            //                qALabName = reader.IsDBNull(reader.GetOrdinal("QALABName"))
            //                    ? "" : reader.GetString(reader.GetOrdinal("QALABName")),
            //                qALab_maxDays = reader.GetInt32(reader.GetOrdinal("QALAB_MAXDAYS")),
            //                representitiveSuperID = reader.GetInt32(reader.GetOrdinal("REPRESENTITIVESUPERID")),
            //                representitiveSuperName = reader.IsDBNull(reader.GetOrdinal("REPRESENTITIVESUPERName"))
            //                    ? "" : reader.GetString(reader.GetOrdinal("REPRESENTITIVESUPERName")),
            //                repSuper_maxDays = reader.GetInt32(reader.GetOrdinal("REPSUPER_MAXDAYS"))
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

        public Ent_CheckListFlow_Sequence viewSequence(int sequenceID)
        {
            Ent_CheckListFlow_Sequence viewResult = new Ent_CheckListFlow_Sequence();
            try
            {
                var query = ctx.CHECKLIST_FLOW_SEQUENCE.FirstOrDefault(s => s.SEQUENCE_ID == sequenceID);
                if (query != null)
                {
                    viewResult.ID = query.SEQUENCE_ID;
                    viewResult.name = query.SEQUENCE_NAME;
                    viewResult.description = query.DESCREPTION;
                    viewResult.isActive = query.IS_ACTIVE;
                    viewResult.technicianID = query.TECHNICIANID.HasValue ? query.TECHNICIANID.Value : 0;
                    viewResult.technician_maxDays = query.TECHNICIAN_MAXDAYS.HasValue ? query.TECHNICIAN_MAXDAYS.Value : 0;
                    viewResult.technicianName = query.TECHNICIANID.HasValue ? ctx.USERS_PROFILE.FirstOrDefault(s => s.PROFILE_ID == query.TECHNICIANID.Value).NAME : "";
                    viewResult.supervisorEngID = query.SUPERVISORENGID.HasValue ? query.SUPERVISORENGID.Value : 0;
                    viewResult.superEng_maxDays = query.SUPERENG_MAXDAYS.HasValue ? query.SUPERENG_MAXDAYS.Value : 0;
                    viewResult.supervisorEngName = query.SUPERVISORENGID.HasValue ? ctx.USERS_PROFILE.FirstOrDefault(s => s.PROFILE_ID == query.SUPERVISORENGID.Value).NAME : "";
                    viewResult.qALabID = query.QALABID.HasValue ? query.QALABID.Value : 0;
                    viewResult.qALab_maxDays = query.QALAB_MAXDAYS.HasValue ? query.QALAB_MAXDAYS.Value : 0;
                    viewResult.qALabName = query.QALABID.HasValue ? ctx.USERS_PROFILE.FirstOrDefault(s => s.PROFILE_ID == query.QALABID.Value).NAME : "";
                    viewResult.representitiveSuperID = query.REPRESENTITIVESUPERID.HasValue ? query.REPRESENTITIVESUPERID.Value : 0;
                    viewResult.repSuper_maxDays = query.REPSUPER_MAXDAYS.HasValue ? query.REPSUPER_MAXDAYS.Value : 0;
                    viewResult.representitiveSuperName = query.REPRESENTITIVESUPERID.HasValue ? ctx.USERS_PROFILE.FirstOrDefault(s => s.PROFILE_ID == query.REPRESENTITIVESUPERID.Value).NAME : "";
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
            //        dbCommand.CommandText = "SP_CLFS_View";
            //        dbCommand.CommandType = CommandType.StoredProcedure;
            //        dbCommand.Parameters.Add("input_id", SqlDbType.Int).Value = sequenceID;
            //        dbCommand.Parameters.Add("viewResultCursor", SqlDbType.Int).Direction = ParameterDirection.Output;

            //        dbConnection.Open();
            //        SqlDataReader reader = dbCommand.ExecuteReader();
            //        if (reader.Read())
            //        {
            //            viewResult = new Ent_CheckListFlow_Sequence()
            //            {
            //                ID = reader.GetInt32(reader.GetOrdinal("SEQUENCE_ID")),
            //                name = reader.GetString(reader.GetOrdinal("SEQUENCE_NAME")),
            //                description = reader.GetString(reader.GetOrdinal("DESCREPTION")),
            //                isActive = (reader.GetString(reader.GetOrdinal("IS_ACTIVE")) == "Y") ? true : false,
            //                technicianID = reader.GetInt32(reader.GetOrdinal("TECHNICIANID")),
            //                technicianName = reader.IsDBNull(reader.GetOrdinal("TECHNICIANName"))
            //                    ? "" : reader.GetString(reader.GetOrdinal("TECHNICIANName")),
            //                technician_maxDays = reader.GetInt32(reader.GetOrdinal("TECHNICIAN_MAXDAYS")),
            //                supervisorEngID = reader.GetInt32(reader.GetOrdinal("SUPERVISORENGID")),
            //                supervisorEngName = reader.IsDBNull(reader.GetOrdinal("SUPERVISORENGName"))
            //                    ? "" : reader.GetString(reader.GetOrdinal("SUPERVISORENGName")),
            //                superEng_maxDays = reader.GetInt32(reader.GetOrdinal("SUPERENG_MAXDAYS")),
            //                qALabID = reader.GetInt32(reader.GetOrdinal("QALABID")),
            //                qALabName = reader.IsDBNull(reader.GetOrdinal("QALABName"))
            //                    ? "" : reader.GetString(reader.GetOrdinal("QALABName")),
            //                qALab_maxDays = reader.GetInt32(reader.GetOrdinal("QALAB_MAXDAYS")),
            //                representitiveSuperID = reader.GetInt32(reader.GetOrdinal("REPRESENTITIVESUPERID")),
            //                representitiveSuperName = reader.IsDBNull(reader.GetOrdinal("REPRESENTITIVESUPERName"))
            //                    ? "" : reader.GetString(reader.GetOrdinal("REPRESENTITIVESUPERName")),
            //                repSuper_maxDays = reader.GetInt32(reader.GetOrdinal("REPSUPER_MAXDAYS"))
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

            //return viewResult;
        }
    }
}