using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EFreshStoreCore.Model.Context;
using EFreshStoreCore.Model.Enums;
using EFreshStoreCore.Model.Interfaces.Managers;
using EFreshStoreCore.Repository;

namespace EFreshStoreCore.Manager
{
   public class AssignOrderManager : CommonManager<AssignOrder>, IAssignOrderManager
    {
        public AssignOrderManager() : base(new AssignOrderRepository())
        {
        }
        public AssignOrder GetById(long id)
        {
            return GetFirstOrDefault(c => c.Id == id);
        }

        public AssignOrder GetByOrderId(long id)
        {
            return GetFirstOrDefault(c => c.OrderId == id);
        }

        public bool DoesAssignOrderExist(long orderId)
       {
            AssignOrder assignOrder = GetFirstOrDefault(c => c.OrderId== orderId);
            return assignOrder != null;
        }

       public ICollection<AssignOrder> GetAll()
       {
           return Get(c=>(c.Order.OrderStateId.HasValue &&c.Order.OrderStateId==(long)OrderStatusEnum.OnProcessing || c.Order.OrderStateId==(long)OrderStatusEnum.Shipped)  ||(c.IsDelivered.HasValue && !c.IsDelivered.Value),c => c.Order ,c=>c.DeliveryMan);
       }

       public ICollection<AssignOrder> GetByDeliveryManId(long id)
       {
           return Get(c => c.DeliveryManId == id, c => c.Order, c=>c.Order.OrderDetails).ToList();
       }

       public AssignOrder GetDeliveryManByOrderId(long orderId)
       {
           return GetFirstOrDefault(c => c.OrderId == orderId,
               c => c.DeliveryMan);
       }
    }
}
