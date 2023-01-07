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
    public class Mdl_ProjectOwner
    {

        public QualityDbEntities ctx = new QualityDbEntities();
        public ResponseMessage insert_updateProjectOwner(Ent_ProjectOwner projectOwner, bool isUpdateForm)
        {
            ResponseMessage response = new ResponseMessage();

            try
            {

                var entity = new PROJECT_OWNER();
                var entityContact = new CONTACT_ID();
                if (projectOwner.projectOwnerID > 0)
                {
                    entity = ctx.PROJECT_OWNER.FirstOrDefault(d => d.PROJEC_OWNER_ID == projectOwner.projectOwnerID);
                    entityContact = ctx.CONTACT_ID.FirstOrDefault(d=>d.CONTACT_ID1==entity.CONTACT_DETAILS_ID);
                }
                    


               
                entityContact.WORK_PHONE_NUBER = projectOwner.contactDetails.workPhoneNumber ?? "";
                entityContact.WORK_EXTENTION = projectOwner.contactDetails.workExtNumber ?? "";
                entityContact.MOBILE_int = projectOwner.contactDetails.mobileNumber ?? "";
                entityContact.EMAIL = projectOwner.contactDetails.emailAddress ?? "";
                entityContact.FAX = projectOwner.contactDetails.fax ?? "";
                entityContact.ADDRESS = projectOwner.contactDetails.addressLine ?? "";
                entityContact.WORK_PLACE = projectOwner.contactDetails.workPlace ?? "";


                entity.PROJ_OWN_TYP_ID = projectOwner.pOTID;
                entity.ACCOUNTABLE = projectOwner.accountable;
                entity.NAME = projectOwner.name;
                entity.DESCRIPTION = projectOwner.description;
                entity.IS_OWNER = projectOwner.isOwner;
                entity.NAME = projectOwner.name;
                entity.DESCRIPTION = projectOwner.description;
                entity.IS_ACTIVE = (projectOwner.isActive);
                entity.MAKER = projectOwner.makerID;
                entity.MAKER_DAT_TIM = DateTime.Now;




                if (projectOwner.projectOwnerID <= 0)
                {
                    ctx.CONTACT_ID.Add(entityContact);
                    ctx.SaveChanges();
                    entity.CONTACT_DETAILS_ID = entityContact.CONTACT_ID1;
                    ctx.PROJECT_OWNER.Add(entity);
                }
                    

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

        public List<Ent_ProjectOwner> searchProjectOwner(string searchName, string searchDescription
            , int searchPOTID, int searchIsActive)
        {
            List<Ent_ProjectOwner> searchResult = new List<Ent_ProjectOwner>();

            try
            {
                searchResult =( from m in  ctx.PROJECT_OWNER join c in ctx.CONTACT_ID on m.CONTACT_DETAILS_ID equals c.CONTACT_ID1 select  new Ent_ProjectOwner
                {
                    name = m.NAME,
                    description = m.DESCRIPTION,
                    isActive = m.IS_ACTIVE,
                    accountable = m.ACCOUNTABLE,
                    contactDetails= new Ent_ContactDetails() { addressLine=c.ADDRESS,emailAddress=c.ADDRESS,fax=c.FAX,mobileNumber=c.MOBILE_int,phoneNumber=c.PHONE_int,workPlace=c.WORK_PLACE },
                    isOwner = m.IS_OWNER,
                    projectOwnerID=m.PROJEC_OWNER_ID
                }).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return searchResult;
        }

        public Ent_ProjectOwner viewProjectOwner(int PROJEC_OWNER_ID)
        {
            Ent_ProjectOwner viewResult = new Ent_ProjectOwner();

            try
            {
                viewResult = (from m in ctx.PROJECT_OWNER
                              join c in ctx.CONTACT_ID on m.CONTACT_DETAILS_ID equals c.CONTACT_ID1
                              select new Ent_ProjectOwner
                              {
                                  name = m.NAME,
                                  description = m.DESCRIPTION,
                                  isActive = m.IS_ACTIVE,
                                  accountable = m.ACCOUNTABLE,
                                  contactDetails = new Ent_ContactDetails() { addressLine = c.ADDRESS, emailAddress = c.ADDRESS, fax = c.FAX, mobileNumber = c.MOBILE_int, phoneNumber = c.PHONE_int, workPlace = c.WORK_PLACE },
                                  isOwner = m.IS_OWNER,
                                  projectOwnerID = m.PROJEC_OWNER_ID
                              }).FirstOrDefault(d => d.projectOwnerID == PROJEC_OWNER_ID);
               
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return viewResult;
        }
    }
}