using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DomainModel.Models.Response
{
    public class RxHeaderResponseModel
    {
        public int SENDER_ID { get; set; }
        public int RECEIVER_ID { get; set; }
        public string TRANSACTION_DATE { get; set; }
        public int RECORD_COUNT { get; set; }
        public int DISPOSITION_FLAG { get; set; }
        public int ORDER_NO { get; set; }
        public int MRN { get; set; }
        public string ORDER_TYPE { get; set; }       
    }
}