using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFreshStoreCore.Model.Context.ViewModels
{
    public class SalesOverTimeVm
    {
        public int ReportType { get; set; }
        public int FromMonth { get; set; }
        public int ToMonth { get; set; }
        public int Year { get; set; }
        public int FromYear { get; set; }
        public int ToYear { get; set; }
    }
}
