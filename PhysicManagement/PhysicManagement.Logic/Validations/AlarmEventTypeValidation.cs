using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhysicManagement.Logic.Validations
{
    public class AlarmEventTypeValidation 
    {
        public class AlarmEventTypeEntityValidate : AbstractValidator<Model.AlarmEventType>
        {

            public AlarmEventTypeEntityValidate()
            {
                
                RuleFor(e => e.Title).NotEmpty().WithMessage("عنوان اجباری است");
                RuleFor(e => e.EnglishTitle).NotEmpty().WithMessage("عنوان باید به انگلیسی وارد شود");

            }
        }
    }
}
