using FluentValidation;

namespace PhysicManagement.Logic.Validations
{
    public class ContourValidation
    {
        public class ContourEntityValidate : AbstractValidator<Model.Contour>
        {
            public ContourEntityValidate()
            {
            }
        }

        public class ContourDetailsEntityValidate : AbstractValidator<Model.ContourDetails>
        {
            public ContourDetailsEntityValidate()
            {
              
            }
        }
    }
}
