﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace PhysicManagement.Logic.Validations
{
    public class ResidentValidation
    {
        public class ResidentEntityValidation : AbstractValidator<Model.Resident>
        {
            public ResidentEntityValidation()
            {
                RuleFor(e => e.FirstName).Length(1,150).WithMessage("نام باید 1 تا 150 حرف داشته باشد");
                RuleFor(e => e.FirstName).Must(Common.Validate.IsPersianName).WithMessage("نام را به زبان فارسی وارد کنید");
                RuleFor(e => e.LastName).Length(1,150).WithMessage("نام خانوادگی 1 تا 150 حرف داشته باشد");
                RuleFor(e => e.LastName).Must(Common.Validate.IsPersianName).WithMessage("نام خانوادگی را به زبان فارسی وارد کنید");
                RuleFor(e => e.Username).NotNull().NotEmpty().WithMessage("نام کاربری نباید خالی باشد");
                RuleFor(e => e.Password).NotNull().NotEmpty().WithMessage("کلمه عبور نباید خالی باشد");
                RuleFor(e => e.Mobile).Must(Common.Validate.IsMobile).WithMessage("شماره تلفن را به درستی وارد کنید");
                RuleFor(e => e.Description).Must(Common.Validate.IsPersianText).WithMessage("توضیحات را به زبان فارسی وارد کنید");
            }
        }
    }
}
