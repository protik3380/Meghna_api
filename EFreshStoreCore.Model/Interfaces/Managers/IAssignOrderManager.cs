using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EFreshStoreCore.Model.Context;

namespace EFreshStoreCore.Model.Interfaces.Managers
{
    public interface IAssignOrderManager : ICommonManager<AssignOrder>
    {
        AssignOrder GetById(long id);
        AssignOrder GetByOrderId(long id);
        bool DoesAssignOrderExist(long orderId);
        ICollection<AssignOrder> GetAll();
        ICollection<AssignOrder> GetByDeliveryManId(long id);
        AssignOrder GetDeliveryManByOrderId(long orderId);
    }
}
