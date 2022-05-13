using System.Collections.Generic;
using EFreshStoreCore.Model.Context;
using EFreshStoreCore.Model.Enums;
using EFreshStoreCore.Model.Interfaces.Managers;
using EFreshStoreCore.Repository;

namespace EFreshStoreCore.Manager
{
    public class CustomerManager : CommonManager<Customer>, ICustomerManager
    {
        public CustomerManager() : base(new CustomerRepository())
        {
        }
        public Customer GetById(long id)
        {
            return GetFirstOrDefault(c => c.Id == id&&c.IsDeleted.HasValue&&!c.IsDeleted.Value,
                c => c.User);
        }

        public Customer GetByUserId(long id)
        {
            return GetFirstOrDefault(c => c.UserId == id&&c.IsDeleted.HasValue&&!c.IsDeleted.Value,
                c => c.User);
        }

        public bool GetByUserEmail(string email)
        {
            Customer customer = GetFirstOrDefault(c => c.Email == email&&!c.IsDeleted.Value&&c.IsDeleted.HasValue);
            if (customer == null)
            {
                return false;
            }
            return true;
        }

        //public override bool Add(Customer entity)
        //{
        //    entity.User = new User
        //    {
        //        Username = entity.Email,
        //        Password = "123",
        //        IsActive = true,
        //        IsDeleted = false,
        //        UserTypeId = (int)UserTypeEnum.Customer
        //    };
        //    return base.Add(entity);
        //}

        public ICollection<Customer> Get()
        {
            return Get(c => c.IsDeleted.HasValue
                            && !c.IsDeleted.Value,
                            c => c.User);
        }

        public int CountCustomer()
        {
            return Get(c => c.IsDeleted.HasValue
                            && !c.IsDeleted.Value).Count;
        }
    }
}