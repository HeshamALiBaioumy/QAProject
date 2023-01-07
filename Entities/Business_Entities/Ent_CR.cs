using QA.Entities.Session_Entities;
using System;
using System.ComponentModel.DataAnnotations;

namespace QA.Entities.Business_Entities
{
    public class Ent_CR
    {
        [DataType(DataType.Text)]
        [Display(Name = "CRID", ResourceType = typeof(Localization.CR))]
        public int CRID { get; set; }

        public int contractorID { get; set; }

        public string contractorName { get; set; }

        [Display(Name = "projectID", ResourceType = typeof(Localization.CR))]
        [Range(minimum: 0, maximum: int.MaxValue, ErrorMessageResourceType = typeof(Localization.CR)
            , ErrorMessageResourceName = "provide_valid_projectID")]
        public int projectID { get; set; }

        public string projectName { get; set; }

        [Display(Name = "projectItemID", ResourceType = typeof(Localization.CR))]
        [Range(minimum: 0, maximum: int.MaxValue, ErrorMessageResourceType = typeof(Localization.CR)
            , ErrorMessageResourceName = "provide_valid_projectItemID")]
        public int projectItemID { get; set; }

        public string projectItemName { get; set; }

        public int flowID { get; set; }

        [Display(Name = "CRTypeMCID", ResourceType = typeof(Localization.CR))]
        public int CRTypeMCID { get; set; }

        public string CRTypeMCName { get; set; }

        [Display(Name = "CRTypeGroupID", ResourceType = typeof(Localization.CR))]
        public int CRTypeGroupID { get; set; }

        public string CRTypeGroupName { get; set; }

        [Display(Name = "CRTypeID", ResourceType = typeof(Localization.CR))]
        [Range(minimum: 0, maximum: int.MaxValue, ErrorMessageResourceType = typeof(Localization.CR)
            , ErrorMessageResourceName = "provide_avalid_CRTypeID")]
        public int CRTypeID { get; set; }

        public string CRTypeName { get; set; }

        [Editable(false)]
        [DataType(DataType.Date)]
        [Display(Name = "registrationDate", ResourceType = typeof(Localization.CR))]
        public DateTime registrationDate { get; set; }

        public string strRegistrationDate
        {
            get
            {
                return (registrationDate == default(DateTime)) ? "" : registrationDate.ToString("dd/MM/yyyy");
            }
        }

        public Ent_CR_Sample sample { get; set; }

        [Editable(false)]
        [DataType(DataType.Text)]
        [Display(Name = "CRStatus", ResourceType = typeof(Localization.CR))]
        public int CRStatus { get; set; }

        public string CRStatusName { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "rejectReason", ResourceType = typeof(Localization.CR))]
        [RegularExpression(RegexEnum.validText, ErrorMessageResourceType = typeof(Localization.Global)
            , ErrorMessageResourceName = "Special_Characters_Not_Allowed")]
        [StringLength(maximumLength: 100, MinimumLength = 5, ErrorMessageResourceType = typeof(Localization.CR)
            , ErrorMessageResourceName = "RejectReason_length_validation")]
        public string rejectReason { get; set; }

        [Editable(false)]
        [DataType(DataType.Date)]
        //[DisplayFormat(DataFormatString = "{dd/MM/yyyy}")]
        [Display(Name = "closurenDate", ResourceType = typeof(Localization.CR))]
        public DateTime closurenDate { get; set; }

        public Ent_MapSelection mapSelection { get; set; }

        public bool isLabRequired { get; set; }

        public bool allowedforAttachments { get; set; }

        public bool allowedforClosure { get; set; }

        public bool allowedforComplaint { get; set; }

        public bool hasAttachments { get; set; }

        public bool allowedforRCV { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "makerID", ResourceType = typeof(Localization.Global))]
        public string makerID { get; set; }

        public override string ToString()
        {
            return String.Concat("CRID: ", CRID, "~ projectItemID: ", projectItemID, "~ CRTypeID: ", CRTypeID
                , "~ registrationDate: ", strRegistrationDate, "~ CRStatus: ", CRStatus
                , "~ closurenDate: "
                , (closurenDate == default(DateTime)) ? "" : closurenDate.ToString("dd/MM/yyyy")
                , "~ makerID: ", makerID);
        }
    }
}