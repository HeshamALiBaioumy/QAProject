using QA.Entities.Session_Entities;
using System;
using System.ComponentModel.DataAnnotations;

namespace QA.Entities.Business_Entities
{
    public class Ent_CR_Sample
    {
        public int sampleID { get; set; }

        public int CRID { get; set; }

        [Display(Name = "sampleMaker", ResourceType = typeof(Localization.CR_Sample))]
        [Required(ErrorMessageResourceType = typeof(Localization.CR_Sample)
            , ErrorMessageResourceName = "provide_sampleMaker")]
        [RegularExpression(RegexEnum.validText, ErrorMessageResourceType = typeof(Localization.Global)
            , ErrorMessageResourceName = "Special_Characters_Not_Allowed")]
        [StringLength(maximumLength: 50, MinimumLength = 3, ErrorMessageResourceType = typeof(Localization.CR_Sample)
            , ErrorMessageResourceName = "sampleMaker_length_validation")]
        public string sampleMaker { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "sampleSize", ResourceType = typeof(Localization.CR_Sample))]
        [Required(ErrorMessageResourceType = typeof(Localization.CR_Sample)
            , ErrorMessageResourceName = "provide_sampleSize")]
        //[RegularExpression(RegexEnum.numbers, ErrorMessageResourceType = typeof(Localization.Global)
            //, ErrorMessageResourceName = "Numbers_Only_Field")]
        [StringLength(maximumLength: 10, MinimumLength = 1, ErrorMessageResourceType = typeof(Localization.CR_Sample)
            , ErrorMessageResourceName = "sampleSize_length_validation")]
        public string sampleSize { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "sampleLength", ResourceType = typeof(Localization.CR_Sample))]
        [Required(ErrorMessageResourceType = typeof(Localization.CR_Sample)
            , ErrorMessageResourceName = "provide_sampleLength")]
        //[RegularExpression(RegexEnum.numbers, ErrorMessageResourceType = typeof(Localization.Global)
        //    , ErrorMessageResourceName = "Numbers_Only_Field")]
        [StringLength(maximumLength: 10, MinimumLength = 1, ErrorMessageResourceType = typeof(Localization.CR_Sample)
            , ErrorMessageResourceName = "sampleLength_length_val")]
        public string sampleLength { get; set; }

        [Display(Name = "sampleUnit", ResourceType = typeof(Localization.CR_Sample))]
        [Range(minimum: 0, maximum: int.MaxValue, ErrorMessageResourceType = typeof(Localization.CR_Sample)
            , ErrorMessageResourceName = "provide_sampleUnit")]
        public int sampleUnitID { get; set; }

        public string sampleUnitName { get; set; }

        public Ent_MapSelection mapSelection { get; set; }

        public override string ToString()
        {
            return String.Concat("sampleID: ", sampleID, "~ CRID: ", CRID, "~ sampleMakerID: ", sampleMaker
                , "~ sampleSize: ", sampleSize, "~ sampleLength: ", sampleLength, "~ sampleUnit: ", sampleUnitID);
        }
    }
}