using QA.Entities.Business_Entities;
using QA.Entities.Session_Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace QA.Entities.View_Entities
{
    public class Vw_CR
    {
        /*               View Details                        */
        public Business_Entities.Ent_CR CR { get; set; }

        public Session_Entities.Ent_UserSession userSession { get; set; }

        /*               List Of Values                       */
        public List<LOV> lOVProjects { get; set; }

        public List<LOV> lOVProjectItems { get; set; }

        public List<LOV> lOVCRTypeMCs { get; set; }

        public List<LOV> lOVCRGroups { get; set; }

        public List<LOV> lOVCRTypes { get; set; }

        public List<LOV> lOVCRStatuses { get; set; }

        public List<LOV> lOVSampleUnits { get; set; }

        /*               Search Details                        */
        [Display(Name = "projectID_Search", ResourceType = typeof(Localization.CR))]
        public int searchProjectID { get; set; }

        [Display(Name = "projectItemID_Search", ResourceType = typeof(Localization.CR))]
        public int searchProjectItemID { get; set; }

        [Display(Name = "CRMCID_Search", ResourceType = typeof(Localization.CR))]
        public int searchCRMCID { get; set; }

        [Display(Name = "CRGroupID_Search", ResourceType = typeof(Localization.CR))]
        public int searchCRGroupID { get; set; }

        [Display(Name = "CRTypeID_Search", ResourceType = typeof(Localization.CR))]
        public int searchCRTypeID { get; set; }

        [Display(Name = "status_Search", ResourceType = typeof(Localization.CR))]
        public int searchStatus { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "CRID_Search_Lbl", ResourceType = typeof(Localization.CR))]
        public string searchCRID { get; set; }
    }

    public class Vw_CR_Project
    {
        public List<LOV> lOVProjectItems { get; set; }

        public Ent_MapSelection mapSelection { get; set; }
    }
}