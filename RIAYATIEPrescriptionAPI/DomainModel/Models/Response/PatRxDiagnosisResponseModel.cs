using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DomainModel.Models.Response
{
    public class PatRxDiagnosisResponseModel
    {
        public string TYPE { get; set; }
        public string DIAG_TYPE { get; set; }
        public string DIAG_CODE { get; set; }
    }
}