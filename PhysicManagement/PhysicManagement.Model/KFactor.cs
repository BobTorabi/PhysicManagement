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
    
    public partial class KFactor
    {
        public int Id { get; set; }
        public string Year { get; set; }
        public int GovernmentalFactor { get; set; }
        public int PrivateFactor { get; set; }
        public bool IsActive { get; set; }
    }
}
