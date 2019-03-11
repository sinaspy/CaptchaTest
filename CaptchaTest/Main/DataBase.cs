using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;

namespace CaptchaTest
{
    public static partial class DataBase
    {
        //Base DataBase
        private static void CreateLogMessage(string logLevel, double executionTime, string commandText, CommandType commandType, string result, int code, SqlParameter[] sqlParameters)
        {
            var log = true;
            if (!logLevel.Equals("Debug"))
                log = true;
            else
                log = !ProjectValues.StoredProceduresBlockList.Contains(commandText);

            if (log)
            {
                //var cp = $"(Command Type: {commandType})";
                var ct = $"(Command Text: {commandText})";
                var cd = $"(Code: {code})";
                var rs = $"(Result: {result})";
                var pr = string.Empty;

                for (int i = sqlParameters.Length - 4; i >= 0; i--)
                    if (sqlParameters[i] != null)
                    {
                        var name = sqlParameters[i].ParameterName;
                        var value = sqlParameters[i].Value?.ToString();

                        pr = $"{name}={value} {pr}";
                    }
                pr = $"(SQL Parameters: {pr.Trim()})";

                var arguments = new object[] { ct + cd + rs + pr, executionTime, null, "BaseDataBase", commandText, 0 };
            }

            return;
        }
        private static QueryResult Execute(string commandText, CommandType commandType, SqlParameter[] parameters)
        {
            QueryResult result;
            var dS = new DataSet();
            var sw = Stopwatch.StartNew();
            var returnCodeID = parameters.Length - 1;
            var resultTextID = parameters.Length - 2;
            var spResultCodeID = parameters.Length - 3;

            try
            {
                using (var connection = new SqlConnection(ProjectValues.DataBaseConfigure.ConnectionString))
                using (var command = new SqlCommand(commandText, connection) { CommandTimeout = ProjectValues.DataBaseConfigure.CommandTimeout, CommandType = commandType })
                using (var dataAdaptor = new SqlDataAdapter(command))
                {
                    command.Parameters.AddRange(parameters);

                    connection.Open();
                    dataAdaptor.Fill(dS);
                }

                if ((int)parameters[returnCodeID].Value != 1)
                {
                    result = new QueryResult { ReturnCode = (int)parameters[returnCodeID].Value, Text = (string)parameters[resultTextID].Value };
                    CreateLogMessage("Error", sw.Elapsed.TotalMilliseconds, commandText, commandType, result.Text, result.ReturnCode, parameters);
                    return result;
                }
                if ((int)parameters[spResultCodeID].Value != 1)
                {
                    result = new QueryResult { SpCode = (int)parameters[spResultCodeID].Value, ReturnCode = (int)parameters[returnCodeID].Value, Text = (string)parameters[resultTextID].Value };
                    CreateLogMessage("Error", sw.Elapsed.TotalMilliseconds, commandText, commandType, result.Text, result.SpCode, parameters);
                    return result;
                }
                result = new QueryResult { DataSet = dS, SpCode = (int)parameters[spResultCodeID].Value, ReturnCode = (int)parameters[returnCodeID].Value, Text = (string)parameters[resultTextID].Value };
                CreateLogMessage("Debug", sw.Elapsed.TotalMilliseconds, commandText, commandType, result.Text, 1, parameters);
                return result;
            }
            catch (Exception ex)
            {
                result = new QueryResult { Text = ex.Message.ToString(), ReturnCode = ex.HResult };
                CreateLogMessage("Fatal", sw.Elapsed.TotalMilliseconds, commandText, commandType, ex.ToString(), ex.HResult, parameters);
                return result;
            }
        }
        private static QueryResult ExecuteStoredProcedure(string storedProcedureName, List<SqlParameter> sqlParameters)
        {
            sqlParameters.Add(new SqlParameter { ParameterName = "@output_status", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Output });
            sqlParameters.Add(new SqlParameter { ParameterName = "@output_message", SqlDbType = SqlDbType.VarChar, Size = 4000, Direction = ParameterDirection.Output });
            sqlParameters.Add(new SqlParameter { ParameterName = "@returnvalue", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.ReturnValue });

            return Execute(storedProcedureName, CommandType.StoredProcedure, sqlParameters.ToArray());
        }

        //DataBase
        public static QueryResult GetBillState(string billType, long cityCode)
        {
            var sqlParameters = new List<SqlParameter>();
            var storedProcedureName = "ud_prc_getBillState";

            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.VarChar, ParameterName = "@billType", Value = billType });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.BigInt, ParameterName = "@cityCode", Value = cityCode });
            return ExecuteStoredProcedure(storedProcedureName, sqlParameters);
        }
        public static QueryResult GetDatabaseInformation(string keyName)
        {
            var sqlParameters = new List<SqlParameter>();
            var storedProcedureName = "ud_prc_getDatabaseInformation";

            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.VarChar, ParameterName = "@keyName", Value = keyName });
            return ExecuteStoredProcedure(storedProcedureName, sqlParameters);
        }
    }
}