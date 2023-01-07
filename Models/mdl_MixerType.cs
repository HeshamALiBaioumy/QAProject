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
    public class mdl_MixerType
    {
        public QualityDbEntities ctx = new QualityDbEntities();
        public ResponseMessage insert_updateMixerType(Ent_MixerType mixerType, bool isUpdateForm)
        {
            ResponseMessage response = new ResponseMessage();

            try
            {
                var entity = new MIXER_TYPES();
                if (mixerType.mixerTypeID > 0)
                    entity = ctx.MIXER_TYPES.FirstOrDefault(d => d.MIXER_TYPE_ID == mixerType.mixerTypeID);

                entity.NAME= mixerType.name;
                entity.DESCRIPTION = mixerType.description;
                //entity.IS_ACTIVE =mixerType.isActive ;
                entity.MAKER = mixerType.makerID;
                entity.MAKER_DAT_TIM = DateTime.Now;
                if (mixerType.mixerTypeID <= 0)
                    ctx.MIXER_TYPES.Add(entity);

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

        public List<Ent_MixerType> searchMixerType(string searchName, string searchDescription, int searchIsActive)
        {
            List<Ent_MixerType> searchResult = new List<Ent_MixerType>();

            try
            {
                searchResult = ctx.MIXER_TYPES.Select(entity => new Ent_MixerType
                {

                    mixerTypeID = entity.MIXER_TYPE_ID,
                    name = entity.NAME,
                    description = entity.DESCRIPTION,
                    //isActive = entity.IS_ACTIVE,
                    makerID = entity.MAKER
                }).ToList();
            
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return searchResult;
        }

        public Ent_MixerType viewMixerType(int mixerTypeID)
        {
            Ent_MixerType viewResult = new Ent_MixerType();

            try
            {
                var entity = ctx.MIXER_TYPES.FirstOrDefault(d => d.MIXER_TYPE_ID == mixerTypeID);
                return new Ent_MixerType
                {

                 
                    name = entity.NAME,
                    description = entity.DESCRIPTION,
                   // isActive = entity.IS_ACTIVE,
                    makerID = entity.MAKER,
                    mixerTypeID = mixerTypeID,
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