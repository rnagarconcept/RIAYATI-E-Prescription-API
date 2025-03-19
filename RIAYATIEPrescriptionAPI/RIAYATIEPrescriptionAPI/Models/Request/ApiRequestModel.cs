using RIAYATIEPrescriptionAPI.Models.Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace RIAYATIEPrescriptionAPI.Models.Request
{
    public class ApiRequestModel
    {
        public ApiRequestModel()
        {
            Settings = new SoapConfigurationSettings();
        }
        public string SoapRequestBaseUrl { get; set; } = ConfigurationManager.AppSettings["SOAP_SERVICE_BASE_URL"];
        public string SoapAction { get; set; }
        public string SoapRequestXml { get; set; }
        public string SoapEndPoint { get; set; }
        public SoapConfigurationSettings Settings { get; set; }
    }
}