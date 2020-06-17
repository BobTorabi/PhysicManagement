using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhysicManagement.Logic.Validations
{
    public class KFactorValidation
    {
        public class KFactorEntityValidate : AbstractValidator<Model.KFactor>
        {
            public KFactorEntityValidate()
            {
                RuleFor(e => e.Year).NotNull().NotEmpty().WithMessage("سال نباید خالی باشد");
            }
        }
    }
}
