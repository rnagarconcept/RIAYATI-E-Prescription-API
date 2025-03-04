using Oracle.ManagedDataAccess.Client;
using System;
using System.Configuration;
using System.Data;

namespace DataAccess
{
    public class OracleRepositoryBase : RepositoryBase
    {
        public string ConnectionString { get; set; } = ConfigurationManager.ConnectionStrings["OracleConnectionString"].ToString();
        public string PackageName { get; set; } = ConfigurationManager.AppSettings["PackageName"].ToString();
      
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
    }
}
