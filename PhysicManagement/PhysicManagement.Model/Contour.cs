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
    
    public partial class Contour
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Contour()
        {
            this.ContourDetails = new HashSet<ContourDetails>();
        }
    
        public long Id { get; set; }
        public Nullable<long> MedicalRecordId { get; set; }
        public Nullable<System.DateTime> ActionDate { get; set; }
        public string DoctorDescription { get; set; }
        public string ResidentDescription { get; set; }
        public Nullable<System.DateTime> ModifyDate { get; set; }
        public string DoctorFullName { get; set; }
        public string DoctorUserId { get; set; }
        public Nullable<bool> IsAccepted { get; set; }
    
        public virtual MedicalRecord MedicalRecord { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ContourDetails> ContourDetails { get; set; }
    }
}
