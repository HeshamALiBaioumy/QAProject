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
    public class Mdl_ProjectOwnerType
    {
        public QualityDbEntities ctx = new QualityDbEntities();

        public ResponseMessage insert_updateProjectOwnerType(Ent_ProjectOwnerType projeOwnerType, bool isUpdateForm)
        {
            ResponseMessage response = new ResponseMessage();

            try
            {
                var entity = new PROJECT_OWNER_TYPE();
                if (projeOwnerType.ProjectOwnerTypeID > 0)
                    entity = ctx.PROJECT_OWNER_TYPE.FirstOrDefault(d => d.PROJ_OWN_TYP_ID == projeOwnerType.ProjectOwnerTypeID);


              
                entity.MAKER = projeOwnerType.makerID;
                entity.MAKER_DAT_TIM = DateTime.Now;
                entity.NAME = projeOwnerType.name;
                entity.DESCRIPTION = projeOwnerType.description;
                entity.IS_VENDOR = projeOwnerType.isVendor;
                entity.IS_ACTIVE = (projeOwnerType.isActive);

                if (projeOwnerType.ProjectOwnerTypeID <= 0)
                    ctx.PROJECT_OWNER_TYPE.Add(entity);

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

        public List<Ent_ProjectOwnerType> searchProjectOwnerType(string searchName, string searchDescription
            , int searchIsVendor, int searchIsActive)
        {
            List<Ent_ProjectOwnerType> searchResult = new List<Ent_ProjectOwnerType>();

            try
            {
                searchResult = ctx.PROJECT_OWNER_TYPE.Select(entity => new Ent_ProjectOwnerType
                {

                    isActive = entity.IS_ACTIVE,
                    isVendor = entity.IS_VENDOR,

                    name = entity.NAME,
                    description = entity.DESCRIPTION,
                    ProjectOwnerTypeID=entity.PROJ_OWN_TYP_ID,
                }).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return searchResult;
        }

        public Ent_ProjectOwnerType viewProjectOwnerType(int PROJ_OWN_TYP_ID)
        {
            Ent_ProjectOwnerType viewResult = new Ent_ProjectOwnerType();

            try
            {
                var entity = ctx.PROJECT_OWNER_TYPE.FirstOrDefault(d => d.PROJ_OWN_TYP_ID == PROJ_OWN_TYP_ID);
                return new Ent_ProjectOwnerType
                {

                    isActive = entity.IS_ACTIVE,
                    isVendor = entity.IS_VENDOR,
                    name = entity.NAME,
                    description = entity.DESCRIPTION,
                    makerID = entity.MAKER
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return viewResult;
        }
    }
}