using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PhysicManagement.Models
{
    public class CountorDataForMedicalRecordVM
    {
        public long MedicalRecordId { get; set; }
        public int CancerId { get; set; }
        public List<Data> OARs { get; set; } = new List<Data>();
        public List<Data> Targets { get; set; } = new List<Data>();
        public string ResidentDescription { get; set; }
        public string DoctorDescription { get; set; }

        public class Data
        {
            public int Id { get; set; }
            public bool Value { get; set; }
            public string Note { get; set; }
        }
    }

}