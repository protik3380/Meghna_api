using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFreshStoreCore.Model.Context.ViewModels
{
    public class OrderVsMonthOrYearVm
    {
        public long Month { get; set; }
        public long Year { get; set; }
        public double OrderRate { get; set; }
    }
}
