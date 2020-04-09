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
                RuleFor(x => x.PhysicTreatmentPlanHostory).NotEmpty().WithMessage("پر کردن تاریخ نقشه درمان فیزیکی اجباری است ");
                RuleFor(x => x.PlannedDose).NotEmpty().WithMessage("پر کردن دز برنامه ریزی شده اجباری است ");
                RuleFor(x => x.Reserve1).NotEmpty().WithMessage("پر کردن فیلد رزرو 1 اجباری است ");
                RuleFor(x => x.ActionUser).NotEmpty().WithMessage("پر کردن کاربر اجباری است ");
                RuleFor(x => x.Evaluation).NotEmpty().WithMessage("پر کردن ارزیابی اجباری است ");

            }
        }
        #endregion
        #region PhysicTreatmentPlanHOstory
        public class PhysicTreatmentPlanHostoryEntityValidate : AbstractValidator<Model.PhysicTreatmentPlanHostory>
        {
            public PhysicTreatmentPlanHostoryEntityValidate()
            {
                RuleFor(x => x.ActionUser).NotEmpty().WithMessage("پر کردن کاربر اجباری است ");
                RuleFor(x => x.ChangeDate).NotEmpty().WithMessage("پر کردن تغییر تاریخ اجباری است ");
                RuleFor(x => x.Note).NotEmpty().WithMessage("پر کردن یادداشت اجباری است ");

            }
        }
        #endregion

    }
}
