using QA.Entities.Session_Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace QA.Entities.Business_Entities
{
    public class Ent_Attachment
    {
        public int ID { get; set; }

        public enum EnumAttachmentTypes { CR };

        public EnumAttachmentTypes attachType { get; set; }

        public int parentID { get; set; }

        public int stepID { get; set; }

        [Display(Name = "sampleCode", ResourceType = typeof(Localization.Attachment))]
        [Required(ErrorMessageResourceType = typeof(Localization.Attachment)
            , ErrorMessageResourceName = "avalid_Sample_Code")]
        [RegularExpression(RegexEnum.validText, ErrorMessageResourceType = typeof(Localization.Global)
            , ErrorMessageResourceName = "Special_Characters_Not_Allowed")]
        [StringLength(maximumLength: 20, MinimumLength = 1, ErrorMessageResourceType = typeof(Localization.Attachment)
            , ErrorMessageResourceName = "SampleCode_length_validation")]
        public string sampleCode { get; set; }

        [Display(Name = "sampleTestCategoryID", ResourceType = typeof(Localization.Attachment))]
        [Range(minimum: 0, maximum: int.MaxValue, ErrorMessageResourceType = typeof(Localization.Attachment)
        , ErrorMessageResourceName = "provide_SampleTestCategory")]
        public int sampleTestCategoryID { get; set; }

        public string sampleTestCategoryName { get; set; }

        [Display(Name = "sampleTestID", ResourceType = typeof(Localization.Attachment))]
        [Range(minimum: 0, maximum: int.MaxValue, ErrorMessageResourceType = typeof(Localization.Attachment)
            , ErrorMessageResourceName = "provide_SampleTest")]
        public int sampleTestID { get; set; }

        public string sampleTestName { get; set; }

        [Display(Name = "attachmentName", ResourceType = typeof(Localization.Attachment))]
        public string attachmentName { get; set; }

        [Required(ErrorMessageResourceType = typeof(Localization.Attachment)
            , ErrorMessageResourceName = "SelectFileValidation")]
        public HttpPostedFileBase attachFile { get; set; }

        public string attachmentPath { get; set; }

        public bool hasAttachment { get; set; }

        [Display(Name = "sampleResult", ResourceType = typeof(Localization.Attachment))]
        [Range(minimum: 0, maximum: int.MaxValue, ErrorMessageResourceType = typeof(Localization.Attachment)
            , ErrorMessageResourceName = "provide_SampleTest_Result")]
        public int sampleResult { get; set; }

        public string sampleResultName
        {
            get
            {
                string resultName = "";
                switch (sampleResult)
                {
                    case 0:
                        resultName = Localization.Attachment.SampleResult_Accepted;
                        break;
                    case 1:
                        resultName = Localization.Attachment.SampleResult_Rejected;
                        break;
                    default:
                        resultName = "Error";
                        break;
                }
                return resultName;
            }
        }

        public string makerID { get; set; }

        public string makerName { get; set; }

        public override string ToString()
        {
            return String.Concat("ID: ", ID, "~ attachType: ", attachType.ToString(), "~ parentID: ", parentID
                , "~ stepID: ", stepID, "~ attachmentName: ", attachmentName);
        }

        /// ////////////////////////// LOV Parameters 

        public List<LOV> lOVSampleTestResult { get; set; }

        public List<LOV> lOVSampleTestCategories { get; set; }

        public List<LOV> lOVSampleTests { get; set; }
    }
}