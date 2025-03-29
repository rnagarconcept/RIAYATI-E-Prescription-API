using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel.Models.Common
{
    public class Authorization
    {
        public string Result { get; set; }
        public string ID { get; set; }
        public string IDPayer { get; set; }
        public string DenialCode { get; set; }
        public string Start { get; set; }
        public string End { get; set; }
        public string Limit { get; set; }
        public string Comments { get; set; }
        public List<Activity> Activity { get; set; }
    }
}
