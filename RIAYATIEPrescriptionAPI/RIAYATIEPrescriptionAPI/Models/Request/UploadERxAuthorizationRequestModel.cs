using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RIAYATIEPrescriptionAPI.Models.Request
{
    public class UploadERxAuthorizationRequestModel : ModelBase
    {
        public string FileContent { get; set; }
        public string FileName { get; set; }
    }
}