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
    public class Mdl_RCVMissmatch
    {
        public Ent_RCV_Missmatch viewRCVMissmatch(int RCVID, int userID)
        {
            Ent_RCV_Missmatch viewResult = null;

            try
            {
                using (SqlConnection dbConnection = new SqlConnection(
                    ConfigurationManager.ConnectionStrings["dbConnectionString"].ConnectionString))
                using (SqlCommand dbCommand = dbConnection.CreateCommand())
                {
                    dbCommand.CommandText = "SP_RCVMissmatch_ViewPending";
                    dbCommand.CommandType = CommandType.StoredProcedure;
                    dbCommand.Parameters.Add("I_RCV_ID", SqlDbType.Int).Value = RCVID;
                    dbCommand.Parameters.Add("I_User_ID", SqlDbType.Int).Value = userID;
                    dbCommand.Parameters.Add("searchResultCursor", SqlDbType.Int).Direction = ParameterDirection.Output;
                    dbCommand.Parameters.Add("viewAttachmentsCursor", SqlDbType.Int).Direction = ParameterDirection.Output;
                    dbCommand.Parameters.Add("viewProjectMapPointsCursor", SqlDbType.Int).Direction = ParameterDirection.Output;
                    dbCommand.Parameters.Add("viewCRMapPointsCursor", SqlDbType.Int).Direction = ParameterDirection.Output;

                    dbConnection.Open();
                    SqlDataReader reader = dbCommand.ExecuteReader(CommandBehavior.CloseConnection);
                    if (reader.Read())
                    {
                        viewResult = new Ent_RCV_Missmatch()
                        {
                            RCVID = reader.GetInt32(reader.GetOrdinal("ID")),
                            projectID = reader.GetInt32(reader.GetOrdinal("PROJECT_ID")),
                            projectName = reader.GetString(reader.GetOrdinal("PROJECT_Name")),
                            CRID = reader.GetInt32(reader.GetOrdinal("CR_ID")),
                            CRRegistrationDate = reader.GetDateTime(reader.GetOrdinal("CR_REGISTER_DATE")),
                            RCVAssignDate = reader.GetDateTime(reader.GetOrdinal("ASSIGN_DATE")),
                            comments = (reader.IsDBNull(reader.GetOrdinal("COMMENTS")) ? ""
                                : reader.GetString(reader.GetOrdinal("COMMENTS"))),
                            isLabRequired = (reader.GetString(reader.GetOrdinal("IS_SAMPLE_REQUIRED")) == "Y") ? true : false,
                            mapProject = new Ent_MapSelection()
                            {
                                zoomLevel = reader.GetInt32(reader.GetOrdinal("ZOOM_LEVEL"))
                            },
                            mapCR = new Ent_MapSelection()
                            {
                                projectMapSelectionType = Ent_MapSelection.getDrawType
                                    (reader.GetString(reader.GetOrdinal("SELECTION_TYPE"))),
                                mapPoints = new List<Ent_MapPoint>()
                            }
                        };

                        viewResult.lstAttachmentNames = new List<string>();
                        viewResult.lstAttachmentPaths = new List<string>();
                        reader.NextResult();
                        while (reader.Read())
                        {
                            viewResult.lstAttachmentNames.Add(reader.GetString(reader.GetOrdinal("ATTACHEMENT_NAME")));
                            viewResult.lstAttachmentPaths.Add(reader.GetString(reader.GetOrdinal("ATTACHEMENT_PATH")));
                        }

                        reader.NextResult();
                        viewResult.mapProject.mapPoints = new List<Ent_MapPoint>();
                        while (reader.Read())
                        {
                            viewResult.mapProject.mapPoints.Add(new Ent_MapPoint()
                            {
                                pointLatitude = reader.GetString(reader.GetOrdinal("LATITUDE")),
                                pointLongitude = reader.GetString(reader.GetOrdinal("LONGITUDE")),
                            });
                        }

                        reader.NextResult();
                        while (reader.Read())
                        {
                            viewResult.mapCR.mapPoints.Add(new Ent_MapPoint()
                            {
                                pointLatitude = reader.GetString(reader.GetOrdinal("LATITUDE")),
                                pointLongitude = reader.GetString(reader.GetOrdinal("LONGITUDE")),
                            });
                        }
                    }

                    reader.Close();
                    dbCommand.Dispose();
                    dbConnection.Close();
                }
            }
            catch (Exception ex)
            {
                string x = ex.Message.Substring(0, 9);
                if (ex.Message.Substring(0, 9).Equals("ORA-24338"))
                {
                    return null;
                }
                else
                {
                    throw ex;
                }
            }

            return viewResult;
        }

        public Ent_RCV_Missmatch viewProjectMissmatch(int projectID)
        {
            Ent_RCV_Missmatch viewResult = null;

            try
            {
                using (SqlConnection dbConnection = new SqlConnection(
                    ConfigurationManager.ConnectionStrings["dbConnectionString"].ConnectionString))
                using (SqlCommand dbCommand = dbConnection.CreateCommand())
                {
                    dbCommand.CommandText = "SP_RCVMissmatch_ViewProject";
                    dbCommand.CommandType = CommandType.StoredProcedure;
                    dbCommand.Parameters.Add("I_Project_ID", SqlDbType.Int).Value = projectID;
                    dbCommand.Parameters.Add("searchResultCursor", SqlDbType.Int).Direction = ParameterDirection.Output;
                    dbCommand.Parameters.Add("viewProjectMapPointsCursor", SqlDbType.Int).Direction = ParameterDirection.Output;

                    dbConnection.Open();
                    SqlDataReader reader = dbCommand.ExecuteReader(CommandBehavior.CloseConnection);
                    if (reader.Read())
                    {
                        viewResult = new Ent_RCV_Missmatch()
                        {
                            RCVID = -1,
                            projectID = reader.GetInt32(reader.GetOrdinal("PROJECT_ID")),
                            projectName = reader.GetString(reader.GetOrdinal("PROJECT_Name")),
                            isLabRequired = (reader.GetString(reader.GetOrdinal("IS_SAMPLE_REQUIRED")) == "Y") ? true : false,
                            mapProject = new Ent_MapSelection()
                            {
                                zoomLevel = reader.GetInt32(reader.GetOrdinal("ZOOM_LEVEL"))
                            },
                        };

                        viewResult.lstAttachmentNames = new List<string>();
                        viewResult.lstAttachmentPaths = new List<string>();

                        reader.NextResult();
                        viewResult.mapProject.mapPoints = new List<Ent_MapPoint>();
                        while (reader.Read())
                        {
                            viewResult.mapProject.mapPoints.Add(new Ent_MapPoint()
                            {
                                pointLatitude = reader.GetString(reader.GetOrdinal("LATITUDE")),
                                pointLongitude = reader.GetString(reader.GetOrdinal("LONGITUDE")),
                            });
                        }
                    }

                    reader.Close();
                    dbCommand.Dispose();
                    dbConnection.Close();
                }
            }
            catch (Exception ex)
            {
                string x = ex.Message.Substring(0, 9);
                if (ex.Message.Substring(0, 9).Equals("ORA-24338"))
                {
                    return null;
                }
                else
                {
                    throw ex;
                }
            }

            return viewResult;
        }

        public ResponseMessage insert_updateRCVMissmatch(Ent_RCV_Missmatch RCVMMCase, bool isRCVCase, bool isUpdateForm)
        {
            ResponseMessage response = new ResponseMessage();

            try
            {
                using (SqlConnection dbConnection = new SqlConnection(
                    ConfigurationManager.ConnectionStrings["dbConnectionString"].ConnectionString))
                using (SqlCommand dbCommand = dbConnection.CreateCommand())
                {
                    dbCommand.CommandText = "SP_RCVMM";
                    dbCommand.CommandType = CommandType.StoredProcedure;
                    OracleParameter paramID = new OracleParameter("I_ID", SqlDbType.Int);
                    paramID.Value = RCVMMCase.ID;
                    paramID.Direction = ParameterDirection.InputOutput;
                    dbCommand.Parameters.Add(paramID);

                    dbCommand.Parameters.Add("I_Is_RCV_Case", SqlDbType.NVarChar).Value = (isRCVCase) ? 'Y' : 'N';
                    dbCommand.Parameters.Add("I_RCV_ID", SqlDbType.Int).Value = RCVMMCase.RCVID;
                    dbCommand.Parameters.Add("I_Project_ID", SqlDbType.Int).Value = RCVMMCase.projectID;

                    dbCommand.Parameters.Add("I_CASE_DESC", SqlDbType.NVarChar).Value = RCVMMCase.caseDescription;

                    /////////////////////// Sample 1 Details //////////////////////////////
                    dbCommand.Parameters.Add("I_SAMPLE1_MAKER", SqlDbType.NVarChar).Value =
                        (RCVMMCase.sample1.sampleMaker == null) ? "" : RCVMMCase.sample1.sampleMaker;
                    dbCommand.Parameters.Add("I_SAMPLE1_SIZE", SqlDbType.NVarChar).Value =
                        (RCVMMCase.sample1.sampleMaker == null) ? "" : RCVMMCase.sample1.sampleSize;
                    dbCommand.Parameters.Add("I_SAMPLE1_LENGTH", SqlDbType.NVarChar).Value =
                        (RCVMMCase.sample1.sampleMaker == null) ? "" : RCVMMCase.sample1.sampleLength;
                    dbCommand.Parameters.Add("I_SAMPLE1_UNIT_ID", SqlDbType.Int).Value =
                        (RCVMMCase.sample1.sampleMaker == null) ? -1 : RCVMMCase.sample1.sampleUnitID;

                    /////////////////////// Map 1 Details //////////////////////////////
                    dbCommand.Parameters.Add("I_MAP1_Lat", SqlDbType.NVarChar).Value =
                        (RCVMMCase.mapSample1.mapPoints == null) ? "" : RCVMMCase.mapSample1.mapPoints[0].pointLatitude;
                    dbCommand.Parameters.Add("I_MAP1_Long", SqlDbType.NVarChar).Value =
                        (RCVMMCase.mapSample1.mapPoints == null) ? "" : RCVMMCase.mapSample1.mapPoints[0].pointLongitude;

                    /////////////////////// Sample 2 Details //////////////////////////////
                    dbCommand.Parameters.Add("I_SAMPLE2_MAKER", SqlDbType.NVarChar).Value =
                        (RCVMMCase.sample2.sampleMaker == null) ? "" : RCVMMCase.sample2.sampleMaker;
                    dbCommand.Parameters.Add("I_SAMPLE2_SIZE", SqlDbType.NVarChar).Value =
                        (RCVMMCase.sample2.sampleMaker == null) ? "" : RCVMMCase.sample2.sampleSize;
                    dbCommand.Parameters.Add("I_SAMPLE2_LENGTH", SqlDbType.NVarChar).Value =
                        (RCVMMCase.sample2.sampleMaker == null) ? "" : RCVMMCase.sample2.sampleLength;
                    dbCommand.Parameters.Add("I_SAMPLE2_UNIT_ID", SqlDbType.Int).Value =
                        (RCVMMCase.sample2.sampleMaker == null) ? -1 : RCVMMCase.sample2.sampleUnitID;

                    /////////////////////// Map 2 Details //////////////////////////////
                    dbCommand.Parameters.Add("I_MAP2_Lat", SqlDbType.NVarChar).Value =
                        (RCVMMCase.mapSample2.mapPoints == null) ? "" : RCVMMCase.mapSample2.mapPoints[0].pointLatitude;
                    dbCommand.Parameters.Add("I_MAP2_Long", SqlDbType.NVarChar).Value =
                        (RCVMMCase.mapSample2.mapPoints == null) ? "" : RCVMMCase.mapSample2.mapPoints[0].pointLongitude;

                    dbCommand.Parameters.Add("I_MAKER", SqlDbType.NVarChar).Value = RCVMMCase.makerID;
                    dbCommand.Parameters.Add("ACTION", SqlDbType.NVarChar).Value = (isUpdateForm) ? "UPDATE" : "NEW";

                    dbConnection.Open();
                    dbCommand.ExecuteNonQuery();

                    if (!(isUpdateForm))
                    {
                        response.UDF = dbCommand.Parameters["I_ID"].Value.ToString();
                    }

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

        public List<Ent_RCV_Missmatch> searchMissmatchCases(int searchRCVID, int searchCRID, int searchProjectID
            , int searchProfileID, int searchRCVMMStatus, int searchPendingOn, int userID)
        {
            List<Ent_RCV_Missmatch> searchResult = new List<Ent_RCV_Missmatch>();

            try
            {
                using (SqlConnection dbConnection = new SqlConnection(
                    ConfigurationManager.ConnectionStrings["dbConnectionString"].ConnectionString))
                using (SqlCommand dbCommand = dbConnection.CreateCommand())
                {
                    dbCommand.CommandText = "SP_RCVMM_Search";
                    dbCommand.CommandType = CommandType.StoredProcedure;
                    dbCommand.Parameters.Add("I_RCV_ID", SqlDbType.Int).Value = searchRCVID;
                    dbCommand.Parameters.Add("I_CR_ID", SqlDbType.Int).Value = searchCRID;
                    dbCommand.Parameters.Add("I_Project_ID", SqlDbType.Int).Value = searchProjectID;
                    dbCommand.Parameters.Add("I_Profile_ID", SqlDbType.Int).Value = searchProfileID;
                    dbCommand.Parameters.Add("I_STATUS_ID", SqlDbType.Int).Value = searchRCVMMStatus;
                    dbCommand.Parameters.Add("I_Pending_On_ID", SqlDbType.Int).Value = searchPendingOn;
                    dbCommand.Parameters.Add("I_User_ID", SqlDbType.Int).Value = userID;
                    dbCommand.Parameters.Add("searchResultCursor", SqlDbType.Int).Direction = ParameterDirection.Output;

                    dbConnection.Open();
                    SqlDataReader reader = dbCommand.ExecuteReader();
                    while (reader.Read())
                    {
                        searchResult.Add(new Ent_RCV_Missmatch()
                        {
                            ID = reader.GetInt32(reader.GetOrdinal("MMC_ID")),
                            isRCVCase = (reader.GetString(reader.GetOrdinal("Is_RCV_Case")) == "Y") ? true : false,
                            RCVID = reader.GetInt32(reader.GetOrdinal("RCV_ID")),
                            CRID = (reader.IsDBNull(reader.GetOrdinal("CR_ID")) ? -1
                                : reader.GetInt32(reader.GetOrdinal("CR_ID"))),
                            CRRegistrationDate = (reader.IsDBNull(reader.GetOrdinal("REGISTER_DATE")) ? default(DateTime)
                                : reader.GetDateTime(reader.GetOrdinal("REGISTER_DATE"))),
                            projectID = reader.GetInt32(reader.GetOrdinal("PROJECT_ID")),
                            projectName = reader.GetString(reader.GetOrdinal("PROJECT_Name")),
                            projectItemID = (reader.IsDBNull(reader.GetOrdinal("PROJECT_ITEM_ID")) ? -1
                                : reader.GetInt32(reader.GetOrdinal("PROJECT_ITEM_ID"))),
                            projectItemName = (reader.IsDBNull(reader.GetOrdinal("PROJECT_ITEM_Name")) ? ""
                                : reader.GetString(reader.GetOrdinal("PROJECT_ITEM_Name"))),
                            cRStatusID = (reader.IsDBNull(reader.GetOrdinal("CR_STATUS")) ? -1
                                : reader.GetInt32(reader.GetOrdinal("CR_STATUS"))),
                            cRStatusName = (reader.IsDBNull(reader.GetOrdinal("CR_CURRENT_STATUS_Name")) ? ""
                                : reader.GetString(reader.GetOrdinal("CR_CURRENT_STATUS_Name"))),
                            caseCreateDate = reader.GetDateTime(reader.GetOrdinal("CASE_CREATE_DATETIME")),
                            comments = (reader.IsDBNull(reader.GetOrdinal("COMMENTS")) ? ""
                                : reader.GetString(reader.GetOrdinal("COMMENTS"))),
                            caseDescription = reader.GetString(reader.GetOrdinal("CASE_DESC")),
                            assignUserID = reader.GetInt32(reader.GetOrdinal("MAKER_Id")),
                            assignUserName = reader.GetString(reader.GetOrdinal("MAKER_Name")),
                            status = reader.GetInt32(reader.GetOrdinal("STATUS_ID")),
                            statusName = (reader.IsDBNull(reader.GetOrdinal("RCVMM_Status_Name")) ? ""
                                : reader.GetString(reader.GetOrdinal("RCVMM_Status_Name"))),
                            pendingOn = reader.GetInt32(reader.GetOrdinal("PENDING_ON_ID")),
                            pendingOnName = (reader.IsDBNull(reader.GetOrdinal("PENDING_ON_Name")) ? ""
                                : reader.GetString(reader.GetOrdinal("PENDING_ON_Name"))),
                            caseCloseDate = (reader.IsDBNull(reader.GetOrdinal("CASE_CLOSE_DATETIME")) ? default(DateTime)
                                : reader.GetDateTime(reader.GetOrdinal("CASE_CLOSE_DATETIME"))),
                            allowedforEdit = (reader.GetString(reader.GetOrdinal("Allowed_For_Edit")) == "Y") ? true : false
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

        public Ent_RCV_Missmatch viewRCVMMCase(int RCVMMID, int userID)
        {
            Ent_RCV_Missmatch viewResult = new Ent_RCV_Missmatch();

            try
            {
                using (SqlConnection dbConnection = new SqlConnection(
                    ConfigurationManager.ConnectionStrings["dbConnectionString"].ConnectionString))
                using (SqlCommand dbCommand = dbConnection.CreateCommand())
                {
                    dbCommand.CommandText = "SP_RCVMMCase_View";
                    dbCommand.CommandType = CommandType.StoredProcedure;
                    dbCommand.Parameters.Add("I_RCVMM_ID", SqlDbType.NVarChar).Value = RCVMMID;
                    dbCommand.Parameters.Add("I_User_ID", SqlDbType.NVarChar).Value = userID;

                    dbCommand.Parameters.Add("searchResultCursor", SqlDbType.Int).Direction = ParameterDirection.Output;
                    dbCommand.Parameters.Add("viewAttachmentsCursor", SqlDbType.Int).Direction = ParameterDirection.Output;
                    dbCommand.Parameters.Add("viewProjectMapPointsCursor", SqlDbType.Int).Direction = ParameterDirection.Output;
                    dbCommand.Parameters.Add("viewCRMapPointsCursor", SqlDbType.Int).Direction = ParameterDirection.Output;

                    dbConnection.Open();
                    SqlDataReader reader = dbCommand.ExecuteReader(CommandBehavior.CloseConnection);
                    if (reader.Read())
                    {
                        viewResult = new Ent_RCV_Missmatch()
                        {
                            ID = reader.GetInt32(reader.GetOrdinal("MMC_ID")),
                            isRCVCase = (reader.GetString(reader.GetOrdinal("Is_RCV_Case")) == "Y") ? true : false,
                            RCVID = reader.GetInt32(reader.GetOrdinal("RCV_ID")),
                            CRID = (reader.IsDBNull(reader.GetOrdinal("CR_ID")) ? -1
                                : reader.GetInt32(reader.GetOrdinal("CR_ID"))),
                            CRRegistrationDate = (reader.IsDBNull(reader.GetOrdinal("REGISTER_DATE")) ? default(DateTime)
                                : reader.GetDateTime(reader.GetOrdinal("REGISTER_DATE"))),
                            projectID = reader.GetInt32(reader.GetOrdinal("PROJECT_ID")),
                            projectName = reader.GetString(reader.GetOrdinal("PROJECT_Name")),
                            RCVAssignDate = (reader.IsDBNull(reader.GetOrdinal("ASSIGN_DATE")) ? default(DateTime)
                                : reader.GetDateTime(reader.GetOrdinal("ASSIGN_DATE"))),
                            comments = (reader.IsDBNull(reader.GetOrdinal("COMMENTS")) ? ""
                                : reader.GetString(reader.GetOrdinal("COMMENTS"))),
                            caseDescription = (reader.IsDBNull(reader.GetOrdinal("CASE_DESC")) ? ""
                                : reader.GetString(reader.GetOrdinal("CASE_DESC"))),
                            mapProject = new Ent_MapSelection()
                            {
                                zoomLevel = reader.GetInt32(reader.GetOrdinal("ZOOM_LEVEL"))
                            },
                            mapCR = new Ent_MapSelection()
                            {
                                projectMapSelectionType = (reader.IsDBNull(reader.GetOrdinal("SELECTION_TYPE")) ?
                                Ent_MapSelection.mapSelectionType.NA
                                : Ent_MapSelection.getDrawType(reader.GetString(reader.GetOrdinal("SELECTION_TYPE")))),
                                mapPoints = new List<Ent_MapPoint>()
                            },
                            sample1 = new Ent_CR_Sample()
                            {
                                sampleID = (reader.IsDBNull(reader.GetOrdinal("SAMPLE_ID_1")) ? -1
                                : reader.GetInt32(reader.GetOrdinal("SAMPLE_ID_1"))),
                                sampleMaker = (reader.IsDBNull(reader.GetOrdinal("SAMPLE_MAKER_1")))
                                        ? "" : reader.GetString(reader.GetOrdinal("SAMPLE_MAKER_1")),
                                sampleSize = (reader.IsDBNull(reader.GetOrdinal("SAMPLE_SIZE_1")))
                                        ? "" : reader.GetString(reader.GetOrdinal("SAMPLE_SIZE_1")),
                                sampleLength = (reader.IsDBNull(reader.GetOrdinal("SAMPLE_LENGTH_1")))
                                        ? "" : reader.GetString(reader.GetOrdinal("SAMPLE_LENGTH_1")),
                                sampleUnitID = (reader.IsDBNull(reader.GetOrdinal("SAMPLE_UNIT_ID_1")) ? -1
                                    : reader.GetInt32(reader.GetOrdinal("SAMPLE_UNIT_ID_1"))),
                                sampleUnitName = (reader.IsDBNull(reader.GetOrdinal("SAMPLE_UNIT_Name_1")))
                                        ? "" : reader.GetString(reader.GetOrdinal("SAMPLE_UNIT_Name_1"))
                            },
                            sample2 = new Ent_CR_Sample()
                            {
                                sampleID = (reader.IsDBNull(reader.GetOrdinal("SAMPLE_ID_2")) ? -1
                                : reader.GetInt32(reader.GetOrdinal("SAMPLE_ID_2"))),
                                sampleMaker = (reader.IsDBNull(reader.GetOrdinal("SAMPLE_MAKER_2")))
                                        ? "" : reader.GetString(reader.GetOrdinal("SAMPLE_MAKER_2")),
                                sampleSize = (reader.IsDBNull(reader.GetOrdinal("SAMPLE_SIZE_2")))
                                        ? "" : reader.GetString(reader.GetOrdinal("SAMPLE_SIZE_2")),
                                sampleLength = (reader.IsDBNull(reader.GetOrdinal("SAMPLE_LENGTH_2")))
                                        ? "" : reader.GetString(reader.GetOrdinal("SAMPLE_LENGTH_2")),
                                sampleUnitID = (reader.IsDBNull(reader.GetOrdinal("SAMPLE_UNIT_ID_2")) ? -1
                                    : reader.GetInt32(reader.GetOrdinal("SAMPLE_UNIT_ID_2"))),
                                sampleUnitName = (reader.IsDBNull(reader.GetOrdinal("SAMPLE_UNIT_Name_2")))
                                        ? "" : reader.GetString(reader.GetOrdinal("SAMPLE_UNIT_Name_2"))
                            },
                            mapSample1 = new Ent_MapSelection()
                            {
                                mapID = (reader.IsDBNull(reader.GetOrdinal("MAP_POINT_ID_1")) ? -1
                                    : reader.GetInt32(reader.GetOrdinal("MAP_POINT_ID_1"))),
                            },
                            mapSample2 = new Ent_MapSelection()
                            {
                                mapID = (reader.IsDBNull(reader.GetOrdinal("MAP_POINT_ID_2")) ? -1
                                    : reader.GetInt32(reader.GetOrdinal("MAP_POINT_ID_2"))),
                            },

                            allowedforEdit = (reader.GetString(reader.GetOrdinal("Allowed_For_Edit")) == "Y") ?
                                true : false
                        };

                        if (!(reader.IsDBNull(reader.GetOrdinal("LATITUDE_Mp1"))))
                        {
                            viewResult.mapSample1.mapPoints = new List<Ent_MapPoint>();
                            viewResult.mapSample1.mapPoints.Add(new Ent_MapPoint()
                            {
                                pointLatitude = reader.GetString(reader.GetOrdinal("LATITUDE_Mp1")),
                                pointLongitude = reader.GetString(reader.GetOrdinal("LONGITUDE_Mp1")),
                            });
                        }

                        if (!(reader.IsDBNull(reader.GetOrdinal("LATITUDE_Mp2"))))
                        {
                            viewResult.mapSample2.mapPoints = new List<Ent_MapPoint>();
                            viewResult.mapSample2.mapPoints.Add(new Ent_MapPoint()
                            {
                                pointLatitude = reader.GetString(reader.GetOrdinal("LATITUDE_Mp2")),
                                pointLongitude = reader.GetString(reader.GetOrdinal("LONGITUDE_Mp2")),
                            });
                        }

                        viewResult.lstAttachmentNames = new List<string>();
                        viewResult.lstAttachmentPaths = new List<string>();
                        reader.NextResult();
                        while (reader.Read())
                        {
                            viewResult.lstAttachmentNames.Add(reader.GetString(reader.GetOrdinal("ATTACHEMENT_NAME")));
                            viewResult.lstAttachmentPaths.Add(reader.GetString(reader.GetOrdinal("ATTACHEMENT_PATH")));
                        }

                        reader.NextResult();
                        viewResult.mapProject.mapPoints = new List<Ent_MapPoint>();
                        while (reader.Read())
                        {
                            viewResult.mapProject.mapPoints.Add(new Ent_MapPoint()
                            {
                                pointLatitude = reader.GetString(reader.GetOrdinal("LATITUDE")),
                                pointLongitude = reader.GetString(reader.GetOrdinal("LONGITUDE")),
                            });
                        }

                        reader.NextResult();
                        while (reader.Read())
                        {
                            viewResult.mapCR.mapPoints.Add(new Ent_MapPoint()
                            {
                                pointLatitude = reader.GetString(reader.GetOrdinal("LATITUDE")),
                                pointLongitude = reader.GetString(reader.GetOrdinal("LONGITUDE")),
                            });
                        }
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