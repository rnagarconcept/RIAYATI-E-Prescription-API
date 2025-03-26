using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel.Models.Common
{
    public class Observation
    {
        public string Type { get; set; }
        public string Code { get; set; }
        public string Value { get; set; }
        public string ValueType { get; set; }
    }
}
