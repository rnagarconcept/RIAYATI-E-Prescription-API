using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RIAYATIEPrescriptionAPI.Models.Request
{
    public class GeteRxTransactionsRequestModel : ModelBase
    {
        [JsonProperty("memberid")]
        public string MemberID { get; set; }

        [JsonProperty("erxreferencenumber")]
        public string ERxReferenceNumber { get; set; }
    }
}