using QA.Entities.Session_Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace QA.Entities.View_Entities
{
    public class Vw_ProjectItem
    {
        /*               View Details                        */
        public Business_Entities.Ent_ProjectItem projectItem { get; set; }

        public Session_Entities.Ent_UserSession userSession { get; set; }

        /*               List Of Values                       */
        public List<LOV> lOVProjects { get; set; }

        public List<LOV> lOVAmountUnits { get; set; }

        /*               Search Details                        */
        [DataType(DataType.Text)]
        [Display(Name = "name_Search", ResourceType = typeof(Localization.ProjectItem))]
        public string searchName { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Description_Search", ResourceType = typeof(Localization.ProjectItem))]
        public string searchDescription { get; set; }

        [Display(Name = "projectID_Search", ResourceType = typeof(Localization.ProjectItem))]
        public string searchProjectID { get; set; }

        [Display(Name = "boolIsActive", ResourceType = typeof(Localization.Global))]
        public int searchIsActive { get; set; }
    }
}