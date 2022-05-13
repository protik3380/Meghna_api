using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EFreshStoreCore.Model.Context;

namespace EFreshStoreCore.Model.Interfaces.Managers
{
    public interface IDeliveryManManager : ICommonManager<DeliveryMan>
    {
        DeliveryMan GetById(long id);
        DeliveryMan GetByUserId(long id);
        bool DoesMobileNoExist(string mobileNo, long userId);
        bool DoesMobileNoExist(string mobileNo);
        DeliveryMan GetByMobileNo(string mobileNo);
        bool DoesNIDExist(string nid, long userId);
        bool DoesNIDExist(string nid);
        bool DoesEmailExist(string email, long userId);
        bool DoesEmailExist(string email);
        ICollection<DeliveryMan> GetAll();
        ICollection<DeliveryMan> GetAllWithMasterDepots();
        ICollection<DeliveryMan> GetActiveDeliveryMen();

    }
}
