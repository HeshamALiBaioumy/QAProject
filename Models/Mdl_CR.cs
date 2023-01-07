using QA.Entities.Business_Entities;
using QA.Entities.Reports_Entities;
using QA.Entities.Session_Entities;
using static QA.Entities.Business_Entities.Ent_MapSelection;
using QA.Entities.View_Entities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OracleClient;
using System.Data.SqlClient;

namespace QA.Models
{
    public class Mdl_CR
    {
        public ResponseMessage insert_updateCR(Ent_CR cr, bool isUpdateForm)
        {
            ResponseMessage response = new ResponseMessage();

            try
            {
                using (SqlConnection dbConnection = new SqlConnection(
                    ConfigurationManager.ConnectionStrings["dbConnectionString"].ConnectionString))
                using (SqlCommand dbCommand = dbConnection.CreateCommand())
                {
                    dbCommand.CommandText = "SP_CR";
                    dbCommand.CommandType = CommandType.StoredProcedure;
                    OracleParameter paramCRID = new OracleParameter("INPUT_ID", SqlDbType.Int);
                    paramCRID.Value = cr.CRID;
                    paramCRID.Direction = ParameterDirection.InputOutput;
                    dbCommand.Parameters.Add(paramCRID);
                    dbCommand.Parameters.Add("I_PROJECT_ID", SqlDbType.Int).Value = cr.projectID;
                    dbCommand.Parameters.Add("I_PROJECT_ITEM_ID", SqlDbType.Int).Value = cr.projectItemID;
                    dbCommand.Parameters.Add("I_CR_TYPE_ID", SqlDbType.Int).Value = cr.CRTypeID;
                    dbCommand.Parameters.Add("I_STATUS", SqlDbType.NVarChar).Value = (cr.CRStatus);
                    dbCommand.Parameters.Add("I_SELECTION_TYPE", SqlDbType.NVarChar).Value
                        = (cr.mapSelection.projectMapSelectionType.ToString());
                    dbCommand.Parameters.Add("I_GEOMETRY_SHAPE_txt", SqlDbType.NVarChar).Value
                        = prepareGeometryCommand(cr.mapSelection.projectMapSelectionType, cr.mapSelection.mapPoints);
                    dbCommand.Parameters.Add("I_CONTRACTOR_ID", SqlDbType.Int).Value = cr.makerID;
                    dbCommand.Parameters.Add("I_SAMPLE_MAKER", SqlDbType.NVarChar).Value =
                        (cr.sample == null) ? "" : cr.sample.sampleMaker;
                    dbCommand.Parameters.Add("I_SAMPLE_SIZE", SqlDbType.NVarChar).Value =
                        (cr.sample == null) ? "" : cr.sample.sampleSize;
                    dbCommand.Parameters.Add("I_SAMPLE_LENGTH", SqlDbType.NVarChar).Value =
                        (cr.sample == null) ? "" : cr.sample.sampleLength;
                    dbCommand.Parameters.Add("I_SAMPLE_UNIT_ID", SqlDbType.Int).Value =
                        (cr.sample == null) ? -1 : cr.sample.sampleUnitID;
                    dbCommand.Parameters.Add("I_MAKER", SqlDbType.NVarChar).Value = cr.makerID;
                    dbCommand.Parameters.Add("ACTION", SqlDbType.NVarChar).Value = (isUpdateForm) ? "UPDATE" : "NEW";

                    dbConnection.Open();
                    dbCommand.ExecuteNonQuery();

                    if (!(isUpdateForm))
                    {
                        response.UDF = dbCommand.Parameters["INPUT_ID"].Value.ToString();
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

        private string prepareProjectMapPointsCommand(List<Ent_MapPoint> lstProjectMapPoints
            , bool isUpdateForm, int CRID)
        {
            try
            {
                string txtCommand = "";

                if (lstProjectMapPoints != null && lstProjectMapPoints.Count > 0)
                {
                    string v_mapSelectionID = (isUpdateForm) ? "(Select MAP_SELECTION_ID From CR Where CR_ID = " + CRID + ")"
                        : "SEQ_MAP_SELECTION_ID.currval";

                    txtCommand = "INSERT ALL ";
                    foreach (Ent_MapPoint mapPoint in lstProjectMapPoints)
                    {
                        txtCommand += "Into MAP_POINTS Values (" + v_mapSelectionID + ", "
                            + "get_ProjectMapPoints_seq, '" + mapPoint.pointLatitude + "', '" + mapPoint.pointLongitude + "') ";
                    }
                    txtCommand += "SELECT 1 FROM dual ";
                }

                return txtCommand;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private string prepareGeometryCommand(mapSelectionType projectMapSelectionType
            , List<Ent_MapPoint> lstProjectMapPoints)
        {
            /* c
             * 4 Digits -> DLTT
             * D -> 2 - Means 2 Diementions
             * L -> 0 - Means Non-Linear
             * TT -> 01 - Means Point
             *    -> 02 -Means Line
             *    -> 03 -Means Polygon
             */
            int SDO_GTYPE = 0;
            string sdo_geometry_command_text = "";
            if (lstProjectMapPoints != null && lstProjectMapPoints.Count > 0)
            {
                switch (projectMapSelectionType)
                {
                    //circle, 
                    case mapSelectionType.pushpin:
                        SDO_GTYPE = 2001;
                        sdo_geometry_command_text = "MDSYS.SDO_GEOMETRY(" + SDO_GTYPE + "," + 5255
                            + ",SDO_POINT_TYPE(" + String.Join(",", lstProjectMapPoints) + ",null),null,null)";
                        break;
                    case mapSelectionType.Line:
                    case mapSelectionType.polyline:
                        SDO_GTYPE = 2002;
                        sdo_geometry_command_text = "MDSYS.SDO_GEOMETRY(" + SDO_GTYPE + "," + 5255
                            + ",MDSYS.SDO_POINT_TYPE(0,0,null),MDSYS.SDO_ELEM_INFO_ARRAY(1,2,1),MDSYS.SDO_ORDINATE_ARRAY("
                            + String.Join(",", lstProjectMapPoints) + "))";
                        break;
                    case mapSelectionType.Rectangle:
                    case mapSelectionType.Polygon:
                        SDO_GTYPE = 2003;
                        sdo_geometry_command_text = "MDSYS.SDO_GEOMETRY(" + SDO_GTYPE + "," + 5255
                            + ",null,MDSYS.SDO_ELEM_INFO_ARRAY(1,1003,1),MDSYS.SDO_ORDINATE_ARRAY("
                            + String.Join(",", lstProjectMapPoints) + "))";
                        break;
                    default:
                        break;
                }
            }
            else
            {
                sdo_geometry_command_text = "null";
            }
            
            return sdo_geometry_command_text;
        }

        public List<Ent_CR> searchPendingCRs(int userID)
        {
            List<Ent_CR> searchResult = new List<Ent_CR>();

            try
            {
                using (SqlConnection dbConnection = new SqlConnection(
                    ConfigurationManager.ConnectionStrings["dbConnectionString"].ConnectionString))
                using (SqlCommand dbCommand = dbConnection.CreateCommand())
                {
                    dbCommand.CommandText = "SP_CR_PendingCRs";
                    dbCommand.CommandType = CommandType.StoredProcedure;
                    dbCommand.Parameters.Add("I_User_ID", SqlDbType.Int).Value = userID;
                    dbCommand.Parameters.Add("searchResultCursor", SqlDbType.Int).Direction = ParameterDirection.Output;

                    dbConnection.Open();
                    SqlDataReader reader = dbCommand.ExecuteReader();
                    while (reader.Read())
                    {
                        searchResult.Add(new Ent_CR()
                        {
                            CRID = reader.GetInt32(reader.GetOrdinal("CR_ID")),
                            contractorID = reader.GetInt32(reader.GetOrdinal("CONTRACTOR_ID")),
                            contractorName = reader.GetString(reader.GetOrdinal("CONTRACTOR_Name")),
                            projectID = reader.GetInt32(reader.GetOrdinal("PROJECT_ID")),
                            projectName = reader.GetString(reader.GetOrdinal("PROJECT_Name")),
                            projectItemID = reader.GetInt32(reader.GetOrdinal("PROJECT_ITEM_ID")),
                            projectItemName = reader.GetString(reader.GetOrdinal("PROJECT_ITEM_Name")),
                            CRTypeMCID = reader.GetInt32(reader.GetOrdinal("CR_TYPE_MC_ID")),
                            CRTypeMCName = reader.GetString(reader.GetOrdinal("CR_TYPE_MC_Name")),
                            CRTypeGroupID = reader.GetInt32(reader.GetOrdinal("CR_TYPE_GROUPS_ID")),
                            CRTypeGroupName = reader.GetString(reader.GetOrdinal("CR_TYPE_GROUPS_Name")),
                            CRTypeID = reader.GetInt32(reader.GetOrdinal("CR_TYPE_ID")),
                            CRTypeName = reader.GetString(reader.GetOrdinal("CR_TYPE_Name")),
                            CRStatus = reader.GetInt32(reader.GetOrdinal("CR_CURRENT_STATUS")),
                            CRStatusName = reader.GetString(reader.GetOrdinal("CR_CURRENT_STATUS_Name")),
                            registrationDate = reader.GetDateTime(reader.GetOrdinal("REGISTER_DATE")),
                            rejectReason = (reader.IsDBNull(reader.GetOrdinal("REJECT_REASON")) ? ""
                                : reader.GetString(reader.GetOrdinal("REJECT_REASON")))
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

        public ResponseMessage recieveCR(int CRID, int userID)
        {
            ResponseMessage result = new ResponseMessage();

            try
            {
                using (SqlConnection dbConnection = new SqlConnection(
                    ConfigurationManager.ConnectionStrings["dbConnectionString"].ConnectionString))
                using (SqlCommand dbCommand = dbConnection.CreateCommand())
                {
                    dbCommand.CommandText = "SP_Recieve_CR";
                    dbCommand.CommandType = CommandType.StoredProcedure;
                    dbCommand.Parameters.Add("I_CR_ID", SqlDbType.Int).Value = CRID;
                    dbCommand.Parameters.Add("I_User_ID", SqlDbType.Int).Value = userID;

                    dbConnection.Open();
                    dbCommand.ExecuteNonQuery();
                    dbCommand.Dispose();
                    dbConnection.Close();
                }

                result.responseStatus = true;
                result.errorMessage = null;
                result.endUserMessage = Localization.CR_Workflow.CR_Recieved_Successfully;
                result.comments = null;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }

        public List<Ent_CR> searchAssignedCRs(int userID)
        {
            List<Ent_CR> searchResult = new List<Ent_CR>();

            try
            {
                using (SqlConnection dbConnection = new SqlConnection(
                    ConfigurationManager.ConnectionStrings["dbConnectionString"].ConnectionString))
                using (SqlCommand dbCommand = dbConnection.CreateCommand())
                {
                    dbCommand.CommandText = "SP_CR_AssignedCRs";
                    dbCommand.CommandType = CommandType.StoredProcedure;
                    dbCommand.Parameters.Add("I_User_ID", SqlDbType.Int).Value = userID;
                    dbCommand.Parameters.Add("searchResultCursor", SqlDbType.Int).Direction = ParameterDirection.Output;

                    dbConnection.Open();
                    SqlDataReader reader = dbCommand.ExecuteReader();
                    while (reader.Read())
                    {
                        searchResult.Add(new Ent_CR()
                        {
                            CRID = reader.GetInt32(reader.GetOrdinal("CR_ID")),
                            contractorID = reader.GetInt32(reader.GetOrdinal("CONTRACTOR_ID")),
                            contractorName = reader.GetString(reader.GetOrdinal("CONTRACTOR_Name")),
                            projectID = reader.GetInt32(reader.GetOrdinal("PROJECT_ID")),
                            projectName = reader.GetString(reader.GetOrdinal("PROJECT_Name")),
                            projectItemID = reader.GetInt32(reader.GetOrdinal("PROJECT_ITEM_ID")),
                            projectItemName = reader.GetString(reader.GetOrdinal("PROJECT_ITEM_Name")),
                            CRTypeMCID = reader.GetInt32(reader.GetOrdinal("CR_TYPE_MC_ID")),
                            CRTypeMCName = reader.GetString(reader.GetOrdinal("CR_TYPE_MC_Name")),
                            CRTypeGroupID = reader.GetInt32(reader.GetOrdinal("CR_TYPE_GROUPS_ID")),
                            CRTypeGroupName = reader.GetString(reader.GetOrdinal("CR_TYPE_GROUPS_Name")),
                            CRTypeID = reader.GetInt32(reader.GetOrdinal("CR_TYPE_ID")),
                            CRTypeName = reader.GetString(reader.GetOrdinal("CR_TYPE_Name")),
                            CRStatus = reader.GetInt32(reader.GetOrdinal("CR_CURRENT_STATUS")),
                            CRStatusName = reader.GetString(reader.GetOrdinal("CR_CURRENT_STATUS_Name")),
                            registrationDate = reader.GetDateTime(reader.GetOrdinal("REGISTER_DATE")),
                            rejectReason = (reader.IsDBNull(reader.GetOrdinal("REJECT_REASON")) ? ""
                                : reader.GetString(reader.GetOrdinal("REJECT_REASON")))
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

        public List<Ent_CR> searchAllCRs(int userID, int searchProjectID, int searchProjectItemID, int searchCRMCID
            , int searchCRGroupID, int searchCRTypeID, int searchStatus, string searchCRID)
        {
            List<Ent_CR> searchResult = new List<Ent_CR>();

            try
            {
                using (SqlConnection dbConnection = new SqlConnection(
                    ConfigurationManager.ConnectionStrings["dbConnectionString"].ConnectionString))
                using (SqlCommand dbCommand = dbConnection.CreateCommand())
                {
                    dbCommand.CommandText = "SP_CR_SearchAllCRs";
                    dbCommand.CommandType = CommandType.StoredProcedure;
                    dbCommand.Parameters.Add("I_User_ID", SqlDbType.Int).Value = userID;
                    dbCommand.Parameters.Add("I_ProjectID", SqlDbType.Int).Value = searchProjectID;
                    dbCommand.Parameters.Add("I_ProjectItemID", SqlDbType.Int).Value = searchProjectItemID;
                    dbCommand.Parameters.Add("I_CRMCID", SqlDbType.Int).Value = searchCRMCID;
                    dbCommand.Parameters.Add("I_CRGroupID", SqlDbType.Int).Value = searchCRGroupID;
                    dbCommand.Parameters.Add("I_CRTypeID", SqlDbType.Int).Value = searchCRTypeID;
                    dbCommand.Parameters.Add("I_CRStatus", SqlDbType.Int).Value = searchStatus;
                    dbCommand.Parameters.Add("I_CRID", SqlDbType.NVarChar).Value = searchCRID;
                    dbCommand.Parameters.Add("searchResultCursor", SqlDbType.Int).Direction = ParameterDirection.Output;

                    dbConnection.Open();
                    SqlDataReader reader = dbCommand.ExecuteReader();
                    while (reader.Read())
                    {
                        searchResult.Add(new Ent_CR()
                        {
                            CRID = reader.GetInt32(reader.GetOrdinal("CR_ID")),
                            contractorID = reader.GetInt32(reader.GetOrdinal("CONTRACTOR_ID")),
                            contractorName = reader.GetString(reader.GetOrdinal("CONTRACTOR_Name")),
                            projectID = reader.GetInt32(reader.GetOrdinal("PROJECT_ID")),
                            projectName = reader.GetString(reader.GetOrdinal("PROJECT_Name")),
                            projectItemID = reader.GetInt32(reader.GetOrdinal("PROJECT_ITEM_ID")),
                            projectItemName = reader.GetString(reader.GetOrdinal("PROJECT_ITEM_Name")),
                            CRTypeMCID = reader.GetInt32(reader.GetOrdinal("CR_TYPE_MC_ID")),
                            CRTypeMCName = reader.GetString(reader.GetOrdinal("CR_TYPE_MC_Name")),
                            CRTypeGroupID = reader.GetInt32(reader.GetOrdinal("CR_TYPE_GROUPS_ID")),
                            CRTypeGroupName = reader.GetString(reader.GetOrdinal("CR_TYPE_GROUPS_Name")),
                            CRTypeID = reader.GetInt32(reader.GetOrdinal("CR_TYPE_ID")),
                            CRTypeName = reader.GetString(reader.GetOrdinal("CR_TYPE_Name")),
                            CRStatus = reader.GetInt32(reader.GetOrdinal("CR_CURRENT_STATUS")),
                            CRStatusName = reader.GetString(reader.GetOrdinal("CR_CURRENT_STATUS_Name")),
                            registrationDate = reader.GetDateTime(reader.GetOrdinal("REGISTER_DATE")),
                            allowedforAttachments = (reader.GetString(reader.GetOrdinal("AllowedForAttachments")) == "Y") ? true : false,
                            allowedforClosure = (reader.GetString(reader.GetOrdinal("AllowedForClosure")) == "Y") ? true : false,
                            allowedforComplaint = (reader.GetString(reader.GetOrdinal("AllowedForComplaint")) == "Y") ? true : false,
                            rejectReason = (reader.IsDBNull(reader.GetOrdinal("REJECT_REASON")) ? ""
                                : reader.GetString(reader.GetOrdinal("REJECT_REASON"))),
                            hasAttachments = (reader.GetString(reader.GetOrdinal("Allowed_ViewAttachments")) == "Y") ? true : false,
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

        public VW_CR_Execute viewCR(int CRID, int userID)
        {
            VW_CR_Execute viewResult = null;

            try
            {
                using (SqlConnection dbConnection = new SqlConnection(
                    ConfigurationManager.ConnectionStrings["dbConnectionString"].ConnectionString))
                using (SqlCommand dbCommand = dbConnection.CreateCommand())
                {
                    dbCommand.CommandText = "SP_CR_View";
                    dbCommand.CommandType = CommandType.StoredProcedure;
                    dbCommand.Parameters.Add("I_CR_ID", SqlDbType.Int).Value = CRID;
                    dbCommand.Parameters.Add("I_User_ID", SqlDbType.Int).Value = userID;
                    dbCommand.Parameters.Add("searchResultCursor", SqlDbType.Int).Direction = ParameterDirection.Output;
                    dbCommand.Parameters.Add("viewProjectMapSelectionsCursor", SqlDbType.Int).Direction = ParameterDirection.Output;
                    dbCommand.Parameters.Add("viewProjectMapPointsCursor", SqlDbType.Int).Direction = ParameterDirection.Output;
                    dbCommand.Parameters.Add("viewCRMapSelectionsCursor", SqlDbType.Int).Direction = ParameterDirection.Output;
                    dbCommand.Parameters.Add("viewCRMapPointsCursor", SqlDbType.Int).Direction = ParameterDirection.Output;
                    dbCommand.Parameters.Add("viewCRSampleMapPointCursor", SqlDbType.Int).Direction = ParameterDirection.Output;

                    dbConnection.Open();
                    SqlDataReader reader = dbCommand.ExecuteReader(CommandBehavior.CloseConnection);
                    if (reader.Read())
                    {
                        viewResult = new VW_CR_Execute()
                        {
                            CR = new Ent_CR()
                            {
                                CRID = reader.GetInt32(reader.GetOrdinal("CR_ID")),
                                contractorID = reader.GetInt32(reader.GetOrdinal("CONTRACTOR_ID")),
                                contractorName = reader.GetString(reader.GetOrdinal("CONTRACTOR_Name")),
                                projectID = reader.GetInt32(reader.GetOrdinal("PROJECT_ID")),
                                projectName = reader.GetString(reader.GetOrdinal("PROJECT_Name")),
                                projectItemID = reader.GetInt32(reader.GetOrdinal("PROJECT_ITEM_ID")),
                                projectItemName = reader.GetString(reader.GetOrdinal("PROJECT_ITEM_Name")),
                                CRTypeMCID = reader.GetInt32(reader.GetOrdinal("CR_TYPE_MC_ID")),
                                CRTypeMCName = reader.GetString(reader.GetOrdinal("CR_TYPE_MC_Name")),
                                CRTypeGroupID = reader.GetInt32(reader.GetOrdinal("CR_TYPE_GROUPS_ID")),
                                CRTypeGroupName = reader.GetString(reader.GetOrdinal("CR_TYPE_GROUPS_Name")),
                                CRTypeID = reader.GetInt32(reader.GetOrdinal("CR_TYPE_ID")),
                                CRTypeName = reader.GetString(reader.GetOrdinal("CR_TYPE_Name")),
                                CRStatus = reader.GetInt32(reader.GetOrdinal("CR_CURRENT_STATUS")),
                                CRStatusName = reader.GetString(reader.GetOrdinal("CR_CURRENT_STATUS_Name")),
                                isLabRequired = (reader.GetString(reader.GetOrdinal("IS_REQUIRE_SAMPLE")) == "Y") ? true : false,
                                rejectReason = (reader.IsDBNull(reader.GetOrdinal("REJECT_REASON")) ? ""
                                : reader.GetString(reader.GetOrdinal("REJECT_REASON"))),
                                sample = new Ent_CR_Sample()
                                {
                                    sampleMaker = (reader.IsDBNull(reader.GetOrdinal("SAMPLE_MAKER")))
                                        ? "" : reader.GetString(reader.GetOrdinal("SAMPLE_MAKER")),
                                    sampleSize = (reader.IsDBNull(reader.GetOrdinal("SAMPLE_SIZE")))
                                        ? "" : reader.GetString(reader.GetOrdinal("SAMPLE_SIZE")),
                                    sampleLength = (reader.IsDBNull(reader.GetOrdinal("SAMPLE_LENGTH")))
                                        ? "" : reader.GetString(reader.GetOrdinal("SAMPLE_LENGTH")),
                                    sampleUnitID = reader.GetInt32(reader.GetOrdinal("SAMPLE_UNIT_ID")),
                                    sampleUnitName = (reader.IsDBNull(reader.GetOrdinal("SAMPLE_UNIT_Name")))
                                        ? "" : reader.GetString(reader.GetOrdinal("SAMPLE_UNIT_Name"))
                                }
                            }
                        };
                    }

                    reader.NextResult();
                    if (reader.Read())
                    {
                        viewResult.projectMapSelection = new Ent_MapSelection()
                        {
                            zoomLevel = reader.GetInt32(reader.GetOrdinal("ZOOM_LEVEL")),
                            centerLatitude = reader.GetString(reader.GetOrdinal("CENTER_LAT")),
                            centerLongitude = reader.GetString(reader.GetOrdinal("CENTER_LONG")),
                            projectMapSelectionType = Ent_MapSelection.getDrawType
                                (reader.GetString(reader.GetOrdinal("SELECTION_TYPE")))
                        };
                    }

                    reader.NextResult();
                    viewResult.projectMapSelection.mapPoints = new List<Ent_MapPoint>();
                    while (reader.Read())
                    {
                        viewResult.projectMapSelection.mapPoints.Add(new Ent_MapPoint()
                        {
                            pointLatitude = reader.GetString(reader.GetOrdinal("LATITUDE")),
                            pointLongitude = reader.GetString(reader.GetOrdinal("LONGITUDE")),
                        });
                    }

                    reader.NextResult();
                    if (reader.Read())
                    {
                        viewResult.CR.mapSelection = new Ent_MapSelection()
                        {
                            zoomLevel = reader.GetInt32(reader.GetOrdinal("ZOOM_LEVEL")),
                            centerLatitude = reader.GetString(reader.GetOrdinal("CENTER_LAT")),
                            centerLongitude = reader.GetString(reader.GetOrdinal("CENTER_LONG")),
                            projectMapSelectionType = Ent_MapSelection.getDrawType
                                (reader.GetString(reader.GetOrdinal("SELECTION_TYPE")))
                        };
                    }

                    reader.NextResult();
                    viewResult.CR.mapSelection.mapPoints = new List<Ent_MapPoint>();
                    while (reader.Read())
                    {
                        viewResult.CR.mapSelection.mapPoints.Add(new Ent_MapPoint()
                        {
                            pointLatitude = reader.GetString(reader.GetOrdinal("LATITUDE")),
                            pointLongitude = reader.GetString(reader.GetOrdinal("LONGITUDE")),
                        });
                    }

                    reader.NextResult();
                    viewResult.CR.sample.mapSelection = new Ent_MapSelection();
                    viewResult.CR.sample.mapSelection.mapPoints = new List<Ent_MapPoint>();
                    while (reader.Read())
                    {
                        viewResult.CR.sample.mapSelection.mapPoints.Add(new Ent_MapPoint()
                        {
                            pointLatitude = reader.GetString(reader.GetOrdinal("LATITUDE")),
                            pointLongitude = reader.GetString(reader.GetOrdinal("LONGITUDE")),
                        });
                    }

                    reader.NextResult();
                    if (reader.Read())
                    {
                        string x = Convert.ToString(dbCommand.Parameters["error_msg"].Value);
                    }

                    reader.Close();
                    dbCommand.Dispose();
                    dbConnection.Close();
                }
            }
            catch (Exception ex)
            {
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

        public int getCRRelatedProject(int CRID)
        {
            int projectID = -1;
            try
            {
                using (SqlConnection dbConnection = new SqlConnection(
                    ConfigurationManager.ConnectionStrings["dbConnectionString"].ConnectionString))
                using (SqlCommand dbCommand = dbConnection.CreateCommand())
                {
                    dbCommand.CommandText = "SP_CR_GetProjectID";
                    dbCommand.CommandType = CommandType.StoredProcedure;
                    dbCommand.Parameters.Add("I_CR_ID", SqlDbType.Int).Value = CRID;
                    dbCommand.Parameters.Add("O_Project_ID", SqlDbType.Int).Direction = ParameterDirection.Output;

                    dbConnection.Open();
                    SqlDataReader reader = dbCommand.ExecuteReader(CommandBehavior.CloseConnection);
                    if (reader.Read())
                    {
                        projectID = reader.GetInt32(reader.GetOrdinal("O_Project_ID"));
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

            return projectID;
        }

        public ResponseMessage executeCR(int CRID, bool isSampleLabRequired
            , List<Ent_MapPoint> lstSampleMapPoints, bool isAcceptCR, string rejectCRReason)
        {
            ResponseMessage response = new ResponseMessage();

            try
            {
                using (SqlConnection dbConnection = new SqlConnection(
                    ConfigurationManager.ConnectionStrings["dbConnectionString"].ConnectionString))
                using (SqlCommand dbCommand = dbConnection.CreateCommand())
                {
                    dbCommand.CommandText = "SP_Execute_CR";
                    dbCommand.CommandType = CommandType.StoredProcedure;
                    dbCommand.Parameters.Add("I_CR_ID", SqlDbType.Int).Value = CRID;
                    dbCommand.Parameters.Add("I_IsAddSample", SqlDbType.NVarChar).Value = (isSampleLabRequired) ? "Y" : "N";
                    dbCommand.Parameters.Add("I_CRSample_GEOMETRYSHAPE_txt", SqlDbType.NVarChar).Value
                        = prepareGeometryCommand(mapSelectionType.pushpin , lstSampleMapPoints);
                        //= prepareProjectMapPointsCommand(lstSampleMapPoints, false, CRID);
                    dbCommand.Parameters.Add("I_ACTION", SqlDbType.NVarChar).Value = (isAcceptCR) ? "ACCEPT" : "REJECT";
                    dbCommand.Parameters.Add("I_rejectReason", SqlDbType.NVarChar).Value = rejectCRReason ?? "";

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

        public ResponseMessage addAttachments(Ent_Attachment attachment)
        {
            ResponseMessage response = new ResponseMessage();

            try
            {
                using (SqlConnection dbConnection = new SqlConnection(
                    ConfigurationManager.ConnectionStrings["dbConnectionString"].ConnectionString))
                using (SqlCommand dbCommand = dbConnection.CreateCommand())
                {
                    dbCommand.CommandText = "SP_CR_AddAttachment";
                    dbCommand.CommandType = CommandType.StoredProcedure;
                    dbCommand.Parameters.Add("I_CR_ID", SqlDbType.Int).Value = attachment.parentID;
                    dbCommand.Parameters.Add("I_MakerID", SqlDbType.Int).Value = attachment.makerID;
                    dbCommand.Parameters.Add("I_ATTACHEMENT_Name", SqlDbType.NVarChar).Value = attachment.attachmentName;
                    dbCommand.Parameters.Add("I_ATTACHEMENT_Path", SqlDbType.NVarChar).Value = attachment.attachmentPath;
                    dbCommand.Parameters.Add("I_Sample_Code", SqlDbType.NVarChar).Value = attachment.sampleCode;
                    dbCommand.Parameters.Add("I_SAMPLE_TEST_ID", SqlDbType.Int).Value = attachment.sampleTestID;
                    dbCommand.Parameters.Add("I_Sample_Result", SqlDbType.Char).Value = (attachment.sampleResult == 0) ? 'Y' : 'N';

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

        public List<Ent_Attachment> viewAttachments(int CRID)
        {
            List<Ent_Attachment> searchResult = new List<Ent_Attachment>();

            try
            {
                using (SqlConnection dbConnection = new SqlConnection(
                    ConfigurationManager.ConnectionStrings["dbConnectionString"].ConnectionString))
                using (SqlCommand dbCommand = dbConnection.CreateCommand())
                {
                    dbCommand.CommandText = "SP_CR_ViewAttachments";
                    dbCommand.CommandType = CommandType.StoredProcedure;
                    dbCommand.Parameters.Add("I_CR_ID", SqlDbType.Int).Value = CRID;
                    dbCommand.Parameters.Add("searchResultCursor", SqlDbType.Int).Direction = ParameterDirection.Output;

                    dbConnection.Open();
                    SqlDataReader reader = dbCommand.ExecuteReader();
                    while (reader.Read())
                    {
                        searchResult.Add(new Ent_Attachment()
                        {
                            parentID = CRID,
                            sampleCode = reader.GetString(reader.GetOrdinal("SAMPLE_CODE")),
                            sampleTestCategoryID = reader.GetInt32(reader.GetOrdinal("SAMPLE_TYPE_ID")),
                            sampleTestCategoryName = reader.GetString(reader.GetOrdinal("SAMPLE_TYPE_Name")),
                            sampleTestID = reader.GetInt32(reader.GetOrdinal("SAMPLE_TEST_ID")),
                            sampleTestName = reader.GetString(reader.GetOrdinal("SAMPLE_TEST_Name")),
                            sampleResult = reader.GetInt32(reader.GetOrdinal("SAMPLE_RESULT")),
                            makerID = reader.GetInt32(reader.GetOrdinal("MAKERID")).ToString(),
                            makerName = reader.GetString(reader.GetOrdinal("MAKERName")),
                            attachmentName = reader.GetString(reader.GetOrdinal("ATTACHEMENT_NAME")),
                            attachmentPath = reader.GetString(reader.GetOrdinal("ATTACHEMENT_PATH")),
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

        public Rpt_CR printCR(int CRID)
        {
            Rpt_CR viewResult = null;

            try
            {
                using (SqlConnection dbConnection = new SqlConnection(
                    ConfigurationManager.ConnectionStrings["dbConnectionString"].ConnectionString))
                using (SqlCommand dbCommand = dbConnection.CreateCommand())
                {
                    dbCommand.CommandText = "SP_CR_Print";
                    dbCommand.CommandType = CommandType.StoredProcedure;
                    dbCommand.Parameters.Add("I_CR_ID", SqlDbType.Int).Value = CRID;
                    dbCommand.Parameters.Add("searchResultCursor", SqlDbType.Int).Direction = ParameterDirection.Output;
                    dbCommand.Parameters.Add("viewMapPointsCursor", SqlDbType.Int).Direction = ParameterDirection.Output;

                    dbConnection.Open();
                    SqlDataReader reader = dbCommand.ExecuteReader(CommandBehavior.CloseConnection);
                    if (reader.Read())
                    {
                        viewResult = new Rpt_CR()
                        {
                            CRID = reader.GetInt32(reader.GetOrdinal("CR_ID")),
                            registrationDate = reader.GetDateTime(reader.GetOrdinal("REGISTER_DATE")),
                            CRTypeName = reader.GetString(reader.GetOrdinal("CR_TYPE_Name")),
                            projectName = reader.GetString(reader.GetOrdinal("PROJECT_NAME")),
                            projectItemName = reader.GetString(reader.GetOrdinal("PROJECT_ITEM_Name")),
                            contractorName = reader.GetString(reader.GetOrdinal("CONTRACTOR_Name")),
                            projectOwnerName = (reader.IsDBNull(reader.GetOrdinal("PO_Name")))
                                        ? "" : reader.GetString(reader.GetOrdinal("PO_Name")),
                            superEngName = reader.GetString(reader.GetOrdinal("SuperEng_Name")),
                            consulatntEngName = (reader.IsDBNull(reader.GetOrdinal("ConsultantEng_Name")))
                                        ? "" : reader.GetString(reader.GetOrdinal("ConsultantEng_Name")),
                            authLabName = reader.GetString(reader.GetOrdinal("AuthLab_Name")),
                            departmentName = reader.GetString(reader.GetOrdinal("Department_Name")),
                            depSectionName = reader.GetString(reader.GetOrdinal("DepSection_Name")),
                            projectregistrationDate = reader.GetDateTime(reader.GetOrdinal("Project_Registration_Date")),
                            sampleMaker = (reader.IsDBNull(reader.GetOrdinal("SAMPLE_MAKER")))
                                        ? "" : reader.GetString(reader.GetOrdinal("SAMPLE_MAKER")),
                            sampleSize = (reader.IsDBNull(reader.GetOrdinal("SAMPLE_SIZE")))
                                        ? "" : reader.GetString(reader.GetOrdinal("SAMPLE_SIZE")),
                            sampleLength = (reader.IsDBNull(reader.GetOrdinal("SAMPLE_LENGTH")))
                                        ? "" : reader.GetString(reader.GetOrdinal("SAMPLE_LENGTH")),
                            sampleUnitName = (reader.IsDBNull(reader.GetOrdinal("SAMPLE_UNIT_Name")))
                                        ? "" : reader.GetString(reader.GetOrdinal("SAMPLE_UNIT_Name")),
                            mapZoomLevel = reader.GetInt32(reader.GetOrdinal("ZOOM_LEVEL")),
                            CRMapSelectionType = Ent_MapSelection.getDrawType(reader.GetString(reader.GetOrdinal("CR_SELECTION_TYPE")))
                        };

                        string sampleLat = (reader.IsDBNull(reader.GetOrdinal("Sample_Lat")))
                                ? "" : reader.GetString(reader.GetOrdinal("Sample_Lat"));
                        string sampleLng = (reader.IsDBNull(reader.GetOrdinal("Sample_Lng")))
                                ? "" : reader.GetString(reader.GetOrdinal("Sample_Lng"));
                        if (sampleLat != "" && sampleLng != "")
                        {
                            viewResult.sampleMapPoints = new List<Ent_MapPoint>();
                            viewResult.sampleMapPoints.Add(new Ent_MapPoint()
                            {
                                pointLatitude = sampleLat,
                                pointLongitude = sampleLng
                            });
                        }
                    }

                    reader.NextResult();
                    viewResult.projectMapPoints = new List<Ent_MapPoint>();
                    viewResult.crMapPoints = new List<Ent_MapPoint>();
                    string obj = "";
                    while (reader.Read())
                    {
                        obj = reader.GetString(reader.GetOrdinal("Obj"));
                        if (obj.Equals("Project"))
                        {
                            viewResult.projectMapPoints.Add(new Ent_MapPoint()
                            {
                                pointLatitude = reader.GetString(reader.GetOrdinal("LATITUDE")),
                                pointLongitude = reader.GetString(reader.GetOrdinal("LONGITUDE")),
                            });
                        }
                        else if (obj.Equals("CR"))
                        {
                            viewResult.crMapPoints.Add(new Ent_MapPoint()
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

        public Vw_SearchCr_Report printSearchCR(DateTime searchCRDateFrom, DateTime searchCRDateTo)
        {
            Vw_SearchCr_Report viewResult = null;

            try
            {
                using (SqlConnection dbConnection = new SqlConnection(
                    ConfigurationManager.ConnectionStrings["dbConnectionString"].ConnectionString))
                using (SqlCommand dbCommand = dbConnection.CreateCommand())
                {
                    dbCommand.CommandText = "SP_CR_Search_Print";
                    dbCommand.CommandType = CommandType.StoredProcedure;
                    dbCommand.Parameters.Add("I_CR_RegDate_From", SqlDbType.DateTime).Value = searchCRDateFrom;
                    dbCommand.Parameters.Add("I_CR_RegDate_To", SqlDbType.DateTime).Value = searchCRDateTo;
                    dbCommand.Parameters.Add("searchResultCursor", SqlDbType.Int).Direction = ParameterDirection.Output;

                    dbConnection.Open();
                    SqlDataReader reader = dbCommand.ExecuteReader(CommandBehavior.CloseConnection);
                    viewResult = new Vw_SearchCr_Report()
                    {
                        searchDateFrom = searchCRDateFrom,
                        searchDateTo = searchCRDateTo,
                        lstSearchCRs = new List<Rpt_CR>()
                    };

                    while (reader.Read())
                    {
                        viewResult.lstSearchCRs.Add(new Rpt_CR()
                        {
                            CRID = reader.GetInt32(reader.GetOrdinal("CR_ID")),
                            CRTypeName = reader.GetString(reader.GetOrdinal("CR_TYPE_Name")),
                            registrationDate = reader.GetDateTime(reader.GetOrdinal("REGISTER_DATE")),
                            projectName = reader.GetString(reader.GetOrdinal("PROJECT_Name")),
                            contractorName = reader.GetString(reader.GetOrdinal("CONTRACTOR_Name")),
                            lastStatusDate = reader.GetDateTime(reader.GetOrdinal("Last_Status_Date")),
                            CRStatusName = reader.GetString(reader.GetOrdinal("STATUS_ARA_DESC"))
                        });
                    }

                    reader.Close();
                    dbCommand.Dispose();
                    dbConnection.Close();
                }
            }
            catch (Exception ex)
            {
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

        public Vw_SearchCr_Report printSearchProjectCR(int searchProjectID, DateTime searchCRDateFrom, DateTime searchCRDateTo)
        {
            Vw_SearchCr_Report viewResult = null;

            try
            {
                using (SqlConnection dbConnection = new SqlConnection(
                    ConfigurationManager.ConnectionStrings["dbConnectionString"].ConnectionString))
                using (SqlCommand dbCommand = dbConnection.CreateCommand())
                {
                    dbCommand.CommandText = "SP_CR_ProjectSearch_Print";
                    dbCommand.CommandType = CommandType.StoredProcedure;
                    dbCommand.Parameters.Add("I_ProjectID", SqlDbType.Int).Value = searchProjectID;
                    dbCommand.Parameters.Add("I_CR_RegDate_From", SqlDbType.DateTime).Value = searchCRDateFrom;
                    dbCommand.Parameters.Add("I_CR_RegDate_To", SqlDbType.DateTime).Value = searchCRDateTo;
                    dbCommand.Parameters.Add("O_Project_Name", SqlDbType.NVarChar, 255).Direction = ParameterDirection.Output;
                    dbCommand.Parameters.Add("O_SuperEng_Name", SqlDbType.NVarChar, 255).Direction = ParameterDirection.Output;
                    dbCommand.Parameters.Add("O_ConsEng_Name", SqlDbType.NVarChar, 255).Direction = ParameterDirection.Output;
                    dbCommand.Parameters.Add("O_Contractor_Name", SqlDbType.NVarChar, 255).Direction = ParameterDirection.Output;
                    dbCommand.Parameters.Add("O_AuthLab_Name", SqlDbType.NVarChar, 255).Direction = ParameterDirection.Output;
                    dbCommand.Parameters.Add("searchResultCursor", SqlDbType.Int).Direction = ParameterDirection.Output;

                    dbConnection.Open();
                    SqlDataReader reader = dbCommand.ExecuteReader(CommandBehavior.CloseConnection);
                    viewResult = new Vw_SearchCr_Report()
                    {
                        searchProjectID = searchProjectID,
                        searchDateFrom = searchCRDateFrom,
                        searchDateTo = searchCRDateTo,

                        projectName = dbCommand.Parameters["O_Project_Name"].Value.ToString(),
                        superEngName = dbCommand.Parameters["O_SuperEng_Name"].Value.ToString(),
                        consEngName = dbCommand.Parameters["O_ConsEng_Name"].Value.ToString(),
                        contractorName = dbCommand.Parameters["O_Contractor_Name"].Value.ToString(),
                        authLabName = dbCommand.Parameters["O_AuthLab_Name"].Value.ToString(),

                        lstSearchCRs = new List<Rpt_CR>()
                    };

                    while (reader.Read())
                    {
                        viewResult.lstSearchCRs.Add(new Rpt_CR()
                        {
                            CRID = reader.GetInt32(reader.GetOrdinal("CR_ID")),
                            registrationDate = reader.GetDateTime(reader.GetOrdinal("REGISTER_DATE")),
                            CRTypeName = reader.GetString(reader.GetOrdinal("CR_TYPE_Name")),
                            CRStatusName = reader.GetString(reader.GetOrdinal("STATUS_ARA_DESC")),
                            lastStatusDate = reader.GetDateTime(reader.GetOrdinal("Last_Status_Date")),
                            labEntryName = reader.GetString(reader.GetOrdinal("AuthLab_Name"))
                        });
                    }

                    reader.Close();
                    dbCommand.Dispose();
                    dbConnection.Close();
                }
            }
            catch (Exception ex)
            {
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

        public Vw_SearchCr_Report printCRSamples(DateTime searchCRDateFrom, DateTime searchCRDateTo)
        {
            Vw_SearchCr_Report viewResult = null;

            try
            {
                using (SqlConnection dbConnection = new SqlConnection(
                    ConfigurationManager.ConnectionStrings["dbConnectionString"].ConnectionString))
                using (SqlCommand dbCommand = dbConnection.CreateCommand())
                {
                    dbCommand.CommandText = "SP_CR_Samples_Report";
                    dbCommand.CommandType = CommandType.StoredProcedure;
                    dbCommand.Parameters.Add("I_CR_RegDate_From", SqlDbType.DateTime).Value = searchCRDateFrom;
                    dbCommand.Parameters.Add("I_CR_RegDate_To", SqlDbType.DateTime).Value = searchCRDateTo;
                    dbCommand.Parameters.Add("searchResultCursor", SqlDbType.Int).Direction = ParameterDirection.Output;

                    dbConnection.Open();
                    SqlDataReader reader = dbCommand.ExecuteReader(CommandBehavior.CloseConnection);
                    viewResult = new Vw_SearchCr_Report()
                    {
                        searchDateFrom = searchCRDateFrom,
                        searchDateTo = searchCRDateTo,
                        lstSearchCRs = new List<Rpt_CR>()
                    };

                    while (reader.Read())
                    {
                        viewResult.lstSearchCRs.Add(new Rpt_CR()
                        {
                            CRID = reader.GetInt32(reader.GetOrdinal("CR_ID")),
                            registrationDate = reader.GetDateTime(reader.GetOrdinal("REGISTER_DATE")),
                            CRStatusName = reader.GetString(reader.GetOrdinal("STATUS_ARA_DESC")),
                            projectName = reader.GetString(reader.GetOrdinal("PROJECT_Name")),
                            contractorName = reader.GetString(reader.GetOrdinal("CONTRACTOR_Name")),
                            sampleID = (reader.IsDBNull(reader.GetOrdinal("SAMPLE_ID"))) ?
                                "" : reader.GetString(reader.GetOrdinal("SAMPLE_ID")),
                            CRTypeName = reader.GetString(reader.GetOrdinal("CR_TYPE_Name")),
                            authLabFeedback = (reader.IsDBNull(reader.GetOrdinal("AUTHLAB_FEEDBACK"))) ?
                                "" : reader.GetString(reader.GetOrdinal("AUTHLAB_FEEDBACK")),
                            authLabFeedbackDate = (reader.IsDBNull(reader.GetOrdinal("AUTHLAB_FEEDBACK_DATE")))
                                        ? default(DateTime) : reader.GetDateTime(reader.GetOrdinal("AUTHLAB_FEEDBACK_DATE"))
                        });
                    }

                    reader.Close();
                    dbCommand.Dispose();
                    dbConnection.Close();
                }
            }
            catch (Exception ex)
            {
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

        public Vw_SearchCr_Report printCRSamplesDetailed(int searchProjectID, DateTime searchCRDateFrom, DateTime searchCRDateTo)
        {
            Vw_SearchCr_Report viewResult = null;

            try
            {
                using (SqlConnection dbConnection = new SqlConnection(
                    ConfigurationManager.ConnectionStrings["dbConnectionString"].ConnectionString))
                using (SqlCommand dbCommand = dbConnection.CreateCommand())
                {
                    dbCommand.CommandText = "SP_CR_SamplesDetailed_Report";
                    dbCommand.CommandType = CommandType.StoredProcedure;
                    dbCommand.Parameters.Add("I_ProjectID", SqlDbType.Int).Value = searchProjectID;
                    dbCommand.Parameters.Add("I_CR_RegDate_From", SqlDbType.DateTime).Value = searchCRDateFrom;
                    dbCommand.Parameters.Add("I_CR_RegDate_To", SqlDbType.DateTime).Value = searchCRDateTo;
                    dbCommand.Parameters.Add("O_Project_Name", SqlDbType.NVarChar, 255).Direction = ParameterDirection.Output;
                    dbCommand.Parameters.Add("O_SuperEng_Name", SqlDbType.NVarChar, 255).Direction = ParameterDirection.Output;
                    dbCommand.Parameters.Add("O_ConsEng_Name", SqlDbType.NVarChar, 255).Direction = ParameterDirection.Output;
                    dbCommand.Parameters.Add("O_Contractor_Name", SqlDbType.NVarChar, 255).Direction = ParameterDirection.Output;
                    dbCommand.Parameters.Add("O_AuthLab_Name", SqlDbType.NVarChar, 255).Direction = ParameterDirection.Output;
                    dbCommand.Parameters.Add("searchResultCursor", SqlDbType.Int).Direction = ParameterDirection.Output;

                    dbConnection.Open();
                    SqlDataReader reader = dbCommand.ExecuteReader(CommandBehavior.CloseConnection);
                    viewResult = new Vw_SearchCr_Report()
                    {
                        searchProjectID = searchProjectID,
                        searchDateFrom = searchCRDateFrom,
                        searchDateTo = searchCRDateTo,

                        projectName = dbCommand.Parameters["O_Project_Name"].Value.ToString(),
                        superEngName = dbCommand.Parameters["O_SuperEng_Name"].Value.ToString(),
                        consEngName = dbCommand.Parameters["O_ConsEng_Name"].Value.ToString(),
                        contractorName = dbCommand.Parameters["O_Contractor_Name"].Value.ToString(),
                        authLabName = dbCommand.Parameters["O_AuthLab_Name"].Value.ToString(),

                        lstSearchCRs = new List<Rpt_CR>()
                    };

                    while (reader.Read())
                    {
                        viewResult.lstSearchCRs.Add(new Rpt_CR()
                        {
                            CRID = reader.GetInt32(reader.GetOrdinal("CR_ID")),
                            registrationDate = reader.GetDateTime(reader.GetOrdinal("REGISTER_DATE")),
                            CRStatusName = reader.GetString(reader.GetOrdinal("STATUS_ARA_DESC")),
                            sampleID = (reader.IsDBNull(reader.GetOrdinal("SAMPLE_ID"))) ?
                                "" : reader.GetString(reader.GetOrdinal("SAMPLE_ID")),
                            CRTypeName = reader.GetString(reader.GetOrdinal("CR_TYPE_Name")),
                            authLabFeedback = (reader.IsDBNull(reader.GetOrdinal("AUTHLAB_FEEDBACK"))) ?
                                "" : reader.GetString(reader.GetOrdinal("AUTHLAB_FEEDBACK")),
                            authLabFeedbackDate = (reader.IsDBNull(reader.GetOrdinal("AUTHLAB_FEEDBACK_DATE")))
                                        ? default(DateTime) : reader.GetDateTime(reader.GetOrdinal("AUTHLAB_FEEDBACK_DATE"))
                        });
                    }

                    reader.Close();
                    dbCommand.Dispose();
                    dbConnection.Close();
                }
            }
            catch (Exception ex)
            {
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
    }
}