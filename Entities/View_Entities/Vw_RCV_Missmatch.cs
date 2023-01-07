using QA.Entities.Business_Entities;
using QA.Entities.Session_Entities;
using System.Collections.Generic;

namespace QA.Entities.View_Entities
{
    public class Vw_RCV_Missmatch 
    {
        /*               View Details                        */
        public List<Ent_RCV_Missmatch> lstRCVMissmatch { get; set; }

        public Ent_RCV_Missmatch RCVMissmatch { get; set; }

        /*               List Of Values                       */
        public List<LOV> lOVRCVs { get; set; }

        public List<LOV> lOVProjects { get; set; }

        public List<LOV> lOVCRs { get; set; }

        public List<LOV> lOVUserProfiles { get; set; }

        public List<LOV> lOVRCVMissmatchStatus { get; set; }

        public List<LOV> lOVSampleUnits { get; set; }

        public List<LOV> lOVPendingOn { get; set; }

        /*               Search Details                        */
        public int searchRCVID { get; set; }

        public int searchCRID { get; set; }

        public int searchProjectID { get; set; }

        public int searchProfileID { get; set; }

        public int searchRCVMMStatus { get; set; }

        public int searchPendingOn { get; set; }
    }
}