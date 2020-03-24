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
                RuleFor(e => e.Id).NotNull().NotEmpty().WithMessage("شناسه نباید مقدار خالی داشته باشد");
                RuleFor(e => e.PatientFirstName).NotNull().NotEmpty().WithMessage("نام نباید خالی باشد");
                RuleFor(e => e.PatientFirstName).Must(Common.Validate.IsPersianName).WithMessage("نام را به زبان فارسی وارد کنید");
                RuleFor(e => e.PatientLastName).NotNull().NotEmpty().WithMessage("نام خانوادگی نباید خالی باشد");
                RuleFor(e => e.PatientLastName).Must(Common.Validate.IsPersianName).WithMessage("نام خانوادگی را به زبان فارسی وارد کنید");
                RuleFor(e => e.DoctorFirstName).NotNull().NotEmpty().WithMessage("نام دکتر نباید خالی باشد");
                RuleFor(e => e.DoctorFirstName).Must(Common.Validate.IsPersianName).WithMessage("نام دکتر را به زبان فارسی وارد کنید");
                RuleFor(e => e.DoctorLastName).NotNull().NotEmpty().WithMessage("نام خانوادگی دکتر نباید خالی باشد ");
                RuleFor(e => e.DoctorLastName).Must(Common.Validate.IsPersianName).WithMessage("نام خانوادگی دکتر را به زبان فارسی وارد کنید");
                RuleFor(e => e.CancerTitle).NotNull().NotEmpty().WithMessage("عنوان سرطان نباید مقدار خالی داشته باشد");
                RuleFor(e => e.CancerTitle).Must(Common.Validate.IsPersianText).WithMessage("عنوان سرطان را به زبان فارسی وارد کنید");
                RuleFor(e => e.CTCode).NotNull().NotEmpty().WithMessage("کد ct نباید مقدار خالی داشته باشد");
                RuleFor(e => e.MRICode).NotNull().NotEmpty().WithMessage("کد MRI نباید مقدار خالی داشته باشد");
                RuleFor(e => e.CTDescription).NotNull().NotEmpty().WithMessage("توضیحات نباید مقدار خالی داشته باشد");
                RuleFor(e => e.CTDescription).Must(Common.Validate.IsPersianText).WithMessage("توضیحات را به زبان فارسی وارد کنید");
                RuleFor(e => e.TPCode).NotNull().NotEmpty().WithMessage("کد TPنباید مقدار خالی داشته باشد");
                RuleFor(e => e.TPDescription).Must(Common.Validate.IsPersianText).WithMessage("توضیحات را به زبان فارسی وارد کنید");

            }
        }
    }
}
