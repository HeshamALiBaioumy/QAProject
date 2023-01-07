using QA.Entities.Session_Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace QA.Entities.Business_Entities
{
    public class Ent_Comment
    {
        public int noteID { get; set; }

        public enum EnumNoteTypes { CR};

        public EnumNoteTypes noteType { get; set; }

        public int parentID { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "noteText", ResourceType = typeof(Localization.Note))]
        [RegularExpression(RegexEnum.validText, ErrorMessageResourceType = typeof(Localization.Global)
            , ErrorMessageResourceName = "Special_Characters_Not_Allowed")]
        [StringLength(maximumLength: 255, MinimumLength = 0, ErrorMessageResourceType = typeof(Localization.Department)
            , ErrorMessageResourceName = "Note_length_validation")]
        public string noteText { get; set; }

        public override string ToString()
        {
            return String.Concat("noteID: ", noteID, "~ noteType: ", noteType.ToString(), "~ parentID: ", parentID
                , "~ noteText: ", noteText);
        }
    }
}