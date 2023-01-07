using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QA.Entities.Business_Entities
{
    public class Ent_Dashboard
    {
        public int Projects_Total { get; set; }

        public int Projects_New { get; set; }

        public int Projects_New_Percent
        {
            get
            {
                int denominator = (this.Projects_Total - this.Projects_New);
                return (denominator == 0) ? 0 : ((100 * this.Projects_New) / denominator);
            }
        }

        public int compliants_Total { get; set; }

        public int compliants_New { get; set; }

        public int compliants_New_Percent
        {
            get
            {
                int denominator = (this.compliants_Total - this.compliants_New);
                return (denominator == 0) ? 0 : ((100 * this.compliants_New) / denominator);
            }
        }

        public int compliants_Closed { get; set; }

        public int compliants_Closed_Percent
        {
            get
            {
                int denominator = (this.compliants_Total - this.compliants_Closed);
                return (denominator == 0) ? 0 : ((100 * this.compliants_Closed) / denominator);
            }
        }

        public int CR_Total { get; set; }

        public int CR_New { get; set; }

        public int CR_New_Percent
        {
            get
            {
                int denominator = (this.CR_Total - this.CR_New);
                return (denominator == 0) ? 0 : ((100 * this.CR_New) / denominator);
            }
        }

        public int CR_Pending_Total { get; set; }

        public int CR_Pending_New { get; set; }

        public int CR_Pending_New_Percent
        {
            get
            {
                int denominator = (this.CR_Pending_Total - this.CR_Pending_New);
                return (denominator == 0) ? 0 : ((100 * this.CR_Pending_New) / denominator);
            }
        }

        public int CR_Recieved_Pending_Total { get; set; }

        public int CR_Recieved_Pending_New { get; set; }

        public int CR_Recieved_Pending_New_Percent
        {
            get
            {
                int denominator = (this.CR_Recieved_Pending_Total - this.CR_Recieved_Pending_New);
                return (denominator == 0) ? 0 : ((100 * this.CR_Recieved_Pending_New) / denominator);
            }
        }

        public int CR_Pending_OverAll
        {
            get
            {
                return this.CR_Pending_Total + this.CR_Recieved_Pending_Total;
            }
        }

        public int CR_Pending_New_OverAll
        {
            get
            {
                return this.CR_Pending_New + this.CR_Recieved_Pending_New;
            }
        }

        public int CR_Pending_New_OverAll_Percent
        {
            get
            {
                int denominator = (this.CR_Pending_OverAll - this.CR_Pending_New_OverAll);
                return (denominator == 0) ? 0 : ((100 * this.CR_Pending_New_OverAll) / denominator);
            }
        }

        public int CR_Accepted_Total { get; set; }

        public int CR_Accepted_New { get; set; }

        public int CR_Accepted_New_Percent
        {
            get
            {
                int denominator = (this.CR_Accepted_Total - this.CR_Accepted_New);
                return (denominator == 0) ? 0 : ((100 * this.CR_Accepted_New) / denominator);
            }
        }

        public int CR_Rejected_Total { get; set; }

        public int CR_Rejected_New { get; set; }

        public int CR_Rejected_New_Percent
        {
            get
            {
                int denominator = (this.CR_Rejected_Total - this.CR_Rejected_New);
                return (denominator == 0) ? 0 : ((100 * this.CR_Rejected_New) / denominator);
            }
        }

        public int CL_Total { get; set; }

        public int CL_New { get; set; }

        public int CL_New_Percent
        {
            get
            {
                int denominator = (this.CL_Total - this.CL_New);
                return (denominator == 0) ? 0 : ((100 * this.CL_New) / denominator);
            }
        }

        public int CL_Done_Maker { get; set; }

        public int CL_Pending_Maker { get; set; }

        public int CL_Pending_New_Maker { get; set; }

        public int CL_Pending_New_Maker_Percent
        {
            get
            {
                int denominator = (this.CL_Pending_Maker - this.CL_Pending_New_Maker);
                return (denominator == 0) ? 0 : ((100 * this.CL_Pending_New_Maker) / denominator);
            }
        }

        public int CL_Done_Cheker { get; set; }

        public int CL_Pending_Cheker { get; set; }

        public int CL_Pending_New_Cheker { get; set; }

        public int CL_Pending_New_Cheker_Percent
        {
            get
            {
                int denominator = (this.CL_Pending_Cheker - this.CL_Pending_New_Cheker);
                return (denominator == 0) ? 0 : ((100 * this.CL_Pending_New_Cheker) / denominator);
            }
        }

        public int CL_Pending_OverAll
        {
            get
            {
                return this.CL_Pending_Maker + this.CL_Pending_Cheker;
            }
        }

        public int CL_Pending_New_OverAll
        {
            get
            {
                return this.CL_Pending_New_Maker + this.CL_Pending_New_Cheker;
            }
        }

        public int CL_Pending_New_OverAll_Percent
        {
            get
            {
                int denominator = (this.CL_Pending_OverAll - this.CL_Pending_New_OverAll);
                return (denominator == 0) ? 0 : ((100 * this.CL_Pending_New_OverAll) / denominator);
            }
        }

        public int CL_Accepted_Total { get; set; }

        public int CL_Rejected_Total { get; set; }

        public int CL_Closed_Total { get; set; }

        public int RCV_Total { get; set; }

        public int RCV_New { get; set; }

        public int RCV_New_Percent
        {
            get
            {
                int denominator = (this.RCV_Total - this.RCV_New);
                return (denominator == 0) ? 0 : ((100 * this.RCV_New) / denominator);
            }
        }

        public int RCV_Match { get; set; }

        public int RCV_Match_Percent
        {
            get
            {
                int denominator = (this.RCV_Total - this.RCV_Match);
                return (denominator == 0) ? 0 : ((100 * this.RCV_Match) / denominator);
            }
        }

        public int RCV_MissMatch { get; set; }

        public int RCV_MissMatch_Percent
        {
            get
            {
                int denominator = (this.RCV_Total - this.RCV_MissMatch);
                return (denominator == 0) ? 0 : ((100 * this.RCV_MissMatch) / denominator);
            }
        }

        public int RCV_Pending { get; set; }

        public int RCV_Pending_Percent
        {
            get
            {
                int denominator = (this.RCV_Total - this.RCV_Pending);
                return (denominator == 0) ? 0 : ((100 * this.RCV_Pending) / denominator);
            }
        }

        public int RCV_New_Pending { get; set; }

        public int RCV_New_Pending_Percent
        {
            get
            {
                int denominator = (this.RCV_Pending - this.RCV_New_Pending);
                return (denominator == 0) ? 0 : ((100 * this.RCV_New_Pending) / denominator);
            }
        }

        public int MMC_Total { get; set; }

        public int MMC_New { get; set; }

        public int MMC_New_Percent
        {
            get
            {
                int denominator = (this.MMC_Total - this.MMC_New);
                return (denominator == 0) ? 0 : ((100 * this.MMC_New) / denominator);
            }
        }

        public int MMC_Pending { get; set; }

        public int MMC_Pending_Percent
        {
            get
            {
                int denominator = (this.MMC_Total - this.MMC_Pending);
                return (denominator == 0) ? 0 : ((100 * this.MMC_Pending) / denominator);
            }
        }

        public int MMC_New_Pending { get; set; }

        public int MMC_New_Pending_Percent
        {
            get
            {
                int denominator = (this.MMC_Pending - this.MMC_New_Pending);
                return (denominator == 0) ? 0 : ((100 * this.MMC_New_Pending) / denominator);
            }
        }

        public int MMC_Fixed { get; set; }

        public int MMC_Fixed_Percent
        {
            get
            {
                int denominator = (this.MMC_Total - this.MMC_Fixed);
                return (denominator == 0) ? 0 : ((100 * this.MMC_Fixed) / denominator);
            }
        }

        public int MMC_Closed { get; set; }

        public int MMC_Closed_Percent
        {
            get
            {
                int denominator = (this.MMC_Total - this.MMC_Closed);
                return (denominator == 0) ? 0 : ((100 * this.MMC_Closed) / denominator);
            }
        }

        public List<List<string>> lstOverAllDashboard { get; set; }

        public List<string> lstOverAll_denominator
        {
            get
            {
                List<string> lst = lstOverAllDashboard[0];
                lst.RemoveAt(0);

                return (lst);
            }
        }

        public List<string> lstOverAll_CR
        {
            get
            {
                List<string> lst = lstOverAllDashboard[1];
                lst.RemoveAt(0);

                return (lst);
            }
        }

        public List<string> lstOverAll_CL
        {
            get
            {
                List<string> lst = lstOverAllDashboard[2];
                lst.RemoveAt(0);

                return (lst);
            }
        }

        public List<string> lstOverAll_MMC
        {
            get
            {
                List<string> lst = lstOverAllDashboard[3];
                lst.RemoveAt(0);

                return (lst);
            }
        }

        public string makerID { get; set; }

        public override string ToString()
        {
            return String.Concat("Projects_Total: ", Projects_Total, "~ Projects_New: ", Projects_New
                , "~ compliants_Total: ", compliants_Total, "~ compliants_New: ", compliants_New
                , "~ compliants_Closed: ", compliants_Closed, "~ CR_Total: ", CR_Total, "~ CR_New: ", CR_New
                , "~ CR_Pending_Total: ", CR_Pending_Total, "~ CR_Pending_New: ", CR_Pending_New, "~ CR_Recieved_Pending_Total: "
                , CR_Recieved_Pending_Total, "~ CR_Recieved_Pending_New: ", CR_Recieved_Pending_New
                , "~ CR_Accepted_Total: ", CR_Accepted_Total, "~ CR_Accepted_New: ", CR_Accepted_New, "~ CR_Rejected_Total: ", CR_Rejected_Total
                , "~ CR_Rejected_New: ", CR_Rejected_New
                , "~ makerID: ", makerID);
        }
    }
}