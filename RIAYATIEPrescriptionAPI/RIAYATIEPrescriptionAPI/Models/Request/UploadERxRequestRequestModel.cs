using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RIAYATIEPrescriptionAPI.Models.Request
{
    public class UploadERxRequestRequestModel: UploadERxAuthorizationRequestModel
    {
        public string ClinicianLogin { get; set; }
        public string ClinicianPwd { get; set; }
    }
}