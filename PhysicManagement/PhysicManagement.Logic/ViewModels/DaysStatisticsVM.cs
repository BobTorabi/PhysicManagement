using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhysicManagement.Logic.ViewModels
{
   public class DaysStatisticsVM
    {
        public int Today { get; set; }
        public int LastWeek { get; set; }
        public int LastMonth { get; set; }
        public int TotalRecords { get; set; }
    }
}
