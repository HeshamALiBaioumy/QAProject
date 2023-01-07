using QA.Entities.Session_Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QA.Entities.Business_Entities
{
    public class Ent_MixerType
    {
        [DataType(DataType.Text)]
        [Display(Name = "ID", ResourceType = typeof(Localization.MixerType))]
        public int mixerTypeID { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Name", ResourceType = typeof(Localization.MixerType))]
        [Required(ErrorMessageResourceType = typeof(Localization.MixerType)
            , ErrorMessageResourceName = "provide_valid_Name")]
        [RegularExpression(RegexEnum.validText, ErrorMessageResourceType = typeof(Localization.Global)
            , ErrorMessageResourceName = "Special_Characters_Not_Allowed")]
        [StringLength(maximumLength: 100, MinimumLength = 5, ErrorMessageResourceType = typeof(Localization.MixerType)
            , ErrorMessageResourceName = "Name_length_validation")]
        [Remote("IsValidMixerTypeName", "MixerType", HttpMethod = "POST", AdditionalFields = "mixerTypeID"
            , ErrorMessageResourceType = typeof(Localization.MixerType), ErrorMessageResourceName = "NameAlreadyExist")]
        public string name { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Description", ResourceType = typeof(Localization.MixerType))]
        [Required(ErrorMessageResourceType = typeof(Localization.MixerType)
            , ErrorMessageResourceName = "provide_valid_Description")]
        [StringLength(maximumLength: 250, MinimumLength = 2, ErrorMessageResourceType = typeof(Localization.MixerType)
            , ErrorMessageResourceName = "Desc_length_validation")]
        public string description { get; set; }

        [Display(Name = "boolIsActive", ResourceType = typeof(Localization.Global))]
        public bool isActive { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "makerID", ResourceType = typeof(Localization.Global))]
        public string makerID { get; set; }

        public override string ToString()
        {
            return String.Concat("mixerTypeID: ", mixerTypeID, "~ name: ", name, "~ description: ", description
                , "~ isActive: ", isActive, "~ makerID: ", makerID);
        }
    }
}