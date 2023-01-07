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
    public class Mdl_FactoryType
    {
        public QualityDbEntities ctx = new QualityDbEntities();
        public ResponseMessage insert_UpdateFactoryType(Ent_FactoryTypes factoryType, bool isUpdateForm)
        {
            ResponseMessage response = new ResponseMessage();

            try
            {

                var result = new FACTORY_TYPES();
                if (isUpdateForm)
                    result = ctx.FACTORY_TYPES.FirstOrDefault(d=>d.FACTORY_TYPE_ID==factoryType.factoryTypeID);

                if (result == null)
                    throw new Exception();


                result.NAME = factoryType.name;
                result.DESCRIPTION = factoryType.description;
                //result.IS_ACTIVE = factoryType.isActive;
                //result.MAKER = ;

                if (!isUpdateForm)
                    ctx.FACTORY_TYPES.Add  (result);

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

        public List<Ent_FactoryTypes> searchFactoryType(string searchName, string searchDescription)
        {
            List<Ent_FactoryTypes> searchResult = new List<Ent_FactoryTypes>();

            try
            {
                searchResult = ctx.FACTORY_TYPES.Select(d=> new Ent_FactoryTypes { 
                
                  description=d.DESCRIPTION,
                  factoryTypeID=d.FACTORY_TYPE_ID,
                  //isActive=d.IS_ACTIVE
                  makerID=d.MAKER,
                  name=d.NAME,
                
                }).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return searchResult;
        }

        public Ent_FactoryTypes viewFactoryType(int factoryTypeID)
        {
            Ent_FactoryTypes viewResult = new Ent_FactoryTypes();

            try
            {
                var query = ctx.FACTORY_TYPES.FirstOrDefault(d=>d.FACTORY_TYPE_ID==factoryTypeID);//.Select(d => new Ent_FactoryTypes
                viewResult=new Ent_FactoryTypes {

                    description = query.DESCRIPTION,
                    factoryTypeID = query.FACTORY_TYPE_ID,
                    //isActive=d.IS_ACTIVE
                    makerID = query.MAKER,
                    name = query.NAME,

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