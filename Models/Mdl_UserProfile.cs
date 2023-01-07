using QA.Entities.Business_Entities;
using QA.Entities.Session_Entities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OracleClient;
using System.Data.SqlClient;

namespace QA.Models
{
    public class Mdl_UserProfile
    {
        public ResponseMessage insert_updateUserProfile(Ent_UserProfile userProfile, bool isUpdateForm)
        {
            ResponseMessage response = new ResponseMessage();

            try
            {
                using (SqlConnection dbConnection = new SqlConnection(
                    ConfigurationManager.ConnectionStrings["dbConnectionString"].ConnectionString))
                using (SqlCommand dbCommand = dbConnection.CreateCommand())
                {
                    dbCommand.CommandText = "SP_USERS_PROFILE";
                    dbCommand.CommandType = CommandType.StoredProcedure;

                    dbCommand.Parameters.Add("INPUT_ID", SqlDbType.Int).Value = userProfile.UserProfileID;
                    dbCommand.Parameters.Add("I_USER_TYPE_ID", SqlDbType.Int).Value = userProfile.userTypeID;
                    dbCommand.Parameters.Add("I_NAME", SqlDbType.NVarChar).Value = userProfile.employeeName;
                    dbCommand.Parameters.Add("I_NATIONALITY_TYPE_ID", SqlDbType.Int).Value = userProfile.nationalityTypeID;
                    dbCommand.Parameters.Add("I_NATIONALITY_ID", SqlDbType.NVarChar).Value = userProfile.nationalityID;
                    dbCommand.Parameters.Add("I_NATIONAL_ID", SqlDbType.NVarChar).Value = userProfile.nationalId;
                    dbCommand.Parameters.Add("I_EMPLOYEE_ID", SqlDbType.NVarChar).Value = userProfile.employeeId;
                    dbCommand.Parameters.Add("I_SUPER_USER_ID", SqlDbType.Int).Value = userProfile.superUserID;
                    dbCommand.Parameters.Add("I_IS_PROJECTOWNER_MEMBER", SqlDbType.NVarChar).Value = (userProfile.isProjectOwnerMember) ? 'Y' : 'N';
                    dbCommand.Parameters.Add("I_PROJECTOWNER_ID", SqlDbType.Int).Value = userProfile.projectOwnerID;
                    dbCommand.Parameters.Add("I_PHONE_NUMBER", SqlDbType.NVarChar).Value = userProfile.contactDetails.phoneNumber ?? "";
                    dbCommand.Parameters.Add("I_WORK_PHONE_NUBER", SqlDbType.NVarChar).Value = userProfile.contactDetails.workPhoneNumber ?? "";
                    dbCommand.Parameters.Add("I_WORK_EXTENTION", SqlDbType.NVarChar).Value = userProfile.contactDetails.workExtNumber ?? "";
                    dbCommand.Parameters.Add("I_MOBILE_NUMBER", SqlDbType.NVarChar).Value = userProfile.contactDetails.mobileNumber;
                    dbCommand.Parameters.Add("I_EMAIL", SqlDbType.NVarChar).Value = userProfile.contactDetails.emailAddress;
                    dbCommand.Parameters.Add("I_FAX", SqlDbType.NVarChar).Value = userProfile.contactDetails.fax ?? "";
                    dbCommand.Parameters.Add("I_ADDRESS", SqlDbType.NVarChar).Value = userProfile.contactDetails.addressLine ?? "";
                    dbCommand.Parameters.Add("I_WORK_PLACE", SqlDbType.NVarChar).Value = userProfile.contactDetails.workPlace ?? "";

                    dbCommand.Parameters.Add("I_USER_NAME", SqlDbType.NVarChar).Value = userProfile.userName;
                    dbCommand.Parameters.Add("I_Hashed_Pass", SqlDbType.NVarChar).Value =
                        PasswordHandler.CreatePasswordHash(userProfile.password);
                    dbCommand.Parameters.Add("I_ROLE_ID", SqlDbType.Int).Value = userProfile.roleID;
                    dbCommand.Parameters.Add("I_EXPIRY_DATE", SqlDbType.DateTime).Value =
                        userProfile.expiryDate ?? (object)DBNull.Value;
                    dbCommand.Parameters.Add("I_IS_ACTIVE", SqlDbType.Char).Value = (userProfile.isActive) ? 'Y' : 'N';
                    dbCommand.Parameters.Add("I_IS_LOCKED", SqlDbType.Char).Value = (userProfile.isLocked) ? 'Y' : 'N';

                    dbCommand.Parameters.Add("I_MAKER", SqlDbType.NVarChar).Value = userProfile.makerID;
                    dbCommand.Parameters.Add("ACTION", SqlDbType.NVarChar).Value = (isUpdateForm) ? "UPDATE" : "NEW";

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

        public List<Ent_UserProfile> searchUserProfile(string searchEmployeeName, string searchUserName
            , string searchEmployeeId, string searchNationalId, string searchNationalityTypeID
            , string searchNationalityID, int searchSuperUserID, int searchUserTypeID
            , int searchIsActive, int searchIsLocked, int searchIsAssistant)
        {
            List<Ent_UserProfile> searchResult = new List<Ent_UserProfile>();

            try
            {
                using (SqlConnection dbConnection = new SqlConnection(
                    ConfigurationManager.ConnectionStrings["dbConnectionString"].ConnectionString))
                using (SqlCommand dbCommand = dbConnection.CreateCommand())
                {
                    dbCommand.CommandText = "SP_USERS_PROFILE_Search";
                    dbCommand.CommandType = CommandType.StoredProcedure;
                    dbCommand.Parameters.Add("I_NAME", SqlDbType.NVarChar).Value = searchEmployeeName ?? "";
                    dbCommand.Parameters.Add("I_USER_NAME", SqlDbType.NVarChar).Value = searchUserName ?? "";
                    dbCommand.Parameters.Add("I_EMPLOYEE_ID", SqlDbType.NVarChar).Value = searchEmployeeId ?? "";
                    dbCommand.Parameters.Add("I_NATIONAL_ID", SqlDbType.NVarChar).Value = searchNationalId ?? "";
                    dbCommand.Parameters.Add("I_NATIONALITY_TYPE_ID", SqlDbType.Int).Value = searchNationalityTypeID;
                    dbCommand.Parameters.Add("I_NATIONALITY_ID", SqlDbType.NVarChar).Value = searchNationalityID;
                    dbCommand.Parameters.Add("I_SUPER_USER_ID", SqlDbType.Int).Value = searchSuperUserID;
                    dbCommand.Parameters.Add("I_USER_TYPE_ID", SqlDbType.Int).Value = searchUserTypeID;
                    dbCommand.Parameters.Add("I_IS_ACTIVE", SqlDbType.NVarChar).Value = (searchIsActive == 0) ? 'Y' : (searchIsActive == 1) ? 'N' : 'E';
                    dbCommand.Parameters.Add("I_IS_LOCKED", SqlDbType.NVarChar).Value = (searchIsLocked == 0) ? 'Y' : (searchIsLocked == 1) ? 'N' : 'E';
                    dbCommand.Parameters.Add("I_IS_ASSISTANT", SqlDbType.NVarChar).Value = (searchIsAssistant == 0) ? 'Y' : (searchIsAssistant == 1) ? 'N' : 'E';
                    dbCommand.Parameters.Add("searchResultCursor", SqlDbType.Int).Direction = ParameterDirection.Output;

                    dbConnection.Open();
                    SqlDataReader reader = dbCommand.ExecuteReader();
                    while (reader.Read())
                    {
                        searchResult.Add(new Ent_UserProfile()
                        {
                            UserProfileID = reader.GetInt32(reader.GetOrdinal("PROFILE_ID")),
                            userName = reader.GetString(reader.GetOrdinal("USER_NAME")),
                            employeeName = reader.GetString(reader.GetOrdinal("NAME")),
                            userTypeID = reader.GetInt32(reader.GetOrdinal("USER_TYPE_ID")),
                            userTypeName = reader.GetString(reader.GetOrdinal("USER_TYPE_Name")),
                            roleID = reader.GetInt32(reader.GetOrdinal("ROLE_ID")),
                            roleName = reader.GetString(reader.GetOrdinal("ROLE_Name")),
                            nationalityTypeID = reader.GetInt32(reader.GetOrdinal("NATIONALITY_TYPE_ID")),
                            nationalityTypeName = Mdl_LOV.getNationalityTypeName(
                                reader.GetInt32(reader.GetOrdinal("NATIONALITY_TYPE_ID"))),
                            nationalityID = reader.GetString(reader.GetOrdinal("NATIONALITY_ID")),
                            nationalityName = reader.GetString(reader.GetOrdinal("NATIONALITY_Name")),
                            nationalId = reader.GetString(reader.GetOrdinal("NATIONAL_ID")),
                            employeeId = reader.GetString(reader.GetOrdinal("EMPLOYEE_ID")),
                            superUserID = (reader.IsDBNull(reader.GetOrdinal("SUPER_USER_ID")))
                                ? -1 : reader.GetInt32(reader.GetOrdinal("SUPER_USER_ID")),
                            superUserName = (reader.IsDBNull(reader.GetOrdinal("SUPER_USER_Name")))
                                ? "" : reader.GetString(reader.GetOrdinal("SUPER_USER_Name")),
                            projectOwnerID = (reader.IsDBNull(reader.GetOrdinal("PROJECTOWNER_ID")))
                                ? -1 : reader.GetInt32(reader.GetOrdinal("PROJECTOWNER_ID")),
                            projectOwnerName = (reader.IsDBNull(reader.GetOrdinal("PROJECTOWNER_Name")))
                                ? "" : reader.GetString(reader.GetOrdinal("PROJECTOWNER_Name")),
                            isAssistantUser = (reader.GetString(reader.GetOrdinal("IS_ASSISTANT")) == "Y") ? true : false,
                            isLocked = (reader.GetString(reader.GetOrdinal("IS_LOCKED")) == "Y") ? true : false,
                            expiryDate = (reader.IsDBNull(reader.GetOrdinal("EXPIRY_DATE")))
                                ? default(DateTime) : reader.GetDateTime(reader.GetOrdinal("EXPIRY_DATE")),
                            isActive = (reader.GetString(reader.GetOrdinal("IS_ACTIVE")) == "Y") ? true : false
                        });
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

            return searchResult;
        }

        public Ent_UserProfile viewUserProfile(int profileId)
        {
            Ent_UserProfile viewResult = new Ent_UserProfile();

            try
            {
                using (SqlConnection dbConnection = new SqlConnection(
                    ConfigurationManager.ConnectionStrings["dbConnectionString"].ConnectionString))
                using (SqlCommand dbCommand = dbConnection.CreateCommand())
                {
                    dbCommand.CommandText = "SP_USERS_PROFILE_VIEW";
                    dbCommand.CommandType = CommandType.StoredProcedure;
                    dbCommand.Parameters.Add("input_id", SqlDbType.NVarChar).Value = profileId;
                    dbCommand.Parameters.Add("viewResultCursor", SqlDbType.Int).Direction = ParameterDirection.Output;

                    dbConnection.Open();
                    SqlDataReader reader = dbCommand.ExecuteReader();
                    if (reader.Read())
                    {
                        viewResult = new Ent_UserProfile()
                        {
                            UserProfileID = reader.GetInt32(reader.GetOrdinal("PROFILE_ID")),
                            userName = reader.GetString(reader.GetOrdinal("USER_NAME")),
                            employeeName = reader.GetString(reader.GetOrdinal("NAME")),
                            userTypeID = reader.GetInt32(reader.GetOrdinal("USER_TYPE_ID")),
                            userTypeName = reader.GetString(reader.GetOrdinal("USER_TYPE_Name")),
                            roleID = reader.GetInt32(reader.GetOrdinal("ROLE_ID")),
                            roleName = reader.GetString(reader.GetOrdinal("ROLE_Name")),
                            nationalityTypeID = reader.GetInt32(reader.GetOrdinal("NATIONALITY_TYPE_ID")),
                            nationalityTypeName = Mdl_LOV.getNationalityTypeName(
                                reader.GetInt32(reader.GetOrdinal("NATIONALITY_TYPE_ID"))),
                            nationalityID = reader.GetString(reader.GetOrdinal("NATIONALITY_ID")),
                            nationalityName = reader.GetString(reader.GetOrdinal("NATIONALITY_Name")),
                            nationalId = reader.GetString(reader.GetOrdinal("NATIONAL_ID")),
                            employeeId = reader.GetString(reader.GetOrdinal("EMPLOYEE_ID")),
                            superUserID = (reader.IsDBNull(reader.GetOrdinal("SUPER_USER_ID")))
                                ? -1 : reader.GetInt32(reader.GetOrdinal("SUPER_USER_ID")),
                            superUserName = (reader.IsDBNull(reader.GetOrdinal("SUPER_USER_Name")))
                                ? "" : reader.GetString(reader.GetOrdinal("SUPER_USER_Name")),
                            projectOwnerID = (reader.IsDBNull(reader.GetOrdinal("PROJECTOWNER_ID")))
                                ? -1 : reader.GetInt32(reader.GetOrdinal("PROJECTOWNER_ID")),
                            projectOwnerName = (reader.IsDBNull(reader.GetOrdinal("PROJECTOWNER_Name")))
                                ? "" : reader.GetString(reader.GetOrdinal("PROJECTOWNER_Name")),
                            isAssistantUser = (reader.GetString(reader.GetOrdinal("IS_ASSISTANT")) == "Y") ? true : false,
                            isLocked = (reader.GetString(reader.GetOrdinal("IS_LOCKED")) == "Y") ? true : false,
                            expiryDate = (reader.IsDBNull(reader.GetOrdinal("EXPIRY_DATE")))
                                ? default(DateTime) : reader.GetDateTime(reader.GetOrdinal("EXPIRY_DATE")),
                            isActive = (reader.GetString(reader.GetOrdinal("IS_ACTIVE")) == "Y") ? true : false,

                            contactDetails = new Ent_ContactDetails()
                            {
                                CDID = reader.GetInt32(reader.GetOrdinal("CONTACT_DETAILS_ID")),
                                phoneNumber = (reader.IsDBNull(reader.GetOrdinal("PHONE_NUMBER")))
                                    ? "" : reader.GetString(reader.GetOrdinal("PHONE_NUMBER")),
                                workPhoneNumber = (reader.IsDBNull(reader.GetOrdinal("WORK_PHONE_NUBER")))
                                    ? "" : reader.GetString(reader.GetOrdinal("WORK_PHONE_NUBER")),
                                workExtNumber = (reader.IsDBNull(reader.GetOrdinal("WORK_EXTENTION")))
                                    ? "" : reader.GetString(reader.GetOrdinal("WORK_EXTENTION")),
                                mobileNumber = reader.GetString(reader.GetOrdinal("MOBILE_NUMBER")),
                                emailAddress = reader.GetString(reader.GetOrdinal("EMAIL")),
                                fax = (reader.IsDBNull(reader.GetOrdinal("FAX")))
                                    ? "" : reader.GetString(reader.GetOrdinal("FAX")),
                                addressLine = (reader.IsDBNull(reader.GetOrdinal("ADDRESS")))
                                    ? "" : reader.GetString(reader.GetOrdinal("ADDRESS")),
                                workPlace = (reader.IsDBNull(reader.GetOrdinal("WORK_PLACE")))
                                    ? "" : reader.GetString(reader.GetOrdinal("WORK_PLACE"))
                            }
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