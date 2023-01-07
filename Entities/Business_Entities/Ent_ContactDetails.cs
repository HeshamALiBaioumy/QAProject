using QA.Entities.Session_Entities;
using System;
using System.ComponentModel.DataAnnotations;

namespace QA.Entities.Business_Entities
{
    public class Ent_ContactDetails
    {
        [DataType(DataType.Text)]
        [Display(Name = "CDID", ResourceType = typeof(Localization.ContactDetails))]
        public int CDID { get; set; }

        [DataType(DataType.PhoneNumber)]
        [Display(Name = "phoneNumber", ResourceType = typeof(Localization.ContactDetails))]
        [RegularExpression(RegexEnum.numbers, ErrorMessageResourceType = typeof(Localization.Global)
            , ErrorMessageResourceName = "Numbers_Only_Field")]
        [StringLength(maximumLength: 20, MinimumLength = 5, ErrorMessageResourceType = typeof(Localization.ContactDetails)
            , ErrorMessageResourceName = "Number_length_5_20")]
        public string phoneNumber { get; set; }

        [DataType(DataType.PhoneNumber)]
        [Display(Name = "workPhoneNumber", ResourceType = typeof(Localization.ContactDetails))]
        [RegularExpression(RegexEnum.numbers, ErrorMessageResourceType = typeof(Localization.Global)
            , ErrorMessageResourceName = "Numbers_Only_Field")]
        [StringLength(maximumLength: 20, MinimumLength = 5, ErrorMessageResourceType = typeof(Localization.ContactDetails)
            , ErrorMessageResourceName = "Number_length_5_20")]
        public string workPhoneNumber { get; set; }

        [DataType(DataType.PhoneNumber)]
        [Display(Name = "workExtNumber", ResourceType = typeof(Localization.ContactDetails))]
        [RegularExpression(RegexEnum.numbers, ErrorMessageResourceType = typeof(Localization.Global)
            , ErrorMessageResourceName = "Numbers_Only_Field")]
        [StringLength(maximumLength: 20, MinimumLength = 5, ErrorMessageResourceType = typeof(Localization.ContactDetails)
            , ErrorMessageResourceName = "Number_length_5_20")]
        public string workExtNumber { get; set; }

        [DataType(DataType.PhoneNumber)]
        [Display(Name = "mobileNumber", ResourceType = typeof(Localization.ContactDetails))]
        [Required(ErrorMessageResourceType = typeof(Localization.ContactDetails), ErrorMessageResourceName = "valid_Mobile")]
        [RegularExpression(RegexEnum.numbers, ErrorMessageResourceType = typeof(Localization.Global)
            , ErrorMessageResourceName = "Numbers_Only_Field")]
        [StringLength(maximumLength: 20, MinimumLength = 5, ErrorMessageResourceType = typeof(Localization.ContactDetails)
            , ErrorMessageResourceName = "Number_length_5_20")]
        public string mobileNumber { get; set; }

        [DataType(DataType.EmailAddress)]
        [Display(Name = "emailAddress", ResourceType = typeof(Localization.ContactDetails))]
        [Required(ErrorMessageResourceType = typeof(Localization.ContactDetails), ErrorMessageResourceName = "valid_Email")]
        [RegularExpression(RegexEnum.email, ErrorMessageResourceType = typeof(Localization.Global)
            , ErrorMessageResourceName = "Email_Only_Field")]
        [StringLength(maximumLength: 50, MinimumLength = 5, ErrorMessageResourceType = typeof(Localization.ContactDetails)
            , ErrorMessageResourceName = "Email_Length_Validation")]
        public string emailAddress { get; set; }

        [DataType(DataType.PhoneNumber)]
        [Display(Name = "fax", ResourceType = typeof(Localization.ContactDetails))]
        [RegularExpression(RegexEnum.numbers, ErrorMessageResourceType = typeof(Localization.Global)
            , ErrorMessageResourceName = "Numbers_Only_Field")]
        [StringLength(maximumLength: 20, MinimumLength = 5, ErrorMessageResourceType = typeof(Localization.ContactDetails)
            , ErrorMessageResourceName = "Number_length_5_20")]
        public string fax { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "AddressLine", ResourceType = typeof(Localization.ContactDetails))]
        [RegularExpression(RegexEnum.validText, ErrorMessageResourceType = typeof(Localization.Global)
            , ErrorMessageResourceName = "Special_Characters_Not_Allowed")]
        [StringLength(maximumLength: 200, MinimumLength = 5, ErrorMessageResourceType = typeof(Localization.ContactDetails)
            , ErrorMessageResourceName = "Address_length_validation")]
        public string addressLine { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "workPlace", ResourceType = typeof(Localization.ContactDetails))]
        [RegularExpression(RegexEnum.validText, ErrorMessageResourceType = typeof(Localization.Global)
            , ErrorMessageResourceName = "Special_Characters_Not_Allowed")]
        [StringLength(maximumLength: 200, MinimumLength = 5, ErrorMessageResourceType = typeof(Localization.ContactDetails)
            , ErrorMessageResourceName = "Address_length_validation")]
        public string workPlace { get; set; }

        public override string ToString()
        {
            return String.Concat("Contact Details ID: ", CDID, "~ phoneNumber: ", phoneNumber, "~ workPhoneNumber: ", workPhoneNumber
                , "~ workExtNumber: ", workExtNumber, "~ mobileNumber: ", mobileNumber, "~ emailAddress: ", emailAddress
                , "~ fax: ", fax, "~ AddressLine: ", addressLine, "~ workPlace: ", workPlace);
        }
    }
}