using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel.Models.Common
{
    public class Prescription
    {
        public Prescription()
        {
            Patient = new Patient();
            Encounter = new Encounter();
            Diagnosis = new List<Diagnosis>();
            Activity = new List<Activity>();
        }
        public string ID { get; set; }
        public string Type { get; set; }
        public string ReferenceNumber { get; set; }
        public string Clinician { get; set; }
        public Patient Patient { get; set; }
        public Encounter Encounter { get; set; }
        public string ReferralReferenceID { get; set; }
        public List<Diagnosis> Diagnosis { get; set; }
        public List<Activity> Activity { get; set; }
    }
}
