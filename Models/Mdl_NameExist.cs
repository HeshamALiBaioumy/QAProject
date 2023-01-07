using System;
using System.Configuration;
using System.Data;
using System.Data.OracleClient;
using System.Data.SqlClient;

namespace QA.Models
{

    public class Mdl_NameExist
    {

        public enum searchEntities
        {
            Department, Department_Section
                , PROJECT_OWNER_TYPE, Project_Owner
                , Factory_Type, Factory
                , Mixer, Mixer_Type
                , SAMPLE_TEST
                , SAMPLE_TYPE
                , CRTypeMC, CR_Type_Group, CRType
                , Complaint
                , CheckListItem, CheckListItemGroup, CheckList, CheckListFlowSequence
                , UserProfile, UserProfile_UniqueNID, UserProfile_UniqueEmpID
                , UserRoles
                , PROJECTS
        };

        public bool validateNameExist(searchEntities searchEntity, int ItemID, string ItemName)
        {
            try
            {
                return validateNameExist(searchEntity, ItemID, ItemName, -1);
            }
            catch (Exception ex)
            {
                return true;
            }
        }

        public bool validateNameExist(searchEntities searchEntity, int ItemID, string ItemName, int parentID)
        {
            try
            {
                string searchEntityName = "";

                switch (searchEntity)
                {
                    case searchEntities.Department:
                        searchEntityName = "department";
                        break;
                    case searchEntities.Department_Section:
                        searchEntityName = "DepartmentSection";
                        break;

                    case searchEntities.PROJECT_OWNER_TYPE:
                        searchEntityName = "projectOwnerType";
                        break;
                    case searchEntities.Project_Owner:
                        searchEntityName = "projectOwner";
                        break;

                    case searchEntities.Factory_Type:
                        searchEntityName = "factoryType";
                        break;
                    case searchEntities.Factory:
                        searchEntityName = "factory";
                        break;

                    case searchEntities.Mixer_Type:
                        searchEntityName = "MixerType";
                        break;
                    case searchEntities.Mixer:
                        searchEntityName = "Mixer";
                        break;

                    case searchEntities.SAMPLE_TYPE:
                        searchEntityName = "SampleType";
                        break;
                    case searchEntities.SAMPLE_TEST:
                        searchEntityName = "SampleTest";
                        break;

                    case searchEntities.CRTypeMC:
                        searchEntityName = "CRTypeMC";
                        break;
                    case searchEntities.CR_Type_Group:
                        searchEntityName = "CR_Type_Group";
                        break;
                    case searchEntities.CRType:
                        searchEntityName = "CR_Type";
                        break;

                    case searchEntities.CheckListItem:
                        searchEntityName = "CheckListItem";
                        break;
                    case searchEntities.CheckListItemGroup:
                        searchEntityName = "CheckListItemGroup";
                        break;
                    case searchEntities.CheckList:
                        searchEntityName = "CheckList";
                        break;
                    case searchEntities.CheckListFlowSequence:
                        searchEntityName = "CheckListFlowSequence";
                        break;

                    case searchEntities.UserProfile:
                        searchEntityName = "UserProfile";
                        break;
                    case searchEntities.UserProfile_UniqueNID:
                        searchEntityName = "UserProfile_UniqueNID";
                        break;
                    case searchEntities.UserProfile_UniqueEmpID:
                        searchEntityName = "UserProfile_UniqueEmpID";
                        break;

                    case searchEntities.UserRoles:
                        searchEntityName = "UserRoles";
                        break;

                    case searchEntities.PROJECTS:
                        searchEntityName = "PROJECTS";
                        break;

                    default:
                        throw new Exception("Un-handled LOV Search Case !!");
                }

                return searchNameExist(searchEntityName, ItemID, ItemName, parentID);
            }
            catch (Exception ex)
            {
                return true;
            }
        }

        private bool searchNameExist(string searchEntity, int ItemID, string ItemName, int parentID)
        {
            bool status;
            try
            {

                using (SqlConnection dbConnection = new SqlConnection(
                    ConfigurationManager.ConnectionStrings["dbConnectionString"].ConnectionString))
                using (SqlCommand dbCommand = dbConnection.CreateCommand())
                {
                    dbCommand.CommandText = "SP_CheckNameExist_V2";
                    dbCommand.CommandType = CommandType.StoredProcedure;
                    dbCommand.Parameters.Add("Inpt_EntityName", SqlDbType.NVarChar).Value = searchEntity;
                    dbCommand.Parameters.Add("Search_NAME", SqlDbType.NVarChar).Value = ItemName;
                    dbCommand.Parameters.Add("update_ID", SqlDbType.Int).Value = ItemID;
                    dbCommand.Parameters.Add("Search_Parent_ID", SqlDbType.Int).Value = parentID;
                    dbCommand.Parameters.Add("OutCount", SqlDbType.Int).Direction = ParameterDirection.Output;

                    dbConnection.Open();

                    dbCommand.ExecuteNonQuery();
                    int count = int.Parse(dbCommand.Parameters["OutCount"].Value.ToString());
                    status = (count == 0) ? true : false;

                    dbCommand.Dispose();
                    dbConnection.Close();
                }
            }
            catch (Exception ex)
            {
                status = true;
            }

            return status;
        }
    }
}