using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace PhysicManagement.Logic.Validations
{
    public class MedicalRecordValidation
    {
        public class MedicalRecordEntityValidation : AbstractValidator<Model.MedicalRecord>
        {
            public MedicalRecordEntityValidation()
            {
                RuleFor(e => e.PatientFirstName).Length(1,150).WithMessage("نام نباید خالی باشد");
                RuleFor(e => e.PatientLastName).Length(1,150).WithMessage("نام خانوادگی نباید خالی باشد");
                RuleFor(e => e.DoctorFirstName).Length(1,150).WithMessage("نام دکتر نباید خالی باشد");
                RuleFor(e => e.DoctorLastName).Length(1,150).WithMessage("نام خانوادگی دکتر نباید خالی باشد ");
                RuleFor(e => e.CTDescription).MaximumLength(200).WithMessage("متن نمی تواند بیشتر از 200 حرف داشته باشد");
                RuleFor(e => e.TPDescription).MaximumLength(200).WithMessage("متن نمی تواند بیشتر از 200 حرف داشته باشد");

            }
        }
    }
}
