using QA.Entities.Session_Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace QA.Entities.View_Entities
{
    public class Vw_CheckListItemGroup
    {
        /*               View Details                        */
        public Business_Entities.Ent_CheckListItemGroup checkListItemGroup { get; set; }

        public Session_Entities.Ent_UserSession userSession { get; set; }

        /*               List Of Values                       */
        public List<LOV> lOVCheckListItems { get; set; }

        /*               Search Details                        */
        [DataType(DataType.Text)]
        [Display(Name = "name_Search", ResourceType = typeof(Localization.ProjectOwner))]
        public string searchName { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Description_Search", ResourceType = typeof(Localization.ProjectOwner))]
        public string searchDescription { get; set; }

        [Display(Name = "itemID_Search", ResourceType = typeof(Localization.ProjectOwner))]
        public List<int> searchLstCLItemIDs { get; set; }

        [Display(Name = "boolIsActive", ResourceType = typeof(Localization.Global))]
        public int searchIsActive { get; set; }
    }
}