using DomainModel.Models.Request;
using DomainModel.Models.Response;
using log4net;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationInsight
{
    public class APIConnectService
    {
        ILog log = LogManager.GetLogger(typeof(APIConnectService));
        public readonly HttpClient httpClient;
        public readonly HttpRequestMessage message;
        public ApiResponseModel responseModel;
        public static readonly string DbStore = ConfigurationManager.AppSettings["DBStore"];
        private static readonly Lazy<APIConnectService> lazy = new Lazy<APIConnectService>(() => new APIConnectService());
        private APIConnectService()
        {
        }
        public static APIConnectService GetInstance
        {
            get
            {
                return lazy.Value;
            }
        }
        public async Task<ApiResponseModel> SendAsync(ApiRequestModel model)
        {            
            try
            {
                responseModel = new ApiResponseModel();
                HttpClient client = new HttpClient { BaseAddress = new Uri(model.ApiUrl) };
                client.DefaultRequestHeaders.Add("Username", model.Auth.F_USER);
                client.DefaultRequestHeaders.Add("Password", model.Auth.F_PWD);
                HttpResponseMessage response = await client.GetAsync(model.EndPoint);
                if (response != null)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    if (response.IsSuccessStatusCode && response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        log.Info($"API Response Status Code {response.StatusCode.ToString()} and Contents {responseContent}");
                        responseModel.Data = responseContent;
                    }
                    else
                    {
                        responseModel.Message = response.ReasonPhrase;
                        log.Info($"API Response Status Code {response.ReasonPhrase} and Contents {responseContent}");
                    }
                    responseModel.StatusCode = Convert.ToInt32(response.StatusCode);
                }
                else
                {
                    log.Error($"Error No Response From API : {model.Auth.F_LIC}");
                    throw new Exception("Error No Response From API");
                }
            }
            catch (Exception ex)
            {
                responseModel.Message = "Error";
                responseModel.ErrorMessages = ex.Message;
                log.Error($"Error api request {ex.Message}", ex);
                throw ex;
            }
            return responseModel;
        }
    }
}
