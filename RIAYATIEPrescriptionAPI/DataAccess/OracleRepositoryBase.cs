using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;

namespace DataAccess
{
    public class OracleRepositoryBase : RepositoryBase
    {
        public string ConnectionString { get; set; } = ConfigurationManager.ConnectionStrings["OracleConnectionString"].ToString();
        public string PackageName { get; set; } = ConfigurationManager.AppSettings["PackageName"].ToString();
        public int LicenseExpiration { get; set; } = Convert.ToInt32(string.IsNullOrEmpty(ConfigurationManager.AppSettings["LICENSES_EXPIRATION"])? "3600" : ConfigurationManager.AppSettings["LICENSES_EXPIRATION"]);
      
        private static OracleConnection con;
        public OracleConnection OpenConnection()
        {
            con = new OracleConnection(ConnectionString);
            con.Open();
            return con;
        }

        public OracleCommand CreateCommand(string commandText, CommandType commandType = CommandType.StoredProcedure)
        {
            var cmd = new OracleCommand(commandText, con);
            cmd.CommandType = commandType;
            return cmd;
        }

        public void CloseConnection()
        {
            try
            {
                if (con != null && con.State == System.Data.ConnectionState.Open)
                {
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string DbSchema
        {
            get
            {
                return ConfigurationManager.AppSettings["DB_SCHEMA"];
            }
        }

        public OracleDbType GetOracleDbType(object value)
        {
            if (value == null)
            {
                throw new Exception("Invalid data type value passed");
            }
            Type type = value.GetType();

            if (type == typeof(string))
            {
                return OracleDbType.NVarchar2;
            }
            else if (type == typeof(int))
            {
                return OracleDbType.Int32;
            }
            else if (type == typeof(long))
            {
                return OracleDbType.Int64;
            }
            else if (type == typeof(decimal))
            {
                return OracleDbType.Decimal;
            }
            else if (type == typeof(double))
            {
                return OracleDbType.Double;
            }
            else if (type == typeof(DateTime))
            {
                return OracleDbType.Date;
            }
            else if (type == typeof(bool))
            {
                return OracleDbType.Boolean;
            }
            else if (type == typeof(byte[]))
            {
                return OracleDbType.Blob;
            }
            else
            {
                return OracleDbType.Varchar2;
            }
        }

        public void BulkInsertData(OracleConnection con, string tableName, List<Dictionary<string, object>> data)
        {
            if (data == null || data.Count == 0)
            {                
                return;
            }

            // Get the column names from the first data row.  Assumes all rows have the same columns.
            string[] columnNames = new string[data[0].Count];
            data[0].Keys.CopyTo(columnNames, 0);
            // Construct the SQL INSERT statement.
            string sql = $"INSERT INTO {tableName} ({string.Join(", ", columnNames)}) VALUES ({string.Join(", ", Array.ConvertAll(columnNames, name => ":" + name))})";

            using (OracleConnection connection = con)
            {
                try
                {
                    connection.Open();
                    using (OracleCommand command = new OracleCommand(sql, connection))
                    {
                        // Add parameters to the command.  Create parameters *once* and then reuse them.
                        foreach (string columnName in columnNames)
                        {
                            OracleParameter parameter = new OracleParameter(columnName, null); // Start with null value
                            parameter.OracleDbType = GetOracleDbType(data[0][columnName]); // Set OracleDbType
                            command.Parameters.Add(parameter);
                        }

                        // Loop through the data and set parameter values for each row.
                        foreach (Dictionary<string, object> rowData in data)
                        {
                            foreach (string columnName in columnNames)
                            {
                                command.Parameters[columnName].Value = rowData[columnName];
                            }
                            command.ExecuteNonQuery(); // Execute the INSERT for each row.
                        }                        
                    }
                }
                catch (OracleException ex)
                {                   
                    throw ex;
                }                
            }
        }
    }
}
