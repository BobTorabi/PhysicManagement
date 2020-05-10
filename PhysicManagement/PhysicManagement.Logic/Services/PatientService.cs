using FluentValidation;
using PhysicManagement.Logic.Validations;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Data.Entity;
using PhysicManagement.Model;

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
        public List<Model.Patient> GetPatientListDontHaveMriOrCTScan()
        {
            using (var db = new Model.PhysicManagementEntities())
            {
                return db.Patient.Where(x => x.MedicalRecord.Any(t => t.CTEnterDate == null || t.MRIEnterDate == null)).Include(x => x.MedicalRecord).OrderByDescending(x => x.RegisterDate).ToList();
            }
        }
        public List<Model.Patient> GetPatientListWithUnsetFusion(string firstName, string lastName, string nationalCode, string mobile, string systemCode, string code)
        {
            using (var db = new Model.PhysicManagementEntities())
            {
                IQueryable<Patient> Queryable = db.Patient.Where(x => x.MedicalRecord.Any(t => t.NeedsFusion == null));
                if (!string.IsNullOrEmpty(firstName))
                {
                    firstName = firstName.Trim();
                    Queryable = Queryable.Where(x => x.FirstName == firstName);
                }
                if (!string.IsNullOrEmpty(lastName))
                {
                    lastName = lastName.Trim();
                    Queryable = Queryable.Where(x => x.LastName == lastName);
                }
                if (!string.IsNullOrEmpty(nationalCode))
                {
                    nationalCode = nationalCode.Trim();
                    Queryable = Queryable.Where(x => x.NationalCode == nationalCode);
                }
                if (!string.IsNullOrEmpty(mobile))
                {
                    mobile = mobile.Trim();
                    Queryable = Queryable.Where(x => x.Mobile == mobile);
                }
                if (!string.IsNullOrEmpty(systemCode))
                {
                    systemCode = systemCode.Trim();
                    Queryable = Queryable.Where(x => x.MedicalRecord.Any(t => t.SystemCode == systemCode));
                }
                if (!string.IsNullOrEmpty(code))
                {
                    code = code.Trim();
                    Queryable = Queryable.Where(x => x.Code == code);
                }
                return Queryable.Include(x => x.MedicalRecord).OrderByDescending(x => x.RegisterDate).ToList();
            }
        }
        public List<Model.Patient> GetPatientListWithUnsetCountor(string firstName, string lastName, string nationalCode, string mobile, string systemCode, string code,bool? hasContour)
        {
            using (var db = new Model.PhysicManagementEntities())
            {
                IQueryable<Patient> Queryable = db.Patient;
                if (hasContour.HasValue)
                {
                    bool HasContour = hasContour.Value;
                    if (HasContour)
                    {
                        Queryable = Queryable.Where(x => x.MedicalRecord.Any(t => t.Contour.Count > 0));
                    }
                    else {
                        Queryable = Queryable.Where(x => x.MedicalRecord.Any(t => t.Contour.Count == 0));
                    }
                }
                if (!string.IsNullOrEmpty(firstName))
                {
                    firstName = firstName.Trim();
                    Queryable = Queryable.Where(x => x.FirstName == firstName);
                }
                if (!string.IsNullOrEmpty(lastName))
                {
                    lastName = lastName.Trim();
                    Queryable = Queryable.Where(x => x.LastName == lastName);
                }
                if (!string.IsNullOrEmpty(nationalCode))
                {
                    nationalCode = nationalCode.Trim();
                    Queryable = Queryable.Where(x => x.NationalCode == nationalCode);
                }
                if (!string.IsNullOrEmpty(mobile))
                {
                    mobile = mobile.Trim();
                    Queryable = Queryable.Where(x => x.Mobile == mobile);
                }
                if (!string.IsNullOrEmpty(systemCode))
                {
                    systemCode = systemCode.Trim();
                    Queryable = Queryable.Where(x => x.MedicalRecord.Any(t => t.SystemCode == systemCode));
                }
                if (!string.IsNullOrEmpty(code))
                {
                    code = code.Trim();
                    Queryable = Queryable.Where(x => x.Code == code);
                }
                return Queryable.Include("MedicalRecord").Include("MedicalRecord.Contour").OrderByDescending(x => x.RegisterDate).ToList();
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
                    firstName = firstName.Trim();
                    QueryablePatient = QueryablePatient.Where(x => x.FirstName.Contains(firstName));
                }
                if (!string.IsNullOrEmpty(lastName))
                {
                    lastName = lastName.Trim();
                    QueryablePatient = QueryablePatient.Where(x => x.LastName.Contains(lastName));
                }
                if (!string.IsNullOrEmpty(mobile))
                {
                    mobile = mobile.Trim();
                    QueryablePatient = QueryablePatient.Where(x => x.Mobile.Contains(mobile));
                }
                if (!string.IsNullOrEmpty(code))
                {
                    code = code.Trim();
                    QueryablePatient = QueryablePatient.Where(x => x.Code.Contains(code));
                }
                if (!string.IsNullOrEmpty(nationalCode))
                {
                    nationalCode = nationalCode.Trim();
                    QueryablePatient = QueryablePatient.Where(x => x.NationalCode.Contains(nationalCode));
                }
                if (!string.IsNullOrEmpty(systemCode))
                {
                    systemCode = systemCode.Trim();
                    QueryablePatient = QueryablePatient.Where(x => x.MedicalRecord.Any(z => z.SystemCode == systemCode));
                }
                QueryablePatient = QueryablePatient.OrderBy(x => x.LastName);

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
                var Entity = db.Patient.Where(x => x.MedicalRecord.Any(t => t.Id == medicalRecordId)).Include(x => x.MedicalRecord).FirstOrDefault();
                var MedicalData = Entity.MedicalRecord.FirstOrDefault();
                return new ViewModels.PatientVMs.MedicalRecordDataWithPatientData()
                {
                    CTDescription = MedicalData.CTDescription,
                    Code = Entity.Code,
                    CTCode = MedicalData.CTCode,
                    SystemCode = MedicalData.SystemCode,
                    FirstName = Entity.FirstName,
                    LastName = Entity.LastName,
                    MedicalRecordId = MedicalData.Id,
                    MRICode = MedicalData.MRICode,
                    PatientId = Entity.Id
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

        public Model.Patient RegisterPatient(string patientFirstName, string patientLastName, string nationalCode, int doctorId, string mobile)
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
                    RegisterDate = System.DateTime.Now

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
                ReceptionDate = PatientObject.RegisterDate
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
