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
                RuleFor(x => x.Description).MaximumLength(200).WithMessage("متنی که نوشته اید نمی تواند بیشتر از 200 حرف باشد");
            }
        }
        #endregion
        #region TreatmentDeviceValidation
        public class TreatmentDeviceEntityValidate : AbstractValidator<Model.TreatmentDevice>
        {
            public TreatmentDeviceEntityValidate()
            {
                RuleFor(x => x.Id).NotNull().NotEmpty().WithMessage("شناسه نمی تواند مقدار خالی داشته باشد ");
                RuleFor(x => x.Title).NotNull().NotEmpty().WithMessage("پر کردن این فیلد اجباری است ");
                RuleFor(x => x.Title).Must(Common.Validate.IsPersianText).WithMessage("این فیلد باید به زبان فارسی پر شود");
                RuleFor(x => x.Code).NotNull().NotEmpty().WithMessage("پر کردن این فیلد اجباری است ");
                RuleFor(x => x.EnglishTitle).NotNull().NotEmpty().WithMessage("پر کردن این فیلد اجباری است ");
                RuleFor(x => x.EnglishTitle).Must(Common.Validate.IsText).WithMessage("عنوان وارد شده صحیح نمی باشد");
                RuleFor(x => x.Description).NotNull().NotEmpty().WithMessage("پر کردن این فیلد اجباری است ");
                RuleFor(x => x.Description).Must(Common.Validate.IsText).WithMessage("اطلاعات وارد شده صحیح نمی باشد");
                RuleFor(x => x.Description).MaximumLength(200).WithMessage("متنی که نوشته اید بیشتر از 200 حرف نمی تواند باشد");
            }
        }
        #endregion




    }
}

