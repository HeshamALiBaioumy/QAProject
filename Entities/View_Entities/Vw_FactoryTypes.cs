using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace QA.Entities.View_Entities
{
    public class Vw_FactoryTypes
    {
        public Business_Entities.Ent_FactoryTypes factoryType { get; set; }

        public Session_Entities.Ent_UserSession userSession { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Factory_type_name", ResourceType = typeof(Localization.FactoryType))]
        public string searchName { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Factory_type_Description", ResourceType = typeof(Localization.FactoryType))]
        public string searchDescription { get; set; }

        [Display(Name = "boolIsActive", ResourceType = typeof(Localization.Global))]
        public int searchIsActive { get; set; }
    }
}