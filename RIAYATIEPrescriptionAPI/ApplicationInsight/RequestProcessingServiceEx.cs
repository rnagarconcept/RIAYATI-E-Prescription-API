using DataAccess;
using DomainModel.Models.Common;
using DomainModel.Models.Request;
using DomainModel.Models.Response;
using log4net;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationInsight
{   
    public class RequestProcessingServiceEx
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(RequestProcessingService));
        private static readonly Lazy<RequestProcessingServiceEx> lazy = new Lazy<RequestProcessingServiceEx>(() => new RequestProcessingServiceEx());
        private RequestProcessingServiceEx()
        {
        }
        public static RequestProcessingServiceEx GetInstance
        {
            get
            {
                return lazy.Value;
            }
        }
        public async Task Process()
        {
            try
            {
                DataTable dtPendingPat = OracleDataAccessRepositoryEx.GetInstance.GETPATPENINGERX();
                var licenseDetails = OracleDataAccessRepository.GetInstance.GET_LICENSE_DETAILS();
                if (dtPendingPat != null && dtPendingPat.Rows.Count > 0)
                {
                    for (int i = 0; i < dtPendingPat.Rows.Count; i++)
                    {
                        int eRxNo = Convert.ToInt16(dtPendingPat.Rows[i]["ERX_NO"]);
                        var pendingRequestJson = ERXGenerateXmlService.GetInstance.GetUploadErxTransactionJson(eRxNo);
                        var licenseFound = licenseDetails.FirstOrDefault(x => x.ERX_NO == eRxNo);
                        if(licenseFound != null && licenseFound.FACILITY_LIC_ID > 0)
                        {
                            var pendingRequest = new PendingRequestEx();
                            pendingRequest.PAYLOAD = pendingRequestJson;
                            pendingRequest.FACILITY_ID = licenseFound.FACILITY_LIC_ID;
                            pendingRequest.REQUEST_TYPE = "ERX-UPLOAD-TRANSACTION";
                            OracleDataAccessRepository.GetInstance.PopulatingPendingRequest(pendingRequest);
                            OracleDataAccessRepository.GetInstance.UPDATE_PAT_ERX_HEADER(eRxNo, 1);
                        }
                        else
                        {
                            log.Warn($"No License Details Foudn For ERX NO {eRxNo}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                log.Error($"Error in populating pending request json {ex.Message}");
            }
        }
    }
}
