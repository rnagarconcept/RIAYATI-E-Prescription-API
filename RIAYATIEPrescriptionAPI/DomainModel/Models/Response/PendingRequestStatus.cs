using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel.Models.Response
{
   public class PendingRequestStatus
    {
        public int RequestId { get; set; }
        public string Status { get; set; }
        public string ErrorMessage { get; set; }
        public int IsProcessing { get; set; }
        public string Response { get; set; }
    }
}
