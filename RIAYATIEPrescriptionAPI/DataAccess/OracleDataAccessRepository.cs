using DomainModel;
using DomainModel.Models.Common;
using DomainModel.Models.Response;
using log4net;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System;
using System.Collections.Generic;
using System.Data;

namespace DataAccess
{
    public class OracleDataAccessRepository : OracleRepositoryBase
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(OracleDataAccessRepository));
        private static readonly Lazy<OracleDataAccessRepository> lazy = new Lazy<OracleDataAccessRepository>(() => new OracleDataAccessRepository());
        private OracleDataAccessRepository()
        {
        }
        public static OracleDataAccessRepository GetInstance
        {
            get
            {
                return lazy.Value;
            }
        }
        public List<FacilityLoginResponseModel> GetFacilityDetails()
        {
            List<FacilityLoginResponseModel> result = CacheManager.Instance.GetCache<List<FacilityLoginResponseModel>>(GlobalConstants.FACILITY_LICENSES);
            if (result == null || result.Count == 0)
            {
                result = new List<FacilityLoginResponseModel>();
                OracleConnection con = null;
                using (con = OpenConnection())
                {
                    var OraCmd = new OracleCommand();
                    OraCmd.Connection = con;
                    OraCmd.CommandText = $"{PackageName}.GET_FACILITY_CREDENATIAL";
                    OraCmd.CommandType = CommandType.StoredProcedure;
                    OraCmd.Parameters.Add(new OracleParameter("p_refcur", OracleDbType.RefCursor, ParameterDirection.Output));
                    if (con.State == ConnectionState.Closed)
                    {
                        con.Open();
                    }
                    OraCmd.ExecuteNonQuery();
                    var reader = ((OracleRefCursor)OraCmd.Parameters["p_refcur"].Value).GetDataReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            var obj = new FacilityLoginResponseModel();
                            obj.F_LIC = reader.ToInt32("F_LIC");
                            obj.F_USER = reader.ToString("F_USER");
                            obj.F_PWD = reader.ToString("F_PWD");
                            result.Add(obj);
                        }
                        CacheManager.Instance.SetCache(GlobalConstants.FACILITY_LICENSES, result, LicenseExpiration);
                    }
                }
            }
            return result;
        }
        public List<OrderingPhyResponseModel> GetErxPatEncounters(int eRxNo)
        {
            List<OrderingPhyResponseModel> result = CacheManager.Instance.GetCache<List<OrderingPhyResponseModel>>(GlobalConstants.PAT_ERX_ENCOUNTERS);
            if (result == null || result.Count == 0)
            {
                OracleConnection con = null;
                using (con = OpenConnection())
                {
                    var OraCmd = new OracleCommand();
                    OraCmd.Connection = con;
                    OraCmd.CommandText = $"{PackageName}.GET_PAT_ERX_ENCOUNTER";
                    OraCmd.CommandType = CommandType.StoredProcedure;
                    OraCmd.Parameters.Add(new OracleParameter("p_refcur", OracleDbType.RefCursor, ParameterDirection.Output));
                    if (con.State == ConnectionState.Closed)
                    {
                        con.Open();
                    }
                    OraCmd.ExecuteNonQuery();
                    var reader = ((OracleRefCursor)OraCmd.Parameters["p_refcur"].Value).GetDataReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            var obj = new OrderingPhyResponseModel();
                            obj.ERX_NO = reader.ToInt32("ERX_NO");
                            obj.ERX_ID = reader.ToInt32("ERX_ID");
                            obj.PAYER_ID = reader.ToInt32("PAYER_ID");
                            obj.CLINICIAN_ID = reader.ToInt32("CLINICIAN_ID");
                            obj.FACILITY_ID = reader.ToInt32("FACILITY_ID");
                            obj.ERX_TYPE = reader.ToString("ERX_TYPE");
                            obj.ERX_COMMENT = reader.ToString("ERX_COMMENT");
                            result.Add(obj);
                        }
                    }
                }
            }
            return result;
        }
        /// <summary>
        /// GetPhysicianLogin
        /// </summary>
        /// <param name="facilityLic"></param>
        /// <param name="phyLic"></param>
        /// <returns></returns>
        public List<PhysicianLoginResponseModel> GetPhysicianCredentials()
        {
            List<PhysicianLoginResponseModel> result = CacheManager.Instance.GetCache<List<PhysicianLoginResponseModel>>(GlobalConstants.PHYSICIAN_CREDENTIALS);
            if (result == null || result.Count == 0)
            {
                result = new List<PhysicianLoginResponseModel>();
                OracleConnection con = null;
                using (con = OpenConnection())
                {
                    var OraCmd = new OracleCommand();
                    OraCmd.Connection = con;
                    OraCmd.CommandText = $"{PackageName}.GET_PHY_CREDENATIAL";
                    OraCmd.CommandType = CommandType.StoredProcedure;
                    OraCmd.Parameters.Add(new OracleParameter("p_refcur", OracleDbType.RefCursor, ParameterDirection.Output));
                    if (con.State == ConnectionState.Closed)
                    {
                        con.Open();
                    }
                    OraCmd.ExecuteNonQuery();
                    var reader = ((OracleRefCursor)OraCmd.Parameters["p_refcur"].Value).GetDataReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            var obj = new PhysicianLoginResponseModel();
                            obj.F_ID = reader.ToInt32("F_ID");
                            obj.C_LIC = reader.ToInt32("C_LIC");
                            obj.C_USER = reader.ToString("C_USER");
                            obj.C_PWD = reader.ToString("C_PWD");
                            obj.F_USER = reader.ToString("F_USER");
                            obj.F_PWD = reader.ToString("F_PWD");
                            result.Add(obj);
                        }
                    }
                    CacheManager.Instance.SetCache(GlobalConstants.PHYSICIAN_CREDENTIALS, result, LicenseExpiration);
                }
            }
                return result;
        }

        public List<LicenseDetailsModel> GetLicenseDetails()
        {
            List<LicenseDetailsModel> result = CacheManager.Instance.GetCache<List<LicenseDetailsModel>>(GlobalConstants.LICENSE_DETAILS);
            if (result == null || result.Count == 0)
            {
                result = new List<LicenseDetailsModel>();
                OracleConnection con = null;
                using (con = OpenConnection())
                {
                    var OraCmd = new OracleCommand();
                    OraCmd.Connection = con;
                    OraCmd.CommandText = $"{PackageName}.GET_LICENSE_DETAILS";
                    OraCmd.CommandType = CommandType.StoredProcedure;
                    OraCmd.Parameters.Add(new OracleParameter("p_refcur", OracleDbType.RefCursor, ParameterDirection.Output));
                    if (con.State == ConnectionState.Closed)
                    {
                        con.Open();
                    }
                    OraCmd.ExecuteNonQuery();
                    var reader = ((OracleRefCursor)OraCmd.Parameters["p_refcur"].Value).GetDataReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            var obj = new LicenseDetailsModel();
                            obj.ERX_NO = reader.ToInt32("ERX_NO");
                            obj.SENDER_ID = reader.ToInt32("SENDER_ID");
                            obj.STATUS = reader.ToInt32("STATUS");
                            obj.UPD_FLAG = reader.ToInt32("UPD_FLAG");
                            obj.MRN = reader.ToString("MRN");
                            obj.ORDER_NO = reader.ToString("ORDER_NO");
                            obj.FACILITY_LIC_ID = reader.ToInt32("FACILITY_LIC_ID");
                            obj.FACILITY_LIC_USER = reader.ToString("FACILITY_LIC_USER");
                            obj.FACILITY_LIC_PWD = reader.ToString("FACILITY_LIC_PWD");
                            obj.FACILITY_LIC_STATUS = reader.ToInt32("FACILITY_LIC_STATUS");
                            obj.CLINICIAN_USER = reader.ToString("CLINICIAN_USER");
                            obj.CLINICIAN_PWD = reader.ToString("CLINICIAN_PWD");
                            obj.CLINICIAN_STATUS = reader.ToInt32("CLINICIAN_STATUS");
                            result.Add(obj);
                        }
                    }
                    CacheManager.Instance.SetCache(GlobalConstants.LICENSE_DETAILS, result, LicenseExpiration);
                }
            }
            return result;
        }


        public List<RxHeaderResponseModel> GetPatRxHeader(int eRxNo)
        {
            var output = new List<RxHeaderResponseModel>();
            OracleConnection con = null;
            using (con = OpenConnection())
            {
                var OraCmd = new OracleCommand();
                OraCmd.Connection = con;
                OraCmd.CommandText = $"{PackageName}.GETPATRXHEADER";
                OraCmd.CommandType = CommandType.StoredProcedure;
                OraCmd.Parameters.Add("P_PRXNO", OracleDbType.Int32, eRxNo, ParameterDirection.Input);
                OraCmd.Parameters.Add(new OracleParameter("p_refcur", OracleDbType.RefCursor, ParameterDirection.Output));
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                OraCmd.ExecuteNonQuery();
                var reader = ((OracleRefCursor)OraCmd.Parameters["p_refcur"].Value).GetDataReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var obj = new RxHeaderResponseModel();
                        obj.SENDER_ID = reader.ToInt32("SENDER_ID");
                        obj.RECEIVER_ID = reader.ToInt32("RECEIVER_ID");
                        obj.TRANSACTION_DATE = reader.ToString("TRANSACTION_DATE");
                        obj.RECORD_COUNT = reader.ToInt32("RECORD_COUNT");
                        obj.DISPOSITION_FLAG = reader.ToInt32("DISPOSITION_FLAG");
                        obj.ORDER_NO = reader.ToInt32("ORDER_NO");
                        obj.MRN = reader.ToInt32("MRN");
                        obj.ORDER_TYPE = reader.ToString("ORDER_TYPE");
                        output.Add(obj);
                    }
                }
            }
            return output;
        }
        public DataTable GetPatRxHeaderTable(int eRxNo)
        {
            var output = new List<RxHeaderResponseModel>();
            OracleConnection con = null;
            using (con = OpenConnection())
            {
                var OraCmd = new OracleCommand();
                OraCmd.Connection = con;
                OraCmd.CommandText = $"{PackageName}.GETPATRXHEADER";
                OraCmd.CommandType = CommandType.StoredProcedure;
                OraCmd.Parameters.Add("P_PRXNO", OracleDbType.Int32, eRxNo, ParameterDirection.Input);
                OraCmd.Parameters.Add(new OracleParameter("p_refcur", OracleDbType.RefCursor, ParameterDirection.Output));
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                OraCmd.ExecuteNonQuery();
                var reader = ((OracleRefCursor)OraCmd.Parameters["p_refcur"].Value).GetDataReader();
                var dataTable = new DataTable();
                dataTable.Load(reader);
                dataTable.TableName = "Header";
                return dataTable;
            }            
        }
       
        public DataTable GetFacilityDetailsTable(string facilityLic)
        {
            DataTable dataTable = null;
            OracleConnection con = null;
            using (con = OpenConnection())
            {
                var OraCmd = new OracleCommand();
                OraCmd.Connection = con;
                OraCmd.CommandText = $"{PackageName}.GET_FACILITY_CREDENATIAL";
                OraCmd.CommandType = CommandType.StoredProcedure;
                OraCmd.Parameters.Add("P_F_LIC", OracleDbType.NVarchar2, facilityLic, ParameterDirection.Input);
                OraCmd.Parameters.Add(new OracleParameter("p_refcur", OracleDbType.RefCursor, ParameterDirection.Output));
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                OraCmd.ExecuteNonQuery();
                var reader = ((OracleRefCursor)OraCmd.Parameters["p_refcur"].Value).GetDataReader();
                dataTable = new DataTable();
                dataTable.Load(reader);
                dataTable.TableName = "FacilityDetails";
            }
            return dataTable;
        }
        public List<PatPrescriptionHeaderResponseModel> GetPatPrescriptionHeader(int eRxNo)
        {
            var output = new List<PatPrescriptionHeaderResponseModel>();
            OracleConnection con = null;
            using (con = OpenConnection())
            {
                var OraCmd = new OracleCommand();
                OraCmd.Connection = con;
                OraCmd.CommandText = $"{PackageName}.GETPATPRESCRIPTIONHEAD";
                OraCmd.CommandType = CommandType.StoredProcedure;
                OraCmd.Parameters.Add("P_ERXNO", OracleDbType.Int32, eRxNo, ParameterDirection.Input);
                OraCmd.Parameters.Add(new OracleParameter("p_refcur", OracleDbType.RefCursor, ParameterDirection.Output));
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                OraCmd.ExecuteNonQuery();
                var reader = ((OracleRefCursor)OraCmd.Parameters["p_refcur"].Value).GetDataReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var obj = new PatPrescriptionHeaderResponseModel();
                        obj.ERX_ID = reader.ToInt32("ERX_ID");
                        obj.ERX_TYPE = reader.ToString("ERX_TYPE");
                        obj.PAYER_ID = reader.ToInt32("PAYER_ID");
                        obj.CLINICIAN_ID = reader.ToInt32("CLINICIAN_ID");
                        output.Add(obj);
                    }
                }
            }
            return output;
        }
        public DataTable GetPatPrescriptionHeaderTable(int eRxNo)
        {            
            OracleConnection con = null;
            using (con = OpenConnection())
            {
                var OraCmd = new OracleCommand();
                OraCmd.Connection = con;
                OraCmd.CommandText = $"{PackageName}.GETPATPRESCRIPTIONHEAD";
                OraCmd.CommandType = CommandType.StoredProcedure;
                OraCmd.Parameters.Add("P_ERXNO", OracleDbType.Int32, eRxNo, ParameterDirection.Input);
                OraCmd.Parameters.Add(new OracleParameter("p_refcur", OracleDbType.RefCursor, ParameterDirection.Output));
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                OraCmd.ExecuteNonQuery();
                var reader = ((OracleRefCursor)OraCmd.Parameters["p_refcur"].Value).GetDataReader();
                var dataTable = new DataTable();
                dataTable.Load(reader);
                dataTable.TableName = "Prescription";
                return dataTable;
            }            
        }
        public List<PatRxPatientTagResponseModel> GetPatRxPatientTag(int eRxNo)
        {
            var output = new List<PatRxPatientTagResponseModel>();
            OracleConnection con = null;
            using (con = OpenConnection())
            {
                var OraCmd = new OracleCommand();
                OraCmd.Connection = con;
                OraCmd.CommandText = $"{PackageName}.GETPATRXPATIENTTAG";
                OraCmd.CommandType = CommandType.StoredProcedure;
                OraCmd.Parameters.Add("P_ERXNO", OracleDbType.Int32, eRxNo, ParameterDirection.Input);
                OraCmd.Parameters.Add(new OracleParameter("p_refcur", OracleDbType.RefCursor, ParameterDirection.Output));
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                OraCmd.ExecuteNonQuery();
                var reader = ((OracleRefCursor)OraCmd.Parameters["p_refcur"].Value).GetDataReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var obj = new PatRxPatientTagResponseModel();
                        obj.MEMBER_ID = reader.ToInt32("MEMBER_ID");
                        obj.NATIONAL_ID = reader.ToInt32("NATIONAL_ID");
                        obj.DOB = reader.ToString("DOB");
                        obj.WEIGHT = reader.ToInt32("WEIGHT");
                        obj.EMAIL = reader.ToString("EMAIL");
                        output.Add(obj);
                    }
                }
            }
            return output;
        }
        public DataTable GetPatRxPatientTagTable(int eRxNo)
        {           
            OracleConnection con = null;
            using (con = OpenConnection())
            {
                var OraCmd = new OracleCommand();
                OraCmd.Connection = con;
                OraCmd.CommandText = $"{PackageName}.GETPATRXPATIENTTAG";
                OraCmd.CommandType = CommandType.StoredProcedure;
                OraCmd.Parameters.Add("P_ERXNO", OracleDbType.Int32, eRxNo, ParameterDirection.Input);
                OraCmd.Parameters.Add(new OracleParameter("p_refcur", OracleDbType.RefCursor, ParameterDirection.Output));
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                OraCmd.ExecuteNonQuery();
                var reader = ((OracleRefCursor)OraCmd.Parameters["p_refcur"].Value).GetDataReader();
                var dataTable = new DataTable();
                dataTable.Load(reader);
                dataTable.TableName = "Patient";
                return dataTable;
            }            
        }
        public List<PatRefNumResponseModel> GetPatWithRefNo(int eRxNo)
        {
            var output = new List<PatRefNumResponseModel>();
            OracleConnection con = null;
            using (con = OpenConnection())
            {
                var OraCmd = new OracleCommand();
                OraCmd.Connection = con;
                OraCmd.CommandText = $"{PackageName}.GETPATWITHREFNO";
                OraCmd.CommandType = CommandType.StoredProcedure;
                OraCmd.Parameters.Add("P_ERXNO", OracleDbType.Int32, eRxNo, ParameterDirection.Input);
                OraCmd.Parameters.Add(new OracleParameter("p_refcur", OracleDbType.RefCursor, ParameterDirection.Output));
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                OraCmd.ExecuteNonQuery();
                var reader = ((OracleRefCursor)OraCmd.Parameters["p_refcur"].Value).GetDataReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var obj = new PatRefNumResponseModel();
                        obj.ERX_NO = reader.ToString("ERX_NO");
                        obj.RES_CODE = reader.ToString("RES_CODE");
                        obj.MEMBER_ID = reader.ToInt32("MEMBER_ID");                        
                        output.Add(obj);
                    }
                }
            }
            return output;
        }
        public List<PatRxEncounterResponseModel> GetPatRxEncounter(int eRxNo)
        {
            var output = new List<PatRxEncounterResponseModel>();
            OracleConnection con = null;
            using (con = OpenConnection())
            {
                var OraCmd = new OracleCommand();
                OraCmd.Connection = con;
                OraCmd.CommandText = $"{PackageName}.GETPATRXENCOUNTER";
                OraCmd.CommandType = CommandType.StoredProcedure;
                OraCmd.Parameters.Add("P_ERXNO", OracleDbType.Int32, eRxNo, ParameterDirection.Input);
                OraCmd.Parameters.Add(new OracleParameter("p_refcur", OracleDbType.RefCursor, ParameterDirection.Output));
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                OraCmd.ExecuteNonQuery();
                var reader = ((OracleRefCursor)OraCmd.Parameters["p_refcur"].Value).GetDataReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var obj = new PatRxEncounterResponseModel();
                        obj.FACILITY_ID = reader.ToInt32("FACILITY_ID");
                        obj.ENCOUNTER_TYPE = reader.ToString("ENCOUNTER_TYPE");                       
                        output.Add(obj);
                    }
                }
            }
            return output;
        }
        public DataTable GetPatRxEncounterTable(int eRxNo)
        {
            var output = new List<PatRxEncounterResponseModel>();
            OracleConnection con = null;
            using (con = OpenConnection())
            {
                var OraCmd = new OracleCommand();
                OraCmd.Connection = con;
                OraCmd.CommandText = $"{PackageName}.GETPATRXENCOUNTER";
                OraCmd.CommandType = CommandType.StoredProcedure;
                OraCmd.Parameters.Add("P_ERXNO", OracleDbType.Int32, eRxNo, ParameterDirection.Input);
                OraCmd.Parameters.Add(new OracleParameter("p_refcur", OracleDbType.RefCursor, ParameterDirection.Output));
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                OraCmd.ExecuteNonQuery();
                var reader = ((OracleRefCursor)OraCmd.Parameters["p_refcur"].Value).GetDataReader();
                var dataTable = new DataTable();
                dataTable.Load(reader);
                dataTable.TableName = "Encounter";
                return dataTable;
            }           
        }
        public List<PatRxDiagnosisResponseModel> GetPatRxDiagnosis(int eRxNo)
        {
            var output = new List<PatRxDiagnosisResponseModel>();
            OracleConnection con = null;
            using (con = OpenConnection())
            {
                var OraCmd = new OracleCommand();
                OraCmd.Connection = con;
                OraCmd.CommandText = $"{PackageName}.GETPATRXDIAGNOSIS";
                OraCmd.CommandType = CommandType.StoredProcedure;
                OraCmd.Parameters.Add("P_ERXNO", OracleDbType.Int32, eRxNo, ParameterDirection.Input);
                OraCmd.Parameters.Add(new OracleParameter("p_refcur", OracleDbType.RefCursor, ParameterDirection.Output));
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                OraCmd.ExecuteNonQuery();
                var reader = ((OracleRefCursor)OraCmd.Parameters["p_refcur"].Value).GetDataReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var obj = new PatRxDiagnosisResponseModel();
                        obj.TYPE = reader.ToString("TYPE");
                        obj.DIAG_TYPE = reader.ToString("DIAG_TYPE");
                        obj.DIAG_CODE = reader.ToString("DIAG_CODE");
                        output.Add(obj);
                    }
                }
            }
            return output;
        }
        public DataTable GetPatRxDiagnosisTable(int eRxNo)
        {
            var output = new List<PatRxDiagnosisResponseModel>();
            OracleConnection con = null;
            using (con = OpenConnection())
            {
                var OraCmd = new OracleCommand();
                OraCmd.Connection = con;
                OraCmd.CommandText = $"{PackageName}.GETPATRXDIAGNOSIS";
                OraCmd.CommandType = CommandType.StoredProcedure;
                OraCmd.Parameters.Add("P_ERXNO", OracleDbType.Int32, eRxNo, ParameterDirection.Input);
                OraCmd.Parameters.Add(new OracleParameter("p_refcur", OracleDbType.RefCursor, ParameterDirection.Output));
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                OraCmd.ExecuteNonQuery();
                var reader = ((OracleRefCursor)OraCmd.Parameters["p_refcur"].Value).GetDataReader();
                var dataTable = new DataTable();
                dataTable.Load(reader);
                dataTable.TableName = "Diagnosis";
                return dataTable;
            }           
        }
        public List<PatRxActivityResponseModel> GetPatRxActivity(int eRxNo)
        {
            var output = new List<PatRxActivityResponseModel>();
            OracleConnection con = null;
            using (con = OpenConnection())
            {
                var OraCmd = new OracleCommand();
                OraCmd.Connection = con;
                OraCmd.CommandText = $"{PackageName}.GETPATRXACTIVITY";
                OraCmd.CommandType = CommandType.StoredProcedure;
                OraCmd.Parameters.Add("P_ERXNO", OracleDbType.Int32, eRxNo, ParameterDirection.Input);
                OraCmd.Parameters.Add(new OracleParameter("p_refcur", OracleDbType.RefCursor, ParameterDirection.Output));
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                OraCmd.ExecuteNonQuery();
                var reader = ((OracleRefCursor)OraCmd.Parameters["p_refcur"].Value).GetDataReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var obj = new PatRxActivityResponseModel();
                        obj.ACT_ID = reader.ToInt32("ACT_ID");
                        obj.ACT_START = reader.ToString("ACT_START");
                        obj.ACT_TYPE = reader.ToString("ACT_TYPE");
                        obj.ACT_CODE = reader.ToString("ACT_CODE");
                        obj.QTY = reader.ToInt32("QTY");
                        obj.DURATION = reader.ToString("DURATION");
                        obj.REFILLS = reader.ToString("REFILLS");
                        obj.ROUTE_OF_ADMIN = reader.ToString("ROUTE_OF_ADMIN");
                        obj.INSTRUCTIONS = reader.ToString("INSTRUCTIONS");
                        obj.FREQ_UNIT = reader.ToString("FREQ_UNIT");
                        obj.FREQ_VALUE = reader.ToString("FREQ_VALUE");
                        obj.FREQ_TYPE = reader.ToString("FREQ_TYPE");
                        output.Add(obj);
                    }
                }
            }
            return output;
        }
        public DataTable GetPatRxActivityTable(int eRxNo)
        {
            var output = new List<PatRxActivityResponseModel>();
            OracleConnection con = null;
            using (con = OpenConnection())
            {
                var OraCmd = new OracleCommand();
                OraCmd.Connection = con;
                OraCmd.CommandText = $"{PackageName}.GETPATRXACTIVITY";
                OraCmd.CommandType = CommandType.StoredProcedure;
                OraCmd.Parameters.Add("P_ERXNO", OracleDbType.Int32, eRxNo, ParameterDirection.Input);
                OraCmd.Parameters.Add(new OracleParameter("p_refcur", OracleDbType.RefCursor, ParameterDirection.Output));
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                OraCmd.ExecuteNonQuery();
                var reader = ((OracleRefCursor)OraCmd.Parameters["p_refcur"].Value).GetDataReader();
                var dataTable = new DataTable();
                dataTable.Load(reader);

                dataTable.TableName = "Activity";
                return dataTable;
            }            
        }
        public List<PatRxObservationResponseModel> GetPatRxObservation(int eRxNo, int actId)
        {
            var output = new List<PatRxObservationResponseModel>();
            OracleConnection con = null;
            using (con = OpenConnection())
            {
                var OraCmd = new OracleCommand();
                OraCmd.Connection = con;
                OraCmd.CommandText = $"{PackageName}.GETPATRXOBSERVATION";
                OraCmd.CommandType = CommandType.StoredProcedure;
                OraCmd.Parameters.Add("P_ERX_NO", OracleDbType.Int32, eRxNo, ParameterDirection.Input);
                OraCmd.Parameters.Add("P_ACT_ID", OracleDbType.Int32, actId, ParameterDirection.Input);
                OraCmd.Parameters.Add(new OracleParameter("p_refcur", OracleDbType.RefCursor, ParameterDirection.Output));
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                OraCmd.ExecuteNonQuery();
                var reader = ((OracleRefCursor)OraCmd.Parameters["p_refcur"].Value).GetDataReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var obj = new PatRxObservationResponseModel();
                        obj.TYPE = reader.ToString("TYPE");
                        obj.ACT_ID = reader.ToString("ACT_ID");
                        obj.OBS_ID = reader.ToString("OBS_ID");
                        obj.TYPE = reader.ToString("TYPE");
                        obj.CODE = reader.ToString("CODE");
                        obj.VALUE = reader.ToString("VALUE");
                        obj.VALUE_TYPE = reader.ToString("VALUE_TYPE");
                        output.Add(obj);
                    }
                }
            }
            return output;
        }
        public DataTable GetPatRxObservationTable(int eRxNo, int actId)
        {
            var output = new List<PatRxObservationResponseModel>();
            OracleConnection con = null;
            using (con = OpenConnection())
            {
                var OraCmd = new OracleCommand();
                OraCmd.Connection = con;
                OraCmd.CommandText = $"{PackageName}.GETPATRXOBSERVATION";
                OraCmd.CommandType = CommandType.StoredProcedure;
                OraCmd.Parameters.Add("P_ERX_NO", OracleDbType.Int32, eRxNo, ParameterDirection.Input);
                OraCmd.Parameters.Add("P_ACT_ID", OracleDbType.Int32, actId, ParameterDirection.Input);
                OraCmd.Parameters.Add(new OracleParameter("p_refcur", OracleDbType.RefCursor, ParameterDirection.Output));
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                OraCmd.ExecuteNonQuery();
                var reader = ((OracleRefCursor)OraCmd.Parameters["p_refcur"].Value).GetDataReader();
                var dataTable = new DataTable();
                dataTable.Load(reader);
                dataTable.TableName = "OBSERVATION";
                return dataTable;
            }           
        }
        public List<PatPendingRxResponseModel> GetPatPendingRx()
        {
            var output = new List<PatPendingRxResponseModel>();
            OracleConnection con = null;
            using (con = OpenConnection())
            {
                var OraCmd = new OracleCommand();
                OraCmd.Connection = con;
                OraCmd.CommandText = $"{PackageName}.GETPATRXACTIVITY";
                OraCmd.CommandType = CommandType.StoredProcedure;                
                OraCmd.Parameters.Add(new OracleParameter("p_refcur", OracleDbType.RefCursor, ParameterDirection.Output));
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                OraCmd.ExecuteNonQuery();
                var reader = ((OracleRefCursor)OraCmd.Parameters["p_refcur"].Value).GetDataReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var obj = new PatPendingRxResponseModel();
                        obj.ERX_NO = reader.ToString("ERX_NO");
                        obj.SENDER_ID = reader.ToString("SENDER_ID");                        
                        output.Add(obj);
                    }
                }
            }
            return output;
        }
        public List<XmlTypeClobResponseModel> GetXmlTypeClob()
        {
            var output = new List<XmlTypeClobResponseModel>();
            OracleConnection con = null;
            using (con = OpenConnection())
            {
                var OraCmd = new OracleCommand();
                OraCmd.Connection = con;
                OraCmd.CommandText = $"{PackageName}.GETPATRXACTIVITY";
                OraCmd.CommandType = CommandType.StoredProcedure;
                OraCmd.Parameters.Add(new OracleParameter("p_refcur", OracleDbType.RefCursor, ParameterDirection.Output));
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                OraCmd.ExecuteNonQuery();
                var reader = ((OracleRefCursor)OraCmd.Parameters["p_refcur"].Value).GetDataReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var obj = new XmlTypeClobResponseModel();                        
                        output.Add(obj);
                    }
                }
            }
            return output;
        }

        /// <summary>
        /// GetOrderingPhy
        /// </summary>
        /// <param name="eRxNo"></param>
        /// <returns></returns>
       
        public string GetOrderingPhyStr(int eRxNo)
        {
            DataTable dataTable = null;
            string clinician = null;
            OracleConnection con = null;
            using (con = OpenConnection())
            {
                string strQry = $"select DISTINCT ERX_NO,CLINICIAN_ID from  pat_erx_encounter where  ERX_NO ='{eRxNo}'";
                OracleDataAdapter Da = new OracleDataAdapter(strQry, con);
                dataTable = new DataTable();
                Da.Fill(dataTable);
                if (dataTable != null && dataTable.Rows.Count > 0)
                {
                    clinician = dataTable.Rows[0]["CLINICIAN_ID"].ToString();
                }
            }
            return clinician;
        }
        public bool UpdateStatusCode(int eRxNo, int responseCode)
        {
            OracleConnection con = null;
            using (con = OpenConnection())
            {
                var OraCmd = new OracleCommand();
                OraCmd.Connection = con;
                OraCmd.CommandText = $"Update PAT_ERX_header SET STATUS=:P_STATUS_CODE,LAST_REQ_STATUS_TIME=SYSDATE where ERX_NO=:P_ERX_NO";
                OraCmd.CommandType = CommandType.Text;
                OraCmd.Parameters.Add(new OracleParameter("P_ERX_NO", OracleDbType.Int32, eRxNo, ParameterDirection.Input));
                OraCmd.Parameters.Add(new OracleParameter("P_STATUS_CODE", OracleDbType.Int32, responseCode, ParameterDirection.Input));
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                OraCmd.ExecuteNonQuery();
                return true;
            }
        }
        public bool UpdateResponseCode(int eRxNo, int responseCode, int dhaResponse)
        {
            OracleConnection con = null;
            using (con = OpenConnection())
            {
                var OraCmd = new OracleCommand();
                OraCmd.Connection = con;               
                OraCmd.CommandType = CommandType.Text;
                OraCmd.CommandText = "Update PAT_ERX_header SET STATUS=:P_STATUS_CODE,RES_CODE=:P_DHA_RESPONSE_CODE,LAST_RES_STATUS_TIME=SYSDATE WHERE ERX_NO= :P_ERX_NO ";
                OraCmd.Parameters.Add(new OracleParameter("P_STATUS_CODE", responseCode));
                OraCmd.Parameters.Add(new OracleParameter("P_DHA_RESPONSE_CODE", dhaResponse));
                OraCmd.Parameters.Add(new OracleParameter("P_ERX_NO", eRxNo));
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                OraCmd.ExecuteNonQuery();
                return true;
            }
        }
        public bool InsertErXHistory(int eRxNo, string prescId, int responseCode)
        {
            bool updresponse = false;
            try
            {
                int TrN = 0;
                OracleCommand cmdTrn = new OracleCommand();
                cmdTrn.CommandType = CommandType.Text;
                cmdTrn.CommandText = "SELECT  NVL(MAX(TRN),0)+1 AS NXTTRN FROM  PAT_ERX_HISTORY";
                using (OracleConnection con = OpenConnection())
                {
                    cmdTrn.Connection = con;
                    var NxtTrn = cmdTrn.ExecuteScalar();
                    if (NxtTrn != null) TrN = Convert.ToInt32(NxtTrn);


                    string strInsQry = "INSERT INTO PAT_ERX_HISTORY (TRN, ERX_NO, PRESC_ID, RESPONSE_CODE, ACT_DATETIME) ";
                    strInsQry += " VALUES(:TRN, :ERX_NO, :PRESC_ID, :RESPONSE_CODE, TO_DATE(:ACT_DATETIME, 'DD/MM/YYYY HH24:MI:SS'))";
                    OracleCommand cmd = new OracleCommand();
                    cmd.CommandType = CommandType.Text;

                    cmd.CommandText = strInsQry;
                    cmd.Parameters.Add(new OracleParameter("TRN", TrN));
                    cmd.Parameters.Add(new OracleParameter("ERXNO", eRxNo));
                    cmd.Parameters.Add(new OracleParameter("PRESC_ID", prescId));
                    cmd.Parameters.Add(new OracleParameter("RESPONSE_CODE", responseCode));
                    cmd.Parameters.Add(new OracleParameter("ACT_DATETIME", System.DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss", System.Globalization.CultureInfo.InvariantCulture)));
                    cmd.Connection = con;
                    cmd.ExecuteNonQuery();
                }
            }
            catch (OracleException ex)
            {
            }
            return updresponse;
        }
        public bool UpdateAuthorizationCode(string[] pstrAuthorization, DataTable dtActivity)
        {
            bool UpdateAuth = false;

            using (OracleConnection con = OpenConnection())
            {
                OracleTransaction oratrns = con.BeginTransaction();
                try
                {
                    OracleCommand cmdupd = new OracleCommand();
                    cmdupd.Transaction = oratrns;
                    cmdupd.CommandType = CommandType.Text;

                    //Update encounter authorisation status based on eRx response

                    cmdupd.CommandText = "UPDATE PAT_ERX_ENCOUNTER SET RESULT=:pRESULT,ID_PAYER=:pID_PAYER,DENIAL_CODE=:pDENIAL_CODE, ";
                    cmdupd.CommandText += "ERX_COMMENT =:pERX_COMMENT,UPD_DATETIME= SYSDATE WHERE ERX_ID=:pERX_ID";

                    //cmdupd.CommandText = "UPDATE PAT_ERX_ENCOUNTER SET RESULT='"+ pstrAuthorization[1] + "',ID_PAYER='"+ pstrAuthorization[2] + "',DENIAL_CODE='"+ pstrAuthorization[3] + "', ";
                    //cmdupd.CommandText += "ERX_COMMENT ='"+ pstrAuthorization[4] + "',UPD_DATETIME= SYSDATE WHERE ERX_ID='"+ pstrAuthorization[0] + "'";

                    cmdupd.Parameters.Add(new OracleParameter("pRESULT", pstrAuthorization[1]));
                    cmdupd.Parameters.Add(new OracleParameter("pID_PAYER", pstrAuthorization[2]));
                    cmdupd.Parameters.Add(new OracleParameter("pDENIAL_CODE", pstrAuthorization[3]));
                    cmdupd.Parameters.Add(new OracleParameter("pERX_COMMENT", pstrAuthorization[4]));
                    cmdupd.Parameters.Add(new OracleParameter("pERX_ID", pstrAuthorization[0]));

                    cmdupd.Connection = con;
                    int i = cmdupd.ExecuteNonQuery();
                    int x = 0;
                    int y = 0;
                    if (i != 0 && dtActivity != null && dtActivity.Rows.Count > 0)
                    {
                        for (int m = 0; m < dtActivity.Rows.Count; m++)
                        {
                            string[] pstrActivity = new string[9];
                            pstrActivity[0] = (dtActivity.Columns.Contains("ID") ? dtActivity.Rows[m]["ID"].ToString() : null);
                            pstrActivity[1] = (dtActivity.Columns.Contains("Type") ? dtActivity.Rows[m]["Type"].ToString() : null);
                            pstrActivity[2] = (dtActivity.Columns.Contains("Code") ? dtActivity.Rows[m]["Code"].ToString() : null);
                            pstrActivity[3] = (dtActivity.Columns.Contains("Quantity") ? dtActivity.Rows[m]["Quantity"].ToString() : null);
                            pstrActivity[4] = (dtActivity.Columns.Contains("Net") ? dtActivity.Rows[m]["Net"].ToString() : null);
                            pstrActivity[5] = (dtActivity.Columns.Contains("List") ? dtActivity.Rows[m]["List"].ToString() : null);
                            pstrActivity[6] = (dtActivity.Columns.Contains("PatientShare") ? dtActivity.Rows[m]["PatientShare"].ToString() : null);
                            pstrActivity[7] = (dtActivity.Columns.Contains("PaymentAmount") ? dtActivity.Rows[m]["PaymentAmount"].ToString() : null);
                            pstrActivity[8] = (dtActivity.Columns.Contains("DenialCode") ? dtActivity.Rows[m]["DenialCode"].ToString() : null);

                            //If Same activity exist Delete the activity before Insert

                            OracleCommand cmddel = new OracleCommand();
                            cmddel.Transaction = oratrns;
                            cmddel.CommandType = CommandType.Text;
                            cmddel.CommandText = "Delete from R_ERX_ACTIVITY Where PRESC_ID = '" + pstrAuthorization[0] + "' and ACT_CODE = '" + pstrActivity[2] + "'";
                            cmddel.Connection = con;
                            int c = cmddel.ExecuteNonQuery();

                            //Insert Into R_ERX_ACTIVITY for Actvity Download track

                            OracleCommand cmdins = new OracleCommand();
                            cmdins.Transaction = oratrns;
                            cmdins.CommandType = CommandType.Text;
                            string strInsQry = "INSERT INTO R_ERX_ACTIVITY (PRESC_ID, ACT_ID, ACT_TYPE, ACT_CODE, QTY, NET, LIST, PAT_SHARE, PAY_AMT, DENIAL_CODE, UPD_DATETIME) ";
                            strInsQry += " VALUES(:PRESC_ID, :ACT_ID, :ACT_TYPE, :ACT_CODE, :QTY,:NET, :LIST, :PAT_SHARE, :PAY_AMT, :DENIAL_CODE, SYSDATE) ";

                            cmdins.CommandText = strInsQry;
                            cmdins.Parameters.Add(new OracleParameter("PRESC_ID", pstrAuthorization[0]));
                            cmdins.Parameters.Add(new OracleParameter("ACT_ID", pstrActivity[0]));
                            cmdins.Parameters.Add(new OracleParameter("ACT_TYPE", pstrActivity[1]));
                            cmdins.Parameters.Add(new OracleParameter("ACT_CODE", pstrActivity[2]));
                            cmdins.Parameters.Add(new OracleParameter("QTY", pstrActivity[3]));
                            cmdins.Parameters.Add(new OracleParameter("NET", pstrActivity[4]));
                            cmdins.Parameters.Add(new OracleParameter("LIST", pstrActivity[5]));
                            cmdins.Parameters.Add(new OracleParameter("PAT_SHARE", pstrActivity[6]));
                            cmdins.Parameters.Add(new OracleParameter("PAY_AMT", pstrActivity[7]));
                            cmdins.Parameters.Add(new OracleParameter("DENIAL_CODE", pstrActivity[8]));

                            cmdins.Connection = con;
                            x += cmdins.ExecuteNonQuery();


                            OracleCommand cmdupdAct = new OracleCommand();
                            cmdupdAct.Transaction = oratrns;
                            cmdupdAct.CommandType = CommandType.Text;

                            //Update Enacounter activity Table as The response of eRx

                            string strupdQryact = "UPDATE PAT_ERX_ACTIVITY SET R_APPROVED_QTY=:R_APPROVED_QTY, ";
                            strupdQryact += " R_NET=:R_NET,R_LIST=:R_LIST, R_PAT_SHARE =:R_PAT_SHARE,R_PAT_AMT=:R_PAT_AMT,";
                            strupdQryact += " R_DEN_CODE =:R_DEN_CODE,UPD_DATETIME=SYSDATE ";
                            strupdQryact += " WHERE ERX_NO = (SELECT ERX_NO from PAT_ERX_ENCOUNTER where ERX_ID =:ERX_ID) ";
                            strupdQryact += " AND ACT_CODE =:ACT_CODE";

                            cmdupdAct.Parameters.Add(new OracleParameter("R_APPROVED_QTY", pstrActivity[3]));
                            cmdupdAct.Parameters.Add(new OracleParameter("R_NET", pstrActivity[4]));
                            cmdupdAct.Parameters.Add(new OracleParameter("R_LIST", pstrActivity[5]));
                            cmdupdAct.Parameters.Add(new OracleParameter("R_PAT_SHARE", pstrActivity[6]));
                            cmdupdAct.Parameters.Add(new OracleParameter("R_PAT_AMT", pstrActivity[7]));
                            cmdupdAct.Parameters.Add(new OracleParameter("R_DEN_CODE", pstrActivity[8]));
                            cmdupdAct.Parameters.Add(new OracleParameter("ERX_ID", pstrAuthorization[0]));
                            cmdupdAct.Parameters.Add(new OracleParameter("ACT_CODE", pstrActivity[2]));

                            cmdupdAct.CommandText = strupdQryact;
                            cmdupdAct.Connection = con;
                            y += cmdupdAct.ExecuteNonQuery();


                        }
                        if (x == dtActivity.Rows.Count)
                        {
                            oratrns.Commit();
                            UpdateAuth = true;

                            OracleCommand cmdupdheader = new OracleCommand();
                            cmdupdheader.CommandType = CommandType.Text;

                            cmdupdheader.CommandText = "Update PAT_ERX_header set  Status=2,LAST_RES_STATUS_TIME=SYSDATE where ERX_NO= (SELECT ERX_NO from PAT_ERX_ENCOUNTER where ERX_ID =:ERX_ID) ";
                            cmdupdheader.Parameters.Add(new OracleParameter("ERX_ID", pstrAuthorization[0]));
                            cmdupdheader.Connection = con;
                            int z = cmdupdheader.ExecuteNonQuery();
                            log.Info(string.Format("eRx Id {0} response updated Successfully.", pstrAuthorization[0]));
                        }
                        else oratrns.Rollback();
                    }
                }
                catch (OracleException ex)
                {
                    log.Error(ex);
                    oratrns.Rollback();
                }
            }
            return UpdateAuth;

        }
        public int InsertXMlError(int _eRXNo, string IN_FILE_NAME, string IN_TRN, string IN_TYPE,
            string IN_OBJ_NAME, string IN_HAAD_FLD, string IN_FLD_VAL, string IN_ADD_REF, string IN_ERROR_TXT)
        {
            int rows = -1;
            OracleConnection con = null;
            try
            {
                OracleCommand cmd = new OracleCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "T_ERX.INSERT_ERX_ERROR";
                cmd.Parameters.Add("IN_ERX_NO", OracleDbType.Int16, ParameterDirection.Input).Value = _eRXNo;
                cmd.Parameters.Add("IN_FILE_NAME", OracleDbType.Varchar2, ParameterDirection.Input).Value = IN_FILE_NAME;
                cmd.Parameters.Add("IN_TRN", OracleDbType.Varchar2, ParameterDirection.Input).Value = IN_TRN;
                cmd.Parameters.Add("IN_TYPE", OracleDbType.Varchar2, ParameterDirection.Input).Value = IN_TYPE;
                cmd.Parameters.Add("IN_OBJ_NAME", OracleDbType.Varchar2, ParameterDirection.Input).Value = IN_OBJ_NAME;
                cmd.Parameters.Add("IN_DHPO_FLD", OracleDbType.Varchar2, ParameterDirection.Input).Value = IN_HAAD_FLD;
                cmd.Parameters.Add("IN_FLD_VAL", OracleDbType.Varchar2, ParameterDirection.Input).Value = IN_FLD_VAL;
                cmd.Parameters.Add("IN_ADD_REF", OracleDbType.Varchar2, ParameterDirection.Input).Value = IN_ADD_REF;
                cmd.Parameters.Add("IN_ERROR_TXT", OracleDbType.Varchar2, ParameterDirection.Input).Value = IN_ERROR_TXT;

                using (con = OpenConnection())
                {
                    //OracleDataAdapter da = new OracleDataAdapter(cmd); 
                    //cmd.BindByName = true;
                    cmd.Connection = con;
                    rows = cmd.ExecuteNonQuery();
                }
            }            
            catch (Exception ex)
            {
                log.Error(ex);
            }
            finally
            {
                if(con.State == ConnectionState.Open)
                {
                    con.Close();
                }               
            }
            return rows;
        }
        public int InsertXMlData(int _RxNo, int _ErrorStatus, string strfilename, string strxmldata, string strmsg)
        {
            int rows = -1;
            OracleConnection con = null;
            try
            {
                OracleCommand cmd = new OracleCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "T_ERX.INSERT_ERX_XML_DATA";

                cmd.Parameters.Add("IN_ERX_NO", OracleDbType.Int16, ParameterDirection.Input).Value = _RxNo;
                cmd.Parameters.Add("IN_STAT", OracleDbType.Int16, ParameterDirection.Input).Value = _ErrorStatus;
                cmd.Parameters.Add("IN_FILE_NAME", OracleDbType.Varchar2, ParameterDirection.Input).Value = strfilename;
                cmd.Parameters.Add("IN_XML_DATA", OracleDbType.Varchar2, ParameterDirection.Input).Value = strxmldata;
                cmd.Parameters.Add("IN_MSG", OracleDbType.Varchar2, ParameterDirection.Input).Value = strmsg;

                using (con = OpenConnection())
                {
                    cmd.Connection = con;
                    rows = cmd.ExecuteNonQuery();
                }
            }           
            catch (Exception ex)
            {
                log.Error(ex);
            }
            finally
            {
                if(con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }

            return rows;
        }
        public Int32 GetRequestErxNo(string strErxid)
        {
            Int32 retval = -1;
            OracleConnection con = null;
            try
            {
                OracleCommand cmd = new OracleCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "T_ERX.GET_ERXNO_FROM_ERXID";

                cmd.Parameters.Add(new OracleParameter("IN_ERX_ID", strErxid));
                cmd.Parameters.Add("eRx_No", OracleDbType.Varchar2, 2000).Direction = ParameterDirection.ReturnValue;

                using (con = OpenConnection())
                {
                    cmd.Connection = con;
                    cmd.BindByName = true;
                    cmd.ExecuteNonQuery();
                    retval = Convert.ToInt32(cmd.Parameters["eRx_No"].Value.ToString());
                }
            }            
            catch (Exception ex) { log.Error(ex); }
            finally
            {
                if(con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
            return retval;
        }
        public DataTable GETPATPENINGERX()
        {
            DataTable dataTable = null;
            OracleConnection con = null;
            try
            {                
                var strQry = "select DISTINCT ERX_NO,SENDER_ID from  PAT_ERX_HEADER where  TRANSACTION_DATE >= SYSDATE - 5 and Status =0 and RECEIVER_ID <> 'CASH'";
                using (con = OpenConnection())
                {                   
                    OracleDataAdapter Da = new OracleDataAdapter(strQry,con);
                    dataTable = new DataTable();
                    Da.Fill(dataTable);
                    dataTable.TableName = "PendingReq";
                }
            }
            catch (Exception ex) { log.Error(ex); }
            finally
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
            return dataTable;
        }
        public ConfigurationSettingsModel GetConfigurationSettings(int status)
        {
            OracleConnection con = null;
            ConfigurationSettingsModel result = null;
            try
            {
                result = CacheManager.Instance.GetCache<ConfigurationSettingsModel>(GlobalConstants.CONFIG_SETTINGS);
                if (result == null)
                {
                    using (con = OpenConnection())
                    {
                        using (OracleCommand OraCmd = new OracleCommand())
                        {
                            OraCmd.Connection = con;
                            OraCmd.CommandType = CommandType.StoredProcedure;
                            OraCmd.CommandText = OraCmd.CommandText = $"{PackageName}.GET_CONFIGURATION_SETTINGS";
                            OraCmd.Parameters.Add("P_STATUS", status);
                            OraCmd.Parameters.Add(new OracleParameter("p_refcur", OracleDbType.RefCursor, ParameterDirection.Output));
                            if (con.State == ConnectionState.Closed)
                            {
                                con.Open();
                            }
                            OraCmd.ExecuteNonQuery();
                            var reader = ((OracleRefCursor)OraCmd.Parameters["p_refcur"].Value).GetDataReader();
                            if (reader.HasRows)
                            {
                                result = new ConfigurationSettingsModel();
                                while (reader.Read())
                                {
                                    result.ApiBaseUrl = reader.ToString("API_URL");
                                    result.ClientId = reader.ToString("CLIENTID");
                                    result.ClientSecret = reader.ToString("CLIENT_SECRET");
                                    result.AppID = reader.ToString("API_ID");
                                    result.ApiKey = reader.ToString("API_KEY");
                                }
                            }
                            CacheManager.Instance.SetCache(GlobalConstants.CONFIG_SETTINGS, result);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error($"Error GetConfigurationSettings {ex.Message}", ex);
            }
            return result;
        }
        public List<PendingRequestsModel> GetPendingRequests()
        {
            OracleConnection con = null;
            var result = new List<PendingRequestsModel>();
            try
            {
                using (con = OpenConnection())
                {
                    using (OracleCommand OraCmd = new OracleCommand())
                    {
                        OraCmd.Connection = con;
                        OraCmd.CommandType = CommandType.StoredProcedure;
                        OraCmd.CommandText = $"{PackageName}.GET_PENDING_REQUESTS";
                        OraCmd.Parameters.Add(new OracleParameter("p_refcur", OracleDbType.RefCursor, ParameterDirection.Output));
                        if (con.State == ConnectionState.Closed)
                        {
                            con.Open();
                        }
                        OraCmd.ExecuteNonQuery();
                        var reader = ((OracleRefCursor)OraCmd.Parameters["p_refcur"].Value).GetDataReader();
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                var obj = new PendingRequestsModel();
                                obj.ID = reader.ToInt32("ERX_NO");
                                obj.REQUEST_TYPE = reader.ToString("RECEIVER_ID");
                                obj.PendingRequest.ERX_NO = reader.ToInt32("ERX_NO");
                                obj.PendingRequest.SENDER_ID = reader.ToInt32("SENDER_ID");
                                obj.PendingRequest.RECEIVER_ID = reader.ToString("RECEIVER_ID");
                                obj.PendingRequest.TRANSACTION_DATE = reader.ToString("TRANSACTION_DATE");
                                obj.PendingRequest.RECORD_COUNT = reader.ToString("RECORD_COUNT");
                                obj.PendingRequest.DISPOSITION_FLAG = reader.ToString("DISPOSITION_FLAG");
                                obj.PendingRequest.STATUS = reader.ToString("STATUS");
                                obj.PendingRequest.LAST_REQ_STATUS_TIME = reader.ToString("LAST_REQ_STATUS_TIME");
                                obj.PendingRequest.LAST_RES_STATUS_TIME = reader.ToString("LAST_RES_STATUS_TIME");
                                obj.PendingRequest.REQUESTED_BY = reader.ToString("REQUESTED_BY");
                                obj.PendingRequest.ORDER_NO = reader.ToString("ORDER_NO");
                                obj.PendingRequest.MRN = reader.ToString("MRN");
                                obj.PendingRequest.ORDER_TYPE = reader.ToString("ORDER_TYPE");
                                obj.PendingRequest.RES_CODE = reader.ToString("RES_CODE");
                                obj.PendingRequest.UPD_FLAG = reader.ToString("UPD_FLAG");
                                obj.PAYLOAD = JsonConvert.SerializeObject(obj.PendingRequest);                               
                                result.Add(obj);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error($"Error GetPendingRequests {ex.Message}", ex);
            }
            return result;
        }
    }
}
