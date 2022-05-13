using System;
using System.Collections.Generic;
using EFreshStoreCore.Model.Context;
using EFreshStoreCore.Model.Interfaces.Managers;
using EFreshStoreCore.Repository;

namespace EFreshStoreCore.Manager
{
    public class ThanaWiseMasterDepotManager : CommonManager<ThanaWiseMasterDepot>, IThanaWiseMasterDepotManager
    {
        public ThanaWiseMasterDepotManager() : base(new ThanaWiseMasterDepotRepository())
        {
        }

        public ICollection<ThanaWiseMasterDepot> GetAll()
        {
            return Get(c=>c.ThanaId.HasValue&&c.MasterDepotId.HasValue,c => c.MasterDepot, c => c.Thana);
        }

        public ICollection<ThanaWiseMasterDepot> GetByMasterDepotId(long id)
        {
            return Get(c => c.MasterDepotId == id, c => c.Thana, c => c.MasterDepot,c=>c.Thana.District);
        }

        public ThanaWiseMasterDepot GetByThana(long thanaId)
        {
            return GetFirstOrDefault(c => c.Id == thanaId, c => c.MasterDepot, c => c.Thana);
        }


        public bool IsExistMasterDepot(ThanaWiseMasterDepot thanaWiseMasterDepot)
        {
            var masterDepot = GetFirstOrDefault(c => c.MasterDepotId == thanaWiseMasterDepot.MasterDepotId && c.ThanaId == thanaWiseMasterDepot.ThanaId);
            if (masterDepot != null)
            {
                return true;
            }
            return false;
        }
    }
}