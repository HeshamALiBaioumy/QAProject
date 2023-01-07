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
    public class Mdl_LOV
    {
        public QualityDbEntities ctx=new QualityDbEntities ();
        public enum searchEntities
        {
            EmptyList
                , Department, Sections, Department_Sections
                , ProjectOwnerType, Project_Owner, projectOwner_IsOwner, SupervisorEngineers, projectOwner_SupervisorEngineers
                    , projectOwner_Departments
                , Factory_Type, Factory
                , Mixer_Type, Mixer
                , CR_Type_MainCategories, CRType_MC_Groups, CRType_Groups, CR_Group_Types, CR_Types, CRType_Categories, CR_Statuses
                    , CR_SampleTest_Result
                , Sample_Categories, Sample_Category_Tests
                , Complaint_Status,UserProfile
                , Checklist_Items, Checklist_Groups, Checklists, Checklist_Flow_Statuses, Checklist_Flow_Sequences
                , Projects, ProjectCRs, Project_Items, Project_Items_All, ContractorAssistant_projects
                , CRs
                , Generic_Units //Project_Milestone_AmtUnits
                , UserProfile_NationalityTypes, UserProfile_Nationalities, UserProfile_UserTypes
                    , UserProfile_UserType_SuperUsers, UserProfile_Roles, UserProfile_Consultants, UserProfile_All_Consultant_Assistants
                    , UserProfile_Consultant_Assistants, UserProfile_Contractor, UserProfile_All_Contractor_Assistants, UserProfile_Contractor_Assistants
                    , UserProfile_AuthLab, UserProfile_QATech, UserProfile_SupervisorEngs, UserProfile_QALAB, UserProfile_RepSuper
                    , UserProfile_QualityEngineers
                , UserRoles_InitialScreens
                , RCVs, RCV_Status, RCVMM_Status, RCVMM_PendingOn
        };

        public List<LOV> fillLOV(searchEntities searchEntity)
        {
            return fillLOV(searchEntity, parentID: -1);
        }

        public List<LOV> fillLOV(searchEntities searchEntity, int parentID)
        {
            try
            {
                var lst=new List<LOV>();
                bool isDBLOV = true;
                string searchEntityName = "";

                switch (searchEntity)
                {
                    case searchEntities.Department:
                            searchEntityName = "department";
                            lst = ctx.DEPARTMENTs.AsNoTracking().Select(d => new LOV { id = d.DEPARTMENT_ID, value = d.NAME, idStr = d.NAME }).ToList();
                        
                        break;
                    case searchEntities.Sections:
                        searchEntityName = "Sections";
                        lst = ctx.DEPARTMENT_SECTION.AsNoTracking().Select(d => new LOV { id = d.DEPARTMENT_SECTION_ID, value = d.NAME, idStr = d.NAME }).ToList();

                        break;
                    case searchEntities.Department_Sections:
                        searchEntityName = "Department_Sections";
                        lst = ctx.DEPARTMENT_SECTION.AsNoTracking().Select(d => new LOV { id = d.DEPARTMENT_SECTION_ID, value = d.NAME, idStr = d.NAME }).ToList();

                        break;

                    case searchEntities.ProjectOwnerType:
                        searchEntityName = "projectOwnertype";
                        lst = ctx.PROJECT_OWNER_TYPE.AsNoTracking().Select(d => new LOV { id = d.PROJ_OWN_TYP_ID, value = d.NAME, idStr = d.NAME }).ToList();

                        break;
                    case searchEntities.Project_Owner:
                        searchEntityName = "projectOwner";
                        lst = ctx.PROJECT_OWNER_TYPE.AsNoTracking().Select(d => new LOV { id = d.PROJ_OWN_TYP_ID, value = d.NAME, idStr = d.NAME }).ToList();

                        break;
                    case searchEntities.projectOwner_IsOwner:
                        searchEntityName = "projectOwner_IsOwner";
                        lst = ctx.PROJECT_OWNER.AsNoTracking().Where(d=>d.IS_OWNER==true).Select(d => new LOV { id = d.PROJEC_OWNER_ID, value = d.NAME, idStr = d.NAME }).ToList();

                        break;
                    case searchEntities.SupervisorEngineers:
                        searchEntityName = "SupervisorEngineers";
                        //lst = ctx.PROJECT_OWNER.AsNoTracking().Select(d => new LOV { id = d.PROJ_OWN_TYP_ID, value = d.NAME.ToString(), idStr = d.NAME }).ToList();

                        break;
                    case searchEntities.projectOwner_SupervisorEngineers:
                        searchEntityName = "projectOwner_SupervisorEngineers";
                        break;
                    case searchEntities.projectOwner_Departments:
                        searchEntityName = "projectOwner_Departments";
                        lst = ctx.DEPARTMENTs.AsNoTracking().Where(s=>s.PROJECT_OWNER_ID == parentID && s.IS_ACTIVE == true).Select(d => new LOV { id = d.DEPARTMENT_ID, value = d.NAME, idStr = d.NAME }).ToList();
                        break;

                    case searchEntities.Factory_Type:
                        searchEntityName = "factoryType";
                        lst = ctx.FACTORY_TYPES.AsNoTracking().Select(d => new LOV { id = d.FACTORY_TYPE_ID, value = d.NAME, idStr = d.NAME }).ToList();

                        break;
                    case searchEntities.Factory:
                        searchEntityName = "factory";
                        lst = ctx.FACTORies.AsNoTracking().Select(d => new LOV { id = d.FACTORY_ID, value = d.NAME, idStr = d.NAME }).ToList();

                        break;

                    case searchEntities.Mixer_Type:
                        searchEntityName = "MixerType";
                        lst = ctx.MIXER_TYPES.AsNoTracking().Select(d => new LOV { id = d.MIXER_TYPE_ID, value = d.NAME, idStr = d.NAME }).ToList();

                        break;

                    case searchEntities.CR_Type_MainCategories:
                        searchEntityName = "CR_Type_MainCategories";
                        lst = ctx.CR_TYPES_MAIN_CATEGORIES.AsNoTracking().Select(d => new LOV { id = d.CR_TYPE_MC_ID, value = d.NAME, idStr = d.NAME }).ToList();

                        break;
                    case searchEntities.CRType_MC_Groups:
                        searchEntityName = "CRType_MC_Groups";
                        break;
                    case searchEntities.CRType_Groups:
                        searchEntityName = "CRType_Groups";
                        break;
                    case searchEntities.CR_Group_Types:
                        searchEntityName = "CR_Group_Types";
                        lst = ctx.CR_TYPE_GROUPS.AsNoTracking().Select(d => new LOV { id = d.CR_TYPE_GROUPS_ID, value = d.NAME, idStr = d.NAME }).ToList();
                        break;
                    case searchEntities.CR_Types:
                        searchEntityName = "CR_Types";
                        lst = ctx.CR_TYPES.AsNoTracking().Select(d => new LOV { id = d.CR_TYPE_ID, value = d.NAME, idStr = d.NAME }).ToList();
                        break;
                    case searchEntities.CR_Statuses:
                        searchEntityName = "CR_Statuses";
                        lst = ctx.CR_STATUSES.AsNoTracking().Select(d => new LOV { id = d.STATUS_SEQ_ID, value = d.STATUS_ENG_DESC, idStr = d.STATUS_ENG_DESC }).ToList();
                        break;

                    case searchEntities.Projects:
                        searchEntityName = "projects";
                        lst = ctx.PROJECTS.AsNoTracking().Select(d => new LOV { id = d.PROJECTS_ID, value = d.NAME, idStr = d.NAME }).ToList();

                        break;
                    case searchEntities.ContractorAssistant_projects:
                        searchEntityName = "ContractorAssistant_projects";
                        lst = ctx.PROJECTS.AsNoTracking().Where(s=>s.CONSULANT_ASSIST_ID == parentID).Select(d => new LOV { id = d.PROJECTS_ID, value = d.NAME, idStr = d.NAME }).ToList();
                        break;
                    case searchEntities.ProjectCRs:
                        searchEntityName = "projectCRs";
                        lst = ctx.CRs.AsNoTracking().Select(d => new LOV { id = d.CR_ID, value = d.REGISTER_DATE.Value.ToString("dd/MM.yyyy"), idStr = d.REGISTER_DATE.Value.ToString("dd/MM.yyyy") }).ToList();
                        break;
                    case searchEntities.Project_Items:
                        searchEntityName = "Project_Items";
                        lst = ctx.PROJECT_ITEMS.AsNoTracking().Where(s => s.PROJECT_ID == parentID).Select(d => new LOV { id = d.PROJECT_ITEMS_ID, value = d.NAME, idStr = d.NAME }).ToList();

                        break;
                    case searchEntities.Project_Items_All:
                        searchEntityName = "Project_Items_All";
                        lst = ctx.PROJECT_ITEMS.AsNoTracking().Select(d => new LOV { id = d.PROJECT_ITEMS_ID, value = d.NAME, idStr = d.NAME }).ToList();

                        break;

                    case searchEntities.CRs:
                        searchEntityName = "CRs";

                        var query = from cr in ctx.CRs
                                    join proj in ctx.PROJECTS on cr.PROJECT_ID equals proj.PROJECTS_ID
                                    join item in ctx.PROJECT_ITEMS on cr.PROJECT_ITEM_ID equals item.PROJECT_ITEMS_ID

                                    select new LOV()
                                    {
                                        id = cr.CR_ID,
                                        value = proj.PROJECTS_ID.ToString()+"-"+ proj.NAME+"-"+item.NAME+"-"+cr.REGISTER_DATE.Value.ToString("dd/MM/yyyy"),
                                        idStr = proj.PROJECTS_ID.ToString()+"-"+ proj.NAME+"-"+item.NAME+"-"+cr.REGISTER_DATE.Value.ToString("dd/MM/yyyy"),
                                    };

                      lst = query.ToList();
                        break;

                    case searchEntities.Sample_Categories:
                        searchEntityName = "Sample_Categories";
                        lst = ctx.SAMPLE_TYPE.Select(d => new LOV { id = d.SAMPLE_TYPE_ID, value = d.NAME, idStr = d.NAME }).ToList();

                        break;
                    case searchEntities.Sample_Category_Tests:
                        searchEntityName = "Sample_Category_Tests";
                        lst = ctx.SAMPLE_TEST.Select(d => new LOV { id = d.SAMPLE_TEST_ID, value = d.NAME, idStr = d.NAME }).ToList();
                        break;

                    case searchEntities.Checklist_Items:
                        searchEntityName = "Checklist_Items";
                        lst = ctx.CL_GROUPS_ITEMS.Select(d => new LOV { id = d.CL_GROUPS_ITEMS_ID, value = d.NAME, idStr = d.NAME }).ToList();
                        break;
                    case searchEntities.Checklist_Groups:
                        searchEntityName = "Checklist_Groups";
                        lst = ctx.CHECK_LIST_GROUPS.Select(d => new LOV { id = d.CHECK_LIST_GROUPS_ID, value = d.NAME, idStr = d.NAME }).ToList();
                        break;
                    case searchEntities.Checklists:
                        searchEntityName = "Checklists";
                        lst = ctx.CHECK_LIST.Select(d => new LOV { id = d.CHECK_LIST_ID, value = d.NAME, idStr = d.NAME }).ToList();
                        break;
                    case searchEntities.Checklist_Flow_Statuses:
                        searchEntityName = "Checklist_Flow_Statuses";
                        lst = ctx.CL_FLOW_STATUSES.Select(d => new LOV { id = d.STATUS_SEQ_ID, value = d.STATUS_ENG_DESC, idStr = d.STATUS_ENG_DESC }).ToList();
                        break;
                    case searchEntities.Checklist_Flow_Sequences:
                        searchEntityName = "Checklist_Flow_Sequences";
                        lst = ctx.CHECKLIST_FLOW_SEQUENCE.Select(d => new LOV { id = d.SEQUENCE_ID, value = d.SEQUENCE_NAME, idStr = d.SEQUENCE_NAME }).ToList();
                        break;

                    case searchEntities.UserProfile_Nationalities:
                        searchEntityName = "UserProfile_Nationalities";
                        break;
                    case searchEntities.UserProfile_UserTypes:
                        searchEntityName = "UserProfile_UserTypes";
                        break;
                    case searchEntities.UserProfile_UserType_SuperUsers:
                        searchEntityName = "UserProfile_UserType_SuperUsers";
                        break;
                    case searchEntities.UserProfile_Roles:
                        searchEntityName = "UserProfile_Roles";
                        break;
                    case searchEntities.UserProfile_Consultants:
                        searchEntityName = "UserProfile_Consultants";
                        break;
                    case searchEntities.UserProfile_All_Consultant_Assistants:
                        searchEntityName = "UserProfile_All_Consultant_Assistants";
                        break;
                    case searchEntities.UserProfile_Consultant_Assistants:
                        searchEntityName = "UserProfile_Consultant_Assistants";
                        break;
                    case searchEntities.UserProfile_Contractor:
                        searchEntityName = "UserProfile_Contractor";
                        break;
                    case searchEntities.UserProfile_All_Contractor_Assistants:
                        searchEntityName = "UserProfile_All_Contractor_Assistants";
                        break;
                    case searchEntities.UserProfile_Contractor_Assistants:
                        searchEntityName = "UserProfile_Contractor_Assistants";
                        break;
                    case searchEntities.UserProfile_AuthLab:
                        searchEntityName = "UserProfile_AuthLab";
                        break;
                    case searchEntities.UserProfile_QATech:
                        searchEntityName = "UserProfile_QATech";
                        var currentType = ctx.USER_PROFILE_TYPE.FirstOrDefault(s => s.NAME == "QATech");
                        if (currentType !=null)
                        {
                            lst = ctx.USERS_PROFILE.Where(s=>s.USER_TYPE_ID == currentType.TYPE_ID)
                                .Select(d => new LOV { id = d.PROFILE_ID, value = d.NAME, idStr = d.NAME }).ToList();
                        }
                        break;
                    case searchEntities.UserProfile_SupervisorEngs:
                        searchEntityName = "UserProfile_SupervisorEngs";
                        break;
                    case searchEntities.UserProfile_QALAB:
                        searchEntityName = "UserProfile_QALAB";
                        break;
                    case searchEntities.UserProfile_RepSuper:
                        searchEntityName = "UserProfile_RepSuper";
                        break;
                    case searchEntities.UserProfile_QualityEngineers:
                        searchEntityName = "UserProfile_QualityEngineers";
                        break;
                    case searchEntities.UserProfile:
                        searchEntityName = "UserProfile";
                        break;
                    case searchEntities.Generic_Units:
                        searchEntityName = "Generic_Units";
                        lst = ctx.LT_MEASURE_UNITS.Where(s=>s.IS_ACTIVE==true).Select(d => new LOV { id = d.ID, value = d.VALUE, idStr = d.VALUE }).ToList();
                        break;

                    case searchEntities.UserRoles_InitialScreens:
                        searchEntityName = "UserRoles_InitialScreens";
                        lst = ctx.LT_INITIALSCREENS.Select(d => new LOV { id = d.ID, value = d.SCREENNAME_ENG, idStr = d.SCREENNAME_ENG }).ToList();

                        break;

                    case searchEntities.RCVs:
                        searchEntityName = "RCVs";
                        break;
                    case searchEntities.RCV_Status:
                        searchEntityName = "RCV_Status";
                        break;
                    case searchEntities.RCVMM_Status:
                        searchEntityName = "RCVMM_Status";
                        break;
                    case searchEntities.RCVMM_PendingOn:
                        searchEntityName = "RCVMM_PendingOn";
                        break;

                    case searchEntities.EmptyList:
                        lst.Add(new LOV() { id = -1, value = Localization.Global.DDL_EmptyList });
                        break;
                    case searchEntities.Complaint_Status:
                        lst.Add(new LOV() { id = -1, value = Localization.Complaint.StatusDDL_Select });
                        lst.Add(new LOV() { id = 0, value = Localization.Complaint.StatusDDL_NewComplaint });
                        lst.Add(new LOV() { id = 1, value = Localization.Complaint.StatusDDL_UnderInvestigation });
                        lst.Add(new LOV() { id = 2, value = Localization.Complaint.StatusDDL_Accepted });
                        lst.Add(new LOV() { id = 3, value = Localization.Complaint.StatusDDL_Rejected });
                        break;
                    case searchEntities.CRType_Categories:
                        lst = ctx.CR_TYPES_MAIN_CATEGORIES.Select(d => new LOV { id = d.CR_TYPE_MC_ID, value = d.NAME, idStr = d.NAME }).ToList();

                        break;
                    //case searchEntities.CR_Statuses:
                    case searchEntities.UserProfile_NationalityTypes:
                    //case searchEntities.Project_Milestone_AmtUnits:
                    case searchEntities.CR_SampleTest_Result:
                        isDBLOV = false;
                        break;
                    default:
                        throw new Exception("Un-handled LOV Search Case !!");
                }
                return lst;
                //if (isDBLOV)
                //{
                //    return searchLOV(searchEntityName, parentID);
                //}
                //else
                //{
                //    return searchLOV(searchEntity);
                //}
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private List<LOV> searchLOV(searchEntities searchEntity)
        {
            List<LOV> lovLst = new List<LOV>();
            switch (searchEntity)
            {
                case searchEntities.EmptyList:
                    lovLst.Add(new LOV() { id = -1, value = Localization.Global.DDL_EmptyList });
                    break;
                case searchEntities.Complaint_Status:
                    lovLst.Add(new LOV() { id = -1, value = Localization.Complaint.StatusDDL_Select });
                    lovLst.Add(new LOV() { id = 0, value = Localization.Complaint.StatusDDL_NewComplaint });
                    lovLst.Add(new LOV() { id = 1, value = Localization.Complaint.StatusDDL_UnderInvestigation });
                    lovLst.Add(new LOV() { id = 2, value = Localization.Complaint.StatusDDL_Accepted });
                    lovLst.Add(new LOV() { id = 3, value = Localization.Complaint.StatusDDL_Rejected });
                    break;
                case searchEntities.CRType_Categories:
                    lovLst.Add(new LOV() { id = -1, value = Localization.Global.DDL_EmptyList });
                    lovLst.Add(new LOV() { id = 0, value = Localization.CRType.DDL_Category_Checkup });
                    lovLst.Add(new LOV() { id = 1, value = Localization.CRType.DDL_Category_withoutSample });
                    break;
                case searchEntities.UserProfile_NationalityTypes:
                    lovLst.Add(new LOV() { id = -1, value = Localization.Global.DDL_EmptyList });
                    lovLst.Add(new LOV() { id = 0, value = Localization.UserProfile.DDL_NatTypes_NID });
                    lovLst.Add(new LOV() { id = 1, value = Localization.UserProfile.DDL_NatTypes_VisitorID });
                    break;
                //case searchEntities.CR_Statuses:
                //    lovLst.Add(new LOV() { id = 0, value = Localization.CR.DDL_CRStatus_NewCR });
                //    lovLst.Add(new LOV() { id = 1, value = Localization.CR.DDL_CRStatus_WIP });
                //    lovLst.Add(new LOV() { id = 2, value = Localization.CR.DDL_CRStatus_Accepted });
                //    lovLst.Add(new LOV() { id = 3, value = Localization.CR.DDL_CRStatus_Rejected });
                //    break;
                //case searchEntities.Project_Milestone_AmtUnits:
                //    lovLst.Add(new LOV() { id = -1, value = Localization.Project.tbl_ProjMlstone_AmtUnit_Select });
                //    lovLst.Add(new LOV() { id = 0, value = Localization.Project.tbl_ProjMlstone_AmtUnit_M });
                //    lovLst.Add(new LOV() { id = 1, value = Localization.Project.tbl_ProjMlstone_AmtUnit_KM });
                //    lovLst.Add(new LOV() { id = 2, value = Localization.Project.tbl_ProjMlstone_AmtUnit_KG });
                //    break;
                case searchEntities.CR_SampleTest_Result:
                    lovLst.Add(new LOV() { id = -1, value = Localization.Global.DDL_EmptyList });
                    lovLst.Add(new LOV() { id = 0, value = Localization.Attachment.SampleResult_Accepted });
                    lovLst.Add(new LOV() { id = 1, value = Localization.Attachment.SampleResult_Rejected });
                    break;
            }

            return lovLst;
        }

        private List<LOV> searchLOV(string searchEntity, int parentID)
        {
            
            List<LOV> searchResult = new List<LOV>();
            searchResult.Add(new LOV() { id = -1, value = Localization.Global.LOVSelect });

            try
            {
                using (SqlConnection dbConnection = new SqlConnection(
                    ConfigurationManager.ConnectionStrings["dbConnectionString"].ConnectionString))
                using (SqlCommand dbCommand = dbConnection.CreateCommand())
                {
                    dbCommand.CommandText = "SP_Search_LOV";
                    dbCommand.CommandType = CommandType.StoredProcedure;
                    dbCommand.Parameters.Add("Inpt_EntityName", SqlDbType.NVarChar).Value = searchEntity;
                    dbCommand.Parameters.Add("inpt_ParentID", SqlDbType.Int).Value = parentID;
                    dbCommand.Parameters.Add("searchResultCursor", SqlDbType.Int).Direction = ParameterDirection.Output;

                    dbConnection.Open();
                    SqlDataReader reader = dbCommand.ExecuteReader();
                    while (reader.Read())
                    {
                        searchResult.Add(new LOV()
                        {
                            id = reader.GetInt32(reader.GetOrdinal("Item_ID")),
                            idStr = (searchEntity.Equals("UserProfile_Nationalities")) ?
                                reader.GetString(reader.GetOrdinal("Item_IDStr")) : null,
                            value = reader.GetString(reader.GetOrdinal("Item_Value"))
                        });
                    }
                    reader.Close();
                    dbCommand.Dispose();
                    dbConnection.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return searchResult;
        }

        public static string getNationalityTypeName(int nationalityTypeID)
        {
            try
            {
                string strResult = "";

                switch (nationalityTypeID)
                {
                    case 0:
                        strResult = Localization.UserProfile.DDL_NatTypes_NID;
                        break;
                    case 1:
                        strResult = Localization.UserProfile.DDL_NatTypes_VisitorID;
                        break;
                    default:
                        strResult = "Unknown Code, plesae Contact Admin!";
                        break;
                }

                return strResult;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public static string getAmountUnitsName(int amountUnitID)
        {
            try
            {
                string strResult = "";

                switch (amountUnitID)
                {
                    case -1:
                        strResult = Localization.Project.tbl_ProjMlstone_AmtUnit_Select;
                        break;
                    case 0:
                        strResult = Localization.Project.tbl_ProjMlstone_AmtUnit_M;
                        break;
                    case 1:
                        strResult = Localization.Project.tbl_ProjMlstone_AmtUnit_KM;
                        break;
                    case 2:
                        strResult = Localization.Project.tbl_ProjMlstone_AmtUnit_KG;
                        break;
                    default:
                        strResult = "Unknown Code, plesae Contact Admin!";
                        break;
                }

                return strResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}