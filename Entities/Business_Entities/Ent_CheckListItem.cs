using QA.Entities.Session_Entities;
using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace QA.Entities.Business_Entities
{
    public class Ent_CheckListItem
    {
        [DataType(DataType.Text)]
        public int checkListItemID { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "name", ResourceType = typeof(Localization.CheckListItem))]
        [Required(ErrorMessageResourceType = typeof(Localization.CheckListItem)
            , ErrorMessageResourceName = "provide_valid_Name")]
        [RegularExpression(RegexEnum.validText, ErrorMessageResourceType = typeof(Localization.Global)
            , ErrorMessageResourceName = "Special_Characters_Not_Allowed")]
        [StringLength(maximumLength: 100, MinimumLength = 5, ErrorMessageResourceType = typeof(Localization.CheckListItem)
            , ErrorMessageResourceName = "name_length_validation")]
        [Remote("IsValidCheckListItem", "CheckListItem", HttpMethod = "POST", AdditionalFields = "checkListItemID"
            , ErrorMessageResourceType = typeof(Localization.CheckListItem), ErrorMessageResourceName = "NameAlreadyExist")]
        public string name { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Description", ResourceType = typeof(Localization.CheckListItem))]
        [Required(ErrorMessageResourceType = typeof(Localization.CheckListItem)
            , ErrorMessageResourceName = "Provide_Description_Validation")]
        [StringLength(maximumLength: 250, MinimumLength = 2, ErrorMessageResourceType = typeof(Localization.CheckListItem)
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
            return String.Concat("checkListItemID: ", checkListItemID, "~ name: ", name, "~ description: ", description
                , "~ isActive: ", isActive, "~ makerID: ", makerID);
        }
    }
}