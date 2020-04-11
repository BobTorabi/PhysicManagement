using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace PhysicManagement.Logic.Validations
{
   public class UserValidation
    {
        public class UserEntityValidate : AbstractValidator<Model.User>
        {
            public UserEntityValidate()
            {
                RuleFor(x => x.FirstName).Length(1, 150).WithMessage("پر کردن نام اجباری است");
                RuleFor(x => x.LastName).Length(1, 150).WithMessage("پر کردن نام خانوادگی اجباری است");
                RuleFor(x => x.Username).Length(1, 50).WithMessage("پر کردن نام کاربری اجباری است");
                RuleFor(x => x.Password).Length(1, 50).WithMessage("پر کردن رمز عبور اجباری است");

            }
        }
    }
}
