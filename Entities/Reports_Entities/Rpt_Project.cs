using QA.Entities.Business_Entities;
using System.Collections.Generic;

namespace QA.Entities.Reports_Entities
{
    public class Rpt_Project
    {
        /*                 Project Details               */
        public int projectID { get; set; }

        public string projectName { get; set; }

        public string projectOwnerName { get; set; }

        public string departmentName { get; set; }

        public string departmentSectionName { get; set; }

        /*           Project Stake-holders                   */

        public string contractorName { get; set; }

        public string superEngName { get; set; }

        public string consulatntEngName { get; set; }

        public string authLabName { get; set; }


        /*           Project Related CR's Statistics          */
        public int CR_Total { get; set; }

        public int CR_WIP { get; set; }

        public int CR_Closed { get; set; }

        public int CR_SuperEng_Pending { get; set; }

        public int CR_SuperEng_Recieved { get; set; }

        public int CR_SuperEng_Accepted { get; set; }

        public int CR_SuperEng_Rejected { get; set; }

        public int CR_ConsEng_Pending { get; set; }

        public int CR_ConsEng_Recieved { get; set; }

        public int CR_ConsEng_Accepted { get; set; }

        public int CR_ConsEng_Rejected { get; set; }

        public int CR_AuthLab_Pending { get; set; }

        public int CR_AuthLab_Recieved { get; set; }

        public int CR_AuthLab_Accepted { get; set; }

        public int CR_AuthLab_Rejected { get; set; }

        public List<SampleStatus> lstSampleTypesStatus { get; set; }

        /*         Project Related Complaints Statistics          */
        public int complaint_Total { get; set; }

        public int complaint_WIP { get; set; }

        public int complaint_Closed { get; set; }

        /*              Project Map Details                   */
        public int mapZoomLevel { get; set; }

        public List<Ent_MapPoint> projectMapPoints { get; set; }

        public string projectMapJEOJSON { get; set; }
    }

    public struct SampleStatus
    {
        public string sampleType { get; set; }
        public int accepted { get; set; }
        public int rejected { get; set; }
    }
}