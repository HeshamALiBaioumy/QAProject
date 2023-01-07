using QA.Entities.Session_Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QA.Entities.Business_Entities
{
    public class Ent_CheckListItemGroup
    {
        [DataType(DataType.Text)]
        public int ID { get; set; }

        [Display(Name = "itemID", ResourceType = typeof(Localization.CheckListItemGroup))]
        [Required(ErrorMessageResourceType = typeof(Localization.CheckListItemGroup)
            , ErrorMessageResourceName = "provide_avalid_Item")]
        public List<int> lstCLItemIDs { get; set; }

        public string lstCLItemNames { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "name", ResourceType = typeof(Localization.CheckListItemGroup))]
        [Required(ErrorMessageResourceType = typeof(Localization.CheckListItemGroup)
            , ErrorMessageResourceName = "provide_valid_Name")]
        [RegularExpression(RegexEnum.validText, ErrorMessageResourceType = typeof(Localization.Global)
            , ErrorMessageResourceName = "Special_Characters_Not_Allowed")]
        [StringLength(maximumLength: 100, MinimumLength = 5, ErrorMessageResourceType = typeof(Localization.CheckListItemGroup)
            , ErrorMessageResourceName = "name_length_validation")]
        [Remote("IsValidCheckListItemGroup", "CheckListItemGroup", HttpMethod = "POST", AdditionalFields = "ID"
            , ErrorMessageResourceType = typeof(Localization.CheckListItemGroup)
            , ErrorMessageResourceName = "nameAlreadyExist")]
        public string name { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Description", ResourceType = typeof(Localization.CheckListItemGroup))]
        [Required(ErrorMessageResourceType = typeof(Localization.CheckListItemGroup)
            , ErrorMessageResourceName = "Provide_Description_Validation")]
        [StringLength(maximumLength: 250, MinimumLength = 2, ErrorMessageResourceType = typeof(Localization.CheckListItemGroup)
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
            return String.Concat("ID: ", ID, "lstCLItemIDs: ", String.Join(",", lstCLItemIDs.ToArray()), "~ name: ", name
                , "~ description: ", description, "~ isActive: ", isActive, "~ makerID: ", makerID);
        }
    }
}