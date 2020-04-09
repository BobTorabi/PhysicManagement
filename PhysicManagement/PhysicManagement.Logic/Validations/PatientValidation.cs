using FluentValidation;

namespace PhysicManagement.Logic.Validations
{
   public class PatientValidation
    {
        public class PatientEntityValidate : AbstractValidator<Model.Patient>
        {
            public PatientEntityValidate()
            {
                RuleFor(x => x.FirstName).Length(1,150).WithMessage("پر کردن نام اجباری است ");
                RuleFor(x => x.LastName).Length(1,150).WithMessage("نام خانوادگی این فیلد اجباری است ");
                RuleFor(x => x.NationalCode).Length(10).WithMessage("پر کردن کد ملی اجباری است");
                RuleFor(x => x.Mobile).Length(11).WithMessage("پر کردن موبایل اجباری است");
                RuleFor(x => x.GenderIsMale).NotEmpty().WithMessage("پر کردن جنسیت اجباری است");
                RuleFor(x => x.Province).Length(1,50).WithMessage("پر کردن استان اجباری است");
                RuleFor(x => x.City).Length(1,50).WithMessage("پر کردن شهر اجباری است");
                RuleFor(x => x.Address).Length(1,250).WithMessage("پر کردن ادرس اجباری است");
            }
        }
    }
}
