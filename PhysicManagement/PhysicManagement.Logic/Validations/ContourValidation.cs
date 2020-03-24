using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace PhysicManagement.Logic.Validations
{
    public class ContourValidation
    {
        public class ContourEntityValidate : AbstractValidator<Model.ContourDetails>
        {
            public ContourEntityValidate()
            {
                RuleFor(e => e.Id).NotNull().NotEmpty().WithMessage("شناسه نباید مقدار خالی داشته باشد");
                RuleFor(e => e.Description).NotNull().NotEmpty().WithMessage("توضیحات نباید خالی باشد");
            }
        }

        public class ContourDetailsEntityValidate : AbstractValidator<Model.ContourDetails>
        {
            public ContourDetailsEntityValidate()
            {
                RuleFor(e => e.Id).NotNull().NotEmpty().WithMessage("شناسه نباید مقدار خالی داشته باشد");
                RuleFor(e => e.Description).NotNull().NotEmpty().WithMessage("توضیحات نباید خالی باشد");
            }
        }
    }
}
