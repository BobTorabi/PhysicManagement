﻿using FluentValidation;

namespace PhysicManagement.Logic.Validations
{
    public class TreatmentValidation
    {
        #region TreatmentPhaseValidation
        public class TreatmentPhaseEntityValidate : AbstractValidator<Model.TreatmentPhase>
        {
            public TreatmentPhaseEntityValidate()
            {
                //RuleFor(x => x.PhaseNumber).NotEmpty().WithMessage("پر کردن فازاجباری است ");
                //RuleFor(x => x.PhaseText).Length(1,50).WithMessage("پر کردن عنوان فازاجباری است ");
                //RuleFor(x => x.Target).Length(1,50).WithMessage("پر کردن هدف اجباری است ");
                //RuleFor(x => x.Dose).NotEmpty().WithMessage("پر کردن دزاجباری است ");
                //RuleFor(x => x.Fraction).NotEmpty().WithMessage("پر کردن فرکشن اجباری است ");

            }
        }
        public class TreatmentPhaseDetailEntityValidate : AbstractValidator<Model.TreatmentPhaseDetail>
        {
            public TreatmentPhaseDetailEntityValidate()
            {
                //RuleFor(x => x.PhaseNumber).NotEmpty().WithMessage("پر کردن فازاجباری است ");
                //RuleFor(x => x.PhaseText).Length(1,50).WithMessage("پر کردن عنوان فازاجباری است ");
                //RuleFor(x => x.Target).Length(1,50).WithMessage("پر کردن هدف اجباری است ");
                //RuleFor(x => x.Dose).NotEmpty().WithMessage("پر کردن دزاجباری است ");
                //RuleFor(x => x.Fraction).NotEmpty().WithMessage("پر کردن فرکشن اجباری است ");

            }
        }
        #endregion
        #region TreatmentProcessValidation
        public class TreatmentProcessEntityValidate : AbstractValidator<Model.TreatmentProcess>
        {
            public TreatmentProcessEntityValidate()
            {
                RuleFor(x => x.StepNumber).NotEmpty().WithMessage("پر کردن شماره مرحله اجباری است ");
                RuleFor(x => x.Title).Length(1,50).WithMessage("پر کردن عنوان اجباری است ");
                RuleFor(x => x.EnglishTitle).Length(1,50).WithMessage("پر کردن عنوان انگلیسی اجباری است ");
                RuleFor(x => x.Description).MaximumLength(200).WithMessage("متنی که نوشته اید نمی تواند بیشتر از 200 حرف باشد");
            }
        }
        #endregion
        #region TreatmentDeviceValidation
        public class TreatmentDeviceEntityValidate : AbstractValidator<Model.TreatmentDevice>
        {
            public TreatmentDeviceEntityValidate()
            {
                RuleFor(x => x.Title).Length(1,150).WithMessage("پر کردن عنوان اجباری است ");
                RuleFor(x => x.Code).Length(1,50).WithMessage("پر کردن کد اجباری است ");
                RuleFor(x => x.EnglishTitle).Length(1,150).WithMessage("پر کردن عنوان انگلیسی اجباری است ");
                RuleFor(x => x.Description).MaximumLength(200).WithMessage("متنی که نوشته اید بیشتر از 200 حرف نمی تواند باشد");
            }
        }
        #endregion

    }
}

