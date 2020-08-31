using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhysicManagement.Logic.Enums
{
    public enum TreatmentProcessType
    {
        /// <summary>
        /// پذیرش بیمار
        /// </summary>
        Reciption = 1,
        /// <summary>
        /// تصویربرداری شده
        /// </summary>
        CTScanMRIEntry = 2,
        /// <summary>
        /// انتخاب نرم افزار
        /// </summary>
        SoftwareEntry =3,
        /// <summary>
        /// کانتورینگ
        /// </summary>
        Contouring = 4,
        /// <summary>
        /// تجویز درمان
        /// </summary>
        PrescribeTreatment = 5,
        /// <summary>
        /// تأیید پلن فیزیک
        /// </summary>
        PhysicsPlan = 6,
        /// <summary>
        /// کارتابل / در حال درمان
        /// </summary>
        UnderTreatment = 7,
        /// <summary>
        /// اتمام دوره درمان
        /// </summary>
        TreatmentCompletion = 8
    }
}
