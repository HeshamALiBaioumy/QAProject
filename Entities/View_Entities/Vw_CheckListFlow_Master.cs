using QA.Entities.Session_Entities;
using System.Collections.Generic;

namespace QA.Entities.View_Entities
{
    public class Vw_CheckListFlow_Master
    {
        /*               View Details                        */
        public Business_Entities.Ent_CheckListFlow_Master CheckListFlow_Master { get; set; }

        public Session_Entities.Ent_UserSession userSession { get; set; }

        /*               List Of Values                       */
        public List<LOV> lOVCheckLists { get; set; }

        public List<LOV> lOVCheckListSequences { get; set; }

        public List<LOV> lOVTechnicianUsers { get; set; }

        public List<LOV> lOVSupervisorEngUsers { get; set; }

        public List<LOV> lOVQALabUsers { get; set; }

        public List<LOV> lOVRepresentitiveSuperUsers { get; set; }

        public List<LOV> lOVCLFlowStatuses { get; set; }

        /*               Search Details                        */
    }
}