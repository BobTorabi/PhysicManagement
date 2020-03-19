using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhysicManagement.Logic.Validations
{
    public class PhysicTreatmentValidation
    {
        public class PhysicTreatmentEntityValidate : AbstractValidator<Model.PhysicTreatment>
        {

            public PhysicTreatmentEntityValidate()
            {
                RuleFor(e => e.PhaseNumber).NotEmpty().WithMessage("");
            }
        }
        public class PhysicTreatmentPlanEntityValidate : AbstractValidator<Model.PhysicTreatmentPlan>
        {

            public PhysicTreatmentPlanEntityValidate()
            {
                RuleFor(e => e.Evaluation).NotEmpty().WithMessage("");
            }
        }
      
    }
}
