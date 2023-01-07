using QA.Entities.Session_Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace QA.Entities.View_Entities
{
    public class Vw_CheckListFlow_Sequence
    {
        /*               View Details                        */
        public Business_Entities.Ent_CheckListFlow_Sequence CheckListFlow_Sequence { get; set; }

        public Session_Entities.Ent_UserSession userSession { get; set; }

        /*               List Of Values                       */
        public List<LOV> lOVCheckLists { get; set; }

        public List<LOV> lOVTechnicianUsers { get; set; }

        public List<LOV> lOVSupervisorEngUsers { get; set; }

        public List<LOV> lOVQALabUsers { get; set; }

        public List<LOV> lOVRepresentitiveSuperUsers { get; set; }

        public List<LOV> lOVCLFlowStatuses { get; set; }

        /*               Search Details                        */
        [DataType(DataType.Text)]
        [Display(Name = "name_Search", ResourceType = typeof(Localization.CheckListFlow_Sequence))]
        public string searchName { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "description_Search", ResourceType = typeof(Localization.CheckListFlow_Sequence))]
        public string searchDescription { get; set; }

        [Display(Name = "boolIsActive", ResourceType = typeof(Localization.Global))]
        public int searchIsActive { get; set; }
    }
}