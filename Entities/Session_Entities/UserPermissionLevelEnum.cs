using QA.Entities.Business_Entities;
using System.ComponentModel.DataAnnotations;

namespace QA.Entities.Session_Entities
{
    public enum UserPermission
    {
        /* 0x164
         Steps to Add a new Screen
            1) Add the new Screen in the below Enum
            2) Add the new screen to "Ent_UserRoles" Class
            3) Add the new screen to "Ent_UserRoles" method "Constructor"
            3) Add the new screen to "Ent_UserRoles" method "convertRolesListToUserRoles"
            4) Add the new screen to "Ent_UserRoles" method "convertUserRolesToRolesList"

            5) Add the new screen to "Login" Controller "Model"
            6) add the new screen to the view Screens
            7) add the new screen to the Model Class
            8) add the new screen to the Database tables
         */

        // projectOwnerType
        [Display(GroupName = "projectOwnerType", Name = "Create")] projectOwnerType_Create = 0x01,
        [Display(GroupName = "projectOwnerType", Name = "Search")] projectOwnerType_Search = 0x02,
        [Display(GroupName = "projectOwnerType", Name = "View")] projectOwnerType_View = 0x03,
        [Display(GroupName = "projectOwnerType", Name = "Edit")] projectOwnerType_Edit = 0x04,

        // ProjectOwner
        [Display(GroupName = "ProjectOwner", Name = "Create")] ProjectOwner_Create = 0x11,
        [Display(GroupName = "ProjectOwner", Name = "Search")] ProjectOwner_Search = 0x12,
        [Display(GroupName = "ProjectOwner", Name = "View")] ProjectOwner_View = 0x13,
        [Display(GroupName = "ProjectOwner", Name = "Edit")] ProjectOwner_Edit = 0x14,

        // FactoryType
        [Display(GroupName = "FactoryType", Name = "Create")] FactoryType_Create = 0x21,
        [Display(GroupName = "FactoryType", Name = "Search")] FactoryType_Search = 0x22,
        [Display(GroupName = "FactoryType", Name = "View")] FactoryType_View = 0x23,
        [Display(GroupName = "FactoryType", Name = "Edit")] FactoryType_Edit = 0x24,

        // Factory
        [Display(GroupName = "Factory", Name = "Create")] Factory_Create = 0x31,
        [Display(GroupName = "Factory", Name = "Search")] Factory_Search = 0x32,
        [Display(GroupName = "Factory", Name = "View")] Factory_View = 0x33,
        [Display(GroupName = "Factory", Name = "Edit")] Factory_Edit = 0x34,

        // Department
        [Display(GroupName = "Department", Name = "Create")] Department_Create = 0x41,
        [Display(GroupName = "Department", Name = "Search")] Department_Search = 0x42,
        [Display(GroupName = "Department", Name = "View")] Department_View = 0x43,
        [Display(GroupName = "Department", Name = "Edit")] Department_Edit = 0x44,

        // DepartmentSection
        [Display(GroupName = "DepartmentSection", Name = "Create")] DepartmentSection_Create = 0x51,
        [Display(GroupName = "DepartmentSection", Name = "Search")] DepartmentSection_Search = 0x52,
        [Display(GroupName = "DepartmentSection", Name = "View")] DepartmentSection_View = 0x53,
        [Display(GroupName = "DepartmentSection", Name = "Edit")] DepartmentSection_Edit = 0x54,

        // MixerType
        [Display(GroupName = "MixerType", Name = "Create")] MixerType_Create = 0x61,
        [Display(GroupName = "MixerType", Name = "Search")] MixerType_Search = 0x62,
        [Display(GroupName = "MixerType", Name = "View")] MixerType_View = 0x63,
        [Display(GroupName = "MixerType", Name = "Edit")] MixerType_Edit = 0x64,

        // Mixer
        [Display(GroupName = "Mixer", Name = "Create")] Mixer_Create = 0x71,
        [Display(GroupName = "Mixer", Name = "Search")] Mixer_Search = 0x72,
        [Display(GroupName = "Mixer", Name = "View")] Mixer_View = 0x73,
        [Display(GroupName = "Mixer", Name = "Edit")] Mixer_Edit = 0x74,

        // SampleTestCategory
        [Display(GroupName = "SampleTestCategory", Name = "Create")] SampleTestCategory_Create = 0x81,
        [Display(GroupName = "SampleTestCategory", Name = "Search")] SampleTestCategory_Search = 0x82,
        [Display(GroupName = "SampleTestCategory", Name = "View")] SampleTestCategory_View = 0x83,
        [Display(GroupName = "SampleTestCategory", Name = "Edit")] SampleTestCategory_Edit = 0x84,

        // SampleTest
        [Display(GroupName = "SampleTest", Name = "Create")] SampleTest_Create = 0x91,
        [Display(GroupName = "SampleTest", Name = "Search")] SampleTest_Search = 0x92,
        [Display(GroupName = "SampleTest", Name = "View")] SampleTest_View = 0x93,
        [Display(GroupName = "SampleTest", Name = "Edit")] SampleTest_Edit = 0x94,

        // Sample
        [Display(GroupName = "Sample", Name = "Create")] Sample_Create = 0x15,
        [Display(GroupName = "Sample", Name = "Search")] Sample_Search = 0x16,
        [Display(GroupName = "Sample", Name = "View")] Sample_View = 0x17,
        [Display(GroupName = "Sample", Name = "Edit")] Sample_Edit = 0x18,

        // CRTYPEMC
        [Display(GroupName = "CRTYPEMC", Name = "Create")] CRTYPEMC_Create = 0x25,
        [Display(GroupName = "CRTYPEMC", Name = "Search")] CRTYPEMC_Search = 0x26,
        [Display(GroupName = "CRTYPEMC", Name = "View")] CRTYPEMC_View = 0x27,
        [Display(GroupName = "CRTYPEMC", Name = "Edit")] CRTYPEMC_Edit = 0x28,

        // CRTypeGroup
        [Display(GroupName = "CRTypeGroup", Name = "Create")] CRTypeGroup_Create = 0x35,
        [Display(GroupName = "CRTypeGroup", Name = "Search")] CRTypeGroup_Search = 0x36,
        [Display(GroupName = "CRTypeGroup", Name = "View")] CRTypeGroup_View = 0x37,
        [Display(GroupName = "CRTypeGroup", Name = "Edit")] CRTypeGroup_Edit = 0x38,

        // CRType
        [Display(GroupName = "CRType", Name = "Create")] CRType_Create = 0x45,
        [Display(GroupName = "CRType", Name = "Search")] CRType_Search = 0x46,
        [Display(GroupName = "CRType", Name = "View")] CRType_View = 0x47,
        [Display(GroupName = "CRType", Name = "Edit")] CRType_Edit = 0x48,

        // CheckListItem
        [Display(GroupName = "CheckListItem", Name = "Create")] CheckListItem_Create = 0x55,
        [Display(GroupName = "CheckListItem", Name = "Search")] CheckListItem_Search = 0x56,
        [Display(GroupName = "CheckListItem", Name = "View")] CheckListItem_View = 0x57,
        [Display(GroupName = "CheckListItem", Name = "Edit")] CheckListItem_Edit = 0x58,

        // CheckListItemGroup
        [Display(GroupName = "CheckListItemGroup", Name = "Create")] CheckListItemGroup_Create = 0x65,
        [Display(GroupName = "CheckListItemGroup", Name = "Search")] CheckListItemGroup_Search = 0x66,
        [Display(GroupName = "CheckListItemGroup", Name = "View")] CheckListItemGroup_View = 0x67,
        [Display(GroupName = "CheckListItemGroup", Name = "Edit")] CheckListItemGroup_Edit = 0x68,

        // CheckLists
        [Display(GroupName = "CheckLists", Name = "Create")] CheckLists_Create = 0x75,
        [Display(GroupName = "CheckLists", Name = "Search")] CheckLists_Search = 0x76,
        [Display(GroupName = "CheckLists", Name = "View")] CheckLists_View = 0x77,
        [Display(GroupName = "CheckLists", Name = "Edit")] CheckLists_Edit = 0x78,

        // UserProfile
        [Display(GroupName = "UserProfile", Name = "Create")] UserProfile_Create = 0x85,
        [Display(GroupName = "UserProfile", Name = "Search")] UserProfile_Search = 0x86,
        [Display(GroupName = "UserProfile", Name = "View")] UserProfile_View = 0x87,
        [Display(GroupName = "UserProfile", Name = "Edit")] UserProfile_Edit = 0x88,

        // UserRoles
        [Display(GroupName = "UserRoles", Name = "Create")] UserRoles_Create = 0x151,
        [Display(GroupName = "UserRoles", Name = "Search")] UserRoles_Search = 0x152,
        [Display(GroupName = "UserRoles", Name = "View")] UserRoles_View = 0x153,
        [Display(GroupName = "UserRoles", Name = "Edit")] UserRoles_Edit = 0x154,

        // Project
        [Display(GroupName = "Project", Name = "Create")] Project_Create = 0x95,
        [Display(GroupName = "Project", Name = "Search")] Project_Search = 0x96,
        [Display(GroupName = "Project", Name = "View")] Project_View = 0x97,
        [Display(GroupName = "Project", Name = "Edit")] Project_Edit = 0x98,

        // Complaint
        [Display(GroupName = "Complaint", Name = "Create")] Complaint_Create = 0x101,
        [Display(GroupName = "Complaint", Name = "Search")] Complaint_Search = 0x102,
        [Display(GroupName = "Complaint", Name = "View")] Complaint_View = 0x103,
        [Display(GroupName = "Complaint", Name = "Edit")] Complaint_Edit = 0x104,

        // CLWF_Sequence
        [Display(GroupName = "CLWF_Sequence", Name = "Create")] CLWF_Sequence_Create = 0x161,
        [Display(GroupName = "CLWF_Sequence", Name = "Search")] CLWF_Sequence_Search = 0x162,
        [Display(GroupName = "CLWF_Sequence", Name = "View")] CLWF_Sequence_View = 0x163,
        [Display(GroupName = "CLWF_Sequence", Name = "Edit")] CLWF_Sequence_Edit = 0x164,

        // CLWF
        [Display(GroupName = "CLWF", Name = "Create")] CLWF_Create = 0x111,
        [Display(GroupName = "CLWF", Name = "Maker")] CLWF_Maker = 0x112,
        [Display(GroupName = "CLWF", Name = "Checker")] CLWF_Checker = 0x113,

        // CR
        [Display(GroupName = "CR", Name = "Create")] CR_Create = 0x121,
        [Display(GroupName = "CR", Name = "Search")] CR_Search = 0x122,
        [Display(GroupName = "CR", Name = "View")] CR_View = 0x123,
        [Display(GroupName = "CR", Name = "Edit")] CR_Edit = 0x124,
        [Display(GroupName = "CR", Name = "Action")] CR_Action = 0x125,
        [Display(GroupName = "CR", Name = "AddAttachments")] CR_AddAttachments = 0x126,
        [Display(GroupName = "CR", Name = "ViewAttachments")] CR_ViewAttachments = 0x127,

        // Dashboard
        [Display(GroupName = "Dashboard", Name = "SuperEng")] Dashboard_SuperEng = 0x131,
        [Display(GroupName = "Dashboard", Name = "ConsultantEng")] Dashboard_ConsultantEng = 0x132,
        [Display(GroupName = "Dashboard", Name = "AuthLab")] Dashboard_AuthLab = 0x133,
        [Display(GroupName = "Dashboard", Name = "Technician")] Dashboard_Technician = 0x134,
        [Display(GroupName = "Dashboard", Name = "CEO")] Dashboard_CEO = 0x135,
        [Display(GroupName = "Dashboard", Name = "QualityEng")] Dashboard_QualityEng = 0x301,

        // Reports
        [Display(GroupName = "Reports", Name = "CR_View")] Reports_CR_View = 0x171,
        [Display(GroupName = "Reports", Name = "CR_Search")] Reports_CR_Search = 0x173,
        [Display(GroupName = "Reports", Name = "CR_Projects_Search")] Reports_CR_Projects_Search = 0x174,
        [Display(GroupName = "Reports", Name = "CR_Samples_Search")] Reports_CR_Samples_Search = 0x175,
        [Display(GroupName = "Reports", Name = "CR_Samples_SearchDetailed")] Reports_CR_Samples_SearchDetailed = 0x176,

        [Display(GroupName = "Reports", Name = "Project_View")] Reports_Project_View = 0x172,

        // Random CR Verification
        [Display(GroupName = "RCV", Name = "RandomSearch")] RCV_RandomSearch = 0x201,
        [Display(GroupName = "RCV", Name = "Action")] RCV_Action = 0x202,
        [Display(GroupName = "RCV", Name = "Search")] RCV_Search = 0x203,
        [Display(GroupName = "RCV", Name = "Edit")] RCV_Edit = 0x204,

        // RCV Missmatch Cases
        [Display(GroupName = "RCVMM", Name = "Create")] RCVMM_Create = 0x205,
        [Display(GroupName = "RCVMM", Name = "Search")] RCVMM_Search = 0x206,
        [Display(GroupName = "RCVMM", Name = "Edit")] RCVMM_Edit = 0x207,
        [Display(GroupName = "RCVMM", Name = "View")] RCVMM_View = 0x208,
        
        // RCVMM Conversation
        [Display(GroupName = "RCV", Name = "Conversation_View")] RCV_Conversation_View = 0x209,
        [Display(GroupName = "RCV", Name = "Conversation_Reply")] RCV_Conversation_Reply = 0x210,
        [Display(GroupName = "RCV", Name = "Conversation_Close")] RCV_Conversation_Close = 0x211,
        [Display(GroupName = "RCV", Name = "Conversation_Escalation")] RCV_Conversation_Escalation = 0x212,
    }
}