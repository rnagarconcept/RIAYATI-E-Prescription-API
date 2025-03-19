using RIAYATIEPrescriptionAPI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace RIAYATIEPrescriptionAPI.Controllers
{
    public class SoapEnevlopUtilService 
    {     
       public static string GetPayload(string key)
        {
            var allPayloads = SoapMessageEnevlopService.ReadAllMessages();
            var found = allPayloads.FirstOrDefault(x => x.Key.Equals(key, StringComparison.CurrentCultureIgnoreCase));
            if (found != null)
            {
                return found.Message;
            }
            return string.Empty;
        }
    }
}