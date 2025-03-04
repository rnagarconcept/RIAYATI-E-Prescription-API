using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RIAYATIEPrescriptionAPI.Models.Response
{
    public class PatRxObservationResponseModel
    {
        public string ERX_NO { get; set; }
        public string ACT_ID { get; set; }
        public string OBS_ID { get; set; }
        public string TYPE { get; set; }
        public string CODE { get; set; }
        public string VALUE { get; set; }
        public string VALUE_TYPE { get; set; }
    }
}