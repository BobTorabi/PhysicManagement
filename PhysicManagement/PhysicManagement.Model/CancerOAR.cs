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
    
    public partial class CancerOAR
    {
        public int Id { get; set; }
        public Nullable<int> CancerId { get; set; }
        public string OrganTitle { get; set; }
        public string Tolerance { get; set; }
        public string Description { get; set; }
        public string Reserve1 { get; set; }
        public string Reserve2 { get; set; }
    
        public virtual Cancer Cancer { get; set; }
    }
}
