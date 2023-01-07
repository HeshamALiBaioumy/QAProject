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
    public class Mdl_CheckLists
    {
        public QualityDbEntities ctx = new QualityDbEntities();
        public ResponseMessage insert_updatCheckLists(Ent_CheckLists checkLists, bool isUpdateForm)
        {
            ResponseMessage response = new ResponseMessage();

            try
            {
                var entity = new CHECK_LIST();
                if (checkLists.ID > 0)
                    entity = ctx.CHECK_LIST.FirstOrDefault(d => d.CHECK_LIST_ID == checkLists.ID);

                entity.NAME = checkLists.name;
                entity.MAKER_DAT_TIM = DateTime.Now;
                entity.DESCRIPTION = checkLists.description;
                entity.IS_ACTIVE = checkLists.isActive;
                entity.MAKER = checkLists.makerID;
                entity.GROUPS_ID = String.Join(",", checkLists.lstCLGroupIDs.ToArray());

                if (checkLists.ID <= 0)
                    ctx.CHECK_LIST.Add(entity);

                ctx.SaveChanges();
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
            //        dbCommand.CommandText = "SP_CHECK_LIST";
            //        dbCommand.CommandType = CommandType.StoredProcedure;
            //        dbCommand.Parameters.Add("INPUT_ID", SqlDbType.Int).Value = checkLists.ID;
            //        dbCommand.Parameters.Add("I_NAME", SqlDbType.NVarChar).Value = checkLists.name;
            //        dbCommand.Parameters.Add("I_DESCRIPTION", SqlDbType.NVarChar).Value = checkLists.description;
            //        dbCommand.Parameters.Add("I_GROUPS_ID", SqlDbType.NVarChar).Value =
            //        String.Join(",", checkLists.lstCLGroupIDs.ToArray());
            //        dbCommand.Parameters.Add("I_IS_ACTIVE", SqlDbType.Char).Value = (checkLists.isActive) ? 'Y' : 'N';
            //        dbCommand.Parameters.Add("I_MAKER", SqlDbType.NVarChar).Value = checkLists.makerID;
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

        public List<Ent_CheckLists> searchCheckListt(string searchName, string searchDescription
            , List<int> searchLstCLGroupIDs, int searchIsActive)
        {
            List<Ent_CheckLists> searchResult = new List<Ent_CheckLists>();

            try
            {
                searchResult = ctx.CHECK_LIST.Where(s => s.NAME.Contains(searchName) ||
                                    s.DESCRIPTION.Contains(searchDescription) ||
                                        s.IS_ACTIVE == (searchIsActive == 0 ? false : true)).ToList().Select(mod => new Ent_CheckLists
                                        {
                                            ID = mod.CHECK_LIST_ID,
                                            lstCLGroupIDs = string.IsNullOrEmpty(mod.GROUPS_ID) ? new List<int>()
                                                            : mod.GROUPS_ID.Split(',').Select(Int32.Parse).ToList(),
                                            name = mod.NAME,
                                            description = mod.DESCRIPTION,
                                            isActive = mod.IS_ACTIVE,
                                            lstCLGroupNames = GroupsName(mod.GROUPS_ID)
                                        }).ToList();
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
            //        dbCommand.CommandText = "SP_CHECK_LIST_SEARCH";
            //        dbCommand.CommandType = CommandType.StoredProcedure;
            //        dbCommand.Parameters.Add("I_SearchName", SqlDbType.NVarChar).Value = searchName ?? "";
            //        dbCommand.Parameters.Add("I_SearchDescription", SqlDbType.NVarChar).Value = searchDescription ?? "";
            //        dbCommand.Parameters.Add("I_IS_ACTIVE", SqlDbType.NVarChar).Value = (searchIsActive == 0) ? 'Y' : (searchIsActive == 1) ? 'N' : 'E';
            //        dbCommand.Parameters.Add("searchResultCursor", SqlDbType.Int).Direction = ParameterDirection.Output;

            //        dbConnection.Open();
            //        SqlDataReader reader = dbCommand.ExecuteReader();
            //        while (reader.Read())
            //        {
            //            searchResult.Add(new Ent_CheckLists()
            //            {
            //                ID = reader.GetInt32(reader.GetOrdinal("CHECK_LIST_ID")),
            //                name = reader.GetString(reader.GetOrdinal("NAME")),
            //                description = reader.GetString(reader.GetOrdinal("DESCRIPTION")),
            //                lstCLGroupIDs = (reader.GetString(reader.GetOrdinal("GROUPS_ID"))).Split(',')
            //                    .Select(Int32.Parse).ToList(),
            //                lstCLGroupNames = reader.GetString(reader.GetOrdinal("GROUPS_Name")),
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

        private  string GroupsName(string gROUPS_ID)
        {
            var groupIds = string.IsNullOrEmpty(gROUPS_ID) ? new List<int>()
                           : gROUPS_ID.Split(',').Select(Int32.Parse).ToList();
            if (groupIds.Count > 0)
            {
                var groupNames = ctx.CHECK_LIST_GROUPS.Where(s => groupIds.Contains(s.CHECK_LIST_GROUPS_ID)).Select(s => s.NAME).ToList();
                return String.Join(",", groupNames.ToArray());
            }
            return "";
        }

        public Ent_CheckLists viewCheckList(int CheckListID)
        {
            Ent_CheckLists viewResult = new Ent_CheckLists();

            var query = ctx.CHECK_LIST.FirstOrDefault(s => s.CHECK_LIST_ID == CheckListID);

            if (query != null)
            {
                viewResult.ID = query.CHECK_LIST_ID;
                viewResult.name = query.NAME;
                viewResult.description = query.DESCRIPTION;
                viewResult.isActive = query.IS_ACTIVE;
                viewResult.lstCLGroupIDs = string.IsNullOrEmpty(query.GROUPS_ID)? new List<int>(): query.GROUPS_ID.Split(',')
                                            .Select(Int32.Parse).ToList();
                if (viewResult.lstCLGroupIDs.Any())
                {
                    var groupNames = ctx.CHECK_LIST_GROUPS.Where(s => viewResult.lstCLGroupIDs.Contains(s.CHECK_LIST_GROUPS_ID)).Select(s => s.NAME).ToList();
                    viewResult.lstCLGroupNames = String.Join(",", groupNames.ToArray());
                }
             
            }
            //try
            //{
            //    using (SqlConnection dbConnection = new SqlConnection(
            //        ConfigurationManager.ConnectionStrings["dbConnectionString"].ConnectionString))
            //    using (SqlCommand dbCommand = dbConnection.CreateCommand())
            //    {
            //        dbCommand.CommandText = "SP_CHECK_LIST_VIEW";
            //        dbCommand.CommandType = CommandType.StoredProcedure;
            //        dbCommand.Parameters.Add("input_id", SqlDbType.NVarChar).Value = CheckListID;
            //        dbCommand.Parameters.Add("viewResultCursor", SqlDbType.Int).Direction = ParameterDirection.Output;

            //        dbConnection.Open();
            //        SqlDataReader reader = dbCommand.ExecuteReader();
            //        if (reader.Read())
            //        {
            //            viewResult = new Ent_CheckLists()
            //            {
            //                ID = reader.GetInt32(reader.GetOrdinal("CHECK_LIST_ID")),
            //                name = reader.GetString(reader.GetOrdinal("NAME")),
            //                description = reader.GetString(reader.GetOrdinal("DESCRIPTION")),
            //                lstCLGroupIDs = (reader.GetString(reader.GetOrdinal("GROUPS_ID"))).Split(',')
            //                    .Select(Int32.Parse).ToList(),
            //                lstCLGroupNames = reader.GetString(reader.GetOrdinal("GROUPS_Name")),
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