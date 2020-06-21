﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class PhysicManagementEntities : DbContext
    {
        public PhysicManagementEntities()
            : base("name=PhysicManagementEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Calendar> Calendar { get; set; }
        public virtual DbSet<Cancer> Cancer { get; set; }
        public virtual DbSet<CancerOAR> CancerOAR { get; set; }
        public virtual DbSet<Doctor> Doctor { get; set; }
        public virtual DbSet<PhysicUser> PhysicUser { get; set; }
        public virtual DbSet<Resident> Resident { get; set; }
        public virtual DbSet<TreatmentDevice> TreatmentDevice { get; set; }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<Patient> Patient { get; set; }
        public virtual DbSet<CancerTarget> CancerTarget { get; set; }
        public virtual DbSet<Contour> Contour { get; set; }
        public virtual DbSet<ContourDetails> ContourDetails { get; set; }
        public virtual DbSet<MedicalRecord> MedicalRecord { get; set; }
        public virtual DbSet<KFactor> KFactor { get; set; }
        public virtual DbSet<TreatmentCategory> TreatmentCategory { get; set; }
        public virtual DbSet<TreatmentCategoryService> TreatmentCategoryService { get; set; }
        public virtual DbSet<AlarmConfig> AlarmConfig { get; set; }
        public virtual DbSet<AlarmEventType> AlarmEventType { get; set; }
        public virtual DbSet<TreatmentProcess> TreatmentProcess { get; set; }
        public virtual DbSet<Alarm> Alarm { get; set; }
        public virtual DbSet<PhysicTreatmentPlanHistory> PhysicTreatmentPlanHistory { get; set; }
        public virtual DbSet<TreatmentPhase> TreatmentPhase { get; set; }
        public virtual DbSet<TreatmentPhaseDetail> TreatmentPhaseDetail { get; set; }
    }
}
