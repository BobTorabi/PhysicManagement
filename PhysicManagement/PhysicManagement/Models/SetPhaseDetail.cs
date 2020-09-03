using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PhysicManagement.Models
{
    public class SetPhaseDetail
    {
        public int PhaseId { get; set; }
        public int PhaseDetailId { get; set; }
        public string Evaluation { get; set; }
        public string PlannedDose { get; set; }
        public string CancerOARId { get; set; }
        public string TargetOARId { get; set; }
    }

}