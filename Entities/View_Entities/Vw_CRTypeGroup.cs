using QA.Entities.Session_Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace QA.Entities.View_Entities
{
    public class Vw_CRTypeGroup
    {
        /*               View Details                        */
        public Business_Entities.Ent_CRTypeGroup CRTypeGroup { get; set; }

        public Session_Entities.Ent_UserSession userSession { get; set; }

        /*               List Of Values                       */
        public List<LOV> lOVCRMainCategories { get; set; }

        /*               Search Details                        */
        [DataType(DataType.Text)]
        [Display(Name = "CRTMCID_Search", ResourceType = typeof(Localization.CRTYPEGroup))]
        public int searchCRMCID { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Name_Search", ResourceType = typeof(Localization.CRTYPEGroup))]
        public string searchName { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Description_Search", ResourceType = typeof(Localization.CRTYPEGroup))]
        public string searchDescription { get; set; }

        [Display(Name = "boolIsActive", ResourceType = typeof(Localization.Global))]
        public int searchIsActive { get; set; }
    }
}