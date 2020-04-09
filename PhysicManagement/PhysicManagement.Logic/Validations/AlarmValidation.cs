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
                RuleFor(x => x.Title).Length(1,150).WithMessage("عنوان باید بین 1 تا 150 حرف باشد.");
                RuleFor(x => x.EnglishTitle).Length(1, 150).WithMessage("عنوان انگلیسی باید بین 1 تا 150 حرف باشد.");

            }
        }
        #endregion
        #region AlarmValidation
        public class AlarmEntityValidate : AbstractValidator<Model.Alarm>
        {
            public AlarmEntityValidate()
            {
                RuleFor(x => x.ReviewText).Length(1,150).WithMessage("پر کردن متن نظر اجباری است ");
                RuleFor(x => x.ReviewUserName).Length(1,150).WithMessage("پر کردن کاربر تایید کننده اجباری است ");
            }
        }
        #endregion
    }
}
