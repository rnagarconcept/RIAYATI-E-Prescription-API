using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RIAYATIEPrescriptionAPI.Models.Request
{
    public class GeteRxTransactionRequestModel : ModelBase
    {
        public string MemberID { get; set; }
        public string ERxReferenceNo { get; set; }
    }
}