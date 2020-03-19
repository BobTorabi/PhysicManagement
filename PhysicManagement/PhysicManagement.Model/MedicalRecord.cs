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
    
    public partial class MedicalRecord
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public MedicalRecord()
        {
            this.Calendar = new HashSet<Calendar>();
            this.Contour = new HashSet<Contour>();
        }
    
        public long Id { get; set; }
        public Nullable<long> PatientId { get; set; }
        public string PatientFirstName { get; set; }
        public string PatientLastName { get; set; }
        public Nullable<int> DoctorId { get; set; }
        public string DoctorFirstName { get; set; }
        public string DoctorLastName { get; set; }
        public Nullable<int> CancerId { get; set; }
        public string CancerTitle { get; set; }
        public string SystemCode { get; set; }
        public Nullable<System.DateTime> ReceptionDate { get; set; }
        public string CTCode { get; set; }
        public Nullable<System.DateTime> CTEnterDate { get; set; }
        public string MRICode { get; set; }
        public Nullable<System.DateTime> MRIEnterDate { get; set; }
        public string CTDescription { get; set; }
        public Nullable<int> TreatmentProcessId { get; set; }
        public Nullable<System.DateTime> LastTreatmentProcessChangeDate { get; set; }
        public string TPCode { get; set; }
        public Nullable<System.DateTime> TPEnterDate { get; set; }
        public Nullable<bool> NeedsFusion { get; set; }
        public string TPDescription { get; set; }
        public Nullable<System.DateTime> ContourAcceptDate { get; set; }
        public string ContourAcceptUser { get; set; }
        public Nullable<int> PhasesCount { get; set; }
        public Nullable<System.DateTime> PhasesPrescribedDate { get; set; }
        public Nullable<bool> IsOnGoing { get; set; }
        public Nullable<System.DateTime> PhysicTreatementAcceptDate { get; set; }
        public string PhysicTreatementAcceptUser { get; set; }
        public Nullable<bool> IsOnCalendar { get; set; }
        public Nullable<System.DateTime> CalendarStartDate { get; set; }
        public Nullable<System.DateTime> calendarEndDate { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Calendar> Calendar { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Contour> Contour { get; set; }
        public virtual Doctor Doctor { get; set; }
        public virtual Patient Patient { get; set; }
    }
}
