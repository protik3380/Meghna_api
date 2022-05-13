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
    public class LPGComboDiscountManager : CommonManager<LPGComboDiscount>, ILPGComboDiscountManager
    {
        public LPGComboDiscountManager() : base(new LPGComboDiscountRepository())
        {
            
        }

        public LPGComboDiscount GetById(long id)
        {
            return GetFirstOrDefault(c => c.Id == id
                                          && !c.IsDeleted);
        }

        public LPGComboDiscount GetValidLpgComboDiscount()
        {
            var lgpDiscount = GetFirstOrDefault(c=> !c.IsDeleted && c.IsActive);
            return lgpDiscount.Validity.Date >= DateTime.Now.Date ? lgpDiscount : null;
        }

        public LPGComboDiscount GetLpgComboDiscount()
        {
            return GetFirstOrDefault(c => !c.IsDeleted);
        }

        public LPGComboDiscount GetActiveDiscount()
        {
            return GetFirstOrDefault(c => c.IsActive
                                                 && !c.IsDeleted);
        }

        public ICollection<LPGComboDiscount> GetAll()
        {
            return Get(c => !c.IsDeleted);
        }

        public ICollection<LPGComboDiscount> GetAllActive()
        {
            return Get(c => !c.IsDeleted && c.IsActive);
        }
    }
}
