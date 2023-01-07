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
    public class Mdl_Mixer
    {
        public QualityDbEntities ctx = new QualityDbEntities();

        public ResponseMessage insert_updateMixer(Ent_Mixer mixer, bool isUpdateForm)
        {
            ResponseMessage response = new ResponseMessage();

            try
            {
                var entity = new MIXER();
                if (mixer.mixerID > 0)
                    entity = ctx.MIXERs.FirstOrDefault(d => d.MIXER_ID == mixer.mixerID);


                entity.TYPE_ID = mixer.mixerTypeID;
                entity.NAME = mixer.name;
                entity.DESCRIPTION = mixer.description;
                entity.FACTORY_ID = mixer.factoryID;
                entity.MIXER_MODEL = mixer.mixerModel;
                entity.MIXER_STATUS = mixer.mixerStatus;
                entity.MIXER_POWER = mixer.mixerPower;
                entity.IS_ACTIVE = mixer.isActive;
                entity.MIXER_CODE = mixer.mixerCode;
                entity.MAKER = mixer.makerID;
                entity.MAKER_DAT_TIME = DateTime.Now;

                if (mixer.mixerID <= 0)
                    ctx.MIXERs.Add(entity);

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

        public List<Ent_Mixer> searchMixer(string searchName, string searchDescription, string searchmixerCode
            , string searchMixerModel, string searchMixerPower, string searchMixerStatus, int searchfactoryID
            , int searchMixerTypeID, int searchIsActive)
        {
            List<Ent_Mixer> searchResult = new List<Ent_Mixer>();

            try
            {
                searchResult =(from entity in  ctx.MIXERs join f in ctx.FACTORies on entity.FACTORY_ID equals f.FACTORY_ID 
                               join ft in ctx.MIXER_TYPES on f.TYPE_ID equals ft.MIXER_TYPE_ID
                select  new Ent_Mixer
                {

                    mixerTypeID = entity.TYPE_ID.Value,
                    name = entity.NAME,
                    description = entity.DESCRIPTION,
                    factoryID = entity.FACTORY_ID.Value,
                    mixerPower = entity.MIXER_POWER,
                    isActive = entity.IS_ACTIVE,
                    mixerCode = entity.MIXER_CODE,
                    mixerModel = entity.MIXER_MODEL,
                    mixerID = entity.MIXER_ID,
                    mixerStatus = entity.MIXER_STATUS,
                    factoryName=f.NAME,
                    mixerTypeName=ft.NAME

                }).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return searchResult;
        }

        public Ent_Mixer viewMixer(int mixerID)
        {
            Ent_Mixer viewResult = new Ent_Mixer();

            try
            {
                var entity = ctx.MIXERs.FirstOrDefault(d => d.MIXER_ID == mixerID);

                return new Ent_Mixer
                {

                    mixerTypeID = entity.TYPE_ID.Value,
                    name = entity.NAME,
                    description = entity.DESCRIPTION,
                    factoryID = entity.FACTORY_ID.Value,
                    mixerPower = entity.MIXER_POWER,
                    isActive = entity.IS_ACTIVE,
                    mixerCode = entity.MIXER_CODE,
                    mixerModel = entity.MIXER_MODEL,
                    mixerID = entity.MIXER_ID,
                    mixerStatus = entity.MIXER_STATUS,
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