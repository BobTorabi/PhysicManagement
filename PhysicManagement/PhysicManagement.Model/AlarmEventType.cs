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
    
    public partial class AlarmEventType
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public AlarmEventType()
        {
            this.AlarmConfig = new HashSet<AlarmConfig>();
        }
    
        public int Id { get; set; }
        public string Title { get; set; }
        public string EnglishTitle { get; set; }
        public string Description { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<int> TreatmentProcessStepId { get; set; }
        public string ShortTitle { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AlarmConfig> AlarmConfig { get; set; }
    }
}
