using QA.Entities.Session_Entities;
using System;
using System.ComponentModel.DataAnnotations;

namespace QA.Entities.Business_Entities
{
    public class Ent_CheckListFlow_Master
    {
        public int ID { get; set; }

        [Display(Name = "cLID", ResourceType = typeof(Localization.CheckListFlow_Master))]
        [Range(minimum: 0, maximum: int.MaxValue, ErrorMessageResourceType = typeof(Localization.CheckListFlow_Master)
            , ErrorMessageResourceName = "provide_avalid_CLID")]
        public int cLID { get; set; }

        public string cLName { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "CLParty", ResourceType = typeof(Localization.CheckListFlow_Master))]
        [Required(ErrorMessageResourceType = typeof(Localization.CheckListFlow_Master)
            , ErrorMessageResourceName = "avalid_Party")]
        [RegularExpression(RegexEnum.validText, ErrorMessageResourceType = typeof(Localization.Global)
            , ErrorMessageResourceName = "Special_Characters_Not_Allowed")]
        [StringLength(maximumLength: 100, MinimumLength = 5, ErrorMessageResourceType = typeof(Localization.CheckListFlow_Master)
            , ErrorMessageResourceName = "Party_length_validation")]
        public string CLParty { get; set; }

        [Display(Name = "cLSequenceID", ResourceType = typeof(Localization.CheckListFlow_Master))]
        [Range(minimum: 0, maximum: int.MaxValue, ErrorMessageResourceType = typeof(Localization.CheckListFlow_Master)
            , ErrorMessageResourceName = "provide_avalid_cLSequence")]
        public int cLSequenceID { get; set; }

        public string cLSequenceName { get; set; }

        [Display(Name = "technicianID", ResourceType = typeof(Localization.CheckListFlow_Master))]
        public int technicianID { get; set; }

        public string technicianName { get; set; }

        [Display(Name = "technician_maxDays", ResourceType = typeof(Localization.CheckListFlow_Master))]
        [Range(minimum: 0, maximum: 10, ErrorMessageResourceType = typeof(Localization.CheckListFlow_Master)
            , ErrorMessageResourceName = "MaxPeriod_Range")]
        [DataType(DataType.Text)]
        public int technician_maxDays { get; set; }

        [Display(Name = "supervisorEngID", ResourceType = typeof(Localization.CheckListFlow_Master))]
        public int supervisorEngID { get; set; }

        public string supervisorEngName { get; set; }

        [Display(Name = "superEng_maxDays", ResourceType = typeof(Localization.CheckListFlow_Master))]
        [Range(minimum: 0, maximum: 10, ErrorMessageResourceType = typeof(Localization.CheckListFlow_Master)
            , ErrorMessageResourceName = "MaxPeriod_Range")]
        [DataType(DataType.Text)]
        public int superEng_maxDays { get; set; }

        [Display(Name = "qALabID", ResourceType = typeof(Localization.CheckListFlow_Master))]
        public int qALabID { get; set; }

        public string qALabName { get; set; }

        [Display(Name = "qALab_maxDays", ResourceType = typeof(Localization.CheckListFlow_Master))]
        [Range(minimum: 0, maximum: 10, ErrorMessageResourceType = typeof(Localization.CheckListFlow_Master)
            , ErrorMessageResourceName = "MaxPeriod_Range")]
        [DataType(DataType.Text)]
        public int qALab_maxDays { get; set; }

        [Display(Name = "representitiveSuperID", ResourceType = typeof(Localization.CheckListFlow_Master))]
        public int representitiveSuperID { get; set; }

        public string representitiveSuperName { get; set; }

        [Display(Name = "repSuper_maxDays", ResourceType = typeof(Localization.CheckListFlow_Master))]
        [Range(minimum: 0, maximum: 10, ErrorMessageResourceType = typeof(Localization.CheckListFlow_Master)
            , ErrorMessageResourceName = "MaxPeriod_Range")]
        [DataType(DataType.Text)]
        public int repSuper_maxDays { get; set; }

        [Editable(false)]
        [DataType(DataType.Date)]
        [Display(Name = "registrationDate", ResourceType = typeof(Localization.CheckListFlow_Master))]
        public DateTime registrationDate { get; set; }

        public string strRegistrationDate
        {
            get
            {
                return registrationDate.ToString("dd/MM/yyyy");
            }
        }

        [Editable(false)]
        [DataType(DataType.Text)]
        [Display(Name = "CLFlowStatus", ResourceType = typeof(Localization.CheckListFlow_Master))]
        public int CLFlowStatus { get; set; }

        public string CLFlowStatusName { get; set; }

        [Editable(false)]
        [DataType(DataType.Date)]
        [Display(Name = "closurenDate", ResourceType = typeof(Localization.CheckListFlow_Master))]
        public DateTime closurenDate { get; set; }

        public string strClosurenDate
        {
            get
            {
                return (closurenDate == default(DateTime)) ? "" : closurenDate.ToString("dd/MM/yyyy");
            }
        }

        public bool allowedforMaker { get; set; }

        public bool allowedforChecker { get; set; }

        public bool allowedforEdit { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "makerID", ResourceType = typeof(Localization.Global))]
        public string makerID { get; set; }

        public override string ToString()
        {
            return String.Concat("cLID: ", cLID, "~ cLName: ", cLName, "~ cLSequenceID: ", cLSequenceID
                , "~ cLSequenceName: ", cLSequenceName, "~ technicianID: ", technicianID
                , "~ technicianName: ", technicianName, "~ technician_maxDays: ", technician_maxDays
                , "~ supervisorEngID: ", supervisorEngID, "~ supervisorEngName: ", supervisorEngName
                , "~ superEng_maxDays: ", superEng_maxDays, "~ qALabID: ", qALabID, "~ qALabName: ", qALabName
                , "~ qALab_maxDays: ", qALab_maxDays, "~ representitiveSuperID: ", representitiveSuperID
                , "~ representitiveSuperName: ", representitiveSuperName, "~ repSuper_maxDays: ", repSuper_maxDays
                , "~ registrationDate: ", registrationDate, "~ CLFlowStatus: ", CLFlowStatus
                , "~ CLFlowStatusName: ", CLFlowStatusName, "~ closurenDate: ", closurenDate
                , "~ allowedforEdit: ", allowedforEdit, "~ makerID: ", makerID);
        }
    }
}