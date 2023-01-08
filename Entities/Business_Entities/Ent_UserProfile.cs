using QA.Entities.Session_Entities;
using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace QA.Entities.Business_Entities
{
    public class Ent_UserProfile
    {
        [DataType(DataType.Text)]
        [Display(Name = "UserID", ResourceType = typeof(Localization.UserProfile))]
        public int UserProfileID { get; set; }

        /*               Step 1 Details                                  */
        [DataType(DataType.Text)]
        [Display(Name = "EmployeeName", ResourceType = typeof(Localization.UserProfile))]
        [Required(ErrorMessageResourceType = typeof(Localization.UserProfile)
            , ErrorMessageResourceName = "please_provide_avalid_UserName")]
        [RegularExpression(RegexEnum.validText, ErrorMessageResourceType = typeof(Localization.Global)
            , ErrorMessageResourceName = "Special_Characters_Not_Allowed")]
        [StringLength(maximumLength: 50, MinimumLength = 5, ErrorMessageResourceType = typeof(Localization.UserProfile)
            , ErrorMessageResourceName = "EmployeeName_length_Validation")]
        public string employeeName { get; set; }

        public int? contactId { get; set; }

        //[الرقم القومي, رقم الإقامة]
        [Display(Name = "nationalityTypeID", ResourceType = typeof(Localization.UserProfile))]
        [Range(minimum: 0, maximum: int.MaxValue, ErrorMessageResourceType = typeof(Localization.UserProfile)
            , ErrorMessageResourceName = "provide_avalid_Ntype")]
        public int nationalityTypeID { get; set; }

        public string nationalityTypeName { get; set; }

        [Display(Name = "nationalityID", ResourceType = typeof(Localization.UserProfile))]
        public string nationalityID { get; set; }

        public string nationalityName { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "NationalId", ResourceType = typeof(Localization.UserProfile))]
        [Required(ErrorMessageResourceType = typeof(Localization.UserProfile)
            , ErrorMessageResourceName = "provide_avalid_NID")]
        [RegularExpression(RegexEnum.validText, ErrorMessageResourceType = typeof(Localization.Global)
            , ErrorMessageResourceName = "Special_Characters_Not_Allowed")]
        [StringLength(maximumLength: 20, MinimumLength = 5, ErrorMessageResourceType = typeof(Localization.UserProfile)
            , ErrorMessageResourceName = "NID_length_Validation")]
        [Remote("IsUniqueNID", "UserProfile", HttpMethod = "POST", AdditionalFields = "UserProfileID"
            , ErrorMessageResourceType = typeof(Localization.UserProfile), ErrorMessageResourceName = "NIDAlreadyExist")]
        public string nationalId { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "employeeId", ResourceType = typeof(Localization.UserProfile))]
        [Required(ErrorMessageResourceType = typeof(Localization.UserProfile)
            , ErrorMessageResourceName = "provide_avalid_EmpID")]
        [RegularExpression(RegexEnum.validText, ErrorMessageResourceType = typeof(Localization.Global)
            , ErrorMessageResourceName = "Special_Characters_Not_Allowed")]
        [StringLength(maximumLength: 20, MinimumLength = 5, ErrorMessageResourceType = typeof(Localization.UserProfile)
            , ErrorMessageResourceName = "EmpID_length_validation")]
        [Remote("IsUniqueEmplyeeID", "UserProfile", HttpMethod = "POST", AdditionalFields = "UserProfileID"
            , ErrorMessageResourceType = typeof(Localization.UserProfile), ErrorMessageResourceName = "EmployeeIDAlreadyExist")]
        public string employeeId { get; set; }

        [Display(Name = "UserTypeID", ResourceType = typeof(Localization.UserProfile))]
        [Range(minimum: 0, maximum: int.MaxValue, ErrorMessageResourceType = typeof(Localization.UserProfile)
            , ErrorMessageResourceName = "provide_valid_Usertype")]
        public int userTypeID { get; set; }

        public string userTypeName { get; set; }

        [Display(Name = "boolIsAssistant", ResourceType = typeof(Localization.UserProfile))]
        public bool isAssistantUser { get; set; }

        [Display(Name = "superUserID", ResourceType = typeof(Localization.UserProfile))]
        [Range(minimum: 0, maximum: int.MaxValue, ErrorMessageResourceType = typeof(Localization.UserProfile)
            , ErrorMessageResourceName = "valid_SuperID")]
        public int superUserID { get; set; }

        public string superUserName { get; set; }

        [Display(Name = "isProjectOwnerMember", ResourceType = typeof(Localization.UserProfile))]
        public bool isProjectOwnerMember { get; set; }

        [Display(Name = "projectOwnerID", ResourceType = typeof(Localization.UserProfile))]
        [Range(minimum: 0, maximum: int.MaxValue, ErrorMessageResourceType = typeof(Localization.UserProfile)
            , ErrorMessageResourceName = "valid_projectOwner")]
        public int projectOwnerID { get; set; }

        public string projectOwnerName { get; set; }

        /*               Step 2 Contact Details                                  */
        public Ent_ContactDetails contactDetails { get; set; }

        /*               Step 3 Employee User Details                                  */
        [DataType(DataType.Text)]
        [Display(Name = "User_name", ResourceType = typeof(Localization.UserProfile))]
        [Required(ErrorMessageResourceType = typeof(Localization.UserProfile)
            , ErrorMessageResourceName = "please_provide_avalid_UserName")]
        [RegularExpression(RegexEnum.validText, ErrorMessageResourceType = typeof(Localization.Global)
            , ErrorMessageResourceName = "Special_Characters_Not_Allowed")]
        [StringLength(maximumLength: 30, MinimumLength = 5, ErrorMessageResourceType = typeof(Localization.UserProfile)
            , ErrorMessageResourceName = "Username_length_Validation")]
        [Remote("IsValidUserProfile", "UserProfile", HttpMethod = "POST", AdditionalFields = "UserProfileID"
            , ErrorMessageResourceType = typeof(Localization.UserProfile)
            , ErrorMessageResourceName = "UsernameAlreadyExist")]
        public string userName { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Password", ResourceType = typeof(Localization.UserProfile))]
        //[Required(ErrorMessageResourceType = typeof(Localization.UserProfile)
        //    , ErrorMessageResourceName = "please_provide_avalid_Password")]
        //[RegularExpression(RegexEnum.validText, ErrorMessageResourceType = typeof(Localization.Global)
        //    , ErrorMessageResourceName = "Special_Characters_Not_Allowed")]
        //[StringLength(maximumLength: 30, MinimumLength = 5, ErrorMessageResourceType = typeof(Localization.UserProfile)
        //    , ErrorMessageResourceName = "Password_length_Validation")]
        public string password { get; set; }

        /*               Step 3 Details                                  */
        [Display(Name = "UserRole", ResourceType = typeof(Localization.UserProfile))]
        [Range(minimum: 0, maximum: int.MaxValue, ErrorMessageResourceType = typeof(Localization.UserProfile)
            , ErrorMessageResourceName = "Provide_valid_RoleID")]
        public int roleID { get; set; }

        public string roleName { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "ExpiryDate", ResourceType = typeof(Localization.UserProfile))]
        public DateTime? expiryDate { get; set; }

        public string strExpiryDate
        {
            get
            {
                return (expiryDate == default(DateTime)) ? "" : ((DateTime)expiryDate).ToString("dd/MM/yyyy");
            }
        }

        [Display(Name = "boolIsActive", ResourceType = typeof(Localization.Global))]
        public bool isActive { get; set; }

        [Display(Name = "boolIsLocked", ResourceType = typeof(Localization.UserProfile))]
        public bool isLocked { get; set; }

        /*                Extra Details                                  */
        [DataType(DataType.Text)]
        [Display(Name = "makerID", ResourceType = typeof(Localization.Global))]
        public string makerID { get; set; }

        public override string ToString()
        {
            return String.Concat("UserProfileID: ", UserProfileID, "~ employeeName: ", employeeName, "~ contactDetails: ", contactDetails.ToString()
                , "~ nationalityTypeID: ", nationalityTypeID, "~ nationalityID: ", nationalityID, "~ nationalId: ", nationalId
                , "~ employeeId: ", employeeId, "~ superUserID: ", superUserID, "~ userName: ", userName, "~ password: ", password
                , "~ roleID: ", roleID, "~ userTypeID: ", userTypeID, "~ expiryDate: ", expiryDate, "~ isActive: ", isActive
                , "~ isLocked: ", isLocked, "~ makerID: ", makerID);
        }
    }
}