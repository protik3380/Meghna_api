using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EFreshStoreCore.Model.Context;
using EFreshStoreCore.Model.Interfaces.Managers;
using EFreshStoreCore.Repository;

namespace EFreshStoreCore.Manager
{
    public class PaymentDetailManager : CommonManager<PaymentDetail>, IPaymentDetailManager
    {
        public PaymentDetailManager() : base(new PaymentDetailRepository())
        {
            
        }

        public PaymentDetail GetByOrderNo(string orderNo)
        {
            return GetFirstOrDefault(c => c.tran_id.ToLower() == orderNo.ToLower());
        }
    }
}
