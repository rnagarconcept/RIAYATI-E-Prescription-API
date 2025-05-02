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
    public class OracleDataAccessRepository : OracleRepositoryBase
    {
        private static string ApiBaseURL = ConfigurationManager.AppSettings["API_BASE_URL"];
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
        public async Task<TokenResponseModel> GetAuthToken(int F_LIC, string F_USER, string F_PWD)
        {
            TokenResponseModel result = null;
            //try
            //{
            //    log.Info($"Start getting token for {F_LIC}");
            //    result = CacheManager.Instance.GetCache<TokenResponseModel>($"{GlobalConstants.AUTH_TOKEN}-{F_LIC}" );
            //    if (result == null || string.IsNullOrEmpty(result.AccessToken))
            //    {
            //        result = new TokenResponseModel();
            //        using (var httpClient = new HttpClient())
            //        {
            //            var request = new HttpRequestMessage();
            //            request.RequestUri = new Uri($"{ApiBaseURL}/{fragment}");
            //            request.Method = HttpMethod.Post;
            //            request.Headers.Add("ClientId", settings.ClientId);
            //            request.Headers.Add("ClientSecret", settings.ClientSecret);
            //            request.Headers.Add("AppId", settings.AppID);
            //            var response = await httpClient.SendAsync(request);
            //            if (response.IsSuccessStatusCode)
            //            {
            //                var responseContent = await response.Content.ReadAsStringAsync();
            //                if (!string.IsNullOrEmpty(responseContent))
            //                {
            //                    result = JsonConvert.DeserializeObject<TokenResponseModel>(responseContent);
            //                    CacheManager.Instance.SetCache(GlobalConstaints.AUTH_TOKEN, result);
            //                }
            //            }
            //            else
            //            {
            //                var responseContent = await response.Content.ReadAsStringAsync();
            //                var message = response.ReasonPhrase;
            //                log.Info($"Error in getting token Message {message} Response Content {responseContent}");
            //            }
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    result.Message = "Error";
            //    log.Info($"Error in getting token {ex.Message} - {ex.InnerException.Message}", ex);
            //}
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
                                obj.ID = reader.ToInt32("ID");
                                obj.FacilityId = reader.ToInt32("FACILITY_ID");
                                obj.REQUEST_TYPE = reader.ToString("REQUEST_TYPE");                               
                                obj.PAYLOAD = reader.ToBlobString("PAYLOAD");
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
        public void UpdatePendingRequestStatus(PendingRequestStatus obj)
        {
            OracleConnection con = null;
            try
            {
                using (con = OpenConnection())
                {
                    using (OracleCommand OraCmd = new OracleCommand())
                    {
                        OraCmd.Connection = con;
                        OraCmd.CommandType = CommandType.StoredProcedure;
                        OraCmd.CommandText = OraCmd.CommandText = $"{PackageName}.UPDATE_PENDING_REQUESTS";
                        OraCmd.Parameters.Add("P_ID", obj.RequestId);
                        OraCmd.Parameters.Add("P_IS_PROCESSING", obj.IsProcessing);
                        OraCmd.Parameters.Add("P_RESPONSE_STATUS_CODE", obj.Status);
                        OraCmd.Parameters.Add("P_ERROR_MESSAGE", obj.ErrorMessage);                        
                        if (obj.Response != null)
                        {
                            if (obj.Response.Length < 32000)
                            {
                                OraCmd.Parameters.Add("P_RESPONSE", obj.Response);
                            }
                            else
                            {
                                OraCmd.Parameters.Add("P_RESPONSE", "Response Too Large");
                            }
                        }
                        else
                        {
                            OraCmd.Parameters.Add("P_RESPONSE", "");
                        }

                        if (con.State == ConnectionState.Closed)
                        {
                            con.Open();
                        }
                        OraCmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error($"Error in Update Pending Requests for Request {obj.RequestId} Error : {ex.Message}");
            }
        }
        
        public int SaveErxResponse(int requestId, ErxResponseModel obj)
        {
            OracleConnection con = null;           
            try
            {
                using (con = OpenConnection())
                {
                    using (OracleCommand OraCmd = new OracleCommand())
                    {
                        OraCmd.Connection = con;
                        OraCmd.CommandType = CommandType.StoredProcedure;
                        OraCmd.CommandText = $"{PackageName}.SAVE_ERX_RESPONSE";
                        OraCmd.Parameters.Add("P_REQ_ID", requestId);
                        OraCmd.Parameters.Add("P_SenderID", obj.ErxAuthorization.Header.SenderID);
                        OraCmd.Parameters.Add("P_ReceiverID", obj.ErxAuthorization.Header.ReceiverID);
                        OraCmd.Parameters.Add("P_TransactionDate", obj.ErxAuthorization.Header.TransactionDate);
                        OraCmd.Parameters.Add("P_RecordCount", obj.ErxAuthorization.Header.RecordCount);
                        OraCmd.Parameters.Add("P_DispositionFlag", obj.ErxAuthorization.Header.DispositionFlag);
                        OraCmd.Parameters.Add("P_PayerID", obj.ErxAuthorization.Header.PayerID);                       
                        if (con.State == ConnectionState.Closed)
                        {
                            con.Open();
                        }
                        OraCmd.ExecuteNonQuery();                       
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error($"Error GetPendingRequests {ex.Message}", ex);
            }
            return 1;
        }

        public void SAVE_ERX_RESPONSE_AUTHORIZATION(int requestId, ErxResponseModel obj)
        {
            OracleConnection con = null;
            try
            {
                using (con = OpenConnection())
                {
                    using (OracleCommand OraCmd = new OracleCommand())
                    {
                        OraCmd.Connection = con;
                        OraCmd.CommandType = CommandType.StoredProcedure;
                        OraCmd.CommandText = $"{PackageName}.SAVE_ERX_RESPONSE_AUTHORIZATION";
                        OraCmd.Parameters.Add("P_REQ_ID", requestId);
                        OraCmd.Parameters.Add("P_AUTH_ID", obj.ErxAuthorization.Authorization.ID);
                        OraCmd.Parameters.Add("P_IDPayer", obj.ErxAuthorization.Authorization.IDPayer);
                        OraCmd.Parameters.Add("P_DenialCode", obj.ErxAuthorization.Authorization.DenialCode);
                        OraCmd.Parameters.Add("P_Start_Date", obj.ErxAuthorization.Authorization.Start);
                        OraCmd.Parameters.Add("P_End_Date", obj.ErxAuthorization.Authorization.End);
                        
                        if (con.State == ConnectionState.Closed)
                        {
                            con.Open();
                        }
                        OraCmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error($"Error GetPendingRequests {ex.Message}", ex);
            }           
        }

        public void SAVE_ERX_RESPONSE_AUTHORIZATION_ACTIVITY(int requestId,int authId, List<ActivityEx> activities)
        {
            OracleConnection con = null;
            try
            {
                using (con = OpenConnection())
                {
                    using (var OraCmd = new OracleCommand())
                    {
                        OraCmd.Connection = con;
                        OraCmd.CommandType = CommandType.StoredProcedure;
                        OraCmd.CommandText = OraCmd.CommandText = $"{PackageName}.PAT_ERX_AUTHORIZATION_ACTIVITY";
                        
                        OraCmd.Parameters.Add(new OracleParameter("P_AUTH_ID", OracleDbType.Int32));
                        OraCmd.Parameters.Add(new OracleParameter("P_REQ_ID", OracleDbType.Int32));
                        OraCmd.Parameters.Add(new OracleParameter("P_Activity_Type", OracleDbType.Int32));
                        OraCmd.Parameters.Add(new OracleParameter("P_Code", OracleDbType.NVarchar2));
                        OraCmd.Parameters.Add(new OracleParameter("P_Quantity", OracleDbType.Int32));
                        OraCmd.Parameters.Add(new OracleParameter("P_Net", OracleDbType.Int32));
                        OraCmd.Parameters.Add(new OracleParameter("P_List", OracleDbType.Int32));
                        OraCmd.Parameters.Add(new OracleParameter("P_PatientShare", OracleDbType.Int32));
                        OraCmd.Parameters.Add(new OracleParameter("P_PaymentAmount", OracleDbType.Int32));

                        int[] P_AUTH_ID = new int[activities.Count];
                        int[] P_REQ_ID = new int[activities.Count];
                        string[] P_Activity_Type = new string[activities.Count];
                        string[] P_Code = new string[activities.Count];
                        string[] P_Quantity = new string[activities.Count];
                        string[] P_Net = new string[activities.Count];
                        string[] P_List = new string[activities.Count];
                        string[] P_PatientShare = new string[activities.Count];
                        string[] P_PaymentAmount = new string[activities.Count];

                        for (int i = 0; i < activities.Count; i++)
                        {
                            P_AUTH_ID[i] = authId;
                            P_REQ_ID[i] = requestId;
                            P_Activity_Type[i] = activities[i].Type;
                            P_Code[i] = activities[i].Code;
                            P_Quantity[i] = activities[i].Quantity;
                            P_Net[i] = activities[i].Net;
                            P_List[i] = activities[i].List;
                            P_PatientShare[i] = activities[i].PatientShare;
                            P_PaymentAmount[i] = activities[i].PaymentAmount;
                        }
                        OraCmd.Parameters["P_AUTH_ID"].Value = P_AUTH_ID;
                        OraCmd.Parameters["P_REQ_ID"].Value = P_REQ_ID;
                        OraCmd.Parameters["P_Activity_Type"].Value = P_Activity_Type;
                        OraCmd.Parameters["P_Code"].Value = P_Code;
                        OraCmd.Parameters["P_Quantity"].Value = P_Quantity;
                        OraCmd.Parameters["P_Net"].Value = P_Net;
                        OraCmd.Parameters["P_List"].Value = P_List;
                        OraCmd.Parameters["P_PatientShare"].Value = P_PatientShare;
                        OraCmd.Parameters["P_PaymentAmount"].Value = P_PaymentAmount;
                        if (con.State == ConnectionState.Closed)
                        {
                            con.Open();
                        }
                        OraCmd.ExecuteNonQuery();                                            
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error($"Error SAVE_ERX_RESPONSE_AUTHORIZATION_ACTIVITY {ex.Message}", ex);
            }
        }

        public void SAVE_ERX_RESPONSE_AUTHORIZATION_ACTIVITY_OBS(int requestId, int authId, List<Observation> observations)
        {
            OracleConnection con = null;
            try
            {
                using (con = OpenConnection())
                {
                    using (var OraCmd = new OracleCommand())
                    {
                        OraCmd.Connection = con;
                        OraCmd.CommandType = CommandType.StoredProcedure;
                        OraCmd.CommandText = OraCmd.CommandText = $"{PackageName}.SAVE_ERX_RESPONSE_AUTH_ACT_OBSERVATION";

                        OraCmd.Parameters.Add(new OracleParameter("P_ACTIVITY_ID", OracleDbType.Int32));
                        OraCmd.Parameters.Add(new OracleParameter("P_REQ_ID", OracleDbType.Int32));
                        OraCmd.Parameters.Add(new OracleParameter("P_Type", OracleDbType.NVarchar2));
                        OraCmd.Parameters.Add(new OracleParameter("P_Code", OracleDbType.NVarchar2));
                        OraCmd.Parameters.Add(new OracleParameter("P_VALUE", OracleDbType.NVarchar2));
                        OraCmd.Parameters.Add(new OracleParameter("P_ValueType", OracleDbType.NVarchar2));
                       
                        int[] P_ACTIVITY_ID = new int[observations.Count];
                        int[] P_REQ_ID = new int[observations.Count];
                        string[] P_Type = new string[observations.Count];
                        string[] P_Code = new string[observations.Count];
                        string[] P_VALUE = new string[observations.Count];
                        string[] P_ValueType = new string[observations.Count];
                       
                        for (int i = 0; i < observations.Count; i++)
                        {
                            P_ACTIVITY_ID[i] = authId;
                            P_REQ_ID[i] = requestId;
                            P_Type[i] = observations[i].Type;
                            P_Code[i] = observations[i].Code;
                            P_VALUE[i] = observations[i].Value;
                            P_ValueType[i] = observations[i].ValueType;                           
                        }
                        OraCmd.Parameters["P_ACTIVITY_ID"].Value = P_ACTIVITY_ID;
                        OraCmd.Parameters["P_REQ_ID"].Value = P_REQ_ID;
                        OraCmd.Parameters["P_Type"].Value = P_Type;
                        OraCmd.Parameters["P_Code"].Value = P_Code;
                        OraCmd.Parameters["P_VALUE"].Value = P_VALUE;
                        OraCmd.Parameters["P_ValueType"].Value = P_ValueType;                      
                        if (con.State == ConnectionState.Closed)
                        {
                            con.Open();
                        }
                        OraCmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error($"Error SAVE_ERX_RESPONSE_AUTHORIZATION_ACTIVITY_OBS {ex.Message}", ex);
            }
        }
      
        public void SAVE_ERX_TRAN_RESPONSE(int requestId, TransactionResponseModel obj)
        {
            OracleConnection con = null;
            try
            {
                using (con = OpenConnection())
                {
                    using (OracleCommand OraCmd = new OracleCommand())
                    {
                        OraCmd.Connection = con;
                        OraCmd.CommandType = CommandType.StoredProcedure;
                        OraCmd.CommandText = $"{PackageName}.SAVE_ERX_TRAN_RESPONSE";
                        OraCmd.Parameters.Add("P_REQ_ID", requestId);
                        OraCmd.Parameters.Add("P_StatusCode", obj.StatusCode);
                        OraCmd.Parameters.Add("P_Message", obj.Message);
                        OraCmd.Parameters.Add("P_UserMessage", obj.UserMessage);
                        OraCmd.Parameters.Add("P_MemberValidation", obj.MemberValidation);
                        OraCmd.Parameters.Add("P_DispositionFlag", obj.DispositionFlag);
                        OraCmd.Parameters.Add("P_EntityID", obj.EntityID);
                        OraCmd.Parameters.Add("P_ReferenceNumber", obj.ReferenceNumber);
                        if (con.State == ConnectionState.Closed)
                        {
                            con.Open();
                        }
                        OraCmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error($"Error SAVE_ERX_TRAN_RESPONSE {ex.Message}", ex);
            }           
        }

        public void SAVE_ERX_TRAN_RESPONSE_ERROR(List<TransactionResponseErrorModel> items)
        {
            OracleConnection con = null;
            try
            {
                using (con = OpenConnection())
                {
                    using (var OraCmd = new OracleCommand())
                    {
                        OraCmd.Connection = con;
                        OraCmd.CommandType = CommandType.StoredProcedure;
                        OraCmd.CommandText = OraCmd.CommandText = $"{PackageName}.SAVE_ERX_TRAN_RESPONSE_ERROR";
                        OraCmd.Parameters.Add(new OracleParameter("P_ENTITY_ID", OracleDbType.NVarchar2));
                        OraCmd.Parameters.Add(new OracleParameter("P_AdditionalReference", OracleDbType.NVarchar2));
                        OraCmd.Parameters.Add(new OracleParameter("P_AdditionalReferenceObjectName", OracleDbType.NVarchar2));
                        OraCmd.Parameters.Add(new OracleParameter("P_Reference", OracleDbType.NVarchar2));
                        OraCmd.Parameters.Add(new OracleParameter("P_ReferenceObjectName", OracleDbType.NVarchar2));
                        OraCmd.Parameters.Add(new OracleParameter("P_PropertyName", OracleDbType.NVarchar2));
                        OraCmd.Parameters.Add(new OracleParameter("P_RuleCode", OracleDbType.NVarchar2));
                        OraCmd.Parameters.Add(new OracleParameter("P_ErrorText", OracleDbType.NVarchar2));
                        OraCmd.Parameters.Add(new OracleParameter("P_ObjectName", OracleDbType.NVarchar2));
                        OraCmd.Parameters.Add(new OracleParameter("P_Transaction_Type", OracleDbType.NVarchar2));

                        string[] P_ENTITY_ID = new string[items.Count];
                        string[] P_AdditionalReference = new string[items.Count];
                        string[] P_AdditionalReferenceObjectName = new string[items.Count];
                        string[] P_Reference = new string[items.Count];
                        string[] P_ReferenceObjectName = new string[items.Count];
                        string[] P_PropertyName = new string[items.Count];
                        string[] P_RuleCode = new string[items.Count];
                        string[] P_ErrorText = new string[items.Count];
                        string[] P_ObjectName = new string[items.Count];
                        string[] P_Transaction_Type = new string[items.Count];
                       

                        for (int i = 0; i < items.Count; i++)
                        {
                            P_ENTITY_ID[i] = items[i].EntityID;
                            P_AdditionalReference[i] = items[i].AdditionalReference;
                            P_AdditionalReferenceObjectName[i] = items[i].AdditionalReferenceObjectName;
                            P_Reference[i] = items[i].Reference;
                            P_ReferenceObjectName[i] = items[i].ReferenceObjectName;
                            P_PropertyName[i] = items[i].PropertyName;
                            P_RuleCode[i] = items[i].RuleCode;
                            P_ErrorText[i] = items[i].ErrorText;
                            P_ObjectName[i] = items[i].ObjectName;
                            P_Transaction_Type[i] = items[i].Transaction;
                            
                        }
                        OraCmd.Parameters["P_ENTITY_ID"].Value = P_ENTITY_ID;
                        OraCmd.Parameters["P_AdditionalReference"].Value = P_AdditionalReference;
                        OraCmd.Parameters["P_AdditionalReferenceObjectName"].Value = P_AdditionalReferenceObjectName;
                        OraCmd.Parameters["P_Reference"].Value = P_Reference;
                        OraCmd.Parameters["P_ReferenceObjectName"].Value = P_ReferenceObjectName;
                        OraCmd.Parameters["P_PropertyName"].Value = P_PropertyName;
                        OraCmd.Parameters["P_RuleCode"].Value = P_RuleCode;
                        OraCmd.Parameters["P_ErrorText"].Value = P_ErrorText;
                        OraCmd.Parameters["P_ObjectName"].Value = P_ObjectName;
                        OraCmd.Parameters["P_Transaction_Type"].Value = P_Transaction_Type;
                        OraCmd.ArrayBindCount = P_ENTITY_ID.Length;
                        if (con.State == ConnectionState.Closed)
                        {
                            con.Open();
                        }
                        OraCmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error($"Error SAVE_ERX_TRAN_RESPONSE_ERROR {ex.Message}", ex);
            }
        }
    }
}
