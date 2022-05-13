using System.Collections;
using EFreshStoreCore.Model.Context;
using System.Collections.Generic;

namespace EFreshStoreCore.Model.Interfaces.Managers
{
    public interface ICustomerManager : ICommonManager<Customer>
    {
        Customer GetById(long id);
        Customer GetByUserId(long id);
        bool GetByUserEmail(string email);
        ICollection<Customer> Get();
        int CountCustomer();
    }
}
