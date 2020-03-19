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
                RuleFor(x => x.FirstName).NotNull().NotEmpty().WithMessage("نام نمیتواند مقدار خالی داشته باشد");
                RuleFor(x => x.LastName).NotNull().NotEmpty().WithMessage("نام خانوادگی نمیتواند مقدار خالی داشته باشد");
                RuleFor(x => x.Code).NotNull().NotEmpty().WithMessage("کد نمیتواند مقدار خالی داشته باشد");
            }
        }
    }
}
