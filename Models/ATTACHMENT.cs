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
    
    public partial class ATTACHMENT
    {
        public int ATTACHMENT_ID { get; set; }
        public string TYPE { get; set; }
        public Nullable<int> PARENT_ID { get; set; }
        public Nullable<int> PARENT_SUB_ID { get; set; }
        public string ATTACHEMENT_NAME { get; set; }
        public string ATTACHEMENT_PATH { get; set; }
        public string SAMPLE_CODE { get; set; }
        public Nullable<int> SAMPLE_TEST_ID { get; set; }
        public string SAMPLE_RESULT { get; set; }
        public Nullable<int> MAKERID { get; set; }
    }
}
