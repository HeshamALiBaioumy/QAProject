using QA.Entities.Session_Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace QA.Entities.View_Entities
{
    public class Vw_Department
    {
        /*               View Details                        */
        public Business_Entities.Ent_Department department { get; set; }

        public Session_Entities.Ent_UserSession userSession { get; set; }

        /*               List Of Values                       */
        public List<LOV> lOVprojectOwners { get; set; }

        /*               Search Details                        */
        [Display(Name = "projectOwnerID", ResourceType = typeof(Localization.Department))]
        public int searchprojectOwnerID { get; set; }

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