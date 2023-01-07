using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace QA.Entities.View_Entities
{
    public class Vw_SampleType
    {
        /*               View Details                        */
        public Business_Entities.Ent_SampleType sampleType { get; set; }

        public Session_Entities.Ent_UserSession userSession { get; set; }

        /*               Search Details                        */
        [DataType(DataType.Text)]
        [Display(Name = "Name_Search", ResourceType = typeof(Localization.SampleType))]
        public string searchName { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Description_Search", ResourceType = typeof(Localization.SampleType))]
        public string searchDescription { get; set; }

        [Display(Name = "boolIsActive", ResourceType = typeof(Localization.Global))]
        public int searchIsActive { get; set; }
    }
}