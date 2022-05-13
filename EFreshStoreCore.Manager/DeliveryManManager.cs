using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EFreshStoreCore.Model.Context;
using EFreshStoreCore.Model.Interfaces.Managers;
using EFreshStoreCore.Repository;

namespace EFreshStoreCore.Manager
{
    public class DeliveryManManager : CommonManager<DeliveryMan>, IDeliveryManManager
    {
        public DeliveryManManager() : base(new DeliveryManRepository())
        {
        }

        public DeliveryMan GetById(long id)
        {
            return GetFirstOrDefault(c => c.Id == id
                                          && !c.IsDeleted , c=> c.Thana, c => c.Thana.District, c=> c.User);
        }
        public DeliveryMan GetByUserId(long id)
        {
            return GetFirstOrDefault(c => c.UserId == id
                                          && !c.IsDeleted , c=> c.Thana, c => c.Thana.District, c=> c.User);
        }

        public bool DoesMobileNoExist(string mobileNo)
        {
            DeliveryMan deliveryMan = GetFirstOrDefault(c => c.MobileNo.Equals(mobileNo)
                                                 && !c.IsDeleted);
            return deliveryMan != null;
        }
        public bool DoesMobileNoExist(string mobileNo, long userId)
        {
            DeliveryMan deliveryMan = GetFirstOrDefault(c => c.MobileNo.Equals(mobileNo)
                                                             && !c.IsDeleted && c.Id != userId);
            return deliveryMan != null;
        }

        public bool DoesNIDExist(string nid)
        {
            DeliveryMan deliveryMan = GetFirstOrDefault(c => !string.IsNullOrEmpty(c.NID) && c.NID.Equals(nid)
                                                             && !c.IsDeleted);
            return deliveryMan != null;
        }
        public bool DoesNIDExist(string nid, long userId)
        {
            DeliveryMan deliveryMan = GetFirstOrDefault(c => !string.IsNullOrEmpty(c.NID) && c.NID.Equals(nid)
                                                             && !c.IsDeleted && c.Id != userId);
            return deliveryMan != null;
        }

        public bool DoesEmailExist(string email)
        {
            DeliveryMan deliveryMan = GetFirstOrDefault(c => !string.IsNullOrEmpty(c.Email) && c.Email.Equals(email)
                                                             && !c.IsDeleted);
            return deliveryMan != null;
        }
        public bool DoesEmailExist(string email, long userId)
        {
            DeliveryMan deliveryMan = GetFirstOrDefault(c => !string.IsNullOrEmpty(c.Email) && c.Email.Equals(email)
                                                             && !c.IsDeleted && c.Id != userId);
            return deliveryMan != null;
        }

        public DeliveryMan GetByMobileNo(string mobileNo)
        {
            return GetFirstOrDefault(c => c.MobileNo.Equals(mobileNo)
                                               && !c.IsDeleted);
        }

        public ICollection<DeliveryMan> GetAll()
        {
            return Get(c => !c.IsDeleted,
             c=>c.Thana,
             c=>c.Thana.District);
        }

        public ICollection<DeliveryMan> GetAllWithMasterDepots()
        {
            return Get(c => !c.IsDeleted,
                c => c.Thana,
                c => c.Thana.District, c => c.MasterDepotDeliveryMen, c=> c.MasterDepotDeliveryMen.Select(x => x.MasterDepot));
        }
        public ICollection<DeliveryMan> GetActiveDeliveryMen()
        {
            return Get(c => !c.IsDeleted && c.IsActive, c => c.Thana,
                c => c.Thana.District);
        }
    }
}
