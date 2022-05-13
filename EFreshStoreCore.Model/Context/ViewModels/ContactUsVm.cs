using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFreshStoreCore.Model.Context.ViewModels
{
    public class ContactUsVm
    {
        public string Subject { get; set; }
        public string Name { get; set; }
        public string MobileNo { get; set; }
        public string Message { get; set; }
        public string Email { get; set; }
    }
}
