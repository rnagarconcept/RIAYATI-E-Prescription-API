using DataAccess;
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
    public class EndpointExpressBase
    {
        protected string ApiBaseURL = ConfigurationManager.AppSettings["API_BASE_URL"];
    }
    public class RequestProcessingService : EndpointExpressBase
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(RequestProcessingService));
        private static readonly Lazy<RequestProcessingService> lazy = new Lazy<RequestProcessingService>(() => new RequestProcessingService());
        private RequestProcessingService()
        {
        }
        public static RequestProcessingService GetInstance
        {
            get
            {
                return lazy.Value;
            }
        }
        public async Task Process()
        {           
            var pendingRequests = OracleDataAccessRepository.GetInstance.GetPendingRequests();
            var facilityLicense = OracleDataAccessRepository.GetInstance.GetFacilityLicenseDetails();
            if (pendingRequests.Count > 0)           
            {
                foreach (var req in pendingRequests)
                {
                    PendingRequestStatus pendingRequstStatus = new PendingRequestStatus();
                    var reqModel = new ApiRequestModel();
                    pendingRequstStatus.RequestId = req.ID;
                    var licenseDetail = facilityLicense.FirstOrDefault(x => x.F_LIC == req.FacilityId);
                    pendingRequstStatus.RequestId = req.ID;
                    if (licenseDetail != null)
                    {
                        log.Info($"Start processing pending request {req.ID}");
                        if (!string.IsNullOrEmpty(req.PAYLOAD))
                        {
                            try
                            {                              
                                var response = new ApiResponseModel();
                                reqModel.Auth = new DomainModel.Models.Common.AuthDetail { F_LIC = licenseDetail.F_LIC, F_USER = licenseDetail.F_USER, F_PWD = licenseDetail.F_PWD };
                                var searchQuery = "";
                                switch (req.REQUEST_TYPE.ToUpper())
                                {
                                    case "ERX-NEW-TRANSACTION":
                                        reqModel.Method = HttpMethod.Get;                                        
                                        reqModel.ApiUrl = $"{ApiBaseURL}/ERX/GetNew";
                                        reqModel.RequestType = "ERX-NEW-TRANSACTION";
                                        response = Task.Run(() => APIConnectService.GetInstance.SendAsync(reqModel)).Result;
                                        if (response.StatusCode == 200)
                                        {                                           
                                        }
                                        SetPendingRequest(req.REQUEST_TYPE.ToUpper(), response, pendingRequstStatus);
                                        break;
                                    case "ERX-SEARCH-TRANSACTION":                                       
                                        reqModel.Method = HttpMethod.Get;
                                        reqModel.ApiUrl = $"{ApiBaseURL}/erx/search";
                                        reqModel.RequestType = "ERX-SEARCH-TRANSACTION";
                                        response = Task.Run(() => APIConnectService.GetInstance.SendAsync(reqModel)).Result;
                                        if (response.StatusCode == 200)
                                        {                                            
                                        }
                                        SetPendingRequest(req.REQUEST_TYPE.ToUpper(), response, pendingRequstStatus);
                                        break;

                                    case "ERX-DOWNLOAD-TRANSACTION":
                                        reqModel.Method = HttpMethod.Post;
                                        reqModel.ApiUrl = $"{ApiBaseURL}/erx/view";
                                        reqModel.RequestType = "ERX-DOWNLOAD-TRANSACTION";                                        
                                        response = await APIConnectService.GetInstance.SendAsync(reqModel);
                                        if (response.StatusCode == 200)
                                        {                                           
                                        }
                                        SetPendingRequest(req.REQUEST_TYPE.ToUpper(), response, pendingRequstStatus);
                                        break;
                                }
                            }
                            catch (Exception ex)
                            {
                                pendingRequstStatus.Status = "ERROR";
                                pendingRequstStatus.IsProcessing = -1;
                                pendingRequstStatus.ErrorMessage = ex.Message;
                                log.Error($"Error in Process Pending Request for {req.ID} - {ex.Message}", ex);
                            }
                        }
                        else
                        {
                            pendingRequstStatus.Status = "ERROR";
                            pendingRequstStatus.IsProcessing = -1;
                            pendingRequstStatus.ErrorMessage = "Invalid Request Payload";
                            log.Error($"Error in Process Pending Request for {req.ID} - Invalid Request Payload");
                        }
                    }
                    else
                    {
                        pendingRequstStatus.Status = "AUTHENTICATION FALIURE";
                        pendingRequstStatus.IsProcessing = -3;
                        pendingRequstStatus.ErrorMessage = "Auto Token Not Recieved";
                        log.Error($"Error in Process Pending Request for {req.ID} - Auth Token can't be null or empty.");
                    }
                    
                    OracleDataAccessRepository.GetInstance.UpdatePendingRequestStatus(pendingRequstStatus);
                    log.Info($"Completed processing pending request {req.ID}");
                }
            }
            else
            {
                log.Info($"Total pending requests are found :  {pendingRequests.Count}");
            }
        }


        //private void SaveEPrescriptionResponse(int requestId, ApiResponseModel response, PendingRequestStatus pendingRequstStatus)
        //{
        //    try
        //    {

        //        var isValidResponse = IsValidResponse(response.Data, pendingRequstStatus);
        //        if (response.StatusCode == 200 && isValidResponse)
        //        {
        //            pendingRequstStatus.IsProcessing = 1;
        //            pendingRequstStatus.Status = "Success";
        //            var result = JsonConvert.DeserializeObject<List<EPrescriptionsResponseModel>>(response.Data);
        //            //var model = result.FirstOrDefault();
        //            foreach (var model in result)
        //            {
        //                OracleDataAccessRepository.GetInstance.SavePrescriptionResponse(requestId, model);
        //            }
        //        }
        //        else
        //        {
        //            pendingRequstStatus.IsProcessing = -1;
        //            pendingRequstStatus.Status = "Failed";
        //            pendingRequstStatus.ErrorMessage = string.IsNullOrEmpty(pendingRequstStatus.ErrorMessage) ? response.Message : pendingRequstStatus.ErrorMessage;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        pendingRequstStatus.IsProcessing = -1;
        //        pendingRequstStatus.Status = "Failed";
        //        pendingRequstStatus.ErrorMessage = ex.Message;
        //    }
        //}
        //private void SaveEPrescriptionDetailResponse(int requestId, int ePrescriptionId, ApiResponseModel response, PendingRequestStatus pendingRequstStatus)
        //{
        //    try
        //    {
        //        var isValidResponse = IsValidResponse(response.Data, pendingRequstStatus);
        //        if (response.StatusCode == 200 && isValidResponse)
        //        {
        //            pendingRequstStatus.IsProcessing = 1;
        //            pendingRequstStatus.Status = "Success";
        //            var result = JsonConvert.DeserializeObject<EPrescriptionDetailsResponseModel>(response.Data);
        //            if (result.EPrescriptionID > 0)
        //            {
        //                OracleDataAccessRepository.GetInstance.SavePrescriptionDetailsResponse(requestId, ePrescriptionId, result);
        //            }
        //            else
        //            {
        //                pendingRequstStatus.IsProcessing = -1;
        //                pendingRequstStatus.Status = "Failed";
        //                pendingRequstStatus.ErrorMessage = "Invalid Response";
        //            }
        //        }
        //        else
        //        {
        //            pendingRequstStatus.IsProcessing = -1;
        //            pendingRequstStatus.Status = "Failed";
        //            pendingRequstStatus.ErrorMessage = string.IsNullOrEmpty(pendingRequstStatus.ErrorMessage) ? response.Message : pendingRequstStatus.ErrorMessage;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        pendingRequstStatus.IsProcessing = -1;
        //        pendingRequstStatus.Status = "Failed";
        //        pendingRequstStatus.ErrorMessage = ex.Message;
        //        log.Error($"Error SaveEPrescriptionDetailResponse Request Id {requestId} Response : {response.Data.ToString()}");
        //    }
        //}

        //private void SaveDispenseResponse(int requestId, string prescriptionId, ApiResponseModel response, PendingRequestStatus pendingRequstStatus)
        //{
        //    try
        //    {
        //        var isValidResponse = IsValidResponse(response.Data, pendingRequstStatus);
        //        if (response.StatusCode == 200 && isValidResponse)
        //        {
        //            pendingRequstStatus.IsProcessing = 1;
        //            pendingRequstStatus.Status = "Success";
        //            var result = JsonConvert.DeserializeObject<List<DispenseResponseModel>>(response.Data);
        //            var eprescriptionId = string.IsNullOrEmpty(prescriptionId) ? 0 : int.Parse(prescriptionId);
        //            OracleDataAccessRepository.GetInstance.SaveDispenseResponse(requestId, eprescriptionId, result);
        //        }
        //        else
        //        {
        //            pendingRequstStatus.IsProcessing = -1;
        //            pendingRequstStatus.Status = "Failed";
        //            if (response != null)
        //            {
        //                pendingRequstStatus.ErrorMessage = string.IsNullOrEmpty(pendingRequstStatus.ErrorMessage) ? response.Message : pendingRequstStatus.ErrorMessage;
        //            }
        //            pendingRequstStatus.ErrorMessage = "Invalid response recieved";
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        pendingRequstStatus.IsProcessing = -1;
        //        pendingRequstStatus.Status = "Failed";
        //        pendingRequstStatus.ErrorMessage = ex.Message;
        //        log.Error($"Error SaveDispenseResponse Request Id {requestId} Response : {response.Data.ToString()}");
        //    }
        //}

        //public bool IsValidResponse(string response, PendingRequestStatus pendingRequstStatus)
        //{
        //    try
        //    {
        //        JObject jsonObject = JObject.Parse(response);
        //        bool responseType = jsonObject.ContainsKey("Type");
        //        dynamic filedResponse = JsonConvert.DeserializeObject(response);
        //        if (responseType)
        //        {
        //            log.Info($"Error response Response Type {responseType} Message {filedResponse.Message.ToString()}");
        //            pendingRequstStatus.IsProcessing = -1;
        //            pendingRequstStatus.Status = filedResponse.Type.ToString();
        //            pendingRequstStatus.ErrorMessage = filedResponse.Message.ToString();
        //            return false;
        //        }
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Error($"Error IsValidResponse Request Id {pendingRequstStatus.RequestId} Response : {response}");
        //        return true;
        //    }
        //}

        private void SetPendingRequest(string reqType, ApiResponseModel response, PendingRequestStatus pendingRequstStatus)
        {
            if (response.StatusCode == 200)
            {
                pendingRequstStatus.Response = response.Data;
                pendingRequstStatus.Status = "SUCCESS";
                pendingRequstStatus.IsProcessing = 1;
            }
            else if (response.StatusCode == 400)
            {
                pendingRequstStatus.Status = "ERROR";
                pendingRequstStatus.IsProcessing = -1;
                pendingRequstStatus.ErrorMessage = "BAD REQUEST";
            }          
        }
    }
}
