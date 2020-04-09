using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhysicManagement.Logic.Validations
{
   public class CancerValidation
    {
        public class CancerEntityValidate : AbstractValidator<Model.Cancer> {

            public CancerEntityValidate() {
                RuleFor(e => e.Id).NotNull().WithMessage("شناسه نمی تواند مقدار خالی داشته باشد");
                RuleFor(e => e.Title).NotEmpty().WithMessage("عنوان اجباری است");
                RuleFor(e => e.EnglishTitle).NotEmpty().WithMessage("عنوان باید به انگلیسی وارد شود");

            }
        }
        public class CancerOAREntityValidate : AbstractValidator<Model.CancerOAR>
        {

            public CancerOAREntityValidate()
            {
                RuleFor(e => e.OrganTitle).NotEmpty().WithMessage("");
            }
        }
        public class CancerTargetEntityValidate : AbstractValidator<Model.CancerTargets>
        {

            public CancerTargetEntityValidate()
            {
                RuleFor(e => e.Title).NotEmpty().WithMessage("");
            }
        }
    }
}
