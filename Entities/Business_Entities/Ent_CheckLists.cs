using QA.Entities.Session_Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace QA.Entities.Business_Entities
{
    public class Ent_CheckLists
    {
        [DataType(DataType.Text)]
        public int ID { get; set; }

        [Display(Name = "GroupID", ResourceType = typeof(Localization.CheckLists))]
        [Required(ErrorMessageResourceType = typeof(Localization.CheckLists)
            , ErrorMessageResourceName = "provide_avalid_Group")]
        public List<int> lstCLGroupIDs { get; set; }

        public string lstCLGroupNames { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "name", ResourceType = typeof(Localization.CheckLists))]
        [Required(ErrorMessageResourceType = typeof(Localization.CheckLists)
            , ErrorMessageResourceName = "provide_valid_Name")]
        [RegularExpression(RegexEnum.validText, ErrorMessageResourceType = typeof(Localization.Global)
            , ErrorMessageResourceName = "Special_Characters_Not_Allowed")]
        [StringLength(maximumLength: 100, MinimumLength = 5, ErrorMessageResourceType = typeof(Localization.CheckLists)
            , ErrorMessageResourceName = "name_length_validation")]
        [Remote("IsValidCheckLists", "CheckLists", HttpMethod = "POST", AdditionalFields = "ID, lstCLGroupIDs"
            , ErrorMessageResourceType = typeof(Localization.CheckLists)
            , ErrorMessageResourceName = "nameAlreadyExist")]
        public string name { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Description", ResourceType = typeof(Localization.CheckLists))]
        [Required(ErrorMessageResourceType = typeof(Localization.CheckLists)
            , ErrorMessageResourceName = "provide_valid_Description")]
        [StringLength(maximumLength: 250, MinimumLength = 2, ErrorMessageResourceType = typeof(Localization.CheckLists)
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
            return String.Concat("ID: ", ID, "~ checkListGroupID: ", String.Join(",", lstCLGroupIDs.ToArray())
                , "~ name: ", name, "~ description: ", description, "~ isActive: ", isActive, "~ makerID: ", makerID);
        }
    }
}