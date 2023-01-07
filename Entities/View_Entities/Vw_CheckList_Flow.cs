using QA.Entities.Business_Entities;
using QA.Entities.Session_Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace QA.Entities.View_Entities
{
    public class Vw_CheckList_Flow
    {
        public int cLFlowID { get; set; }

        public int cLID { get; set; }

        [Editable(false)]
        [DataType(DataType.Text)]
        [Display(Name = "CLParty", ResourceType = typeof(Localization.CheckListFlow_Master))]
        public string CLParty { get; set; }

        [Editable(false)]
        [DataType(DataType.Date)]
        [Display(Name = "registrationDate", ResourceType = typeof(Localization.CheckListFlow_Master))]
        public DateTime registrationDate { get; set; }

        public string strRegistrationDate
        {
            get
            {
                
                return (registrationDate == default(DateTime)) ? "" : registrationDate.ToString("dd/MM/yyyy");
            }
        }

        public List<LOV> lOVCLItemsAvailable { get; set; }

        public List<Ent_CheckList_Flow> lstClItems { get; set; }

        public string makerID { get; set; }

        public override string ToString()
        {
            return String.Concat("cLFlowID: ", cLFlowID, "~ cLID: ", cLID, "~ CLParty: ", CLParty
                , "~ registrationDate: ", strRegistrationDate
                , "~ lstClItems: ", (lstClItems != null) ? lstClItems.ToString() : "", "~ makerID: ", makerID);
        }
    }
}