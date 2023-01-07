using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QA.Entities.Session_Entities
{
    public class RegexEnum
    {
        public const string validText = @"^[\u0600-\u065F\u066A-\u06EF\u06FA-\u06FFa-zA-Z|@]*[\u0600-\u065F\u066A-\u06EF\u06FA-\u06FFa-zA-Z-_ 0-9|@]*$";

        public const string numbers = @"^[0-9]+$";

        public const string email = @"^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?(?:\.[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?)*$";
    }
}