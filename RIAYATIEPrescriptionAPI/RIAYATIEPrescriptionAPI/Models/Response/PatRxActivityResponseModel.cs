using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RIAYATIEPrescriptionAPI.Models.Response
{
    public class PatRxActivityResponseModel
    {
        public int ACT_ID { get; set; }
        public string ACT_START { get; set; }
        public string ACT_TYPE { get; set; }
        public string ACT_CODE { get; set; }
        public int QTY { get; set; }
        public string DURATION { get; set; }
        public string REFILLS { get; set; }
        public string ROUTE_OF_ADMIN { get; set; }
        public string INSTRUCTIONS { get; set; }
        public string FREQ_UNIT { get; set; }
        public string FREQ_VALUE { get; set; }
        public string FREQ_TYPE { get; set; }
    }
}