using QA.Entities.Business_Entities;
using System;
using System.Collections.Generic;
using System.Configuration;

namespace QA.Entities.Reports_Entities
{
    public class Rpt_CR
    {
        /*           CR Details                 */
        public int CRID { get; set; }

        public string CRTypeName { get; set; }

        public DateTime registrationDate { get; set; }

        public string strRegistrationDate
        {
            get
            {
                return (registrationDate == default(DateTime)) ? "" : registrationDate.ToString("dd/MM/yyyy");
            }
        }

        public DateTime lastStatusDate { get; set; }

        public string strlastStatusDate
        {
            get
            {
                return (lastStatusDate == default(DateTime)) ? "" : lastStatusDate.ToString(ConfigurationManager.AppSettings["dateFormat"].ToString());
            }
        }

        public string CRStatusName { get; set; }
        /*           Project Details                 */
        public string projectName { get; set; }

        public string projectItemName { get; set; }

        public DateTime projectregistrationDate { get; set; }

        public string strProjectregistrationDate
        {
            get
            {
                return (registrationDate == default(DateTime)) ? "" : registrationDate.ToString("dd/MM/yyyy");
            }
        }

        public string contractorName { get; set; }

        public string projectOwnerName { get; set; }

        public string departmentName { get; set; }

        public string depSectionName { get; set; }

        public string superEngName { get; set; }

        public string consulatntEngName { get; set; }

        public string authLabName { get; set; }

        public string labEntryName { get; set; }

        public string authLabFeedback { get; set; }

        public string authLabFeedbackValue
        {
            get
            {
                String result = "";

                if (this.authLabFeedback == "Y")
                {
                    result = Localization.CR.Report_SearchCRSamples_LabFeed_Accept;
                }
                else if (this.authLabFeedback == "N")
                {
                    result = Localization.CR.Report_SearchCRSamples_LabFeed_Reject;
                }
                else
                {
                    result = this.authLabFeedback;
                }

                return result;
            }
        }

        public DateTime authLabFeedbackDate { get; set; }

        public string strAuthLabFeedbackDate
        {
            get
            {
                return (authLabFeedbackDate == default(DateTime)) ? "" : authLabFeedbackDate
                    .ToString(ConfigurationManager.AppSettings["dateFormat"].ToString());
            }
        }

        public string sampleID { get; set; }

        public string sampleMaker { get; set; }

        public string sampleSize { get; set; }

        public string sampleLength { get; set; }

        public string sampleUnitName { get; set; }

        /*              Project Map Details                   */
        public int mapZoomLevel { get; set; }

        public List<Ent_MapPoint> projectMapPoints { get; set; }

        public string projectMapJEOJSON { get; set; }

        /*              CR Map Details                   */
        public Ent_MapSelection.mapSelectionType CRMapSelectionType { get; set; }

        public List<Ent_MapPoint> crMapPoints { get; set; }

        public string CrMapJEOJSON { get; set; }

        /*              Sample Map Details                   */
        public Ent_MapSelection.mapSelectionType sampleMapSelectionType { get; set; }

        public List<Ent_MapPoint> sampleMapPoints { get; set; }

        public string SampleMapJEOJSON { get; set; }

        public override string ToString()
        {
            return String.Concat("CRID: ", CRID, "~ contractorName: ", contractorName, "~ projectName: ", projectName
                , "~ projectItemName: ", projectItemName, "~ CRTypeMCName: ", "~ CRTypeName: ", CRTypeName, "~ registrationDate: "
                , strRegistrationDate);
        }
    }
}