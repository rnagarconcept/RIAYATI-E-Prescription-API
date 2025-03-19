using DomainModel;
using DomainModel.Models.Request;
using log4net;
using RIAYATIEPrescriptionAPI.Services;
using System;
using System.Configuration;
using System.Threading.Tasks;
using System.Web.Http;

namespace RIAYATIEPrescriptionAPI.Controllers
{
    [RoutePrefix("api/erxvalidationtransaction")]
    public class ERxValidateTransactionController : ApiController
    {
       private readonly ILog log = LogManager.GetLogger(typeof(ERxValidateTransactionController));

        [HttpGet]
        [Route("downloadtransactionfile")]
        public async Task<IHttpActionResult> DownloadTransactionFile([FromBody] DownloadTransactionRequestModel model)
        {           
            try
            {
                ErxDHAWebRequestService webService = new ErxDHAWebRequestService();
                var soapPayload = SoapEnevlopUtilService.GetPayload(GlobalConstants.DOWNLOAD_TRANSACTION_FILE_SOAP_ENEVLOP);
                var request = new ApiRequestModel();
                request.StringContent = string.Format(soapPayload, model.Login, model.Password, model.FieldId);
                request.EndPoint = "DownloadTransactionFile";
                var responseMessage = await webService.Post(request);
                return Ok(responseMessage);
            }
            catch (Exception ex)
            {
                log.Error($"Error in GetNewTransactions {ex.Message}", ex);
                return InternalServerError(new Exception(ex.Message));
            }
        }

        [HttpGet]
        [Route("getnewtransactions")]
        public async Task<IHttpActionResult> GetNewTransactions([FromBody] GeteRxTransactionsRequestModel model)
        {
            try
            {
                ErxDHAWebRequestService webService = new ErxDHAWebRequestService();
                var soapPayload = SoapEnevlopUtilService.GetPayload(GlobalConstants.GET_NEW_TRANSACTIONS_SOAP_ENEVLOP);
                var request = new ApiRequestModel();
                request.StringContent = string.Format(soapPayload, model.Login, model.Password, model.MemberID, model.ERxReferenceNumber);
                request.EndPoint = "GetNewTransactions";
                var responseMessage = await webService.Post(request);
                return Ok(responseMessage);
            }
            catch (Exception ex)
            {
                log.Error($"Error in GetNewTransactions {ex.Message}", ex);                
                return InternalServerError(new Exception(ex.Message));
            }
        }

        [HttpGet]
        [Route("geterxtransaction")]
        public async Task<IHttpActionResult> GeteRxTransaction([FromBody] GeteRxTransactionsRequestModel model)
        {
            try
            {
                ErxDHAWebRequestService webService = new ErxDHAWebRequestService();
                var soapPayload = SoapEnevlopUtilService.GetPayload(GlobalConstants.GETE_RX_TRANSACTION_SOAP_ENEVLOP);
                var request = new ApiRequestModel();
                request.StringContent = string.Format(soapPayload, model.Login, model.Password, model.MemberID, model.ERxReferenceNumber);
                request.EndPoint = "GeteRxTransaction";
                var responseMessage = await webService.Post(request);
                return Ok(responseMessage);
            }
            catch (Exception ex)
            {
                log.Error($"Error in GeteRxTransaction {ex.Message}", ex);
                return InternalServerError(new Exception(ex.Message));
            }
        }

        [HttpGet]
        [Route("searchtransactions")]
        public async Task<IHttpActionResult> SearchTransactions([FromBody] SearchTransactionRequestModel model)
        {
            try
            {
                ErxDHAWebRequestService webService = new ErxDHAWebRequestService();
                var soapPayload = SoapEnevlopUtilService.GetPayload(GlobalConstants.SEARCH_TRANSACTIONS_SOAP_ENEVLOP);
                var request = new ApiRequestModel();
                request.StringContent = string.Format(soapPayload, model.Login, model.Password, model.MemberID, model.Direction,model.CallerLicense,model.ClinicianLicense, model.MemberID,model.ERxReferenceNo,model.TransactionStatus,model.TransactionFromDate,model.TransactionToDate,model.MinRecordCount,model.MaxRecordCount);
                request.EndPoint = "SearchTransactions";
                var responseMessage = await webService.Post(request);
                return Ok(responseMessage);
            }
            catch (Exception ex)
            {
                log.Error($"Error in SearchTransactions {ex.Message}", ex);
                return InternalServerError(new Exception(ex.Message));
            }
        }

        [HttpGet]
        [Route("settransactiondownloaded")]
        public async Task<IHttpActionResult> SetTransactionDownloaded([FromBody] SetTransactionDownloadedRequestModel model)
        {
            try
            {
                ErxDHAWebRequestService webService = new ErxDHAWebRequestService();
                var soapPayload = SoapEnevlopUtilService.GetPayload(GlobalConstants.SET_TRANSACTION_DOWNLOADED_SOAP_ENEVLOP);
                var request = new ApiRequestModel();
                request.StringContent = string.Format(soapPayload, model.Login, model.Password, model.FieldID);
                request.EndPoint = "SetTransactionDownloaded";
                var responseMessage = await webService.Post(request);
                return Ok(responseMessage);
            }
            catch (Exception ex)
            {
                log.Error($"Error in SetTransactionDownloaded {ex.Message}", ex);
                return InternalServerError(new Exception(ex.Message));
            }
        }

        [HttpGet]
        [Route("uploaderxauthorization")]
        public async Task<IHttpActionResult> UploadERxAuthorization([FromBody] UploadERxAuthorizationRequestModel model)
        {
            try
            {
                ErxDHAWebRequestService webService = new ErxDHAWebRequestService();
                var soapPayload = SoapEnevlopUtilService.GetPayload(GlobalConstants.UPLOAD_ERX_AUTHORIZATION_SOAP_ENEVLOP);
                var request = new ApiRequestModel();
                request.StringContent = string.Format(soapPayload, model.Login, model.Password, model.FileContent, model.FileName);
                request.EndPoint = "UploadERxAuthorization";
                var responseMessage = await webService.Post(request);
                return Ok(responseMessage);
            }
            catch (Exception ex)
            {
                log.Error($"Error in UploadERxAuthorization {ex.Message}", ex);
                return InternalServerError(new Exception(ex.Message));
            }
        }

        [HttpGet]
        [Route("uploaderxrequest")]
        public async Task<IHttpActionResult> UploadERxRequest([FromBody] UploadERxRequestRequestModel model)
        {
            try
            {
                ErxDHAWebRequestService webService = new ErxDHAWebRequestService();
                var soapPayload = SoapEnevlopUtilService.GetPayload(GlobalConstants.UPLOAD_ERX_REQUEST_SOAP_ENEVLOP);
                var request = new ApiRequestModel();
                request.StringContent = string.Format(soapPayload, model.Login, model.Password, model.ClinicianLogin, model.ClinicianPwd, model.FileContent, model.FileName);
                request.EndPoint = "UploadERxRequest";
                var responseMessage = await webService.Post(request);
                return Ok(responseMessage);
            }
            catch (Exception ex)
            {
                log.Error($"Error in UploadERxRequest {ex.Message}", ex);
                return InternalServerError(new Exception(ex.Message));
            }
        }
    }
}
