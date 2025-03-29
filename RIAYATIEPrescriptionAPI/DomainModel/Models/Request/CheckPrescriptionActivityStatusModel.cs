using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel.Models.Request
{
   public class CheckPrescriptionActivityStatusModel
    {
        public int TransactionId { get; set; }
        public int ActivityId { get; set; }
    }
}
