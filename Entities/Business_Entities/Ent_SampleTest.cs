using QA.Entities.Session_Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QA.Entities.Business_Entities
{
    public class Ent_SampleTest
    {
        [DataType(DataType.Text)]
        [Display(Name = "ID", ResourceType = typeof(Localization.SampleTest))]
        public int ID { get; set; }

        [Display(Name = "SampleTestCategoryID", ResourceType = typeof(Localization.SampleTest))]
        [Range(minimum: 0, maximum: int.MaxValue, ErrorMessageResourceType = typeof(Localization.SampleTest)
            , ErrorMessageResourceName = "provide_avalid_CategoryID")]
        public int sampleTestCategoryID { get; set; }

        public string sampleTestCategoryName { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Name", ResourceType = typeof(Localization.SampleTest))]
        [Required(ErrorMessageResourceType = typeof(Localization.SampleTest)
            , ErrorMessageResourceName = "provide_valid_Name")]
        [RegularExpression(RegexEnum.validText, ErrorMessageResourceType = typeof(Localization.Global)
            , ErrorMessageResourceName = "Special_Characters_Not_Allowed")]
        [StringLength(maximumLength: 100, MinimumLength = 5, ErrorMessageResourceType = typeof(Localization.SampleTest)
            , ErrorMessageResourceName = "name_length_validation")]
        [Remote("IsValidSampleTest", "SampleTest", HttpMethod = "POST", AdditionalFields = "ID"
            , ErrorMessageResourceType = typeof(Localization.SampleTest), ErrorMessageResourceName = "NameAlreadyExist")]
        public string name { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Description", ResourceType = typeof(Localization.SampleTest))]
        [Required(ErrorMessageResourceType = typeof(Localization.SampleTest)
            , ErrorMessageResourceName = "Provide_Description_Validation")]
        [StringLength(maximumLength: 250, MinimumLength = 2, ErrorMessageResourceType = typeof(Localization.SampleTest)
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
            return String.Concat("ID: ", ID, " ~ sampleTestCategoryID: ", sampleTestCategoryID, "~ name: ", name, "~ description: ", description
                , "~ isActive: ", isActive, "~ makerID: ", makerID);
        }
    }
}