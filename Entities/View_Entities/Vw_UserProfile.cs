using QA.Entities.Session_Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace QA.Entities.View_Entities
{
    public class Vw_UserProfile
    {
        /*               View Details                        */
        public Business_Entities.Ent_UserProfile userProfile { get; set; }

        public Session_Entities.Ent_UserSession userSession { get; set; }

        public bool isAssistantUser { get; set; }

        /*               List Of Values                       */
        public List<LOV> lOVNationalityTypes { get; set; }

        public List<LOV> lOVNationalities { get; set; }

        public List<LOV> lOVSuperUsers { get; set; }

        public List<LOV> lOVRoles { get; set; }

        public List<LOV> lOVUserTypes { get; set; }

        public List<LOV> lOVProjectOwners { get; set; }

        /*               Search Details                        */
        [DataType(DataType.Text)]
        [Display(Name = "employeeName_Search", ResourceType = typeof(Localization.UserProfile))]
        public string searchEmployeeName { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "userName_Search", ResourceType = typeof(Localization.UserProfile))]
        public string searchUserName { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "employeeId_Search", ResourceType = typeof(Localization.UserProfile))]
        public string searchEmployeeId { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "NationalId_Search", ResourceType = typeof(Localization.UserProfile))]
        public string searchNationalId { get; set; }


        [Display(Name = "nationalityTypeID_Search", ResourceType = typeof(Localization.UserProfile))]
        public string searchNationalityTypeID { get; set; }

        [Display(Name = "nationalityID_Search", ResourceType = typeof(Localization.UserProfile))]
        public string searchNationalityID { get; set; }


        [Display(Name = "SuperUserID_Search", ResourceType = typeof(Localization.UserProfile))]
        public int searchSuperUserID { get; set; }

        [Display(Name = "UserTypeID_Search", ResourceType = typeof(Localization.UserProfile))]
        public int searchUserTypeID { get; set; }


        [Display(Name = "boolIsActive", ResourceType = typeof(Localization.Global))]
        public int searchIsActive { get; set; }

        [Display(Name = "boolIsLocked", ResourceType = typeof(Localization.UserProfile))]
        public int searchIsLocked { get; set; }

        [Display(Name = "boolIsAssistant", ResourceType = typeof(Localization.UserProfile))]
        public int searchIsAssistant { get; set; }
    }
}