using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RIAYATIEPrescriptionAPI.Models.Common
{
    public class ConfigurationSettingsModel
    {
        public string ApiBaseUrl { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string AppID { get; set; }
        public string ApiKey { get; set; }
    }
}