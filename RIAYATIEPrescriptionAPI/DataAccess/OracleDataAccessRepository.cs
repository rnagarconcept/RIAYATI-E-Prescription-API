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
                                obj.REQUEST_TYPE = reader.ToString("RECEIVER_ID");                               
                                obj.ID = reader.ToInt32("ID");
                                obj.REQUEST_TYPE = reader.ToString("REQUEST_TYPE");
                                obj.PAYLOAD = reader.ToString("PAYLOAD");
                                obj.FacilityId = reader.ToInt32("FacilityId");
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
    }
}
