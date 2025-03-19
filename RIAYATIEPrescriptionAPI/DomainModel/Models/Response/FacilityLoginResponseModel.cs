using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DomainModel.Models.Response
{
    public class FacilityLoginResponseModel
    {       
        public int F_LIC { get; set; }
        public string F_USER { get; set; }
        public string F_PWD { get; set; }
    }
}