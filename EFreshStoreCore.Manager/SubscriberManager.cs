using System.Collections.Generic;
using EFreshStoreCore.Model.Context;
using EFreshStoreCore.Model.Interfaces.Managers;
using EFreshStoreCore.Repository;

namespace EFreshStoreCore.Manager
{
    public class SubscriberManager : CommonManager<Subscriber>, ISubscriberManager
    {
        public SubscriberManager() : base(new SubscriberRepository())
        {
        }

        public bool IsEmailExist(string email)
        {
            Subscriber subscriber = GetFirstOrDefault(c => c.Email == email);
            if (subscriber != null)
            {
                return true;
            }
                return false;
        }

        public ICollection<Subscriber> Get()
        {
            return Get(c => !c.IsDeleted);
        }
    }
}
