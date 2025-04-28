using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel.Models.Common
{
   public class ActivityEx
    {
        public ActivityEx()
        {            
            Observation = new List<Observation>();
        }
        public string ID { get; set; }
        public string Type { get; set; }
        public string Code { get; set; }
        public string Quantity { get; set; }
        public string Net { get; set; }
        public string List { get; set; }
        public string PatientShare { get; set; }
        public string PaymentAmount { get; set; }
        public string DenialCode { get; set; }
        public string Comments { get; set; }
        public List<Observation> Observation { get; set; }      
    }
}
