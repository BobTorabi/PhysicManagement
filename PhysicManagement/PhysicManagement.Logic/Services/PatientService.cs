﻿using FluentValidation;
using PhysicManagement.Logic.Validations;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Data.Entity;
using PhysicManagement.Model;
using PhysicManagement.Common;
using PhysicManagement.Logic.ViewModels;

namespace PhysicManagement.Logic.Services
{
    public class PatientService
    {
        #region Patient section
        public List<Model.Patient> GetPatientList()
        {
            using (var db = new Model.PhysicManagementEntities())
            {
                return db.Patient.OrderBy(x => x.FirstName).ToList();
            }
        }
        public PagedList<Model.MedicalRecord> GetPatientListDontHaveMriOrCTScan(string firstName,string lastName,string mobile,
            string nationalCode,string systemCode,string code,int CurrentPage = 1, int pageSize = 30)
        {
            using (var db = new Model.PhysicManagementEntities())
            {
                IQueryable<Model.MedicalRecord> QueryableMR = db.MedicalRecord.Where(t => t.CTEnterDate == null && t.MRIEnterDate == null).Include(x => x.Patient);
                if (!string.IsNullOrEmpty(firstName))
                {
                    firstName = firstName.Trim().ToPersian();
                    QueryableMR = QueryableMR.Where(e => e.Patient.FirstName.Contains(firstName));
                }
                if (!string.IsNullOrEmpty(lastName))
                {
                    lastName = lastName.Trim().ToPersian();
                    QueryableMR = QueryableMR.Where(e => e.Patient.LastName.Contains(lastName));
                }
                if (!string.IsNullOrEmpty(mobile))
                {
                    mobile = mobile.Trim().toEnglishNumber();
                    QueryableMR = QueryableMR.Where(e => e.Patient.Mobile.Contains(mobile));
                }
                if (!string.IsNullOrEmpty(nationalCode))
                {
                    nationalCode = nationalCode.Trim().toEnglishNumber();
                    QueryableMR = QueryableMR.Where(e => e.Patient.NationalCode.Contains(nationalCode));
                }
                if (!string.IsNullOrEmpty(systemCode))
                {
                    systemCode = systemCode.Trim().toEnglishNumber();
                    QueryableMR = QueryableMR.Where(e => e.SystemCode.Contains(systemCode));
                }
                if (!string.IsNullOrEmpty(code))
                {
                    mobile = code.Trim().toEnglishNumber();
                    QueryableMR = QueryableMR.Where(e => e.Patient.Code.Contains(code));
                }
                QueryableMR = QueryableMR.OrderByDescending(x => x.ReceptionDate);
                return new ViewModels.PagedList<Model.MedicalRecord>()
                {
                    CurrentPage = CurrentPage,
                    PageSize = pageSize,
                    TotalRecords = QueryableMR.Count(),
                    Records = QueryableMR.Skip((CurrentPage - 1) * pageSize).Take(pageSize).ToList()
                };
                //var medicalIds = db.MedicalRecord.Where(t => t.CTEnterDate == null && t.MRIEnterDate == null).Select(x=>x.Id).ToList();
               
                //return db.Patient
                //    .Where(x => x.MedicalRecord.Any(t => medicalIds.Contains(t.Id) ))
                //    .Include(x => x.MedicalRecord).OrderByDescending(x => x.RegisterDate).ToList();
            }
        }

        public ViewModels.DaysStatisticsVM GetTotalPatientsStatistics()
        {
            using (var db = new Model.PhysicManagementEntities())
            {
                DateTime TodayStart = DateTime.Now.Date;
                DateTime TodayEnd = TodayStart.AddDays(1).AddSeconds(-1);
                DateTime LastWeekStart = TodayStart.AddDays(-7);
                DateTime LastMonthStart = TodayStart.AddMonths(-1);
                return new ViewModels.DaysStatisticsVM
                {
                    Today = db.Patient.Count(e => e.RegisterDate >= TodayStart && e.RegisterDate <= TodayEnd),
                    LastWeek = db.Patient.Count(e => e.RegisterDate >= LastWeekStart && e.RegisterDate <= TodayEnd),
                    LastMonth = db.Patient.Count(e => e.RegisterDate >= LastMonthStart && e.RegisterDate <= TodayEnd),
                    TotalRecords = db.Patient.Count()
                };
            }
        }

        public dynamic GetPatientsReceptionStatistics()
        {
            using (var db = new Model.PhysicManagementEntities())
            {

                DateTime TodayStart = DateTime.Now.Date;
                DateTime TodayEnd = TodayStart.AddDays(1).AddSeconds(-1);

                DateTime YesterDayStart = TodayStart.AddDays(-1);
                DateTime YesterDayEnd = YesterDayStart.AddDays(1).AddSeconds(-1);

                DateTime LastWeekStart = TodayStart.AddDays(-7);
                DateTime LastMonthStart = TodayStart.AddMonths(-1);

                int TotalReceptionForToday = db.Patient.Count(e => e.RegisterDate >= TodayStart && e.RegisterDate <= TodayEnd);
                int TotalReceptionForYesterDay = db.Patient.Count(e => e.RegisterDate >= YesterDayStart && e.RegisterDate <= YesterDayEnd);
                decimal TodayScale = 0;
                if (TotalReceptionForYesterDay!= 0)
                {
                    TodayScale = ((decimal)TotalReceptionForToday - (decimal)TotalReceptionForYesterDay) / (decimal)TotalReceptionForYesterDay * 100;
                }

                return new ViewModels.DaysStatisticsVM
                {
                    Today = TodayScale,
                };
            }
        }

        public List<Model.MedicalRecord> GetPatientListWithUnsetFusion(string firstName, string lastName, string nationalCode, string mobile, string systemCode, string code)
        {
            using (var db = new Model.PhysicManagementEntities())
            {
                IQueryable<MedicalRecord> Queryable = db.MedicalRecord.Where(t => t.NeedsFusion == null);
                if (!string.IsNullOrEmpty(firstName))
                {
                    firstName = firstName.Trim().ToPersian();
                    Queryable = Queryable.Where(x => x.Patient.FirstName == firstName);
                }
                if (!string.IsNullOrEmpty(lastName))
                {
                    lastName = lastName.Trim().ToPersian();
                    Queryable = Queryable.Where(x => x.Patient.LastName == lastName);
                }
                if (!string.IsNullOrEmpty(nationalCode))
                {
                    nationalCode = nationalCode.Trim().toEnglishNumber();
                    Queryable = Queryable.Where(x => x.Patient.NationalCode == nationalCode);
                }
                if (!string.IsNullOrEmpty(mobile))
                {
                    mobile = mobile.Trim().toEnglishNumber();
                    Queryable = Queryable.Where(x => x.Patient.Mobile == mobile);
                }
                if (!string.IsNullOrEmpty(systemCode))
                {
                    systemCode = systemCode.Trim().toEnglishNumber();
                    Queryable = Queryable.Where(x => x.SystemCode == systemCode);
                }
                if (!string.IsNullOrEmpty(code))
                {
                    code = code.Trim().toEnglishNumber();
                    Queryable = Queryable.Where(x => x.Patient.Code == code);
                }
                return Queryable.Include(x => x.Patient).OrderByDescending(x => x.SystemCode).ToList();
            }
        }
        public List<Model.MedicalRecord> GetPatientListWithUnsetCountor(string firstName, string lastName, string nationalCode, string mobile, string systemCode, string code, bool? hasContour)
        {
            using (var db = new Model.PhysicManagementEntities())
            {
                IQueryable<MedicalRecord> Queryable = db.MedicalRecord;
                if (hasContour.HasValue)
                {
                    bool HasContour = hasContour.Value;
                    if (HasContour)
                    {
                        Queryable = Queryable.Where(x => x.Contour.Count > 0);
                    }
                    else
                    {
                        Queryable = Queryable.Where(x => x.Contour.Count == 0);
                    }
                }
                if (!string.IsNullOrEmpty(firstName))
                {
                    firstName = firstName.Trim().ToPersian();
                    Queryable = Queryable.Where(x => x.Patient.FirstName == firstName);
                }
                if (!string.IsNullOrEmpty(lastName))
                {
                    lastName = lastName.Trim().ToPersian();
                    Queryable = Queryable.Where(x => x.Patient.LastName == lastName);
                }
                if (!string.IsNullOrEmpty(nationalCode))
                {
                    nationalCode = nationalCode.Trim().toEnglishNumber();
                    Queryable = Queryable.Where(x => x.Patient.NationalCode == nationalCode);
                }
                if (!string.IsNullOrEmpty(mobile))
                {
                    mobile = mobile.Trim().toEnglishNumber();
                    Queryable = Queryable.Where(x => x.Patient.Mobile == mobile);
                }
                if (!string.IsNullOrEmpty(systemCode))
                {
                    systemCode = systemCode.Trim().toEnglishNumber();
                    Queryable = Queryable.Where(x => x.SystemCode == systemCode);
                }
                if (!string.IsNullOrEmpty(code))
                {
                    code = code.Trim().toEnglishNumber();
                    Queryable = Queryable.Where(x => x.Patient.Code == code);
                }
                return Queryable.Include("Patient").Include("Contour").OrderByDescending(x => x.SystemCode).ToList();
            }
        }


        public bool SetPatientMediacalRecordCTScanData(int medicalRecordId, string CTScanCode, string CTScanDescription, string MRICode)
        {
            using (var db = new Model.PhysicManagementEntities())
            {
                try
                {
                    var MedicalRecordData = db.MedicalRecord.FirstOrDefault(x => x.Id == medicalRecordId);
                    MedicalRecordData.MRIEnterDate = DateTime.Now;
                    MedicalRecordData.CTEnterDate = DateTime.Now;
                    MedicalRecordData.CTCode = CTScanCode;
                    MedicalRecordData.CTDescription = CTScanDescription;
                    MedicalRecordData.MRICode = MRICode;
                    MedicalRecordData.IsOnGoing = true;
                    MedicalRecordData.IsOnCalendar = false;
                    db.SaveChanges();
                    return true;
                }
                catch
                {
                    return false;
                }
            }

        }
        public bool SetPatientMediacalRecordCPAndFusion(int medicalRecordId, string TPDescription, bool needFusion)
        {
            using (var db = new Model.PhysicManagementEntities())
            {
                try
                {
                    var MedicalRecordData = db.MedicalRecord.FirstOrDefault(x => x.Id == medicalRecordId);
                    MedicalRecordData.NeedsFusion = needFusion;
                    MedicalRecordData.TPDescription = TPDescription;
                    MedicalRecordData.TPCode = TPDescription;
                    MedicalRecordData.TPEnterDate = DateTime.Now;
                    db.SaveChanges();
                    return true;
                }
                catch
                {
                    return false;
                }
            }

        }
        public ViewModels.PagedList<Model.Patient> GetPatientListWithFilters(string firstName, string lastName, string mobile,
            string nationalCode, string systemCode, string code, int CurrentPage = 1, int pageSize = 30)
        {

            using (var db = new Model.PhysicManagementEntities())
            {
                
                IQueryable<Model.Patient> QueryablePatient = db.Patient.Include(x => x.MedicalRecord);

                if (!string.IsNullOrEmpty(firstName))
                {
                    firstName = firstName.Trim().ToPersian();
                    QueryablePatient = QueryablePatient.Where(x => x.FirstName.Contains(firstName));
                }
                if (!string.IsNullOrEmpty(lastName))
                {
                    lastName = lastName.Trim().ToPersian();
                    QueryablePatient = QueryablePatient.Where(x => x.LastName.Contains(lastName));
                }
                if (!string.IsNullOrEmpty(mobile))
                {
                    mobile = mobile.Trim().toEnglishNumber();
                    QueryablePatient = QueryablePatient.Where(x => x.Mobile.Contains(mobile));
                }
                if (!string.IsNullOrEmpty(code))
                {
                    code = code.Trim().toEnglishNumber();
                    QueryablePatient = QueryablePatient.Where(x => x.Code.Contains(code));
                }
                if (!string.IsNullOrEmpty(nationalCode))
                {
                    nationalCode = nationalCode.Trim().toEnglishNumber();
                    QueryablePatient = QueryablePatient.Where(x => x.NationalCode.Contains(nationalCode));
                }
                if (!string.IsNullOrEmpty(systemCode))
                {
                    systemCode = systemCode.Trim().toEnglishNumber();
                    QueryablePatient = QueryablePatient.Where(x => x.MedicalRecord.Any(z=>z.SystemCode == systemCode));
                }
                QueryablePatient = QueryablePatient.OrderByDescending(x => x.RegisterDate);
                return new ViewModels.PagedList<Model.Patient>()
                {
                    CurrentPage = CurrentPage,
                    PageSize = pageSize,
                    TotalRecords = QueryablePatient.Count(),
                    Records = QueryablePatient.Skip((CurrentPage - 1) * pageSize).Take(pageSize).ToList()
                };
            }
        }
        public Model.Patient GetPatientById(long entityId)
        {
            using (var db = new Model.PhysicManagementEntities())
            {
                var Entity = db.Patient.Where(x => x.Id == entityId).Include(x => x.MedicalRecord).FirstOrDefault();
                return Entity;
            }
        }
        public Model.Patient GetPatientByNationalCode(string nationalCode)
        {
            using (var db = new Model.PhysicManagementEntities())
            {
                var Entity = db.Patient.Where(x => x.NationalCode == nationalCode).FirstOrDefault();
                return Entity;
            }
        }
        public Model.Patient GetPatientByName(string lastName)
        {
            using (var db = new Model.PhysicManagementEntities())
            {
                var Entity = db.Patient.Where(x => x.LastName == lastName).FirstOrDefault();
                return Entity;
            }
        }

        public Model.Patient GetPatientByCode(string code)
        {
            using (var db = new Model.PhysicManagementEntities())
            {
                var Entity = db.Patient.Where(x => x.Code == code).FirstOrDefault();
                return Entity;
            }
        }
        public ViewModels.PatientVMs.MedicalRecordDataWithPatientData GetPatientByMedicalRecordId(long medicalRecordId)
        {
            using (var db = new Model.PhysicManagementEntities())
            {
                var Entity = db.MedicalRecord.Where(x => x.Id == medicalRecordId).Include(x => x.Patient).FirstOrDefault();
                return new ViewModels.PatientVMs.MedicalRecordDataWithPatientData()
                {
                    CTDescription = Entity.CTDescription,
                    Code = Entity.Patient.Code,
                    CTCode = Entity.CTCode,
                    SystemCode = Entity.SystemCode,
                    FirstName = Entity.Patient.FirstName,
                    LastName = Entity.Patient.LastName,
                    MedicalRecordId = Entity.Id,
                    MRICode = Entity.MRICode,
                    PatientId = Entity.PatientId
                };
            }
        }
        public Model.Patient GetPatientByDoctorName(string lastname)
        {
            using (var db = new Model.PhysicManagementEntities())
            {
                var entity = db.MedicalRecord.Where(x => x.DoctorLastName == lastname).FirstOrDefault();

                var Entity = Convert.ToInt32(entity.PatientId);
                var patient = GetPatientById(Entity);
                return patient;
            }
        }

        public bool AddPatient(Model.Patient entity)
        {
            var validation = new PatientValidation.PatientEntityValidate().Validate(entity);
            if (!validation.IsValid)
                throw new ValidationException(validation.Errors);

            using (var db = new Model.PhysicManagementEntities())
            {
                db.Patient.Add(entity);
                return db.SaveChanges() == 1;
            }
        }
        public bool UpdatePatient(Model.Patient entity)
        {
            var validation = new PatientValidation.PatientEntityValidate().Validate(entity);
            if (!validation.IsValid)
                throw new ValidationException(validation.Errors);

            using (var db = new Model.PhysicManagementEntities())
            {
                var Entity = db.Patient.Find(entity.Id);
                Entity.FirstName = entity.FirstName;
                Entity.LastName = entity.LastName;
                Entity.Mobile = entity.Mobile;
                Entity.NationalCode = entity.NationalCode;
                Entity.Code = entity.Code;
                Entity.Province = entity.Province;
                Entity.City = entity.City;
                Entity.Address = entity.Address;
                Entity.RegisterDate = entity.RegisterDate;
                Entity.GenderIsMale = entity.GenderIsMale;

                return db.SaveChanges() == 1;
            }
        }
        public bool DeletePatient(int entityId)
        {
            using (var db = new Model.PhysicManagementEntities())
            {
                var Entity = db.Patient.Find(entityId);
                if (Entity == null)
                    throw new ValidationException("این رکورد در پایگاه داده وجود ندارد");

                db.Patient.Remove(Entity);
                return db.SaveChanges() == 1;

            }
        }
        private Object thisLock = new Object();

        /// <summary>
        /// این متد برای به دست آوردن شماره ی بعدی شناسه یکتای بیمار استفاده می گردد
        /// </summary>
        /// <returns></returns>
        public string GetNewPatientCodeToRegister()
        {
            lock (thisLock)
            {
                using (var db = new Model.PhysicManagementEntities())
                {
                    var PatientNo = db.Patient.Max(x => x.Code);
                    if (string.IsNullOrEmpty(PatientNo))
                    {
                        return "1000";
                    }
                    else
                    {
                        return (int.Parse(PatientNo) + 1).ToString();
                    }
                }
            }
        }

        public Model.Patient RegisterPatient(string patientFirstName, string patientLastName, string nationalCode, int doctorId, string mobile, string description)
        {
            // بررسی وجود بیمار با استفاده از کدملی
            var PatientObject = GetPatientByNationalCode(nationalCode);
            // چنین بیماری از قبل وجو نداشته و فرایند ثبت جدید باید انجام شود
            if (PatientObject == null)
            {
                bool IsAffected = AddPatient(new Model.Patient
                {
                    Address = "",
                    Code = GetNewPatientCodeToRegister(),
                    City = "",
                    FirstName = patientFirstName,
                    GenderIsMale = null,
                    LastName = patientLastName,
                    Mobile = mobile,
                    NationalCode = nationalCode,
                    Province = "",
                    RegisterDate = System.DateTime.Now,


                });

                if (!IsAffected)
                    throw Common.MegaException.ThrowException("امکان ثبت این کاربر وجود ندارد.لطفا با واحد فنی تماس بگیرید.");
            }
            PatientObject = GetPatientByNationalCode(nationalCode);

            Model.Doctor DoctorObject = new DoctorService().GetDoctorById(doctorId);
            MedicalRecordService medicalRecordService = new MedicalRecordService();
            var IsMedicalRecordInserted = medicalRecordService.AddMedicalRecord(new Model.MedicalRecord
            {
                DoctorId = doctorId,
                DoctorFirstName = DoctorObject.FirstName,
                DoctorLastName = DoctorObject.LastName,
                PatientFirstName = PatientObject.FirstName,
                PatientLastName = PatientObject.LastName,
                PatientId = Convert.ToInt32(PatientObject.Id),
                ReceptionDate = PatientObject.RegisterDate,
                TreatmentProcessId = 1, // پذیرش شده
                LastTreatmentProcessChangeDate = DateTime.Now,
            });
            if (!IsMedicalRecordInserted)
                throw Common.MegaException.ThrowException("امکان ثبت این کاربر وجود ندارد.لطفا با واحد فنی تماس بگیرید.");

            return PatientObject;

        }
        public bool PatientSearch(string info)
        {
            if (info == null)
            {
                throw Common.MegaException.ThrowException("هیچ اطلاعاتی وارد نشده");
            }
            else
            {
                var entity = GetPatientByName(info);
                if (entity == null)
                {
                    var Entity = GetPatientByDoctorName(info);
                    if (Entity == null)
                    {
                        GetPatientByCode(info);


                    }
                    else
                        throw Common.MegaException.ThrowException("اطلاعات وارد شذه صحیح نمی باشد");


                }
            }
            return true;
        }
        #endregion
        // MediacalRecord CRUD needed


    }

}
