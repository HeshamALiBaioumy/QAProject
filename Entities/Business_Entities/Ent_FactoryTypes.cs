using QA.Entities.Session_Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QA.Entities.Business_Entities
{
    public class Ent_FactoryTypes
    {
        // Property attributes (Data Annotation)

        [DataType(DataType.Text)]
        [Display(Name = "Factory_Type_ID", ResourceType = typeof(Localization.FactoryType))]
        public int factoryTypeID { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Factory_type_name", ResourceType = typeof(Localization.FactoryType))]
        [Required(ErrorMessageResourceType = typeof(Localization.FactoryType)
            , ErrorMessageResourceName = "please_provide_avalid_Factory_Type_Name")]
        [RegularExpression(RegexEnum.validText, ErrorMessageResourceType = typeof(Localization.Global)
            , ErrorMessageResourceName = "Special_Characters_Not_Allowed")]
        [StringLength(maximumLength: 100, MinimumLength = 5, ErrorMessageResourceType = typeof(Localization.FactoryType)
            , ErrorMessageResourceName = "Factory_Type_name_length_should_be_between_5_100_characters")]
        [Remote("IsValidfactoryTypeName", "FactoryType", HttpMethod = "POST", AdditionalFields = "factoryTypeID"
            , ErrorMessageResourceType = typeof(Localization.FactoryType), ErrorMessageResourceName = "FactoryTypenameAlreadyExist")]
        public string name { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Factory_type_Description", ResourceType = typeof(Localization.FactoryType))]
        [Required(ErrorMessageResourceType = typeof(Localization.FactoryType)
            , ErrorMessageResourceName = "please_provide_avalid_Factory_Type_Description")]
        [StringLength(maximumLength: 250, MinimumLength = 2, ErrorMessageResourceType = typeof(Localization.FactoryType)
            , ErrorMessageResourceName = "Factory_Type_Desc_length_should_be_between_2_250_characters")]
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
            return String.Concat("factoryTypeID: ", factoryTypeID, "~ name: ", name, "~ description: ", description
                , "~ isActive: ", isActive, "~ makerID: ", makerID);
        }
    }
}