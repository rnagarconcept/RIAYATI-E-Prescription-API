using ApplicationInsight.Services;
using DataAccess;
using DomainModel.Models.Request;
using DomainModel.Models.Response;
using log4net;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationInsight
{
    public class EndpointExpressBase
    {
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
            if (pendingRequests.Count > 0)
            {
                var facilityLicenses = OracleDataAccessRepository.GetInstance.GetFacilityDetails();
                var patErxEncounters = OracleDataAccessRepository.GetInstance.GetErxPatEncounters(0);
                var physicianCreddetails = OracleDataAccessRepository.GetInstance.GetPhysicianCredentials();
                var licenseDetails = OracleDataAccessRepository.GetInstance.GetLicenseDetails();
                foreach (var req in pendingRequests)
                {
                    var license = facilityLicenses.FirstOrDefault(x => x.F_LIC == req.PendingRequest.SENDER_ID);
                    if (license != null)
                    {
                        var patErxEncounter = patErxEncounters.FirstOrDefault(x => x.ERX_NO == req.PendingRequest.ERX_NO);
                        if(patErxEncounter != null)
                        {
                            var physicianCred = physicianCreddetails.FirstOrDefault(x => (x.F_ID == req.PendingRequest.SENDER_ID) && (x.C_LIC == patErxEncounter.CLINICIAN_ID));
                            if(physicianCred != null)
                            {
                                var requestXML = await ClsGenerateERXxml.GetInstance.GenerateXMLFile(req.PendingRequest.ERX_NO);
                                if (!string.IsNullOrEmpty(requestXML))
                                {

                                }
                            }
                            else
                            {
                                log.WarnFormat($"There is no physician credentials found for facility {req.PendingRequest.SENDER_ID} and ERX {req.PendingRequest.ERX_NO} and clinician Id {patErxEncounter.CLINICIAN_ID}");
                            }
                        }
                        else
                        {
                            log.WarnFormat($"There is no Pat Encounter found for facility {req.PendingRequest.SENDER_ID} and ERX {req.PendingRequest.ERX_NO}");
                        }
                    }
                    else
                    {
                        log.WarnFormat($"There is no facility license found for facility {req.PendingRequest.SENDER_ID}");
                    }
                  
                    //pendingRequstStatus.RequestId = req.ID;
                    //if (!string.IsNullOrEmpty(authTokenResponse.AccessToken))
                    //{
                    //    log.Info($"Start processing pending request {req.ID}");
                    //    if (!string.IsNullOrEmpty(req.PAYLOAD))
                    //    {
                    //        try
                    //        {
                    //            var reqModel = new ApiRequestModel();
                    //            var response = new ApiResponseModel();
                    //            reqModel.AuthToken = authTokenResponse.AccessToken;
                    //            reqModel.CustomHeaders.Add(new CustomHeaders { Key = "MalaffiAPIKey", Value = settings.ApiKey });
                    //            var searchQuery = "";
                    //            switch (req.REQUEST_TYPE.ToUpper())
                    //            {
                    //                case "E-PRESCRIPTION":
                    //                    reqModel.Method = HttpMethod.Get;
                    //                    var model = JsonConvert.DeserializeObject<EPrescriptionSearchRequestModel>(req.PAYLOAD);
                    //                    searchQuery = GetSearchQuery(model, searchQuery);
                    //                    reqModel.ApiUrl = $"{settings.ApiBaseUrl}/Eprescription/Integration/v1/{ServiceEndpoints.GetEPrescription}?{searchQuery}";
                    //                    reqModel.RequestType = "E-PRESCRIPTIONS";
                    //                    response = Task.Run(() => APIConnectService.GetInstance.SendAsync(reqModel)).Result;
                    //                    if (response.StatusCode == 200)
                    //                    {
                    //                        SaveEPrescriptionResponse(req.ID, response, pendingRequstStatus);
                    //                    }
                    //                    SetPendingRequest(req.REQUEST_TYPE.ToUpper(), response, pendingRequstStatus);
                    //                    break;
                    //                case "E-PRESCRIPTION-DETAILS":
                    //                    var drm = JsonConvert.DeserializeObject<EPrescriptionDetailRequestModel>(req.PAYLOAD);
                    //                    searchQuery = GetSearchQuery(drm, searchQuery);
                    //                    if (!string.IsNullOrEmpty(drm.EPrescriptionID))
                    //                    {
                    //                        searchQuery += $"&EPrescriptionID={drm.EPrescriptionID}";
                    //                    }
                    //                    reqModel.Method = HttpMethod.Get;
                    //                    reqModel.ApiUrl = $"{settings.ApiBaseUrl}/Eprescription/Integration/v1/{ServiceEndpoints.GetEPrescriptionDetail}?{searchQuery}";
                    //                    reqModel.RequestType = "E-PRESCRIPTION-DETAILS";
                    //                    response = Task.Run(() => APIConnectService.GetInstance.SendAsync(reqModel)).Result;
                    //                    if (response.StatusCode == 200)
                    //                    {
                    //                        SaveEPrescriptionDetailResponse(req.ID, Convert.ToInt32(drm.EPrescriptionID), response, pendingRequstStatus);
                    //                    }
                    //                    SetPendingRequest(req.REQUEST_TYPE.ToUpper(), response, pendingRequstStatus);
                    //                    break;

                    //                case "MANAGE-DISPENSE":
                    //                    reqModel.Method = HttpMethod.Post;
                    //                    reqModel.ApiUrl = $"{settings.ApiBaseUrl}/Eprescription/Integration/v1/{ServiceEndpoints.ManageDispense}";
                    //                    reqModel.RequestType = "MANAGE-DISPENSE";
                    //                    var obj = JsonConvert.DeserializeObject<ManageDispenseRequest>(req.PAYLOAD);
                    //                    reqModel.Data = req.PAYLOAD;
                    //                    response = Task.Run(() => APIConnectService.GetInstance.SendAsync(reqModel)).Result;
                    //                    if (response.StatusCode == 200)
                    //                    {
                    //                        SaveDispenseResponse(req.ID, obj.EPrescriptionID, response, pendingRequstStatus);
                    //                    }
                    //                    SetPendingRequest(req.REQUEST_TYPE.ToUpper(), response, pendingRequstStatus);
                    //                    break;
                    //            }
                    //        }
                    //        catch (Exception ex)
                    //        {
                    //            pendingRequstStatus.Status = "ERROR";
                    //            pendingRequstStatus.IsProcessing = -1;
                    //            pendingRequstStatus.ErrorMessage = ex.Message;
                    //            log.Error($"Error in Process Pending Request for {req.ID} - {ex.Message}", ex);
                    //        }
                    //    }
                    //    else
                    //    {
                    //        pendingRequstStatus.Status = "ERROR";
                    //        pendingRequstStatus.IsProcessing = -1;
                    //        pendingRequstStatus.ErrorMessage = "Invalid Request Payload";
                    //        log.Error($"Error in Process Pending Request for {req.ID} - Invalid Request Payload");
                    //    }
                    //}
                    //else
                    //{
                    //    pendingRequstStatus.Status = "AUTHENTICATION FALIURE";
                    //    pendingRequstStatus.IsProcessing = -3;
                    //    pendingRequstStatus.ErrorMessage = "Auto Token Not Recieved";
                    //    log.Error($"Error in Process Pending Request for {req.ID} - Auth Token can't be null or empty.");
                    //}

                    //log.Info($"Update Pending Request Payload : {JsonConvert.SerializeObject(pendingRequests)}");
                    //OracleDataAccessRepository.GetInstance.UpdatePendingRequestStatus(pendingRequstStatus);
                    //log.Info($"Completed processing pending request {req.ID}");
                }
            }
            else
            {
                log.Info($"Total pending requests are found :  {pendingRequests.Count}");
            }
        }

        //private static string GetSearchQuery(EPrescriptionSearchRequestModel model, string searchQuery)
        //{
        //    if (!string.IsNullOrEmpty(model.SearchType))
        //    {
        //        searchQuery = $"SearchType={model.SearchType}";
        //    }
        //    if (!string.IsNullOrEmpty(model.SearchText))
        //    {
        //        if (string.IsNullOrEmpty(searchQuery))
        //        {
        //            searchQuery += $"SearchText={ model.SearchText}";
        //        }
        //        else
        //        {
        //            searchQuery += $"&SearchText={ model.SearchText}";
        //        }
        //    }
        //    if (!string.IsNullOrEmpty(model.Dob))
        //    {
        //        if (string.IsNullOrEmpty(searchQuery))
        //        {
        //            searchQuery += $"DOB={ model.Dob}";
        //        }
        //        else
        //        {
        //            searchQuery += $"&DOB={ model.Dob}";
        //        }
        //    }

        //    return searchQuery;
        //}

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

        //private void SetPendingRequest(string reqType, ApiResponseModel response, PendingRequestStatus pendingRequstStatus)
        //{
        //    if (response.StatusCode == 200)
        //    {
        //        pendingRequstStatus.Response = response.Data;
        //        pendingRequstStatus.Status = "SUCCESS";
        //        pendingRequstStatus.IsProcessing = 1;
        //    }
        //    else if (response.StatusCode == 400)
        //    {
        //        pendingRequstStatus.Status = "ERROR";
        //        pendingRequstStatus.IsProcessing = -1;
        //        pendingRequstStatus.ErrorMessage = "BAD REQUEST";
        //    }
        //    else
        //    {
        //        if (reqType.ToUpper() == "MANAGE-DISPENSE")
        //        {
        //            pendingRequstStatus.Status = "COMMUNICATION ERROR";
        //            pendingRequstStatus.IsProcessing = -2;
        //            pendingRequstStatus.ErrorMessage = "An unexpected error occurred on communication channel with malaffi. We will retry again later. Please proceed with dispensing.";
        //        }
        //        else
        //        {
        //            pendingRequstStatus.Status = "COMMUNICATION ERROR";
        //            pendingRequstStatus.IsProcessing = -3;
        //            pendingRequstStatus.ErrorMessage = "An unexpected error occurred on communication channel with malaffi.";
        //        }

        //    }
        //}

    }
}
