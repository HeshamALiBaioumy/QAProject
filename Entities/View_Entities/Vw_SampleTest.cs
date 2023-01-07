using QA.Entities.Business_Entities;
using QA.Entities.Session_Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace QA.Entities.View_Entities
{
    public class Vw_SampleTest
    {
        /*               View Details                        */
        public Ent_SampleTest SampleTest { get; set; }

        public Ent_UserSession userSession { get; set; }

        /*               List Of Values                       */
        public List<LOV> lOVSampleTestCategories { get; set; }

        /*               Search Details                        */
        [DataType(DataType.Text)]
        [Display(Name = "SampleTestCategoryID_Search", ResourceType = typeof(Localization.SampleTest))]
        public int searchSampleTestCategoryID { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Name_Search", ResourceType = typeof(Localization.SampleTest))]
        public string searchName { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Description_Search", ResourceType = typeof(Localization.SampleTest))]
        public string searchDescription { get; set; }

        [Display(Name = "boolIsActive", ResourceType = typeof(Localization.Global))]
        public int searchIsActive { get; set; }
    }
}