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
    public class Mdl_CRType
    {

        public QualityDbEntities ctx = new QualityDbEntities();
        public ResponseMessage insert_updateCRType(Ent_CRType cRType, bool isUpdateForm)
        {
            ResponseMessage response = new ResponseMessage();

            try
            {
                var entity = new CR_TYPES();
                if (cRType.ID > 0)
                    entity = ctx.CR_TYPES.FirstOrDefault(d => d.CR_TYPE_ID == cRType.ID);



                entity.IS_ACTIVE = cRType.isActive;
                entity.MAKER = cRType.makerID;
                entity.MAKER_DAT_TIM = DateTime.Now;
                entity.CRTG_ID = cRType.groupID;                
                entity.NAME = cRType.name;
                entity.DESCRIPTION = cRType.description;
                entity.CATEGORY = cRType.CRcategory;

                if (cRType.ID <= 0)
                    ctx.CR_TYPES.Add(entity);

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

        public List<Ent_CRType> searchCRType(string searchName, string searchDescription
            , int searchCRTMCID, int searchgroupID, int searchCategoryID, int searchIsActive)
        {
            List<Ent_CRType> searchResult = new List<Ent_CRType>();

            try
            {
                searchResult = (from tg in ctx.CR_TYPE_GROUPS
                                join mc in ctx.CR_TYPES_MAIN_CATEGORIES on tg.CRTMC_ID equals mc.CR_TYPE_MC_ID
                                join t in ctx.CR_TYPES on tg.CR_TYPE_GROUPS_ID equals t.CRTG_ID
                                select new Ent_CRType
                                {
                                    ID = t.CR_TYPE_ID,
                                    CRTMCID = mc.CR_TYPE_MC_ID,
                                    CRTMCName = mc.NAME,
                                    groupID = tg.CR_TYPE_GROUPS_ID,
                                    groupname = tg.NAME,
                                    name = t.NAME,
                                    description = t.DESCRIPTION,
                                    CRcategory = t.CATEGORY.Value,
                                    isActive = t.IS_ACTIVE
                                }).ToList();

               
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return searchResult;
        }

        public Ent_CRType viewCRType(int CRTypeID)
        {
            Ent_CRType viewResult = new Ent_CRType();

            try
            {
                viewResult = (from tg in ctx.CR_TYPE_GROUPS
                                join mc in ctx.CR_TYPES_MAIN_CATEGORIES on tg.CRTMC_ID equals mc.CR_TYPE_MC_ID
                                join t in ctx.CR_TYPES on tg.CR_TYPE_GROUPS_ID equals t.CRTG_ID
                                select new Ent_CRType
                                {
                                    ID = t.CR_TYPE_ID,
                                    CRTMCID = mc.CR_TYPE_MC_ID,
                                    CRTMCName = mc.NAME,
                                    groupID = tg.CR_TYPE_GROUPS_ID,
                                    groupname = tg.NAME,
                                    name = t.NAME,
                                    description = t.DESCRIPTION,
                                    CRcategory = t.CATEGORY.Value,
                                    isActive = t.IS_ACTIVE
                                }).FirstOrDefault(d=>d.ID==CRTypeID);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return viewResult;
        }

        public bool viewCRType_SampleRequired(int CRTypeID)
        {
            bool isSampleRequired = false;

            try
            {
                using (SqlConnection dbConnection = new SqlConnection(
                    ConfigurationManager.ConnectionStrings["dbConnectionString"].ConnectionString))
                using (SqlCommand dbCommand = dbConnection.CreateCommand())
                {
                    dbCommand.CommandText = "SP_CR_TYPES_ChckSmplRquired";
                    dbCommand.CommandType = CommandType.StoredProcedure;
                    dbCommand.Parameters.Add("input_id", SqlDbType.Int).Value = CRTypeID;
                    dbCommand.Parameters.Add("isSampleRequired", SqlDbType.Int).Direction = ParameterDirection.Output;

                    dbConnection.Open();
                    dbCommand.ExecuteNonQuery();

                    isSampleRequired = (dbCommand.Parameters["isSampleRequired"].Value.ToString() == "0") ? true : false;

                    dbCommand.Dispose();
                    dbConnection.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return isSampleRequired;
        }
    }
}