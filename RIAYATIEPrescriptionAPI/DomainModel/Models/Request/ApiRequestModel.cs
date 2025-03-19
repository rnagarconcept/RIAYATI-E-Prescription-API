using DomainModel.Models.Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Web;

namespace DomainModel.Models.Request
{
    public class ApiRequestModel
    {
        public ApiRequestModel()
        {
            CustomHeaders = new List<CustomHeaders>();
        }
        public HttpMethod Method { get; set; }
        public object Data { get; set; }
        public string StringContent { get; set; }
        public string AuthToken { get; set; }
        public string ApiUrl { get; set; }
        public string EndPoint { get; set; }
        public string RequestType { get; set; }
        public List<CustomHeaders> CustomHeaders { get; set; }
        public SoapConfigurationSettings SoapConfigurationSettings { get; set; }
    }

    public class CustomHeaders
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
