using QA.Entities.Session_Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace QA.Entities.View_Entities
{
    public class Vw_CR_Report_ProjectCrs
    {
        /*               View Details                        */

        /*               Search Details                        */
        [Display(Name = "Report_SearchProjCr_Project", ResourceType = typeof(Localization.CR))]
        [Required(ErrorMessageResourceType = typeof(Localization.CR)
            , ErrorMessageResourceName = "Report_SearchProjCr_provide_avalid_Project")]
        [Range(minimum: 0, maximum: int.MaxValue, ErrorMessageResourceType = typeof(Localization.CR)
            , ErrorMessageResourceName = "Report_SearchProjCr_provide_avalid_Project")]
        public int searchProjectID { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Report_SearchProjCr_Date_From", ResourceType = typeof(Localization.CR))]
        [Required(ErrorMessageResourceType = typeof(Localization.CR)
            , ErrorMessageResourceName = "provide_avalid_FromDate")]
        public DateTime searchCRDateFrom { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Report_SearchProjCr_Date_To", ResourceType = typeof(Localization.CR))]
        [Required(ErrorMessageResourceType = typeof(Localization.CR)
            , ErrorMessageResourceName = "provide_avalid_ToDate")]
        public DateTime searchCRDateTo { get; set; }

        /*               List Of Values                       */
        public List<LOV> lOVProjects { get; set; }
    }
}