using FluentValidation;

namespace PhysicManagement.Logic.Validations
{
   public class PatientValidation
    {
        public class PatientEntityValidate : AbstractValidator<Model.Patient>
        {
            public PatientEntityValidate()
            {
                RuleFor(x => x.Id).NotNull().WithMessage("شناسه نمی تواند مقدار خالی داشته باشد ");
                RuleFor(x => x.FirstName).NotNull().NotEmpty().WithMessage("پر کردن این فیلد اجباری است ");
                RuleFor(x => x.FirstName).Must(Common.Validate.IsPersianName).WithMessage("این فیلد باید به زبان فارسی پر شود");
                RuleFor(x => x.LastName).NotNull().NotEmpty().WithMessage("پر کردن این فیلد اجباری است ");
                RuleFor(x => x.LastName).Must(Common.Validate.IsPersianName).WithMessage("این فیلد باید به زبان فارسی پر شود");
                RuleFor(x => x.NationalCode).NotEmpty().NotNull().WithMessage("پر کردن این فیلد اجباری است");
                RuleFor(x => x.NationalCode).Must(Common.Validate.IsNationalCode).WithMessage("کد ملی وارد شده صحیح نمی باشد");
                RuleFor(x => x.Mobile).NotNull().NotEmpty().WithMessage("پر کردن این فیلد اجباری است");
                RuleFor(x => x.Mobile).Must(Common.Validate.IsMobile).WithMessage("شماره وارد شده صحیح نمی باشد");
                RuleFor(x => x.GenderIsMale).NotEmpty().WithMessage("پر کردن این فیلد اجباری است");
                RuleFor(x => x.Province).NotNull().NotEmpty().WithMessage("پر کردن این فیلد اجباری است");
                RuleFor(x => x.Province).Must(Common.Validate.IsPersianName).WithMessage("این فیلد باید به ربان فارسی پر شود");
                RuleFor(x => x.City).NotNull().NotEmpty().WithMessage("پر کردن این فیلد اجباری است");
                RuleFor(x => x.City).Must(Common.Validate.IsPersianName).WithMessage("این فیلد باید به زبان فارسی پر شود");
                RuleFor(x => x.Address).NotNull().NotEmpty().WithMessage("پر کردن این فیلد اجباری است");
                RuleFor(x => x.Address).Must(Common.Validate.IsPersianText).WithMessage("این فیلد باید به زبان فارسی پر شود");
                RuleFor(x => x.RegisterDate).NotNull().NotEmpty().WithMessage("پر کردن این فیلد اجباری است");
            }
        }
    }
}
