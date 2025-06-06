﻿using DomainModel.Models.Request;
using DomainModel.Models.Response;
using log4net;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
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
        public HttpRequestMessage message;
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
                HttpResponseMessage response = null;
                responseModel = new ApiResponseModel();
                HttpClient client = new HttpClient { BaseAddress = new Uri(model.ApiUrl) };
                client.DefaultRequestHeaders.Add("Username", model.Auth.F_USER);
                client.DefaultRequestHeaders.Add("Password", model.Auth.F_PWD);
                if (model.CustomHeaders != null && model.CustomHeaders.Count > 0)
                {
                    foreach (var header in model.CustomHeaders)
                    {
                        httpClient.DefaultRequestHeaders.Add(header.Key, header.Value);
                    }
                }
                if (model.Data != null)
                {                    
                    var jsonContent = model.Data;
                    message = new HttpRequestMessage();
                    message.Content = new StringContent(jsonContent.ToString(), Encoding.UTF8, "application/json");
                }
                
                if(model.Method == HttpMethod.Post)
                {
                    FormUrlEncodedContent urlParams = null;
                    //if (model.Parameters != null && model.Parameters.Count > 0)
                    //{
                    //    urlParams = new FormUrlEncodedContent(model.Parameters);
                    //}
                    response = await client.PostAsync(model.EndPoint,message.Content);
                }
                if(model.Method == HttpMethod.Get)
                {
                    response = await client.PostAsync(model.EndPoint, message.Content);
                }               
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

        public async Task<ApiResponseModel> SendAsyncStub(ApiRequestModel model)
        {
            try
            {
                HttpResponseMessage response = null;
                responseModel = new ApiResponseModel();
                responseModel.StatusCode = 200;
                responseModel.Data = ReadJsonFile();
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

        private string ReadJsonFile()
        {
            var basePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "ErxResponse.json");
            var jsonContent = File.ReadAllText(basePath);
            return jsonContent;
        }
    }
}
