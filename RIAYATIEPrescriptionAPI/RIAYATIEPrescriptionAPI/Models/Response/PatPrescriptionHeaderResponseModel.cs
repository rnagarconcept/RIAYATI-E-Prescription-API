using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RIAYATIEPrescriptionAPI.Models.Response
{
    public class PatPrescriptionHeaderResponseModel
    {
        public int ERX_ID { get; set; }
        public string ERX_TYPE { get; set; }
        public int PAYER_ID { get; set; }
        public int CLINICIAN_ID { get; set; }
    }
}