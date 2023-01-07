using QA.Entities.Session_Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace QA.Entities.View_Entities
{
    public class Vw_Mixer
    {
        public Business_Entities.Ent_Mixer mixer { get; set; }

        public Session_Entities.Ent_UserSession userSession { get; set; }

        /*               List Of Values                       */
        public List<LOV> lOVMixerTypes { get; set; }

        public List<LOV> lOVFactories { get; set; }

        /*               Search Details                        */
        [DataType(DataType.Text)]
        [Display(Name = "name_Search", ResourceType = typeof(Localization.Mixer))]
        public string searchName { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "description_Search", ResourceType = typeof(Localization.Mixer))]
        public string searchDescription { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "MixerCode_Search", ResourceType = typeof(Localization.Mixer))]
        public string searchMixerCode { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "mixerModel_Search", ResourceType = typeof(Localization.Mixer))]
        public string searchMixerModel { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "mixerPower_Search", ResourceType = typeof(Localization.Mixer))]
        public string searchMixerPower { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "mixer_status_Search", ResourceType = typeof(Localization.Mixer))]
        public string searchMixerStatus { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "factoryID_Search", ResourceType = typeof(Localization.Mixer))]
        public int searchfactoryID { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "MixerTypeID_Search", ResourceType = typeof(Localization.Mixer))]
        public int searchMixerTypeID { get; set; }

        [Display(Name = "boolIsActive", ResourceType = typeof(Localization.Global))]
        public int searchIsActive { get; set; }
    }
}