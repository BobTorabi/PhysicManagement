using FluentValidation;

namespace PhysicManagement.Logic.Validations
{
    public class PhysicTreatmentPlanValidate : AbstractValidator<Model.PhysicTreatmentPlan>
    {
        public PhysicTreatmentPlanValidate()
        {

        }
    }

    public class PhysicTreatmentPlanDetailValidate : AbstractValidator<Model.PhysicTreatmentPlanDetail>
    {
        public PhysicTreatmentPlanDetailValidate()
        {

        }
    }
}
