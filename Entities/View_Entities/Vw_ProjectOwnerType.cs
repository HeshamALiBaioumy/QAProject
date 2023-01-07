using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace QA.Entities.View_Entities
{
    public class Vw_ProjectOwnerType
    {
        /*               View Details                        */
        public Business_Entities.Ent_ProjectOwnerType projectOwnerType { get; set; }

        public Session_Entities.Ent_UserSession userSession { get; set; }

        /*               Search Details                        */
        [DataType(DataType.Text)]
        [Display(Name = "POT_name_Search", ResourceType = typeof(Localization.PROJECT_OWNER_TYPE))]
        public string searchName { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "POT_Description_Search", ResourceType = typeof(Localization.PROJECT_OWNER_TYPE))]
        public string searchDescription { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "POT_IsVendor_Search", ResourceType = typeof(Localization.PROJECT_OWNER_TYPE))]
        public int searchIsVendor { get; set; }

        [Display(Name = "boolIsActive", ResourceType = typeof(Localization.Global))]
        public int searchIsActive { get; set; }
    }
}