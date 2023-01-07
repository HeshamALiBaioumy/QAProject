using System;
using System.Configuration;
using System.Data;
using System.Data.OracleClient;
using System.Data.SqlClient;
using System.IO;
using System.Text;

namespace QA.Models
{
    public class Mdl_Log
    {
        private string userID;
        private string functionName;
        private string inputParameters;
        private string outputResponse;
        private bool status;
        private Exception logException;

        private Mdl_Log()
        {

        }

        public Mdl_Log(string userID, string functionName, string inputParameters, string outputResponse, bool status)
        {
            this.userID = userID;
            this.functionName = functionName;
            this.inputParameters = inputParameters;
            this.outputResponse = outputResponse;
            this.status = status;
        }

        public Mdl_Log(string userID, string functionName, string inputParameters, string outputResponse, Exception logException
            , bool status)
        {
            this.userID = userID;
            this.functionName = functionName;
            this.inputParameters = inputParameters;
            this.outputResponse = outputResponse;
            this.status = status;
            this.logException = logException;
        }

        public void insertLogMessge()
        {
            try
            {
                if (bool.Parse(ConfigurationManager.AppSettings["AllowDBLog"].ToString()))
                {
                    insertDBLogMessge();
                }

                if (bool.Parse(ConfigurationManager.AppSettings["AllowFileLog"].ToString()))
                {
                    insertFileLogMessge();
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void insertDBLogMessge()
        {
            try
            {
                using (SqlConnection dbConnection = new SqlConnection(
                    ConfigurationManager.ConnectionStrings["dbConnectionString"].ConnectionString))
                using (SqlCommand dbCommand = dbConnection.CreateCommand())
                {
                    dbCommand.CommandText = "SP_LogMessage";
                    dbCommand.CommandType = CommandType.StoredProcedure;
                    dbCommand.Parameters.Add("userID", SqlDbType.NVarChar).Value = this.userID;
                    dbCommand.Parameters.Add("functionName", SqlDbType.NVarChar).Value = this.functionName;
                    dbCommand.Parameters.Add("InputParameters", SqlDbType.NVarChar).Value = this.inputParameters;
                    dbCommand.Parameters.Add("OutputParameters", SqlDbType.NVarChar).Value = this.outputResponse;
                    dbCommand.Parameters.Add("Status", SqlDbType.Char).Value = (this.status) ? 'Y' : 'N';

                    dbConnection.Open();
                    dbCommand.ExecuteNonQuery();
                    dbCommand.Dispose();
                    dbConnection.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void insertFileLogMessge()
        {
            try
            {
                string filePath = AppDomain.CurrentDomain.BaseDirectory + @"\QALog" + DateTime.Now.ToString("dd-MM-yyyy");
                File.AppendAllText(filePath, String.Concat("User ID: ", userID, "\r\nDateTime: ", DateTime.Now
                    , "\r\nfunction Name: ", functionName, "\r\ninput Parameters: ", inputParameters
                    , "\r\noutput Response: ", outputResponse, (logException == null) ? "\r\nlogException: null" 
                        : "\r\nlogException: " + logException.Message + "\r\n" + logException.InnerException + "\r\n" 
                        + logException.StackTrace
                    , "\r\nstatus: ", status
                    , "\r\n -- -- -- -- -- *** -- -- -- -- -- -- \r\n"));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}