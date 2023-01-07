using QA.Entities.Session_Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QA.Entities.Business_Entities
{
    public class Ent_Note
    {
        public int ID { get; set; }

        // CR, ...
        public int noteType { get; set; }

        public int noteTypeID { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "noteText", ResourceType = typeof(Localization.Note))]
        [StringLength(maximumLength: 250, ErrorMessageResourceType = typeof(Localization.Note)
            , ErrorMessageResourceName = "Note_length_validation")]
        [RegularExpression(RegexEnum.validText, ErrorMessageResourceType = typeof(Localization.Global)
            , ErrorMessageResourceName = "Special_Characters_Not_Allowed")]
        public string noteText { get; set; }

        public override string ToString()
        {
            return String.Concat("ID: ", ID, "~ noteType: ", noteType, "~ noteTypeID: ", noteTypeID, "~ noteText: ", noteText);
        }
    }
}