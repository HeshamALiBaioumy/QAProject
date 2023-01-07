using QA.Entities.Session_Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace QA.Entities.Business_Entities
{
    public class Ent_RCV_Missmatch
    {
        /*                             RCV Details                               */
        public int RCVID { get; set; }

        public int projectID { get; set; }

        public string projectName { get; set; }

        public int projectItemID { get; set; }

        public string projectItemName { get; set; }

        public int CRID { get; set; }

        public string strCRID
        {
            get
            {
                return (CRID <= 0) ? "" : CRID.ToString();
            }
        }

        public DateTime CRRegistrationDate { get; set; }

        public string strCRRegistrationDate
        {
            get
            {
                return (CRRegistrationDate == default(DateTime)) ? "" : CRRegistrationDate.ToString("dd/MM/yyyy");
            }
        }

        public int cRStatusID { get; set; }

        public string cRStatusName { get; set; }

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

        public string comments { get; set; }

        public bool isLabRequired { get; set; }

        /*                     RCV Missmatch Case Details                        */
        public int ID { get; set; }

        public DateTime caseCreateDate { get; set; }

        public string strCaseCreateDate
        {
            get
            {
                return (caseCreateDate == default(DateTime)) ? "" : caseCreateDate.ToString("dd/MM/yyyy");
            }
        }

        [Required(ErrorMessageResourceType = typeof(Localization.RCV_Missmatch)
            , ErrorMessageResourceName = "Create_caseDescription_Val_Required")]
        [RegularExpression(RegexEnum.validText, ErrorMessageResourceType = typeof(Localization.Global)
            , ErrorMessageResourceName = "Special_Characters_Not_Allowed")]
        [StringLength(maximumLength: 1000, MinimumLength = 5, ErrorMessageResourceType = typeof(Localization.RCV_Missmatch)
            , ErrorMessageResourceName = "Create_caseDescription_Val_length")]
        [AllowHtml]
        public string caseDescription { get; set; }

        public string plainCaseDescription
        {
            get
            {
                return Regex.Replace(Regex.Replace(this.caseDescription, "<.*?>", " "), "&nbsp;", " ");
            }
        }

        public Ent_MapSelection mapProject { get; set; }

        public Ent_MapSelection mapCR { get; set; }

        public Ent_CR_Sample sample1 { get; set; }

        public Ent_MapSelection mapSample1 { get; set; }

        public Ent_CR_Sample sample2 { get; set; }

        public Ent_MapSelection mapSample2 { get; set; }

        public int status { get; set; }

        public string statusName { get; set; }

        public int pendingOn { get; set; }

        public string pendingOnName { get; set; }

        public DateTime caseCloseDate { get; set; }

        public string strCaseCloseDate
        {
            get
            {
                return (caseCloseDate == default(DateTime)) ? "" : caseCloseDate.ToString("dd/MM/yyyy");
            }
        }

        public bool allowedforEdit { get; set; }

        public bool isRCVCase { get; set; }

        public bool allowedforClose { get; set; }

        public bool allowedforAction { get; set; }

        public List<string> lstAttachmentNames { get; set; }

        public List<string> lstAttachmentPaths { get; set; }

        public string makerID { get; set; }

        public override string ToString()
        {
            return String.Concat("ID: ", ID, "~ RCVID: ", RCVID, "~ projectID: ", projectID, "~ projectName: "
                , projectName, "~ CRID: ", CRID, "~ CRRegistrationDate: ", CRRegistrationDate, "~ assignUserID: "
                , assignUserID, "~ assignUserName: ", assignUserName, "~ RCVAssignDate: ", RCVAssignDate
                , "~ comments: ", comments, "~ isLabRequired: ", isLabRequired, "~ caseCreateDate: ", caseCreateDate
                , "~ caseDescription: ", caseDescription, "~ mapProject: ", (mapProject != null) ? mapProject.ToString() : "NA"
                , "~ sample1: ", (sample1 != null) ? sample1.ToString() : "NA", "~ sample2: "
                , sample2.ToString(), "~ status: ", status, "~ statusName: "
                , statusName, "~ caseCloseDate: ", caseCloseDate, "~ allowedforEdit: ", allowedforEdit
                , "~ allowedforClose: ", allowedforClose, "~ allowedforAction: ", allowedforAction
                , "~ lstAttachmentNames: ", (lstAttachmentNames != null) ? lstAttachmentNames.ToString() : ""
                , "~ lstAttachmentPaths: ", (lstAttachmentPaths != null) ? lstAttachmentPaths.ToString() : ""
                , "~ makerID: ", makerID);
        }
    }
}