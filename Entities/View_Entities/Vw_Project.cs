using QA.Entities.Session_Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace QA.Entities.View_Entities
{
    public class Vw_Project
    {
        /*               View Details                        */
        public Business_Entities.Ent_Project project { get; set; }

        public Session_Entities.Ent_UserSession userSession { get; set; }

        /*               List Of Values                       */
        public List<LOV> lOVProjectOwners { get; set; }

        public List<LOV> lOVSupervisorEngineers { get; set; }

        public List<LOV> lOVDepartments { get; set; }

        public List<LOV> lOVSections { get; set; }

        public List<LOV> lOVConsultants { get; set; }

        public List<LOV> lOVConsultantAssistant { get; set; }

        public List<LOV> lOVContractors { get; set; }

        public List<LOV> lOVContractorAssistant { get; set; }

        public List<LOV> lOVAuthorizedLabs { get; set; }

        public List<LOV> lOVQATechnicians { get; set; }

        public List<LOV> lOVQualityAssuranceEngineers { get; set; }

        public List<LOV> lOVMilestoneAmtUnits { get; set; }

        /*               Search Details                        */
        [DataType(DataType.Text)]
        [Display(Name = "name_Search", ResourceType = typeof(Localization.Project))]
        public string searchName { get; set; }

        [Display(Name = "projectOwnerID_Search", ResourceType = typeof(Localization.Project))]
        public int searchProjectOwnerID { get; set; }

        [Display(Name = "supervisorEngID_Search", ResourceType = typeof(Localization.Project))]
        public int searchSupervisorEngID { get; set; }

        [Display(Name = "departmentID_Search", ResourceType = typeof(Localization.Project))]
        public int searchDepartmentID { get; set; }

        [Display(Name = "departmentSectionID_Search", ResourceType = typeof(Localization.Project))]
        public int searchDepartmentSectionID { get; set; }

        [Display(Name = "consultantID_Search", ResourceType = typeof(Localization.Project))]
        public int searchConsultantID { get; set; }

        [Display(Name = "consultantAssistantID_Search", ResourceType = typeof(Localization.Project))]
        public int searchConsultantAssistantID { get; set; }

        [Display(Name = "contractorID_Search", ResourceType = typeof(Localization.Project))]
        public int searchContractorID { get; set; }

        [Display(Name = "contractorAssistantID_Search", ResourceType = typeof(Localization.Project))]
        public int searchContractorAssistantID { get; set; }

        [Display(Name = "authorizedLabID_Search", ResourceType = typeof(Localization.Project))]
        public int searchAuthorizedLabID { get; set; }

        [Display(Name = "QATechnicianID_Search", ResourceType = typeof(Localization.Project))]
        public int searchQATechnicianID { get; set; }

        [Display(Name = "QualityEngineerID_Search", ResourceType = typeof(Localization.Project))]
        public int searchQualityEngineerID { get; set; }

        [Display(Name = "boolIsActive", ResourceType = typeof(Localization.Global))]
        public int searchIsActive { get; set; }
    }
}