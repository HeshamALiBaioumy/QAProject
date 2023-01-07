using QA.Entities.Session_Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace QA.Entities.View_Entities
{
    public class Vw_Complaint
    {
        /*               View Details                        */
        public Business_Entities.Ent_Complaint complaint { get; set; }

        public Session_Entities.Ent_UserSession userSession { get; set; }

        /*               List Of Values                       */
        public List<LOV> lOVProjects { get; set; }

        public List<LOV> lOVCRs { get; set; }

        public List<LOV> lOVComplaintStatus { get; set; }

        /*               Search Details                        */
        [Display(Name = "ProjectID_Search", ResourceType = typeof(Localization.Complaint))]
        public int searchProjectID { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "CRID_Search", ResourceType = typeof(Localization.Complaint))]
        public int searchCRID { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "description_Search", ResourceType = typeof(Localization.Complaint))]
        public string searchDescription { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Comments_Search", ResourceType = typeof(Localization.Complaint))]
        public string searchComments { get; set; }

        [Display(Name = "ComplaintStatus_Search", ResourceType = typeof(Localization.Global))]
        public int searchcomplaintStatus { get; set; }
    }
}