using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QA.Entities.View_Entities
{
    public class Vw_CR_Workflow
    {
        /*               View Details                        */
        public List<Business_Entities.Ent_CR> lstCRs { get; set; }

        public Session_Entities.Ent_UserSession userSession { get; set; }
    }
}