using FluentValidation;

namespace PhysicManagement.Logic.Validations
{
    public class PhysicTreatmentValidation
    {
        #region PhysicTreatmetnValidation
        public class PhysicTreatmentEntityValidate : AbstractValidator<Model.PhysicTreatment>
        {

            public PhysicTreatmentEntityValidate()
            {
                RuleFor(x => x.PhaseNumber).NotEmpty().WithMessage("پر کردن شماره فاز اجباری است ");
            }
        }
        #endregion
        #region PhysicTreatmentPlanValidation
        public class PhysicTreatmentPlanEntityValidate : AbstractValidator<Model.PhysicTreatmentPlan>
        {

            public PhysicTreatmentPlanEntityValidate()
            {
                RuleFor(x => x.PlannedDose).Length(1,500).WithMessage("پر کردن دز برنامه ریزی شده اجباری است ");
                RuleFor(x => x.Reserve1).Length(1,50).WithMessage("پر کردن فیلد رزرو 1 اجباری است ");
                RuleFor(x => x.Evaluation).Length(1,500).WithMessage("پر کردن ارزیابی اجباری است ");

            }
        }
        #endregion
        #region PhysicTreatmentPlanHOstory
        public class PhysicTreatmentPlanHostoryEntityValidate : AbstractValidator<Model.PhysicTreatmentPlanHostory>
        {
            public PhysicTreatmentPlanHostoryEntityValidate()
            {
                RuleFor(x => x.Note).Length(1,500).WithMessage("پر کردن یادداشت اجباری است ");

            }
        }
        #endregion

    }
}
