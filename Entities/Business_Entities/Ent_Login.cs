using QA.Entities.Session_Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace QA.Entities.Business_Entities
{
    public class Ent_Login
    {
        public int userID;

        [DataType(DataType.Text)]
        [Display(Name = "UserName", ResourceType = typeof(Localization.Login))]
        [Required(ErrorMessageResourceType = typeof(Localization.Login)
            , ErrorMessageResourceName = "valid_Name")]
        [RegularExpression(RegexEnum.validText, ErrorMessageResourceType = typeof(Localization.Global)
            , ErrorMessageResourceName = "Special_Characters_Not_Allowed")]
        [StringLength(maximumLength: 50, MinimumLength = 3, ErrorMessageResourceType = typeof(Localization.Login)
            , ErrorMessageResourceName = "Name_length_validation")]
        public string userName { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Password", ResourceType = typeof(Localization.Login))]
        [Required(ErrorMessageResourceType = typeof(Localization.Login)
            , ErrorMessageResourceName = "valid_Password")]
        [RegularExpression(RegexEnum.validText, ErrorMessageResourceType = typeof(Localization.Global)
            , ErrorMessageResourceName = "Special_Characters_Not_Allowed")]
        [StringLength(maximumLength: 50, MinimumLength = 3, ErrorMessageResourceType = typeof(Localization.Login)
            , ErrorMessageResourceName = "Password_length_validation")]
        public string password { get; set; }

        public string errorMessage { get; set; }

        public override string ToString()
        {
            return String.Concat("userID: ", userID, "~ userName: ", userName, "~ password: *****"
                , "~ errorMessage: ", errorMessage);
        }
    }
}