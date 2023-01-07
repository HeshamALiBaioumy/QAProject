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
    public class Mdl_SampleType
    {
        public QualityDbEntities ctx = new QualityDbEntities();

        public ResponseMessage insert_updateSampleType(Ent_SampleType sampleType, bool isUpdateForm)
        {
            ResponseMessage response = new ResponseMessage();

            try
            {

                var entity = new SAMPLE_TEST_CATEGORIES();
                if (sampleType.ID > 0)
                    entity = ctx.SAMPLE_TEST_CATEGORIES.FirstOrDefault(d => d.SAMPLE_TEST_CATEGORIES_ID == sampleType.ID);



                entity.IS_ACTIVE = sampleType.isActive;
                entity.MAKER = sampleType.makerID;
                entity.MAKER_DAT_TIM = DateTime.Now;
                entity.NAME = sampleType.name;
                entity.DESCRIPTION = sampleType.description;

                if (sampleType.ID <= 0)
                    ctx.SAMPLE_TEST_CATEGORIES.Add(entity);

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

        public List<Ent_SampleType> searchSampleType(string searchName, string searchDescription, int searchIsActive)
        {
            List<Ent_SampleType> searchResult = new List<Ent_SampleType>();

            try
            {
                searchResult = (from d in ctx.SAMPLE_TEST_CATEGORIES

                                select new Ent_SampleType
                                {
                                    ID = d.SAMPLE_TEST_CATEGORIES_ID,
                                    name = d.NAME,
                                    description = d.DESCRIPTION,
                                    isActive = d.IS_ACTIVE
                                }).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return searchResult;
        }

        public Ent_SampleType viewSampleType(int sampleTypeID)
        {
            Ent_SampleType viewResult = new Ent_SampleType();

            try
            {
                viewResult = (from d in ctx.SAMPLE_TEST_CATEGORIES

                              select new Ent_SampleType
                                {
                                    ID = d.SAMPLE_TEST_CATEGORIES_ID,
                                    name = d.NAME,
                                    description = d.DESCRIPTION,
                                    isActive = d.IS_ACTIVE
                                }).FirstOrDefault(d=>d.ID==sampleTypeID);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return viewResult;
        }
    }
}