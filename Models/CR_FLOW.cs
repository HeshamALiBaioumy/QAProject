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
    
    public partial class CR_FLOW
    {
        public int FLOW_ID { get; set; }
        public Nullable<int> CRTF_ID { get; set; }
        public bool IS_SUPERVISOR_ENG { get; set; }
        public Nullable<System.DateTime> SUPERVISOR_DATE { get; set; }
        public Nullable<int> SUPERVISOR_ENG_ID { get; set; }
        public Nullable<System.DateTime> SUPERVISOR_FEEDBACK { get; set; }
        public bool IS_CONSULTANT { get; set; }
        public Nullable<System.DateTime> CONSULTANT_ASSIGN { get; set; }
        public Nullable<int> CONSULTANT_ID { get; set; }
        public Nullable<System.DateTime> CONSULTANT_FEEDBACK { get; set; }
        public bool IS_LAB { get; set; }
        public Nullable<System.DateTime> LAB_ASSIGN { get; set; }
        public Nullable<int> LAB_ID { get; set; }
        public Nullable<System.DateTime> LAB_FEEDBACK { get; set; }
        public bool MAKER { get; set; }
        public Nullable<System.DateTime> MAKER_DAT_TIM { get; set; }
    }
}
