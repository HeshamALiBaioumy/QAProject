using QA.Entities.Business_Entities;
using QA.Entities.Session_Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QA.Entities.View_Entities
{
    public class Vw_Dashboard
    {
        public Ent_Dashboard dashboard { get; set; }

        public ResponseMessage response { get; set; }
    }
}