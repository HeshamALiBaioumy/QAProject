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
    public class Mdl_SampleTest
    {
        public QualityDbEntities ctx = new QualityDbEntities();
        public ResponseMessage insert_updateSampleTest(Ent_SampleTest sampleTest, bool isUpdateForm)
        {
            ResponseMessage response = new ResponseMessage();

            try
            {
                var entity = new SAMPLE_TEST();
                if (sampleTest.ID > 0)
                    entity = ctx.SAMPLE_TEST.FirstOrDefault(d => d.SAMPLE_TEST_ID == sampleTest.ID);



                entity.IS_ACTIVE = sampleTest.isActive;
                entity.MAKER = sampleTest.makerID;
                entity.MAKER_DAT_TIM = DateTime.Now;
                entity.NAME = sampleTest.name;
                entity.DESCRIPTION = sampleTest.description;
                entity.CATEGORY_ID = sampleTest.sampleTestCategoryID;

                if (sampleTest.ID <= 0)
                    ctx.SAMPLE_TEST.Add(entity);

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

        public List<Ent_SampleTest> searchSampleTest(string searchName, string searchDescription, int searchSampleCategoryID
            , int searchIsActive)
        {
            List<Ent_SampleTest> searchResult = new List<Ent_SampleTest>();

            try
            {
                searchResult = (from d in ctx.SAMPLE_TEST
                                join po in ctx.SAMPLE_TEST_CATEGORIES on d.CATEGORY_ID equals po.SAMPLE_TEST_CATEGORIES_ID
                                select new Ent_SampleTest
                                {
                                    sampleTestCategoryName = po.NAME,
                                    sampleTestCategoryID = po.SAMPLE_TEST_CATEGORIES_ID,
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

        public Ent_SampleTest viewSampleTest(int sampleTestID)
        {
            Ent_SampleTest viewResult = new Ent_SampleTest();

            try
            {
                viewResult = (from d in ctx.SAMPLE_TEST
                                join po in ctx.SAMPLE_TEST_CATEGORIES on d.CATEGORY_ID equals po.SAMPLE_TEST_CATEGORIES_ID
                                select new Ent_SampleTest
                                {
                                    sampleTestCategoryName = po.NAME,
                                    sampleTestCategoryID = po.SAMPLE_TEST_CATEGORIES_ID,
                                    name = d.NAME,
                                    description = d.DESCRIPTION,
                                    isActive = d.IS_ACTIVE
                                }).FirstOrDefault(d=>d.ID== sampleTestID);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return viewResult;
        }
    }
}