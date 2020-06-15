using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PhysicManagement.Models
{
    public class MedicalRecordTreatmentPhaseVM
    {
        public long MedicalRecordId { get; set; }
        public List<Phase> Phases { get; set; } = new List<Phase>();

        public class Phase {
            public int No { get; set; }
            public int Fraction { get; set; }
            public int DeviceId { get; set; }
            public List<cancerAOR> cancerAORs { get; set; }
        }
        public class cancerAOR {
            public int Id { get; set; }
            public string Dose { get; set; }
        }
    }
}