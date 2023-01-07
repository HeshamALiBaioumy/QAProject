using QA.Entities.Business_Entities;
using QA.Entities.Session_Entities;
using System;
using System.Configuration;
using System.Data;
using System.Data.OracleClient;
using System.Data.SqlClient;
using System.Linq;

namespace QA.Models
{
    public class Mdl_Login
    {
        public ResponseLogin validateLogin(Ent_Login loginCredentials)
        {

            var result=new ResponseLogin();
            QualityDbEntities db = new QualityDbEntities();


            var user = db.USERS_LOGIN.AsNoTracking()
                                .Where(d => d.USER_NAME.ToLower() == loginCredentials.userName.ToLower()
                                    && d.HASHE_PASS.ToLower() == loginCredentials.password.ToLower())
                                .FirstOrDefault();
            if (user != null)
            {
               var userRoles= (from ur in db.USER_ROLES join r in db.LT_INITIALSCREENS on ur.INITIAL_SCREEN_ID equals r.ID where ur.IS_ACTIVE == true  select new Ent_UserRoles {

                   initialScreenID = r.ID,
                   initialScreenName = r.SCREENNAME_ENG,
                   CheckListItemGroup_Edit = ur.CHECKLISTITEMGROUP_EDIT,
                   CheckListItemGroup_Search = ur.CHECKLISTITEMGROUP_SEARCH,
                   CheckListItemGroup_View = ur.CHECKLISTITEMGROUP_VIEW,
                   CheckListItem_Create = ur.CHECKLISTITEM_CREATE,
                   CheckListItemGroup_Create=ur.CHECKLISTITEMGROUP_CREATE,
                   CheckListItem_Edit=ur.CHECKLISTITEM_EDIT,
                   CheckListItem_Search= ur.CHECKLISTITEM_SEARCH,
                   CheckListItem_View = ur.CHECKLISTITEM_VIEW,
                   CheckLists_Create= ur.CHECKLISTS_CREATE,
                   CheckLists_Edit= ur.CHECKLISTS_EDIT,
                   CheckLists_Search= ur.CHECKLISTS_SEARCH,
                   CheckLists_View= ur.CHECKLISTS_VIEW,
                   CLWF_Checker=ur.CLWF_CHECKER,
                   CLWF_Create=ur.CLWF_CREATE,
                   CLWF_Maker=ur.CLWF_MAKER,
                   CLWF_Sequence_Create=ur.CLWFSEQUENCE_CREATE,
                   CLWF_Sequence_Edit=ur.CLWFSEQUENCE_EDIT,
                   CLWF_Sequence_Search=ur.CLWFSEQUENCE_SEARCH,
                   CLWF_Sequence_View = ur.CLWFSEQUENCE_VIEW,
                   Complaint_Create = ur.COMPLAINT_CREATE,
                   Complaint_Edit =ur.COMPLAINT_EDIT,
                   Complaint_Search = ur.COMPLAINT_SEARCH,
                   Complaint_View = ur.COMPLAINT_VIEW,
                   CRTypeGroup_Create = ur.CRTYPEGROUP_CREATE,
                   CRTypeGroup_Edit= ur.CRTYPEGROUP_EDIT,
                   CRTypeGroup_Search=ur.CRTYPEGROUP_SEARCH,
                   CRTypeGroup_View= ur.CRTYPEGROUP_VIEW,
                   CRTYPEMC_Create=ur.CRTYPEMC_CREATE,
                   CRTYPEMC_Edit= ur.CRTYPEMC_EDIT,
                   CRTYPEMC_Search= ur.CRTYPEMC_SEARCH,
                   CRTYPEMC_View= ur.CRTYPEMC_VIEW,
                   CRType_Create=ur.CRTYPE_CREATE,
                   CRType_Edit=ur.CRTYPE_EDIT,
                   CRType_Search= ur.CRTYPE_SEARCH,
                   CRType_View= ur.CRTYPE_VIEW,
                   CR_Action=ur.CR_ACTION,
                   CR_AddAttachments= ur.CR_ADDATTACHMENTS,
                   CR_Create=ur.CR_CREATE,
                   CR_Edit=ur.CR_EDIT,
                   CR_Search=ur.CR_SEARCH,
                   CR_View= ur.CR_VIEW,
                  // Dashboard_AuthLab= ur.CRTYPEMC_CREATE,
                  // Dashboard_CEO=ur.DASHBOARD_CEO,
                   CR_ViewAttachments=ur.CR_VIEWATTACHMENTS,
                   //Dashboard_QualityEng=ur.DASHBOARD_QUALITYENG,
                   //Dashboard_ConsultantEng= ur.DASHBOARD_CONSULTANTENG,
                   //Dashboard_Technician=ur.DASHBOARD_TECHNICIAN,   
                   DepartmentSection_Create=    ur.DEPARTMENTSECTION_CREATE,
                  // Dashboard_SuperEng=  ur.DASHBOARD_SUPERENG,
                   DepartmentSection_Edit = ur.DEPARTMENTSECTION_EDIT,
                   FactoryType_Search=ur.FACTORYTYPE_SEARCH, 
                   DepartmentSection_Search= ur.DEPARTMENTSECTION_SEARCH,
                   Department_Edit= ur.DEPARTMENT_EDIT,
                   Department_Search=   ur.DEPARTMENT_SEARCH,
                   Department_View= ur.DEPARTMENT_VIEW,   
                   Department_Create= ur.DEPARTMENT_CREATE,
                   FactoryType_Create=  ur.FACTORYTYPE_CREATE,
                   FactoryType_Edit= ur.FACTORYTYPE_EDIT,
                   DepartmentSection_View= ur.DEPARTMENTSECTION_VIEW,
                   Factory_Search=  ur.FACTORY_SEARCH,
                   Factory_View= ur.FACTORY_VIEW,  
                   Factory_Edit= ur.FACTORY_EDIT,
                  // makerID= user.makerID,
                   Factory_Create= ur.FACTORY_CREATE,
                   MixerType_Create=ur.MIXERTYPE_CREATE,
                   MixerType_Edit=  ur.MIXERTYPE_EDIT,
                   MixerType_Search= ur.MIXERTYPE_SEARCH,
                   Mixer_Create= ur.MIXER_CREATE,
                   projectOwnerType_Edit=ur.PROJECTOWNERTYPE_EDIT,
                   Mixer_View= ur.MIXER_VIEW,
                   Mixer_Edit= ur.MIXER_EDIT,
                   Mixer_Search= ur.MIXER_SEARCH,
                   projectOwnerType_Create= ur.PROJECTOWNERTYPE_CREATE,
                   projectOwnerType_View= ur.PROJECTOWNERTYPE_VIEW,
                   UserRoles_View= ur.USERROLES_VIEW,
                   UserRoles_Search= ur.USERROLES_SEARCH,
                   ProjectOwner_View= ur.PROJECTOWNER_VIEW,
                   Project_Create= ur.PROJECT_CREATE,
                   Project_Edit= ur.PROJECT_EDIT,
                   FactoryType_View = ur.FACTORYTYPE_VIEW,
                   isActive = ur.IS_ACTIVE,
                   MixerType_View = ur.MIXERTYPE_VIEW,
                   projectOwnerType_Search=ur.PROJECTOWNERTYPE_SEARCH,
                   ProjectOwner_Create= ur.PROJECTOWNER_CREATE,
                   ProjectOwner_Edit= ur.PROJECTOWNER_EDIT,
                   ProjectOwner_Search= ur.PROJECTOWNER_SEARCH,
                   Project_Search= ur.PROJECT_SEARCH,
                   Project_View= ur.PROJECT_VIEW,
                   RCVMM_Create= ur.RCVMM_CREATE,
                   RCVMM_Edit= ur.RCVMM_EDIT,
                   RCV_Conversation_Close= ur.RCV_CONV_CLOSE,
                   RCV_Action= ur.RCV_ACTION,
                   RCV_Conversation_Reply= ur.RCV_CONV_REPLY,
                   RCV_Conversation_View= ur.RCV_CONV_VIEW,
                   RCV_Search= ur.RCV_SEARCH,
                   RCV_Edit= ur.RCV_EDIT,
                   RCVMM_View= ur.RCVMM_VIEW,
                   Reports_CR_Samples_Search= ur.REPORTS_CR_SAMPLES_SEARCH,
                   RCV_RandomSearch= ur.RCV_RANDOMSEARCH,
                   Reports_CR_Projects_Search= ur.REPORTS_CR_PROJECTS_SEARCH,
                   RCV_Conversation_Escalation = ur.RCV_CONV_ESCALATION,
                   Reports_CR_View= ur.REPORTS_CR_VIEW,
                   RCVMM_Search= ur.RCVMM_SEARCH,
                   Reports_CR_Search= ur.REPORTS_CR_SEARCH,
                   Reports_Project_View= ur.REPORTS_PROJECT_VIEW,
                   SampleTestCategory_Create= ur.SAMPLETESTCATEGORY_CREATE,
                   roleName=r.SCREENNAME_ENG,
                   SampleTestCategory_Edit= ur.SAMPLETESTCATEGORY_EDIT,
                   Reports_CR_Samples_SearchDetailed= ur.REPORTS_CR_PROJECTS_SEARCH,
                   SampleTestCategory_Search= ur.SAMPLETESTCATEGORY_SEARCH,
                   SampleTestCategory_View= ur.SAMPLETESTCATEGORY_VIEW,
                   SampleTest_Create= ur.SAMPLETEST_CREATE,
                   SampleTest_Edit= ur.SAMPLETEST_EDIT,
                   SampleTest_Search= ur.SAMPLETEST_SEARCH,
                   SampleTest_View= ur.SAMPLETEST_VIEW,
                   UserProfile_Create= ur.USERPROFILE_CREATE,
                   UserProfile_Edit = ur.USERPROFILE_EDIT,
                   UserProfile_Search = ur.USERPROFILE_SEARCH,
                   UserProfile_View= ur.USERPROFILE_VIEW,
                   userRoleID=ur.ROLE_ID,
                   UserRoles_Create= ur.USERROLES_CREATE,
                   UserRoles_Edit= ur.USERROLES_EDIT

               }).FirstOrDefault();

                result = new ResponseLogin()
                {
                    userID = user.PROFILE_ID,
                    userRoles = userRoles,
                    errorMessage = "",
                    isInitialPassword = true,
                    responseStatus=true
                    
                
                };
            }
           
            return result;
        }

        public ResponseMessage changePassword(Ent_Login_ResetPassword loginCredentials)
        {
            ResponseMessage response = new ResponseMessage();
            response.responseStatus = false;
            try
            {
                using (SqlConnection dbConnection = new SqlConnection(
                    ConfigurationManager.ConnectionStrings["dbConnectionString"].ConnectionString))
                using (SqlCommand dbCommand = dbConnection.CreateCommand())
                {
                    dbCommand.CommandText = "SP_Login_ChangePassword";
                    dbCommand.CommandType = CommandType.StoredProcedure;
                    //dbCommand.Parameters.Add("I_USERID", SqlDbType.NVarChar) = loginCredentials.userID.ToString();
                    //dbCommand.Parameters.Add("I_password", SqlDbType.NVarChar) =PasswordHandler.CreatePasswordHash(loginCredentials.password);

                    dbConnection.Open();
                    dbCommand.ExecuteNonQuery();
                    dbCommand.Dispose();
                    dbConnection.Close();
                }

                response.responseStatus = true;
            }
            
            catch (Exception ex)
            {
                response.responseStatus = false;
                response.errorMessage = ex.Message;

                throw ex;
            }

            return response;
        }
    }
}