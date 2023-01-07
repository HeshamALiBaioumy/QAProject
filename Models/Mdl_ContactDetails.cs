using QA.Entities.Business_Entities;
using QA.Entities.Session_Entities;
using System;
using System.Configuration;
using System.Data;
using System.Data.OracleClient;
using System.Data.SqlClient;

namespace QA.Models
{
    public class Mdl_ContactDetails
    {
        public ResponseMessage insert_updateContactDetails(Ent_ContactDetails contactDetails, bool isUpdateForm)
        {
            ResponseMessage response = new ResponseMessage();

            try
            {
                using (SqlConnection dbConnection = new SqlConnection(
                    ConfigurationManager.ConnectionStrings["dbConnectionString"].ConnectionString))
                using (SqlCommand dbCommand = dbConnection.CreateCommand())
                {
                    dbCommand.CommandText = "SP_CONTACT_ID";
                    dbCommand.CommandType = CommandType.StoredProcedure;
                    dbCommand.Parameters.Add("INPUT_ID", SqlDbType.Int).Value = contactDetails.CDID;
                    dbCommand.Parameters.Add("I_PHONE_NUMBER", SqlDbType.VarChar).Value = contactDetails.phoneNumber;
                    dbCommand.Parameters.Add("I_WORK_PHONE_NUBER", SqlDbType.VarChar).Value = contactDetails.workPhoneNumber;
                    dbCommand.Parameters.Add("I_WORK_EXTENTION", SqlDbType.VarChar).Value = contactDetails.workExtNumber;
                    dbCommand.Parameters.Add("I_MOBILE_NUMBER", SqlDbType.VarChar).Value = contactDetails.mobileNumber;
                    dbCommand.Parameters.Add("I_EMAIL", SqlDbType.VarChar).Value = contactDetails.emailAddress;
                    dbCommand.Parameters.Add("I_FAX", SqlDbType.VarChar).Value = contactDetails.fax;
                    dbCommand.Parameters.Add("I_ADDRESS", SqlDbType.VarChar).Value = contactDetails.addressLine;
                    dbCommand.Parameters.Add("I_WORK_PLACE", SqlDbType.VarChar).Value = contactDetails.workPlace;
                    dbCommand.Parameters.Add("ACTION", SqlDbType.NVarChar).Value = (isUpdateForm) ? "NEW" : "UPDATE";

                    dbConnection.Open();
                    dbCommand.ExecuteNonQuery();
                    dbCommand.Dispose();
                    dbConnection.Close();
                }

                response.responseStatus = true;
            }
            catch (Exception ex)
            {
                response.responseStatus = false;
                response.errorMessage = ex.Message;
                response.comments = ex.StackTrace;
                response.endUserMessage = Localization.ErrorMessages.ErrorWhileConnectingDBpleaseConsultAdmin;
            }

            return response;
        }
      
        public Ent_ContactDetails viewContactDetails(int ContactDetailsID)
        {
            Ent_ContactDetails viewResult = new Ent_ContactDetails();

            try
            {
                using (SqlConnection dbConnection = new SqlConnection(
                    ConfigurationManager.ConnectionStrings["dbConnectionString"].ConnectionString))
                using (SqlCommand dbCommand = dbConnection.CreateCommand())
                {
                    dbCommand.CommandText = "SP_CONTACT_ID_VIEW";
                    dbCommand.CommandType = CommandType.StoredProcedure;
                    dbCommand.Parameters.Add("input_id", SqlDbType.NVarChar).Value = ContactDetailsID;
                    dbCommand.Parameters.Add("viewResultCursor", SqlDbType.Int).Direction = ParameterDirection.Output;

                    dbConnection.Open();
                    SqlDataReader reader = dbCommand.ExecuteReader();
                    if (reader.Read())
                    {
                        viewResult = new Ent_ContactDetails()
                        {
                            CDID = reader.GetInt32(reader.GetOrdinal("CONTACT_ID")),
                            phoneNumber= reader.GetString(reader.GetOrdinal("PHONE_NUMBER")),
                            mobileNumber = reader.GetString(reader.GetOrdinal("MOBILE_NUMBER")),
                            emailAddress = reader.GetString(reader.GetOrdinal("EMAIL"))
                        };
                    }
                    reader.Close();
                    dbCommand.Dispose();
                    dbConnection.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return viewResult;
        }

    }
}