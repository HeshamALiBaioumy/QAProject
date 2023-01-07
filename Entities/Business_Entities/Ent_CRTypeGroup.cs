using QA.Entities.Session_Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QA.Entities.Business_Entities
{
    public class Ent_CRTypeGroup
    {
        [DataType(DataType.Text)]
        [Display(Name = "ID", ResourceType = typeof(Localization.CRTYPEGroup))]
        public int ID { get; set; }

        [Display(Name = "CRTMCID", ResourceType = typeof(Localization.CRTYPEGroup))]
        [Range(minimum: 0, maximum: int.MaxValue, ErrorMessageResourceType = typeof(Localization.CRTYPEGroup)
            , ErrorMessageResourceName = "provide_valid_CRTMCID")]
        public int CRTMCID { get; set; }

        public string CRTMCName { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Name", ResourceType = typeof(Localization.CRTYPEGroup))]
        [Required(ErrorMessageResourceType = typeof(Localization.CRTYPEGroup)
            , ErrorMessageResourceName = "provide_valid_Name")]
        [RegularExpression(RegexEnum.validText, ErrorMessageResourceType = typeof(Localization.Global)
            , ErrorMessageResourceName = "Special_Characters_Not_Allowed")]
        [StringLength(maximumLength: 100, MinimumLength = 5, ErrorMessageResourceType = typeof(Localization.CRTYPEGroup)
            , ErrorMessageResourceName = "Name_length_validation")]
        [Remote("IsValidCRTypeGroup", "CRTypeGroup", HttpMethod = "POST", AdditionalFields = "ID, CRTMCID"
            , ErrorMessageResourceType = typeof(Localization.CRTYPEGroup)
            , ErrorMessageResourceName = "NameAlreadyExist")]
        public string name { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Description", ResourceType = typeof(Localization.CRTYPEGroup))]
        [Required(ErrorMessageResourceType = typeof(Localization.CRTYPEGroup)
            , ErrorMessageResourceName = "provide_valid_Description")]
        [StringLength(maximumLength: 250, MinimumLength = 2, ErrorMessageResourceType = typeof(Localization.CRTYPEGroup)
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
            return String.Concat("ID: ", ID, "~ CRTMCID: ", CRTMCID, "~ name: ", name
                , "~ description: ", description, "~ isActive: ", isActive, "~ makerID: ", makerID);
        }
    }
}