using QA.Entities.Session_Entities;
using QA.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QA.Entities.Business_Entities
{
    public class Ent_ProjectItem
    {
        [DataType(DataType.Text)]
        [Display(Name = "ID", ResourceType = typeof(Localization.ProjectItem))]
        public int ID { get; set; }

        [Display(Name = "projectID", ResourceType = typeof(Localization.ProjectItem))]
        [Required(ErrorMessageResourceType = typeof(Localization.ProjectItem)
            , ErrorMessageResourceName = "provide_avalid_projectID")]
        public int projectID { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "name", ResourceType = typeof(Localization.ProjectItem))]
        [Required(ErrorMessageResourceType = typeof(Localization.ProjectItem)
            , ErrorMessageResourceName = "Name_Required_Validation")]
        [RegularExpression(RegexEnum.validText, ErrorMessageResourceType = typeof(Localization.Global)
            , ErrorMessageResourceName = "Special_Characters_Not_Allowed")]
        [StringLength(maximumLength: 50, MinimumLength = 5, ErrorMessageResourceType = typeof(Localization.ProjectItem)
            , ErrorMessageResourceName = "Name_length_Validation")]
        [Remote("IsValidProjectItemName", "ProjectItem", HttpMethod = "POST", AdditionalFields = "ID,projectID"
            , ErrorMessageResourceType = typeof(Localization.ProjectItem), ErrorMessageResourceName = "ProjectItemAlreadyExist")]
        public string name { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Description", ResourceType = typeof(Localization.ProjectItem))]
        [Required(ErrorMessageResourceType = typeof(Localization.ProjectItem)
            , ErrorMessageResourceName = "Description_Required_Validation")]
        [StringLength(maximumLength: 250, MinimumLength = 2, ErrorMessageResourceType = typeof(Localization.ProjectItem)
            , ErrorMessageResourceName = "Description_Length_Validation")]
        [RegularExpression(RegexEnum.validText, ErrorMessageResourceType = typeof(Localization.Global)
            , ErrorMessageResourceName = "Special_Characters_Not_Allowed")]
        public string description { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "amount", ResourceType = typeof(Localization.ProjectItem))]
        [Required(ErrorMessageResourceType = typeof(Localization.ProjectItem)
            , ErrorMessageResourceName = "Amount_Required_Validation")]
        [RegularExpression(RegexEnum.numbers, ErrorMessageResourceType = typeof(Localization.Global)
            , ErrorMessageResourceName = "Numbers_Only_Field")]
        public double amount { get; set; }

        // Look-up table – وحدات القياس
        [Display(Name = "amountUnit", ResourceType = typeof(Localization.ProjectItem))]
        [Required(ErrorMessageResourceType = typeof(Localization.ProjectItem)
            , ErrorMessageResourceName = "provide_avalid_AmountUnit")]
        public int amountUnitID { get; set; }

        public string txtAmountUnit { get; set; }

        //public string txtAmountUnit
        //{
        //    get
        //    {
        //        string result = "";

        //        switch (amountUnitID)
        //        {
        //            case 0:
        //                result = Localization.Project.tbl_ProjMlstone_AmtUnit_M;
        //                break;
        //            case 1:
        //                result = Localization.Project.tbl_ProjMlstone_AmtUnit_KM;
        //                break;
        //            case 2:
        //                result = Localization.Project.tbl_ProjMlstone_AmtUnit_KG;
        //                break;
        //            default:
        //                result = "un-handled Mapping";
        //                break;
        //        }

        //        return result;
        //    }
        //}

        [Display(Name = "percentage", ResourceType = typeof(Localization.ProjectItem))]
        public int percentage { get; set; }

        public override string ToString()
        {
            return String.Concat("ID: ", ID, "~ projectID: ", projectID, "~ name: ", name, "~ description: ", description
                , "~ amount: ", amount, "~ amountUnitID: ", amountUnitID, "~ percentage: ", percentage);
        }
    }
}