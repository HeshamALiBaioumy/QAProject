using QA.Entities.Session_Entities;
using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace QA.Entities.Business_Entities
{
    public class Ent_CheckListFlow_Sequence
    {
        public int ID { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "name", ResourceType = typeof(Localization.CheckListFlow_Sequence))]
        [Required(ErrorMessageResourceType = typeof(Localization.CheckListFlow_Sequence)
            , ErrorMessageResourceName = "avalid_Name")]
        [RegularExpression(RegexEnum.validText, ErrorMessageResourceType = typeof(Localization.Global)
            , ErrorMessageResourceName = "Special_Characters_Not_Allowed")]
        [StringLength(maximumLength: 100, MinimumLength = 5, ErrorMessageResourceType = typeof(Localization.CheckListFlow_Sequence)
            , ErrorMessageResourceName = "Name_length_validation")]
        [Remote("IsValidSequenceName", "CheckListsWF_Sequence", HttpMethod = "POST", AdditionalFields = "ID"
            , ErrorMessageResourceType = typeof(Localization.CheckListFlow_Sequence), ErrorMessageResourceName = "NameAlreadyExist")]
        public string name { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "description", ResourceType = typeof(Localization.CheckListFlow_Sequence))]
        [Required(ErrorMessageResourceType = typeof(Localization.CheckListFlow_Sequence)
            , ErrorMessageResourceName = "avalid_Description")]
        [RegularExpression(RegexEnum.validText, ErrorMessageResourceType = typeof(Localization.Global)
            , ErrorMessageResourceName = "Special_Characters_Not_Allowed")]
        [StringLength(maximumLength: 200, MinimumLength = 5, ErrorMessageResourceType = typeof(Localization.CheckListFlow_Sequence)
            , ErrorMessageResourceName = "Description_length_validation")]
        public string description { get; set; }

        [Display(Name = "technicianID", ResourceType = typeof(Localization.CheckListFlow_Sequence))]
        public int technicianID { get; set; }

        public string technicianName { get; set; }

        [Display(Name = "technician_maxDays", ResourceType = typeof(Localization.CheckListFlow_Sequence))]
        [Range(minimum: 0, maximum: 10, ErrorMessageResourceType = typeof(Localization.CheckListFlow_Sequence)
            , ErrorMessageResourceName = "MaxPeriod_Range")]
        [DataType(DataType.Text)]
        public int technician_maxDays { get; set; }

        [Display(Name = "supervisorEngID", ResourceType = typeof(Localization.CheckListFlow_Sequence))]
        public int supervisorEngID { get; set; }

        public string supervisorEngName { get; set; }

        [Display(Name = "superEng_maxDays", ResourceType = typeof(Localization.CheckListFlow_Sequence))]
        [Range(minimum: 0, maximum: 10, ErrorMessageResourceType = typeof(Localization.CheckListFlow_Sequence)
            , ErrorMessageResourceName = "MaxPeriod_Range")]
        [DataType(DataType.Text)]
        public int superEng_maxDays { get; set; }

        [Display(Name = "qALabID", ResourceType = typeof(Localization.CheckListFlow_Sequence))]
        public int qALabID { get; set; }

        public string qALabName { get; set; }

        [Display(Name = "qALab_maxDays", ResourceType = typeof(Localization.CheckListFlow_Sequence))]
        [Range(minimum: 0, maximum: 10, ErrorMessageResourceType = typeof(Localization.CheckListFlow_Sequence)
            , ErrorMessageResourceName = "MaxPeriod_Range")]
        [DataType(DataType.Text)]
        public int qALab_maxDays { get; set; }

        [Display(Name = "representitiveSuperID", ResourceType = typeof(Localization.CheckListFlow_Sequence))]
        public int representitiveSuperID { get; set; }

        public string representitiveSuperName { get; set; }

        [Display(Name = "repSuper_maxDays", ResourceType = typeof(Localization.CheckListFlow_Sequence))]
        [Range(minimum: 0, maximum: 10, ErrorMessageResourceType = typeof(Localization.CheckListFlow_Sequence)
            , ErrorMessageResourceName = "MaxPeriod_Range")]
        [DataType(DataType.Text)]
        public int repSuper_maxDays { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "makerID", ResourceType = typeof(Localization.Global))]
        public string makerID { get; set; }

        [Display(Name = "boolIsActive", ResourceType = typeof(Localization.Global))]
        public bool isActive { get; set; }

        public override string ToString()
        {
            return String.Concat("Sequence ID: ", ID, "~ name: ", name, "~ description: ", description
                , "~ technicianID: ", technicianID, "~ technicianName: ", technicianName, "~ technician_maxDays: ", technician_maxDays
                , "~ supervisorEngID: ", supervisorEngID, "~ supervisorEngName: ", supervisorEngName
                , "~ superEng_maxDays: ", superEng_maxDays, "~ qALabID: ", qALabID, "~ qALabName: ", qALabName
                , "~ qALab_maxDays: ", qALab_maxDays, "~ representitiveSuperID: ", representitiveSuperID
                , "~ representitiveSuperName: ", representitiveSuperName, "~ repSuper_maxDays: ", repSuper_maxDays
                , "~ isActive: ", isActive, "~ makerID: ", makerID);
        }
    }
}