using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel.Models.Response
{
   public class TransactionResponseErrorModel
    {
        public string EntityID { get; set; }
        public string AdditionalReference { get; set; }
        public string AdditionalReferenceObjectName { get; set; }
        public string Reference { get; set; }
        public string ReferenceObjectName { get; set; }
        public string PropertyName { get; set; }
        public string RuleCode { get; set; }
        public string ErrorText { get; set; }
        public string ObjectName { get; set; }
        public string Transaction { get; set; }
    }
}
