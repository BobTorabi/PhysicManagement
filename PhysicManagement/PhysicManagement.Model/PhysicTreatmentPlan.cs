//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PhysicManagement.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class PhysicTreatmentPlan
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PhysicTreatmentPlan()
        {
            this.PhysicTreatmentPlanHostory = new HashSet<PhysicTreatmentPlanHostory>();
        }
    
        public int Id { get; set; }
        public Nullable<int> PhysicTreatmentId { get; set; }
        public Nullable<int> CancerOARId { get; set; }
        public string PlannedDose { get; set; }
        public string Evaluation { get; set; }
        public Nullable<bool> HadContour { get; set; }
        public Nullable<System.DateTime> ActionDate { get; set; }
        public string ActionUser { get; set; }
        public string Reserve1 { get; set; }
        public string Reserve2 { get; set; }
        public Nullable<bool> IsAcceptedByPhysic { get; set; }
        public Nullable<bool> IsAcceptedByDoctor { get; set; }
        public string AcceptedDoctorUser { get; set; }
    
        public virtual PhysicTreatment PhysicTreatment { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PhysicTreatmentPlanHostory> PhysicTreatmentPlanHostory { get; set; }
    }
}