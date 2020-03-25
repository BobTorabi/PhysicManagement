using FluentValidation;

namespace PhysicManagement.Logic.Validations
{
    public class AlarmValidation
    {
        #region AlarmTypeValidation
        public class AlarmTypeEntityValidate : AbstractValidator<Model.AlarmType>
        {
            public AlarmTypeEntityValidate()
            {
                RuleFor(x => x.Id).NotNull().NotEmpty().WithMessage("شناسه نمی تواند مقدار خالی داشته باشد ");
                RuleFor(x => x.Title).NotNull().NotEmpty().WithMessage("پر کردن این فیلد اجباری است ");
                RuleFor(x => x.Title).Must(Common.Validate.IsPersianText).WithMessage("عنوان وارد شده صحیح نمی باشد");
                RuleFor(x => x.IsActive).NotNull().NotEmpty().WithMessage("پر کردن این فیلد اجباری است ");
                RuleFor(x => x.Description).NotNull().NotEmpty().WithMessage("پر کردن این فیلد اجباری است ");
                RuleFor(x => x.Description).Must(Common.Validate.IsText).WithMessage("اطلاعات وارد شده صحیح نمی باشد");
                RuleFor(x => x.Description).MaximumLength(200).WithMessage("متنی که نوشته اید نمی تواند بیشتر از 200 حرف باشد");
                RuleFor(x => x.EnglishTitle).NotNull().NotEmpty().WithMessage("پر کردن این فیلد اجباری است ");
                RuleFor(x => x.EnglishTitle).Must(Common.Validate.IsText).WithMessage("عنوان وارد شده صحیح نمی باشد");
            }
        }
        #endregion
        #region AlarmValidation
        public class AlarmEntityValidate : AbstractValidator<Model.Alarm>
        {
            public AlarmEntityValidate()
            {
                RuleFor(x => x.Id).NotNull().NotEmpty().WithMessage("شناسه نمی تواند مقدار خالی داشته باشد ");
                RuleFor(x => x.AlarmTypeId).NotNull().NotEmpty().WithMessage("پر کردن این فیلد اجباری است ");
                RuleFor(x => x.AlarmTypeTitle).NotNull().NotEmpty().WithMessage("پر کردن این فیلد اجباری است ");
                RuleFor(x => x.GenerateDate).NotNull().NotEmpty().WithMessage("پر کردن این فیلد اجباری است ");
                RuleFor(x => x.GenerateTreatmentPhaseId).NotNull().NotEmpty().WithMessage("پر کردن این فیلد اجباری است ");
                RuleFor(x => x.GenerateTreatmentPhaseTitle).NotNull().NotEmpty().WithMessage("پر کردن این فیلد اجباری است ");
                RuleFor(x => x.GenerateUser).NotNull().NotEmpty().WithMessage("پر کردن این فیلد اجباری است ");
                RuleFor(x => x.IsActive).NotNull().NotEmpty().WithMessage("پر کردن این فیلد اجباری است ");
                RuleFor(x => x.IsOnBoard).NotNull().NotEmpty().WithMessage("پر کردن این فیلد اجباری است ");
                RuleFor(x => x.ReviewDate).NotNull().NotEmpty().WithMessage("پر کردن این فیلد اجباری است ");
                RuleFor(x => x.ReviewText).NotNull().NotEmpty().WithMessage("پر کردن این فیلد اجباری است ");
                RuleFor(x => x.ReviewText).Must(Common.Validate.IsText).WithMessage("اطلاعات وارد شده صحیح نمی باشد");
                RuleFor(x => x.ReviewUserName).NotNull().NotEmpty().WithMessage("پر کردن این فیلد اجباری است ");
            }
        }
        #endregion
    }
}
