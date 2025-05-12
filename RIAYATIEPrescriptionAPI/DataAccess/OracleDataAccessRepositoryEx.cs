using DomainModel;
using DomainModel.Models.Common;
using DomainModel.Models.Response;
using log4net;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class OracleDataAccessRepositoryEx : OracleRepositoryBase
    {
        private string ApiBaseURL = ConfigurationManager.AppSettings["API_BASE_URL"];
        private readonly ILog log = LogManager.GetLogger(typeof(OracleDataAccessRepositoryEx));
        private static readonly Lazy<OracleDataAccessRepositoryEx> lazy = new Lazy<OracleDataAccessRepositoryEx>(() => new OracleDataAccessRepositoryEx());
        private OracleDataAccessRepositoryEx()
        {
        }
        public static OracleDataAccessRepositoryEx GetInstance
        {
            get
            {
                return lazy.Value;
            }
        }
        public List<FacilityLoginResponseModel> GetFacilityLicenseDetails()
        {
            List<FacilityLoginResponseModel> result = CacheManager.Instance.GetCache<List<FacilityLoginResponseModel>>(GlobalConstants.FACILITY_LICENSES);
            try
            {
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
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }
        //Get Patient ERX Header row Based on Unique eRxNo.
        public DataTable GETPATRXHEADER(int _eRxNo)
        {
            OracleCommand cmd = new OracleCommand();

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "T_ERX.GETPATRXHEADER";

            cmd.BindByName = true;
            cmd.Connection = OpenConnection();
            cmd.Parameters.Add("PRXNO", OracleDbType.Int16, ParameterDirection.Input).Value = _eRxNo;
            cmd.Parameters.Add(new OracleParameter("io_cursor", OracleDbType.RefCursor)).Direction = ParameterDirection.ReturnValue;
            //OracleDataAdapter da = new OracleDataAdapter(cmd);
            cmd.ExecuteNonQuery();

            Oracle.ManagedDataAccess.Types.OracleRefCursor t = (Oracle.ManagedDataAccess.Types.OracleRefCursor)cmd.Parameters["io_cursor"].Value;
            OracleDataReader rdr = t.GetDataReader();
            var dataTable = new DataTable();
            dataTable.Load(rdr);
            dataTable.TableName = "Header";

            return dataTable;
        }

        //Get Physician and Faclility Login and password using physician License no. and Faciliy Id
        public DataTable GetPhysicianLogin(string strFacilityLic, string strphyLic)
        {
            OracleCommand cmd = new OracleCommand();

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "T_ERX.GET_PHY_CREDENATIAL";

            cmd.BindByName = true;
            cmd.Connection = OpenConnection();
            cmd.Parameters.Add("P_F_LIC ", OracleDbType.Varchar2, ParameterDirection.Input).Value = strFacilityLic;
            cmd.Parameters.Add("P_C_LIC ", OracleDbType.Varchar2, ParameterDirection.Input).Value = strphyLic;
            cmd.Parameters.Add(new OracleParameter("io_cursor", OracleDbType.RefCursor)).Direction = ParameterDirection.ReturnValue;
            //OracleDataAdapter da = new OracleDataAdapter(cmd);
            cmd.ExecuteNonQuery();

            Oracle.ManagedDataAccess.Types.OracleRefCursor t = (Oracle.ManagedDataAccess.Types.OracleRefCursor)cmd.Parameters["io_cursor"].Value;
            OracleDataReader rdr = t.GetDataReader();
            var dataTable = new DataTable();
            dataTable.Load(rdr);
            dataTable.TableName = "FacilityPhyDetails";

            return dataTable;
        }

        //Get Faclility Login and password using Faciliy License
        public DataTable GetFacilityDetails(string strFacilityLic)
        {
            OracleCommand cmd = new OracleCommand();

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "T_ERX.GET_FACILITY_CREDENATIAL";

            cmd.BindByName = true;
            cmd.Connection = OpenConnection();
            cmd.Parameters.Add("P_F_LIC ", OracleDbType.Varchar2, ParameterDirection.Input).Value = strFacilityLic;
            cmd.Parameters.Add(new OracleParameter("io_cursor", OracleDbType.RefCursor)).Direction = ParameterDirection.ReturnValue;
            //OracleDataAdapter da = new OracleDataAdapter(cmd);
            cmd.ExecuteNonQuery();

            Oracle.ManagedDataAccess.Types.OracleRefCursor t = (Oracle.ManagedDataAccess.Types.OracleRefCursor)cmd.Parameters["io_cursor"].Value;
            OracleDataReader rdr = t.GetDataReader();
            var dataTable = new DataTable();
            dataTable.Load(rdr);
            dataTable.TableName = "FacilityDetails";

            return dataTable;
        }

        //Get Patient Prescription tag Header row Based on Unique eRx no
        public DataTable GETPATPRESCRIPTIONHEAD(int _eRxNo)
        {
            OracleCommand cmd = new OracleCommand();
            cmd.CommandType = CommandType.Text;

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "T_ERX.GETPATPRESCRIPTIONHEAD";

            cmd.BindByName = true;
            cmd.Connection = OpenConnection();
            cmd.Parameters.Add("PRXNO", OracleDbType.Int16, ParameterDirection.Input).Value = _eRxNo;
            cmd.Parameters.Add(new OracleParameter("io_cursor", OracleDbType.RefCursor)).Direction = ParameterDirection.ReturnValue;
            //OracleDataAdapter da = new OracleDataAdapter(cmd);
            cmd.ExecuteNonQuery();

            Oracle.ManagedDataAccess.Types.OracleRefCursor t = (Oracle.ManagedDataAccess.Types.OracleRefCursor)cmd.Parameters["io_cursor"].Value;
            OracleDataReader rdr = t.GetDataReader();
            var dataTable = new DataTable();
            dataTable.Load(rdr);

            dataTable.TableName = "Prescription";
            return dataTable;

        }

        //Get Patient Details row Based on Unique eRx no
        public DataTable GETPATRXPATIENTTAG(int _eRxNo)
        {
            OracleCommand cmd = new OracleCommand();
            cmd.CommandType = CommandType.Text;

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "T_ERX.GETPATRXPATIENTTAG";

            cmd.BindByName = true;
            cmd.Connection = OpenConnection();
            cmd.Parameters.Add("PRXNO", OracleDbType.Int16, ParameterDirection.Input).Value = _eRxNo;
            cmd.Parameters.Add(new OracleParameter("io_cursor", OracleDbType.RefCursor)).Direction = ParameterDirection.ReturnValue;
            //OracleDataAdapter da = new OracleDataAdapter(cmd);
            cmd.ExecuteNonQuery();

            Oracle.ManagedDataAccess.Types.OracleRefCursor t = (Oracle.ManagedDataAccess.Types.OracleRefCursor)cmd.Parameters["io_cursor"].Value;
            OracleDataReader rdr = t.GetDataReader();
            var dataTable = new DataTable();
            dataTable.Load(rdr);

            dataTable.TableName = "Patient";
            return dataTable;

        }

        public DataTable GETPATWITHREFNO(int _eRxNo)
        {
            OracleCommand cmd = new OracleCommand();
            cmd.CommandType = CommandType.Text;

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "T_ERX.GETPATWITHREFNO";

            cmd.BindByName = true;
            cmd.Connection = OpenConnection();
            cmd.Parameters.Add("PRXNO", OracleDbType.Int16, ParameterDirection.Input).Value = _eRxNo;
            cmd.Parameters.Add(new OracleParameter("io_cursor", OracleDbType.RefCursor)).Direction = ParameterDirection.ReturnValue;
            //OracleDataAdapter da = new OracleDataAdapter(cmd);
            cmd.ExecuteNonQuery();

            Oracle.ManagedDataAccess.Types.OracleRefCursor t = (Oracle.ManagedDataAccess.Types.OracleRefCursor)cmd.Parameters["io_cursor"].Value;
            OracleDataReader rdr = t.GetDataReader();
            var dataTable = new DataTable();
            dataTable.Load(rdr);

            dataTable.TableName = "PatwithRefNo";
            return dataTable;

        }

        public DataTable GETPATRXENCOUNTER(int _eRxNo)
        {
            OracleCommand cmd = new OracleCommand();
            cmd.CommandType = CommandType.Text;

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "T_ERX.GETPATRXENCOUNTER";

            cmd.BindByName = true;
            cmd.Connection = OpenConnection();
            cmd.Parameters.Add("PRXNO", OracleDbType.Int16, ParameterDirection.Input).Value = _eRxNo;
            cmd.Parameters.Add(new OracleParameter("io_cursor", OracleDbType.RefCursor)).Direction = ParameterDirection.ReturnValue;
            //OracleDataAdapter da = new OracleDataAdapter(cmd);
            cmd.ExecuteNonQuery();

            Oracle.ManagedDataAccess.Types.OracleRefCursor t = (Oracle.ManagedDataAccess.Types.OracleRefCursor)cmd.Parameters["io_cursor"].Value;
            OracleDataReader rdr = t.GetDataReader();
            var dataTable = new DataTable();
            dataTable.Load(rdr);

            dataTable.TableName = "Encounter";
            return dataTable;
        }

        public DataTable GETPATRXDIAGNOSIS(int _eRxNo)
        {
            OracleCommand cmd = new OracleCommand();
            cmd.CommandType = CommandType.Text;

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "T_ERX.GETPATRXDIAGNOSIS";

            cmd.BindByName = true;
            cmd.Connection = OpenConnection();
            cmd.Parameters.Add("PRXNO", OracleDbType.Int16, ParameterDirection.Input).Value = _eRxNo;
            cmd.Parameters.Add(new OracleParameter("io_cursor", OracleDbType.RefCursor)).Direction = ParameterDirection.ReturnValue;
            //OracleDataAdapter da = new OracleDataAdapter(cmd);
            cmd.ExecuteNonQuery();

            Oracle.ManagedDataAccess.Types.OracleRefCursor t = (Oracle.ManagedDataAccess.Types.OracleRefCursor)cmd.Parameters["io_cursor"].Value;
            OracleDataReader rdr = t.GetDataReader();
            var dataTable = new DataTable();
            dataTable.Load(rdr);

            dataTable.TableName = "Diagnosis";
            return dataTable;
        }

        public DataTable GETPATRXACTIVITY(int _eRxNo)
        {
            OracleCommand cmd = new OracleCommand();
            cmd.CommandType = CommandType.Text;

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "T_ERX.GETPATRXACTIVITY";

            cmd.BindByName = true;
            cmd.Connection = OpenConnection();
            cmd.Parameters.Add("PRXNO", OracleDbType.Int16, ParameterDirection.Input).Value = _eRxNo;
            cmd.Parameters.Add(new OracleParameter("io_cursor", OracleDbType.RefCursor)).Direction = ParameterDirection.ReturnValue;
            //OracleDataAdapter da = new OracleDataAdapter(cmd);
            cmd.ExecuteNonQuery();

            Oracle.ManagedDataAccess.Types.OracleRefCursor t = (Oracle.ManagedDataAccess.Types.OracleRefCursor)cmd.Parameters["io_cursor"].Value;
            OracleDataReader rdr = t.GetDataReader();
            var dataTable = new DataTable();
            dataTable.Load(rdr);

            dataTable.TableName = "Activity";
            return dataTable;
        }

        public DataTable GETPATRXOBSERVATION(int _eRxNo, int _actId)
        {
            OracleCommand cmd = new OracleCommand();
            cmd.CommandType = CommandType.Text;

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "T_ERX.GETPATRXOBSERVATION";

            cmd.BindByName = true;
            cmd.Connection = OpenConnection();
            cmd.Parameters.Add("PRXNO", OracleDbType.Int16, ParameterDirection.Input).Value = _eRxNo;
            cmd.Parameters.Add(new OracleParameter("io_cursor", OracleDbType.RefCursor)).Direction = ParameterDirection.ReturnValue;
            //OracleDataAdapter da = new OracleDataAdapter(cmd);
            cmd.ExecuteNonQuery();

            Oracle.ManagedDataAccess.Types.OracleRefCursor t = (Oracle.ManagedDataAccess.Types.OracleRefCursor)cmd.Parameters["io_cursor"].Value;
            OracleDataReader rdr = t.GetDataReader();
            var dataTable = new DataTable();
            dataTable.Load(rdr);

            dataTable.TableName = "OBSERVATION";
            return dataTable;

        }

        public DataTable GETPATPENINGERX()
        {
            DataTable dataTable = null;
            try
            {
                var con = OpenConnection();
                if(con.State != ConnectionState.Open)
                {
                    con.Open();
                }
                string strQry = "select DISTINCT ERX_NO,SENDER_ID from  PAT_ERX_HEADER where  TRANSACTION_DATE >= SYSDATE - 5 and Status =0 and RECEIVER_ID <> 'CASH'";
                OracleDataAdapter Da = new OracleDataAdapter(strQry, con);
                dataTable = new DataTable();
                Da.Fill(dataTable);
                dataTable.TableName = "PendingReq";
            }
            catch (Exception ex) { throw ex; }
            return dataTable;
        }

        //testing
        public DataTable GetXMLTYPECLOB()
        {
            DataTable dataTable = null;
            try
            {
                string strQry = "select * from  TEMP_TABLE_XML_FILE";
                OracleDataAdapter Da = new OracleDataAdapter(strQry, OpenConnection());
                dataTable = new DataTable();
                Da.Fill(dataTable);
                dataTable.TableName = "PendingReq";
            }
            catch (Exception ex) { throw ex; }
            return dataTable;
        }

        //To get the prescriving physician License using prescription no.
        public string GetOrderingPhy(int _eRxNo)
        {
            DataTable dataTable = null;
            string clinician = null;
            try
            {
                string strQry = "select DISTINCT ERX_NO,CLINICIAN_ID from  pat_erx_encounter where  ERX_NO ='" + _eRxNo + "'";
                OracleDataAdapter Da = new OracleDataAdapter(strQry, OpenConnection());
                dataTable = new DataTable();
                Da.Fill(dataTable);
                if (dataTable != null && dataTable.Rows.Count > 0)
                {
                    clinician = dataTable.Rows[0]["CLINICIAN_ID"].ToString();
                }
            }
            catch (Exception ex) { throw ex; }
            return clinician;
        }

        public bool UpdateStatusCode(int _eRxNo, int _ResponseCode)
        {
            bool updresponse = false;
            try
            {
                OracleCommand cmd = new OracleCommand();
                cmd.CommandType = CommandType.Text;

                cmd.CommandText = "Update PAT_ERX_header set  Status=:STATUSCODE,LAST_REQ_STATUS_TIME=SYSDATE where ERX_NO= :ERXNO ";
                cmd.Parameters.Add(new OracleParameter("STATUSCODE", _ResponseCode));
                cmd.Parameters.Add(new OracleParameter("ERXNO", _eRxNo));
                using (OracleConnection con = OpenConnection())
                {
                    cmd.Connection = con;
                    cmd.ExecuteNonQuery();
                }
            }
            catch (OracleException ex)
            {
                throw ex;
            }
            return updresponse;

        }

        //Update Patient eRx Respose Code
        public bool UpdateResponseCode(int _eRxNo, int _ResponseCode, int _DHAResponse)
        {
            bool updresponse = false;
            try
            {
                OracleCommand cmd = new OracleCommand();
                cmd.CommandType = CommandType.Text;

                cmd.CommandText = "Update PAT_ERX_header set  Status=:STATUSCODE,RES_CODE=:DHARes,LAST_RES_STATUS_TIME=SYSDATE where ERX_NO= :ERXNO ";
                cmd.Parameters.Add(new OracleParameter("STATUSCODE", _ResponseCode));
                cmd.Parameters.Add(new OracleParameter("DHARes", _DHAResponse));
                cmd.Parameters.Add(new OracleParameter("ERXNO", _eRxNo));

                using (OracleConnection con = OpenConnection())
                {
                    cmd.Connection = con;
                    int i = cmd.ExecuteNonQuery();
                    if (i == 1) updresponse = true;
                }
            }
            catch (OracleException ex)
            {
                throw ex;
            }
            return updresponse;

        }

        //Patient eRx transaction history
        public bool InsertErXHistory(int _eRxNo, string strPrescId, int _ResponseCode)
        {
            bool updresponse = false;
            try
            {
                int _TrN = 0;
                OracleCommand cmdTrn = new OracleCommand();
                cmdTrn.CommandType = CommandType.Text;
                cmdTrn.CommandText = "SELECT  NVL(MAX(TRN),0)+1 AS NXTTRN FROM  PAT_ERX_HISTORY";
                using (OracleConnection con = OpenConnection())
                {
                    cmdTrn.Connection = con;
                    var NxtTrn = cmdTrn.ExecuteScalar();
                    if (NxtTrn != null) _TrN = Convert.ToInt32(NxtTrn);
                }

                string strInsQry = "INSERT INTO PAT_ERX_HISTORY (TRN, ERX_NO, PRESC_ID, RESPONSE_CODE, ACT_DATETIME) ";
                strInsQry += " VALUES(:TRN, :ERX_NO, :PRESC_ID, :RESPONSE_CODE, TO_DATE(:ACT_DATETIME, 'DD/MM/YYYY HH24:MI:SS'))";
                OracleCommand cmd = new OracleCommand();
                cmd.CommandType = CommandType.Text;

                cmd.CommandText = strInsQry;
                cmd.Parameters.Add(new OracleParameter("TRN", _TrN));
                cmd.Parameters.Add(new OracleParameter("ERXNO", _eRxNo));
                cmd.Parameters.Add(new OracleParameter("PRESC_ID", strPrescId));
                cmd.Parameters.Add(new OracleParameter("RESPONSE_CODE", _ResponseCode));
                cmd.Parameters.Add(new OracleParameter("ACT_DATETIME", System.DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss", System.Globalization.CultureInfo.InvariantCulture)));

                using (OracleConnection con = OpenConnection())
                {
                    cmd.Connection = con;
                    cmd.ExecuteNonQuery();
                }
            }
            catch (OracleException ex)
            {
                throw ex;
            }
            return updresponse;

        }

        //Update Patient eRx-Authorization response into Encounter Table
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

                            /*
                             * string strupdQryact = "UPDATE PAT_ERX_ACTIVITY SET R_APPROVED_QTY='" + pstrActivity[3] + "', ";
                            strupdQryact += " R_NET='" + pstrActivity[4] + "',R_LIST='" + pstrActivity[5] + "', ";
                            strupdQryact += " R_PAT_SHARE ='" + pstrActivity[6] + "',R_PAT_AMT='" + pstrActivity[7] + "', ";
                            strupdQryact += " R_DEN_CODE ='" + pstrActivity[8] + "',UPD_DATETIME=SYSDATE ";
                            strupdQryact += " WHERE ERX_NO = (SELECT ERX_NO from PAT_ERX_ENCOUNTER where ERX_ID ='" + pstrAuthorization[0] + "') ";
                            strupdQryact += " AND ACT_CODE = '" + pstrActivity[2] + "'"; 
                            *
                            */

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
                            log.Info($"eRx Id {pstrAuthorization[0]} response updated Successfully.");
                        }
                        else oratrns.Rollback();
                    }
                }
                catch (OracleException ex)
                {
                    throw ex;
                    oratrns.Rollback();
                }
            }
            return UpdateAuth;

        }

        //IN_USER NUMBER, IN_PRSC NUMBER, IN_MNO NUMBER,IN_R_STAT NUMBER, IN_ULD_STAT NUMBER, IN_RPL_STAT NUMBER 
        public int InsertXMlError(int _eRXNo, string IN_FILE_NAME, string IN_TRN, string IN_TYPE,
            string IN_OBJ_NAME, string IN_HAAD_FLD, string IN_FLD_VAL, string IN_ADD_REF, string IN_ERROR_TXT)
        {
            int rows = -1;
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

                using (OracleConnection con = OpenConnection())
                {
                    //OracleDataAdapter da = new OracleDataAdapter(cmd); 
                    //cmd.BindByName = true;
                    cmd.Connection = con;
                    rows = cmd.ExecuteNonQuery();
                }
            }
            catch (OracleException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                OpenConnection().Close();
            }
            return rows;
        }

        public int InsertXMlData(int _RxNo, int _ErrorStatus, string strfilename, string strxmldata, string strmsg)
        {
            int rows = -1;
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

                using (OracleConnection con = OpenConnection())
                {
                    cmd.Connection = con;
                    rows = cmd.ExecuteNonQuery();
                }
            }
            catch (OracleException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return rows;
        }

        public Int32 GetRequestErxNo(string strErxid)
        {
            Int32 retval = -1;
            try
            {
                OracleCommand cmd = new OracleCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "T_ERX.GET_ERXNO_FROM_ERXID";

                cmd.Parameters.Add(new OracleParameter("IN_ERX_ID", strErxid));
                cmd.Parameters.Add("eRx_No", OracleDbType.Varchar2, 2000).Direction = ParameterDirection.ReturnValue;

                using (OracleConnection con = OpenConnection())
                {
                    cmd.Connection = con;
                    cmd.BindByName = true;
                    cmd.ExecuteNonQuery();
                    retval = Convert.ToInt32(cmd.Parameters["eRx_No"].Value.ToString());
                }
            }
            catch (OracleException ex) { throw ex; }
            catch (Exception ex) { throw ex; }
            return retval;
        }

        public void PopulatePendingRequest(int erxno, int senderId, string jsonContent)
        {

        }
    }
}
