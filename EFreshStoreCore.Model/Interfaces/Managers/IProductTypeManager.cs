using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EFreshStoreCore.Model.Context;

namespace EFreshStoreCore.Model.Interfaces.Managers
{
    public interface IProductTypeManager : ICommonManager<ProductType>
    {
        ICollection<ProductType> GetAll();
        ICollection<ProductType> GetActiveProductTypes();
        ProductType GetById(long id);
    }
}
