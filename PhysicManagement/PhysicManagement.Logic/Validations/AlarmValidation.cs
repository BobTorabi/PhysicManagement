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
                RuleFor(x => x.Title).Null().WithMessage("عنوان نمی تواند نال باشد")
                    .Empty().WithMessage("پر کردن عنوان اجباری است ");
                RuleFor(x => x.Title).Must(Common.Validate.IsPersianText).WithMessage("عنوان وارد شده صحیح نمی باشد");
                RuleFor(x => x.Description).Null().WithMessage("توضیحات نمی تواند نال باشد")
                    .Empty().WithMessage("پر کردن توضیحات اجباری است ");
                RuleFor(x => x.Description).Must(Common.Validate.IsText).WithMessage("اطلاعات وارد شده صحیح نمی باشد");
                RuleFor(x => x.Description).MaximumLength(200).WithMessage("متنی که نوشته اید نمی تواند بیشتر از 200 حرف باشد");
                RuleFor(x => x.EnglishTitle).Null().WithMessage("عنوان انگلیسی نمی تواند نال باشد")
                    .Empty().WithMessage("پر کردن عنوان انگلیسی اجباری است ");
                RuleFor(x => x.EnglishTitle).Must(Common.Validate.IsText).WithMessage("عنوان انگلیسی وارد شده صحیح نمی باشد");
            }
        }
        #endregion
        #region AlarmValidation
        public class AlarmEntityValidate : AbstractValidator<Model.Alarm>
        {
            public AlarmEntityValidate()
            {
                RuleFor(x => x.ReviewDate).Null().WithMessage("نظر تاریخ نمی تواند نال باشد")
                    .Empty().WithMessage("پر کردن نظر تاریخ اجباری است ");
                RuleFor(x => x.ReviewText).NotNull().WithMessage("متن نظر نمی تواند نال باشد")
                    .NotEmpty().WithMessage("پر کردن متن نظر اجباری است ");
                RuleFor(x => x.ReviewText).Must(Common.Validate.IsText).WithMessage("اطلاعات وارد شده صحیح نمی باشد");
                RuleFor(x => x.ReviewUserName).Null().WithMessage("کاربر تایید کننده نمی تواند نال باشد")
                    .Empty().WithMessage("پر کردن کاربر تایید کننده اجباری است ");
            }
        }
        #endregion
    }
}
