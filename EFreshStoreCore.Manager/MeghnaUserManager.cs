using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using EFreshStoreCore.Model.Context;
using EFreshStoreCore.Model.Enums;
using EFreshStoreCore.Model.Interfaces.Managers;
using EFreshStoreCore.Repository;

namespace EFreshStoreCore.Manager
{
    public class MeghnaUserManager : CommonManager<MeghnaUser>, IMeghnaUserManager
    {
        public MeghnaUserManager() : base(new MeghnaUserRepository())
        {
        }

        
        public MeghnaUser GetById(long id)
        {
            return GetFirstOrDefault(c => c.Id == id&&!c.IsDeleted.Value&&c.IsActive.Value,
                c => c.User);
        }

        public bool GetByUserEmail(string email)
        {
            MeghnaUser user = GetFirstOrDefault(c => c.Email == email && !c.IsDeleted.Value && c.IsActive.Value);
            if (user == null)
            {
                return false;
            }
            return true;
        }

        public MeghnaUser GetByUserId(long id)
        {
            return GetFirstOrDefault(c => c.UserId == id && !c.IsDeleted.Value && c.IsActive.Value,
                c => c.User,
                c=>c.MeghnaDepartment,
                c=>c.MeghnaDesignation);
        }

        public override bool Add(MeghnaUser entity)
        {
            entity.User = new User
            {
                Username = entity.Email,
                Password = "123456",
                IsActive = true,
                IsDeleted = false,
                UserTypeId = (int)UserTypeEnum.MeghnaUser
            };
            return base.Add(entity);
        }

        public override bool Add(ICollection<MeghnaUser> entities)
        {
            foreach (MeghnaUser meghnaUser in entities)
            {
                meghnaUser.User = new User
                {
                    Username = meghnaUser.Email,
                    Password = "123456",
                    IsActive = true,
                    IsDeleted = false,
                    UserTypeId = (int)UserTypeEnum.MeghnaUser
                };
            }
            return base.Add(entities);
        }

        public ICollection<MeghnaUser> GetAll()
        {
           return Get(c => c.IsActive.HasValue
                           && c.IsActive.Value
                           && c.IsDeleted.HasValue
                           && !c.IsDeleted.Value,
               c=>c.MeghnaDepartment,
                            c=>c.MeghnaDesignation);
        }

        public int CountMeghnaUser()
        {
            return Get(c => c.IsActive.HasValue
                           && c.IsActive.Value
                           && c.IsDeleted.HasValue
                           && !c.IsDeleted.Value).Count;
        }
    }
}