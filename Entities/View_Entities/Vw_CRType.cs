using QA.Entities.Session_Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace QA.Entities.View_Entities
{
    public class Vw_CRType
    {
        /*               View Details                        */
        public Business_Entities.Ent_CRType CRType { get; set; }

        public Session_Entities.Ent_UserSession userSession { get; set; }

        /*               List Of Values                       */
        public List<LOV> lOVCRMC { get; set; }

        public List<LOV> lOVCRGroups { get; set; }

        public List<LOV> lOVCategories { get; set; }

        /*               Search Details                        */
        [DataType(DataType.Text)]
        [Display(Name = "name_Search", ResourceType = typeof(Localization.CRType))]
        public string searchName { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "description_Search", ResourceType = typeof(Localization.CRType))]
        public string searchDescription { get; set; }

        [Display(Name = "CRTMCID_Search", ResourceType = typeof(Localization.CRType))]
        public int searchCRTMCID { get; set; }

        [Display(Name = "groupID_Search", ResourceType = typeof(Localization.CRType))]
        public int searchgroupID { get; set; }

        [Display(Name = "Category_Search", ResourceType = typeof(Localization.CRType))]
        public int searchCategoryID { get; set; }

        [Display(Name = "boolIsActive", ResourceType = typeof(Localization.Global))]
        public int searchIsActive { get; set; }
    }
}