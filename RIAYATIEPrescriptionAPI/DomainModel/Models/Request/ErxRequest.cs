﻿using DomainModel.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel.Models.Request
{
   public class ErxRequest
    {
        public ErxRequest()
        {
            Header = new Header();
            Prescription = new Prescription();
        }

        public Header Header { get; set; }
        public Prescription Prescription { get; set; }
    }   
}
