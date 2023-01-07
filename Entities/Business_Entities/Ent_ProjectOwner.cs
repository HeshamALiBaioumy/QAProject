using QA.Entities.Session_Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QA.Entities.Business_Entities
{
    public class Ent_ProjectOwner
    {
        [DataType(DataType.Text)]
        [Display(Name = "projectOwnerID", ResourceType = typeof(Localization.ProjectOwner))]
        public int projectOwnerID { get; set; }

        [Display(Name = "pOTID", ResourceType = typeof(Localization.ProjectOwner))]
        [Range(minimum: 0, maximum: int.MaxValue, ErrorMessageResourceType = typeof(Localization.ProjectOwner)
            , ErrorMessageResourceName = "provide_avalid_POT")]
        public int pOTID { get; set; }

        public string pOTName { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "name", ResourceType = typeof(Localization.ProjectOwner))]
        [Required(ErrorMessageResourceType = typeof(Localization.ProjectOwner)
            , ErrorMessageResourceName = "provide_avalid_Name")]
        [RegularExpression(RegexEnum.validText, ErrorMessageResourceType = typeof(Localization.Global)
            , ErrorMessageResourceName = "Special_Characters_Not_Allowed")]
        [StringLength(maximumLength: 100, MinimumLength = 5, ErrorMessageResourceType = typeof(Localization.ProjectOwner)
            , ErrorMessageResourceName = "name_length_validation")]
        //[Remote("IsValidProjectOwner", "", HttpMethod = "POST", AdditionalFields = "projectOwnerID,pOTID"
          //  , ErrorMessageResourceType = typeof(Localization.ProjectOwner), ErrorMessageResourceName = "ProjectOwnerNameAlreadyExist")]
        public string name { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Description", ResourceType = typeof(Localization.ProjectOwner))]
        [Required(ErrorMessageResourceType = typeof(Localization.ProjectOwner)
            , ErrorMessageResourceName = "avalid_Description")]
        [StringLength(maximumLength: 250, MinimumLength = 2, ErrorMessageResourceType = typeof(Localization.ProjectOwner)
            , ErrorMessageResourceName = "length_Description_validation")]
        [RegularExpression(RegexEnum.validText, ErrorMessageResourceType = typeof(Localization.Global)
            , ErrorMessageResourceName = "Special_Characters_Not_Allowed")]
        public string description { get; set; }

        [Display(Name = "boolIsOwner", ResourceType = typeof(Localization.ProjectOwner))]
        public bool isOwner { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "accountable", ResourceType = typeof(Localization.ProjectOwner))]
        [Required(ErrorMessageResourceType = typeof(Localization.ProjectOwner)
            , ErrorMessageResourceName = "avalid_Accountable")]
        [StringLength(maximumLength: 50, MinimumLength = 2, ErrorMessageResourceType = typeof(Localization.ProjectOwner)
            , ErrorMessageResourceName = "length_Accountable_validation")]
        [RegularExpression(RegexEnum.validText, ErrorMessageResourceType = typeof(Localization.Global)
            , ErrorMessageResourceName = "Special_Characters_Not_Allowed")]
        public string accountable { get; set; }

        public Ent_ContactDetails contactDetails { get; set; }

        [Display(Name = "boolIsActive", ResourceType = typeof(Localization.Global))]
        public bool isActive { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "makerID", ResourceType = typeof(Localization.Global))]
        public string makerID { get; set; }

        public override string ToString()
        {
            return String.Concat("projectOwnerID: ", projectOwnerID, "~ pOTID: ", pOTID, "~ name: ", name
                , "~ description: ", description, "~ isOwner: ", isOwner, "~ Accountable: ", accountable
                , "~ contactDetails: ", contactDetails.ToString(), "~ isActive: ", isActive, "~ makerID: ", makerID);
        }
    }
}