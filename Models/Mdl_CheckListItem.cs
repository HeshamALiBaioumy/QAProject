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
    public class Mdl_CheckListItem
    {
        public QualityDbEntities ctx = new QualityDbEntities();
        public ResponseMessage insert_updatCheckListItem(Ent_CheckListItem checkListItem, bool isUpdateForm)
        {
            ResponseMessage response = new ResponseMessage();

            try
            {

                var entity = ctx.CL_GROUPS_ITEMS.FirstOrDefault(d => d.CL_GROUPS_ITEMS_ID == checkListItem.checkListItemID);
                if (entity == null)
                {
                    entity = new CL_GROUPS_ITEMS();
                    entity.NAME = checkListItem.name;
                    entity.DESCRIPTION = checkListItem.description;
                    entity.MAKER = checkListItem.makerID;
                    entity.IS_ACTIVE = checkListItem.isActive ;
                    ctx.CL_GROUPS_ITEMS.Add(entity);

                }
                else
                {
                    entity.NAME = checkListItem.name;
                    entity.DESCRIPTION = checkListItem.description;
                    entity.MAKER = checkListItem.makerID;
                    entity.IS_ACTIVE = checkListItem.isActive;

                }
                ctx.SaveChanges();

                //using (SqlConnection dbConnection = new SqlConnection(
                //    ConfigurationManager.ConnectionStrings["dbConnectionString"].ConnectionString))
                //using (SqlCommand dbCommand = dbConnection.CreateCommand())
                //{
                //    dbCommand.CommandText = "SP_CL_GROUPS_ITEMS";
                //    dbCommand.CommandType = CommandType.StoredProcedure;
                //    dbCommand.Parameters.Add("INPUT_D", SqlDbType.Int).Value = checkListItem.checkListItemID;
                //    dbCommand.Parameters.Add("I_NAME", SqlDbType.NVarChar).Value = checkListItem.name;
                //    dbCommand.Parameters.Add("I_DESCRIPTION", SqlDbType.NVarChar).Value = checkListItem.description;
                //    dbCommand.Parameters.Add("I_IS_ACTIVE", SqlDbType.Char).Value = (checkListItem.isActive) ? 'Y' : 'N';
                //    dbCommand.Parameters.Add("I_MAKER", SqlDbType.NVarChar).Value = checkListItem.makerID;
                //    dbCommand.Parameters.Add("ACTION", SqlDbType.NVarChar).Value = (isUpdateForm) ? "UPDATE" : "NEW";

                //    dbConnection.Open();
                //    dbCommand.ExecuteNonQuery();
                //    dbCommand.Dispose();
                //    dbConnection.Close();
                //}

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

        public List<Ent_CheckListItem> searchCheckListItem(string searchName, string searchDescription, int searchIsActive)
        {
            List<Ent_CheckListItem> searchResult = new List<Ent_CheckListItem>();

            try
            {
                searchResult= ctx.CL_GROUPS_ITEMS.Where(d => d.NAME == searchName).Select(s=> new Ent_CheckListItem {

                    checkListItemID = s.CL_GROUPS_ITEMS_ID,
                    name = s.NAME,
                    description = s.DESCRIPTION

                }).ToList();

                //using (SqlConnection dbConnection = new SqlConnection(
                //    ConfigurationManager.ConnectionStrings["dbConnectionString"].ConnectionString))
                //using (SqlCommand dbCommand = dbConnection.CreateCommand())
                //{
                //    dbCommand.CommandText = "SP_CL_GROUPS_ITEMS_SEARCH";
                //    dbCommand.CommandType = CommandType.StoredProcedure;
                //    dbCommand.Parameters.Add("I_SearchName", SqlDbType.NVarChar).Value = searchName ?? "";
                //    dbCommand.Parameters.Add("I_SearchDescription", SqlDbType.NVarChar).Value = searchDescription ?? "";
                //    dbCommand.Parameters.Add("I_IS_ACTIVE", SqlDbType.NVarChar).Value = (searchIsActive == 0) ? 'Y' : (searchIsActive == 1) ? 'N' : 'E';
                //    dbCommand.Parameters.Add("searchResultCursor", SqlDbType.Int).Direction = ParameterDirection.Output;

                //    dbConnection.Open();
                //    SqlDataReader reader = dbCommand.ExecuteReader();
                //    while (reader.Read())
                //    {
                //        searchResult.Add(new Ent_CheckListItem()
                //        {
                //            checkListItemID = reader.GetInt32(reader.GetOrdinal("CL_GROUPS_ITEMS_ID")),
                //            name = reader.GetString(reader.GetOrdinal("NAME")),
                //            description = reader.GetString(reader.GetOrdinal("DESCRIPTION")),
                //            isActive = (reader.GetString(reader.GetOrdinal("IS_ACTIVE")) == "Y") ? true : false
                //        });
                //    }
                //    reader.Close();
                //    dbCommand.Dispose();
                //    dbConnection.Close();
                //}
            }
            catch 
            {
               
            }

            return searchResult;
        }

        public Ent_CheckListItem viewCheckListItem(int CheckListItemID)
        {
            Ent_CheckListItem viewResult = new Ent_CheckListItem();

            try
            {
                var query = ctx.CL_GROUPS_ITEMS.FirstOrDefault(d => d.CL_GROUPS_ITEMS_ID == CheckListItemID);
                if (query != null)
                {
                    viewResult.checkListItemID = query.CL_GROUPS_ITEMS_ID;
                    viewResult.name = query.NAME;
                    viewResult.description = query.DESCRIPTION;
                    viewResult.isActive = query.IS_ACTIVE;
                }
                 
                //using (SqlConnection dbConnection = new SqlConnection(
                //    ConfigurationManager.ConnectionStrings["dbConnectionString"].ConnectionString))
                //using (SqlCommand dbCommand = dbConnection.CreateCommand())
                //{
                //    dbCommand.CommandText = "SP_CL_GROUPS_ITEMS_VIEW";
                //    dbCommand.CommandType = CommandType.StoredProcedure;
                //    dbCommand.Parameters.Add("input_id", SqlDbType.NVarChar).Value = CheckListItemID;
                //    dbCommand.Parameters.Add("viewResultCursor", SqlDbType.Int).Direction = ParameterDirection.Output;

                //    dbConnection.Open();
                //    SqlDataReader reader = dbCommand.ExecuteReader();
                //    if (reader.Read())
                //    {
                //        viewResult = new Ent_CheckListItem()
                //        {
                //            checkListItemID = reader.GetInt32(reader.GetOrdinal("CL_GROUPS_ITEMS_ID")),
                //            name = reader.GetString(reader.GetOrdinal("NAME")),
                //            description = reader.GetString(reader.GetOrdinal("DESCRIPTION")),
                //            isActive = (reader.GetString(reader.GetOrdinal("IS_ACTIVE")) == "Y") ? true : false
                //        };
                //    }
                //    reader.Close();
                //    dbCommand.Dispose();
                //    dbConnection.Close();
                //}
            }
            catch 
            {
               
            }

            return viewResult;
        }
    }
}