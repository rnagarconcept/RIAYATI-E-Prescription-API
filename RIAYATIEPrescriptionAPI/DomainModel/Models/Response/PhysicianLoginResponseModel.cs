using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DomainModel.Models.Response
{
    public class PhysicianLoginResponseModel
    {
        public int C_LIC { get; set; }
        public string C_USER { get; set; }
        public string C_PWD { get; set; }
        public string F_LIC { get; set; }
        public string F_USER { get; set; }
        public string F_PWD { get; set; }
        public int F_ID { get; set; }
    }
}