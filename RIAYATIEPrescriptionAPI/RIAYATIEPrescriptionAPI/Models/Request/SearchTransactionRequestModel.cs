using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RIAYATIEPrescriptionAPI.Models.Request
{
    public class SearchTransactionRequestModel : ModelBase
    {
        public string Direction { get; set; }
        public string CallerLicense { get; set; }
        public string ClinicianLicense { get; set; }
        public string MemberID { get; set; }
        public string ERxReferenceNo { get; set; }
        public string TransactionStatus { get; set; }
        public string TransactionFromDate { get; set; }
        public string TransactionToDate { get; set; }
        public string MinRecordCount { get; set; }
        public string MaxRecordCount { get; set; }
    }
}