using QA.Entities.Session_Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace QA.Entities.Business_Entities
{
    public class Ent_RCV
    {
        public int RCVID { get; set; }

        public int projectID { get; set; }
        
        public string projectName { get; set; }

        public int CRID { get; set; }

        public DateTime CRRegistrationDate { get; set; }

        public string strCRRegistrationDate
        {
            get
            {
                return (CRRegistrationDate == default(DateTime)) ? "" : CRRegistrationDate.ToString("dd/MM/yyyy");
            }
        }

        public bool isLabRequired { get; set; }

        public int assignUserID { get; set; }

        public string assignUserName { get; set; }

        public DateTime RCVAssignDate { get; set; }

        public string strRCVAssignDate
        {
            get
            {
                return (RCVAssignDate == default(DateTime)) ? "" : RCVAssignDate.ToString("dd/MM/yyyy");
            }
        }

        public int status { get; set; }

        public string statusName { get; set; }

        [DataType(DataType.Text)]
        [RegularExpression(RegexEnum.validText, ErrorMessageResourceType = typeof(Localization.Global)
            , ErrorMessageResourceName = "Special_Characters_Not_Allowed")]
        [StringLength(maximumLength: 250, ErrorMessageResourceType = typeof(Localization.RCV)
            , ErrorMessageResourceName = "FeedBack_Comments_LengthValidation")]
        public string comments { get; set; }

        public DateTime RCVClosureDate { get; set; }

        public string strRCVClosureDate
        {
            get
            {
                return (RCVClosureDate == default(DateTime)) ? "" : RCVClosureDate.ToString("dd/MM/yyyy");
            }
        }

        public List<HttpPostedFileBase> attachFiles { get; set; }

        public List<string> lstAttachmentNames { get; set; }

        public List<string> lstAttachmentPaths { get; set; }

        public bool allowedforAction { get; set; }

        public bool allowedforEdit { get; set; }
    }
}