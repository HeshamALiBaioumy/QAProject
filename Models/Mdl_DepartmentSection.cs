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
    public class Mdl_DepartmentSection
    {
        public QualityDbEntities ctx = new QualityDbEntities();
        public ResponseMessage insert_updateDepartmentSection(Ent_DepartmentSection departmentSection, bool isUpdateForm)
        {

            ResponseMessage response = new ResponseMessage();

            try
            {

                var entity = new DEPARTMENT_SECTION();
                if (departmentSection.sectionID > 0)
                    entity = ctx.DEPARTMENT_SECTION.FirstOrDefault(d => d.DEPARTMENT_SECTION_ID == departmentSection.sectionID);

                entity.IS_ACTIVE = departmentSection.isActive;
                entity.MAKER = departmentSection.makerID;
                entity.MAKER_DAT_TIM = DateTime.Now;
                entity.DEPARTMENT_ID = departmentSection.departmentID;
                entity.NAME = departmentSection.name;
                entity.DESCRIPTION = departmentSection.description;
                entity.IS_ACTIVE= departmentSection.isActive;


                if (departmentSection.sectionID <= 0)
                    ctx.DEPARTMENT_SECTION.Add(entity);

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

        public List<Ent_DepartmentSection> searchDepartmentSection(string searchName, string searchDescription
            , int searchDepartmentID, int searchIsActive)
        {
            List<Ent_DepartmentSection> searchResult = new List<Ent_DepartmentSection>();

            try
            {
                searchResult = (from d in ctx.DEPARTMENT_SECTION
                                join po in ctx.DEPARTMENTs on d.DEPARTMENT_ID equals po.DEPARTMENT_ID
                                select new Ent_DepartmentSection
                                {
                                    sectionID = d.DEPARTMENT_SECTION_ID,
                                    departmentID = po.DEPARTMENT_ID,
                                    departmentName = po.NAME,
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

        public Ent_DepartmentSection viewDepartmentSection(int departmentSectionID)
        {
            Ent_DepartmentSection viewResult = new Ent_DepartmentSection();

            try
            {
                viewResult = (from d in ctx.DEPARTMENT_SECTION
                                join po in ctx.DEPARTMENTs on d.DEPARTMENT_ID equals po.DEPARTMENT_ID
                                select new Ent_DepartmentSection
                                {
                                    sectionID = d.DEPARTMENT_SECTION_ID,
                                    departmentID = po.DEPARTMENT_ID,
                                    departmentName = po.NAME,
                                    name = d.NAME,
                                    description = d.DESCRIPTION,
                                    isActive = d.IS_ACTIVE
                                }).FirstOrDefault(d=>d.sectionID== departmentSectionID);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return viewResult;
        }
    }
}