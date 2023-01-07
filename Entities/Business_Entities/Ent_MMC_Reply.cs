using System;
using System.Collections.Generic;

namespace QA.Entities.Business_Entities
{
    public class Ent_MMC_Reply
    {
        public int ID { get; set; }

        public string strID
        {
            get
            {
                return (this.ID == -1) ? "xx" : this.ID.ToString();
            }
        }

        public int replyUserID { get; set; }

        public string replyUserName { get; set; }

        public DateTime replyDate { get; set; }

        public string strReplyDate
        {
            get
            {
                return (replyDate == default(DateTime)) ? "" : replyDate.ToString("dd/MM/yyyy HH:mm:ss");
            }
        }

        public string replyAction { get; set; }

        public string getReplyAction
        {
            get
            {
                string responseText = "";

                switch (this.replyAction)
                {
                    case "Reply_Accept":
                        responseText = Localization.MMC_Conversation.View_Replies_Action_Accept;
                        break;
                    case "Reply_Reject":
                        responseText = Localization.MMC_Conversation.View_Replies_Action_Reject;
                        break;
                    case "Close_Fixed":
                        responseText = Localization.MMC_Conversation.View_Replies_Action_Fixed;
                        break;
                    case "Close_Closed":
                        responseText = Localization.MMC_Conversation.View_Replies_Action_Closed;
                        break;
                    case "I":
                        responseText = Localization.MMC_Conversation.View_Replies_Action_CaseCreate;
                        break;
                    default:
                        responseText = this.replyAction;
                        break;
                }

                return responseText;
            }
        }

        public string replyMessage { get; set; }

        public List<string> lstAttachmentNames { get; set; }

        public List<string> lstAttachmentPaths { get; set; }

        public int getAttachmentsCount
        {
            get
            {
                return lstAttachmentNames.Count;
            }
        }

        public override string ToString()
        {
            return String.Concat("ID: ", ID, "~ replyUserID: ", replyUserID, "~ replyUserName: ", replyUserName
                , "~ replyDate: ", strReplyDate, "~ replyAction: ", replyAction, "~ replyMessage: ", replyMessage);
        }
    }
}