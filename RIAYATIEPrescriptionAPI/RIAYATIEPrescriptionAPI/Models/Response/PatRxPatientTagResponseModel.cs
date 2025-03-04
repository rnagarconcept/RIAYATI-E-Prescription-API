using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RIAYATIEPrescriptionAPI.Models.Response
{
    public class PatRxPatientTagResponseModel
    {
        public int MEMBER_ID { get; set; }
        public int NATIONAL_ID { get; set; }
        public string DOB { get; set; }
        public int WEIGHT { get; set; }
        public string EMAIL { get; set; }
    }
}