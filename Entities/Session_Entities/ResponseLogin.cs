using QA.Entities.Business_Entities;
using System;

namespace QA.Entities.Session_Entities
{
    public class ResponseLogin
    {
        public int userID { get; set; }

        public Ent_UserRoles userRoles { get; set; }

        public bool responseStatus { get; set; }

        public string errorMessage { get; set; }

        public bool isInitialPassword { get; set; }

        public override string ToString()
        {
            return String.Concat("Login ID: ", userID, "~ Status: ", responseStatus
                , " ~ technical Message: ", errorMessage, " ~ isInitialPassword: ", isInitialPassword);
        }
    }
}