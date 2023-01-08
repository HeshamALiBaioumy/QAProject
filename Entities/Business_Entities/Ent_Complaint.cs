using QA.Entities.Session_Entities;
using System;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace QA.Entities.Business_Entities
{
    public class Ent_Complaint
    {
        [DataType(DataType.Text)]
        [Display(Name = "complaintID", ResourceType = typeof(Localization.Complaint))]
        public int complaintID { get; set; }

        public int projectID { get; set; }

        public string projectName { get; set; }

        [Display(Name = "CRID", ResourceType = typeof(Localization.Complaint))]
        [Range(minimum: 0, maximum: int.MaxValue, ErrorMessageResourceType = typeof(Localization.Complaint)
            , ErrorMessageResourceName = "provide_valid_CRID")]
        public int CRID { get; set; }

        public string CRName { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "description", ResourceType = typeof(Localization.Complaint))]
        [Required(ErrorMessageResourceType = typeof(Localization.Complaint)
            , ErrorMessageResourceName = "valid_Description")]
        [RegularExpression(RegexEnum.validText, ErrorMessageResourceType = typeof(Localization.Global)
            , ErrorMessageResourceName = "Special_Characters_Not_Allowed")]
        [StringLength(maximumLength: 200, MinimumLength = 5, ErrorMessageResourceType = typeof(Localization.Complaint)
            , ErrorMessageResourceName = "Description_length_validation")]
        public string description { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "comments", ResourceType = typeof(Localization.Complaint))]
        [Required(ErrorMessageResourceType = typeof(Localization.Complaint)
            , ErrorMessageResourceName = "valid_Comment")]
        [RegularExpression(RegexEnum.validText, ErrorMessageResourceType = typeof(Localization.Global)
            , ErrorMessageResourceName = "Special_Characters_Not_Allowed")]
        [StringLength(maximumLength: 100, ErrorMessageResourceType = typeof(Localization.Complaint)
            , ErrorMessageResourceName = "Comment_length_validation")]
        public string comments { get; set; }

        [Display(Name = "attachmentName", ResourceType = typeof(Localization.Attachment))]
        public string attachmentName { get; set; }

        //[Required(ErrorMessageResourceType = typeof(Localization.Attachment)
        //    , ErrorMessageResourceName = "SelectFileValidation")]
        public HttpPostedFileBase attachFile { get; set; }

        public string attachmentPath { get; set; }

        public bool hasAttachment { get; set; }

        [Editable(false)]
        [DataType(DataType.Text)]
        [Display(Name = "NotificationList", ResourceType = typeof(Localization.Complaint))]
        public string NotificationList { get; set; }

        [Editable(false)]
        [Display(Name = "complaintStatus", ResourceType = typeof(Localization.Complaint))]
        public int complaintStatus { get; set; }

        public DateTime regisetDate { get; set; }

        public string ComplaintStatusStr
        {
            get
            {
                string strStatus = "";

                switch (complaintStatus)
                {
                    case 0:
                        strStatus = Localization.Complaint.StatusDDL_NewComplaint;
                        break;
                    case 1:
                        strStatus = Localization.Complaint.StatusDDL_UnderInvestigation;
                        break;
                    case 2:
                        strStatus = Localization.Complaint.StatusDDL_Accepted;
                        break;
                    case 3:
                        strStatus = Localization.Complaint.StatusDDL_Rejected;
                        break;
                    default:
                        strStatus = complaintStatus.ToString();
                        break;
                }

                return strStatus;
            }
        }

        [DataType(DataType.Text)]
        [Display(Name = "makerID", ResourceType = typeof(Localization.Global))]
        public string makerID { get; set; }

        public override string ToString()
        {
            return String.Concat("complaintID: ", complaintID, "~ projectID: ", projectID, "~ projectName: ", projectName
                , "~ CRID: ", CRID, "~ CRName: ", CRName, "~ description: ", description, "~ comments: ", comments
                , "~ attachmentName: ", attachmentName, "~ attachmentPath: ", attachmentPath, "~ NotificationList: "
                , NotificationList, "~ complaintStatus: ", complaintStatus, "~ makerID: ", makerID);
        }
    }
}