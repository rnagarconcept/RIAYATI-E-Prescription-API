using System;
using System.Data;
using System.Data.Common;

namespace DataAccess
{
    public static class OracleDataReaderEx
    {
        public static int ToInt32(this DbDataReader reader, string columnName)
        {
            if (reader[columnName] is DBNull)
                return 0;
            var ordinal = reader.GetOrdinal(columnName);
            return reader.GetInt32(ordinal);
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
    }
}
