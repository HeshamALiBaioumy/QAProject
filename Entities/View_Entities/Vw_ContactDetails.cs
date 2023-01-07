using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QA.Entities.View_Entities
{
    public class Vw_ContactDetails
    {
        public Business_Entities.Ent_ContactDetails contactDetails { get; set; }

        public Session_Entities.Ent_UserSession userSession { get; set; }

    }
}