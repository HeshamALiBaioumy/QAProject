using QA.Entities.Reports_Entities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace QA.Entities.View_Entities
{
    public class Vw_SearchCr_Report
    {
        public int searchProjectID { get; set; }

        public DateTime searchDateFrom { get; set; }

        public string strSearchDateFrom
        {
            get
            {
                return this.searchDateFrom.ToString(ConfigurationManager.AppSettings["dateFormat"].ToString());
            }
        }

        public DateTime searchDateTo { get; set; }

        public string strSearchDateTo
        {
            get
            {
                return this.searchDateTo.ToString(ConfigurationManager.AppSettings["dateFormat"].ToString());
            }
        }

        public string projectName { get; set; }

        public string superEngName { get; set; }

        public string consEngName { get; set; }

        public string contractorName { get; set; }

        public string authLabName { get; set; }

        public List<Rpt_CR> lstSearchCRs { get; set; }
    }
}