using QA.Entities.Session_Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace QA.Entities.View_Entities
{
    public class Vw_ProjectOwner
    {
        /*               View Details                        */
        public Business_Entities.Ent_ProjectOwner projectOwner { get; set; }

        public Session_Entities.Ent_UserSession userSession { get; set; }

        /*               List Of Values                       */
        public List<LOV> lOVProjectOwnerType { get; set; }

        /*               Search Details                        */
        [DataType(DataType.Text)]
        [Display(Name = "name_Search", ResourceType = typeof(Localization.ProjectOwner))]
        public string searchName { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Description_Search", ResourceType = typeof(Localization.ProjectOwner))]
        public string searchDescription { get; set; }

        [Display(Name = "pOTID_Search", ResourceType = typeof(Localization.ProjectOwner))]
        public int searchPOTID { get; set; }

        [Display(Name = "boolIsActive", ResourceType = typeof(Localization.Global))]
        public int searchIsActive { get; set; }
    }
}