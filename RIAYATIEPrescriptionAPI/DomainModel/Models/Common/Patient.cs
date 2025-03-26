using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel.Models.Common
{
    public class Patient
    {
        public string MemberID { get; set; }
        public string NationalIDNumber { get; set; }
        public string DateOfBirth { get; set; }
        public string FullName { get; set; }
        public string Gender { get; set; }
        public string ContactNumber { get; set; }
        public string Weight { get; set; }
        public string Email { get; set; }
    }
}
