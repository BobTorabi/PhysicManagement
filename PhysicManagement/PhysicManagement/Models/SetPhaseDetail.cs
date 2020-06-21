using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PhysicManagement.Models
{
    public class SetPhaseDetail
    {
        public string PhaseId { get; set; }
        public string PhaseDetailId { get; set; }
        public string Evaluation { get; set; }
        public string PlannedDose { get; set; }
        public string CancerOARId { get; set; }
    }

}