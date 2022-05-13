using System.Collections.Generic;
using EFreshStoreCore.Model.Context;

namespace EFreshStoreCore.Model.Interfaces.Managers
{
    public interface ISubscriberManager : ICommonManager<Subscriber>
    {
        bool IsEmailExist(string email);
        ICollection<Subscriber> Get();
    }
   
}
