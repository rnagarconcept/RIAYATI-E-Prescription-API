using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DomainModel.Models.Common
{
    public class PendingRequestsModel
    {
        public PendingRequestsModel()
        {
            PendingRequest = new PendingRequest();
        }
        public int ID { get; set; }
        public int SEARCH_TYPE { get; set; }
        public string SEARCH_TEXT { get; set; }
        public string DOB { get; set; }
        public string REQUEST_TYPE { get; set; }
        public string PAYLOAD { get; set; }
        public string EPrescriptionID { get; set; }
        public PendingRequest PendingRequest { get; set; }
    }
}