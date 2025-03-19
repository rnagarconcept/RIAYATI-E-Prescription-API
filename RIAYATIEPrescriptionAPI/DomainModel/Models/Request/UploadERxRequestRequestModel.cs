using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DomainModel.Models.Request
{
    public class UploadERxRequestRequestModel: UploadERxAuthorizationRequestModel
    {
        public string ClinicianLogin { get; set; }
        public string ClinicianPwd { get; set; }
    }
}