using QA.Entities.Session_Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QA.Entities.Business_Entities
{
    public class Ent_Factory
    {
        [DataType(DataType.Text)]
        [Display(Name = "factoryID", ResourceType = typeof(Localization.Factory))]
        public int factoryID { get; set; }


        [DataType(DataType.Text)]
        [Display(Name = "mixerCount", ResourceType = typeof(Localization.Factory))]
        public int mixerCount { get; set; }

        [Display(Name = "factoryTypeID", ResourceType = typeof(Localization.Factory))]
        [Range(minimum: 0, maximum: int.MaxValue, ErrorMessageResourceType = typeof(Localization.Factory)
            , ErrorMessageResourceName = "provide_valid_factoryTypeID")]
        public int factoryTypeID { get; set; }

        public string factoryTypeName { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "name", ResourceType = typeof(Localization.Factory))]
        [Required(ErrorMessageResourceType = typeof(Localization.Factory)
            , ErrorMessageResourceName = "valid_Name")]
        [RegularExpression(RegexEnum.validText, ErrorMessageResourceType = typeof(Localization.Global)
            , ErrorMessageResourceName = "Special_Characters_Not_Allowed")]
        [StringLength(maximumLength: 100, MinimumLength = 5, ErrorMessageResourceType = typeof(Localization.Factory)
            , ErrorMessageResourceName = "Name_length_validation")]
        [Remote("IsValidFactoryName", "Factory", HttpMethod = "POST", AdditionalFields = "factoryID,factoryTypeID"
            , ErrorMessageResourceType = typeof(Localization.Factory), ErrorMessageResourceName = "NameAlreadyExist")]
        public string name { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "description", ResourceType = typeof(Localization.Factory))]
        [Required(ErrorMessageResourceType = typeof(Localization.Factory)
            , ErrorMessageResourceName = "avalid_Description")]
        [RegularExpression(RegexEnum.validText, ErrorMessageResourceType = typeof(Localization.Global)
            , ErrorMessageResourceName = "Special_Characters_Not_Allowed")]
        [StringLength(maximumLength: 200, MinimumLength = 5, ErrorMessageResourceType = typeof(Localization.Factory)
            , ErrorMessageResourceName = "Description_length_validation")]
        public string description { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "factoryCode", ResourceType = typeof(Localization.Factory))]
        [Required(ErrorMessageResourceType = typeof(Localization.Factory)
            , ErrorMessageResourceName = "valid_factoryCode")]
        [RegularExpression(RegexEnum.validText, ErrorMessageResourceType = typeof(Localization.Global)
            , ErrorMessageResourceName = "Special_Characters_Not_Allowed")]
        [StringLength(maximumLength: 50, MinimumLength = 2, ErrorMessageResourceType = typeof(Localization.Factory)
            , ErrorMessageResourceName = "factoryCode_length_validation")]
        public string factoryCode { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "addressLine", ResourceType = typeof(Localization.Factory))]
        [Required(ErrorMessageResourceType = typeof(Localization.Factory)
            , ErrorMessageResourceName = "valid_addressLine")]
        [RegularExpression(RegexEnum.validText, ErrorMessageResourceType = typeof(Localization.Global)
            , ErrorMessageResourceName = "Special_Characters_Not_Allowed")]
        [StringLength(maximumLength: 200, MinimumLength = 5, ErrorMessageResourceType = typeof(Localization.Factory)
            , ErrorMessageResourceName = "addressLine_length_validation")]
        public string addressLine { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "factoryPower", ResourceType = typeof(Localization.Factory))]
        [Required(ErrorMessageResourceType = typeof(Localization.Factory)
            , ErrorMessageResourceName = "provide_valid_factoryPower")]
        [RegularExpression(RegexEnum.validText, ErrorMessageResourceType = typeof(Localization.Global)
            , ErrorMessageResourceName = "Special_Characters_Not_Allowed")]
        [StringLength(maximumLength: 50, MinimumLength = 2, ErrorMessageResourceType = typeof(Localization.Factory)
            , ErrorMessageResourceName = "factoryPower_length_validation")]
        public string factoryPower { get; set; }

        [Display(Name = "boolIsActive", ResourceType = typeof(Localization.Global))]
        public bool isActive { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "makerID", ResourceType = typeof(Localization.Global))]
        public string makerID { get; set; }

        public override string ToString()
        {
            return String.Concat("factoryID: ", factoryID, "~ factoryTypeID: ", factoryTypeID, "~ factoryCode: ", factoryCode
                , "~ name: ", name, "~ description: ", description, "~ addressLine: ", addressLine, "~ factoryPower: ", factoryPower
                , "~ isActive: ", isActive, "~ makerID: ", makerID);
        }
    }
}