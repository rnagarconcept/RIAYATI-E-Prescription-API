﻿using DomainModel.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel.Models.Request
{
   public class UploadErxRequestTransactionRequestModel
    {
        public UploadErxRequestTransactionRequestModel()
        {
            ErxRequest = new ErxRequest();
        }

        public ErxRequest ErxRequest { get; set; }
    }
}
