using QA.Entities.Session_Entities;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QA.Entities.Business_Entities
{
    public class Ent_Login_ResetPassword
    {
        public int userID;

        [DataType(DataType.Password)]
        [Display(Name = "Password", ResourceType = typeof(Localization.Login))]
        [Required(ErrorMessageResourceType = typeof(Localization.Login)
            , ErrorMessageResourceName = "valid_Password")]
        [RegularExpression(RegexEnum.validText, ErrorMessageResourceType = typeof(Localization.Global)
            , ErrorMessageResourceName = "Special_Characters_Not_Allowed")]
        [StringLength(maximumLength: 50, MinimumLength = 3, ErrorMessageResourceType = typeof(Localization.Login)
            , ErrorMessageResourceName = "Password_length_validation")]
        public string password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "ConfirmPassword", ResourceType = typeof(Localization.Login))]
        [NotMapped]
        [CompareAttribute("password", ErrorMessageResourceType = typeof(Localization.Login)
            , ErrorMessageResourceName = "ResetPassword_PasswordMissmatch")]
        [Required(ErrorMessageResourceType = typeof(Localization.Login)
            , ErrorMessageResourceName = "valid_Password")]
        [RegularExpression(RegexEnum.validText, ErrorMessageResourceType = typeof(Localization.Global)
            , ErrorMessageResourceName = "Special_Characters_Not_Allowed")]
        [StringLength(maximumLength: 50, MinimumLength = 3, ErrorMessageResourceType = typeof(Localization.Login)
            , ErrorMessageResourceName = "Password_length_validation")]
        public string confirmPassword { get; set; }

        public string errorMessage { get; set; }

        public override string ToString()
        {
            return String.Concat("userID: ", userID, "~ password: *****"
                , "~ errorMessage: ", errorMessage);
        }
    }
}