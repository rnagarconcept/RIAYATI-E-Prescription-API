using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel.Models.Common
{
   public class LicenseDetailsModel
    {
        public int ERX_NO { get; set; }
        public int SENDER_ID { get; set; }
        public int STATUS { get; set; }
        public int UPD_FLAG { get; set; }
        public string MRN { get; set; }
        public string ORDER_NO { get; set; }
        public int FACILITY_LIC_ID { get; set; }
        public string FACILITY_LIC_USER { get; set; }
        public string FACILITY_LIC_PWD { get; set; }
        public int FACILITY_LIC_STATUS { get; set; }
        public string CLINICIAN_USER { get; set; }
        public string CLINICIAN_PWD { get; set; }
        public int CLINICIAN_STATUS { get; set; }
    }
}
