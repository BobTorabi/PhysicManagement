using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhysicManagement.Logic.ViewModels
{
  public  class PatientVMs
    {
        public class MedicalRecordDataWithPatientData {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Code{ get; set; }
            public long PatientId { get; set; }
            public long MedicalRecordId { get; set; }
            public string SystemCode { get; set; }
            public string CTCode { get; set; }
            public string MRICode { get; set; }
            public string CTDescription { get; set; }
        }
    }
}
