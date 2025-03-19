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
            var payload = JsonConvert.SerializeObject(model);
            try
            {
                responseModel = new ApiResponseModel();
                log.Info($"Start sending api request with payload \n {payload}\n");
                var httpClient = new HttpClient();
                var message = new HttpRequestMessage();
                message.RequestUri = new Uri($"{model.ApiUrl}");
                if (!string.IsNullOrEmpty(model.AuthToken))
                {
                    httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {model.AuthToken}");
                }
                if (model.CustomHeaders != null && model.CustomHeaders.Count > 0)
                {
                    foreach (var header in model.CustomHeaders)
                    {
                        httpClient.DefaultRequestHeaders.Add(header.Key, header.Value);
                    }
                }
                if (model.Data != null)
                {
                    //var jsonContent = JsonConvert.SerializeObject(model.Data);
                    var jsonContent = model.Data;
                    message.Content = new StringContent(jsonContent.ToString(), Encoding.UTF8, "application/json");
                }
                message.Method = model.Method;
                var response = Task.Run(() => httpClient.SendAsync(message)).Result;
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
                    log.Error($"Error No Response From API : {payload}");
                    throw new Exception("No response return from API");
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
