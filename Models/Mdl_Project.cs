using QA.Entities.Business_Entities;
using QA.Entities.Reports_Entities;
using QA.Entities.Session_Entities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OracleClient;
using static QA.Entities.Business_Entities.Ent_MapSelection;
using System.Data.SqlClient;
using System.Linq;

namespace QA.Models
{
    public class Mdl_Project
    {
        public QualityDbEntities ctx = new QualityDbEntities();
        public ResponseMessage insert_updateProject(Ent_Project project, bool isUpdateForm)
        {

            ResponseMessage response = new ResponseMessage();
            try
            {
                var entity = new PROJECT();
                if (project.ID > 0)
                    entity = ctx.PROJECTS.FirstOrDefault(d => d.PROJECTS_ID == project.ID);



                entity.IS_ACTIVE = project.isActive;
                entity.MAKER = project.makerID;
                entity.MAKER_DAT_TIM = DateTime.Now;
                entity.PROJECT_OWNER_ID = project.projectOwnerID;
                entity.NAME = project.name;
                entity.START_DATE = project.startDate;
                entity.END_DATE = project.endDate;
                entity.SUPERVISOR_ENG_ID = project.supervisorEngID;
                entity.SECTION_ID = project.departmentSectionID;
                entity.CONSULTANT_ID = project.consultantID;
                entity.CONSULANT_ASSIST_ID = project.consultantAssistantID;
                entity.CONTRACTOR_ID = project.contractorID;
                entity.CONTRACTOR_ASSIST_ID = project.contractorAssistantID;
                entity.AUTH_LAB_ID = project.authorizedLabID;
                entity.QA_TECHNICIAN_ID = project.QATechnicianID;
                entity.QUALITY_ENG_ID = project.QualityAssuranceEngID;

                if (project.ID <= 0)
                {
                    ctx.PROJECTS.Add(entity);
                    ctx.SaveChanges();
                    prepareProjectMilestoneCommand(project.projectMileStones, false, project.ID);
                    var map = new MAP_SELECTIONS
                    {
                        ZOOM_LEVEL = project.mapSelection.zoomLevel,
                        CENTER_LAT = project.mapSelection.centerLatitude,
                        CENTER_LONG = project.mapSelection.centerLongitude,
                        SELECTION_TYPE = project.mapSelection.projectMapSelectionType.ToString(),
                        CIRCLE_DIAMETER = project.mapSelection.circleDiameter.ToString(),
                        // GEOMETRY_SHAPE = project.sha,
                    };
                    ctx.MAP_SELECTIONS.Add(map);
                    ctx.SaveChanges();
                    entity = ctx.PROJECTS.FirstOrDefault(s => s.PROJECTS_ID == entity.PROJECTS_ID);
                    entity.MAP_SELECTION_ID = map.MAP_SELECTION_ID;
                    ctx.SaveChanges();
                }
                else
                {
                    prepareProjectMilestoneCommand(project.projectMileStones, false, project.ID);

                    var mapSection = ctx.MAP_SELECTIONS.FirstOrDefault(s => s.MAP_SELECTION_ID == project.mapSelection.mapID);
                    if (mapSection != null)
                    {
                        mapSection.ZOOM_LEVEL = project.mapSelection.zoomLevel;
                        mapSection.CENTER_LAT = project.mapSelection.centerLatitude;
                        mapSection.CENTER_LONG = project.mapSelection.centerLongitude;
                        mapSection.SELECTION_TYPE = project.mapSelection.projectMapSelectionType.ToString();
                        mapSection.CIRCLE_DIAMETER = project.mapSelection.circleDiameter.ToString();
                    }

                }

                ctx.SaveChanges();



                response.responseStatus = true;

            }
            catch (Exception ex)
            {
                response.responseStatus = false;
                response.errorMessage = ex.Message;
                response.comments = ex.StackTrace;
                response.endUserMessage = Localization.ErrorMessages.ErrorWhileConnectingDBpleaseConsultAdmin;
            }
            //try
            //{
            //    using (SqlConnection dbConnection = new SqlConnection(
            //        ConfigurationManager.ConnectionStrings["dbConnectionString"].ConnectionString))
            //    using (SqlCommand dbCommand = dbConnection.CreateCommand())
            //    {
            //        dbCommand.CommandText = "SP_PROJECTS";
            //        dbCommand.CommandType = CommandType.StoredProcedure;
            //        dbCommand.Parameters.Add("INPUT_ID", SqlDbType.Int).Value = project.ID;
            //        dbCommand.Parameters.Add("I_NAME", SqlDbType.NVarChar).Value = project.name;
            //        dbCommand.Parameters.Add("I_START_DATE", SqlDbType.DateTime).Value = project.startDate;
            //        dbCommand.Parameters.Add("I_END_DATE", SqlDbType.DateTime).Value = project.endDate;
            //        dbCommand.Parameters.Add("I_IS_ACTIVE", SqlDbType.Char).Value = (project.isActive) ? 'Y' : 'N';
            //        dbCommand.Parameters.Add("I_PROJECT_OWNER_ID", SqlDbType.Int).Value = project.projectOwnerID;
            //        dbCommand.Parameters.Add("I_SUPERVISOR_ENG_ID", SqlDbType.Int).Value = project.supervisorEngID;
            //        dbCommand.Parameters.Add("I_SECTION_ID", SqlDbType.Int).Value = project.departmentSectionID;
            //        dbCommand.Parameters.Add("I_CONSULTANT_ID", SqlDbType.Int).Value = project.consultantID;
            //        dbCommand.Parameters.Add("I_CONSULANT_ASSIST_ID", SqlDbType.Int).Value = project.consultantAssistantID;
            //        dbCommand.Parameters.Add("I_CONTRACTOR_ID", SqlDbType.Int).Value = project.contractorID;
            //        dbCommand.Parameters.Add("I_CONTRACTOR_ASSIST_ID", SqlDbType.Int).Value = project.contractorAssistantID;
            //        dbCommand.Parameters.Add("I_AUTH_LAB_ID", SqlDbType.Int).Value = project.authorizedLabID;
            //        dbCommand.Parameters.Add("I_QA_TECHNICIAN_ID", SqlDbType.Int).Value = project.QATechnicianID;
            //        dbCommand.Parameters.Add("I_Quality_Eng_ID", SqlDbType.Int).Value = project.QualityAssuranceEngID;
            //        dbCommand.Parameters.Add("I_ZOOM_LEVEL", SqlDbType.Int).Value = project.mapSelection.zoomLevel;
            //        dbCommand.Parameters.Add("I_CENTER_LAT", SqlDbType.NVarChar).Value = project.mapSelection.centerLatitude;
            //        dbCommand.Parameters.Add("I_CENTER_LONG", SqlDbType.NVarChar).Value = project.mapSelection.centerLongitude;
            //        dbCommand.Parameters.Add("I_SELECTION_TYPE", SqlDbType.NVarChar).Value =
            //            project.mapSelection.projectMapSelectionType.ToString();
            //        dbCommand.Parameters.Add("I_PROJECT_ITEMS_Type", SqlDbType.NVarChar).Value
            //            = prepareProjectMilestoneCommand(project.projectMileStones, isUpdateForm, project.ID);
            //        dbCommand.Parameters.Add("I_GEOMETRY_SHAPE_txt", SqlDbType.NVarChar).Value
            //            = prepareGeometryCommand(project.mapSelection.projectMapSelectionType, project.mapSelection.mapPoints);
            //        dbCommand.Parameters.Add("I_MAKER", SqlDbType.NVarChar).Value = project.makerID;
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

            return response;
        }

        private bool prepareProjectMilestoneCommand(List<Ent_ProjectItem> lstProjectMilestones
            , bool isUpdateForm, int projectID)
        {

            try
            {
                foreach (var item in lstProjectMilestones)
                {
                    var entity = new PROJECT_ITEMS();
                    if (item.ID > 0)
                        entity = ctx.PROJECT_ITEMS.FirstOrDefault(d => d.PROJECT_ITEMS_ID == item.ID);

                    entity.PROJECT_ID = projectID;
                    entity.NAME = item.name;
                    entity.DESCRIPTION = item.description;
                    entity.AMOUNT = int.Parse(item.amount.ToString());
                    entity.UNIT_ID = item.amountUnitID;
                    entity.ITEM_PERCENTAGE = item.percentage;
                    if (item.ID < 0)
                        ctx.PROJECT_ITEMS.Add(entity);
                    ctx.SaveChanges();

                    return true;

                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return true;
            //try
            //{
            //    string txtCommand = "";

            //    if (isUpdateForm)
            //    {
            //        txtCommand = "INSERT ALL ";
            //        foreach (Ent_ProjectItem milestone in lstProjectMilestones)
            //        {
            //            txtCommand += "Into PROJECT_ITEMS Values (get_ProjectMilestones_seq, " + projectID
            //                + ", '" + milestone.name + "', '" + milestone.description + "', " + milestone.amount
            //                + ", " + milestone.amountUnitID + ", " + milestone.percentage + ") ";
            //        }
            //        txtCommand += "SELECT 1 FROM dual ";
            //    }
            //    else
            //    {
            //        txtCommand = "INSERT ALL ";
            //        foreach (Ent_ProjectItem milestone in lstProjectMilestones)
            //        {
            //            txtCommand += "Into PROJECT_ITEMS Values (get_ProjectMilestones_seq" +
            //                ", SEQ_PROJECTS_ID.currval, '" + milestone.name + "', '" + milestone.description + "', "
            //                + milestone.amount + ", " + milestone.amountUnitID + ", " + milestone.percentage + ") ";
            //        }
            //        txtCommand += "SELECT 1 FROM dual ";
            //    }

            //    return txtCommand;
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}
        }

        private string prepareProjectMapPointsCommand(List<Ent_MapPoint> lstProjectMapPoints
            , bool isUpdateForm, int projectID)
        {
            try
            {
                string txtCommand = "";

                if (isUpdateForm)
                {
                    txtCommand = "INSERT ALL ";
                    foreach (Ent_MapPoint mapPoint in lstProjectMapPoints)
                    {
                        txtCommand += "Into MAP_POINTS Values ((Select MAP_SELECTION_ID From PROJECTS Where PROJECTS_ID = "
                            + projectID + "), get_ProjectMapPoints_seq, '" + mapPoint.pointLatitude + "', '"
                            + mapPoint.pointLongitude + "') ";
                    }
                    txtCommand += "SELECT 1 FROM dual ";
                }
                else
                {
                    txtCommand = "INSERT ALL ";
                    foreach (Ent_MapPoint mapPoint in lstProjectMapPoints)
                    {
                        txtCommand += "Into MAP_POINTS Values (SEQ_MAP_SELECTION_ID.currval, "
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

            return sdo_geometry_command_text;
        }

        public List<Ent_Project> searchProject(string searchName, int searchProjectOwnerID, int searchSupervisorEngID
            , int searchDepartmentID, int searchDepartmentSectionID, int searchConsultantID
            , int searchConsultantAssistantID, int searchContractorID, int searchContractorAssistantID
            , int searchAuthorizedLabID, int searchQATechnicianID, int searchQualityEngineerID
            , int searchIsActive)
        {
            List<Ent_Project> searchResult = new List<Ent_Project>();

            var query = from proj in ctx.PROJECTS
                        join prof in ctx.USERS_PROFILE on proj.SUPERVISOR_ENG_ID equals prof.PROFILE_ID
                        join projOwn in ctx.PROJECT_OWNER on proj.PROJECT_OWNER_ID equals projOwn.PROJEC_OWNER_ID
                        join depSec in ctx.DEPARTMENT_SECTION on proj.SECTION_ID equals depSec.DEPARTMENT_SECTION_ID
                        join dep in ctx.DEPARTMENTs on depSec.DEPARTMENT_ID equals dep.DEPARTMENT_ID
                        join ce in ctx.USERS_PROFILE on proj.CONSULTANT_ID equals ce.PROFILE_ID
                        from cee in ctx.USERS_PROFILE.DefaultIfEmpty()
                        join CEA in ctx.USERS_PROFILE on proj.CONSULANT_ASSIST_ID equals CEA.PROFILE_ID
                        from CEAE in ctx.USERS_PROFILE.DefaultIfEmpty()
                        join CNE in ctx.USERS_PROFILE on proj.CONTRACTOR_ID equals CNE.PROFILE_ID
                        join CNEA in ctx.USERS_PROFILE on proj.CONTRACTOR_ASSIST_ID equals CNEA.PROFILE_ID
                        join AL in ctx.USERS_PROFILE on proj.AUTH_LAB_ID equals AL.PROFILE_ID
                        join QT in ctx.USERS_PROFILE on proj.QA_TECHNICIAN_ID equals QT.PROFILE_ID
                        join QAE in ctx.USERS_PROFILE on proj.QUALITY_ENG_ID equals QAE.PROFILE_ID

                        select new Ent_Project()
                        {
                            ID = proj.PROJECTS_ID,
                            name = proj.NAME,
                            startDate = proj.START_DATE != null ? proj.START_DATE.Value : default(DateTime),
                            endDate = proj.END_DATE != null ? proj.END_DATE.Value : default(DateTime),
                            registerDate = proj.REGISTER_DATE != null ? proj.REGISTER_DATE.Value : default(DateTime),
                            isActive = proj.IS_ACTIVE,
                            projectOwnerID = proj.PROJECT_OWNER_ID.HasValue ? proj.PROJECT_OWNER_ID.Value : 0,
                            projectOwnerName = projOwn.NAME,
                            supervisorEngID = proj.SUPERVISOR_ENG_ID.HasValue ? proj.SUPERVISOR_ENG_ID.Value : 0,
                            supervisorEngName = prof.NAME,
                            departmentID = depSec.DEPARTMENT_ID.HasValue ? depSec.DEPARTMENT_ID.Value : 0,
                            departmentName = dep.NAME,
                            departmentSectionID = depSec.DEPARTMENT_SECTION_ID,
                            departmentSectionName = depSec.NAME,
                            contractorID = CNE.PROFILE_ID,
                            contractorName = CNE.NAME,
                            contractorAssistantID = CNEA.PROFILE_ID,
                            contractorAssistantName = CNEA.NAME,
                            authorizedLabID = AL.PROFILE_ID,
                            authorizedLabName = AL.NAME,
                            QATechnicianID = proj.QA_TECHNICIAN_ID.HasValue ? proj.QA_TECHNICIAN_ID.Value : 0,
                            QATechnicianName = QT.NAME,
                            QualityAssuranceEngID = proj.QUALITY_ENG_ID.HasValue ? proj.QUALITY_ENG_ID.Value : 0,
                            QualityAssuranceEngName = QAE.NAME
                        };

            if (!string.IsNullOrEmpty(searchName))
            {
                searchName = searchName.ToLower();
                query = query.Where(s => s.name.ToLower().Contains(searchName));
            }

            if (searchProjectOwnerID > 0)
            {
                query = query.Where(s => s.projectOwnerID == searchProjectOwnerID);
            }

            if (searchSupervisorEngID > 0)
            {
                query = query.Where(s => s.supervisorEngID == searchSupervisorEngID);
            }


            if (searchDepartmentID > 0)
            {
                query = query.Where(s => s.departmentID == searchDepartmentID);
            }

            if (searchDepartmentSectionID > 0)
            {
                query = query.Where(s => s.departmentSectionID == searchDepartmentSectionID);
            }

            if (searchConsultantID > 0)
            {
                query = query.Where(s => s.consultantAssistantID == searchConsultantID);
            }

            if (searchContractorAssistantID > 0)
            {
                query = query.Where(s => s.consultantAssistantID == searchContractorAssistantID);
            }

            if (searchContractorID > 0)
            {
                query = query.Where(s => s.contractorID == searchContractorID);
            }

            if (searchContractorAssistantID > 0)
            {
                query = query.Where(s => s.contractorAssistantID == searchContractorAssistantID);
            }

            if (searchAuthorizedLabID > 0)
            {
                query = query.Where(s => s.authorizedLabID == searchAuthorizedLabID);
            }

            if (searchQATechnicianID > 0)
            {
                query = query.Where(s => s.QATechnicianID == searchQATechnicianID);
            }

            if (searchQualityEngineerID > 0)
            {
                query = query.Where(s => s.QualityAssuranceEngID == searchQualityEngineerID);
            }

            if (searchIsActive >= 0)
            {
                query = query.Where(s => s.isActive == (searchIsActive == 1 ? true : false));
            }

            searchResult = query.ToList();
            //try
            //{
            //    using (SqlConnection dbConnection = new SqlConnection(
            //        ConfigurationManager.ConnectionStrings["dbConnectionString"].ConnectionString))
            //    using (SqlCommand dbCommand = dbConnection.CreateCommand())
            //    {
            //        dbCommand.CommandText = "SP_PROJECTS_Search";
            //        dbCommand.CommandType = CommandType.StoredProcedure;
            //        dbCommand.Parameters.Add("I_SearchName", SqlDbType.NVarChar).Value = searchName ?? "";
            //        dbCommand.Parameters.Add("I_PROJECT_OWNER_ID", SqlDbType.Int).Value = searchProjectOwnerID;
            //        dbCommand.Parameters.Add("I_SUPERVISOR_ENG_ID", SqlDbType.Int).Value = searchSupervisorEngID;
            //        dbCommand.Parameters.Add("I_DEPARTMENT_ID", SqlDbType.Int).Value = searchDepartmentID;
            //        dbCommand.Parameters.Add("I_SECTION_ID", SqlDbType.Int).Value = searchDepartmentSectionID;
            //        dbCommand.Parameters.Add("I_CONSULTANT_ID", SqlDbType.Int).Value = searchConsultantID;
            //        dbCommand.Parameters.Add("I_CONSULANT_ASSIST_ID", SqlDbType.Int).Value = searchConsultantAssistantID;
            //        dbCommand.Parameters.Add("I_CONTRACTOR_ID", SqlDbType.Int).Value = searchContractorID;
            //        dbCommand.Parameters.Add("I_CONTRACTOR_ASSIST_ID", SqlDbType.Int).Value = searchContractorAssistantID;
            //        dbCommand.Parameters.Add("I_AUTH_LAB_ID", SqlDbType.Int).Value = searchAuthorizedLabID;
            //        dbCommand.Parameters.Add("I_QA_TECHNICIAN_ID", SqlDbType.Int).Value = searchQATechnicianID;
            //        dbCommand.Parameters.Add("I_Quality_Eng_ID", SqlDbType.Int).Value = searchQualityEngineerID;
            //        dbCommand.Parameters.Add("I_IS_ACTIVE", SqlDbType.NVarChar).Value = (searchIsActive == 0) ? 'Y' : (searchIsActive == 1) ? 'N' : 'E';
            //        dbCommand.Parameters.Add("searchResultCursor", SqlDbType.Int).Direction = ParameterDirection.Output;

            //        dbConnection.Open();
            //        SqlDataReader reader = dbCommand.ExecuteReader();
            //        while (reader.Read())
            //        {
            //            searchResult.Add(new Ent_Project()
            //            {
            //                ID = reader.GetInt32(reader.GetOrdinal("PROJECTS_ID")),
            //                name = reader.GetString(reader.GetOrdinal("NAME")),
            //                startDate = reader.GetDateTime(reader.GetOrdinal("START_DATE")),
            //                endDate = reader.GetDateTime(reader.GetOrdinal("END_DATE")),
            //                registerDate = reader.GetDateTime(reader.GetOrdinal("REGISTER_DATE")),
            //                projectOwnerID = reader.GetInt32(reader.GetOrdinal("PROJECT_OWNER_ID")),
            //                projectOwnerName = reader.GetString(reader.GetOrdinal("PROJECT_OWNER_Name")),
            //                supervisorEngID = reader.GetInt32(reader.GetOrdinal("SUPERVISOR_ENG_ID")),
            //                supervisorEngName = reader.GetString(reader.GetOrdinal("SUPERVISOR_ENG_Name")),
            //                departmentID = reader.GetInt32(reader.GetOrdinal("DEPARTMENT_ID")),
            //                departmentName = reader.GetString(reader.GetOrdinal("DEPARTMENT_Name")),
            //                departmentSectionID = reader.GetInt32(reader.GetOrdinal("SECTION_ID")),
            //                departmentSectionName = reader.GetString(reader.GetOrdinal("SECTION_Name")),
            //                consultantID = (reader.IsDBNull(reader.GetOrdinal("CONSULTANT_ID")) ? -1
            //                    : reader.GetInt32(reader.GetOrdinal("CONSULTANT_ID"))),
            //                consultantName = (reader.IsDBNull(reader.GetOrdinal("CONSULTANT_Name")) ? ""
            //                    : reader.GetString(reader.GetOrdinal("CONSULTANT_Name"))),
            //                consultantAssistantID = (reader.IsDBNull(reader.GetOrdinal("CONSULANT_ASSIST_ID")) ? -1
            //                    : reader.GetInt32(reader.GetOrdinal("CONSULANT_ASSIST_ID"))),
            //                consultantAssistantName = (reader.IsDBNull(reader.GetOrdinal("CONSULANT_ASSIST_Name")) ? ""
            //                    : reader.GetString(reader.GetOrdinal("CONSULANT_ASSIST_Name"))),
            //                contractorID = reader.GetInt32(reader.GetOrdinal("CONTRACTOR_ID")),
            //                contractorName = reader.GetString(reader.GetOrdinal("CONTRACTOR_Name")),
            //                contractorAssistantID = reader.GetInt32(reader.GetOrdinal("CONTRACTOR_ASSIST_ID")),
            //                contractorAssistantName = reader.GetString(reader.GetOrdinal("CONTRACTOR_ASSIST_Name")),
            //                authorizedLabID = reader.GetInt32(reader.GetOrdinal("AUTH_LAB_ID")),
            //                authorizedLabName = reader.GetString(reader.GetOrdinal("AUTH_LAB_Name")),
            //                QATechnicianID = reader.GetInt32(reader.GetOrdinal("QA_TECHNICIAN_ID")),
            //                QATechnicianName = reader.GetString(reader.GetOrdinal("QA_TECHNICIAN_Name")),
            //                QualityAssuranceEngID = reader.GetInt32(reader.GetOrdinal("QUALITY_ENG_ID")),
            //                QualityAssuranceEngName = reader.GetString(reader.GetOrdinal("QUALITY_ENG_Name")),
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

            return searchResult;
        }

        public Ent_Project viewProject(int PROJECTS_ID)
        {
            Ent_Project viewResult = new Ent_Project();

            var query = from proj in ctx.PROJECTS
                        join prof in ctx.USERS_PROFILE on proj.SUPERVISOR_ENG_ID equals prof.PROFILE_ID
                        join projOwn in ctx.PROJECT_OWNER on proj.PROJECT_OWNER_ID equals projOwn.PROJEC_OWNER_ID
                        join depSec in ctx.DEPARTMENT_SECTION on proj.SECTION_ID equals depSec.DEPARTMENT_SECTION_ID
                        join dep in ctx.DEPARTMENTs on depSec.DEPARTMENT_ID equals dep.DEPARTMENT_ID
                        join ce in ctx.USERS_PROFILE on proj.CONSULTANT_ID equals ce.PROFILE_ID
                        from cee in ctx.USERS_PROFILE.DefaultIfEmpty()
                        join CEA in ctx.USERS_PROFILE on proj.CONSULANT_ASSIST_ID equals CEA.PROFILE_ID
                        from CEAE in ctx.USERS_PROFILE.DefaultIfEmpty()
                        join CNE in ctx.USERS_PROFILE on proj.CONTRACTOR_ID equals CNE.PROFILE_ID
                        join CNEA in ctx.USERS_PROFILE on proj.CONTRACTOR_ASSIST_ID equals CNEA.PROFILE_ID
                        join AL in ctx.USERS_PROFILE on proj.AUTH_LAB_ID equals AL.PROFILE_ID
                        join QT in ctx.USERS_PROFILE on proj.QA_TECHNICIAN_ID equals QT.PROFILE_ID
                        join QAE in ctx.USERS_PROFILE on proj.QUALITY_ENG_ID equals QAE.PROFILE_ID

                        where proj.PROJECTS_ID == PROJECTS_ID

                        select new Ent_Project()
                        {
                            ID = proj.PROJECTS_ID,
                            name = proj.NAME,
                            startDate = proj.START_DATE != null ? proj.START_DATE.Value : default(DateTime),
                            endDate = proj.END_DATE != null ? proj.END_DATE.Value : default(DateTime),
                            registerDate = proj.REGISTER_DATE != null ? proj.REGISTER_DATE.Value : default(DateTime),
                            isActive = proj.IS_ACTIVE,
                            projectOwnerID = proj.PROJECT_OWNER_ID.HasValue ? proj.PROJECT_OWNER_ID.Value : 0,
                            projectOwnerName = projOwn.NAME,
                            supervisorEngID = proj.SUPERVISOR_ENG_ID.HasValue ? proj.SUPERVISOR_ENG_ID.Value : 0,
                            supervisorEngName = prof.NAME,
                            departmentID = depSec.DEPARTMENT_ID.HasValue ? depSec.DEPARTMENT_ID.Value : 0,
                            departmentName = dep.NAME,
                            departmentSectionID = depSec.DEPARTMENT_SECTION_ID,
                            departmentSectionName = depSec.NAME,
                            contractorID = CNE.PROFILE_ID,
                            contractorName = CNE.NAME,
                            contractorAssistantID = CNEA.PROFILE_ID,
                            contractorAssistantName = CNEA.NAME,
                            authorizedLabID = AL.PROFILE_ID,
                            authorizedLabName = AL.NAME,
                            QATechnicianID = proj.QA_TECHNICIAN_ID.HasValue ? proj.QA_TECHNICIAN_ID.Value : 0,
                            QATechnicianName = QT.NAME,
                            QualityAssuranceEngID = proj.QUALITY_ENG_ID.HasValue ? proj.QUALITY_ENG_ID.Value : 0,
                            QualityAssuranceEngName = QAE.NAME
                        };

            viewResult = query.FirstOrDefault();

            return viewResult;
            //try
            //{
            //    using (SqlConnection dbConnection = new SqlConnection(
            //        ConfigurationManager.ConnectionStrings["dbConnectionString"].ConnectionString))
            //    using (SqlCommand dbCommand = dbConnection.CreateCommand())
            //    {
            //        dbCommand.CommandText = "SP_PROJECTS_VIEW";
            //        dbCommand.CommandType = CommandType.StoredProcedure;
            //        dbCommand.Parameters.Add("input_id", SqlDbType.NVarChar).Value = PROJECTS_ID;
            //        dbCommand.Parameters.Add("viewResultCursor", SqlDbType.Int).Direction = ParameterDirection.Output;
            //        dbCommand.Parameters.Add("viewMileStonesCursor", SqlDbType.Int).Direction = ParameterDirection.Output;
            //        dbCommand.Parameters.Add("viewMapSelectionsCursor", SqlDbType.Int).Direction = ParameterDirection.Output;
            //        dbCommand.Parameters.Add("viewMapPointsCursor", SqlDbType.Int).Direction = ParameterDirection.Output;

            //        dbConnection.Open();
            //        SqlDataReader reader = dbCommand.ExecuteReader(CommandBehavior.CloseConnection);
            //        if (reader.Read())
            //        {
            //            viewResult = new Ent_Project()
            //            {
            //                ID = reader.GetInt32(reader.GetOrdinal("PROJECTS_ID")),
            //                name = reader.GetString(reader.GetOrdinal("NAME")),
            //                startDate = reader.GetDateTime(reader.GetOrdinal("START_DATE")),
            //                endDate = reader.GetDateTime(reader.GetOrdinal("END_DATE")),
            //                registerDate = reader.GetDateTime(reader.GetOrdinal("REGISTER_DATE")),
            //                projectOwnerID = reader.GetInt32(reader.GetOrdinal("PROJECT_OWNER_ID")),
            //                projectOwnerName = reader.GetString(reader.GetOrdinal("PROJECT_OWNER_Name")),
            //                supervisorEngID = reader.GetInt32(reader.GetOrdinal("SUPERVISOR_ENG_ID")),
            //                supervisorEngName = reader.GetString(reader.GetOrdinal("SUPERVISOR_ENG_Name")),
            //                departmentID = reader.GetInt32(reader.GetOrdinal("DEPARTMENT_ID")),
            //                departmentName = reader.GetString(reader.GetOrdinal("DEPARTMENT_Name")),
            //                departmentSectionID = reader.GetInt32(reader.GetOrdinal("SECTION_ID")),
            //                departmentSectionName = reader.GetString(reader.GetOrdinal("SECTION_Name")),
            //                consultantID = (reader.IsDBNull(reader.GetOrdinal("CONSULTANT_ID")) ? -1
            //                    : reader.GetInt32(reader.GetOrdinal("CONSULTANT_ID"))),
            //                consultantName = (reader.IsDBNull(reader.GetOrdinal("CONSULTANT_Name")) ? ""
            //                    : reader.GetString(reader.GetOrdinal("CONSULTANT_Name"))),
            //                consultantAssistantID = (reader.IsDBNull(reader.GetOrdinal("CONSULANT_ASSIST_ID")) ? -1
            //                    : reader.GetInt32(reader.GetOrdinal("CONSULANT_ASSIST_ID"))),
            //                consultantAssistantName = (reader.IsDBNull(reader.GetOrdinal("CONSULANT_ASSIST_Name")) ? ""
            //                    : reader.GetString(reader.GetOrdinal("CONSULANT_ASSIST_Name"))),
            //                contractorID = reader.GetInt32(reader.GetOrdinal("CONTRACTOR_ID")),
            //                contractorName = reader.GetString(reader.GetOrdinal("CONTRACTOR_Name")),
            //                contractorAssistantID = reader.GetInt32(reader.GetOrdinal("CONTRACTOR_ASSIST_ID")),
            //                contractorAssistantName = reader.GetString(reader.GetOrdinal("CONTRACTOR_ASSIST_Name")),
            //                authorizedLabID = reader.GetInt32(reader.GetOrdinal("AUTH_LAB_ID")),
            //                authorizedLabName = reader.GetString(reader.GetOrdinal("AUTH_LAB_Name")),
            //                QATechnicianID = reader.GetInt32(reader.GetOrdinal("QA_TECHNICIAN_ID")),
            //                QATechnicianName = reader.GetString(reader.GetOrdinal("QA_TECHNICIAN_Name")),
            //                QualityAssuranceEngID = reader.GetInt32(reader.GetOrdinal("QUALITY_ENG_ID")),
            //                QualityAssuranceEngName = reader.GetString(reader.GetOrdinal("QUALITY_ENG_Name")),
            //                isActive = (reader.GetString(reader.GetOrdinal("IS_ACTIVE")) == "Y") ? true : false
            //            };
            //        }

            //        reader.NextResult();
            //        viewResult.projectMileStones = new List<Ent_ProjectItem>();
            //        while (reader.Read())
            //        {
            //            viewResult.projectMileStones.Add(new Ent_ProjectItem()
            //            {
            //                name = reader.GetString(reader.GetOrdinal("NAME")),
            //                description = reader.GetString(reader.GetOrdinal("DESCRIPTION")),
            //                amount = reader.GetInt32(reader.GetOrdinal("AMOUNT")),
            //                amountUnitID = reader.GetInt32(reader.GetOrdinal("UNIT_ID")),
            //                txtAmountUnit = reader.GetString(reader.GetOrdinal("UNIT_Name")),
            //                percentage = reader.GetInt32(reader.GetOrdinal("ITEM_PERCENTAGE")),
            //            });
            //        }

            //        reader.NextResult();
            //        if (reader.Read())
            //        {
            //            viewResult.mapSelection = new Ent_MapSelection()
            //            {
            //                zoomLevel = reader.GetInt32(reader.GetOrdinal("ZOOM_LEVEL")),
            //                centerLatitude = reader.GetString(reader.GetOrdinal("CENTER_LAT")),
            //                centerLongitude = reader.GetString(reader.GetOrdinal("CENTER_LONG")),
            //                projectMapSelectionType = Ent_MapSelection.mapSelectionType.Polygon
            //            };
            //        }

            //        reader.NextResult();
            //        viewResult.mapSelection.mapPoints = new List<Ent_MapPoint>();
            //        while (reader.Read())
            //        {
            //            viewResult.mapSelection.mapPoints.Add(new Ent_MapPoint()
            //            {
            //                pointLatitude = reader.GetString(reader.GetOrdinal("LATITUDE")),
            //                pointLongitude = reader.GetString(reader.GetOrdinal("LONGITUDE")),
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

            //return viewResult;
        }

        public Ent_MapSelection getProjectMapDetails(int PROJECTS_ID)
        {
            Ent_MapSelection vw_mapSelection = new Ent_MapSelection();
            var project = ctx.PROJECTS.FirstOrDefault(s => s.PROJECTS_ID == PROJECTS_ID);
            if (project != null && project.SECTION_ID != null)
            {
                var sectionMap = ctx.MAP_SELECTIONS.FirstOrDefault(s => s.MAP_SELECTION_ID == project.SECTION_ID);
                if (sectionMap != null)
                {
                    vw_mapSelection = new Ent_MapSelection
                    {
                        mapID = sectionMap.MAP_SELECTION_ID,
                        zoomLevel = sectionMap.ZOOM_LEVEL.HasValue ? sectionMap.ZOOM_LEVEL.Value : 0,
                        centerLatitude = sectionMap.CENTER_LAT,
                        centerLongitude = sectionMap.CENTER_LONG,
                        projectMapSelectionType = (mapSelectionType)Enum.Parse(typeof(mapSelectionType), sectionMap.SELECTION_TYPE),
                    };
                    var points = ctx.MAP_POINTS.Where(s => s.MAP_ID == vw_mapSelection.mapID)
                                    .Select(mod => new Ent_MapPoint
                                    {
                                        pointLatitude = mod.LATITUDE,
                                        pointLongitude = mod.LONGITUDE,
                                    }).ToList(); 
                }
            }

            return vw_mapSelection;
            //try
            //{
            //    using (SqlConnection dbConnection = new SqlConnection(
            //        ConfigurationManager.ConnectionStrings["dbConnectionString"].ConnectionString))
            //    using (SqlCommand dbCommand = dbConnection.CreateCommand())
            //    {
            //        dbCommand.CommandText = "SP_PROJECT_Map_View";
            //        dbCommand.CommandType = CommandType.StoredProcedure;
            //        dbCommand.Parameters.Add("input_id", SqlDbType.NVarChar).Value = PROJECTS_ID;
            //        dbCommand.Parameters.Add("viewMapSelectionsCursor", SqlDbType.Int).Direction = ParameterDirection.Output;
            //        dbCommand.Parameters.Add("viewMapPointsCursor", SqlDbType.Int).Direction = ParameterDirection.Output;

            //        dbConnection.Open();
            //        SqlDataReader reader = dbCommand.ExecuteReader(CommandBehavior.CloseConnection);
            //        if (reader.Read())
            //        {
            //            vw_mapSelection = new Ent_MapSelection()
            //            {
            //                zoomLevel = reader.GetInt32(reader.GetOrdinal("ZOOM_LEVEL")),
            //                centerLatitude = reader.GetString(reader.GetOrdinal("CENTER_LAT")),
            //                centerLongitude = reader.GetString(reader.GetOrdinal("CENTER_LONG")),
            //                projectMapSelectionType = Ent_MapSelection.mapSelectionType.Polygon
            //            };
            //        }

            //        reader.NextResult();
            //        vw_mapSelection.mapPoints = new List<Ent_MapPoint>();
            //        while (reader.Read())
            //        {
            //            vw_mapSelection.mapPoints.Add(new Ent_MapPoint()
            //            {
            //                pointLatitude = reader.GetString(reader.GetOrdinal("LATITUDE")),
            //                pointLongitude = reader.GetString(reader.GetOrdinal("LONGITUDE")),
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

        public Rpt_Project printProjectDetails(int projectID)
        {
            Rpt_Project viewResult = null;

            try
            {
                using (SqlConnection dbConnection = new SqlConnection(
                    ConfigurationManager.ConnectionStrings["dbConnectionString"].ConnectionString))
                using (SqlCommand dbCommand = dbConnection.CreateCommand())
                {
                    dbCommand.CommandText = "SP_Project_Print";
                    dbCommand.CommandType = CommandType.StoredProcedure;

                    dbCommand.Parameters.Add("I_PROJECTS_ID", SqlDbType.Int).Value = projectID;

                    dbCommand.Parameters.Add("O_Project_Name", SqlDbType.NVarChar, 150).Direction = ParameterDirection.Output;
                    dbCommand.Parameters.Add("O_Project_POName", SqlDbType.NVarChar, 150).Direction = ParameterDirection.Output;
                    dbCommand.Parameters.Add("O_Project_DepName", SqlDbType.NVarChar, 150).Direction = ParameterDirection.Output;
                    dbCommand.Parameters.Add("O_Project_DepSecName", SqlDbType.NVarChar, 150).Direction = ParameterDirection.Output;
                    dbCommand.Parameters.Add("O_Project_ContractorName", SqlDbType.NVarChar, 150).Direction = ParameterDirection.Output;
                    dbCommand.Parameters.Add("O_Project_SuperEngName", SqlDbType.NVarChar, 150).Direction = ParameterDirection.Output;
                    dbCommand.Parameters.Add("O_Project_ConsEngName", SqlDbType.NVarChar, 150).Direction = ParameterDirection.Output;
                    dbCommand.Parameters.Add("O_Project_AuthLabName", SqlDbType.NVarChar, 150).Direction = ParameterDirection.Output;
                    dbCommand.Parameters.Add("O_ZOOM_LEVEL", SqlDbType.Int).Direction = ParameterDirection.Output;

                    dbCommand.Parameters.Add("O_CR_Total", SqlDbType.Int).Direction = ParameterDirection.Output;
                    dbCommand.Parameters.Add("O_CR_WIP", SqlDbType.Int).Direction = ParameterDirection.Output;
                    dbCommand.Parameters.Add("O_CR_Closed", SqlDbType.Int).Direction = ParameterDirection.Output;
                    dbCommand.Parameters.Add("O_CR_SuperEng_Pending", SqlDbType.Int).Direction = ParameterDirection.Output;
                    dbCommand.Parameters.Add("O_CR_SuperEng_Recieved", SqlDbType.Int).Direction = ParameterDirection.Output;
                    dbCommand.Parameters.Add("O_CR_SuperEng_Accepted", SqlDbType.Int).Direction = ParameterDirection.Output;
                    dbCommand.Parameters.Add("O_CR_SuperEng_Rejected", SqlDbType.Int).Direction = ParameterDirection.Output;
                    dbCommand.Parameters.Add("O_CR_ConsEng_Pending", SqlDbType.Int).Direction = ParameterDirection.Output;
                    dbCommand.Parameters.Add("O_CR_ConsEng_Recieved", SqlDbType.Int).Direction = ParameterDirection.Output;
                    dbCommand.Parameters.Add("O_CR_ConsEng_Accepted", SqlDbType.Int).Direction = ParameterDirection.Output;
                    dbCommand.Parameters.Add("O_CR_ConsEng_Rejected", SqlDbType.Int).Direction = ParameterDirection.Output;
                    dbCommand.Parameters.Add("O_CR_AuthLab_Pending", SqlDbType.Int).Direction = ParameterDirection.Output;
                    dbCommand.Parameters.Add("O_CR_AuthLab_Recieved", SqlDbType.Int).Direction = ParameterDirection.Output;
                    dbCommand.Parameters.Add("O_CR_AuthLab_Accepted", SqlDbType.Int).Direction = ParameterDirection.Output;
                    dbCommand.Parameters.Add("O_CR_AuthLab_Rejected", SqlDbType.Int).Direction = ParameterDirection.Output;

                    dbCommand.Parameters.Add("O_Complaint_Total", SqlDbType.Int).Direction = ParameterDirection.Output;
                    dbCommand.Parameters.Add("O_Complaint_WIP", SqlDbType.Int).Direction = ParameterDirection.Output;
                    dbCommand.Parameters.Add("O_Complaint_Closed", SqlDbType.Int).Direction = ParameterDirection.Output;

                    dbCommand.Parameters.Add("searchResultCursor", SqlDbType.Int).Direction = ParameterDirection.Output;
                    dbCommand.Parameters.Add("viewMapPointsCursor", SqlDbType.Int).Direction = ParameterDirection.Output;

                    dbConnection.Open();
                    SqlDataReader reader = dbCommand.ExecuteReader(CommandBehavior.CloseConnection);

                    viewResult = new Rpt_Project()
                    {
                        projectID = projectID,
                        projectName = dbCommand.Parameters["O_Project_Name"].Value.ToString(),
                        projectOwnerName = dbCommand.Parameters["O_Project_POName"].Value.ToString(),
                        departmentName = dbCommand.Parameters["O_Project_DepName"].Value.ToString(),
                        departmentSectionName = dbCommand.Parameters["O_Project_DepSecName"].Value.ToString(),
                        contractorName = dbCommand.Parameters["O_Project_ContractorName"].Value.ToString(),
                        superEngName = dbCommand.Parameters["O_Project_SuperEngName"].Value.ToString(),
                        consulatntEngName = dbCommand.Parameters["O_Project_ConsEngName"].Value.ToString(),
                        authLabName = dbCommand.Parameters["O_Project_AuthLabName"].Value.ToString(),
                        mapZoomLevel = int.Parse(dbCommand.Parameters["O_ZOOM_LEVEL"].Value.ToString()),

                        CR_Total = int.Parse(dbCommand.Parameters["O_CR_Total"].Value.ToString()),
                        CR_WIP = int.Parse(dbCommand.Parameters["O_CR_WIP"].Value.ToString()),
                        CR_Closed = int.Parse(dbCommand.Parameters["O_CR_Closed"].Value.ToString()),
                        CR_SuperEng_Pending = int.Parse(dbCommand.Parameters["O_CR_SuperEng_Pending"].Value.ToString()),
                        CR_SuperEng_Recieved = int.Parse(dbCommand.Parameters["O_CR_SuperEng_Recieved"].Value.ToString()),
                        CR_SuperEng_Accepted = int.Parse(dbCommand.Parameters["O_CR_SuperEng_Accepted"].Value.ToString()),
                        CR_SuperEng_Rejected = int.Parse(dbCommand.Parameters["O_CR_SuperEng_Rejected"].Value.ToString()),
                        CR_ConsEng_Pending = int.Parse(dbCommand.Parameters["O_CR_ConsEng_Pending"].Value.ToString()),
                        CR_ConsEng_Recieved = int.Parse(dbCommand.Parameters["O_CR_ConsEng_Recieved"].Value.ToString()),
                        CR_ConsEng_Accepted = int.Parse(dbCommand.Parameters["O_CR_ConsEng_Accepted"].Value.ToString()),
                        CR_ConsEng_Rejected = int.Parse(dbCommand.Parameters["O_CR_ConsEng_Rejected"].Value.ToString()),
                        CR_AuthLab_Pending = int.Parse(dbCommand.Parameters["O_CR_AuthLab_Pending"].Value.ToString()),
                        CR_AuthLab_Recieved = int.Parse(dbCommand.Parameters["O_CR_AuthLab_Recieved"].Value.ToString()),
                        CR_AuthLab_Accepted = int.Parse(dbCommand.Parameters["O_CR_AuthLab_Accepted"].Value.ToString()),
                        CR_AuthLab_Rejected = int.Parse(dbCommand.Parameters["O_CR_AuthLab_Rejected"].Value.ToString()),

                        complaint_Total = int.Parse(dbCommand.Parameters["O_Complaint_Total"].Value.ToString()),
                        complaint_WIP = int.Parse(dbCommand.Parameters["O_Complaint_WIP"].Value.ToString()),
                        complaint_Closed = int.Parse(dbCommand.Parameters["O_Complaint_Closed"].Value.ToString())
                    };

                    viewResult.lstSampleTypesStatus = new List<SampleStatus>();
                    while (reader.Read())
                    {
                        if (!(reader.IsDBNull(reader.GetOrdinal("SampleType"))))
                        {
                            viewResult.lstSampleTypesStatus.Add(new SampleStatus()
                            {
                                sampleType = reader.GetString(reader.GetOrdinal("SampleType")),
                                accepted = reader.GetInt32(reader.GetOrdinal("Sample_Type_Accepted")),
                                rejected = reader.GetInt32(reader.GetOrdinal("Sample_Type_Rejected"))
                            });
                        }
                    }

                    reader.NextResult();
                    viewResult.projectMapPoints = new List<Ent_MapPoint>();
                    while (reader.Read())
                    {
                        viewResult.projectMapPoints.Add(new Ent_MapPoint()
                        {
                            pointLatitude = reader.GetString(reader.GetOrdinal("LATITUDE")),
                            pointLongitude = reader.GetString(reader.GetOrdinal("LONGITUDE")),
                        });
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
    }
}