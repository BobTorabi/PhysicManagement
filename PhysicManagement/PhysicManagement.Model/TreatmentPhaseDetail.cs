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
    
    public partial class TreatmentPhaseDetail
    {
        public long Id { get; set; }
        public Nullable<long> TreatmentPhaseId { get; set; }
        public Nullable<long> MedicalRecordId { get; set; }
        public string PatientFirstName { get; set; }
        public string PatientLastName { get; set; }
        public Nullable<int> CancerOARId { get; set; }
        public string CancerOARTitle { get; set; }
        public string CancerOARTolerance { get; set; }
        public Nullable<int> CancerTargetId { get; set; }
        public string CancerTargetTitle { get; set; }
        public string CancerTargetOptimum { get; set; }
        public string PrescribedDose { get; set; }
        public Nullable<System.DateTime> PrescribedDate { get; set; }
        public string PrescribedUser { get; set; }
        public Nullable<bool> PresciptionHasApproved { get; set; }
        public Nullable<bool> PhysicPlanHasAccepted { get; set; }
        public string Description { get; set; }
        public Nullable<bool> Reserved1 { get; set; }
        public Nullable<bool> Reserved2 { get; set; }
        public string Reserve3 { get; set; }
        public Nullable<bool> HadContour { get; set; }
        public string PlannedDose { get; set; }
        public string Evaluation { get; set; }
        public string PhysicUserFullName { get; set; }
        public Nullable<int> AcceptedDoctorUserId { get; set; }
        public Nullable<System.DateTime> AcceptedDoctorDate { get; set; }
        public string AcceptedDoctorFullName { get; set; }
        public string DoctorDescription { get; set; }
    }
}
