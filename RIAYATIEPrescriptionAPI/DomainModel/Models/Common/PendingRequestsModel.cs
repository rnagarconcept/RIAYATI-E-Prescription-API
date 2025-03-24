using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DomainModel.Models.Common
{
    public class PendingRequestsModel
    {      
        public int ID { get; set; } 
        public string REQUEST_TYPE { get; set; }
        public string PAYLOAD { get; set; }
        public int FacilityId { get; set; }
        public int Status { get; set; }
    }
}