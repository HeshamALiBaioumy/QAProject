using QA.Entities.Session_Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace QA.Entities.View_Entities
{
    public class Vw_DepartmentSection
    {
        /*               View Details                        */
        public Business_Entities.Ent_DepartmentSection section { get; set; }

        public Session_Entities.Ent_UserSession userSession { get; set; }

        /*               List Of Values                       */
        public List<LOV> lOVDepartments { get; set; }

        /*               Search Details                        */
        [Display(Name = "departmentID_Search", ResourceType = typeof(Localization.DepartmentSection))]
        public int searchDepartmentID { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "name_Search", ResourceType = typeof(Localization.DepartmentSection))]
        public string searchName { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "description_Search", ResourceType = typeof(Localization.DepartmentSection))]
        public string searchDescription { get; set; }

        [Display(Name = "boolIsActive", ResourceType = typeof(Localization.Global))]
        public int searchIsActive { get; set; }
    }
}