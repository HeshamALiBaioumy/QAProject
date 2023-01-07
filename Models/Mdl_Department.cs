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
    public class Mdl_Department
    {

        public QualityDbEntities ctx = new QualityDbEntities();

        public ResponseMessage insert_updateDepartment(Ent_Department  department, bool isUpdateForm)
        {
            ResponseMessage response = new ResponseMessage();

            try
            {
                var entity = new DEPARTMENT();
                if (department.departmentID > 0)
                    entity = ctx.DEPARTMENTs.FirstOrDefault(d => d.DEPARTMENT_ID == department.departmentID);


              
                entity.IS_ACTIVE = department.isActive;
                entity.MAKER = department.makerID;
                entity.MAKER_DAT_TIM = DateTime.Now;
                entity.PROJECT_OWNER_ID = department.projectOwnerID;
                entity.NAME = department.name;
                entity.DESCREPTION = department.description;

                if (department.departmentID <= 0)
                    ctx.DEPARTMENTs.Add(entity);

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

        public List<Ent_Department> searchDepartment(string searchName, string searchDescription
            , int searchprojectOwnerID, int searchIsActive)
        {
            List<Ent_Department> searchResult = new List<Ent_Department>();

            try
            {
                searchResult =(from d in  ctx.DEPARTMENTs join po in ctx.PROJECT_OWNER on d.PROJECT_OWNER_ID equals po.PROJEC_OWNER_ID select  new Ent_Department
                {
                    departmentID = d.DEPARTMENT_ID,
                    projectOwnerName = po.NAME,
                    name = d.NAME,
                    description = d.DESCREPTION,
                    isActive = d.IS_ACTIVE.Value
                }).ToList();

               
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return searchResult;
        }

        public Ent_Department viewDepartment(int DepartmentID)
        {
            Ent_Department viewResult = new Ent_Department();

            try
            {
                viewResult = (from d in ctx.DEPARTMENTs
                                join po in ctx.PROJECT_OWNER on d.PROJECT_OWNER_ID equals po.PROJEC_OWNER_ID
                                select new Ent_Department
                                {
                                    departmentID = d.DEPARTMENT_ID,
                                    projectOwnerName = po.NAME,
                                    name = d.NAME,
                                    description = d.DESCREPTION,
                                    //isActive = entity.IS_ACTIVE
                                }).FirstOrDefault(d=>d.departmentID== DepartmentID);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return viewResult;
        }
    }
}