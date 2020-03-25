using FluentValidation;

namespace PhysicManagement.Logic.Validations
{
    public class TreatmentValidation
    {
        #region TreatmentPhaseValidation
        public class TreatmentPhaseEntityValidate : AbstractValidator<Model.TreatmentPhase>
        {
            public TreatmentPhaseEntityValidate()
            {
                RuleFor(x => x.Id).NotNull().NotEmpty().WithMessage("شناسه نمی تواند مقدار خالی داشته باشد ");
                RuleFor(x => x.MedicalRecordId).NotNull().NotEmpty().WithMessage("پر کردن این فیلد اجباری است ");
                RuleFor(x => x.PhaseNumber).NotNull().NotEmpty().WithMessage("پر کردن این فیلد اجباری است ");
                RuleFor(x => x.PhaseText).NotNull().NotEmpty().WithMessage("پر کردن این فیلد اجباری است ");
                RuleFor(x => x.PrescribeDate).NotNull().NotEmpty().WithMessage("پر کردن این فیلد اجباری است ");
                RuleFor(x => x.PrescribesdUser).NotNull().NotEmpty().WithMessage("پر کردن این فیلد اجباری است ");
                RuleFor(x => x.Target).NotNull().NotEmpty().WithMessage("پر کردن این فیلد اجباری است ");
                RuleFor(x => x.Description).NotNull().NotEmpty().WithMessage("پر کردن این فیلد اجباری است ");
                RuleFor(x => x.Dose).NotNull().NotEmpty().WithMessage("پر کردن این فیلد اجباری است ");
                RuleFor(x => x.Fraction).NotNull().NotEmpty().WithMessage("پر کردن این فیلد اجباری است ");

            }
        }
        #endregion
        #region TreatmentProcessValidation
        public class TreatmentProcessEntityValidate : AbstractValidator<Model.TreatmentProcess>
        {
            public TreatmentProcessEntityValidate()
            {
                RuleFor(x => x.Id).NotNull().NotEmpty().WithMessage("شناسه نمی تواند مقدار خالی داشته باشد ");
                RuleFor(x => x.StepNumber).NotNull().NotEmpty().WithMessage("پر کردن این فیلد اجباری است ");
                RuleFor(x => x.Title).NotNull().NotEmpty().WithMessage("پر کردن این فیلد اجباری است ");
                RuleFor(x => x.Title).Must(Common.Validate.IsPersianText).WithMessage("اطلاعات وارد شده صحیح نمی باشد");
                RuleFor(x => x.EnglishTitle).NotNull().NotEmpty().WithMessage("پر کردن این فیلد اجباری است ");
                RuleFor(x => x.EnglishTitle).Must(Common.Validate.IsText).WithMessage("اطلاعات وارد شده صحیح نمی باشد");
                RuleFor(x => x.Description).NotNull().NotEmpty().WithMessage("پر کردن این فیلد اجباری است ");
                RuleFor(x => x.Description).Must(Common.Validate.IsText).WithMessage("اطلاعات وارد شده صحیح نمی باشد");
            }
        }
        #endregion




    }
}

