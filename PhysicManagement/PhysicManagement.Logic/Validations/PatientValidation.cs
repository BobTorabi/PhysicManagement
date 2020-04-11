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
                RuleFor(x => x.NationalCode).Length(10).WithMessage("کد ملی وارد شده صحیح نمی باشد");
            }
        }
    }
}
