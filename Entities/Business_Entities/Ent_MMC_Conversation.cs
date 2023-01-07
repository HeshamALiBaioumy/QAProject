using QA.Entities.Session_Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;

namespace QA.Entities.Business_Entities
{
    public class Ent_MMC_Conversation
    {
        public int MMCID { get; set; }

        public string technicianName { get; set; }

        public string supervisorEngName { get; set; }

        public string QualityAssuranceEngName { get; set; }

        public string contractorAssName { get; set; }

        public bool allowedforReply { get; set; }

        public bool allowedforClosure { get; set; }

        public bool allowedforEscalation { get; set; }

        public List<Ent_MMC_Reply> repliesHistory { get; set; }

        public int replyUserID { get; set; }

        public enum feedbackActions { Reply_Accept, Reply_Reject, Close_Fixed, Close_Closed };

        public feedbackActions replyAction { get; set; }

        [Required(ErrorMessageResourceType = typeof(Localization.RCV_Missmatch)
            , ErrorMessageResourceName = "Reply_ReplyMessage_Val_Required")]
        [RegularExpression(RegexEnum.validText, ErrorMessageResourceType = typeof(Localization.Global)
            , ErrorMessageResourceName = "Special_Characters_Not_Allowed")]
        [StringLength(maximumLength: 1000, MinimumLength = 5, ErrorMessageResourceType = typeof(Localization.RCV_Missmatch)
            , ErrorMessageResourceName = "Reply_ReplyMessage_Val_Length")]
        [AllowHtml]
        public string replyMessage { get; set; }

        public List<HttpPostedFileBase> replyAttachments { get; set; }

        public List<string> lstAttachmentNames { get; set; }

        public List<string> lstAttachmentPaths { get; set; }

        public override string ToString()
        {
            return string.Concat("MMCID: ", MMCID, "~ technicianName: ", technicianName, "~ supervisorEngName: "
                , supervisorEngName, "~ QualityAssuranceEngName: ", QualityAssuranceEngName, "~ contractorAssName: ", contractorAssName
                , "~ allowedforReply: ", allowedforReply, "~ allowedforClosure: ", allowedforClosure, "~ allowedforEscalation: ", allowedforEscalation
                , "~ replyUserID: ", replyUserID, "~ replyAction: ", replyAction.ToString(), "~ replyMessage: ", replyMessage
                , "~ repliesHistory: ", (repliesHistory != null) ? repliesHistory.ToString() : ""
                , "~ replyAttachments: ", (replyAttachments != null) ? replyAttachments.ToString() : ""
                , "~ lstAttachmentNames: ", (lstAttachmentNames != null) ? lstAttachmentNames.ToString() : ""
                , "~ lstAttachmentPaths: ", (lstAttachmentPaths != null) ? lstAttachmentPaths.ToString() : "");
        }
    }
}