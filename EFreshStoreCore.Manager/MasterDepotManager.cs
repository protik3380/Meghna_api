using System;
using System.Collections.Generic;
using System.Linq;
using EFreshStoreCore.Model.Context;
using EFreshStoreCore.Model.Enums;
using EFreshStoreCore.Model.Interfaces.Managers;
using EFreshStoreCore.Repository;

namespace EFreshStoreCore.Manager
{
    public class MasterDepotManager : CommonManager<MasterDepot>, IMasterDepotManager
    {
        public MasterDepotManager() : base(new MasterDepotRepository())
        {
        }

        public override bool Add(MasterDepot entity)
        {
            entity.User = new User
            {
                Username = entity.Email,
                Password = "123456",
                IsActive = true,
                IsDeleted = false,
                UserTypeId = (int)UserTypeEnum.MasterDepotUser
            };
            return base.Add(entity);
        }

        public ICollection<MasterDepot> GetAll()
        {
            return Get(c => !c.IsDeleted);
        }

        public ICollection<MasterDepot> GetActiveMasterDepots()
        {
            return Get(c => !c.IsDeleted && c.IsActive).OrderBy(c => c.Name).ToList();
        }

        public MasterDepot GetById(long id)
        {
            return GetFirstOrDefault(c => c.Id == id && !c.IsDeleted, c=> c.ThanaWiseMasterDepots);
        }

        public bool DoesMasterDepotEmailExist(string email)
        {
            MasterDepot masterDepot = GetFirstOrDefault(c => c.Email.ToLower().Equals(email.ToLower())
                                                             && !c.IsDeleted);
            return masterDepot != null;
        }

        public MasterDepot GetByUserId(long id)
        {
            var masterDepotUser = GetFirstOrDefault(c => c.UserId == id, o=> o.Orders);
            return masterDepotUser;
        }

        public int CountMasterDepot()
        {
            return Get(c => c.IsActive && !c.IsDeleted).Count;
        }

        public MasterDepot GetByThanaAndProduct(long thanaId)
        {
            IThanaWiseMasterDepotManager _thanaWiseMasterDepotManager = new ThanaWiseMasterDepotManager();
            IMasterDepotManager masterDepotManager = new MasterDepotManager();

            List<ThanaWiseMasterDepot> thanaWiseMasterDepots;
            thanaWiseMasterDepots = (List<ThanaWiseMasterDepot>) _thanaWiseMasterDepotManager.GetAll();
            List<MasterDepot> masterDepots = new List<MasterDepot>();
            MasterDepot aDepot = new MasterDepot();
            foreach (var thanaWiseMasterDepot in thanaWiseMasterDepots)
            {
                if (thanaWiseMasterDepot.ThanaId == thanaId)
                {
                    masterDepots.Add(masterDepotManager.GetById((long) thanaWiseMasterDepot.MasterDepotId));
                    aDepot = masterDepots.FirstOrDefault();
                    return aDepot;
                }
            }
            return aDepot;
        }
    }
}