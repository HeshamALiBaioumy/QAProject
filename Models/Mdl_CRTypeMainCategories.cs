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
    public class Mdl_CRTypeMainCategories
    {
        public QualityDbEntities ctx = new QualityDbEntities();

        public ResponseMessage insert_updateCRTypeMC(Ent_CRTypeMainCategories cRTypeMainCategories, bool isUpdateForm)
        {
            ResponseMessage response = new ResponseMessage();

            try
            {
                var entity = new CR_TYPES_MAIN_CATEGORIES();
                if (cRTypeMainCategories.ID > 0)
                    entity = ctx.CR_TYPES_MAIN_CATEGORIES.FirstOrDefault(d => d.CR_TYPE_MC_ID == cRTypeMainCategories.ID);



                entity.IS_ACTIVE = entity.IS_ACTIVE;
                entity.MAKER = cRTypeMainCategories.makerID;
                entity.MAKER_DAT_TIM = DateTime.Now;
                entity.NAME = cRTypeMainCategories.name;
                entity.DESCRIPTION = cRTypeMainCategories.description;

                if (cRTypeMainCategories.ID  <= 0)
                    ctx.CR_TYPES_MAIN_CATEGORIES.Add(entity);

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

        public List<Ent_CRTypeMainCategories> searchCRTypeMainCategories(string searchName, string searchDescription
            , int searchIsActive)
        {
            List<Ent_CRTypeMainCategories> searchResult = new List<Ent_CRTypeMainCategories>();

            try
            {
                searchResult = ctx.CR_TYPES_MAIN_CATEGORIES.Select(entity => new Ent_CRTypeMainCategories
                {

                    ID = entity.CR_TYPE_MC_ID,
                    name = entity.NAME,
                    isActive = entity.IS_ACTIVE,
                    description = entity.DESCRIPTION,
                    makerID = entity.MAKER
                }).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return searchResult;
        }

        public Ent_CRTypeMainCategories viewCRTypeMainCategories(int CR_TYPE_MC_ID)
        {
            Ent_CRTypeMainCategories viewResult = new Ent_CRTypeMainCategories();

            try
            {
                viewResult = ctx.CR_TYPES_MAIN_CATEGORIES.Select(entity => new Ent_CRTypeMainCategories
                {

                    ID = entity.CR_TYPE_MC_ID,
                    name = entity.NAME,
                    isActive = entity.IS_ACTIVE,
                    description = entity.DESCRIPTION,
                    makerID = entity.MAKER
                }).FirstOrDefault(d=>d.ID== CR_TYPE_MC_ID);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return viewResult;
        }
    }
}