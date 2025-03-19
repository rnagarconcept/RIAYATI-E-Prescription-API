using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel.Models.Response
{
    public class ApiResponseModel
    {
        public string Message { get; set; }
        public string ErrorMessages { get; set; }
        public string Data { get; set; }
        public int StatusCode { get; set; }
    }
}
