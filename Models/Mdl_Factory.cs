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
    public class Mdl_Factory
    {
        public QualityDbEntities ctx = new QualityDbEntities();
        public ResponseMessage insert_updateFactory(Ent_Factory factory, bool isUpdateForm)
        {
            ResponseMessage response = new ResponseMessage();

            try
            {
                var entity = new FACTORY();
                if (factory.factoryID > 0)
                    entity = ctx.FACTORies.FirstOrDefault(d=>d.FACTORY_ID==factory.factoryID);


                entity.FACTORY_CODA = factory.factoryCode;
                entity.TYPE_ID = factory.factoryTypeID;
                entity.NAME = factory.name;
                entity.DESCRIPTION = factory.description;
                entity.ADDRESS = factory.addressLine;
                entity.FACTORY_POWER = factory.factoryPower;
                entity.IS_ACTIVE = factory.isActive;
                entity.MAKER = factory.makerID;
                entity.MAKER_DAT_TIM = DateTime.Now;

                if(factory.factoryID <= 0)
                    ctx.FACTORies.Add(entity);

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

        public List<Ent_Factory> searchFactory(string searchName, string searchDescription, string searchFactoryCode
            , string searchAddressLine, string searchFactoryPower, int searchfactoryTypeID, int searchIsActive)
        {
            List<Ent_Factory> searchResult = new List<Ent_Factory>();

            try
            {
                searchResult = ctx.FACTORies.Select(entity => new Ent_Factory
                {

                    factoryCode = entity.FACTORY_CODA,
                    factoryTypeID = entity.TYPE_ID.Value,
                    name = entity.NAME,
                    description = entity.DESCRIPTION,
                    addressLine = entity.ADDRESS,
                    factoryPower = entity.FACTORY_POWER,
                    isActive = entity.IS_ACTIVE,
                    makerID = entity.MAKER
                }).ToList();
            
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return searchResult;
        }

        public Ent_Factory viewFactory(int factoryID)
        {
            Ent_Factory viewResult = new Ent_Factory();

            try
            {

               var  entity = ctx.FACTORies.FirstOrDefault(d => d.FACTORY_ID == factoryID);
                return new Ent_Factory
                {

                    factoryCode = entity.FACTORY_CODA,
                    factoryTypeID = entity.TYPE_ID.Value,
                    name = entity.NAME,
                    description = entity.DESCRIPTION,
                    addressLine = entity.ADDRESS,
                    factoryPower = entity.FACTORY_POWER,
                    isActive = entity.IS_ACTIVE,
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