using QA.Entities.Business_Entities;
using QA.Entities.Session_Entities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OracleClient;
using System.Data.SqlClient;
using System.Linq;

namespace QA.Models
{
    public class Mdl_UserProfile
    {
        public QualityDbEntities ctx = new QualityDbEntities();
        public ResponseMessage insert_updateUserProfile(Ent_UserProfile userProfile, bool isUpdateForm)
        {
            ResponseMessage response = new ResponseMessage();

            try
            {
                if (isUpdateForm == false)
                {
                    var contact = new CONTACT_ID
                    {
                        WORK_PHONE_NUBER = userProfile.contactDetails.phoneNumber,
                        PHONE_int = userProfile.contactDetails.phoneNumber,
                        MOBILE_int = userProfile.contactDetails.mobileNumber,
                        WORK_EXTENTION = userProfile.contactDetails.workExtNumber,
                        EMAIL = userProfile.contactDetails.emailAddress,
                        FAX = userProfile.contactDetails.fax,
                        WORK_PLACE = userProfile.contactDetails.workExtNumber,
                        ADDRESS = userProfile.contactDetails.addressLine
                    };

                    ctx.CONTACT_ID.Add(contact);
                    ctx.SaveChanges();

                    var profile = new USERS_PROFILE
                    {
                        USER_TYPE_ID = userProfile.userTypeID,
                        NAME = userProfile.employeeName,
                        CONTACT_DETAILS_ID = contact.CONTACT_ID1,
                        NATIONALITY_TYPE_ID = userProfile.nationalityTypeID,
                        NATIONAL_ID = userProfile.nationalId,
                        NATIONALITY_ID = userProfile.nationalityID,
                        EMPLOYEE_ID = userProfile.employeeId,
                        SUPER_USER_ID = userProfile.superUserID,
                        IS_PROJECTOWNER_MEMBER = userProfile.isProjectOwnerMember,
                        PROJECTOWNER_ID = userProfile.projectOwnerID,
                        MAKER_ID = userProfile.makerID,
                        MAKER_DT_TIM = DateTime.Now,
                    };

                    ctx.USERS_PROFILE.Add(profile);
                    ctx.SaveChanges();

                    var logins = new USERS_LOGIN
                    {
                        PROFILE_ID = profile.PROFILE_ID,
                        USER_NAME = userProfile.userName,
                        HASHE_PASS = userProfile.password,
                        ROLE_ID = userProfile.roleID,
                        EXPIRY_DATE = userProfile.expiryDate,
                        IS_ACTIVE = userProfile.isActive,
                        RECORD_STAT = true,
                        INITIAL_STAT = true,
                        IS_LOCKED = userProfile.isLocked,
                        
                    };

                    ctx.USERS_LOGIN.Add(logins);
                    ctx.SaveChanges();
                }
                else
                {
                    var contact = ctx.CONTACT_ID.FirstOrDefault(s=>s.CONTACT_ID1 == userProfile.contactDetails.CDID);


                    contact.WORK_PHONE_NUBER = userProfile.contactDetails.phoneNumber;
                    contact.PHONE_int = userProfile.contactDetails.phoneNumber;
                    contact.MOBILE_int = userProfile.contactDetails.mobileNumber;
                    contact.WORK_EXTENTION = userProfile.contactDetails.workExtNumber;
                    contact.EMAIL = userProfile.contactDetails.emailAddress;
                    contact.FAX = userProfile.contactDetails.fax;
                    contact.WORK_PLACE = userProfile.contactDetails.workExtNumber;
                    contact.ADDRESS = userProfile.contactDetails.addressLine; 

                    ctx.SaveChanges();

                    var profile = ctx.USERS_PROFILE.FirstOrDefault(s=>s.PROFILE_ID == userProfile.UserProfileID);

                    profile.USER_TYPE_ID = userProfile.userTypeID;
                    profile.NAME = userProfile.employeeName;
                    profile.CONTACT_DETAILS_ID = contact.CONTACT_ID1;
                    profile.NATIONALITY_TYPE_ID = userProfile.nationalityTypeID;
                    profile.NATIONAL_ID = userProfile.nationalId;
                    profile.NATIONALITY_ID = userProfile.nationalityID;
                    profile.EMPLOYEE_ID = userProfile.employeeId;
                    profile.SUPER_USER_ID = userProfile.superUserID;
                    profile.IS_PROJECTOWNER_MEMBER = userProfile.isProjectOwnerMember;
                    profile.PROJECTOWNER_ID = userProfile.projectOwnerID;
                    profile.MAKER_ID = userProfile.makerID;
                    profile.MAKER_DT_TIM = DateTime.Now; 

                    ctx.SaveChanges();

                    var logins = ctx.USERS_LOGIN.FirstOrDefault(s => s.PROFILE_ID == userProfile.UserProfileID);

                    logins.PROFILE_ID = profile.PROFILE_ID;
                    logins.USER_NAME = userProfile.userName;
                    logins.HASHE_PASS = userProfile.password;
                    logins.ROLE_ID = userProfile.roleID;
                    logins.EXPIRY_DATE = userProfile.expiryDate;
                    logins.IS_ACTIVE = userProfile.isActive;
                    logins.RECORD_STAT = true;
                    logins.INITIAL_STAT = true;
                    logins.IS_LOCKED = userProfile.isLocked;

                    ctx.SaveChanges();
                }
            }
            catch (Exception ex)
            {

                response.responseStatus = false;
                response.errorMessage = ex.Message;
                response.comments = ex.StackTrace;
                response.endUserMessage = Localization.ErrorMessages.ErrorWhileConnectingDBpleaseConsultAdmin;
            }
            response.responseStatus = true;
            return response;
            //try
            //{
            //    using (SqlConnection dbConnection = new SqlConnection(
            //        ConfigurationManager.ConnectionStrings["dbConnectionString"].ConnectionString))
            //    using (SqlCommand dbCommand = dbConnection.CreateCommand())
            //    {
            //        dbCommand.CommandText = "SP_USERS_PROFILE";
            //        dbCommand.CommandType = CommandType.StoredProcedure;

            //        dbCommand.Parameters.Add("INPUT_ID", SqlDbType.Int).Value = userProfile.UserProfileID;
            //        dbCommand.Parameters.Add("I_USER_TYPE_ID", SqlDbType.Int).Value = userProfile.userTypeID;
            //        dbCommand.Parameters.Add("I_NAME", SqlDbType.NVarChar).Value = userProfile.employeeName;
            //        dbCommand.Parameters.Add("I_NATIONALITY_TYPE_ID", SqlDbType.Int).Value = userProfile.nationalityTypeID;
            //        dbCommand.Parameters.Add("I_NATIONALITY_ID", SqlDbType.NVarChar).Value = userProfile.nationalityID;
            //        dbCommand.Parameters.Add("I_NATIONAL_ID", SqlDbType.NVarChar).Value = userProfile.nationalId;
            //        dbCommand.Parameters.Add("I_EMPLOYEE_ID", SqlDbType.NVarChar).Value = userProfile.employeeId;
            //        dbCommand.Parameters.Add("I_SUPER_USER_ID", SqlDbType.Int).Value = userProfile.superUserID;
            //        dbCommand.Parameters.Add("I_IS_PROJECTOWNER_MEMBER", SqlDbType.NVarChar).Value = (userProfile.isProjectOwnerMember) ? 'Y' : 'N';
            //        dbCommand.Parameters.Add("I_PROJECTOWNER_ID", SqlDbType.Int).Value = userProfile.projectOwnerID;
            //        dbCommand.Parameters.Add("I_PHONE_NUMBER", SqlDbType.NVarChar).Value = userProfile.contactDetails.phoneNumber ?? "";
            //        dbCommand.Parameters.Add("I_WORK_PHONE_NUBER", SqlDbType.NVarChar).Value = userProfile.contactDetails.workPhoneNumber ?? "";
            //        dbCommand.Parameters.Add("I_WORK_EXTENTION", SqlDbType.NVarChar).Value = userProfile.contactDetails.workExtNumber ?? "";
            //        dbCommand.Parameters.Add("I_MOBILE_NUMBER", SqlDbType.NVarChar).Value = userProfile.contactDetails.mobileNumber;
            //        dbCommand.Parameters.Add("I_EMAIL", SqlDbType.NVarChar).Value = userProfile.contactDetails.emailAddress;
            //        dbCommand.Parameters.Add("I_FAX", SqlDbType.NVarChar).Value = userProfile.contactDetails.fax ?? "";
            //        dbCommand.Parameters.Add("I_ADDRESS", SqlDbType.NVarChar).Value = userProfile.contactDetails.addressLine ?? "";
            //        dbCommand.Parameters.Add("I_WORK_PLACE", SqlDbType.NVarChar).Value = userProfile.contactDetails.workPlace ?? "";

            //        dbCommand.Parameters.Add("I_USER_NAME", SqlDbType.NVarChar).Value = userProfile.userName;
            //        dbCommand.Parameters.Add("I_Hashed_Pass", SqlDbType.NVarChar).Value =
            //            PasswordHandler.CreatePasswordHash(userProfile.password);
            //        dbCommand.Parameters.Add("I_ROLE_ID", SqlDbType.Int).Value = userProfile.roleID;
            //        dbCommand.Parameters.Add("I_EXPIRY_DATE", SqlDbType.DateTime).Value =
            //            userProfile.expiryDate ?? (object)DBNull.Value;
            //        dbCommand.Parameters.Add("I_IS_ACTIVE", SqlDbType.Char).Value = (userProfile.isActive) ? 'Y' : 'N';
            //        dbCommand.Parameters.Add("I_IS_LOCKED", SqlDbType.Char).Value = (userProfile.isLocked) ? 'Y' : 'N';

            //        dbCommand.Parameters.Add("I_MAKER", SqlDbType.NVarChar).Value = userProfile.makerID;
            //        dbCommand.Parameters.Add("ACTION", SqlDbType.NVarChar).Value = (isUpdateForm) ? "UPDATE" : "NEW";

            //        dbConnection.Open();
            //        dbCommand.ExecuteNonQuery();
            //        dbCommand.Dispose();
            //        dbConnection.Close();
            //    }

            //    response.responseStatus = true;
            //}
            //catch (Exception ex)
            //{
            //    response.responseStatus = false;
            //    response.errorMessage = ex.Message;
            //    response.comments = ex.StackTrace;
            //    response.endUserMessage = Localization.ErrorMessages.ErrorWhileConnectingDBpleaseConsultAdmin;
            //}

            //return response;
        }

        public List<Ent_UserProfile> searchUserProfile(string searchEmployeeName, string searchUserName
            , string searchEmployeeId, string searchNationalId, string searchNationalityTypeID
            , string searchNationalityID, int searchSuperUserID, int searchUserTypeID
            , int searchIsActive, int searchIsLocked, int searchIsAssistant)
        {
            List<Ent_UserProfile> searchResult = new List<Ent_UserProfile>();

            try
            {
                var query = from PFL in ctx.USERS_PROFILE
                            join Lgn in ctx.USERS_LOGIN on PFL.PROFILE_ID equals Lgn.PROFILE_ID
                            join Typ in ctx.USER_PROFILE_TYPE on PFL.USER_TYPE_ID equals Typ.TYPE_ID
                            join rls in ctx.USER_ROLES on Lgn.ROLE_ID equals rls.ROLE_ID
                            join Cntrs in ctx.COUNTRIES on PFL.NATIONALITY_ID equals Cntrs.COUNTRY_CODE

                            select new Ent_UserProfile
                            {
                                UserProfileID = PFL.PROFILE_ID,
                                userName = Lgn.USER_NAME,
                                employeeName = PFL.NAME,
                                employeeId = PFL.EMPLOYEE_ID,
                                userTypeID = PFL.USER_TYPE_ID.HasValue ? PFL.USER_TYPE_ID.Value : 0,
                                userTypeName = Typ.NAME,
                                roleID = Lgn.ROLE_ID.HasValue? Lgn.ROLE_ID.Value:0,
                                roleName = rls.NAME,
                                nationalityTypeID = PFL.NATIONALITY_TYPE_ID.HasValue ? PFL.NATIONALITY_TYPE_ID.Value : 0,
                               // nationalityTypeName = PFL.NATIONALITY_TYPE_ID.HasValue ? PFL.NATIONALITY_TYPE_ID.Value.ToString() : "",
                                nationalityID = PFL.NATIONALITY_ID,
                                nationalityName = Cntrs.ENGLISHNAME,
                                nationalId = PFL.NATIONAL_ID,
                                superUserID = PFL.SUPER_USER_ID.HasValue ? PFL.SUPER_USER_ID.Value:0,
                              
                                superUserName = PFL.SUPER_USER_ID.HasValue ?
                                                        ctx.USERS_PROFILE.FirstOrDefault(s=>s.PROFILE_ID ==PFL.SUPER_USER_ID.Value).NAME:"",
                                projectOwnerID = PFL.PROJECTOWNER_ID.HasValue? PFL.PROJECTOWNER_ID.Value:0,
                                projectOwnerName = PFL.PROJECTOWNER_ID.HasValue?
                                                ctx.PROJECT_OWNER.FirstOrDefault(s=>s.PROJEC_OWNER_ID== PFL.PROJECTOWNER_ID.Value).NAME:"",
                                isAssistantUser = Typ.IS_ASSISTANT=="N"?false:true,
                                isLocked = Lgn.IS_LOCKED,
                                expiryDate = Lgn.EXPIRY_DATE,
                                isActive = Lgn.IS_ACTIVE,
                            };


                if (!string.IsNullOrEmpty(searchEmployeeName) && searchEmployeeName !="-1")
                {
                    searchEmployeeName = searchEmployeeName.ToLower();
                    query = query.Where(s => s.employeeName.ToLower().Contains(searchEmployeeName));
                }

                if (!string.IsNullOrEmpty(searchUserName) && searchUserName != "-1")
                {
                    searchUserName = searchUserName.ToLower();
                    query = query.Where(s => s.userName.ToLower().Contains(searchUserName));
                }

                if (!string.IsNullOrEmpty(searchEmployeeId) && searchEmployeeId != "-1")
                {
                    searchEmployeeId = searchEmployeeId.ToLower();
                    query = query.Where(s => s.employeeId.ToLower().Contains(searchEmployeeId));
                }

                if (!string.IsNullOrEmpty(searchNationalId) && searchNationalId != "-1")
                {
                    searchNationalId = searchNationalId.ToLower();
                    query = query.Where(s => s.nationalId.ToLower().Contains(searchNationalId));
                }

              

                if (!string.IsNullOrEmpty(searchNationalityID) && searchNationalityID != "-1")
                {
                    searchNationalityTypeID = searchNationalityID.ToLower();
                    query = query.Where(s => s.nationalityID.ToLower().Contains(searchNationalityTypeID));
                }

                //if (!string.IsNullOrEmpty(searchNationalityTypeID) && searchNationalityTypeID != "-1")
                //{
                //    var convert =int.Parse( searchNationalityTypeID);
                //    query = query.Where(s => s.nationalityTypeID == convert);
                //}
                if (searchIsAssistant >= 0)
                {
                    query = query.Where(s => s.superUserID == searchSuperUserID);
                }

                if (searchUserTypeID > 0)
                {
                    query = query.Where(s => s.userTypeID == searchUserTypeID);
                }

                if (searchIsActive > 0)
                {
                    query = query.Where(s => s.isActive == (searchIsActive ==1?true:false));
                }

                if (searchIsLocked >= 0)
                {
                    query = query.Where(s => s.isLocked == (searchIsLocked == 1 ? true : false));
                }

                if (searchIsAssistant >= 0)
                {
                    query = query.Where(s => s.isAssistantUser == (searchIsAssistant == 1 ? true : false));
                }


                searchResult = query.ToList();
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return searchResult;
            //try
            //{
            //    using (SqlConnection dbConnection = new SqlConnection(
            //        ConfigurationManager.ConnectionStrings["dbConnectionString"].ConnectionString))
            //    using (SqlCommand dbCommand = dbConnection.CreateCommand())
            //    {
            //        dbCommand.CommandText = "SP_USERS_PROFILE_Search";
            //        dbCommand.CommandType = CommandType.StoredProcedure;
            //        dbCommand.Parameters.Add("I_NAME", SqlDbType.NVarChar).Value = searchEmployeeName ?? "";
            //        dbCommand.Parameters.Add("I_USER_NAME", SqlDbType.NVarChar).Value = searchUserName ?? "";
            //        dbCommand.Parameters.Add("I_EMPLOYEE_ID", SqlDbType.NVarChar).Value = searchEmployeeId ?? "";
            //        dbCommand.Parameters.Add("I_NATIONAL_ID", SqlDbType.NVarChar).Value = searchNationalId ?? "";
            //        dbCommand.Parameters.Add("I_NATIONALITY_TYPE_ID", SqlDbType.Int).Value = searchNationalityTypeID;
            //        dbCommand.Parameters.Add("I_NATIONALITY_ID", SqlDbType.NVarChar).Value = searchNationalityID;
            //        dbCommand.Parameters.Add("I_SUPER_USER_ID", SqlDbType.Int).Value = searchSuperUserID;
            //        dbCommand.Parameters.Add("I_USER_TYPE_ID", SqlDbType.Int).Value = searchUserTypeID;
            //        dbCommand.Parameters.Add("I_IS_ACTIVE", SqlDbType.NVarChar).Value = (searchIsActive == 0) ? 'Y' : (searchIsActive == 1) ? 'N' : 'E';
            //        dbCommand.Parameters.Add("I_IS_LOCKED", SqlDbType.NVarChar).Value = (searchIsLocked == 0) ? 'Y' : (searchIsLocked == 1) ? 'N' : 'E';
            //        dbCommand.Parameters.Add("I_IS_ASSISTANT", SqlDbType.NVarChar).Value = (searchIsAssistant == 0) ? 'Y' : (searchIsAssistant == 1) ? 'N' : 'E';
            //        dbCommand.Parameters.Add("searchResultCursor", SqlDbType.Int).Direction = ParameterDirection.Output;

            //        dbConnection.Open();
            //        SqlDataReader reader = dbCommand.ExecuteReader();
            //        while (reader.Read())
            //        {
            //            searchResult.Add(new Ent_UserProfile()
            //            {
            //                UserProfileID = reader.GetInt32(reader.GetOrdinal("PROFILE_ID")),
            //                userName = reader.GetString(reader.GetOrdinal("USER_NAME")),
            //                employeeName = reader.GetString(reader.GetOrdinal("NAME")),
            //                userTypeID = reader.GetInt32(reader.GetOrdinal("USER_TYPE_ID")),
            //                userTypeName = reader.GetString(reader.GetOrdinal("USER_TYPE_Name")),
            //                roleID = reader.GetInt32(reader.GetOrdinal("ROLE_ID")),
            //                roleName = reader.GetString(reader.GetOrdinal("ROLE_Name")),
            //                nationalityTypeID = reader.GetInt32(reader.GetOrdinal("NATIONALITY_TYPE_ID")),
            //                nationalityTypeName = Mdl_LOV.getNationalityTypeName(
            //                    reader.GetInt32(reader.GetOrdinal("NATIONALITY_TYPE_ID"))),
            //                nationalityID = reader.GetString(reader.GetOrdinal("NATIONALITY_ID")),
            //                nationalityName = reader.GetString(reader.GetOrdinal("NATIONALITY_Name")),
            //                nationalId = reader.GetString(reader.GetOrdinal("NATIONAL_ID")),
            //                employeeId = reader.GetString(reader.GetOrdinal("EMPLOYEE_ID")),
            //                superUserID = (reader.IsDBNull(reader.GetOrdinal("SUPER_USER_ID")))
            //                    ? -1 : reader.GetInt32(reader.GetOrdinal("SUPER_USER_ID")),
            //                superUserName = (reader.IsDBNull(reader.GetOrdinal("SUPER_USER_Name")))
            //                    ? "" : reader.GetString(reader.GetOrdinal("SUPER_USER_Name")),
            //                projectOwnerID = (reader.IsDBNull(reader.GetOrdinal("PROJECTOWNER_ID")))
            //                    ? -1 : reader.GetInt32(reader.GetOrdinal("PROJECTOWNER_ID")),
            //                projectOwnerName = (reader.IsDBNull(reader.GetOrdinal("PROJECTOWNER_Name")))
            //                    ? "" : reader.GetString(reader.GetOrdinal("PROJECTOWNER_Name")),
            //                isAssistantUser = (reader.GetString(reader.GetOrdinal("IS_ASSISTANT")) == "Y") ? true : false,
            //                isLocked = (reader.GetString(reader.GetOrdinal("IS_LOCKED")) == "Y") ? true : false,
            //                expiryDate = (reader.IsDBNull(reader.GetOrdinal("EXPIRY_DATE")))
            //                    ? default(DateTime) : reader.GetDateTime(reader.GetOrdinal("EXPIRY_DATE")),
            //                isActive = (reader.GetString(reader.GetOrdinal("IS_ACTIVE")) == "Y") ? true : false
            //            });
            //        }
            //        reader.Close();
            //        dbCommand.Dispose();
            //        dbConnection.Close();
            //    }
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}

           
        }

        public Ent_UserProfile viewUserProfile(int profileId)
        {
            Ent_UserProfile viewResult = new Ent_UserProfile();

            try
            {
                var query = from PFL in ctx.USERS_PROFILE
                            join Lgn in ctx.USERS_LOGIN on PFL.PROFILE_ID equals Lgn.PROFILE_ID
                            join Typ in ctx.USER_PROFILE_TYPE on PFL.USER_TYPE_ID equals Typ.TYPE_ID
                            join rls in ctx.USER_ROLES on Lgn.ROLE_ID equals rls.ROLE_ID
                            join Cntrs in ctx.COUNTRIES on PFL.NATIONALITY_ID equals Cntrs.COUNTRY_CODE
                            where PFL.PROFILE_ID == profileId

                            select new Ent_UserProfile
                            {
                                UserProfileID = PFL.PROFILE_ID,
                                userName = Lgn.USER_NAME,
                                employeeId = PFL.EMPLOYEE_ID,
                                userTypeID = PFL.USER_TYPE_ID.HasValue ? PFL.USER_TYPE_ID.Value : 0,
                                userTypeName = Typ.NAME,
                                roleID = Lgn.ROLE_ID.HasValue ? Lgn.ROLE_ID.Value : 0,
                                roleName = rls.NAME,
                                nationalityTypeID = PFL.NATIONALITY_TYPE_ID.HasValue ? PFL.NATIONALITY_TYPE_ID.Value : 0,
                                nationalityTypeName = PFL.NATIONALITY_TYPE_ID.HasValue ? PFL.NATIONALITY_TYPE_ID.Value.ToString() : "",
                                nationalityID = PFL.NATIONALITY_ID,
                                nationalityName = Cntrs.ENGLISHNAME,
                                nationalId = PFL.NATIONAL_ID,
                                superUserID = PFL.SUPER_USER_ID.HasValue ? PFL.SUPER_USER_ID.Value : 0,

                                superUserName = PFL.SUPER_USER_ID.HasValue ?
                                                        ctx.USERS_PROFILE.FirstOrDefault(s => s.PROFILE_ID == PFL.SUPER_USER_ID.Value).NAME : "",
                                projectOwnerID = PFL.PROJECTOWNER_ID.HasValue ? PFL.PROJECTOWNER_ID.Value : 0,
                                projectOwnerName = PFL.PROJECTOWNER_ID.HasValue ?
                                                ctx.PROJECT_OWNER.FirstOrDefault(s => s.PROJEC_OWNER_ID == PFL.PROJECTOWNER_ID.Value).NAME : "",
                                isAssistantUser = Typ.IS_ASSISTANT == "N" ? false : true,
                                isLocked = Lgn.IS_LOCKED,
                                expiryDate = Lgn.EXPIRY_DATE,
                                isActive = Lgn.IS_ACTIVE,
                                contactId = PFL.CONTACT_DETAILS_ID,
                            };

                viewResult = query.FirstOrDefault();
                var contactDetails = ctx.CONTACT_ID.FirstOrDefault(s=>s.CONTACT_ID1 == viewResult.contactId);
                if (contactDetails !=null)
                {
                    viewResult.contactDetails = new Ent_ContactDetails
                    {
                        CDID = contactDetails.CONTACT_ID1,
                        phoneNumber = contactDetails.PHONE_int,
                        workPhoneNumber = contactDetails.WORK_PHONE_NUBER,
                        mobileNumber = contactDetails.MOBILE_int,
                        emailAddress = contactDetails.EMAIL,
                        fax = contactDetails.FAX,
                        addressLine = contactDetails.ADDRESS,
                        workPlace = contactDetails.WORK_PLACE,
                    };

                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return viewResult;
            //try
            //{
            //    using (SqlConnection dbConnection = new SqlConnection(
            //        ConfigurationManager.ConnectionStrings["dbConnectionString"].ConnectionString))
            //    using (SqlCommand dbCommand = dbConnection.CreateCommand())
            //    {
            //        dbCommand.CommandText = "SP_USERS_PROFILE_VIEW";
            //        dbCommand.CommandType = CommandType.StoredProcedure;
            //        dbCommand.Parameters.Add("input_id", SqlDbType.NVarChar).Value = profileId;
            //        dbCommand.Parameters.Add("viewResultCursor", SqlDbType.Int).Direction = ParameterDirection.Output;

            //        dbConnection.Open();
            //        SqlDataReader reader = dbCommand.ExecuteReader();
            //        if (reader.Read())
            //        {
            //            viewResult = new Ent_UserProfile()
            //            {
            //                UserProfileID = reader.GetInt32(reader.GetOrdinal("PROFILE_ID")),
            //                userName = reader.GetString(reader.GetOrdinal("USER_NAME")),
            //                employeeName = reader.GetString(reader.GetOrdinal("NAME")),
            //                userTypeID = reader.GetInt32(reader.GetOrdinal("USER_TYPE_ID")),
            //                userTypeName = reader.GetString(reader.GetOrdinal("USER_TYPE_Name")),
            //                roleID = reader.GetInt32(reader.GetOrdinal("ROLE_ID")),
            //                roleName = reader.GetString(reader.GetOrdinal("ROLE_Name")),
            //                nationalityTypeID = reader.GetInt32(reader.GetOrdinal("NATIONALITY_TYPE_ID")),
            //                nationalityTypeName = Mdl_LOV.getNationalityTypeName(
            //                    reader.GetInt32(reader.GetOrdinal("NATIONALITY_TYPE_ID"))),
            //                nationalityID = reader.GetString(reader.GetOrdinal("NATIONALITY_ID")),
            //                nationalityName = reader.GetString(reader.GetOrdinal("NATIONALITY_Name")),
            //                nationalId = reader.GetString(reader.GetOrdinal("NATIONAL_ID")),
            //                employeeId = reader.GetString(reader.GetOrdinal("EMPLOYEE_ID")),
            //                superUserID = (reader.IsDBNull(reader.GetOrdinal("SUPER_USER_ID")))
            //                    ? -1 : reader.GetInt32(reader.GetOrdinal("SUPER_USER_ID")),
            //                superUserName = (reader.IsDBNull(reader.GetOrdinal("SUPER_USER_Name")))
            //                    ? "" : reader.GetString(reader.GetOrdinal("SUPER_USER_Name")),
            //                projectOwnerID = (reader.IsDBNull(reader.GetOrdinal("PROJECTOWNER_ID")))
            //                    ? -1 : reader.GetInt32(reader.GetOrdinal("PROJECTOWNER_ID")),
            //                projectOwnerName = (reader.IsDBNull(reader.GetOrdinal("PROJECTOWNER_Name")))
            //                    ? "" : reader.GetString(reader.GetOrdinal("PROJECTOWNER_Name")),
            //                isAssistantUser = (reader.GetString(reader.GetOrdinal("IS_ASSISTANT")) == "Y") ? true : false,
            //                isLocked = (reader.GetString(reader.GetOrdinal("IS_LOCKED")) == "Y") ? true : false,
            //                expiryDate = (reader.IsDBNull(reader.GetOrdinal("EXPIRY_DATE")))
            //                    ? default(DateTime) : reader.GetDateTime(reader.GetOrdinal("EXPIRY_DATE")),
            //                isActive = (reader.GetString(reader.GetOrdinal("IS_ACTIVE")) == "Y") ? true : false,

            //                contactDetails = new Ent_ContactDetails()
            //                {
            //                    CDID = reader.GetInt32(reader.GetOrdinal("CONTACT_DETAILS_ID")),
            //                    phoneNumber = (reader.IsDBNull(reader.GetOrdinal("PHONE_NUMBER")))
            //                        ? "" : reader.GetString(reader.GetOrdinal("PHONE_NUMBER")),
            //                    workPhoneNumber = (reader.IsDBNull(reader.GetOrdinal("WORK_PHONE_NUBER")))
            //                        ? "" : reader.GetString(reader.GetOrdinal("WORK_PHONE_NUBER")),
            //                    workExtNumber = (reader.IsDBNull(reader.GetOrdinal("WORK_EXTENTION")))
            //                        ? "" : reader.GetString(reader.GetOrdinal("WORK_EXTENTION")),
            //                    mobileNumber = reader.GetString(reader.GetOrdinal("MOBILE_NUMBER")),
            //                    emailAddress = reader.GetString(reader.GetOrdinal("EMAIL")),
            //                    fax = (reader.IsDBNull(reader.GetOrdinal("FAX")))
            //                        ? "" : reader.GetString(reader.GetOrdinal("FAX")),
            //                    addressLine = (reader.IsDBNull(reader.GetOrdinal("ADDRESS")))
            //                        ? "" : reader.GetString(reader.GetOrdinal("ADDRESS")),
            //                    workPlace = (reader.IsDBNull(reader.GetOrdinal("WORK_PLACE")))
            //                        ? "" : reader.GetString(reader.GetOrdinal("WORK_PLACE"))
            //                }
            //            };
            //        }
            //        reader.Close();
            //        dbCommand.Dispose();
            //        dbConnection.Close();
            //    }
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}


        }
    }
}