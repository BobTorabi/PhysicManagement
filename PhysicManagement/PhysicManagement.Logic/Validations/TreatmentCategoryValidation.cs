using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace PhysicManagement.Logic.Validations
{
   public class TreatmentCategoryValidation
    {
        #region Treatment Category Validation
        public class TreatmentCategoryEntityValidate : AbstractValidator<Model.TreatmentCategory>
        {
            public TreatmentCategoryEntityValidate()
            {
                RuleFor(x => x.Title).Length(1, 250).WithMessage("پر کردن عنوان اجباری است ");
                RuleFor(x => x.Description).MaximumLength(200).WithMessage("متنی که نوشته اید نمی تواند بیشتر از 200 حرف باشد");
            }
        }
        #endregion
        #region TreatmentCategory Service Validation
        public class TreatmentCategoryServiceEntityValidate : AbstractValidator<Model.TreatmentCategoryService>
        {
            public TreatmentCategoryServiceEntityValidate()
            {
                RuleFor(x => x.Title).Length(1, 250).WithMessage("پر کردن عنوان اجباری است ");
                RuleFor(x => x.Description).MaximumLength(200).WithMessage("متنی که نوشته اید نمی تواند بیشتر از 200 حرف باشد");
            }
        }
        #endregion


    }
}
