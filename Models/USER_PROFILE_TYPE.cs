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
    
    public partial class USER_PROFILE_TYPE
    {
        public int TYPE_ID { get; set; }
        public string TYPE_CODE { get; set; }
        public string NAME { get; set; }
        public string DESCRIPTION { get; set; }
        public string IS_ASSISTANT { get; set; }
        public Nullable<int> PARENT_TYPE_ID { get; set; }
        public bool IS_ACTIVE { get; set; }
        public string MAKER_ID { get; set; }
        public Nullable<System.DateTime> MAKER_DT_TIM { get; set; }
    }
}
