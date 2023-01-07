using QA.Entities.Session_Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QA.Entities.Business_Entities
{
    public class Ent_UserRoles
    {
        public int userRoleID { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Role_Name", ResourceType = typeof(Localization.UserRoles))]
        [Required(ErrorMessageResourceType = typeof(Localization.UserRoles)
            , ErrorMessageResourceName = "provide_valid_Name")]
        [RegularExpression(RegexEnum.validText, ErrorMessageResourceType = typeof(Localization.Global)
            , ErrorMessageResourceName = "Special_Characters_Not_Allowed")]
        [StringLength(maximumLength: 100, MinimumLength = 5, ErrorMessageResourceType = typeof(Localization.UserRoles)
            , ErrorMessageResourceName = "Name_length_validation")]
        [Remote("IsValidUserRoleName", "UserRoles", HttpMethod = "POST", AdditionalFields = "userRoleID"
            , ErrorMessageResourceType = typeof(Localization.UserRoles), ErrorMessageResourceName = "NameAlreadyExist")]
        public string roleName { get; set; }

        [Display(Name = "initialScreenID", ResourceType = typeof(Localization.UserRoles))]
        [Range(minimum: 0, maximum: int.MaxValue, ErrorMessageResourceType = typeof(Localization.UserRoles)
            , ErrorMessageResourceName = "provide_avalid_initialScreenID")]
        public int initialScreenID { get; set; }

        public string initialScreenName { get; set; }

        [Display(Name = "boolIsActive", ResourceType = typeof(Localization.Global))]
        public bool isActive { get; set; }

        public string makerID { get; set; }

        // projectOwnerType
        public bool projectOwnerType
        {
            get
            {
                return (projectOwnerType_Create || projectOwnerType_Search);
            }
        }

        public bool projectOwnerType_Create { get; set; }

        public bool projectOwnerType_Search { get; set; }

        public bool projectOwnerType_View { get; set; }

        public bool projectOwnerType_Edit { get; set; }

        // ProjectOwner
        public bool ProjectOwner
        {
            get
            {
                return (ProjectOwner_Create || ProjectOwner_Search);
            }
        }

        public bool ProjectOwner_Create { get; set; }

        public bool ProjectOwner_Search { get; set; }

        public bool ProjectOwner_View { get; set; }

        public bool ProjectOwner_Edit { get; set; }

        // FactoryType
        public bool FactoryType
        {
            get
            {
                return (FactoryType_Create || FactoryType_Search);
            }
        }

        public bool FactoryType_Create { get; set; }

        public bool FactoryType_Search { get; set; }

        public bool FactoryType_View { get; set; }

        public bool FactoryType_Edit { get; set; }

        // Factory
        public bool Factory
        {
            get
            {
                return (Factory_Create || Factory_Search);
            }
        }

        public bool Factory_Create { get; set; }

        public bool Factory_Search { get; set; }

        public bool Factory_View { get; set; }

        public bool Factory_Edit { get; set; }

        // Department
        public bool Department
        {
            get
            {
                return (Department_Create || Department_Search);
            }
        }

        public bool Department_Create { get; set; }

        public bool Department_Search { get; set; }

        public bool Department_View { get; set; }

        public bool Department_Edit { get; set; }

        // DepartmentSection
        public bool DepartmentSection
        {
            get
            {
                return (DepartmentSection_Create || DepartmentSection_Search);
            }
        }

        public bool DepartmentSection_Create { get; set; }

        public bool DepartmentSection_Search { get; set; }

        public bool DepartmentSection_View { get; set; }

        public bool DepartmentSection_Edit { get; set; }

        // MixerType
        public bool MixerType
        {
            get
            {
                return (MixerType_Create || MixerType_Search);
            }
        }

        public bool MixerType_Create { get; set; }

        public bool MixerType_Search { get; set; }

        public bool MixerType_View { get; set; }

        public bool MixerType_Edit { get; set; }

        // Mixer
        public bool Mixer
        {
            get
            {
                return (Mixer_Create || Mixer_Search);
            }
        }

        public bool Mixer_Create { get; set; }

        public bool Mixer_Search { get; set; }

        public bool Mixer_View { get; set; }

        public bool Mixer_Edit { get; set; }

        // SampleTestCategory
        public bool SampleTestCategory
        {
            get
            {
                return (SampleTestCategory_Create || SampleTestCategory_Search);
            }
        }

        public bool SampleTestCategory_Create { get; set; }

        public bool SampleTestCategory_Search { get; set; }

        public bool SampleTestCategory_View { get; set; }

        public bool SampleTestCategory_Edit { get; set; }

        // SampleTest
        public bool SampleTest
        {
            get
            {
                return (SampleTest_Create || SampleTest_Search);
            }
        }

        public bool SampleTest_Create { get; set; }

        public bool SampleTest_Search { get; set; }

        public bool SampleTest_View { get; set; }

        public bool SampleTest_Edit { get; set; }

        // CRTYPEMC
        public bool CRTYPEMC
        {
            get
            {
                return (CRTYPEMC_Create || CRTYPEMC_Search);
            }
        }

        public bool CRTYPEMC_Create { get; set; }

        public bool CRTYPEMC_Search { get; set; }

        public bool CRTYPEMC_View { get; set; }

        public bool CRTYPEMC_Edit { get; set; }

        // CRTypeGroup
        public bool CRTypeGroup
        {
            get
            {
                return (CRTypeGroup_Create || CRTypeGroup_Search);
            }
        }

        public bool CRTypeGroup_Create { get; set; }

        public bool CRTypeGroup_Search { get; set; }

        public bool CRTypeGroup_View { get; set; }

        public bool CRTypeGroup_Edit { get; set; }

        // CRType
        public bool CRType
        {
            get
            {
                return (CRType_Create || CRType_Search);
            }
        }

        public bool CRType_Create { get; set; }

        public bool CRType_Search { get; set; }

        public bool CRType_View { get; set; }

        public bool CRType_Edit { get; set; }

        // CheckListItem
        public bool CheckListItem
        {
            get
            {
                return (CheckListItem_Create || CheckListItem_Search);
            }
        }

        public bool CheckListItem_Create { get; set; }

        public bool CheckListItem_Search { get; set; }

        public bool CheckListItem_View { get; set; }

        public bool CheckListItem_Edit { get; set; }

        // CheckListItemGroup
        public bool CheckListItemGroup
        {
            get
            {
                return (CheckListItemGroup_Create || ProjectOwner_Search);
            }
        }

        public bool CheckListItemGroup_Create { get; set; }

        public bool CheckListItemGroup_Search { get; set; }

        public bool CheckListItemGroup_View { get; set; }

        public bool CheckListItemGroup_Edit { get; set; }

        // CheckLists
        public bool CheckLists
        {
            get
            {
                return (CheckLists_Create || CheckLists_Search);
            }
        }

        public bool CheckLists_Create { get; set; }

        public bool CheckLists_Search { get; set; }

        public bool CheckLists_View { get; set; }

        public bool CheckLists_Edit { get; set; }

        // UserProfile
        public bool UserProfile
        {
            get
            {
                return (UserProfile_Create || UserProfile_Search);
            }
        }

        public bool UserProfile_Create { get; set; }

        public bool UserProfile_Search { get; set; }

        public bool UserProfile_View { get; set; }

        public bool UserProfile_Edit { get; set; }

        // UserRoles
        public bool UserRoles
        {
            get
            {
                return (UserRoles_Create || UserRoles_Search);
            }
        }

        public bool UserRoles_Create { get; set; }

        public bool UserRoles_Search { get; set; }

        public bool UserRoles_View { get; set; }

        public bool UserRoles_Edit { get; set; }

        // Project
        public bool Project
        {
            get
            {
                return (Project_Create || Project_Search);
            }
        }

        public bool Project_Create { get; set; }

        public bool Project_Search { get; set; }

        public bool Project_View { get; set; }

        public bool Project_Edit { get; set; }

        // Complaint
        public bool Complaint
        {
            get
            {
                return (Complaint_Create || Complaint_Search);
            }
        }

        public bool Complaint_Create { get; set; }

        public bool Complaint_Search { get; set; }

        public bool Complaint_View { get; set; }

        public bool Complaint_Edit { get; set; }

        // CLWF_Sequence
        public bool CLWF_Sequence
        {
            get
            {
                return (CLWF_Sequence_Create || CLWF_Sequence_Search);
            }
        }

        public bool CLWF_Sequence_Create { get; set; }

        public bool CLWF_Sequence_Search { get; set; }

        public bool CLWF_Sequence_View { get; set; }

        public bool CLWF_Sequence_Edit { get; set; }

        // CLWF
        public bool CLWF
        {
            get
            {
                return (CLWF_Create || CLWF_Maker || CLWF_Checker);
            }
        }

        public bool CLWF_Create { get; set; }

        public bool CLWF_Maker { get; set; }

        public bool CLWF_Checker { get; set; }

        // CR
        public bool CR
        {
            get
            {
                return (CR_Create || CR_Search || CR_Action || CR_AddAttachments || CR_ViewAttachments || CR_Edit || CR_View);
            }
        }

        public bool CR_Create { get; set; }

        public bool CR_Edit { get; set; }

        public bool CR_Action { get; set; }

        public bool CR_AddAttachments { get; set; }

        public bool CR_ViewAttachments { get; set; }

        public bool CR_View { get; set; }

        public bool CR_Search { get; set; }

        // Dashboard
        public bool Dashboard
        {
            get
            {
                return (Dashboard_SuperEng || Dashboard_ConsultantEng || Dashboard_AuthLab 
                    || Dashboard_Technician || Dashboard_CEO || Dashboard_QualityEng);
            }
        }

        public bool Dashboard_SuperEng { get; set; }

        public bool Dashboard_ConsultantEng { get; set; }

        public bool Dashboard_AuthLab { get; set; }

        public bool Dashboard_Technician { get; set; }

        public bool Dashboard_CEO { get; set; }

        public bool Dashboard_QualityEng { get; set; }

        // Reports - CR - Menu
        public bool CR_Reports
        {
            get
            {
                return (Reports_CR_Search || Reports_CR_Projects_Search || Reports_CR_Samples_Search
                    || Reports_CR_Samples_SearchDetailed);
            }
        }

        public bool Reports_CR_Search { get; set; }

        public bool Reports_CR_Projects_Search { get; set; }

        public bool Reports_CR_Samples_Search { get; set; }

        public bool Reports_CR_Samples_SearchDetailed { get; set; }

        // Reports - CR - Menu
        public bool Reports_CR_View { get; set; }

        public bool Reports_Project_View { get; set; }

        // Random CR Verification
        public bool RCV
        {
            get
            {
                return (RCV_RandomSearch || RCV_Action || RCV_Search || RCV_Edit);
            }
        }

        public bool RCV_RandomSearch { get; set; }

        public bool RCV_Action { get; set; }

        public bool RCV_Search { get; set; }

        public bool RCV_Edit { get; set; }

        // RCV Missmatch Cases

        public bool RCV_Missmatch
        {
            get
            {
                return (RCVMM_Create || RCVMM_Search || RCVMM_Edit || RCVMM_View);
            }
        }

        public bool RCVMM_Create { get; set; }

        public bool RCVMM_Search { get; set; }

        public bool RCVMM_Edit { get; set; }

        public bool RCVMM_View { get; set; }

        // RCVMM Conversation
        public bool RCV_Conversation
        {
            get
            {
                return (RCV_Conversation_View || RCV_Conversation_Reply || RCV_Conversation_Close || RCV_Conversation_Escalation);
            }
        }

        public bool RCV_Conversation_View { get; set; }

        public bool RCV_Conversation_Reply { get; set; }

        public bool RCV_Conversation_Close { get; set; }

        public bool RCV_Conversation_Escalation { get; set; }

        public bool RCV_Reports
        {
            get
            {
                return (true);
            }
        }

        public Ent_UserRoles()
        {
            projectOwnerType_Create = false;
            projectOwnerType_Search = false;
            projectOwnerType_View = false;
            projectOwnerType_Edit = false;
            ProjectOwner_Create = false;
            ProjectOwner_Search = false;
            ProjectOwner_View = false;
            ProjectOwner_Edit = false;
            FactoryType_Create = false;
            FactoryType_Search = false;
            FactoryType_View = false;
            FactoryType_Edit = false;
            Factory_Create = false;
            Factory_Search = false;
            Factory_View = false;
            Factory_Edit = false;
            Department_Create = false;
            Department_Search = false;
            Department_View = false;
            Department_Edit = false;
            DepartmentSection_Create = false;
            DepartmentSection_Search = false;
            DepartmentSection_View = false;
            DepartmentSection_Edit = false;
            MixerType_Create = false;
            MixerType_Search = false;
            MixerType_View = false;
            MixerType_Edit = false;
            Mixer_Create = false;
            Mixer_Search = false;
            Mixer_View = false;
            Mixer_Edit = false;
            SampleTestCategory_Create = false;
            SampleTestCategory_Search = false;
            SampleTestCategory_View = false;
            SampleTestCategory_Edit = false;
            SampleTest_Create = false;
            SampleTest_Search = false;
            SampleTest_View = false;
            SampleTest_Edit = false;
            CRTYPEMC_Create = false;
            CRTYPEMC_Search = false;
            CRTYPEMC_View = false;
            CRTYPEMC_Edit = false;
            CRTypeGroup_Create = false;
            CRTypeGroup_Search = false;
            CRTypeGroup_View = false;
            CRTypeGroup_Edit = false;
            CRType_Create = false;
            CRType_Search = false;
            CRType_View = false;
            CRType_Edit = false;
            CheckListItem_Create = false;
            CheckListItem_Search = false;
            CheckListItem_View = false;
            CheckListItem_Edit = false;
            CheckListItemGroup_Create = false;
            CheckListItemGroup_Search = false;
            CheckListItemGroup_View = false;
            CheckListItemGroup_Edit = false;
            CheckLists_Create = false;
            CheckLists_Search = false;
            CheckLists_View = false;
            CheckLists_Edit = false;
            UserProfile_Create = false;
            UserProfile_Search = false;
            UserProfile_View = false;
            UserProfile_Edit = false;
            Project_Create = false;
            Project_Search = false;
            Project_View = false;
            Project_Edit = false;
            Complaint_Create = false;
            Complaint_Search = false;
            Complaint_View = false;
            Complaint_Edit = false;
            CLWF_Sequence_Create = false;
            CLWF_Sequence_Search = false;
            CLWF_Sequence_View = false;
            CLWF_Sequence_Edit = false;
            CLWF_Create = false;
            CLWF_Maker = false;
            CLWF_Checker = false;
            CR_Create = false;
            CR_Edit = false;
            CR_Action = false;
            CR_AddAttachments = false;
            CR_ViewAttachments = false;
            CR_View = false;
            CR_Search = false;
            Dashboard_SuperEng = false;
            Dashboard_ConsultantEng = false;
            Dashboard_AuthLab = false;
            Dashboard_Technician = false;
            Dashboard_CEO = false;

            Dashboard_QualityEng = false;
            Reports_CR_View = false;
            Reports_Project_View = false;
            Reports_CR_Search = false;
            Reports_CR_Projects_Search = false;
            Reports_CR_Samples_Search = false;
            Reports_CR_Samples_SearchDetailed = false;
            RCV_RandomSearch = false;
            RCV_Action = false;
            RCV_Search = false;
            RCV_Edit = false;
            RCVMM_Create = false;
            RCVMM_Search = false;
            RCVMM_Edit = false;
            RCVMM_View = false;
            RCV_Conversation_View = false;
            RCV_Conversation_Reply = false;
            RCV_Conversation_Close = false;
            RCV_Conversation_Escalation = false;
        }

        public Ent_UserRoles convertRolesListToUserRoles(IList<UserPermission> theApprovedPermissionLevelList)
        {
            try
            {
                foreach (UserPermission role in theApprovedPermissionLevelList)
                {
                    switch (role)
                    {
                        case UserPermission.projectOwnerType_Create: this.projectOwnerType_Create = true; break;
                        case UserPermission.projectOwnerType_Search: this.projectOwnerType_Search = true; break;
                        case UserPermission.projectOwnerType_View: this.projectOwnerType_View = true; break;
                        case UserPermission.projectOwnerType_Edit: this.projectOwnerType_Edit = true; break;

                        case UserPermission.ProjectOwner_Create: this.ProjectOwner_Create = true; break;
                        case UserPermission.ProjectOwner_Search: this.ProjectOwner_Search = true; break;
                        case UserPermission.ProjectOwner_View: this.ProjectOwner_View = true; break;
                        case UserPermission.ProjectOwner_Edit: this.ProjectOwner_Edit = true; break;

                        case UserPermission.FactoryType_Create: this.FactoryType_Create = true; break;
                        case UserPermission.FactoryType_Search: this.FactoryType_Search = true; break;
                        case UserPermission.FactoryType_View: this.FactoryType_View = true; break;
                        case UserPermission.FactoryType_Edit: this.FactoryType_Edit = true; break;

                        case UserPermission.Factory_Create: this.Factory_Create = true; break;
                        case UserPermission.Factory_Search: this.Factory_Search = true; break;
                        case UserPermission.Factory_View: this.Factory_View = true; break;
                        case UserPermission.Factory_Edit: this.Factory_Edit = true; break;

                        case UserPermission.Department_Create: this.Department_Create = true; break;
                        case UserPermission.Department_Search: this.Department_Search = true; break;
                        case UserPermission.Department_View: this.Department_View = true; break;
                        case UserPermission.Department_Edit: this.Department_Edit = true; break;

                        case UserPermission.DepartmentSection_Create: this.DepartmentSection_Create = true; break;
                        case UserPermission.DepartmentSection_Search: this.DepartmentSection_Search = true; break;
                        case UserPermission.DepartmentSection_View: this.DepartmentSection_View = true; break;
                        case UserPermission.DepartmentSection_Edit: this.DepartmentSection_Edit = true; break;

                        case UserPermission.MixerType_Create: this.MixerType_Create = true; break;
                        case UserPermission.MixerType_Search: this.MixerType_Search = true; break;
                        case UserPermission.MixerType_View: this.MixerType_View = true; break;
                        case UserPermission.MixerType_Edit: this.MixerType_Edit = true; break;

                        case UserPermission.Mixer_Create: this.Mixer_Create = true; break;
                        case UserPermission.Mixer_Search: this.Mixer_Search = true; break;
                        case UserPermission.Mixer_View: this.Mixer_View = true; break;
                        case UserPermission.Mixer_Edit: this.Mixer_Edit = true; break;

                        case UserPermission.SampleTestCategory_Create: this.SampleTestCategory_Create = true; break;
                        case UserPermission.SampleTestCategory_Search: this.SampleTestCategory_Search = true; break;
                        case UserPermission.SampleTestCategory_View: this.SampleTestCategory_View = true; break;
                        case UserPermission.SampleTestCategory_Edit: this.SampleTestCategory_Edit = true; break;

                        case UserPermission.SampleTest_Create: this.SampleTest_Create = true; break;
                        case UserPermission.SampleTest_Search: this.SampleTest_Search = true; break;
                        case UserPermission.SampleTest_View: this.SampleTest_View = true; break;
                        case UserPermission.SampleTest_Edit: this.SampleTest_Edit = true; break;

                        case UserPermission.CRTYPEMC_Create: this.CRTYPEMC_Create = true; break;
                        case UserPermission.CRTYPEMC_Search: this.CRTYPEMC_Search = true; break;
                        case UserPermission.CRTYPEMC_View: this.CRTYPEMC_View = true; break;
                        case UserPermission.CRTYPEMC_Edit: this.CRTYPEMC_Edit = true; break;

                        case UserPermission.CRTypeGroup_Create: this.CRTypeGroup_Create = true; break;
                        case UserPermission.CRTypeGroup_Search: this.CRTypeGroup_Search = true; break;
                        case UserPermission.CRTypeGroup_View: this.CRTypeGroup_View = true; break;
                        case UserPermission.CRTypeGroup_Edit: this.CRTypeGroup_Edit = true; break;

                        case UserPermission.CRType_Create: this.CRType_Create = true; break;
                        case UserPermission.CRType_Search: this.CRType_Search = true; break;
                        case UserPermission.CRType_View: this.CRType_View = true; break;
                        case UserPermission.CRType_Edit: this.CRType_Edit = true; break;

                        case UserPermission.CheckListItem_Create: this.CheckListItem_Create = true; break;
                        case UserPermission.CheckListItem_Search: this.CheckListItem_Search = true; break;
                        case UserPermission.CheckListItem_View: this.CheckListItem_View = true; break;
                        case UserPermission.CheckListItem_Edit: this.CheckListItem_Edit = true; break;

                        case UserPermission.CheckListItemGroup_Create: this.CheckListItemGroup_Create = true; break;
                        case UserPermission.CheckListItemGroup_Search: this.CheckListItemGroup_Search = true; break;
                        case UserPermission.CheckListItemGroup_View: this.CheckListItemGroup_View = true; break;
                        case UserPermission.CheckListItemGroup_Edit: this.CheckListItemGroup_Edit = true; break;

                        case UserPermission.CheckLists_Create: this.CheckLists_Create = true; break;
                        case UserPermission.CheckLists_Search: this.CheckLists_Search = true; break;
                        case UserPermission.CheckLists_View: this.CheckLists_View = true; break;
                        case UserPermission.CheckLists_Edit: this.CheckLists_Edit = true; break;

                        case UserPermission.UserProfile_Create: this.UserProfile_Create = true; break;
                        case UserPermission.UserProfile_Search: this.UserProfile_Search = true; break;
                        case UserPermission.UserProfile_View: this.UserProfile_View = true; break;
                        case UserPermission.UserProfile_Edit: this.UserProfile_Edit = true; break;

                        case UserPermission.UserRoles_Create: this.UserRoles_Create = true; break;
                        case UserPermission.UserRoles_Search: this.UserRoles_Search = true; break;
                        case UserPermission.UserRoles_View: this.UserRoles_View = true; break;
                        case UserPermission.UserRoles_Edit: this.UserRoles_Edit = true; break;

                        case UserPermission.Project_Create: this.Project_Create = true; break;
                        case UserPermission.Project_Search: this.Project_Search = true; break;
                        case UserPermission.Project_View: this.Project_View = true; break;
                        case UserPermission.Project_Edit: this.Project_Edit = true; break;

                        case UserPermission.Complaint_Create: this.Complaint_Create = true; break;
                        case UserPermission.Complaint_Search: this.Complaint_Search = true; break;
                        case UserPermission.Complaint_View: this.Complaint_View = true; break;
                        case UserPermission.Complaint_Edit: this.Complaint_Edit = true; break;

                        case UserPermission.CLWF_Sequence_Create: this.CLWF_Sequence_Create = true; break;
                        case UserPermission.CLWF_Sequence_Search: this.CLWF_Sequence_Search = true; break;
                        case UserPermission.CLWF_Sequence_View: this.CLWF_Sequence_View = true; break;
                        case UserPermission.CLWF_Sequence_Edit: this.CLWF_Sequence_Edit = true; break;

                        case UserPermission.CLWF_Create: this.CLWF_Create = true; break;
                        case UserPermission.CLWF_Maker: this.CLWF_Maker = true; break;
                        case UserPermission.CLWF_Checker: this.CLWF_Checker = true; break;

                        case UserPermission.CR_Create: this.CR_Create = true; break;
                        case UserPermission.CR_Edit: this.CR_Edit = true; break;
                        case UserPermission.CR_Action: this.CR_Action = true; break;
                        case UserPermission.CR_AddAttachments: this.CR_AddAttachments = true; break;
                        case UserPermission.CR_ViewAttachments: this.CR_ViewAttachments = true; break;
                        case UserPermission.CR_View: this.CR_View = true; break;
                        case UserPermission.CR_Search: this.CR_Search = true; break;

                        case UserPermission.Dashboard_SuperEng: this.Dashboard_SuperEng = true; break;
                        case UserPermission.Dashboard_ConsultantEng: this.Dashboard_ConsultantEng = true; break;
                        case UserPermission.Dashboard_AuthLab: this.Dashboard_AuthLab = true; break;
                        case UserPermission.Dashboard_Technician: this.Dashboard_Technician = true; break;
                        case UserPermission.Dashboard_CEO: this.Dashboard_CEO = true; break;
                        case UserPermission.Dashboard_QualityEng: this.Dashboard_QualityEng = true; break;

                        case UserPermission.Reports_CR_View: this.Reports_CR_View = true; break;
                        case UserPermission.Reports_Project_View: this.Reports_Project_View = true; break;

                        case UserPermission.Reports_CR_Search: this.Reports_CR_Search = true; break;
                        case UserPermission.Reports_CR_Projects_Search: this.Reports_CR_Projects_Search = true; break;
                        case UserPermission.Reports_CR_Samples_Search: this.Reports_CR_Samples_Search = true; break;
                        case UserPermission.Reports_CR_Samples_SearchDetailed: this.Reports_CR_Samples_SearchDetailed = true; break;

                        case UserPermission.RCV_RandomSearch: this.RCV_RandomSearch = true; break;
                        case UserPermission.RCV_Action: this.RCV_Action = true; break;
                        case UserPermission.RCV_Search: this.RCV_Search = true; break;
                        case UserPermission.RCV_Edit: this.RCV_Edit = true; break;

                        case UserPermission.RCVMM_Create: this.RCVMM_Create = true; break;
                        case UserPermission.RCVMM_Search: this.RCVMM_Search = true; break;
                        case UserPermission.RCVMM_Edit: this.RCVMM_Edit = true; break;
                        case UserPermission.RCVMM_View: this.RCVMM_View = true; break;

                        case UserPermission.RCV_Conversation_View: this.RCV_Conversation_View = true; break;
                        case UserPermission.RCV_Conversation_Reply: this.RCV_Conversation_Reply = true; break;
                        case UserPermission.RCV_Conversation_Close: this.RCV_Conversation_Close = true; break;
                        case UserPermission.RCV_Conversation_Escalation: this.RCV_Conversation_Escalation = true; break;
                    }
                }

                return this;
            }
            catch (Exception ex)
            {
                throw new HttpUnhandledException("User Privileges Error !, Kindly restart the Application");
            }
        }

        public IList<UserPermission> convertUserRolesToRolesList()
        {
            try
            {
                IList<UserPermission> userRoles = new List<UserPermission>();

                // projectOwnerType
                if (this.projectOwnerType_Create) userRoles.Add(UserPermission.projectOwnerType_Create);
                if (this.projectOwnerType_Search) userRoles.Add(UserPermission.projectOwnerType_Search);
                if (this.projectOwnerType_View) userRoles.Add(UserPermission.projectOwnerType_View);
                if (this.projectOwnerType_Edit) userRoles.Add(UserPermission.projectOwnerType_Edit);

                // ProjectOwner
                if (this.ProjectOwner_Create) userRoles.Add(UserPermission.ProjectOwner_Create);
                if (this.ProjectOwner_Search) userRoles.Add(UserPermission.ProjectOwner_Search);
                if (this.ProjectOwner_View) userRoles.Add(UserPermission.ProjectOwner_View);
                if (this.ProjectOwner_Edit) userRoles.Add(UserPermission.ProjectOwner_Edit);

                // FactoryType
                if (this.FactoryType_Create) userRoles.Add(UserPermission.FactoryType_Create);
                if (this.FactoryType_Search) userRoles.Add(UserPermission.FactoryType_Search);
                if (this.FactoryType_View) userRoles.Add(UserPermission.FactoryType_View);
                if (this.FactoryType_Edit) userRoles.Add(UserPermission.FactoryType_Edit);

                // Factory
                if (this.Factory_Create) userRoles.Add(UserPermission.Factory_Create);
                if (this.Factory_Search) userRoles.Add(UserPermission.Factory_Search);
                if (this.Factory_View) userRoles.Add(UserPermission.Factory_View);
                if (this.Factory_Edit) userRoles.Add(UserPermission.Factory_Edit);

                // Department
                if (this.Department_Create) userRoles.Add(UserPermission.Department_Create);
                if (this.Department_Search) userRoles.Add(UserPermission.Department_Search);
                if (this.Department_View) userRoles.Add(UserPermission.Department_View);
                if (this.Department_Edit) userRoles.Add(UserPermission.Department_Edit);

                // DepartmentSection
                if (this.DepartmentSection_Create) userRoles.Add(UserPermission.DepartmentSection_Create);
                if (this.DepartmentSection_Search) userRoles.Add(UserPermission.DepartmentSection_Search);
                if (this.DepartmentSection_View) userRoles.Add(UserPermission.DepartmentSection_View);
                if (this.DepartmentSection_Edit) userRoles.Add(UserPermission.DepartmentSection_Edit);

                // MixerType
                if (this.MixerType_Create) userRoles.Add(UserPermission.MixerType_Create);
                if (this.MixerType_Search) userRoles.Add(UserPermission.MixerType_Search);
                if (this.MixerType_View) userRoles.Add(UserPermission.MixerType_View);
                if (this.MixerType_Edit) userRoles.Add(UserPermission.MixerType_Edit);

                // Mixer
                if (this.Mixer_Create) userRoles.Add(UserPermission.Mixer_Create);
                if (this.Mixer_Search) userRoles.Add(UserPermission.Mixer_Search);
                if (this.Mixer_View) userRoles.Add(UserPermission.Mixer_View);
                if (this.Mixer_Edit) userRoles.Add(UserPermission.Mixer_Edit);

                // SampleTestCategory
                if (this.SampleTestCategory_Create) userRoles.Add(UserPermission.SampleTestCategory_Create);
                if (this.SampleTestCategory_Search) userRoles.Add(UserPermission.SampleTestCategory_Search);
                if (this.SampleTestCategory_View) userRoles.Add(UserPermission.SampleTestCategory_View);
                if (this.SampleTestCategory_Edit) userRoles.Add(UserPermission.SampleTestCategory_Edit);

                // SampleTest
                if (this.SampleTest_Create) userRoles.Add(UserPermission.SampleTest_Create);
                if (this.SampleTest_Search) userRoles.Add(UserPermission.SampleTest_Search);
                if (this.SampleTest_View) userRoles.Add(UserPermission.SampleTest_View);
                if (this.SampleTest_Edit) userRoles.Add(UserPermission.SampleTest_Edit);

                // CRTYPEMC
                if (this.CRTYPEMC_Create) userRoles.Add(UserPermission.CRTYPEMC_Create);
                if (this.CRTYPEMC_Search) userRoles.Add(UserPermission.CRTYPEMC_Search);
                if (this.CRTYPEMC_View) userRoles.Add(UserPermission.CRTYPEMC_View);
                if (this.CRTYPEMC_Edit) userRoles.Add(UserPermission.CRTYPEMC_Edit);

                // CRTypeGroup
                if (this.CRTypeGroup_Create) userRoles.Add(UserPermission.CRTypeGroup_Create);
                if (this.CRTypeGroup_Search) userRoles.Add(UserPermission.CRTypeGroup_Search);
                if (this.CRTypeGroup_View) userRoles.Add(UserPermission.CRTypeGroup_View);
                if (this.CRTypeGroup_Edit) userRoles.Add(UserPermission.CRTypeGroup_Edit);

                // CRType
                if (this.CRType_Create) userRoles.Add(UserPermission.CRType_Create);
                if (this.CRType_Search) userRoles.Add(UserPermission.CRType_Search);
                if (this.CRType_View) userRoles.Add(UserPermission.CRType_View);
                if (this.CRType_Edit) userRoles.Add(UserPermission.CRType_Edit);

                // CheckListItem
                if (this.CheckListItem_Create) userRoles.Add(UserPermission.CheckListItem_Create);
                if (this.CheckListItem_Search) userRoles.Add(UserPermission.CheckListItem_Search);
                if (this.CheckListItem_View) userRoles.Add(UserPermission.CheckListItem_View);
                if (this.CheckListItem_Edit) userRoles.Add(UserPermission.CheckListItem_Edit);

                // CheckListItemGroup
                if (this.CheckListItemGroup_Create) userRoles.Add(UserPermission.CheckListItemGroup_Create);
                if (this.CheckListItemGroup_Search) userRoles.Add(UserPermission.CheckListItemGroup_Search);
                if (this.CheckListItemGroup_View) userRoles.Add(UserPermission.CheckListItemGroup_View);
                if (this.CheckListItemGroup_Edit) userRoles.Add(UserPermission.CheckListItemGroup_Edit);

                // CheckLists
                if (this.CheckLists_Create) userRoles.Add(UserPermission.CheckLists_Create);
                if (this.CheckLists_Search) userRoles.Add(UserPermission.CheckLists_Search);
                if (this.CheckLists_View) userRoles.Add(UserPermission.CheckLists_View);
                if (this.CheckLists_Edit) userRoles.Add(UserPermission.CheckLists_Edit);

                // UserProfile
                if (this.UserProfile_Create) userRoles.Add(UserPermission.UserProfile_Create);
                if (this.UserProfile_Search) userRoles.Add(UserPermission.UserProfile_Search);
                if (this.UserProfile_View) userRoles.Add(UserPermission.UserProfile_View);
                if (this.UserProfile_Edit) userRoles.Add(UserPermission.UserProfile_Edit);

                // UserRoles
                if (this.UserRoles_Create) userRoles.Add(UserPermission.UserRoles_Create);
                if (this.UserRoles_Search) userRoles.Add(UserPermission.UserRoles_Search);
                if (this.UserRoles_View) userRoles.Add(UserPermission.UserRoles_View);
                if (this.UserRoles_Edit) userRoles.Add(UserPermission.UserRoles_Edit);

                // Project
                if (this.Project_Create) userRoles.Add(UserPermission.Project_Create);
                if (this.Project_Search) userRoles.Add(UserPermission.Project_Search);
                if (this.Project_View) userRoles.Add(UserPermission.Project_View);
                if (this.Project_Edit) userRoles.Add(UserPermission.Project_Edit);

                // Complaint
                if (this.Complaint_Create) userRoles.Add(UserPermission.Complaint_Create);
                if (this.Complaint_Search) userRoles.Add(UserPermission.Complaint_Search);
                if (this.Complaint_View) userRoles.Add(UserPermission.Complaint_View);
                if (this.Complaint_Edit) userRoles.Add(UserPermission.Complaint_Edit);

                // CLWF_Sequence
                if (this.CLWF_Sequence_Create) userRoles.Add(UserPermission.CLWF_Sequence_Create);
                if (this.CLWF_Sequence_Search) userRoles.Add(UserPermission.CLWF_Sequence_Search);
                if (this.CLWF_Sequence_View) userRoles.Add(UserPermission.CLWF_Sequence_View);
                if (this.CLWF_Sequence_Edit) userRoles.Add(UserPermission.CLWF_Sequence_Edit);

                // CLWF
                if (this.CLWF_Create) userRoles.Add(UserPermission.CLWF_Create);
                if (this.CLWF_Maker) userRoles.Add(UserPermission.CLWF_Maker);
                if (this.CLWF_Checker) userRoles.Add(UserPermission.CLWF_Checker);

                // CR
                if (this.CR_Create) userRoles.Add(UserPermission.CR_Create);
                if (this.CR_Edit) userRoles.Add(UserPermission.CR_Edit);
                if (this.CR_Action) userRoles.Add(UserPermission.CR_Action);
                if (this.CR_AddAttachments) userRoles.Add(UserPermission.CR_AddAttachments);
                if (this.CR_ViewAttachments) userRoles.Add(UserPermission.CR_ViewAttachments);
                if (this.CR_View) userRoles.Add(UserPermission.CR_View);
                if (this.CR_Search) userRoles.Add(UserPermission.CR_Search);

                // Dashboard
                if (this.Dashboard_SuperEng) userRoles.Add(UserPermission.Dashboard_SuperEng);
                if (this.Dashboard_ConsultantEng) userRoles.Add(UserPermission.Dashboard_ConsultantEng);
                if (this.Dashboard_AuthLab) userRoles.Add(UserPermission.Dashboard_AuthLab);
                if (this.Dashboard_Technician) userRoles.Add(UserPermission.Dashboard_Technician);
                if (this.Dashboard_CEO) userRoles.Add(UserPermission.Dashboard_CEO);
                if (this.Dashboard_QualityEng) userRoles.Add(UserPermission.Dashboard_QualityEng);

                // Reports
                if (this.Reports_CR_View) userRoles.Add(UserPermission.Reports_CR_View);
                if (this.Reports_Project_View) userRoles.Add(UserPermission.Reports_Project_View);

                if (this.Reports_CR_Search) userRoles.Add(UserPermission.Reports_CR_Search);
                if (this.Reports_CR_Projects_Search) userRoles.Add(UserPermission.Reports_CR_Projects_Search);
                if (this.Reports_CR_Samples_Search) userRoles.Add(UserPermission.Reports_CR_Samples_Search);
                if (this.Reports_CR_Samples_SearchDetailed) userRoles.Add(UserPermission.Reports_CR_Samples_SearchDetailed);

                // Random CR Verification
                if (this.RCV_RandomSearch) userRoles.Add(UserPermission.RCV_RandomSearch);
                if (this.RCV_Action) userRoles.Add(UserPermission.RCV_Action);
                if (this.RCV_Search) userRoles.Add(UserPermission.RCV_Search);
                if (this.RCV_Edit) userRoles.Add(UserPermission.RCV_Edit);

                // Random CR Verification Missmatch Cases
                if (this.RCVMM_Create) userRoles.Add(UserPermission.RCVMM_Create);
                if (this.RCVMM_Search) userRoles.Add(UserPermission.RCVMM_Search);
                if (this.RCVMM_Edit) userRoles.Add(UserPermission.RCVMM_Edit);
                if (this.RCVMM_View) userRoles.Add(UserPermission.RCVMM_View);

                // Random CR Verification Missmatch Cases Conversation Workflow
                if (this.RCV_Conversation_View) userRoles.Add(UserPermission.RCV_Conversation_View);
                if (this.RCV_Conversation_Reply) userRoles.Add(UserPermission.RCV_Conversation_Reply);
                if (this.RCV_Conversation_Close) userRoles.Add(UserPermission.RCV_Conversation_Close);
                if (this.RCV_Conversation_Escalation) userRoles.Add(UserPermission.RCV_Conversation_Escalation);

                return userRoles;
            }
            catch (Exception ex)
            {
                throw new HttpUnhandledException("User Privileges Error !, Kindly restart the Application");
            }
        }
    }
}