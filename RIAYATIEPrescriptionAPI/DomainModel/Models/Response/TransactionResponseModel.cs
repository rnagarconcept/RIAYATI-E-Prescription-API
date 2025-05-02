using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel.Models.Response
{
   public class TransactionResponseModel
    {
        public TransactionResponseModel()
        {
            Error = new List<TransactionResponseErrorModel>();
        }
        public int Id { get; set; }
        public int REQ_ID { get; set; }
        public string StatusCode { get; set; }
        public string Message { get; set; }
        public string UserMessage { get; set; }
        public string MemberValidation { get; set; }
        public string DispositionFlag { get; set; }
        public string EntityID { get; set; }
        public string ReferenceNumber { get; set; }
        public string CreatedOn { get; set; }
        public List<TransactionResponseErrorModel> Error { get; set; }
    }
}
