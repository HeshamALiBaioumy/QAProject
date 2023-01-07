using QA.Entities.Session_Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace QA.Entities.Business_Entities
{
    public class Ent_Department
    {
        [DataType(DataType.Text)]
        [Display(Name = "departmentID", ResourceType = typeof(Localization.Department))]
        public int departmentID { get; set; }

        [Display(Name = "projectOwnerID", ResourceType = typeof(Localization.Department))]
        [Range(minimum: 0, maximum: int.MaxValue, ErrorMessageResourceType = typeof(Localization.DepartmentSection)
            , ErrorMessageResourceName = "provide_avalid_DepartmentID")]
        public int projectOwnerID { get; set; }

        public string projectOwnerName { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "name", ResourceType = typeof(Localization.Department))]
        [Required(ErrorMessageResourceType = typeof(Localization.Department)
            , ErrorMessageResourceName = "avalid_Name")]
        [RegularExpression(RegexEnum.validText, ErrorMessageResourceType = typeof(Localization.Global)
            , ErrorMessageResourceName = "Special_Characters_Not_Allowed")]
        [StringLength(maximumLength: 100, MinimumLength = 5, ErrorMessageResourceType = typeof(Localization.Department)
            , ErrorMessageResourceName = "Name_length_validation")]
        [Remote("IsValidDepartmentName", "Department", HttpMethod = "POST", AdditionalFields = "departmentID,projectOwnerID"
            , ErrorMessageResourceType = typeof(Localization.Department), ErrorMessageResourceName = "NameAlreadyExist")]
        public string name { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "description", ResourceType = typeof(Localization.Department))]
        [Required(ErrorMessageResourceType = typeof(Localization.Department)
            , ErrorMessageResourceName = "avalid_Description")]
        [RegularExpression(RegexEnum.validText, ErrorMessageResourceType = typeof(Localization.Global)
            , ErrorMessageResourceName = "Special_Characters_Not_Allowed")]
        [StringLength(maximumLength: 200, MinimumLength = 5, ErrorMessageResourceType = typeof(Localization.Department)
            , ErrorMessageResourceName = "Description_length_validation")]
        public string description { get; set; }

        [Display(Name = "boolIsActive", ResourceType = typeof(Localization.Global))]
        public bool isActive { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "makerID", ResourceType = typeof(Localization.Global))]
        public string makerID { get; set; }

        public override string ToString()
        {
            return String.Concat("departmentID: ", departmentID, "~ projectOwnerID: ", projectOwnerID
                , "~ projectOwnerName: ", projectOwnerName, "~ name: ", name, "~ description: ", description
                , "~ isActive: ", isActive, "~ makerID: ", makerID);
        }
    }
}