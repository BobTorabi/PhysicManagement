using FluentValidation;
using PhysicManagement.Logic.Validations;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Data.Entity;

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
        public ViewModels.PagedList<Model.Patient> GetPatientListWithFilters(string firstName, string lastName, string mobile, string nationalCode, string systemCode, int CurrentPage = 1, int pageSize = 30)
        {
            using (var db = new Model.PhysicManagementEntities())
            {
                IQueryable<Model.Patient> QueryablePatient = db.Patient.Include("MedicalRecord");
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
                QueryablePatient = QueryablePatient.OrderBy(x => x.RegisterDate);

                return new ViewModels.PagedList<Model.Patient>()
                {
                    CurrentPage = CurrentPage,
                    PageSize = pageSize,
                    TotalRecords = QueryablePatient.Count(),
                    Records = QueryablePatient.Skip((CurrentPage - 1) * pageSize).Take(pageSize).ToList(),
                };
            }
        }
        public Model.Patient GetPatientById(long entityId)
        {
            using (var db = new Model.PhysicManagementEntities())
            {
                var Entity = db.Patient.Where(x=>x.Id == entityId).Include(x=>x.MedicalRecord).FirstOrDefault();
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
                    else {
                        return (int.Parse(PatientNo) + 1).ToString();
                    }
                }
            }
        }

        public string RegisterPatient(string patientFirstName, string patientLastName, string nationalCode, int doctorId, string mobile)
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
            string cod = Common.FileID.GetUniqueNumber(5, 10000, 99999).ToString();
            MedicalRecordService medicalRecordService = new MedicalRecordService();
            var IsMedicalRecordInserted = medicalRecordService.AddMedicalRecord(new Model.MedicalRecord
            {
                SystemCode = cod,
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

            return PatientObject.Id.ToString();

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
