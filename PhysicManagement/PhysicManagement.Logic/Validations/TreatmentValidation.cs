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
                RuleFor(x => x.PhaseNumber).NotEmpty().WithMessage("پر کردن فازاجباری است ");
                RuleFor(x => x.PhaseText).NotEmpty().WithMessage("پر کردن عنوان فازاجباری است ");
                RuleFor(x => x.Target).NotEmpty().WithMessage("پر کردن هدف اجباری است ");
                RuleFor(x => x.Description).NotEmpty().WithMessage("پر کردن توضیحات اجباری است ");
                RuleFor(x => x.Dose).NotEmpty().WithMessage("پر کردن دزاجباری است ");
                RuleFor(x => x.Fraction).NotEmpty().WithMessage("پر کردن فرکشن اجباری است ");

            }
        }
        #endregion
        #region TreatmentProcessValidation
        public class TreatmentProcessEntityValidate : AbstractValidator<Model.TreatmentProcess>
        {
            public TreatmentProcessEntityValidate()
            {
                RuleFor(x => x.Id).NotNull.NotEmpty().WithMessage("شناسه نمی تواند مقدار خالی داشته باشد ");
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
                RuleFor(x => x.Title).NotEmpty().WithMessage("پر کردن عنوان اجباری است ");
                RuleFor(x => x.Title).Must(Common.Validate.IsPersianText).WithMessage("عنوان باید به زبان فارسی پر شود");
                RuleFor(x => x.Code).NotEmpty().WithMessage("پر کردن کد اجباری است ");
                RuleFor(x => x.EnglishTitle).NotEmpty().WithMessage("پر کردن عنوان انگلیسی اجباری است ");
                RuleFor(x => x.EnglishTitle).Must(Common.Validate.IsText).WithMessage("عنوان انگلیسی وارد شده صحیح نمی باشد");
                RuleFor(x => x.Description).NotEmpty().WithMessage("پر کردن توضیحات اجباری است ");
                RuleFor(x => x.Description).Must(Common.Validate.IsText).WithMessage("توضیحات وارد شده صحیح نمی باشد");
                RuleFor(x => x.Description).MaximumLength(200).WithMessage("متنی که نوشته اید بیشتر از 200 حرف نمی تواند باشد");
            }
        }
        #endregion




    }
}

