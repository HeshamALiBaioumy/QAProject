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
    public class Mdl_UserRoles
    {
        public QualityDbEntities ctx=new QualityDbEntities();
        public ResponseMessage insert_updateRole(Ent_UserRoles userRoles, bool isUpdateForm)
        {
            ResponseMessage response = new ResponseMessage();

            try
            {
                var entity = new USER_ROLES();
                if (isUpdateForm)
                {
                    entity = ctx.USER_ROLES.FirstOrDefault(d => d.ROLE_ID == userRoles.userRoleID);
                    if (entity == null) throw new Exception();
                }


                //entity.ROLE_ID = userRoles.userRoleID;
                entity.NAME = userRoles.roleName;
                entity.INITIAL_SCREEN_ID = userRoles.initialScreenID;
                entity.IS_ACTIVE = userRoles.isActive;
                entity.MAKER = userRoles.makerID;

                entity.PROJECTOWNERTYPE_CREATE = (userRoles.projectOwnerType_Create) ;
                entity.PROJECTOWNERTYPE_SEARCH = (userRoles.projectOwnerType_Search) ;
                entity.PROJECTOWNERTYPE_VIEW = (userRoles.projectOwnerType_View) ;
                entity.PROJECTOWNERTYPE_EDIT= (userRoles.projectOwnerType_Edit) ;

                entity.PROJECTOWNER_CREATE= (userRoles.ProjectOwner_Create) ;
                entity.PROJECTOWNER_SEARCH= (userRoles.ProjectOwner_Search) ;
                entity.PROJECTOWNER_VIEW = (userRoles.ProjectOwner_View) ;
                entity.PROJECTOWNER_EDIT = (userRoles.ProjectOwner_Edit) ;

                 entity.FACTORYTYPE_CREATE= (userRoles.FactoryType_Create) ;
                 entity.FACTORYTYPE_SEARCH = (userRoles.FactoryType_Search) ;
                 entity.FACTORYTYPE_VIEW = (userRoles.FactoryType_View) ;
                 entity.FACTORYTYPE_EDIT = (userRoles.FactoryType_Edit) ;

                 entity.FACTORY_CREATE = (userRoles.Factory_Create) ;
                 entity.FACTORY_SEARCH = (userRoles.Factory_Search) ;
                 entity.FACTORY_VIEW = (userRoles.Factory_View) ;
                 entity.FACTORY_EDIT = (userRoles.Factory_Edit) ;
                 entity.DEPARTMENT_CREATE = (userRoles.Department_Create) ;
                 entity.DEPARTMENT_SEARCH = (userRoles.Department_Search) ;
                 entity.DEPARTMENT_VIEW = (userRoles.Department_View) ;
                 entity.DEPARTMENT_EDIT = (userRoles.Department_Edit) ;

                 entity.DEPARTMENTSECTION_CREATE = (userRoles.DepartmentSection_Create) ;
                 entity.DEPARTMENTSECTION_SEARCH = (userRoles.DepartmentSection_Search) ;
                 entity.DEPARTMENTSECTION_VIEW = (userRoles.DepartmentSection_View) ;
                 entity.DEPARTMENTSECTION_EDIT = (userRoles.DepartmentSection_Edit) ;

                 entity.MIXERTYPE_CREATE = (userRoles.MixerType_Create) ;
                 entity.MIXERTYPE_SEARCH = (userRoles.MixerType_Search) ;
                 entity.MIXERTYPE_VIEW = (userRoles.MixerType_View) ;
                 entity.MIXERTYPE_EDIT = (userRoles.MixerType_Edit) ;

                 entity.MIXER_CREATE = (userRoles.Mixer_Create) ;
                 entity.MIXER_SEARCH = (userRoles.Mixer_Search) ;
                 entity.MIXER_VIEW = (userRoles.Mixer_View) ;
                 entity.MIXER_EDIT = (userRoles.Mixer_Edit) ;
                
                 entity.SAMPLETESTCATEGORY_CREATE = (userRoles.SampleTestCategory_Create) ;
                 entity.SAMPLETESTCATEGORY_SEARCH = (userRoles.SampleTestCategory_Search) ;
                 entity.SAMPLETESTCATEGORY_VIEW = (userRoles.SampleTestCategory_View) ;
                 entity.SAMPLETESTCATEGORY_EDIT = (userRoles.SampleTestCategory_Edit) ;

                 entity.SAMPLETEST_CREATE = (userRoles.SampleTest_Create) ;
                 entity.SAMPLETEST_SEARCH = (userRoles.SampleTest_Search) ;
                 entity.SAMPLETEST_VIEW = (userRoles.SampleTest_View) ;
                entity.SAMPLETEST_EDIT = (userRoles.SampleTest_Edit);

                 entity.CRTYPEMC_CREATE = (userRoles.CRTYPEMC_Create) ;
                 entity.CRTYPEMC_SEARCH = (userRoles.CRTYPEMC_Search) ;
                 entity.CRTYPEMC_VIEW = (userRoles.CRTYPEMC_View) ;
                 entity.CRTYPEMC_EDIT = (userRoles.CRTYPEMC_Edit) ;

                 entity.CRTYPEGROUP_CREATE = (userRoles.CRTypeGroup_Create) ;
                 entity.CRTYPEGROUP_SEARCH = (userRoles.CRTypeGroup_Search) ;
                 entity.CRTYPEGROUP_VIEW = (userRoles.CRTypeGroup_View) ;
                 entity.CRTYPEGROUP_EDIT = (userRoles.CRTypeGroup_Edit) ;

                 entity.CRTYPE_CREATE = (userRoles.CRType_Create) ;
                 entity.CRTYPE_SEARCH = (userRoles.CRType_Search) ;
                 entity.CRTYPE_VIEW = (userRoles.CRType_View) ;
                 entity.CRTYPE_EDIT = (userRoles.CRType_Edit) ;

                 entity.CHECKLISTITEM_CREATE = (userRoles.CheckListItem_Create) ;
                 entity.CHECKLISTITEM_SEARCH = (userRoles.CheckListItem_Search) ;
                 entity.CHECKLISTITEM_VIEW = (userRoles.CheckListItem_View) ;
                 entity.CHECKLISTITEM_EDIT = (userRoles.CheckListItem_Edit) ;

                 entity.CHECKLISTITEMGROUP_CREATE = (userRoles.CheckListItemGroup_Create) ;
                 entity.CHECKLISTITEMGROUP_SEARCH = (userRoles.CheckListItemGroup_Search) ;
                 entity.CHECKLISTITEMGROUP_VIEW = (userRoles.CheckListItemGroup_View) ;
                 entity.CHECKLISTITEMGROUP_EDIT = (userRoles.CheckListItemGroup_Edit) ;

                 entity.CHECKLISTS_CREATE = (userRoles.CheckLists_Create) ;
                 entity.CHECKLISTS_SEARCH = (userRoles.CheckLists_Search) ;
                 entity.CHECKLISTS_VIEW = (userRoles.CheckLists_View) ;
                 entity.CHECKLISTS_EDIT = (userRoles.CheckLists_Edit) ;

                 entity.USERPROFILE_CREATE = (userRoles.UserProfile_Create) ;
                 entity.USERPROFILE_SEARCH = (userRoles.UserProfile_Search) ;
                 entity.USERPROFILE_VIEW = (userRoles.UserProfile_View) ;
                 entity.USERPROFILE_EDIT = (userRoles.UserProfile_Edit) ; 

                 entity.USERROLES_CREATE = (userRoles.UserRoles_Create) ;
                 entity.USERROLES_SEARCH = (userRoles.UserRoles_Search) ;
                 entity.USERROLES_VIEW = (userRoles.UserRoles_View) ;
                 entity.USERROLES_EDIT = (userRoles.UserRoles_Edit) ;

                 entity.PROJECT_CREATE = (userRoles.Project_Create) ;
                 entity.PROJECT_SEARCH = (userRoles.Project_Search) ;
                 entity.PROJECT_VIEW = (userRoles.Project_View) ;
                 entity.PROJECT_EDIT = (userRoles.Project_Edit) ;

                 entity.COMPLAINT_CREATE = (userRoles.Complaint_Create) ;
                 entity.COMPLAINT_SEARCH = (userRoles.Complaint_Search) ;
                 entity.COMPLAINT_VIEW = (userRoles.Complaint_View) ;
                 entity.COMPLAINT_EDIT = (userRoles.Complaint_Edit) ;

                 entity.CLWFSEQUENCE_CREATE = (userRoles.CLWF_Sequence_Create) ;
                 entity.CLWFSEQUENCE_SEARCH = (userRoles.CLWF_Sequence_Search) ;
                 entity.CLWFSEQUENCE_VIEW = (userRoles.CLWF_Sequence_View) ;
                 entity.CLWFSEQUENCE_EDIT = (userRoles.CLWF_Sequence_Edit) ;

                 entity.CLWF_CREATE = (userRoles.CLWF_Create) ;
                 entity.CLWF_MAKER = (userRoles.CLWF_Maker) ;
                 entity.CLWF_CHECKER = (userRoles.CLWF_Checker) ;


                 entity.CR_CREATE = (userRoles.CR_Create) ;
                 entity.CR_EDIT = (userRoles.CR_Edit) ;
                 entity.CR_ACTION = (userRoles.CR_Action) ;
                 entity.CR_ADDATTACHMENTS = (userRoles.CR_AddAttachments) ;
                 entity.CR_VIEWATTACHMENTS = (userRoles.CR_ViewAttachments) ;
                 entity.CR_VIEW = (userRoles.CR_View) ;
                 entity.CR_SEARCH = (userRoles.CR_Search) ;

                 entity.DASHBOARD_SUPERENG = (userRoles.Dashboard_SuperEng) ;
                 entity.DASHBOARD_CONSULTANTENG = (userRoles.Dashboard_ConsultantEng) ;
                 entity.DASHBOARD_AUTHLAB = (userRoles.Dashboard_AuthLab) ;
                 entity.DASHBOARD_TECHNICIAN = (userRoles.Dashboard_Technician) ;
                 entity.DASHBOARD_CEO = (userRoles.Dashboard_CEO) ;
                 entity.DASHBOARD_QUALITYENG = (userRoles.Dashboard_QualityEng) ;

                 entity.REPORTS_CR_VIEW = (userRoles.Reports_CR_View) ;
                 entity.REPORTS_CR_SEARCH = (userRoles.Reports_CR_Search) ;
                 entity.REPORTS_CR_PROJECTS_SEARCH = (userRoles.Reports_CR_Projects_Search) ;
                 entity.REPORTS_CR_SAMPLES_SEARCH = (userRoles.Reports_CR_Samples_Search) ;
                 entity.REPORTS_CR_SMPL_SRCHDET = (userRoles.Reports_CR_Samples_SearchDetailed) ;
                 entity.REPORTS_PROJECT_VIEW = (userRoles.Reports_Project_View) ;

                 entity.RCV_RANDOMSEARCH = (userRoles.RCV_RandomSearch) ;
                 entity.RCV_ACTION = (userRoles.RCV_Action) ;
                 entity.RCV_SEARCH = (userRoles.RCV_Search) ;
                 entity.RCV_EDIT = (userRoles.RCV_Edit) ;

                 entity.RCVMM_CREATE = (userRoles.RCVMM_Create) ;
                 entity.RCVMM_SEARCH = (userRoles.RCVMM_Search) ;
                 entity.RCVMM_EDIT = (userRoles.RCVMM_Edit) ;
                 entity.RCVMM_VIEW = (userRoles.RCVMM_View) ;

                 entity.RCV_CONV_VIEW = (userRoles.RCV_Conversation_View) ;
                 entity.RCV_CONV_REPLY = (userRoles.RCV_Conversation_Reply) ;
                 entity.RCV_CONV_CLOSE = (userRoles.RCV_Conversation_Close) ;
                 entity.RCV_CONV_ESCALATION = (userRoles.RCV_Conversation_Escalation) ;
                entity.MAKER_DATETIME = DateTime.Now;


              if (!isUpdateForm)
                    ctx.USER_ROLES.Add(entity);



                ctx.SaveChanges();


                //using (SqlConnection dbConnection = new SqlConnection(
                //    ConfigurationManager.ConnectionStrings["dbConnectionString"].ConnectionString))
                //using (SqlCommand dbCommand = dbConnection.CreateCommand())
                //{
                //    dbCommand.CommandText = "SP_UserRoles";
                //    dbCommand.CommandType = CommandType.StoredProcedure;

                //    dbCommand.Parameters.Add("INPUT_ID", SqlDbType.Int).Value = userRoles.userRoleID;
                //    dbCommand.Parameters.Add("I_NAME", SqlDbType.VarChar).Value = userRoles.roleName;
                //    dbCommand.Parameters.Add("I_InitialScreenID", SqlDbType.Int).Value = userRoles.initialScreenID;
                //    dbCommand.Parameters.Add("I_IS_ACTIVE", SqlDbType.Char).Value = (userRoles.isActive) ? 'Y' : 'N';
                //    dbCommand.Parameters.Add("I_MAKER", SqlDbType.NVarChar).Value = userRoles.makerID;
                //    dbCommand.Parameters.Add("ACTION", SqlDbType.NVarChar).Value = (isUpdateForm) ? "UPDATE" : "NEW";

                //    dbCommand.Parameters.Add("I_projectOwnerType_Create", SqlDbType.Char).Value = (userRoles.projectOwnerType_Create) ? 'Y' : 'N';
                //    dbCommand.Parameters.Add("I_projectOwnerType_Search", SqlDbType.Char).Value = (userRoles.projectOwnerType_Search) ? 'Y' : 'N';
                //    dbCommand.Parameters.Add("I_projectOwnerType_View", SqlDbType.Char).Value = (userRoles.projectOwnerType_View) ? 'Y' : 'N';
                //    dbCommand.Parameters.Add("I_projectOwnerType_Edit", SqlDbType.Char).Value = (userRoles.projectOwnerType_Edit) ? 'Y' : 'N';

                //    dbCommand.Parameters.Add("I_ProjectOwner_Create", SqlDbType.Char).Value = (userRoles.ProjectOwner_Create) ? 'Y' : 'N';
                //    dbCommand.Parameters.Add("I_ProjectOwner_Search", SqlDbType.Char).Value = (userRoles.ProjectOwner_Search) ? 'Y' : 'N';
                //    dbCommand.Parameters.Add("I_ProjectOwner_View", SqlDbType.Char).Value = (userRoles.ProjectOwner_View) ? 'Y' : 'N';
                //    dbCommand.Parameters.Add("I_ProjectOwner_Edit", SqlDbType.Char).Value = (userRoles.ProjectOwner_Edit) ? 'Y' : 'N';

                //    dbCommand.Parameters.Add("I_FactoryType_Create", SqlDbType.Char).Value = (userRoles.FactoryType_Create) ? 'Y' : 'N';
                //    dbCommand.Parameters.Add("I_FactoryType_Search", SqlDbType.Char).Value = (userRoles.FactoryType_Search) ? 'Y' : 'N';
                //    dbCommand.Parameters.Add("I_FactoryType_View", SqlDbType.Char).Value = (userRoles.FactoryType_View) ? 'Y' : 'N';
                //    dbCommand.Parameters.Add("I_FactoryType_Edit", SqlDbType.Char).Value = (userRoles.FactoryType_Edit) ? 'Y' : 'N';

                //    dbCommand.Parameters.Add("I_Factory_Create", SqlDbType.Char).Value = (userRoles.Factory_Create) ? 'Y' : 'N';
                //    dbCommand.Parameters.Add("I_Factory_Search", SqlDbType.Char).Value = (userRoles.Factory_Search) ? 'Y' : 'N';
                //    dbCommand.Parameters.Add("I_Factory_View", SqlDbType.Char).Value = (userRoles.Factory_View) ? 'Y' : 'N';
                //    dbCommand.Parameters.Add("I_Factory_Edit", SqlDbType.Char).Value = (userRoles.Factory_Edit) ? 'Y' : 'N';

                //    dbCommand.Parameters.Add("I_Department_Create", SqlDbType.Char).Value = (userRoles.Department_Create) ? 'Y' : 'N';
                //    dbCommand.Parameters.Add("I_Department_Search", SqlDbType.Char).Value = (userRoles.Department_Search) ? 'Y' : 'N';
                //    dbCommand.Parameters.Add("I_Department_View", SqlDbType.Char).Value = (userRoles.Department_View) ? 'Y' : 'N';
                //    dbCommand.Parameters.Add("I_Department_Edit", SqlDbType.Char).Value = (userRoles.Department_Edit) ? 'Y' : 'N';

                //    dbCommand.Parameters.Add("I_DepartmentSection_Create", SqlDbType.Char).Value = (userRoles.DepartmentSection_Create) ? 'Y' : 'N';
                //    dbCommand.Parameters.Add("I_DepartmentSection_Search", SqlDbType.Char).Value = (userRoles.DepartmentSection_Search) ? 'Y' : 'N';
                //    dbCommand.Parameters.Add("I_DepartmentSection_View", SqlDbType.Char).Value = (userRoles.DepartmentSection_View) ? 'Y' : 'N';
                //    dbCommand.Parameters.Add("I_DepartmentSection_Edit", SqlDbType.Char).Value = (userRoles.DepartmentSection_Edit) ? 'Y' : 'N';

                //    dbCommand.Parameters.Add("I_MixerType_Create", SqlDbType.Char).Value = (userRoles.MixerType_Create) ? 'Y' : 'N';
                //    dbCommand.Parameters.Add("I_MixerType_Search", SqlDbType.Char).Value = (userRoles.MixerType_Search) ? 'Y' : 'N';
                //    dbCommand.Parameters.Add("I_MixerType_View", SqlDbType.Char).Value = (userRoles.MixerType_View) ? 'Y' : 'N';
                //    dbCommand.Parameters.Add("I_MixerType_Edit", SqlDbType.Char).Value = (userRoles.MixerType_Edit) ? 'Y' : 'N';

                //    dbCommand.Parameters.Add("I_Mixer_Create", SqlDbType.Char).Value = (userRoles.Mixer_Create) ? 'Y' : 'N';
                //    dbCommand.Parameters.Add("I_Mixer_Search", SqlDbType.Char).Value = (userRoles.Mixer_Search) ? 'Y' : 'N';
                //    dbCommand.Parameters.Add("I_Mixer_View", SqlDbType.Char).Value = (userRoles.Mixer_View) ? 'Y' : 'N';
                //    dbCommand.Parameters.Add("I_Mixer_Edit", SqlDbType.Char).Value = (userRoles.Mixer_Edit) ? 'Y' : 'N';

                //    dbCommand.Parameters.Add("I_SampleTestCategory_Create", SqlDbType.Char).Value = (userRoles.SampleTestCategory_Create) ? 'Y' : 'N';
                //    dbCommand.Parameters.Add("I_SampleTestCategory_Search", SqlDbType.Char).Value = (userRoles.SampleTestCategory_Search) ? 'Y' : 'N';
                //    dbCommand.Parameters.Add("I_SampleTestCategory_View", SqlDbType.Char).Value = (userRoles.SampleTestCategory_View) ? 'Y' : 'N';
                //    dbCommand.Parameters.Add("I_SampleTestCategory_Edit", SqlDbType.Char).Value = (userRoles.SampleTestCategory_Edit) ? 'Y' : 'N';

                //    dbCommand.Parameters.Add("I_SampleTest_Create", SqlDbType.Char).Value = (userRoles.SampleTest_Create) ? 'Y' : 'N';
                //    dbCommand.Parameters.Add("I_SampleTest_Search", SqlDbType.Char).Value = (userRoles.SampleTest_Search) ? 'Y' : 'N';
                //    dbCommand.Parameters.Add("I_SampleTest_View", SqlDbType.Char).Value = (userRoles.SampleTest_View) ? 'Y' : 'N';
                //    dbCommand.Parameters.Add("I_SampleTest_Edit", SqlDbType.Char).Value = (userRoles.SampleTest_Edit) ? 'Y' : 'N';

                //    dbCommand.Parameters.Add("I_CRTYPEMC_Create", SqlDbType.Char).Value = (userRoles.CRTYPEMC_Create) ? 'Y' : 'N';
                //    dbCommand.Parameters.Add("I_CRTYPEMC_Search", SqlDbType.Char).Value = (userRoles.CRTYPEMC_Search) ? 'Y' : 'N';
                //    dbCommand.Parameters.Add("I_CRTYPEMC_View", SqlDbType.Char).Value = (userRoles.CRTYPEMC_View) ? 'Y' : 'N';
                //    dbCommand.Parameters.Add("I_CRTYPEMC_Edit", SqlDbType.Char).Value = (userRoles.CRTYPEMC_Edit) ? 'Y' : 'N';

                //    dbCommand.Parameters.Add("I_CRTypeGroup_Create", SqlDbType.Char).Value = (userRoles.CRTypeGroup_Create) ? 'Y' : 'N';
                //    dbCommand.Parameters.Add("I_CRTypeGroup_Search", SqlDbType.Char).Value = (userRoles.CRTypeGroup_Search) ? 'Y' : 'N';
                //    dbCommand.Parameters.Add("I_CRTypeGroup_View", SqlDbType.Char).Value = (userRoles.CRTypeGroup_View) ? 'Y' : 'N';
                //    dbCommand.Parameters.Add("I_CRTypeGroup_Edit", SqlDbType.Char).Value = (userRoles.CRTypeGroup_Edit) ? 'Y' : 'N';

                //    dbCommand.Parameters.Add("I_CRType_Create", SqlDbType.Char).Value = (userRoles.CRType_Create) ? 'Y' : 'N';
                //    dbCommand.Parameters.Add("I_CRType_Search", SqlDbType.Char).Value = (userRoles.CRType_Search) ? 'Y' : 'N';
                //    dbCommand.Parameters.Add("I_CRType_View", SqlDbType.Char).Value = (userRoles.CRType_View) ? 'Y' : 'N';
                //    dbCommand.Parameters.Add("I_CRType_Edit", SqlDbType.Char).Value = (userRoles.CRType_Edit) ? 'Y' : 'N';

                //    dbCommand.Parameters.Add("I_CheckListItem_Create", SqlDbType.Char).Value = (userRoles.CheckListItem_Create) ? 'Y' : 'N';
                //    dbCommand.Parameters.Add("I_CheckListItem_Search", SqlDbType.Char).Value = (userRoles.CheckListItem_Search) ? 'Y' : 'N';
                //    dbCommand.Parameters.Add("I_CheckListItem_View", SqlDbType.Char).Value = (userRoles.CheckListItem_View) ? 'Y' : 'N';
                //    dbCommand.Parameters.Add("I_CheckListItem_Edit", SqlDbType.Char).Value = (userRoles.CheckListItem_Edit) ? 'Y' : 'N';

                //    dbCommand.Parameters.Add("I_CheckListItemGroup_Create", SqlDbType.Char).Value = (userRoles.CheckListItemGroup_Create) ? 'Y' : 'N';
                //    dbCommand.Parameters.Add("I_CheckListItemGroup_Search", SqlDbType.Char).Value = (userRoles.CheckListItemGroup_Search) ? 'Y' : 'N';
                //    dbCommand.Parameters.Add("I_CheckListItemGroup_View", SqlDbType.Char).Value = (userRoles.CheckListItemGroup_View) ? 'Y' : 'N';
                //    dbCommand.Parameters.Add("I_CheckListItemGroup_Edit", SqlDbType.Char).Value = (userRoles.CheckListItemGroup_Edit) ? 'Y' : 'N';

                //    dbCommand.Parameters.Add("I_CheckLists_Create", SqlDbType.Char).Value = (userRoles.CheckLists_Create) ? 'Y' : 'N';
                //    dbCommand.Parameters.Add("I_CheckLists_Search", SqlDbType.Char).Value = (userRoles.CheckLists_Search) ? 'Y' : 'N';
                //    dbCommand.Parameters.Add("I_CheckLists_View", SqlDbType.Char).Value = (userRoles.CheckLists_View) ? 'Y' : 'N';
                //    dbCommand.Parameters.Add("I_CheckLists_Edit", SqlDbType.Char).Value = (userRoles.CheckLists_Edit) ? 'Y' : 'N';

                //    dbCommand.Parameters.Add("I_UserProfile_Create", SqlDbType.Char).Value = (userRoles.UserProfile_Create) ? 'Y' : 'N';
                //    dbCommand.Parameters.Add("I_UserProfile_Search", SqlDbType.Char).Value = (userRoles.UserProfile_Search) ? 'Y' : 'N';
                //    dbCommand.Parameters.Add("I_UserProfile_View", SqlDbType.Char).Value = (userRoles.UserProfile_View) ? 'Y' : 'N';
                //    dbCommand.Parameters.Add("I_UserProfile_Edit", SqlDbType.Char).Value = (userRoles.UserProfile_Edit) ? 'Y' : 'N';

                //    dbCommand.Parameters.Add("I_UserRoles_Create", SqlDbType.Char).Value = (userRoles.UserRoles_Create) ? 'Y' : 'N';
                //    dbCommand.Parameters.Add("I_UserRoles_Search", SqlDbType.Char).Value = (userRoles.UserRoles_Search) ? 'Y' : 'N';
                //    dbCommand.Parameters.Add("I_UserRoles_View", SqlDbType.Char).Value = (userRoles.UserRoles_View) ? 'Y' : 'N';
                //    dbCommand.Parameters.Add("I_UserRoles_Edit", SqlDbType.Char).Value = (userRoles.UserRoles_Edit) ? 'Y' : 'N';

                //    dbCommand.Parameters.Add("I_Project_Create", SqlDbType.Char).Value = (userRoles.Project_Create) ? 'Y' : 'N';
                //    dbCommand.Parameters.Add("I_Project_Search", SqlDbType.Char).Value = (userRoles.Project_Search) ? 'Y' : 'N';
                //    dbCommand.Parameters.Add("I_Project_View", SqlDbType.Char).Value = (userRoles.Project_View) ? 'Y' : 'N';
                //    dbCommand.Parameters.Add("I_Project_Edit", SqlDbType.Char).Value = (userRoles.Project_Edit) ? 'Y' : 'N';

                //    dbCommand.Parameters.Add("I_Complaint_Create", SqlDbType.Char).Value = (userRoles.Complaint_Create) ? 'Y' : 'N';
                //    dbCommand.Parameters.Add("I_Complaint_Search", SqlDbType.Char).Value = (userRoles.Complaint_Search) ? 'Y' : 'N';
                //    dbCommand.Parameters.Add("I_Complaint_View", SqlDbType.Char).Value = (userRoles.Complaint_View) ? 'Y' : 'N';
                //    dbCommand.Parameters.Add("I_Complaint_Edit", SqlDbType.Char).Value = (userRoles.Complaint_Edit) ? 'Y' : 'N';

                //    dbCommand.Parameters.Add("I_CLWFSequence_Create", SqlDbType.Char).Value = (userRoles.CLWF_Sequence_Create) ? 'Y' : 'N';
                //    dbCommand.Parameters.Add("I_CLWFSequence_Search", SqlDbType.Char).Value = (userRoles.CLWF_Sequence_Search) ? 'Y' : 'N';
                //    dbCommand.Parameters.Add("I_CLWFSequence_View", SqlDbType.Char).Value = (userRoles.CLWF_Sequence_View) ? 'Y' : 'N';
                //    dbCommand.Parameters.Add("I_CLWFSequence_Edit", SqlDbType.Char).Value = (userRoles.CLWF_Sequence_Edit) ? 'Y' : 'N';

                //    dbCommand.Parameters.Add("I_CLWF_Create", SqlDbType.Char).Value = (userRoles.CLWF_Create) ? 'Y' : 'N';
                //    dbCommand.Parameters.Add("I_CLWF_Maker", SqlDbType.Char).Value = (userRoles.CLWF_Maker) ? 'Y' : 'N';
                //    dbCommand.Parameters.Add("I_CLWF_Checker", SqlDbType.Char).Value = (userRoles.CLWF_Checker) ? 'Y' : 'N';

                //    dbCommand.Parameters.Add("I_CR_Create", SqlDbType.Char).Value = (userRoles.CR_Create) ? 'Y' : 'N';
                //    dbCommand.Parameters.Add("I_CR_Edit", SqlDbType.Char).Value = (userRoles.CR_Edit) ? 'Y' : 'N';
                //    dbCommand.Parameters.Add("I_CR_Action", SqlDbType.Char).Value = (userRoles.CR_Action) ? 'Y' : 'N';
                //    dbCommand.Parameters.Add("I_CR_AddAttachments", SqlDbType.Char).Value = (userRoles.CR_AddAttachments) ? 'Y' : 'N';
                //    dbCommand.Parameters.Add("I_CR_ViewAttachments", SqlDbType.Char).Value = (userRoles.CR_ViewAttachments) ? 'Y' : 'N';
                //    dbCommand.Parameters.Add("I_CR_View", SqlDbType.Char).Value = (userRoles.CR_View) ? 'Y' : 'N';
                //    dbCommand.Parameters.Add("I_CR_Search", SqlDbType.Char).Value = (userRoles.CR_Search) ? 'Y' : 'N';

                //    dbCommand.Parameters.Add("I_DASHBOARD_SuperEng", SqlDbType.Char).Value = (userRoles.Dashboard_SuperEng) ? 'Y' : 'N';
                //    dbCommand.Parameters.Add("I_DASHBOARD_ConsultantEng", SqlDbType.Char).Value = (userRoles.Dashboard_ConsultantEng) ? 'Y' : 'N';
                //    dbCommand.Parameters.Add("I_DASHBOARD_AuthLab", SqlDbType.Char).Value = (userRoles.Dashboard_AuthLab) ? 'Y' : 'N';
                //    dbCommand.Parameters.Add("I_DASHBOARD_Technician", SqlDbType.Char).Value = (userRoles.Dashboard_Technician) ? 'Y' : 'N';
                //    dbCommand.Parameters.Add("I_DASHBOARD_CEO", SqlDbType.Char).Value = (userRoles.Dashboard_CEO) ? 'Y' : 'N';
                //    dbCommand.Parameters.Add("I_DASHBOARD_QUALITYENG", SqlDbType.Char).Value = (userRoles.Dashboard_QualityEng) ? 'Y' : 'N';

                //    dbCommand.Parameters.Add("I_REPORTS_CR_VIEW", SqlDbType.Char).Value = (userRoles.Reports_CR_View) ? 'Y' : 'N';
                //    dbCommand.Parameters.Add("I_REPORTS_CR_SEARCH", SqlDbType.Char).Value = (userRoles.Reports_CR_Search) ? 'Y' : 'N';
                //    dbCommand.Parameters.Add("I_REPORTS_CR_PROJECTS_SEARCH", SqlDbType.Char).Value = (userRoles.Reports_CR_Projects_Search) ? 'Y' : 'N';
                //    dbCommand.Parameters.Add("I_REPORTS_CR_SAMPLES_SEARCH", SqlDbType.Char).Value = (userRoles.Reports_CR_Samples_Search) ? 'Y' : 'N';
                //    dbCommand.Parameters.Add("I_REPORTS_CR_SMPL_SRCHDET", SqlDbType.Char).Value = (userRoles.Reports_CR_Samples_SearchDetailed) ? 'Y' : 'N';
                //    dbCommand.Parameters.Add("I_REPORTS_PROJECT_VIEW", SqlDbType.Char).Value = (userRoles.Reports_Project_View) ? 'Y' : 'N';

                //    dbCommand.Parameters.Add("I_RCV_RANDOMSEARCH", SqlDbType.Char).Value = (userRoles.RCV_RandomSearch) ? 'Y' : 'N';
                //    dbCommand.Parameters.Add("I_RCV_ACTION", SqlDbType.Char).Value = (userRoles.RCV_Action) ? 'Y' : 'N';
                //    dbCommand.Parameters.Add("I_RCV_SEARCH", SqlDbType.Char).Value = (userRoles.RCV_Search) ? 'Y' : 'N';
                //    dbCommand.Parameters.Add("I_RCV_EDIT", SqlDbType.Char).Value = (userRoles.RCV_Edit) ? 'Y' : 'N';

                //    dbCommand.Parameters.Add("I_RCVMM_CREATE", SqlDbType.Char).Value = (userRoles.RCVMM_Create) ? 'Y' : 'N';
                //    dbCommand.Parameters.Add("I_RCVMM_SEARCH", SqlDbType.Char).Value = (userRoles.RCVMM_Search) ? 'Y' : 'N';
                //    dbCommand.Parameters.Add("I_RCVMM_EDIT", SqlDbType.Char).Value = (userRoles.RCVMM_Edit) ? 'Y' : 'N';
                //    dbCommand.Parameters.Add("I_RCVMM_VIEW", SqlDbType.Char).Value = (userRoles.RCVMM_View) ? 'Y' : 'N';

                //    dbCommand.Parameters.Add("I_RCV_CONV_VIEW", SqlDbType.Char).Value = (userRoles.RCV_Conversation_View) ? 'Y' : 'N';
                //    dbCommand.Parameters.Add("I_RCV_CONV_REPLY", SqlDbType.Char).Value = (userRoles.RCV_Conversation_Reply) ? 'Y' : 'N';
                //    dbCommand.Parameters.Add("I_RCV_CONV_CLOSE", SqlDbType.Char).Value = (userRoles.RCV_Conversation_Close) ? 'Y' : 'N';
                //    dbCommand.Parameters.Add("I_RCV_CONV_ESCALATION", SqlDbType.Char).Value = (userRoles.RCV_Conversation_Escalation) ? 'Y' : 'N';

                //    dbConnection.Open();
                //    dbCommand.ExecuteNonQuery();
                //    dbCommand.Dispose();
                //    dbConnection.Close();
                //}

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

        public List<Ent_UserRoles> searchRoles(string searchName, int searchIsActive)
        {
            List<Ent_UserRoles> searchResult = new List<Ent_UserRoles>();
            try
            {
                 searchResult = ctx.USER_ROLES.Select(userRoles => new Ent_UserRoles
                {

                    roleName = userRoles.NAME,
                    initialScreenID = userRoles.INITIAL_SCREEN_ID,
                    isActive = userRoles.IS_ACTIVE,
                    makerID = userRoles.MAKER,

                    projectOwnerType_Create = (userRoles.PROJECTOWNERTYPE_CREATE),
                    projectOwnerType_Search = (userRoles.PROJECTOWNERTYPE_SEARCH),
                    projectOwnerType_View = (userRoles.PROJECTOWNERTYPE_VIEW),
                    projectOwnerType_Edit = (userRoles.PROJECTOWNERTYPE_EDIT),

                    ProjectOwner_Create = (userRoles.PROJECTOWNER_CREATE),
                    ProjectOwner_Search = (userRoles.PROJECTOWNER_SEARCH),
                    ProjectOwner_View = (userRoles.PROJECTOWNER_VIEW),
                    ProjectOwner_Edit = (userRoles.PROJECTOWNER_EDIT),

                    FactoryType_Create = (userRoles.FACTORYTYPE_CREATE),
                    FactoryType_Search = (userRoles.FACTORYTYPE_SEARCH),
                    FactoryType_View = (userRoles.FACTORYTYPE_VIEW),
                    FactoryType_Edit = (userRoles.FACTORYTYPE_EDIT),

                    Factory_Create = (userRoles.FACTORY_CREATE),
                    Factory_Search = (userRoles.FACTORY_SEARCH),
                    Factory_View = (userRoles.FACTORY_VIEW),
                    Factory_Edit = (userRoles.FACTORY_EDIT),
                    Department_Create = (userRoles.DEPARTMENT_CREATE),
                    Department_Search = (userRoles.DEPARTMENT_SEARCH),
                    Department_View = (userRoles.DEPARTMENT_VIEW),
                    Department_Edit = (userRoles.DEPARTMENT_EDIT),

                    DepartmentSection_Create = (userRoles.DEPARTMENTSECTION_CREATE),
                    DepartmentSection_Search = (userRoles.DEPARTMENTSECTION_SEARCH),
                    DepartmentSection_View = (userRoles.DEPARTMENTSECTION_VIEW),
                    DepartmentSection_Edit = (userRoles.DEPARTMENTSECTION_EDIT),

                    MixerType_Create = (userRoles.MIXERTYPE_CREATE),
                    MixerType_Search = (userRoles.MIXERTYPE_SEARCH),
                    MixerType_View = (userRoles.MIXERTYPE_VIEW),
                    MixerType_Edit = (userRoles.MIXERTYPE_EDIT),

                    Mixer_Create = (userRoles.MIXER_CREATE),
                    Mixer_Search = (userRoles.MIXER_SEARCH),
                    Mixer_View = (userRoles.MIXER_VIEW),
                    Mixer_Edit = (userRoles.MIXER_EDIT),

                    SampleTestCategory_Create = (userRoles.SAMPLETESTCATEGORY_CREATE),
                    SampleTestCategory_Search = (userRoles.SAMPLETESTCATEGORY_SEARCH),
                    SampleTestCategory_View = (userRoles.SAMPLETESTCATEGORY_VIEW),
                    SampleTestCategory_Edit = (userRoles.SAMPLETESTCATEGORY_EDIT),

                    SampleTest_Create = (userRoles.SAMPLETEST_CREATE),
                    SampleTest_Search = (userRoles.SAMPLETEST_SEARCH),
                    SampleTest_View = (userRoles.SAMPLETEST_VIEW),
                    SampleTest_Edit = (userRoles.SAMPLETEST_EDIT),

                    CRTYPEMC_Create = (userRoles.CRTYPEMC_CREATE),
                    CRTYPEMC_Search = (userRoles.CRTYPEMC_SEARCH),
                    CRTYPEMC_View = (userRoles.CRTYPEMC_VIEW),
                    CRTYPEMC_Edit = (userRoles.CRTYPEMC_EDIT),

                    CRTypeGroup_Create = (userRoles.CRTYPEGROUP_CREATE),
                    CRTypeGroup_Search = (userRoles.CRTYPEGROUP_SEARCH),
                    CRTypeGroup_View = (userRoles.CRTYPEGROUP_VIEW),
                    CRTypeGroup_Edit = (userRoles.CRTYPEGROUP_EDIT),

                    CRType_Create = (userRoles.CRTYPE_CREATE),
                    CRType_Search = (userRoles.CRTYPE_SEARCH),
                    CRType_View = (userRoles.CRTYPE_VIEW),
                    CRType_Edit = (userRoles.CRTYPE_EDIT),

                    CheckListItem_Create = (userRoles.CHECKLISTITEM_CREATE),
                    CheckListItem_Search = (userRoles.CHECKLISTITEM_SEARCH),
                    CheckListItem_View = (userRoles.CHECKLISTITEM_VIEW),
                    CheckListItem_Edit = (userRoles.CHECKLISTITEM_EDIT),

                    CheckListItemGroup_Create = (userRoles.CHECKLISTITEMGROUP_CREATE),
                    CheckListItemGroup_Search = (userRoles.CHECKLISTITEMGROUP_SEARCH),
                    CheckListItemGroup_View = (userRoles.CHECKLISTITEMGROUP_VIEW),
                    CheckListItemGroup_Edit = (userRoles.CHECKLISTITEMGROUP_EDIT),

                    CheckLists_Create = (userRoles.CHECKLISTS_CREATE),
                    CheckLists_Search = (userRoles.CHECKLISTS_SEARCH),
                    CheckLists_View = (userRoles.CHECKLISTS_VIEW),
                    CheckLists_Edit = (userRoles.CHECKLISTS_EDIT),

                    UserProfile_Create = (userRoles.USERPROFILE_CREATE),
                    UserProfile_Search = (userRoles.USERPROFILE_SEARCH),
                    UserProfile_View = (userRoles.USERPROFILE_VIEW),
                    UserProfile_Edit = (userRoles.USERPROFILE_EDIT),

                    UserRoles_Create = (userRoles.USERROLES_CREATE),
                    UserRoles_Search = (userRoles.USERROLES_SEARCH),
                    UserRoles_View = (userRoles.USERROLES_VIEW),
                    UserRoles_Edit = (userRoles.USERROLES_EDIT),

                    Project_Create = (userRoles.PROJECT_CREATE),
                    Project_Search = (userRoles.PROJECT_SEARCH),
                    Project_View = (userRoles.PROJECT_VIEW),
                    Project_Edit = (userRoles.PROJECT_EDIT),

                    Complaint_Create = (userRoles.COMPLAINT_CREATE),
                    Complaint_Search = (userRoles.COMPLAINT_SEARCH),
                    Complaint_View = (userRoles.COMPLAINT_VIEW),
                    Complaint_Edit = (userRoles.COMPLAINT_EDIT),

                    CLWF_Sequence_Create = (userRoles.CLWFSEQUENCE_CREATE),
                    CLWF_Sequence_Search = (userRoles.CLWFSEQUENCE_SEARCH),
                    CLWF_Sequence_View = (userRoles.CLWFSEQUENCE_VIEW),
                    CLWF_Sequence_Edit = (userRoles.CLWFSEQUENCE_EDIT),

                    CLWF_Create = (userRoles.CLWF_CREATE),
                    CLWF_Maker = (userRoles.CLWF_MAKER),
                    CLWF_Checker = (userRoles.CLWF_CHECKER),


                    CR_Create = (userRoles.CR_CREATE),
                    CR_Edit = (userRoles.CR_EDIT),
                    CR_Action = (userRoles.CR_ACTION),
                    CR_AddAttachments = (userRoles.CR_ADDATTACHMENTS),
                    CR_ViewAttachments = (userRoles.CR_VIEWATTACHMENTS),
                    CR_View = (userRoles.CR_VIEW),
                    CR_Search = (userRoles.CR_SEARCH),

                    Dashboard_SuperEng = (userRoles.DASHBOARD_SUPERENG),
                    Dashboard_ConsultantEng = (userRoles.DASHBOARD_CONSULTANTENG),
                    Dashboard_AuthLab = (userRoles.DASHBOARD_AUTHLAB),
                    Dashboard_Technician = (userRoles.DASHBOARD_TECHNICIAN),
                    Dashboard_CEO = (userRoles.DASHBOARD_CEO),
                    Dashboard_QualityEng = (userRoles.DASHBOARD_QUALITYENG),

                    Reports_CR_View = (userRoles.REPORTS_CR_VIEW),
                    Reports_CR_Search = (userRoles.REPORTS_CR_SEARCH),
                    Reports_CR_Projects_Search = (userRoles.REPORTS_CR_PROJECTS_SEARCH),
                    Reports_CR_Samples_Search = (userRoles.REPORTS_CR_SAMPLES_SEARCH),
                    Reports_CR_Samples_SearchDetailed = (userRoles.REPORTS_CR_SMPL_SRCHDET),
                    Reports_Project_View = (userRoles.REPORTS_PROJECT_VIEW),

                    RCV_RandomSearch = (userRoles.RCV_RANDOMSEARCH),
                    RCV_Action = (userRoles.RCV_ACTION),
                    RCV_Search = (userRoles.RCV_SEARCH),
                    RCV_Edit = (userRoles.RCV_EDIT),

                    RCVMM_Create = (userRoles.RCVMM_CREATE),
                    RCVMM_Search = (userRoles.RCVMM_SEARCH),
                    RCVMM_Edit = (userRoles.RCVMM_EDIT),
                    RCVMM_View = (userRoles.RCVMM_VIEW),

                    RCV_Conversation_View = (userRoles.RCV_CONV_VIEW),
                    RCV_Conversation_Reply = (userRoles.RCV_CONV_REPLY),
                    RCV_Conversation_Close = (userRoles.RCV_CONV_CLOSE),
                    RCV_Conversation_Escalation = (userRoles.RCV_CONV_ESCALATION)
                }).ToList();
               

            }
            catch (Exception ex)
            {
                throw ex;
            }
           

            return searchResult;
        }

        public Ent_UserRoles viewRole(int RoleID)
        {
            Ent_UserRoles viewResult = new Ent_UserRoles();

            try
            {
                var userRoles = ctx.USER_ROLES.FirstOrDefault(d => d.ROLE_ID == RoleID);
                if(userRoles != null)
                {
                    viewResult = new Ent_UserRoles
                    {

                        roleName = userRoles.NAME,
                        initialScreenID = userRoles.INITIAL_SCREEN_ID,
                        isActive = userRoles.IS_ACTIVE,
                        makerID = userRoles.MAKER,

                        projectOwnerType_Create = (userRoles.PROJECTOWNERTYPE_CREATE),
                        projectOwnerType_Search = (userRoles.PROJECTOWNERTYPE_SEARCH),
                        projectOwnerType_View = (userRoles.PROJECTOWNERTYPE_VIEW),
                        projectOwnerType_Edit = (userRoles.PROJECTOWNERTYPE_EDIT),

                        ProjectOwner_Create = (userRoles.PROJECTOWNER_CREATE),
                        ProjectOwner_Search = (userRoles.PROJECTOWNER_SEARCH),
                        ProjectOwner_View = (userRoles.PROJECTOWNER_VIEW),
                        ProjectOwner_Edit = (userRoles.PROJECTOWNER_EDIT),

                        FactoryType_Create = (userRoles.FACTORYTYPE_CREATE),
                        FactoryType_Search = (userRoles.FACTORYTYPE_SEARCH),
                        FactoryType_View = (userRoles.FACTORYTYPE_VIEW),
                        FactoryType_Edit = (userRoles.FACTORYTYPE_EDIT),

                        Factory_Create = (userRoles.FACTORY_CREATE),
                        Factory_Search = (userRoles.FACTORY_SEARCH),
                        Factory_View = (userRoles.FACTORY_VIEW),
                        Factory_Edit = (userRoles.FACTORY_EDIT),
                        Department_Create = (userRoles.DEPARTMENT_CREATE),
                        Department_Search = (userRoles.DEPARTMENT_SEARCH),
                        Department_View = (userRoles.DEPARTMENT_VIEW),
                        Department_Edit = (userRoles.DEPARTMENT_EDIT),

                        DepartmentSection_Create = (userRoles.DEPARTMENTSECTION_CREATE),
                        DepartmentSection_Search = (userRoles.DEPARTMENTSECTION_SEARCH),
                        DepartmentSection_View = (userRoles.DEPARTMENTSECTION_VIEW),
                        DepartmentSection_Edit = (userRoles.DEPARTMENTSECTION_EDIT),

                        MixerType_Create = (userRoles.MIXERTYPE_CREATE),
                        MixerType_Search = (userRoles.MIXERTYPE_SEARCH),
                        MixerType_View = (userRoles.MIXERTYPE_VIEW),
                        MixerType_Edit = (userRoles.MIXERTYPE_EDIT),

                        Mixer_Create = (userRoles.MIXER_CREATE),
                        Mixer_Search = (userRoles.MIXER_SEARCH),
                        Mixer_View = (userRoles.MIXER_VIEW),
                        Mixer_Edit = (userRoles.MIXER_EDIT),

                        SampleTestCategory_Create = (userRoles.SAMPLETESTCATEGORY_CREATE),
                        SampleTestCategory_Search = (userRoles.SAMPLETESTCATEGORY_SEARCH),
                        SampleTestCategory_View = (userRoles.SAMPLETESTCATEGORY_VIEW),
                        SampleTestCategory_Edit = (userRoles.SAMPLETESTCATEGORY_EDIT),

                        SampleTest_Create = (userRoles.SAMPLETEST_CREATE),
                        SampleTest_Search = (userRoles.SAMPLETEST_SEARCH),
                        SampleTest_View = (userRoles.SAMPLETEST_VIEW),
                        SampleTest_Edit = (userRoles.SAMPLETEST_EDIT),

                        CRTYPEMC_Create = (userRoles.CRTYPEMC_CREATE),
                        CRTYPEMC_Search = (userRoles.CRTYPEMC_SEARCH),
                        CRTYPEMC_View = (userRoles.CRTYPEMC_VIEW),
                        CRTYPEMC_Edit = (userRoles.CRTYPEMC_EDIT),

                        CRTypeGroup_Create = (userRoles.CRTYPEGROUP_CREATE),
                        CRTypeGroup_Search = (userRoles.CRTYPEGROUP_SEARCH),
                        CRTypeGroup_View = (userRoles.CRTYPEGROUP_VIEW),
                        CRTypeGroup_Edit = (userRoles.CRTYPEGROUP_EDIT),

                        CRType_Create = (userRoles.CRTYPE_CREATE),
                        CRType_Search = (userRoles.CRTYPE_SEARCH),
                        CRType_View = (userRoles.CRTYPE_VIEW),
                        CRType_Edit = (userRoles.CRTYPE_EDIT),

                        CheckListItem_Create = (userRoles.CHECKLISTITEM_CREATE),
                        CheckListItem_Search = (userRoles.CHECKLISTITEM_SEARCH),
                        CheckListItem_View = (userRoles.CHECKLISTITEM_VIEW),
                        CheckListItem_Edit = (userRoles.CHECKLISTITEM_EDIT),

                        CheckListItemGroup_Create = (userRoles.CHECKLISTITEMGROUP_CREATE),
                        CheckListItemGroup_Search = (userRoles.CHECKLISTITEMGROUP_SEARCH),
                        CheckListItemGroup_View = (userRoles.CHECKLISTITEMGROUP_VIEW),
                        CheckListItemGroup_Edit = (userRoles.CHECKLISTITEMGROUP_EDIT),

                        CheckLists_Create = (userRoles.CHECKLISTS_CREATE),
                        CheckLists_Search = (userRoles.CHECKLISTS_SEARCH),
                        CheckLists_View = (userRoles.CHECKLISTS_VIEW),
                        CheckLists_Edit = (userRoles.CHECKLISTS_EDIT),

                        UserProfile_Create = (userRoles.USERPROFILE_CREATE),
                        UserProfile_Search = (userRoles.USERPROFILE_SEARCH),
                        UserProfile_View = (userRoles.USERPROFILE_VIEW),
                        UserProfile_Edit = (userRoles.USERPROFILE_EDIT),

                        UserRoles_Create = (userRoles.USERROLES_CREATE),
                        UserRoles_Search = (userRoles.USERROLES_SEARCH),
                        UserRoles_View = (userRoles.USERROLES_VIEW),
                        UserRoles_Edit = (userRoles.USERROLES_EDIT),

                        Project_Create = (userRoles.PROJECT_CREATE),
                        Project_Search = (userRoles.PROJECT_SEARCH),
                        Project_View = (userRoles.PROJECT_VIEW),
                        Project_Edit = (userRoles.PROJECT_EDIT),

                        Complaint_Create = (userRoles.COMPLAINT_CREATE),
                        Complaint_Search = (userRoles.COMPLAINT_SEARCH),
                        Complaint_View = (userRoles.COMPLAINT_VIEW),
                        Complaint_Edit = (userRoles.COMPLAINT_EDIT),

                        CLWF_Sequence_Create = (userRoles.CLWFSEQUENCE_CREATE),
                        CLWF_Sequence_Search = (userRoles.CLWFSEQUENCE_SEARCH),
                        CLWF_Sequence_View = (userRoles.CLWFSEQUENCE_VIEW),
                        CLWF_Sequence_Edit = (userRoles.CLWFSEQUENCE_EDIT),

                        CLWF_Create = (userRoles.CLWF_CREATE),
                        CLWF_Maker = (userRoles.CLWF_MAKER),
                        CLWF_Checker = (userRoles.CLWF_CHECKER),


                        CR_Create = (userRoles.CR_CREATE),
                        CR_Edit = (userRoles.CR_EDIT),
                        CR_Action = (userRoles.CR_ACTION),
                        CR_AddAttachments = (userRoles.CR_ADDATTACHMENTS),
                        CR_ViewAttachments = (userRoles.CR_VIEWATTACHMENTS),
                        CR_View = (userRoles.CR_VIEW),
                        CR_Search = (userRoles.CR_SEARCH),

                        Dashboard_SuperEng = (userRoles.DASHBOARD_SUPERENG),
                        Dashboard_ConsultantEng = (userRoles.DASHBOARD_CONSULTANTENG),
                        Dashboard_AuthLab = (userRoles.DASHBOARD_AUTHLAB),
                        Dashboard_Technician = (userRoles.DASHBOARD_TECHNICIAN),
                        Dashboard_CEO = (userRoles.DASHBOARD_CEO),
                        Dashboard_QualityEng = (userRoles.DASHBOARD_QUALITYENG),

                        Reports_CR_View = (userRoles.REPORTS_CR_VIEW),
                        Reports_CR_Search = (userRoles.REPORTS_CR_SEARCH),
                        Reports_CR_Projects_Search = (userRoles.REPORTS_CR_PROJECTS_SEARCH),
                        Reports_CR_Samples_Search = (userRoles.REPORTS_CR_SAMPLES_SEARCH),
                        Reports_CR_Samples_SearchDetailed = (userRoles.REPORTS_CR_SMPL_SRCHDET),
                        Reports_Project_View = (userRoles.REPORTS_PROJECT_VIEW),

                        RCV_RandomSearch = (userRoles.RCV_RANDOMSEARCH),
                        RCV_Action = (userRoles.RCV_ACTION),
                        RCV_Search = (userRoles.RCV_SEARCH),
                        RCV_Edit = (userRoles.RCV_EDIT),

                        RCVMM_Create = (userRoles.RCVMM_CREATE),
                        RCVMM_Search = (userRoles.RCVMM_SEARCH),
                        RCVMM_Edit = (userRoles.RCVMM_EDIT),
                        RCVMM_View = (userRoles.RCVMM_VIEW),

                        RCV_Conversation_View = (userRoles.RCV_CONV_VIEW),
                        RCV_Conversation_Reply = (userRoles.RCV_CONV_REPLY),
                        RCV_Conversation_Close = (userRoles.RCV_CONV_CLOSE),
                        RCV_Conversation_Escalation = (userRoles.RCV_CONV_ESCALATION)
                    };
                } 
                
                
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return viewResult;
        }
    }
}