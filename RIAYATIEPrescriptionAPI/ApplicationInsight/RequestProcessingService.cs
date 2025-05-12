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
        protected bool debugg = string.IsNullOrEmpty(ConfigurationManager.AppSettings["DEBUGG"]) ? false : Convert.ToBoolean(ConfigurationManager.AppSettings["DEBUGG"]);
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
                                switch (req.REQUEST_TYPE.ToUpper())
                                {

                                    case "ERX-NEW-TRANSACTION":
                                        reqModel.Method = HttpMethod.Get;
                                        reqModel.ApiUrl = $"{ApiBaseURL}/ERX/GetNew";
                                        reqModel.RequestType = "ERX-NEW-TRANSACTION";
                                        if (!debugg)
                                        {
                                            response = await APIConnectService.GetInstance.SendAsync(reqModel);
                                        }
                                        else
                                        {
                                            response = await APIConnectService.GetInstance.SendAsyncStub(reqModel);
                                        }
                                        SaveTransactionResponse(req.ID, pendingRequstStatus, response);
                                        break;
                                    case "ERX-SEARCH-TRANSACTION":
                                        reqModel.Method = HttpMethod.Get;
                                        reqModel.ApiUrl = $"{ApiBaseURL}/erx/search";
                                        reqModel.RequestType = "ERX-SEARCH-TRANSACTION";
                                        response = Task.Run(() => APIConnectService.GetInstance.SendAsync(reqModel)).Result;
                                        SaveTransactionResponse(req.ID, pendingRequstStatus, response);
                                        break;

                                    case "ERX-DOWNLOAD-TRANSACTION":
                                        var erxDownloadModel = JsonConvert.DeserializeObject<DownloadTransactionRequestModel>(req.PAYLOAD);
                                        reqModel.Method = HttpMethod.Get;
                                        reqModel.ApiUrl = $"{ApiBaseURL}/erx/view?id={erxDownloadModel.Id}";
                                        reqModel.RequestType = "ERX-DOWNLOAD-TRANSACTION";
                                        if (!debugg)
                                        {
                                            response = await APIConnectService.GetInstance.SendAsync(reqModel);
                                        }
                                        else
                                        {
                                            response = await APIConnectService.GetInstance.SendAsyncStub(reqModel);
                                        }
                                        SaveTransactionResponse(req.ID, pendingRequstStatus, response);
                                        break;

                                    case "ERX_CHECK_ACTIVITY_STATUS":
                                        var activityStatusModel = JsonConvert.DeserializeObject<CheckPrescriptionActivityStatusModel>(req.PAYLOAD);
                                        reqModel.Method = HttpMethod.Get;
                                        reqModel.ApiUrl = $"{ApiBaseURL}";
                                        reqModel.EndPoint = $"ERX/CheckActivityStatus?id={activityStatusModel.TransactionId}";
                                        reqModel.RequestType = "ERX_CHECK_ACTIVITY_STATUS";
                                        if (!debugg)
                                        {
                                            response = await APIConnectService.GetInstance.SendAsync(reqModel);
                                        }
                                        else
                                        {
                                            response = await APIConnectService.GetInstance.SendAsyncStub(reqModel);
                                        }
                                        SaveTransactionResponse(req.ID, pendingRequstStatus, response);
                                        break;

                                    case "ERX-UPLOAD-TRANSACTION":
                                        var erxUploadModel = JsonConvert.DeserializeObject<UploadErxRequestTransactionRequestModel>(req.PAYLOAD);
                                        reqModel.Method = HttpMethod.Post;
                                        reqModel.Data = req.PAYLOAD;
                                        reqModel.ApiUrl = $"{ApiBaseURL}";
                                        reqModel.EndPoint = "ERX/PostRequest";
                                        reqModel.RequestType = "ERX-UPLOAD-TRANSACTION";
                                        if (!debugg)
                                        {
                                            response = await APIConnectService.GetInstance.SendAsync(reqModel);
                                        }
                                        else
                                        {
                                            response = await APIConnectService.GetInstance.SendAsyncStub(reqModel);
                                        }
                                        SaveTransactionResponse(req.ID, pendingRequstStatus, response);
                                        break;

                                    case "ERX_UPLOAD_AUTH_TRANSACTION":
                                        var authTranModel = JsonConvert.DeserializeObject<UploadERxAuthorizationRequestModel>(req.PAYLOAD);
                                        reqModel.Method = HttpMethod.Post;
                                        reqModel.Data = req.PAYLOAD;
                                        reqModel.ApiUrl = $"{ApiBaseURL}";
                                        reqModel.EndPoint = "ERX/PostAuthorization";
                                        reqModel.RequestType = "ERX_UPLOAD_AUTH_TRANSACTION";
                                        if (!debugg)
                                        {
                                            response = await APIConnectService.GetInstance.SendAsync(reqModel);
                                        }
                                        else
                                        {
                                            response = await APIConnectService.GetInstance.SendAsyncStub(reqModel);
                                        }                                        
                                        SaveTransactionResponse(req.ID, pendingRequstStatus, response);
                                        break;

                                    case "ERX-SET-TRANSACTION-DOWNLOAD":
                                        var downloadModel = JsonConvert.DeserializeObject<SetTransactionDownloadedRequestModel>(req.PAYLOAD);
                                        reqModel.Method = HttpMethod.Post;
                                        reqModel.Parameters.Add("Id", downloadModel.Id); // Dictionary type string,string                                     
                                        reqModel.ApiUrl = $"{ApiBaseURL}/";
                                        reqModel.EndPoint = $"ERX/SetDownloaded?id={downloadModel.Id}";
                                        reqModel.RequestType = "ERX-SET-TRANSACTION-DOWNLOAD";
                                        if (!debugg)
                                        {
                                            response = await APIConnectService.GetInstance.SendAsync(reqModel);
                                        }
                                        else
                                        {
                                            response = await APIConnectService.GetInstance.SendAsyncStub(reqModel);
                                        }
                                        SaveTransactionResponse(req.ID, pendingRequstStatus, response);
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

        private void SaveTransactionResponse(int requestId, PendingRequestStatus pendingRequstStatus, ApiResponseModel response)
        {
            try
            {
                pendingRequstStatus.Response = response.Data;
                pendingRequstStatus.RequestId = requestId;
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
                else
                {
                    pendingRequstStatus.Status = "ERROR";
                    pendingRequstStatus.IsProcessing = -1;
                    pendingRequstStatus.ErrorMessage = response.ErrorMessages;
                }

                OracleDataAccessRepository.GetInstance.UpdatePendingRequestStatus(pendingRequstStatus);
                if (!string.IsNullOrEmpty(response.Data))
                {
                    var tranResponse = JsonConvert.DeserializeObject<TransactionResponseModel>(response.Data);
                    OracleDataAccessRepository.GetInstance.SAVE_ERX_TRAN_RESPONSE(requestId, tranResponse);
                    OracleDataAccessRepository.GetInstance.SAVE_ERX_TRAN_RESPONSE_ERROR(tranResponse.Error);
                }
                else
                {
                    log.Info($"No response from api for request {requestId}");
                }
            }
            catch (Exception ex)
            {
                log.Error($"Error in SaveTransactionResponse for Request Id {requestId} {ex.Message}");
            }
        }
    }
}
