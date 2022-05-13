using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EFreshStoreCore.Model.Context;

namespace EFreshStoreCore.Model.Interfaces.Managers
{
    public interface ILPGComboDiscountManager : ICommonManager<LPGComboDiscount>
    {
        LPGComboDiscount GetById(long id);
        LPGComboDiscount GetValidLpgComboDiscount();
        LPGComboDiscount GetLpgComboDiscount();
        LPGComboDiscount GetActiveDiscount();
        ICollection<LPGComboDiscount> GetAll();
        ICollection<LPGComboDiscount> GetAllActive();
    }
}
