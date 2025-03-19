using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DomainModel.Models.Common
{
    public class PendingRequest
    {
        public int ERX_NO { get; set; }
        public int SENDER_ID { get; set; }
        public string RECEIVER_ID { get; set; }
        public string TRANSACTION_DATE { get; set; }
        public string RECORD_COUNT { get; set; }
        public string DISPOSITION_FLAG { get; set; }
        public string STATUS { get; set; }
        public string LAST_REQ_STATUS_TIME { get; set; }
        public string LAST_RES_STATUS_TIME { get; set; }
        public string REQUESTED_BY { get; set; }
        public string ORDER_NO { get; set; }
        public string MRN { get; set; }
        public string ORDER_TYPE { get; set; }
        public string RES_CODE { get; set; }
        public string UPD_FLAG { get; set; }
    }
}