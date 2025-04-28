using Oracle.ManagedDataAccess.Types;
using System;
using System.Data;
using System.Data.Common;
using System.Text;

namespace DataAccess
{
    public static class OracleDataReaderEx
    {
        public static int ToInt32(this DbDataReader reader, string columnName)
        {
            if (reader[columnName] is DBNull)
                return 0;
            var ordinal = reader.GetOrdinal(columnName);
            return Convert.ToInt32(reader.GetValue(ordinal));
        }

        public static string ToString(this DbDataReader reader, string columnName)
        {
            if (reader[columnName] is DBNull)
                return string.Empty;
            var ordinal = reader.GetOrdinal(columnName);
            return reader.GetString(ordinal);
        }

        public static bool ToBoolen(this DbDataReader reader, string columnName)
        {
            if (reader[columnName] is DBNull)
                return false;
            var ordinal = reader.GetOrdinal(columnName);
            return reader.GetInt32(ordinal) > 0;
        }

        public static DateTime ToDateTime(this DbDataReader reader, string columnName)
        {
            if (reader[columnName] is DBNull)
                return DateTime.Now;
            var ordinal = reader.GetOrdinal(columnName);
            return reader.GetDateTime(ordinal);
        }

        public static DateTime? ToDateTimeNullable(this DbDataReader reader, string columnName)
        {
            if (reader[columnName] is DBNull)
                return (DateTime?)null;
            var ordinal = reader.GetOrdinal(columnName);
            return reader.GetDateTime(ordinal);
        }

        public static string ToBlobString(this DbDataReader reader, string columnName)
        {
            if (reader[columnName] is DBNull)
                return string.Empty;
            int columnIndex = reader.GetOrdinal(columnName);            
            byte[] blobBytes = (byte[])reader.GetValue(columnIndex);           
            string result = Encoding.UTF8.GetString(blobBytes);
            return result;            
        }
    }
}
