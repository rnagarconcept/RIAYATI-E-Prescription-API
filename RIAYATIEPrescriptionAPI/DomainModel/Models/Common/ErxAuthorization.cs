using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel.Models.Common
{
   public class ErxAuthorization
    {
        public Header Header { get; set; }
        public Authorization Authorization { get; set; }
    }
}
