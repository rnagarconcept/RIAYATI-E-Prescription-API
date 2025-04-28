using DomainModel.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DomainModel.Models.Request
{
    public class UploadERxAuthorizationRequestModel : ModelBase
    {
        public UploadERxAuthorizationRequestModel()
        {
            ErxAuthorization = new ErxAuthorization();
        }
        public ErxAuthorization ErxAuthorization { get; set; }
    }

    public class ErxAuthorization
    {
        public ErxAuthorization()
        {
            Header = new Header();
            Authorization = new Authorization();
        }
        public Header Header { get; set; }
        public Authorization Authorization { get; set; }
    }
}