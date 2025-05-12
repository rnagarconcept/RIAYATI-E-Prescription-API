using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel.Models.Common
{
   public class PendingRequestEx
    {     
        public string REQUEST_TYPE { get; set; }
        public int FACILITY_ID { get; set; }
        public string PAYLOAD { get; set; }
        public int Status { get; set; }
    }
}
