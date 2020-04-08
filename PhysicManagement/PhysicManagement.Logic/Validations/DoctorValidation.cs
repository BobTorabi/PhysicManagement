using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace PhysicManagement.Logic.Validations
{
    public class DoctorValidation
    {
        public class DoctorEntityValidate : AbstractValidator<Model.Doctor>
        {
            public DoctorEntityValidate()
            {
                RuleFor(e => e.Id).NotNull().WithMessage("شناسه نمی تواند مقدار خالی داشته باشد");
                RuleFor(x => x.FirstName).NotNull().NotEmpty().WithMessage("نام نمی تواند مقدار خالی داشته باشد");
                RuleFor(e => e.FirstName).Must(Common.Validate.IsPersianName).WithMessage("نام را به زبان فارسی وارد کنید");
                RuleFor(x => x.Username).NotNull().NotEmpty().WithMessage("نام کاربری نمی تواند مقدار خالی داشته باشد");
                RuleFor(x => x.Password).NotNull().NotEmpty().WithMessage("رمز عبور نمی تواند مقدار خالی داشته باشد");
                RuleFor(x => x.LastName).NotNull().NotEmpty().WithMessage("نام خانوادگی نمی تواند مقدار خالی داشته باشد");
                RuleFor(e => e.LastName).Must(Common.Validate.IsPersianName).WithMessage("نام خانوادگی را به زبان فارسی وارد کنید");
                RuleFor(x => x.Code).NotNull().NotEmpty().WithMessage("کد نمی تواند مقدار خالی داشته باشد");
            }
        }
    }
}
