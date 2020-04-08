using FluentValidation;

namespace PhysicManagement.Logic.Validations
{
   public class PatientValidation
    {
        public class PatientEntityValidate : AbstractValidator<Model.Patient>
        {
            public PatientEntityValidate()
            {
                RuleFor(x => x.FirstName).NotEmpty().WithMessage("پر کردن نام اجباری است ");
                RuleFor(x => x.FirstName).Must(Common.Validate.IsPersianName).WithMessage("نام باید به زبان فارسی پر شود");
                RuleFor(x => x.LastName).NotEmpty().WithMessage("نام خانوادگی این فیلد اجباری است ");
                RuleFor(x => x.LastName).Must(Common.Validate.IsPersianName).WithMessage("این فیلد باید به زبان فارسی پر شود");
                RuleFor(x => x.NationalCode).NotEmpty().WithMessage("پر کردن کد ملی اجباری است");
                RuleFor(x => x.NationalCode).Must(Common.Validate.IsNationalCode).WithMessage("کد ملی وارد شده صحیح نمی باشد");
                RuleFor(x => x.Mobile).NotEmpty().WithMessage("پر کردن موبایل اجباری است");
                RuleFor(x => x.Mobile).Must(Common.Validate.IsMobile).WithMessage("شماره موبایل وارد شده صحیح نمی باشد");
                RuleFor(x => x.GenderIsMale).NotEmpty().WithMessage("پر کردن جنسیت اجباری است");
                RuleFor(x => x.Province).NotEmpty().WithMessage("پر کردن استان اجباری است");
                RuleFor(x => x.Province).Must(Common.Validate.IsPersianName).WithMessage("استان باید به زبان فارسی پر شود");
                RuleFor(x => x.City).NotEmpty().WithMessage("پر کردن شهر اجباری است");
                RuleFor(x => x.City).Must(Common.Validate.IsPersianName).WithMessage("شهر باید به زبان فارسی پر شود");
                RuleFor(x => x.Address).NotEmpty().WithMessage("پر کردن ادرس اجباری است");
                RuleFor(x => x.Address).Must(Common.Validate.IsPersianText).WithMessage("ادرس باید به زبان فارسی پر شود");
            }
        }
    }
}
