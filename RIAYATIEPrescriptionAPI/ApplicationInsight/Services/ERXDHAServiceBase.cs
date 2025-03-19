using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace ApplicationInsight.Services
{
    public abstract class ERXDHAServiceBase
    {
        private readonly string soapEndpoint = ConfigurationManager.AppSettings["SOAP_ENDPOINT"];
        private readonly string soapUploadAction = "http://tempuri.org/MyMethod"; // SOAP action (from WSDL)
       
    }
}