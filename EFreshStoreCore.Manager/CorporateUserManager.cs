using System;
using System.Collections.Generic;
using System.Linq;
using EFreshStoreCore.Model.Context;
using EFreshStoreCore.Model.Enums;
using EFreshStoreCore.Model.Interfaces.Managers;
using EFreshStoreCore.Repository;

namespace EFreshStoreCore.Manager
{
    public class CorporateUserManager : CommonManager<CorporateUser>, ICorporateUserManager
    {
        public CorporateUserManager() : base(new CorporateUserRepository())
        {
        }

        public CorporateUser GetById(long id)
        {
            return GetFirstOrDefault(c => c.Id == id,
                c => c.CorporateContract);
        }

        public bool GetByUserEmail(string email)
        {
            CorporateUser user = GetFirstOrDefault(c => c.Email == email, c => c.User);
            if (user == null)
            {
                return false;
            }
            return true;
        }

        public CorporateUser GetByUserId(long id)
        {
            return GetFirstOrDefault(c => c.UserId == id,
                c => c.CorporateContract,
                c => c.CorporateDepartment,
                c => c.CorporateDesignation);
        }

        public override bool Add(CorporateUser entity)
        {
            entity.User = new User
            {
                Username = entity.Email,
                Password = "123456",
                IsActive = true,
                IsDeleted = false,
                UserTypeId = (int)UserTypeEnum.Corporate
            };
            return base.Add(entity);
        }

        public override bool Add(ICollection<CorporateUser> entities)
        {
            foreach (CorporateUser corporateUser in entities)
            {
                corporateUser.User = new User
                {
                    Username = corporateUser.Email,
                    Password = "123456",
                    IsActive = true,
                    IsDeleted = false,
                    UserTypeId = (int)UserTypeEnum.Corporate
                };
            }

            return base.Add(entities);
        }

        public ICollection<CorporateUser> GetAll()
        {
            return Get(c => c.IsActive.HasValue
                            && c.IsActive.Value
                            && c.IsDeleted.HasValue
                            && !c.IsDeleted.Value, c => c.CorporateContract);
        }

        public ICollection<CorporateUser> GetByCorporateId(long id)
        {
            return Get(c => c.CorporateContractId == id
                            && c.IsActive.HasValue
                            && c.IsActive.Value
                            && c.IsDeleted.HasValue
                            && !c.IsDeleted.Value,
                   c => c.CorporateContract,
                                c => c.CorporateDepartment,
                                c => c.CorporateDesignation);
        }

        public int CountCorporateUser()
        {
            var totalCorporateUsers = Get(c => c.IsActive.HasValue
                                               && c.IsActive.Value
                                               && c.IsDeleted.HasValue
                                               && !c.IsDeleted.Value).Count;
            return totalCorporateUsers;
        }
    }
}