using QA.Entities.Session_Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace QA.Entities.Business_Entities
{
    public class Ent_CheckList_Flow
    {
        public int cLGID { get; set; }

        public string cLGName { get; set; }

        public int cLItemID { get; set; }

        public string cLItemName { get; set; }

        [Range(minimum: 0, maximum: int.MaxValue, ErrorMessageResourceType = typeof(Localization.CheckListFlow_Master)
            , ErrorMessageResourceName = "Provide_Available_Status")]
        public int isCLItemAvailable { get; set; }

        public string strIsCLItemAvailable
        {
            get
            {
                string result = "NA - Error";

                switch (this.isCLItemAvailable)
                {
                    case 0:
                        result = @Localization.CheckListFlow_Master.CLF_Maker_IsAvailable_Available;
                        break;
                    case 1:
                        result = @Localization.CheckListFlow_Master.CLF_Maker_IsAvailable_NotAvailable;
                        break;
                }

                return result;
            }
        }

        [DataType(DataType.Text)]
        [Required(ErrorMessageResourceType = typeof(Localization.CheckListFlow_Master)
            , ErrorMessageResourceName = "Avalid_Comment")]
        [RegularExpression(RegexEnum.validText, ErrorMessageResourceType = typeof(Localization.Global)
            , ErrorMessageResourceName = "Special_Characters_Not_Allowed")]
        [StringLength(maximumLength: 250, MinimumLength = 2, ErrorMessageResourceType = typeof(Localization.CheckListFlow_Master)
            , ErrorMessageResourceName = "Comment_length_validation")]
        public string comment { get; set; }

        public string attachmentName { get; set; }

        public HttpPostedFileBase attachFile { get; set; }

        public string attachmentPath { get; set; }

        public bool hasAttachment { get; set; }

        public static List<LOV> fillCheckListMakerAvailableItems()
        {
            try
            {
                List<LOV> lstResult = new List<LOV>();
                lstResult.Add(new LOV() { id = -1, value = @Localization.CheckListFlow_Master.CLF_Maker_IsAvailable_Select });
                lstResult.Add(new LOV() { id = 0, value = @Localization.CheckListFlow_Master.CLF_Maker_IsAvailable_Available });
                lstResult.Add(new LOV() { id = 1, value = @Localization.CheckListFlow_Master.CLF_Maker_IsAvailable_NotAvailable });

                return lstResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public override string ToString()
        {
            return string.Concat("cLGID: ", cLGID, "~ cLGName: ", cLGName, "~ cLItemID: ", cLItemID
                , "~ cLItemName: ", cLItemName, "~ isCLItemAvailable: ", isCLItemAvailable, "~ comment: ", comment
                , "~ attachmentName: ", attachmentName, "~ hasAttachment: ", hasAttachment);
        }
    }
}