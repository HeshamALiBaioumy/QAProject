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
    public class Mdl_CheckListItemGroup
    {
        public QualityDbEntities ctx = new QualityDbEntities();
        public ResponseMessage insert_updatCheckListItemGroup(Ent_CheckListItemGroup checkListItemGroup, bool isUpdateForm)
        {
            ResponseMessage response = new ResponseMessage();

            try
            {
                var entity = new CHECK_LIST_GROUPS();
                if (checkListItemGroup.ID > 0)
                    entity = ctx.CHECK_LIST_GROUPS.FirstOrDefault(d => d.CHECK_LIST_GROUPS_ID == checkListItemGroup.ID);



                entity.ITEMS_ID = String.Join(",", checkListItemGroup.lstCLItemIDs.ToArray());
                entity.NAME = checkListItemGroup.name;
                entity.MAKER_DAT_TIM = DateTime.Now;
                entity.DESCRIPTION = checkListItemGroup.description;
                entity.IS_ACTIVE = checkListItemGroup.isActive;
                entity.MAKER = checkListItemGroup.makerID;

                if (checkListItemGroup.ID <= 0)
                    ctx.CHECK_LIST_GROUPS.Add(entity);

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

            //try
            //{
            //    using (SqlConnection dbConnection = new SqlConnection(
            //        ConfigurationManager.ConnectionStrings["dbConnectionString"].ConnectionString))
            //    using (SqlCommand dbCommand = dbConnection.CreateCommand())
            //    {
            //        dbCommand.CommandText = "SP_CHECK_LIST_GROUPS";
            //        dbCommand.CommandType = CommandType.StoredProcedure;
            //        dbCommand.Parameters.Add("INPUT_ID", SqlDbType.Int).Value = checkListItemGroup.ID;
            //        dbCommand.Parameters.Add("I_ITEMS_ID", SqlDbType.NVarChar).Value =
            //            String.Join(",", checkListItemGroup.lstCLItemIDs.ToArray());
            //        dbCommand.Parameters.Add("I_Name", SqlDbType.NVarChar).Value = checkListItemGroup.name;
            //        dbCommand.Parameters.Add("I_DESCRIPTION", SqlDbType.NVarChar).Value = checkListItemGroup.description;
            //        dbCommand.Parameters.Add("I_IS_ACTIVE", SqlDbType.Char).Value = (checkListItemGroup.isActive) ? 'Y' : 'N';
            //        dbCommand.Parameters.Add("I_MAKER", SqlDbType.NVarChar).Value = checkListItemGroup.makerID;
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

            return response;
        }

        public List<Ent_CheckListItemGroup> searchMCheckListItemGroup(string SearchName, string SearchDescription
            , List<int> searchLstCLItemIDs, int searchIsActive)
        {
            List<Ent_CheckListItemGroup> searchResult = new List<Ent_CheckListItemGroup>();

            searchResult = ctx.CHECK_LIST_GROUPS.Where(s => s.NAME.Contains(SearchName) || s.DESCRIPTION.Contains(SearchDescription)
            || s.IS_ACTIVE == (searchIsActive == 0 ? false : true)).ToList().Select(mod => new Ent_CheckListItemGroup
            {
                ID = mod.CHECK_LIST_GROUPS_ID,
                lstCLItemIDs = string.IsNullOrEmpty(mod.ITEMS_ID) ? new List<int>() : mod.ITEMS_ID.Split(',').Select(Int32.Parse).ToList(),
                name = mod.NAME,
                description = mod.DESCRIPTION,
                isActive = mod.IS_ACTIVE,
                lstCLItemNames = GroupsName(mod.ITEMS_ID),
            }).ToList();
            //try
            //{
            //    using (SqlConnection dbConnection = new SqlConnection(
            //        ConfigurationManager.ConnectionStrings["dbConnectionString"].ConnectionString))
            //    using (SqlCommand dbCommand = dbConnection.CreateCommand())
            //    {
            //        dbCommand.CommandText = "SP_CHECK_LIST_GROUPS_SEARCH";
            //        dbCommand.CommandType = CommandType.StoredProcedure;
            //        dbCommand.Parameters.Add("I_SearchName", SqlDbType.NVarChar).Value = SearchName ?? "";
            //        dbCommand.Parameters.Add("I_SearchDescription", SqlDbType.NVarChar).Value = SearchDescription ?? "";
            //        dbCommand.Parameters.Add("I_IS_ACTIVE", SqlDbType.NVarChar).Value = (searchIsActive == 0) ? 'Y' : (searchIsActive == 1) ? 'N' : 'E';
            //        dbCommand.Parameters.Add("searchResultCursor", SqlDbType.Int).Direction = ParameterDirection.Output;

            //        dbConnection.Open();
            //        SqlDataReader reader = dbCommand.ExecuteReader();
            //        while (reader.Read())
            //        {
            //            searchResult.Add(new Ent_CheckListItemGroup()
            //            {
            //                ID = reader.GetInt32(reader.GetOrdinal("CHECK_LIST_GROUPS_ID")),
            //                name = reader.GetString(reader.GetOrdinal("NAME")),
            //                description = reader.GetString(reader.GetOrdinal("DESCRIPTION")),
            //                lstCLItemIDs = (reader.GetString(reader.GetOrdinal("ITEMS_ID"))).Split(',').Select(Int32.Parse).ToList(),
            //                lstCLItemNames = reader.GetString(reader.GetOrdinal("ITEMS_Name")),
            //                isActive = (reader.GetString(reader.GetOrdinal("IS_ACTIVE")) == "Y") ? true : false
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
        private string GroupsName(string ITEMS_ID)
        {
            var groupIds = string.IsNullOrEmpty(ITEMS_ID) ? new List<int>()
                           : ITEMS_ID.Split(',').Select(Int32.Parse).ToList();
            if (groupIds.Count > 0)
            {
                var groupNames = ctx.CL_GROUPS_ITEMS.Where(s => groupIds.Contains(s.CL_GROUPS_ITEMS_ID)).Select(s => s.NAME).ToList();
                return String.Join(",", groupNames.ToArray());
            }
            return "";
        }
        public Ent_CheckListItemGroup viewCheckListITemGroup(int CheckListITemGroupID)
        {
            Ent_CheckListItemGroup viewResult = new Ent_CheckListItemGroup();

            try
            {
                var query = ctx.CHECK_LIST_GROUPS.FirstOrDefault(s => s.CHECK_LIST_GROUPS_ID == CheckListITemGroupID);

                if (query != null)
                {
                    viewResult.ID = query.CHECK_LIST_GROUPS_ID;
                    viewResult.lstCLItemIDs =string.IsNullOrEmpty(query.ITEMS_ID)?new List<int>() :query.ITEMS_ID.Split(',').Select(Int32.Parse).ToList();
                    viewResult.name = query.NAME;
                    viewResult.description = query.DESCRIPTION;
                    viewResult.isActive = query.IS_ACTIVE;
                    if (viewResult.lstCLItemIDs.Any())
                    {
                        var itemNames = ctx.CL_GROUPS_ITEMS.Where(s => viewResult.lstCLItemIDs.Contains(s.CL_GROUPS_ITEMS_ID)).Select(s => s.NAME).ToList();
                        viewResult.lstCLItemNames = String.Join(",", itemNames.ToArray());
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
            //        dbCommand.CommandText = "SP_CHECK_LIST_GROUPS_VIEW";
            //        dbCommand.CommandType = CommandType.StoredProcedure;
            //        dbCommand.Parameters.Add("input_id", SqlDbType.NVarChar).Value = CheckListITemGroupID;
            //        dbCommand.Parameters.Add("viewResultCursor", SqlDbType.Int).Direction = ParameterDirection.Output;

            //        dbConnection.Open();
            //        SqlDataReader reader = dbCommand.ExecuteReader();
            //        if (reader.Read())
            //        {
            //            viewResult = new Ent_CheckListItemGroup()
            //            {
            //                ID = reader.GetInt32(reader.GetOrdinal("CHECK_LIST_GROUPS_ID")),
            //                name = reader.GetString(reader.GetOrdinal("NAME")),
            //                description = reader.GetString(reader.GetOrdinal("DESCRIPTION")),
            //                lstCLItemIDs = (reader.GetString(reader.GetOrdinal("ITEMS_ID"))).Split(',').Select(Int32.Parse).ToList(),
            //                lstCLItemNames = reader.GetString(reader.GetOrdinal("ITEMS_Name")),
            //                isActive = (reader.GetString(reader.GetOrdinal("IS_ACTIVE")) == "Y") ? true : false
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