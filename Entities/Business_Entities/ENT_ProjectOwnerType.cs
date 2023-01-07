using QA.Entities.Session_Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QA.Entities.Business_Entities
{
    public class Ent_ProjectOwnerType
    {
        [DataType(DataType.Text)]
        [Display(Name = "ProjectOwner_Type_ID", ResourceType = typeof(Localization.PROJECT_OWNER_TYPE))]
        public int ProjectOwnerTypeID { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "ProjectOwner_type_name", ResourceType = typeof(Localization.PROJECT_OWNER_TYPE))]
        [Required(ErrorMessageResourceType = typeof(Localization.PROJECT_OWNER_TYPE)
            , ErrorMessageResourceName = "please_provide_avalid_ProjectOwner_Type_Name")]
        [RegularExpression(RegexEnum.validText, ErrorMessageResourceType = typeof(Localization.PROJECT_OWNER_TYPE)
            , ErrorMessageResourceName = "Special_Characters_Not_Allowed")]
        [StringLength(maximumLength: 100, MinimumLength = 5, ErrorMessageResourceType = typeof(Localization.PROJECT_OWNER_TYPE)
            , ErrorMessageResourceName = "ProjectOwner_Type_name_length_should_be_between_5_100_characters")]
        [Remote("IsValidProjectOwnerTypeName", "ProjectOwnerType", HttpMethod = "POST", AdditionalFields = "ProjectOwnerTypeID"
            , ErrorMessageResourceType = typeof(Localization.PROJECT_OWNER_TYPE), ErrorMessageResourceName = "ProjectOwnerTypenameAlreadyExist")]
        public string name { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "ProjectOwner_type_Description", ResourceType = typeof(Localization.PROJECT_OWNER_TYPE))]
        [Required(ErrorMessageResourceType = typeof(Localization.PROJECT_OWNER_TYPE)
            , ErrorMessageResourceName = "please_provide_avalid_ProjectOwner_Type_Description")]
        [RegularExpression(RegexEnum.validText, ErrorMessageResourceType = typeof(Localization.PROJECT_OWNER_TYPE)
            , ErrorMessageResourceName = "Special_Characters_Not_Allowed")]
        [StringLength(maximumLength: 250, MinimumLength = 2, ErrorMessageResourceType = typeof(Localization.PROJECT_OWNER_TYPE)
            , ErrorMessageResourceName = "ProjectOwner_Type_Desc_length_should_be_between_2_250_characters")]
        public string description { get; set; }

        [Display(Name = "boolIsVendor", ResourceType = typeof(Localization.Global))]
        public bool isVendor { get; set; }

        [Display(Name = "boolIsActive", ResourceType = typeof(Localization.Global))]
        public bool isActive { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "makerID", ResourceType = typeof(Localization.Global))]
        public string makerID { get; set; }

        public override string ToString()
        {
            return String.Concat("ProjectOwnerTypeID: ", ProjectOwnerTypeID, "~ name: ", name, "~ description: ", description
                , "~ isVendor: ", isVendor, "~ isActive: ", isActive, "~ makerID: ", makerID);
        }
    }
}