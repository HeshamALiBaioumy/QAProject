using QA.Entities.Session_Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QA.Entities.Business_Entities
{
    public class Ent_CRType
    {
        [DataType(DataType.Text)]
        [Display(Name = "ID", ResourceType = typeof(Localization.CRType))]
        public int ID { get; set; }

        [Display(Name = "CRTMCID", ResourceType = typeof(Localization.CRType))]
        public int CRTMCID { get; set; }

        public string CRTMCName { get; set; }

        [Display(Name = "groupID", ResourceType = typeof(Localization.CRType))]
        [Range(minimum: 0, maximum: int.MaxValue, ErrorMessageResourceType = typeof(Localization.CRType)
            , ErrorMessageResourceName = "provide_avalid_groupID")]
        public int groupID { get; set; }

        public string groupname { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "name", ResourceType = typeof(Localization.CRType))]
        [Required(ErrorMessageResourceType = typeof(Localization.CRType)
            , ErrorMessageResourceName = "avalid_Name")]
        [RegularExpression(RegexEnum.validText, ErrorMessageResourceType = typeof(Localization.Global)
            , ErrorMessageResourceName = "Special_Characters_Not_Allowed")]
        [StringLength(maximumLength: 100, MinimumLength = 5, ErrorMessageResourceType = typeof(Localization.CRType)
            , ErrorMessageResourceName = "Name_length_validation")]
        [Remote("IsValidCRType", "CRType", HttpMethod = "POST", AdditionalFields = "ID, groupID"
            , ErrorMessageResourceType = typeof(Localization.CRType), ErrorMessageResourceName = "NameAlreadyExist")]
        public string name { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "description", ResourceType = typeof(Localization.CRType))]
        [Required(ErrorMessageResourceType = typeof(Localization.CRType)
            , ErrorMessageResourceName = "avalid_Description")]
        [RegularExpression(RegexEnum.validText, ErrorMessageResourceType = typeof(Localization.Global)
            , ErrorMessageResourceName = "Special_Characters_Not_Allowed")]
        [StringLength(maximumLength: 200, MinimumLength = 5, ErrorMessageResourceType = typeof(Localization.CRType)
            , ErrorMessageResourceName = "Description_length_validation")]
        public string description { get; set; }

        [Display(Name = "Category", ResourceType = typeof(Localization.CRType))]
        [Range(minimum: 0, maximum: int.MaxValue, ErrorMessageResourceType = typeof(Localization.CRType)
            , ErrorMessageResourceName = "provide_avalid_Category")]
        public int CRcategory { get; set; }

        public string CRcategoryStr
        {
            get
            {
                string categoryString = "";

                switch (CRcategory)
                {
                    case 0:
                        categoryString = Localization.CRType.DDL_Category_Checkup;
                        break;
                    case 1:
                        categoryString = Localization.CRType.DDL_Category_withoutSample;
                        break;
                    default:
                        categoryString = CRcategory.ToString();
                        break;
                }

                return categoryString;
            }
        }

        [Display(Name = "boolIsActive", ResourceType = typeof(Localization.Global))]
        public bool isActive { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "makerID", ResourceType = typeof(Localization.Global))]
        public string makerID { get; set; }

        public override string ToString()
        {
            return String.Concat("ID: ", ID
                , "~ CRTMCID: ", CRTMCID, "~ CRTMCName: ", CRTMCName
                , "~ groupID: ", groupID, "~ groupname: ", groupname
                , "~ name: ", name, "~ description: ", description
                , "~ CRcategory: " + CRcategory, "~ isActive: ", isActive, "~ makerID: ", makerID);
        }
    }
}