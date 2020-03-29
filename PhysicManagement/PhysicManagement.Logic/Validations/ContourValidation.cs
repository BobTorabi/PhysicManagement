﻿using FluentValidation;

namespace PhysicManagement.Logic.Validations
{
    public class ContourValidation
    {
        public class ContourEntityValidate : AbstractValidator<Model.Contour>
        {
            public ContourEntityValidate()
            {
                RuleFor(e => e.Id).NotNull().NotEmpty().WithMessage("شناسه نباید مقدار خالی داشته باشد");
                RuleFor(e => e.Description).NotNull().NotEmpty().WithMessage("توضیحات نباید خالی باشد");
                RuleFor(e => e.Description).MaximumLength(200).WithMessage("متنی که نوشته اید نمی تواند بیشتر از 200 حرف داشته باشد");
            }
        }

        public class ContourDetailsEntityValidate : AbstractValidator<Model.ContourDetails>
        {
            public ContourDetailsEntityValidate()
            {
                RuleFor(e => e.Id).NotNull().NotEmpty().WithMessage("شناسه نباید مقدار خالی داشته باشد");
                RuleFor(e => e.Description).NotNull().NotEmpty().WithMessage("توضیحات نباید خالی باشد");
            }
        }
    }
}
