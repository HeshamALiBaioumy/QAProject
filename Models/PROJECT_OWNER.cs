//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace QA.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class PROJECT_OWNER
    {
        public int PROJEC_OWNER_ID { get; set; }
        public Nullable<int> PROJ_OWN_TYP_ID { get; set; }
        public string NAME { get; set; }
        public string DESCRIPTION { get; set; }
        public bool IS_OWNER { get; set; }
        public string ACCOUNTABLE { get; set; }
        public Nullable<int> CONTACT_DETAILS_ID { get; set; }
        public bool IS_ACTIVE { get; set; }
        public string MAKER { get; set; }
        public Nullable<System.DateTime> MAKER_DAT_TIM { get; set; }
    }
}