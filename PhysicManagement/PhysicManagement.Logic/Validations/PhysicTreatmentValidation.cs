using FluentValidation;

namespace PhysicManagement.Logic.Validations
{
    public class PhysicTreatmentValidation
    {
        #region PhysicTreatmetnValidation
        public class PhysicTreatmentEntityValidate : AbstractValidator<Model.PhysicTreatment>
        {

            public PhysicTreatmentEntityValidate()
            {
                RuleFor(x => x.Id).NotNull().NotEmpty().WithMessage("شناسه نمی تواند مقدار خالی داشته باشد ");
                RuleFor(x => x.MedicalRecordId).NotNull().NotEmpty().WithMessage("پر کردن این فیلد اجباری است ");
                RuleFor(x => x.PhaseNumber).NotNull().NotEmpty().WithMessage("پر کردن این فیلد اجباری است ");
                RuleFor(x => x.PhysicTreatmentPlan).NotNull().NotEmpty().WithMessage("پر کردن این فیلد اجباری است ");
                RuleFor(x => x.ActionDate).NotNull().NotEmpty().WithMessage("پر کردن این فیلد اجباری است ");
                RuleFor(x => x.ActionUser).NotNull().NotEmpty().WithMessage("پر کردن این فیلد اجباری است ");
            }
        }
        #endregion
        #region PhysicTreatmentPlanValidation
        public class PhysicTreatmentPlanEntityValidate : AbstractValidator<Model.PhysicTreatmentPlan>
        {

            public PhysicTreatmentPlanEntityValidate()
            {
                RuleFor(x => x.Id).NotNull().NotEmpty().WithMessage("شناسه نمی تواند مقدار خالی داشته باشد ");
                RuleFor(x => x.IsAcceptedByDoctor).NotNull().NotEmpty().WithMessage("پر کردن این فیلد اجباری است ");
                RuleFor(x => x.IsAcceptedByPhysic).NotNull().NotEmpty().WithMessage("پر کردن این فیلد اجباری است ");
                RuleFor(x => x.PhysicTreatment).NotNull().NotEmpty().WithMessage("پر کردن این فیلد اجباری است ");
                RuleFor(x => x.PhysicTreatmentId).NotNull().NotEmpty().WithMessage("پر کردن این فیلد اجباری است ");
                RuleFor(x => x.PhysicTreatmentPlanHostory).NotNull().NotEmpty().WithMessage("پر کردن این فیلد اجباری است ");
                RuleFor(x => x.PlannedDose).NotNull().NotEmpty().WithMessage("پر کردن این فیلد اجباری است ");
                RuleFor(x => x.Reserve1).NotNull().NotEmpty().WithMessage("پر کردن این فیلد اجباری است ");
                RuleFor(x => x.Reserve2).NotNull().NotEmpty().WithMessage("پر کردن این فیلد اجباری است ");
                RuleFor(x => x.AcceptedDoctorUser).NotNull().NotEmpty().WithMessage("پر کردن این فیلد اجباری است ");
                RuleFor(x => x.ActionDate).NotNull().NotEmpty().WithMessage("پر کردن این فیلد اجباری است ");
                RuleFor(x => x.ActionUser).NotNull().NotEmpty().WithMessage("پر کردن این فیلد اجباری است ");
                RuleFor(x => x.CancerOARId).NotNull().NotEmpty().WithMessage("پر کردن این فیلد اجباری است ");
                RuleFor(x => x.Evaluation).NotNull().NotEmpty().WithMessage("پر کردن این فیلد اجباری است ");
                RuleFor(x => x.HadContour).NotNull().NotEmpty().WithMessage("پر کردن این فیلد اجباری است ");
            }
        }
        #endregion
        #region PhysicTreatmentPlanHOstory
        public class PhysicTreatmentPlanHostoryEntityValidate : AbstractValidator<Model.PhysicTreatmentPlanHostory>
        {
            public PhysicTreatmentPlanHostoryEntityValidate()
            {
                RuleFor(x => x.Id).NotNull().NotEmpty().WithMessage("شناسه نمی تواند مقدار خالی داشته باشد ");
                RuleFor(x => x.ActionUser).NotNull().NotEmpty().WithMessage("پر کردن این فیلد اجباری است ");
                RuleFor(x => x.ChangeDate).NotNull().NotEmpty().WithMessage("پر کردن این فیلد اجباری است ");
                RuleFor(x => x.HasAlarm).NotNull().NotEmpty().WithMessage("پر کردن این فیلد اجباری است ");
                RuleFor(x => x.IsDoctor).NotNull().NotEmpty().WithMessage("پر کردن این فیلد اجباری است ");
                RuleFor(x => x.Note).NotNull().NotEmpty().WithMessage("پر کردن این فیلد اجباری است ");
                RuleFor(x => x.PhysicTreatmentPlan).NotNull().NotEmpty().WithMessage("پر کردن این فیلد اجباری است ");
                RuleFor(x => x.PhysicTreatmentPlanId).NotNull().NotEmpty().WithMessage("پر کردن این فیلد اجباری است ");


            }
        }
        #endregion

    }
}
