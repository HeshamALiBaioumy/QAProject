using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace QA.Entities.View_Entities
{
    public class Vw_MixerTypes
    {
        /*               View Details                        */
        public Business_Entities.Ent_MixerType mixerType { get; set; }

        public Session_Entities.Ent_UserSession userSession { get; set; }

        /*               Search Details                        */
        [DataType(DataType.Text)]
        [Display(Name = "name_Search", ResourceType = typeof(Localization.Department))]
        public string searchName { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "description_Search", ResourceType = typeof(Localization.Department))]
        public string searchDescription { get; set; }

        [Display(Name = "boolIsActive", ResourceType = typeof(Localization.Global))]
        public int searchIsActive { get; set; }
    }
}