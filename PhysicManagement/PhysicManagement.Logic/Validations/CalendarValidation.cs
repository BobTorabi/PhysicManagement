using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace PhysicManagement.Logic.Validations
{
   public class CalendarValidation
    {
        public class CalendarEntityValidate : AbstractValidator<Model.Calendar>
        {
            public CalendarEntityValidate()
            {
                RuleFor(e => e.Id).NotNull().NotEmpty().WithMessage("شناسه نمی تواند مقدار خالی داشته باشد");
                RuleFor(e => e.PatientFullName).Must(Common.Validate.IsPersianName).WithMessage("نام بیمار باید فارسی وارد شود");
                RuleFor(e => e.PatientFullName).NotNull().NotEmpty().WithMessage("نام بیمار نمیتواند مقدار خالی داشته باشد");
                RuleFor(e => e.DoctorFullName).Must(Common.Validate.IsPersianName).WithMessage("نام دکتر باید فارسی وارد شود");
                RuleFor(e => e.DoctorFullName).NotNull().NotEmpty().WithMessage("نام دکتر نمیتواند مقدار خالی داشته باشد");
                RuleFor(e => e.SessionNumber).NotNull().NotEmpty().WithMessage("شماره جلسه نمیتواند مقدار خالی داشته باشد");
                RuleFor(e => e.AttendanceStatusId).NotNull().NotEmpty().WithMessage("شناسه حضور نمیتواند مقدار خالی داشته باشد");
                RuleFor(e => e.AttendanceStatusId).NotNull().NotEmpty().WithMessage("شناسه درمان جسمی نمیتواند مقدار خالی داشته باشد");

            }
        }
    }
}
