using QA.Entities.Business_Entities;
using QA.Entities.Session_Entities;
using QA.Entities.View_Entities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OracleClient;
using System.Data.SqlClient;
using System.Linq;

namespace QA.Models
{
    public class Mdl_CheckListFlow
    {
        public QualityDbEntities ctx = new QualityDbEntities();
        public ResponseMessage insert_updatCheckListFlow(Ent_CheckListFlow_Master checkListFlowMaster, bool isUpdateForm)
        {
            ResponseMessage response = new ResponseMessage();

            try
            {
                var entity = ctx.CHECKLIST_FLOW_SEQUENCE.FirstOrDefault(d => d.SEQUENCE_ID == checkListFlowMaster.ID);
                if (entity == null)
                {
                    entity = new CHECKLIST_FLOW_SEQUENCE();
                    entity.SEQUENCE_NAME = checkListFlowMaster.cLSequenceName;
                    entity.MAKER = checkListFlowMaster.makerID;
                    entity.TECHNICIANID = checkListFlowMaster.technicianID;
                    entity.QALAB_MAXDAYS = checkListFlowMaster.qALab_maxDays;
                    entity.REPSUPER_MAXDAYS = checkListFlowMaster.superEng_maxDays;
                    entity.SUPERVISORENGID = checkListFlowMaster.supervisorEngID;
                    entity.TECHNICIAN_MAXDAYS = checkListFlowMaster.technician_maxDays;
                    entity.REPRESENTITIVESUPERID = checkListFlowMaster.representitiveSuperID;
                    entity.QALABID = checkListFlowMaster.qALabID;
                    ctx.CHECKLIST_FLOW_SEQUENCE.Add(entity);

                }
                else
                {
                    entity = new CHECKLIST_FLOW_SEQUENCE();
                    entity.SEQUENCE_NAME = checkListFlowMaster.cLSequenceName;
                    entity.MAKER = checkListFlowMaster.makerID;
                    entity.TECHNICIANID = checkListFlowMaster.technicianID;
                    entity.QALAB_MAXDAYS = checkListFlowMaster.qALab_maxDays;
                    entity.REPSUPER_MAXDAYS = checkListFlowMaster.superEng_maxDays;
                    entity.SUPERVISORENGID = checkListFlowMaster.supervisorEngID;
                    entity.TECHNICIAN_MAXDAYS = checkListFlowMaster.technician_maxDays;
                    entity.REPRESENTITIVESUPERID = checkListFlowMaster.representitiveSuperID;
                    entity.QALABID = checkListFlowMaster.qALabID;


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

        public List<Ent_CheckListFlow_Master> searchPendingCheckLists(int userID)
        {
            List<Ent_CheckListFlow_Master> searchResult = new List<Ent_CheckListFlow_Master>();

            try
            {
                var query =
                      from flowMaster in ctx.CHECKLIST_FLOW_MASTER
                      join flow in ctx.CHECKLIST_FLOW on flowMaster.CHECKLISTID equals flow.CL_FLOW_SEQ
                      join flowstatus in ctx.CL_FLOW_STATUSES on flowMaster.STATUS equals flowstatus.STATUS_SEQ_ID

                      where (flowMaster.TECHNICIANID == userID || flowMaster.QALABID == userID || flowMaster.SUPERVISORENGID == userID
                                || flowMaster.REPRESENTITIVESUPERID == userID)

                      select new Ent_CheckListFlow_Master()
                      {
                          ID = flowMaster.ID,
                          cLID = flow.CL_FLOW_ID.HasValue ? flow.CL_FLOW_ID.Value : 0,
                          cLName = ctx.CHECK_LIST.FirstOrDefault(s => s.CHECK_LIST_ID == (flow.CL_FLOW_ID.HasValue ? flow.CL_FLOW_ID.Value : 0)).NAME,
                          technicianID = flowMaster.TECHNICIANID.HasValue ? flowMaster.TECHNICIANID.Value : 0,
                          technicianName = flowMaster.TECHNICIANID.HasValue ? ctx.USERS_PROFILE.FirstOrDefault(s => s.PROFILE_ID == flowMaster.TECHNICIANID.Value).NAME : "",
                          technician_maxDays = flowMaster.TECHNICIAN_MAXDAYS.HasValue ? flowMaster.TECHNICIAN_MAXDAYS.Value : 0,
                          supervisorEngID = flowMaster.SUPERVISORENGID.HasValue ? flowMaster.SUPERVISORENGID.Value : 0,
                          supervisorEngName = flowMaster.SUPERVISORENGID.HasValue ? ctx.USERS_PROFILE.FirstOrDefault(s => s.PROFILE_ID == flowMaster.SUPERVISORENGID.Value).NAME : "",
                          superEng_maxDays = flowMaster.SUPERVISORENGID.HasValue ? flowMaster.SUPERVISORENGID.Value : 0,
                          qALabID = flowMaster.QALABID.HasValue ? flowMaster.QALABID.Value : 0,
                          qALabName = flowMaster.QALABID.HasValue ? ctx.USERS_PROFILE.FirstOrDefault(s => s.PROFILE_ID == flowMaster.QALABID.Value).NAME : "",
                          qALab_maxDays = flowMaster.QALAB_MAXDAYS.HasValue ? flowMaster.QALAB_MAXDAYS.Value : 0,
                          representitiveSuperID = flowMaster.REPRESENTITIVESUPERID.HasValue ? flowMaster.REPRESENTITIVESUPERID.Value : 0,
                          representitiveSuperName = flowMaster.REPRESENTITIVESUPERID.HasValue ? ctx.USERS_PROFILE.FirstOrDefault(s => s.PROFILE_ID == flowMaster.REPRESENTITIVESUPERID.Value).NAME : "",
                          repSuper_maxDays = flowMaster.REPSUPER_MAXDAYS.HasValue ? flowMaster.REPSUPER_MAXDAYS.Value : 0,
                          registrationDate = flowMaster.REGISTRATIONDATE.HasValue ? flowMaster.REGISTRATIONDATE.Value : default(DateTime),
                          CLFlowStatus = flowMaster.STATUS.HasValue ? flowMaster.STATUS.Value : 0,
                          CLFlowStatusName = flowstatus.STATUS_ENG_DESC,
                          closurenDate = flowMaster.CLOSURENDATE.HasValue ? flowMaster.CLOSURENDATE.Value : default(DateTime),
                          allowedforMaker = (flowMaster.STATUS == 0 && (flowMaster.TECHNICIANID == userID || flowMaster.SUPERVISORENGID == userID)) ? true : false,
                          allowedforEdit = ((flowMaster.STATUS == 1 || flowMaster.STATUS == 2) && flowMaster.SUPERENG_ACTION == userID.ToString()) == true ? true
                                            : ((flowMaster.STATUS == 1 || flowMaster.STATUS == 2 || flowMaster.STATUS == 3) && flowMaster.SUPERVISORENGID != null) == true ? true
                                            : ((flowMaster.STATUS == 1 || flowMaster.STATUS == 2 || flowMaster.STATUS == 3 || flowMaster.STATUS == 4)
                                               && (flowMaster.SUPERVISORENGID != null && flowMaster.SUPERVISORENGID != -1) && (flowMaster.SUPERENG_ACTION != null && flowMaster.SUPERENG_ACTION != "-1") && (flowMaster.QALABID != null && flowMaster.QALABID != -1)
                                               && (flowMaster.QALAB_ACTION != null && flowMaster.QALAB_ACTION != "-1") && flowMaster.REPRESENTITIVESUPERID == userID) == true ? true : false
                      };

                searchResult = query.ToList();

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
            //        dbCommand.CommandText = "SP_CLFlow_Pending";
            //        dbCommand.CommandType = CommandType.StoredProcedure;
            //        dbCommand.Parameters.Add("I_User_ID", SqlDbType.Int).Value = userID;
            //        dbCommand.Parameters.Add("searchResultCursor", SqlDbType.Int).Direction = ParameterDirection.Output;

            //        dbConnection.Open();
            //        SqlDataReader reader = dbCommand.ExecuteReader();
            //        while (reader.Read())
            //        {
            //            searchResult.Add(new Ent_CheckListFlow_Master()
            //            {
            //                ID = reader.GetInt32(reader.GetOrdinal("ID")),
            //                cLID = reader.GetInt32(reader.GetOrdinal("CHECKLISTID")),
            //                cLName = reader.GetString(reader.GetOrdinal("CHECKLISTName")),
            //                CLParty = reader.GetString(reader.GetOrdinal("CL_PARTY")),
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
            //                repSuper_maxDays = reader.GetInt32(reader.GetOrdinal("REPSUPER_MAXDAYS")),
            //                registrationDate = reader.GetDateTime(reader.GetOrdinal("REGISTRATIONDATE")),
            //                CLFlowStatus = reader.GetInt32(reader.GetOrdinal("STATUS")),
            //                CLFlowStatusName = reader.GetString(reader.GetOrdinal("Status_Desc")),
            //                allowedforMaker = (reader.GetString(reader.GetOrdinal("Allowed_for_Maker")) == "Y") ? true : false,
            //                allowedforChecker = (reader.GetString(reader.GetOrdinal("Allowed_for_Checker")) == "Y") ? true : false,
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

            return searchResult;
        }

        public Vw_CheckList_Flow viewCLMaker(int CLID, int userID)
        {
            Vw_CheckList_Flow viewResult = null;

            try
            {
                var checkMaster = ctx.CHECKLIST_FLOW_MASTER.Any(s => s.TECHNICIANID == userID
                                                        && s.ID == CLID && s.SUPERVISORENGID == userID && s.STATUS == 0);
                if (checkMaster == true)
                {
                    var currentMaster = ctx.CHECKLIST_FLOW_MASTER.FirstOrDefault(s => s.ID == CLID);
                    viewResult = new Vw_CheckList_Flow()
                    {

                        cLFlowID = CLID,
                        cLID = currentMaster.CHECKLISTID.HasValue ? currentMaster.CHECKLISTID.Value : 0,
                        CLParty = currentMaster.CL_PARTY,
                        registrationDate = currentMaster.REGISTRATIONDATE.HasValue ? currentMaster.REGISTRATIONDATE.Value : default(DateTime)
                    };
                    var checkList = ctx.CHECK_LIST.FirstOrDefault(s => s.CHECK_LIST_ID == CLID);
                    var groupIds = checkList.GROUPS_ID.Split(',').Select(Int32.Parse).ToList();
                    var currentGroups = ctx.CHECK_LIST_GROUPS.Where(s => groupIds.Contains(s.CHECK_LIST_GROUPS_ID));
                    viewResult.lstClItems = new List<Ent_CheckList_Flow>();
                    foreach (var currentGroup in currentGroups)
                    {
                        var itemId = int.Parse(currentGroup.ITEMS_ID);
                        viewResult.lstClItems.Add(new Ent_CheckList_Flow
                        {
                            cLGID = currentGroup.CHECK_LIST_GROUPS_ID,
                            cLGName = currentGroup.NAME,
                            cLItemID = itemId,
                            cLItemName = ctx.CL_GROUPS_ITEMS.FirstOrDefault(s => s.CL_GROUPS_ITEMS_ID == itemId).NAME,
                        });
                    }

                }
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
            //        dbCommand.CommandText = "SP_CHECKLISTFlow_ViewMaker";
            //        dbCommand.CommandType = CommandType.StoredProcedure;
            //        dbCommand.Parameters.Add("I_CLFlow_ID", SqlDbType.Int).Value = CLID;
            //        dbCommand.Parameters.Add("I_User_ID", SqlDbType.Int).Value = userID;
            //        dbCommand.Parameters.Add("I_CHECKLISTID", SqlDbType.Int).Direction = ParameterDirection.Output;
            //        dbCommand.Parameters.Add("I_CL_PARTY", SqlDbType.NVarChar, 250).Direction = ParameterDirection.Output;
            //        dbCommand.Parameters.Add("I_REGISTRATIONDATE", SqlDbType.DateTime).Direction = ParameterDirection.Output;
            //        dbCommand.Parameters.Add("searchResultCursor", SqlDbType.Int).Direction = ParameterDirection.Output;

            //        dbConnection.Open();
            //        SqlDataReader reader = dbCommand.ExecuteReader(CommandBehavior.CloseConnection);

            //        viewResult = new Vw_CheckList_Flow()
            //        {
            //            cLFlowID = CLID,
            //            cLID = int.Parse(dbCommand.Parameters["I_CHECKLISTID"].Value.ToString()),
            //            CLParty = dbCommand.Parameters["I_CL_PARTY"].Value.ToString(),
            //            registrationDate = DateTime.Parse(dbCommand.Parameters["I_REGISTRATIONDATE"].Value.ToString())
            //        };
            //        viewResult.lstClItems = new List<Ent_CheckList_Flow>();
            //        while (reader.Read())
            //        {
            //            viewResult.lstClItems.Add(new Ent_CheckList_Flow()
            //            {
            //                cLGID = reader.GetInt32(reader.GetOrdinal("CLG_ID")),
            //                cLGName = reader.GetString(reader.GetOrdinal("CLG_Name")),
            //                cLItemID = reader.GetInt32(reader.GetOrdinal("CL_Item_ID")),
            //                cLItemName = reader.GetString(reader.GetOrdinal("CL_Item_Name"))
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

            return viewResult;
        }

        public ResponseMessage checkListMaker(Vw_CheckList_Flow checkListFlow)
        {
            ResponseMessage response = new ResponseMessage();

            try
            {
                foreach (var lstItem in checkListFlow.lstClItems)
                {
                    if (lstItem.hasAttachment)
                    {
                        var attachment = new ATTACHMENT
                        {
                            ATTACHEMENT_NAME = lstItem.attachmentName,
                            ATTACHEMENT_PATH = lstItem.attachmentPath,
                            MAKERID = int.Parse(checkListFlow.makerID),
                            TYPE = "CheckList_Maker",
                            PARENT_ID = checkListFlow.cLFlowID,
                            PARENT_SUB_ID = checkListFlow.cLID,
                        };
                        ctx.ATTACHMENTS.Add(attachment);
                        ctx.SaveChanges();

                        var checkList_Flow = new CHECKLIST_FLOW
                        {
                            CL_FLOW_ID = checkListFlow.cLFlowID,
                            CL_ID = checkListFlow.cLID,
                            CL_GROUP_ID = lstItem.cLGID,
                            CL_ITEM_ID = lstItem.cLItemID,
                            AVAILABLE_ITEM = lstItem.isCLItemAvailable == 1 ? true : false,
                            COMMENTS = lstItem.comment,
                            CL_FLOW_SEQ = attachment.ATTACHMENT_ID,
                        };

                        ctx.CHECKLIST_FLOW.Add(checkList_Flow);
                        ctx.SaveChanges();

                        var checkList_Flows = new CHECKLIST_FLOW
                        {
                            CL_FLOW_ID = checkListFlow.cLFlowID,
                            CL_ID = checkListFlow.cLID,
                            CL_GROUP_ID = lstItem.cLGID,
                            CL_ITEM_ID = lstItem.cLItemID,
                            AVAILABLE_ITEM = lstItem.isCLItemAvailable == 1 ? true : false,
                            COMMENTS = lstItem.comment,
                            CL_FLOW_SEQ = -1,
                        };

                        ctx.CHECKLIST_FLOW.Add(checkList_Flows);
                        ctx.SaveChanges();


                        var userProfile = ctx.USERS_PROFILE.FirstOrDefault(s => s.MAKER_ID == checkListFlow.makerID);
                        if (userProfile != null)
                        {
                            var type = ctx.USER_PROFILE_TYPE.FirstOrDefault(s => s.TYPE_ID == userProfile.USER_TYPE_ID);
                            if (type.TYPE_CODE == "QT" || type.TYPE_CODE == "SE")
                            {
                                var currentFlowMaster = ctx.CHECKLIST_FLOW_MASTER.FirstOrDefault(s => s.ID == checkListFlow.cLFlowID);
                                currentFlowMaster.STATUS = type.TYPE_CODE == "QT" ? 1 : 2;
                                currentFlowMaster.TECHNICIAN_ACTION = "Maker";
                                currentFlowMaster.TECHNICIAN_ACTION_DATE = DateTime.Now;
                            }
                        }

                    }
                }
                response.responseStatus = true;
                return response;
            }
            catch (Exception ex)
            {

                response.responseStatus = false;
                response.errorMessage = ex.Message;
                response.comments = ex.StackTrace;
                response.endUserMessage = Localization.ErrorMessages.ErrorWhileConnectingDBpleaseConsultAdmin;
            }

            //try
            //{
            //    using (SqlConnection dbConnection = new SqlConnection(
            //        ConfigurationManager.ConnectionStrings["dbConnectionString"].ConnectionString))
            //    using (SqlCommand dbCommand = dbConnection.CreateCommand())
            //    {
            //        dbCommand.CommandText = "SP_CHECKLIST_Flow_Maker";
            //        dbCommand.CommandType = CommandType.StoredProcedure;

            //        dbConnection.Open();
            //        SqlTransaction transaction = dbConnection.BeginTransaction(IsolationLevel.ReadCommitted);
            //        dbCommand.Transaction = transaction;

            //        try
            //        {
            //            int countLast = (checkListFlow.lstClItems.Count) - 1;
            //            for (int i = 0; i < checkListFlow.lstClItems.Count; i++)
            //            {
            //                dbCommand.Parameters.Add("I_CL_Flow_ID", SqlDbType.Int).Value = checkListFlow.cLFlowID;
            //                dbCommand.Parameters.Add("I_CL_ID", SqlDbType.Int).Value = checkListFlow.cLID;
            //                dbCommand.Parameters.Add("I_CLG_ID", SqlDbType.Int).Value = checkListFlow.lstClItems[i].cLGID;
            //                dbCommand.Parameters.Add("I_CLItem_ID", SqlDbType.Int).Value =
            //                    checkListFlow.lstClItems[i].cLItemID;
            //                dbCommand.Parameters.Add("I_Item_IsAvailable", SqlDbType.Char).Value
            //                    = (checkListFlow.lstClItems[i].isCLItemAvailable == 0) ? 'Y' : 'N';
            //                dbCommand.Parameters.Add("I_Comments", SqlDbType.NVarChar).Value =
            //                    checkListFlow.lstClItems[i].comment;
            //                dbCommand.Parameters.Add("I_HasAttachment", SqlDbType.Char).Value =
            //                    (checkListFlow.lstClItems[i].hasAttachment) ? 'Y' : 'N';
            //                dbCommand.Parameters.Add("I_Attachment_Name", SqlDbType.NVarChar).Value =
            //                    (checkListFlow.lstClItems[i].hasAttachment) ? checkListFlow.lstClItems[i].attachmentName : "";
            //                dbCommand.Parameters.Add("I_Attachment_Path", SqlDbType.NVarChar).Value =
            //                    (checkListFlow.lstClItems[i].hasAttachment) ? checkListFlow.lstClItems[i].attachmentPath : "";
            //                dbCommand.Parameters.Add("I_CLMaker_Action", SqlDbType.Char).Value = (i == countLast) ? 'Y' : 'N';
            //                dbCommand.Parameters.Add("I_MAKER", SqlDbType.NVarChar).Value = checkListFlow.makerID;

            //                dbCommand.ExecuteNonQuery();
            //                dbCommand.Parameters.Clear();
            //            }

            //            transaction.Commit();
            //        }
            //        catch (Exception ex)
            //        {
            //            transaction.Rollback();
            //            throw ex;
            //        }
            //        finally
            //        {
            //            dbCommand.Dispose();
            //            dbConnection.Close();
            //        }
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

            return response;
        }

        public Vw_CheckList_Flow viewCLCheker(int CLID, int userID)
        {
            Vw_CheckList_Flow viewResult = null;

            try
            {
                var checkFlowMaster = ctx.CHECKLIST_FLOW_MASTER.FirstOrDefault(s => s.ID == CLID &&
                                                                        (s.QALABID == userID || s.SUPERVISORENGID == userID || s.REPRESENTITIVESUPERID == userID)
                                                                        && (s.STATUS == 1 || s.STATUS == 2 || s.STATUS == 3 || s.STATUS == 4));
                if (checkFlowMaster != null)
                {
                    viewResult = new Vw_CheckList_Flow()
                    {
                        cLFlowID = CLID,
                        CLParty = checkFlowMaster.CL_PARTY,
                        registrationDate = checkFlowMaster.REGISTRATIONDATE.HasValue ? checkFlowMaster.REGISTRATIONDATE.Value : default(DateTime)
                    };

                    var query =
                    from checkList in ctx.CHECKLIST_FLOW
                    join chgroup in ctx.CHECK_LIST_GROUPS on checkList.CL_GROUP_ID equals chgroup.CHECK_LIST_GROUPS_ID
                    join item in ctx.CL_GROUPS_ITEMS on checkList.CL_ITEM_ID equals item.CL_GROUPS_ITEMS_ID
                    join attach in ctx.ATTACHMENTS on checkList.ATTACHMENT_ID equals attach.ATTACHMENT_ID
                    where attach.PARENT_SUB_ID == chgroup.CHECK_LIST_GROUPS_ID && attach.TYPE == "CheckList_Maker" && attach.PARENT_ID == checkFlowMaster.ID
                    select new Ent_CheckList_Flow()
                    {
                        cLGID = chgroup.CHECK_LIST_GROUPS_ID,
                        cLGName = chgroup.NAME,
                        cLItemName = item.NAME,
                        isCLItemAvailable = checkList.AVAILABLE_ITEM == true ? 1 : 0,
                        comment = checkList.COMMENTS,
                        attachmentName = attach.ATTACHEMENT_NAME,
                        attachmentPath = attach.ATTACHEMENT_PATH,

                    };

                    viewResult.lstClItems = query.ToList();
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
            //        dbCommand.CommandText = "SP_CHECKLISTFlow_ViewChecker";
            //        dbCommand.CommandType = CommandType.StoredProcedure;

            //        dbCommand.Parameters.Add("I_CLFlow_ID", SqlDbType.Int).Value = CLID;
            //        dbCommand.Parameters.Add("I_User_ID", SqlDbType.Int).Value = userID;
            //        dbCommand.Parameters.Add("I_CL_PARTY", SqlDbType.NVarChar, 250).Direction = ParameterDirection.Output;
            //        dbCommand.Parameters.Add("I_REGISTRATIONDATE", SqlDbType.DateTime).Direction = ParameterDirection.Output;
            //        dbCommand.Parameters.Add("searchResultCursor", SqlDbType.Int).Direction = ParameterDirection.Output;

            //        dbConnection.Open();
            //        SqlDataReader reader = dbCommand.ExecuteReader(CommandBehavior.CloseConnection);

            //        viewResult = new Vw_CheckList_Flow()
            //        {
            //            cLFlowID = CLID,
            //            CLParty = dbCommand.Parameters["I_CL_PARTY"].Value.ToString(),
            //            registrationDate = DateTime.Parse(dbCommand.Parameters["I_REGISTRATIONDATE"].Value.ToString())
            //        };

            //        viewResult.lstClItems = new List<Ent_CheckList_Flow>();
            //        while (reader.Read())
            //        {
            //            viewResult.lstClItems.Add(new Ent_CheckList_Flow()
            //            {
            //                cLGID = reader.GetInt32(reader.GetOrdinal("CL_GROUP_ID")),
            //                cLGName = reader.GetString(reader.GetOrdinal("CLG_Name")),
            //                cLItemID = reader.GetInt32(reader.GetOrdinal("CL_ITEM_ID")),
            //                cLItemName = reader.GetString(reader.GetOrdinal("CL_Item_Name")),
            //                isCLItemAvailable = (reader.GetString(reader.GetOrdinal("AVAILABLE_ITEM")).Equals("Y")) ? 0 : 1,
            //                comment = reader.GetString(reader.GetOrdinal("COMMENTS")),
            //                attachmentName = (reader.IsDBNull(reader.GetOrdinal("ATTACHEMENT_NAME")))
            //                    ? "" : reader.GetString(reader.GetOrdinal("ATTACHEMENT_NAME")),
            //                attachmentPath = (reader.IsDBNull(reader.GetOrdinal("ATTACHEMENT_PATH")))
            //                    ? "" : reader.GetString(reader.GetOrdinal("ATTACHEMENT_PATH"))
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

            
        }

        public ResponseMessage checkListCheker(int CLID, int userID, bool actionIsAccept)
        {
            ResponseMessage response = new ResponseMessage();



            try
            {
                var checkFlowMaster = ctx.CHECKLIST_FLOW_MASTER.FirstOrDefault(s => s.ID == CLID &&
                                                                  (s.QALABID == userID || s.SUPERVISORENGID == userID || s.REPRESENTITIVESUPERID == userID)
                                                                  && (s.STATUS == 1 || s.STATUS == 2 || s.STATUS == 3 || s.STATUS == 4));

                if (checkFlowMaster !=null)
                {
                    var userProfile = ctx.USERS_PROFILE.FirstOrDefault(s => s.MAKER_ID == userID.ToString());
                    if (userProfile != null)
                    {
                        var type = ctx.USER_PROFILE_TYPE.FirstOrDefault(s => s.TYPE_ID == userProfile.USER_TYPE_ID);
                        if ( type.TYPE_CODE == "SE")
                        {
                            checkFlowMaster.STATUS = 3;
                            checkFlowMaster.CLOSURENDATE = null;
                            checkFlowMaster.SUPERENG_ACTION_DATE = DateTime.Now;
                            checkFlowMaster.SUPERENG_ACTION = (actionIsAccept)? "CHECKER-ACCEPT" : "CHECKER-REJECT";
                            ctx.SaveChanges();

                        }

                        if (type.TYPE_CODE == "QL")
                        {
                            checkFlowMaster.STATUS = 4;
                            checkFlowMaster.CLOSURENDATE = null;
                            checkFlowMaster.QALAB_ACTION_DATE = DateTime.Now;
                            checkFlowMaster.QALAB_ACTION = (actionIsAccept) ? "CHECKER-ACCEPT" : "CHECKER-REJECT";
                            ctx.SaveChanges();

                        }

                        if (type.TYPE_CODE == "RS")
                        {
                            checkFlowMaster.STATUS = (actionIsAccept)?7:8;
                            checkFlowMaster.RS_ACTION = (actionIsAccept) ? "CHECKER-ACCEPT" : "CHECKER-REJECT";
                            checkFlowMaster.RS_ACTION_DATE = DateTime.Now;
                            checkFlowMaster.CLOSURENDATE = DateTime.Now;
                            ctx.SaveChanges();

                        }
                    }
                }

            }
            catch (Exception)
            {

                throw;
            }


            try
            {
                using (SqlConnection dbConnection = new SqlConnection(
                    ConfigurationManager.ConnectionStrings["dbConnectionString"].ConnectionString))
                using (SqlCommand dbCommand = dbConnection.CreateCommand())
                {
                    dbCommand.CommandText = "SP_CHECKLIST_Flow_Checker";
                    dbCommand.CommandType = CommandType.StoredProcedure;
                    dbCommand.Parameters.Add("I_CLFlow_ID", SqlDbType.Int).Value = CLID;
                    dbCommand.Parameters.Add("I_CLChecker_Action", SqlDbType.Char).Value = (actionIsAccept) ? 'A' : 'R';
                    dbCommand.Parameters.Add("I_User_ID", SqlDbType.NVarChar).Value = userID;

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
    }
}