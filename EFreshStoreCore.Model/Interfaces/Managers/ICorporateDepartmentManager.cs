using System.Collections.Generic;
using EFreshStoreCore.Model.Context;

namespace EFreshStoreCore.Model.Interfaces.Managers
{
    public interface ICorporateDepartmentManager : ICommonManager<CorporateDepartment>
    {
        ICollection<CorporateDepartment> GetAll();
        ICollection<CorporateDepartment> GetActiveDepartments();
        CorporateDepartment GetById(long id);
        bool DoesCorporateDepartmentExist(string name);
        bool SoftDelete(CorporateDepartment entity);
    }
}
