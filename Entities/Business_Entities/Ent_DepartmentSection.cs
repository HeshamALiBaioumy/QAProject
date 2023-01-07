using QA.Entities.Session_Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace QA.Entities.Business_Entities
{
    public class Ent_DepartmentSection
    {
        [DataType(DataType.Text)]
        [Display(Name = "sectionID", ResourceType = typeof(Localization.DepartmentSection))]
        public int sectionID { get; set; }

        [Display(Name = "departmentID", ResourceType = typeof(Localization.DepartmentSection))]
        [Required(ErrorMessageResourceType = typeof(Localization.DepartmentSection)
            , ErrorMessageResourceName = "provide_avalid_DepartmentID")]
        [Range(minimum: 0, maximum: int.MaxValue, ErrorMessageResourceType = typeof(Localization.DepartmentSection)
            , ErrorMessageResourceName = "provide_avalid_DepartmentID")]
        public int departmentID { get; set; }

        public string departmentName { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "name", ResourceType = typeof(Localization.DepartmentSection))]
        [Required(ErrorMessageResourceType = typeof(Localization.DepartmentSection)
            , ErrorMessageResourceName = "avalid_Name")]
        [RegularExpression(RegexEnum.validText, ErrorMessageResourceType = typeof(Localization.Global)
            , ErrorMessageResourceName = "Special_Characters_Not_Allowed")]
        [StringLength(maximumLength: 100, MinimumLength = 5, ErrorMessageResourceType = typeof(Localization.DepartmentSection)
            , ErrorMessageResourceName = "Name_length_validation")]
        [Remote("IsValidDepartmentSectionName", "DepartmentSection", HttpMethod = "POST", AdditionalFields = "sectionID,departmentID"
            , ErrorMessageResourceType = typeof(Localization.DepartmentSection), ErrorMessageResourceName = "NameAlreadyExist")]
        public string name { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "description", ResourceType = typeof(Localization.DepartmentSection))]
        [Required(ErrorMessageResourceType = typeof(Localization.DepartmentSection)
            , ErrorMessageResourceName = "avalid_Description")]
        [RegularExpression(RegexEnum.validText, ErrorMessageResourceType = typeof(Localization.Global)
            , ErrorMessageResourceName = "Special_Characters_Not_Allowed")]
        [StringLength(maximumLength: 200, MinimumLength = 5, ErrorMessageResourceType = typeof(Localization.DepartmentSection)
            , ErrorMessageResourceName = "Description_length_validation")]
        public string description { get; set; }

        [Display(Name = "boolIsActive", ResourceType = typeof(Localization.Global))]
        public bool isActive { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "makerID", ResourceType = typeof(Localization.Global))]
        public string makerID { get; set; }

        public override string ToString()
        {
            return String.Concat("sectionID: ", sectionID, "~ departmentID: ", departmentID
                , "~ departmentName: ", departmentName, "~ name: ", name, "~ description: ", description
                , "~ isActive: ", isActive, "~ makerID: ", makerID);
        }
    }
}