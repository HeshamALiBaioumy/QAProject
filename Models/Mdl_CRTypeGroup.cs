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
    public class Mdl_CRTypeGroup
    {
        public QualityDbEntities ctx = new QualityDbEntities();
        public ResponseMessage insert_updateCRTypeGroup(Ent_CRTypeGroup cRTypeGroup, bool isUpdateForm)
        {
            ResponseMessage response = new ResponseMessage();

            try
            {
                var entity = new CR_TYPE_GROUPS();
                if (cRTypeGroup.ID > 0)
                    entity = ctx.CR_TYPE_GROUPS.FirstOrDefault(d => d.CR_TYPE_GROUPS_ID == cRTypeGroup.ID);



                entity.IS_ACTIVE = cRTypeGroup.isActive;
                entity.MAKER = cRTypeGroup.makerID;
                entity.MAKER_DAT_TIM = DateTime.Now;
                entity.CRTMC_ID = cRTypeGroup.CRTMCID;
                entity.NAME = cRTypeGroup.name;
                entity.DESCRIPTION = cRTypeGroup.description;

                if (cRTypeGroup.ID <= 0)
                    ctx.CR_TYPE_GROUPS.Add(entity);

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

        public List<Ent_CRTypeGroup> searchCRTypeGroup(string searchName, string searchDescription, int searchCRMCID
            , int searchIsActive)
        {
            List<Ent_CRTypeGroup> searchResult = new List<Ent_CRTypeGroup>();

            try
            {

                searchResult = (from tg in ctx.CR_TYPE_GROUPS
                                join mc in ctx.CR_TYPES_MAIN_CATEGORIES on tg.CRTMC_ID equals mc.CR_TYPE_MC_ID
                                select new Ent_CRTypeGroup
                                {
                                    ID = tg.CR_TYPE_GROUPS_ID,
                                    CRTMCID = tg.CRTMC_ID.Value,
                                    CRTMCName = mc.NAME,
                                    name = tg.NAME,
                                    description = tg.DESCRIPTION,
                                    isActive = tg.IS_ACTIVE
                                }).ToList();

                
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return searchResult;
        }

        public Ent_CRTypeGroup viewCRTypeGroup(int CR_TYPE_GROUPS_ID)
        {
            Ent_CRTypeGroup viewResult = new Ent_CRTypeGroup();

            try
            {
                viewResult = (from tg in ctx.CR_TYPE_GROUPS
                                join mc in ctx.CR_TYPES_MAIN_CATEGORIES on tg.CRTMC_ID equals mc.CR_TYPE_MC_ID
                                select new Ent_CRTypeGroup
                                {
                                    ID = tg.CR_TYPE_GROUPS_ID,
                                    CRTMCID = tg.CRTMC_ID.Value,
                                    CRTMCName = mc.NAME,
                                    name = tg.NAME,
                                    description = tg.DESCRIPTION,
                                    isActive = tg.IS_ACTIVE
                                }).FirstOrDefault(d=>d.ID== CR_TYPE_GROUPS_ID);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return viewResult;
        }
    }
}