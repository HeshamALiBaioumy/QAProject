using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QA.Entities.Session_Entities
{
    public class ResponseMessage
    {
        public bool responseStatus { get; set; }

        public string errorMessage { get; set; }

        public string endUserMessage { get; set; }

        public string comments { get; set; }

        // Generic Used Field
        public string UDF { get; set; }

        public override string ToString()
        {
            return String.Concat("Status: ", responseStatus, "~ End user Message: ", endUserMessage
                , "~ technical Message: ", errorMessage, "~ More Details: ", comments);
        }
    }
}