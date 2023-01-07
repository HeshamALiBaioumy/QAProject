using QA.Entities.Session_Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace QA.Entities.Business_Entities
{
    public class Ent_Project
    {
        [DataType(DataType.Text)]
        public int ID { get; set; }

        /*     ---          Step 1                    ----    */
        [DataType(DataType.Text)]
        [Display(Name = "name", ResourceType = typeof(Localization.Project))]
        [Required(ErrorMessageResourceType = typeof(Localization.Project)
            , ErrorMessageResourceName = "Name_Required_Validation")]
        [RegularExpression(RegexEnum.validText, ErrorMessageResourceType = typeof(Localization.Global)
            , ErrorMessageResourceName = "Special_Characters_Not_Allowed")]
        [StringLength(maximumLength: 50, MinimumLength = 5, ErrorMessageResourceType = typeof(Localization.Project)
            , ErrorMessageResourceName = "Name_length_Validation")]
        [Remote("IsValidProject", "Project", HttpMethod = "POST", AdditionalFields = "ID"
            , ErrorMessageResourceType = typeof(Localization.Project), ErrorMessageResourceName = "NameAlreadyExist")]
        public string name { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "startDate", ResourceType = typeof(Localization.Project))]
        [Required(ErrorMessageResourceType = typeof(Localization.Project)
            , ErrorMessageResourceName = "provide_avalid_StartDate")]
        public DateTime startDate { get; set; }

        public string txtStartDate { get { return startDate.ToString("dd/MM/yyyy"); } }

        [DataType(DataType.Date)]
        [Display(Name = "endDate", ResourceType = typeof(Localization.Project))]
        [Required(ErrorMessageResourceType = typeof(Localization.Project)
            , ErrorMessageResourceName = "provide_avalid_EndDate")]
        public DateTime endDate { get; set; }

        public string txtEndDate { get { return endDate.ToString("dd/MM/yyyy"); } }

        [DataType(DataType.Date)]
        [Display(Name = "registerDate", ResourceType = typeof(Localization.Project))]
        [Required(ErrorMessageResourceType = typeof(Localization.Project)
            , ErrorMessageResourceName = "provide_valid_RegistrationDate")]
        public DateTime registerDate { get; set; }

        public string txtRegisterDate { get { return registerDate.ToString("dd/MM/yyyy"); } }

        [Display(Name = "boolIsActive", ResourceType = typeof(Localization.Global))]
        public bool isActive { get; set; }

        /*     ---          Step 2                    ----    */
        // must: isowner = true
        [Display(Name = "projectOwnerID", ResourceType = typeof(Localization.Project))]
        [Range(minimum: 0, maximum: int.MaxValue, ErrorMessageResourceType = typeof(Localization.Project)
            , ErrorMessageResourceName = "provide_avalid_projectOwner")]
        public int projectOwnerID { get; set; }

        public string projectOwnerName { get; set; }

        // Displayed as per the selected Project Owner
        [Display(Name = "supervisorEngID", ResourceType = typeof(Localization.Project))]
        [Range(minimum: 0, maximum: int.MaxValue, ErrorMessageResourceType = typeof(Localization.Project)
            , ErrorMessageResourceName = "provide_avalid_SupervisorEng")]
        public int supervisorEngID { get; set; }

        public string supervisorEngName { get; set; }

        // Displayed as per the selected Project Owner
        [Display(Name = "departmentID", ResourceType = typeof(Localization.Project))]
        [Range(minimum: 0, maximum: int.MaxValue, ErrorMessageResourceType = typeof(Localization.Project)
            , ErrorMessageResourceName = "provide_avalid_Department")]
        public int departmentID { get; set; }

        public string departmentName { get; set; }

        // Displayed as per the selected Project Owner and Department
        [Display(Name = "departmentSectionID", ResourceType = typeof(Localization.Project))]
        [Range(minimum: 0, maximum: int.MaxValue, ErrorMessageResourceType = typeof(Localization.Project)
            , ErrorMessageResourceName = "provide_avalid_DepartmentSection")]
        public int departmentSectionID { get; set; }

        public string departmentSectionName { get; set; }

        [Display(Name = "consultantID", ResourceType = typeof(Localization.Project))]
        //[Range(minimum: 0, maximum: int.MaxValue, ErrorMessageResourceType = typeof(Localization.Project)
        //    , ErrorMessageResourceName = "provide_avalid_Consultant")]
        public int consultantID { get; set; }

        public string consultantName { get; set; }

        // Displayed as per the selected Consultant
        [Display(Name = "consultantAssistantID", ResourceType = typeof(Localization.Project))]
        //[Range(minimum: 0, maximum: int.MaxValue, ErrorMessageResourceType = typeof(Localization.Project)
        //    , ErrorMessageResourceName = "provide_avalid_ConsultantAssistant")]
        public int consultantAssistantID { get; set; }

        public string consultantAssistantName { get; set; }

        [Display(Name = "contractorID", ResourceType = typeof(Localization.Project))]
        [Range(minimum: 0, maximum: int.MaxValue, ErrorMessageResourceType = typeof(Localization.Project)
            , ErrorMessageResourceName = "provide_avalid_Contractor")]
        public int contractorID { get; set; }

        public string contractorName { get; set; }

        // Displayed as per the selected Contractor
        [Display(Name = "contractorAssistantID", ResourceType = typeof(Localization.Project))]
        [Range(minimum: 0, maximum: int.MaxValue, ErrorMessageResourceType = typeof(Localization.Project)
            , ErrorMessageResourceName = "provide_avalid_ContractorAssistant")]
        public int contractorAssistantID { get; set; }

        public string contractorAssistantName { get; set; }

        [Display(Name = "authorizedLabID", ResourceType = typeof(Localization.Project))]
        [Range(minimum: 0, maximum: int.MaxValue, ErrorMessageResourceType = typeof(Localization.Project)
            , ErrorMessageResourceName = "provide_avalid_AuthorizedLab")]
        public int authorizedLabID { get; set; }

        public string authorizedLabName { get; set; }

        [Display(Name = "QATechnicianID", ResourceType = typeof(Localization.Project))]
        [Range(minimum: 0, maximum: int.MaxValue, ErrorMessageResourceType = typeof(Localization.Project)
            , ErrorMessageResourceName = "provide_avalid_QAtechnician")]
        public int QATechnicianID { get; set; }

        public string QATechnicianName { get; set; }

        [Display(Name = "QualityAssuranceEngID", ResourceType = typeof(Localization.Project))]
        [Range(minimum: 0, maximum: int.MaxValue, ErrorMessageResourceType = typeof(Localization.Project)
            , ErrorMessageResourceName = "provide_avalid_QualityEng")]
        public int QualityAssuranceEngID { get; set; }

        public string QualityAssuranceEngName { get; set; }

        /*     ---          Step 3 Milestones                   ----    */
        public List<Ent_ProjectItem> projectMileStones { get; set; }

        /*     ---          Step 4 Maps                   ----    */
        public Ent_MapSelection mapSelection { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "makerID", ResourceType = typeof(Localization.Global))]
        public string makerID { get; set; }

        public override string ToString()
        {
            return String.Concat("ID: ", ID, "~ name: ", name, "~ projectOwnerID: ", projectOwnerID
                , "~ supervisorEngID: ", supervisorEngID, "~ departmentID: ", departmentID, "~ departmentSectionID: ", departmentSectionID
                , "~ consultantID: ", consultantID, "~ consultantAssistantID: ", consultantAssistantID
                , "~ contractorID: ", contractorID, "~ contractorAssistantID: ", contractorAssistantID
                , "~ authorizedLabID: ", authorizedLabID, "~ QATechnicianID: ", QATechnicianID, "~ startDate: ", startDate.ToString("dd-MM-YYYY")
                , "~ endDate: ", endDate.ToString("dd-MM-YYYY"), "~ mapSelection: ", (mapSelection == null) ? "" : mapSelection.ToString()
                , "~ registerDate: ", registerDate.ToString("dd-MM-YYYY"), "~ isActive: ", isActive, "~ makerID: ", makerID);
        }
    }
}