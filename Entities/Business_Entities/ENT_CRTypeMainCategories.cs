using QA.Entities.Session_Entities;
using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace QA.Entities.Business_Entities
{
    public class Ent_CRTypeMainCategories
    {
        [DataType(DataType.Text)]
        [Display(Name = "ID", ResourceType = typeof(Localization.CR_TYPES_MAIN_CATEGORIES))]
        public int ID { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Name", ResourceType = typeof(Localization.CR_TYPES_MAIN_CATEGORIES))]
        [Required(ErrorMessageResourceType = typeof(Localization.CR_TYPES_MAIN_CATEGORIES)
            , ErrorMessageResourceName = "provide_valid_Name")]
        [RegularExpression(RegexEnum.validText, ErrorMessageResourceType = typeof(Localization.Global)
            , ErrorMessageResourceName = "Special_Characters_Not_Allowed")]
        [StringLength(maximumLength: 100, MinimumLength = 5, ErrorMessageResourceType = typeof(Localization.CR_TYPES_MAIN_CATEGORIES)
            , ErrorMessageResourceName = "Name_length_validation")]
        [Remote("IsValidCRTypeMCName", "CRTypeMC", HttpMethod = "POST", AdditionalFields = "ID"
            , ErrorMessageResourceType = typeof(Localization.CR_TYPES_MAIN_CATEGORIES)
            , ErrorMessageResourceName = "NameAlreadyExist")]
        public string name { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Description", ResourceType = typeof(Localization.CR_TYPES_MAIN_CATEGORIES))]
        [Required(ErrorMessageResourceType = typeof(Localization.CR_TYPES_MAIN_CATEGORIES)
            , ErrorMessageResourceName = "provide_valid_Description")]
        [StringLength(maximumLength: 250, MinimumLength = 2, ErrorMessageResourceType = typeof(Localization.CR_TYPES_MAIN_CATEGORIES)
            , ErrorMessageResourceName = "Description_length_validation")]
        [RegularExpression(RegexEnum.validText, ErrorMessageResourceType = typeof(Localization.Global)
            , ErrorMessageResourceName = "Special_Characters_Not_Allowed")]
        public string description { get; set; }

        [Display(Name = "boolIsActive", ResourceType = typeof(Localization.Global))]
        public bool isActive { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "makerID", ResourceType = typeof(Localization.Global))]
        public string makerID { get; set; }

        public override string ToString()
        {
            return String.Concat("ID: ", ID, "~ name: ", name, "~ description: ", description
                , "~ isActive: ", isActive, "~ makerID: ", makerID);
        }
    }
}