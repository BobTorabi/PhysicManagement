﻿using FluentValidation;
using PhysicManagement.Logic.Validations;
using System.Collections.Generic;
using System.Linq;
using System;
using PhysicManagement.Logic.ViewModels;
using PhysicManagement.Common;
using System.Data.Entity;

namespace PhysicManagement.Logic.Services
{
    public class TreatmentService
    {
        #region TreatmentPhase Section

        public PagedList<Model.TreatmentPhase> GetTreatmentPhasesList(string firstName, string lastName, string mobile,
             string nationalCode, string systemCode, string code, int CurrentPage = 1, int pageSize = 30)
        {
            using (var db = new Model.PhysicManagementEntities())
            {
                return null;
                //IQueryable<Model.MedicalRecord> QueryableMR = db.MedicalRecord.Where(t => t.ContourAcceptDate != null).Include(x => x.Patient);
                //if (!string.IsNullOrEmpty(firstName))
                //{
                //    firstName = firstName.Trim().ToPersian();
                //    QueryableMR = QueryableMR.Where(e => e.Patient.FirstName.Contains(firstName));
                //}
                //if (!string.IsNullOrEmpty(lastName))
                //{
                //    lastName = lastName.Trim().ToPersian();
                //    QueryableMR = QueryableMR.Where(e => e.Patient.LastName.Contains(lastName));
                //}
                //if (!string.IsNullOrEmpty(mobile))
                //{
                //    mobile = mobile.Trim().toEnglishNumber();
                //    QueryableMR = QueryableMR.Where(e => e.Patient.Mobile.Contains(mobile));
                //}
                //if (!string.IsNullOrEmpty(nationalCode))
                //{
                //    nationalCode = nationalCode.Trim().toEnglishNumber();
                //    QueryableMR = QueryableMR.Where(e => e.Patient.NationalCode.Contains(nationalCode));
                //}
                //if (!string.IsNullOrEmpty(systemCode))
                //{
                //    systemCode = systemCode.Trim().toEnglishNumber();
                //    QueryableMR = QueryableMR.Where(e => e.SystemCode.Contains(systemCode));
                //}
                //if (!string.IsNullOrEmpty(code))
                //{
                //    mobile = code.Trim().toEnglishNumber();
                //    QueryableMR = QueryableMR.Where(e => e.Patient.Code.Contains(code));
                //}
                //QueryableMR = QueryableMR.OrderByDescending(x => x.ReceptionDate);
                //return new ViewModels.PagedList<Model.TreatmentPhase>()
                //{
                //    CurrentPage = CurrentPage,
                //    PageSize = pageSize,
                //    TotalRecords = QueryableMR.Count(),
                //    Records = QueryableMR.Skip((CurrentPage - 1) * pageSize).Take(pageSize).ToList()
                //};
            }
        }
        public Model.TreatmentPhase GetTreatmentPhaseById(int entityId)
        {
            using (var db = new Model.PhysicManagementEntities())
            {
                var Entity = db.TreatmentPhase.Find(entityId);
                return Entity;
            }
        }
        public bool AddTreatmentPhase(Model.TreatmentPhase entity)
        {
            var validation = new TreatmentValidation.TreatmentPhaseEntityValidate().Validate(entity);
            if (!validation.IsValid)
                throw new ValidationException(validation.Errors);

            Logic.Services.MedicalRecordService md = new MedicalRecordService();
            ////var TreatmentPhaseObject = md.GetMedicalRecordById(Convert.ToInt64(entity.MedicalRecordId.GetValueOrDefault()));
            //if (TreatmentPhaseObject == null)
            //    throw Common.MegaException.ThrowException("شناسه پرونده پزشکی وارد شده در پایگاه داده وجود ندارد.");
            //entity.MedicalRecordId = Convert.ToInt64(TreatmentPhaseObject.MRICode);

            PatientService pa = new PatientService();
            var TreatmentPhaseObject2 = pa.GetPatientById(Convert.ToInt32(entity.PrescribesdUser));
            if (TreatmentPhaseObject2 == null)
                throw Common.MegaException.ThrowException("کاربر وارد شده در پایگاه داده وجود ندارد.");
            entity.PrescribesdUser = TreatmentPhaseObject2.LastName;

            using (var db = new Model.PhysicManagementEntities())
            {
                db.TreatmentPhase.Add(entity);
                return db.SaveChanges() == 1;
            }
        }
        public bool UpdateTreatmentPhase(Model.TreatmentPhase entity)
        {
            var validation = new TreatmentValidation.TreatmentPhaseEntityValidate().Validate(entity);
            if (!validation.IsValid)
                throw new ValidationException(validation.Errors);

            using (var db = new Model.PhysicManagementEntities())
            {
                var Entity = db.TreatmentPhase.Find(entity.Id);
                if (Entity == null)
                    throw Common.MegaException.ThrowException("این رکورد در پایگاه داده پیدا نشد.");

                //Logic.Services.MedicalRecordService md = new MedicalRecordService();
                //var TreatmentPhaseObject = md.GetMedicalRecordById(Convert.ToInt64(entity.MedicalRecordId.GetValueOrDefault()));
                //if (TreatmentPhaseObject == null)
                //    throw Common.MegaException.ThrowException("شناسه پرونده پزشکی وارد شده در پایگاه داده وجود ندارد.");
                //entity.MedicalRecordId = Convert.ToInt64(TreatmentPhaseObject.MRICode);

                PatientService pa = new PatientService();
                var TreatmentPhaseObject2 = pa.GetPatientById(Convert.ToInt32(entity.PrescribesdUser));
                if (TreatmentPhaseObject2 == null)
                    throw Common.MegaException.ThrowException("کاربر وارد شده در پایگاه داده وجود ندارد.");
                entity.PrescribesdUser = TreatmentPhaseObject2.LastName;

                Entity.PhaseNumber = entity.PhaseNumber;
                Entity.PhaseText = entity.PhaseText;
                Entity.PrescribeDate = entity.PrescribeDate;
                Entity.Target = entity.Target;
                Entity.Description = entity.Description;
                Entity.Dose = entity.Dose;
                Entity.Fraction = entity.Fraction;

                return db.SaveChanges() == 1;

            }
        }
        public bool DeleteTreatmentPhase(int entityId)
        {
            using (var db = new Model.PhysicManagementEntities())
            {
                var Entity = db.TreatmentPhase.Find(entityId);
                if (Entity == null)
                    throw new ValidationException("این رکورد در پایگاه داده وجود ندارد");

                db.TreatmentPhase.Remove(Entity);
                return db.SaveChanges() == 1;
            }
        }
        public List<Model.TreatmentPhase> GetTreatmentList()
        {
            using (var db = new Model.PhysicManagementEntities())
            {
                return db.TreatmentPhase.OrderBy(x => x.Id).ToList();
            }
        }
        #endregion
        #region TreatmentProcess Section
        public List<Model.TreatmentProcess> GetTreatmentProcessList()
        {
            using (var db = new Model.PhysicManagementEntities())
            {
                return db.TreatmentProcess.OrderBy(x => x.Title).ToList();
            }
        }
        public Model.TreatmentProcess GetTreatmentProcessById(int entityId)
        {
            using (var db = new Model.PhysicManagementEntities())
            {
                var Entity = db.TreatmentProcess.Find(entityId);
                return Entity;
            }
        }
        public bool AddTreatmentProcess(Model.TreatmentProcess entity)
        {
            var validation = new TreatmentValidation.TreatmentProcessEntityValidate().Validate(entity);
            if (!validation.IsValid)
                throw new ValidationException(validation.Errors);

            using (var db = new Model.PhysicManagementEntities())
            {
                db.TreatmentProcess.Add(entity);
                return db.SaveChanges() == 1;
            }
        }
        public bool UpdateTreatmentProcess(Model.TreatmentProcess entity)
        {
            var validation = new TreatmentValidation.TreatmentProcessEntityValidate().Validate(entity);
            if (!validation.IsValid)
                throw new ValidationException(validation.Errors);

            using (var db = new Model.PhysicManagementEntities())
            {
                var Entity = db.TreatmentProcess.Find(entity.Id);
                Entity.StepNumber = entity.StepNumber;
                Entity.Title = entity.Title;
                Entity.Description = entity.Description;
                Entity.EnglishTitle = entity.EnglishTitle;

                return db.SaveChanges() == 1;
            }
        }
        public bool DeleteTreatmentProcess(int entityId)
        {
            using (var db = new Model.PhysicManagementEntities())
            {
                var Entity = db.TreatmentProcess.Find(entityId);
                if (Entity == null)
                    throw new ValidationException("این رکورد در پایگاه داده وجود ندارد");

                db.TreatmentProcess.Remove(Entity);
                return db.SaveChanges() == 1;
            }
        }
        #endregion
        #region TreatmentDevice Section
        public List<Model.TreatmentDevice> GetTreatmentDeviceList()
        {
            using (var db = new Model.PhysicManagementEntities())
            {
                return db.TreatmentDevice.OrderBy(x => x.Title).OrderBy(e => e.Code).ToList();
            }
        }
        public Model.TreatmentDevice GetTreatmentDeviceById(int entityId)
        {
            using (var db = new Model.PhysicManagementEntities())
            {
                var Entity = db.TreatmentDevice.Find(entityId);
                return Entity;
            }
        }
        public bool AddTreatmentDevice(Model.TreatmentDevice entity)
        {
            var validation = new TreatmentValidation.TreatmentDeviceEntityValidate().Validate(entity);
            if (!validation.IsValid)
                throw new ValidationException(validation.Errors);

            using (var db = new Model.PhysicManagementEntities())
            {
                db.TreatmentDevice.Add(entity);
                return db.SaveChanges() == 1;
            }
        }
        public bool UpdateTreatmentDevice(Model.TreatmentDevice entity)
        {
            var validation = new TreatmentValidation.TreatmentDeviceEntityValidate().Validate(entity);
            if (!validation.IsValid)
                throw new ValidationException(validation.Errors);

            using (var db = new Model.PhysicManagementEntities())
            {
                var Entity = db.TreatmentDevice.Find(entity.Id);
                Entity.Title = entity.Title;
                Entity.Code = entity.Code;
                Entity.Description = entity.Description;
                Entity.EnglishTitle = entity.EnglishTitle;

                return db.SaveChanges() == 1;
            }
        }
        public bool DeleteTreatmentDevice(int entityId)
        {
            using (var db = new Model.PhysicManagementEntities())
            {
                var Entity = db.TreatmentDevice.Find(entityId);
                if (Entity == null)
                    throw new ValidationException("این رکورد در پایگاه داده وجود ندارد");

                db.TreatmentDevice.Remove(Entity);
                return db.SaveChanges() == 1;
            }
        }
        #endregion


    }
}
