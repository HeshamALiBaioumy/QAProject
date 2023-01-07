using QA.Entities.Session_Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace QA.Entities.View_Entities
{
    public class Vw_RCV
    {
        /*               View Details                        */
        public List<Business_Entities.Ent_RCV> lstRCVs { get; set; }

        public Business_Entities.Ent_RCV RCV { get; set; }

        /*               List Of Values                       */
        public List<LOV> lOVProjects { get; set; }

        public List<LOV> lOVCRs { get; set; }

        public List<LOV> lOVUserProfiles { get; set; }

        public List<LOV> lOVRCVStatus { get; set; }

        /*               Search Details                        */
        [Display(Name = "Search_projectID", ResourceType = typeof(Localization.RCV))]
        public int searchProjectID { get; set; }

        [Display(Name = "Search_CRID", ResourceType = typeof(Localization.RCV))]
        public int searchCRID { get; set; }

        [Display(Name = "Search_ProfileID", ResourceType = typeof(Localization.RCV))]
        public int searchProfileID { get; set; }

        [Display(Name = "Search_Status", ResourceType = typeof(Localization.RCV))]
        public int searchStatus { get; set; }

        [Display(Name = "Search_boolIsLabRequired", ResourceType = typeof(Localization.RCV))]
        public string searchIsLabRequired { get; set; }
    }
}