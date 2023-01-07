using QA.Entities.Session_Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace QA.Entities.View_Entities
{
    public class Vw_Factory
    {
        /*               View Details                        */
        public Business_Entities.Ent_Factory factory { get; set; }

        public Session_Entities.Ent_UserSession userSession { get; set; }

        /*               List Of Values                       */
        public List<LOV> lOVfactoryTypes { get; set; }

        /*               Search Details                        */
        [DataType(DataType.Text)]
        [Display(Name = "name_Search", ResourceType = typeof(Localization.Factory))]
        public string searchName { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "description_Search", ResourceType = typeof(Localization.Factory))]
        public string searchDescription { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "AddressLine_Search", ResourceType = typeof(Localization.Factory))]
        public string searchAddressLine { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "FactoryPower_Search", ResourceType = typeof(Localization.Factory))]
        public string searchFactoryPower { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "factoryCode_Search", ResourceType = typeof(Localization.Factory))]
        public string searchFactoryCode { get; set; }

        [Display(Name = "factoryTypeID_Search", ResourceType = typeof(Localization.Factory))]
        public int searchfactoryTypeID { get; set; }

        [Display(Name = "boolIsActive", ResourceType = typeof(Localization.Global))]
        public int searchIsActive { get; set; }


        [Display(Name = "mixerCount", ResourceType = typeof(Localization.Factory))]
        public int mixerCount { get; set; }
    }
}