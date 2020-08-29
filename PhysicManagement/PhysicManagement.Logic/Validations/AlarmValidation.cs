using FluentValidation;

namespace PhysicManagement.Logic.Validations
{
    #region AlarmValidation
    public class AlarmValidation
    {
        
        public class AlarmEntityValidate : AbstractValidator<Model.Alarm>
        {
            public AlarmEntityValidate()
            {
                
            }
        }
    }
    #endregion

    #region DoctorAlarmValidation
    public class DoctorAlarmValidation
    {

        public class DoctorAlarmEntityValidate : AbstractValidator<Model.DoctorAlarm>
        {
            public DoctorAlarmEntityValidate()
            {

            }
        }
    }
    #endregion

    #region ResidentAlarmValidation
    public class ResidentAlarmValidation
    {

        public class ResidentAlarmEntityValidate : AbstractValidator<Model.ResidentAlarm>
        {
            public ResidentAlarmEntityValidate()
            {

            }
        }
    }
    #endregion

    #region PhysicUserAlarmValidation
    public class PhysicUserAlarmValidation
    {

        public class PhysicUserAlarmEntityValidate : AbstractValidator<Model.PhysicUserAlarm>
        {
            public PhysicUserAlarmEntityValidate()
            {

            }
        }
    }
    #endregion

    #region AlarmConfigValidation
    public class AlarmConfigValidation
    {

        public class AlarmConfigEntityValidate : AbstractValidator<Model.AlarmConfig>
        {
            public AlarmConfigEntityValidate()
            {

            }
        }
    }
    #endregion

}
