using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel.Models.Common
{
    public class Header
    {
        public string SenderID { get; set; }
        public string ReceiverID { get; set; }
        public string TransactionDate { get; set; }
        public string RecordCount { get; set; }
        public string DispositionFlag { get; set; }
        public string PayerID { get; set; }
    }
}
