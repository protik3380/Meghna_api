using System.Collections.Generic;
using EFreshStoreCore.Model.Context;

namespace EFreshStoreCore.Model.Interfaces.Managers
{
    public interface IMeghnaDesignationManager : ICommonManager<MeghnaDesignation>
    {
        ICollection<MeghnaDesignation> GetAll();
        ICollection<MeghnaDesignation> GetActiveDesignations();
        MeghnaDesignation GetById(long id);
        bool DoesMeghnaDesignationExist(string name);
        bool SoftDelete(MeghnaDesignation entity);
    }
}
