using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel.Models.Common
{
   public class Activity
    {
        public Activity()
        {
            Frequency = new Frequency();
            Observation = new List<Observation>();
        }
        public string ID { get; set; }
        public string Type { get; set; }
        public string Code { get; set; }
        public string Quantity { get; set; }
        public string Duration { get; set; }
        public string UnitId { get; set; }
        public string Refills { get; set; }
        public string RoutOfAdmin { get; set; }
        public string Instructions { get; set; }
        public Frequency Frequency { get; set; }
        public List<Observation> Observation { get; set; }
        public string Start { get; set; }
    }
}
