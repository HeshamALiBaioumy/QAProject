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
    
    public partial class CR_WORKFOW
    {
        public int CR_ID { get; set; }
        public Nullable<int> CONTRACTOR_ID { get; set; }
        public Nullable<System.DateTime> REGISTRATION_DATE { get; set; }
        public Nullable<int> SUPERVISORENG_ID { get; set; }
        public Nullable<System.DateTime> SUPERVISORENG_RECIEVEDATE { get; set; }
        public string SUPERVISORENG_FEEDBACK { get; set; }
        public Nullable<System.DateTime> SUPERVISORENG_FEEDBACK_DATE { get; set; }
        public Nullable<int> SUPERVISORENG_COMMENTID { get; set; }
        public Nullable<int> CONSULTANTENG_ID { get; set; }
        public Nullable<System.DateTime> CONSULTANTENG_RECIEVEDATE { get; set; }
        public string CONSULTANTENG_FEEDBACK { get; set; }
        public Nullable<System.DateTime> CONSULTANTENG_FEEDBACK_DATE { get; set; }
        public Nullable<int> CONSULTANTENG_COMMENTID { get; set; }
        public Nullable<bool> IS_REQUIRE_SAMPLE { get; set; }
        public Nullable<int> AUTHLAB_ID { get; set; }
        public Nullable<System.DateTime> AUTHLAB_RECIEVEDATE { get; set; }
        public string AUTHLAB_FEEDBACK { get; set; }
        public Nullable<System.DateTime> AUTHLAB_FEEDBACK_DATE { get; set; }
        public Nullable<int> AUTHLAB_COMMENTID { get; set; }
        public Nullable<int> CR_CURRENT_STATUS { get; set; }
        public Nullable<System.DateTime> CONTRACTOR_UPDATE_RECIEVEDATE { get; set; }
        public Nullable<System.DateTime> CONTRACTOR_UPDATE_UPDATEDATE { get; set; }
        public string REJECT_REASON { get; set; }
    }
}
