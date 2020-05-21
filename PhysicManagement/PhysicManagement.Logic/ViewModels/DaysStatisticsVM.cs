using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhysicManagement.Logic.ViewModels
{
   public class DaysStatisticsVM
    {
        public decimal UnsetRecord { get; set; }
        public decimal Today { get; set; }
        public decimal LastWeek { get; set; }
        public decimal LastMonth { get; set; }
        public decimal TotalRecords { get; set; }
    }
}
