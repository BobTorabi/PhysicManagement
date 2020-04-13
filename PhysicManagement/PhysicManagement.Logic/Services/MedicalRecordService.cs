using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PhysicManagement.Logic.Validations;
using FluentValidation;

namespace PhysicManagement.Logic.Services
{
    public class MedicalRecordService
    {
        #region MedicalRecord section

        public List<Model.MedicalRecord> GetMedicalRecordList()
        {
            using (var db = new Model.PhysicManagementEntities())
            {
                return db.MedicalRecord.OrderBy(x => x.Id).ToList();
            }
        }
        public Model.MedicalRecord GetMedicalRecordById(int entityId)
        {
            using (var db = new Model.PhysicManagementEntities())
            {
                var Entity = db.MedicalRecord.Find(entityId);
                return Entity;
            }
        }
        public bool AddMedicalRecord(Model.MedicalRecord entity)
        {
            var valition = new MedicalRecordValidation.MedicalRecordEntityValidation().Validate(entity);
            if (!valition.IsValid)
                throw new ValidationException(valition.Errors);


            using (var db = new Model.PhysicManagementEntities())
            {
                entity.SystemCode = GetSystemCodeToRegister();
                entity.ReceptionDate = DateTime.Now;
                db.MedicalRecord.Add(entity);
                return db.SaveChanges() == 1;
            }
        }
        public bool UpdateMedicalRecord(Model.MedicalRecord entity)
        {
            var validtion = new MedicalRecordValidation.MedicalRecordEntityValidation().Validate(entity);
            if (!validtion.IsValid)
                throw new ValidationException(validtion.Errors);

            using (var db = new Model.PhysicManagementEntities())
            {
                var Entity = db.MedicalRecord.Find(entity.Id);
                Entity.PatientFirstName = entity.PatientFirstName;
                Entity.PatientLastName = entity.PatientLastName;
                Entity.DoctorFirstName = entity.DoctorFirstName;
                Entity.DoctorLastName = entity.DoctorLastName;
                Entity.CTCode = entity.CTCode;
                Entity.CTDescription = entity.CTDescription;
                Entity.TPCode = entity.TPCode;
                Entity.TPDescription = entity.TPDescription;
                Entity.MRICode = entity.MRICode;
                Entity.CancerTitle = entity.CancerTitle;

                return db.SaveChanges() == 1;
            }
        }
        public bool DeleteMedicalRecord(Model.MedicalRecord entityId)
        {
            using (var db = new Model.PhysicManagementEntities())
            {
                var Entity = db.MedicalRecord.Find(entityId);
                if (Entity == null)
                    throw new ValidationException("این رکورد در پایگاه دادهوجود ندارد");
                db.MedicalRecord.Remove(Entity);
                return db.SaveChanges() == 1;
            }
        }

        public bool UpdateMedicalRecordForDoctorChange(long medicalRecordId,int doctorId)
        {
            using (var db = new Model.PhysicManagementEntities())
            {
                var Entity = db.MedicalRecord.Find(medicalRecordId);
                if (Entity == null)
                    throw Common.MegaException.ThrowException("");

                Model.Doctor DoctorObject = new DoctorService().GetDoctorById(doctorId);
                if (DoctorObject == null)
                    throw Common.MegaException.ThrowException("");

                Entity.DoctorFirstName = DoctorObject.FirstName;
                Entity.DoctorLastName = DoctorObject.LastName;
                Entity.DoctorId = DoctorObject.Id;
                return db.SaveChanges() == 1;
            }
        }

        private Object thisLock = new Object();

        /// <summary>
        /// این متد برای به دست آوردن شماره ی بعدی شناسه سیستمی پرونده استفاده می گردد
        /// </summary>
        /// <returns></returns>
        public string GetSystemCodeToRegister()
        {
            lock (thisLock)
            {
                using (var db = new Model.PhysicManagementEntities())
                {
                    var SystemCode = db.MedicalRecord.Max(x => x.SystemCode);
                    if (string.IsNullOrEmpty(SystemCode))
                    {
                        return "1000";
                    }
                    else
                    {
                        return (int.Parse(SystemCode) + 1).ToString();
                    }
                }
            }
        }
        #endregion
    }
}
