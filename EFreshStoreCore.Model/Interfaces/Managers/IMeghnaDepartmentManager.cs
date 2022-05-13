using System.Collections.Generic;
using EFreshStoreCore.Model.Context;

namespace EFreshStoreCore.Model.Interfaces.Managers
{
    public interface IMeghnaDepartmentManager : ICommonManager<MeghnaDepartment>
    {
        ICollection<MeghnaDepartment> GetAll();
        ICollection<MeghnaDepartment> GetActiveDepartments();
        MeghnaDepartment GetById(long id);
        bool DoesMeghnaDepartmentExist(string name);
        bool SoftDelete(MeghnaDepartment entity);
    }
}
