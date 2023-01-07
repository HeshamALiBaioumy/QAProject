using QA.Entities.Session_Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QA.Entities.Business_Entities
{
    public class Ent_Mixer
    {
        [DataType(DataType.Text)]
        [Display(Name = "ID", ResourceType = typeof(Localization.Mixer))]
        public int mixerID { get; set; }

        [Display(Name = "factoryID", ResourceType = typeof(Localization.Mixer))]
        [Range(minimum: 0, maximum: int.MaxValue, ErrorMessageResourceType = typeof(Localization.Mixer)
            , ErrorMessageResourceName = "provide_avalid_factory")]
        public int factoryID { get; set; }

        public string factoryName { get; set; }

        [Display(Name = "mixerTypeID", ResourceType = typeof(Localization.Mixer))]
        [Range(minimum: 0, maximum: int.MaxValue, ErrorMessageResourceType = typeof(Localization.Mixer)
            , ErrorMessageResourceName = "provide_avalid_mixerType")]
        public int mixerTypeID { get; set; }

        public string mixerTypeName { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Name", ResourceType = typeof(Localization.Mixer))]
        [Required(ErrorMessageResourceType = typeof(Localization.Mixer)
            , ErrorMessageResourceName = "provide_valid_Name")]
        [RegularExpression(RegexEnum.validText, ErrorMessageResourceType = typeof(Localization.Global)
            , ErrorMessageResourceName = "Special_Characters_Not_Allowed")]
        [StringLength(maximumLength: 100, MinimumLength = 5, ErrorMessageResourceType = typeof(Localization.Mixer)
            , ErrorMessageResourceName = "Name_length_validation")]
        [Remote("IsValidMixer", "Mixer", HttpMethod = "POST", AdditionalFields = "mixerID, mixerTypeID"
            , ErrorMessageResourceType = typeof(Localization.Mixer), ErrorMessageResourceName = "NameAlreadyExist")]
        public string name { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "description", ResourceType = typeof(Localization.Mixer))]
        [Required(ErrorMessageResourceType = typeof(Localization.Mixer)
            , ErrorMessageResourceName = "provide_valid_Description")]
        [RegularExpression(RegexEnum.validText, ErrorMessageResourceType = typeof(Localization.Global)
            , ErrorMessageResourceName = "Special_Characters_Not_Allowed")]
        [StringLength(maximumLength: 200, MinimumLength = 5, ErrorMessageResourceType = typeof(Localization.Mixer)
            , ErrorMessageResourceName = "Description_length_validation")]
        public string description { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "mixerCode", ResourceType = typeof(Localization.Mixer))]
        [Required(ErrorMessageResourceType = typeof(Localization.Mixer)
            , ErrorMessageResourceName = "provide_valid_mixerCode")]
        [RegularExpression(RegexEnum.validText, ErrorMessageResourceType = typeof(Localization.Global)
            , ErrorMessageResourceName = "Special_Characters_Not_Allowed")]
        [StringLength(maximumLength: 50, MinimumLength = 5, ErrorMessageResourceType = typeof(Localization.Mixer)
            , ErrorMessageResourceName = "mixerCode_length_validation")]
        public string mixerCode { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "mixerModel", ResourceType = typeof(Localization.Mixer))]
        [Required(ErrorMessageResourceType = typeof(Localization.Mixer)
            , ErrorMessageResourceName = "provide_valid_mixerModel")]
        [RegularExpression(RegexEnum.validText, ErrorMessageResourceType = typeof(Localization.Global)
            , ErrorMessageResourceName = "Special_Characters_Not_Allowed")]
        [StringLength(maximumLength: 50, MinimumLength = 5, ErrorMessageResourceType = typeof(Localization.Mixer)
            , ErrorMessageResourceName = "mixerModel_length_validation")]
        public string mixerModel { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "mixerPower", ResourceType = typeof(Localization.Mixer))]
        [Required(ErrorMessageResourceType = typeof(Localization.Mixer)
            , ErrorMessageResourceName = "provide_valid_mixerPower")]
        [RegularExpression(RegexEnum.validText, ErrorMessageResourceType = typeof(Localization.Global)
            , ErrorMessageResourceName = "Special_Characters_Not_Allowed")]
        [StringLength(maximumLength: 50, MinimumLength = 5, ErrorMessageResourceType = typeof(Localization.Mixer)
            , ErrorMessageResourceName = "mixerPower_length_validation")]
        public string mixerPower { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "mixer_status", ResourceType = typeof(Localization.Mixer))]
        [Required(ErrorMessageResourceType = typeof(Localization.Mixer)
            , ErrorMessageResourceName = "provide_valid_mixer_status")]
        [RegularExpression(RegexEnum.validText, ErrorMessageResourceType = typeof(Localization.Global)
            , ErrorMessageResourceName = "Special_Characters_Not_Allowed")]
        [StringLength(maximumLength: 200, MinimumLength = 5, ErrorMessageResourceType = typeof(Localization.Mixer)
            , ErrorMessageResourceName = "mixer_status_length_validation")]
        public string mixerStatus { get; set; }

        [Display(Name = "boolIsActive", ResourceType = typeof(Localization.Global))]
        public bool isActive { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "makerID", ResourceType = typeof(Localization.Global))]
        public string makerID { get; set; }

        public override string ToString()
        {
            return String.Concat("mixerID: ", mixerID, "~ factoryID: ", factoryID, "~ mixerTypeID: ", mixerTypeID
                , "~ name: ", name, "~ mixerCode: ", mixerCode, "~ mixerModel: ", mixerModel, "~ mixerPower: ", mixerPower
                , "~ description: ", description, "~ mixerstatus: ", mixerStatus, "~ isActive: ", isActive, "~ makerID: ", makerID);
        }
    }
}