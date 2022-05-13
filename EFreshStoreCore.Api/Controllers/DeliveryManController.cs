using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using EFreshStoreCore.Api.Utility;
using EFreshStoreCore.Manager;
using EFreshStoreCore.Model.Context;
using EFreshStoreCore.Model.Context.ViewModels;
using EFreshStoreCore.Model.Dtos;
using EFreshStoreCore.Model.Enums;
using EFreshStoreCore.Model.Interfaces.Managers;

namespace EFreshStoreCore.Api.Controllers
{
    public class DeliveryManController : ApiController
    {
        private readonly IDeliveryManManager _deliveryManManager;
        private readonly IMasterDepotManager _masterDepotManager;
        private readonly IMasterDepotDeliveryManManager _masterDepotDeliveryManManager;
        private readonly IUserManager _userManager;
        private readonly IOrderManager _orderManager;
        private readonly IOrderDetailManager _orderDetailManager;
        private readonly IAssignOrderManager _assignOrderManager;

        public DeliveryManController()
        {
            _deliveryManManager = new DeliveryManManager();
            _masterDepotManager = new MasterDepotManager();
            _masterDepotDeliveryManManager = new MasterDepotDeliveryManManager();
            _userManager = new UserManager();
            _orderManager = new OrderManager();
            _orderDetailManager = new OrderDetailManager();
            _assignOrderManager = new AssignOrderManager();
        }

        public IHttpActionResult GetAll()
        {
            try
            {
                var deliveryMen = _deliveryManManager.GetAll();
                if (deliveryMen == null) return NotFound();
                return Ok(deliveryMen.OrderBy(c => c.Name).ToList());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        public IHttpActionResult GetAllWithMasterDepots(long? id)
        {
            try
            {
                if (id == null)
                {
                    var deliveryMen = _deliveryManManager.GetAllWithMasterDepots();
                    if (deliveryMen == null) return NotFound();
                    foreach (var man in deliveryMen)
                    {
                        man.MasterDepotDeliveryMen =
                            man.MasterDepotDeliveryMen.Where(m => m.IsActive && !m.IsDeleted).ToList();
                    }
                    return Ok(deliveryMen.OrderBy(c => c.Name).ToList());
                }
                var masterDepotUser = _masterDepotManager.GetByUserId((long)id);
                var masterDepotDeliveryMen = _masterDepotDeliveryManManager.GetDeliveryMenByMasterDepotId(masterDepotUser.Id);
                var deliveryManList = masterDepotDeliveryMen.Select(c => c.DeliveryMan);
                return Ok(deliveryManList.OrderBy(c => c.Name).ToList());


            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "Admin, MasterDepotUser")]
        public IHttpActionResult GetAllActive()
        {
            try
            {
                var deliveryMen = _deliveryManManager.GetActiveDeliveryMen();
                if (deliveryMen == null) return NotFound();
                return Ok(deliveryMen.OrderBy(c => c.Name).ToList());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        public IHttpActionResult GetAllDeliveryManByMasterDepot(long masterDepotId)
        {
            try
            {
                var masterDepot = _masterDepotManager.GetByUserId(masterDepotId);
                if (masterDepot == null)
                {
                    return NotFound();
                }
                var deliveryMen = _masterDepotDeliveryManManager.GetDeliveryMenByMasterDepotId(masterDepot.Id).Select(c => c.DeliveryMan);
                return Ok(deliveryMen.OrderBy(c => c.Name).ToList());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "Admin, MasterDepotUser")]
        public IHttpActionResult GetAllDeliveryManByMasterDepots([FromUri]long[] masterDepotIds)
        {
            try
            {

                var deliveryMen = _masterDepotDeliveryManManager.GetDeliveryMenByMasterDepotIds(masterDepotIds)
                    .GroupBy(x => x.DeliveryManId).Select(d => new DeliveryMan()
                    {
                        Id = d.First(m => m.DeliveryManId == d.Key).DeliveryMan.Id,
                        Email = d.First(m => m.DeliveryManId == d.Key).DeliveryMan.Email,
                        MobileNo = d.First(m => m.DeliveryManId == d.Key).DeliveryMan.MobileNo,
                        Name = d.First(m => m.DeliveryManId == d.Key).DeliveryMan.Name,
                        NID = d.First(m => m.DeliveryManId == d.Key).DeliveryMan.NID,
                        Thana = d.First(m => m.DeliveryManId == d.Key).DeliveryMan.Thana,
                        MasterDepotDeliveryMen = d.ToList()
                    });
                return Ok(deliveryMen.OrderBy(c => c.Name).ToList());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        public IHttpActionResult GetDeliveryMen(long? id)
        {
            try
            {
                if (id == null)
                {
                    var deliveryMen = _deliveryManManager.GetAll();
                    if (deliveryMen == null) return NotFound();
                    return Ok(deliveryMen.OrderBy(c => c.Name).ToList());
                }
                var masterDepotUser = _masterDepotManager.GetByUserId((long)id);
                var masterDepotDeliveryMen = _masterDepotDeliveryManManager.GetDeliveryMenByMasterDepotId(masterDepotUser.Id);
                var deliveryManList = masterDepotDeliveryMen.Select(c => c.DeliveryMan);
                return Ok(deliveryManList.OrderBy(c => c.Name).ToList());


            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        public IHttpActionResult GetById(long id)
        {
            try
            {
                var deliveryMan = _deliveryManManager.GetById(id);
                if (deliveryMan == null) return NotFound();
                deliveryMan.MasterDepotDeliveryMen = _masterDepotDeliveryManManager.GetByDeliveryMenId(id);

                return Ok(deliveryMan);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        public IHttpActionResult GetShortInfoById(long id)
        {
            try
            {
                var deliveryMan = _deliveryManManager.GetById(id);
                if (deliveryMan == null) return NotFound();
                deliveryMan.MasterDepotDeliveryMen = _masterDepotDeliveryManManager.GetByDeliveryMenId(id);
                var masterDepots = "";
                if (deliveryMan.MasterDepotDeliveryMen != null)
                {
                    var names = deliveryMan.MasterDepotDeliveryMen.Select(x => x.MasterDepot).Select(x => x.Name).ToArray();
                    masterDepots = string.Join(",", names);
                }
                var deliveryManToReturn = new DeliveryManShortInfoToReturnDto
                {
                    Id = deliveryMan.Id,
                    Name = deliveryMan.Name,
                    Email = deliveryMan.Email ?? "Not set yet",
                    MobileNo = deliveryMan.MobileNo,
                    Address = deliveryMan.Address ?? "Not set yet",
                    NID = deliveryMan.NID ?? "Not set yet",
                    ImageUrl = deliveryMan.ImageUrl,
                    Thana = deliveryMan.Thana == null ? "Not set yet" : deliveryMan.Thana.Name,
                    District = deliveryMan.Thana.District == null ? "Not set yet" : deliveryMan.Thana.District.Name,
                    MasterDepots = masterDepots
                };

                return Ok(deliveryManToReturn);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "Admin, MasterDepotUser")]
        [HttpPost]
        public IHttpActionResult Create([FromBody] DeliveryManDto aDeliveryMan)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    if (aDeliveryMan.CurrentUserId != null)
                    {
                        var masterDepotUser = _masterDepotManager.GetByUserId((long)aDeliveryMan.CurrentUserId);
                        aDeliveryMan.MasterDepotIds = new[] { masterDepotUser.Id };
                    }
                    if (_deliveryManManager.DoesMobileNoExist(aDeliveryMan.MobileNo))
                    {
                        return BadRequest("Mobile no already exists");
                    }

                    if (_deliveryManManager.DoesEmailExist(aDeliveryMan.Email))
                    {
                        return BadRequest("Email already exists");
                    }
                    if (_deliveryManManager.DoesNIDExist(aDeliveryMan.NID))
                    {
                        return BadRequest("NID already exists");
                    }

                    DeliveryMan deliveryMan = new DeliveryMan();


                    deliveryMan.ImageUrl = GetImageUrlBySavingImage(aDeliveryMan.ImageByte, "Content/img/DeliveryMan/no-image.png");
                    deliveryMan.CreatedBy = aDeliveryMan.CreatedBy;
                    deliveryMan.Name = aDeliveryMan.Name;
                    deliveryMan.NID = aDeliveryMan.NID;
                    deliveryMan.ThanaId = aDeliveryMan.ThanaId;
                    deliveryMan.Email = aDeliveryMan.Email;
                    deliveryMan.MobileNo = aDeliveryMan.MobileNo;
                    deliveryMan.Address = aDeliveryMan.Address;
                    deliveryMan.CreatedOn = DateTime.UtcNow.AddHours(6);
                    deliveryMan.IsActive = aDeliveryMan.IsActive;
                    deliveryMan.IsDeleted = false;
                    deliveryMan.User = new User
                    {
                        Username = aDeliveryMan.MobileNo,
                        Password = aDeliveryMan.User.Password,
                        UserTypeId = (long)UserTypeEnum.DeliveryMan,
                        CreatedBy = aDeliveryMan.CreatedBy,
                        CreatedOn = DateTime.Now,
                        IsDeleted = false,
                        IsActive = true
                    };

                    foreach (var id in aDeliveryMan.MasterDepotIds)
                    {
                        MasterDepotDeliveryMan masterDepotDeliveryMan = new MasterDepotDeliveryMan
                        {
                            MasterDepotId = id,
                            DeliveryManId = deliveryMan.Id,
                            IsActive = true,
                            CreatedOn = DateTime.UtcNow.AddHours(6),
                            CreatedBy = (long)aDeliveryMan.CreatedBy
                        };
                        deliveryMan.MasterDepotDeliveryMen.Add(masterDepotDeliveryMan);
                    }


                    bool isSaved = _deliveryManManager.Add(deliveryMan);
                    if (isSaved)
                    {
                        return Created(new Uri(Request.RequestUri.ToString()), aDeliveryMan);
                    }
                }
                return BadRequest("Could not create delivery man");
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "Admin, MasterDepotUser")]
        [HttpPost]
        public IHttpActionResult Edit([FromBody] DeliveryManDto aDeliveryMan)
        {
            try
            {
                var deliveryMan = _deliveryManManager.GetById(aDeliveryMan.Id);
                if (deliveryMan == null)
                {
                    return NotFound();
                }
                if (_deliveryManManager.DoesMobileNoExist(aDeliveryMan.MobileNo, aDeliveryMan.Id))
                {
                    return BadRequest("Mobile no already exists");
                }

                if (_deliveryManManager.DoesEmailExist(aDeliveryMan.Email, aDeliveryMan.Id))
                {
                    return BadRequest("Email already exists");
                }
                if (_deliveryManManager.DoesNIDExist(aDeliveryMan.NID, aDeliveryMan.Id))
                {
                    return BadRequest("NID already exists");
                }

                deliveryMan.ImageUrl = GetImageUrlBySavingImage(aDeliveryMan.ImageByte, aDeliveryMan.ImageUrl);
                deliveryMan.ModifiedBy = aDeliveryMan.ModifiedBy;
                deliveryMan.Name = aDeliveryMan.Name;
                deliveryMan.NID = aDeliveryMan.NID;
                deliveryMan.ThanaId = aDeliveryMan.ThanaId;
                deliveryMan.Email = aDeliveryMan.Email;
                deliveryMan.MobileNo = aDeliveryMan.MobileNo;
                deliveryMan.Address = aDeliveryMan.Address;
                deliveryMan.ModifiedOn = DateTime.Now;
                deliveryMan.IsActive = aDeliveryMan.IsActive;

                deliveryMan.User = _userManager.GetById((long)aDeliveryMan.UserId);
                deliveryMan.User.Username = aDeliveryMan.MobileNo;
                if (!string.IsNullOrEmpty(aDeliveryMan.User.Password))
                {
                    deliveryMan.User.Password = aDeliveryMan.User.Password;
                }
                deliveryMan.User.ModifiedBy = aDeliveryMan.ModifiedBy;
                deliveryMan.User.ModifiedOn = DateTime.Now;

                _userManager.Update(deliveryMan.User);


                if (aDeliveryMan.CurrentUserId == null)
                {
                    var assignedMasterDepots = _masterDepotDeliveryManManager.GetByDeliveryMenId(aDeliveryMan.Id);
                    var masterDepotsTobeUpdated = assignedMasterDepots
                        .Where(m => !aDeliveryMan.MasterDepotIds.Contains((long)m.MasterDepotId)).ToList();

                    var masterDepotIds = assignedMasterDepots.Select(x => x.MasterDepotId);
                    aDeliveryMan.MasterDepotIds = aDeliveryMan.MasterDepotIds.Where(x => !masterDepotIds.Contains(x)).ToArray();
                    foreach (var masterDepot in masterDepotsTobeUpdated)
                    {
                        masterDepot.IsActive = false;
                        masterDepot.IsDeleted = true;
                        masterDepot.ModifiedOn = DateTime.UtcNow.AddHours(6);
                        masterDepot.ModifiedBy = aDeliveryMan.ModifiedBy;
                    }

                    foreach (var id in aDeliveryMan.MasterDepotIds)
                    {
                        MasterDepotDeliveryMan masterDepotDeliveryMan = new MasterDepotDeliveryMan();
                        masterDepotDeliveryMan.MasterDepotId = id;
                        masterDepotDeliveryMan.DeliveryManId = deliveryMan.Id;
                        masterDepotDeliveryMan.IsActive = true;
                        masterDepotDeliveryMan.CreatedOn = DateTime.UtcNow.AddHours(6);
                        masterDepotDeliveryMan.CreatedBy = (long)aDeliveryMan.CreatedBy;

                        masterDepotsTobeUpdated.Add(masterDepotDeliveryMan);
                    }

                    _masterDepotDeliveryManManager.Update(masterDepotsTobeUpdated);
                }

                bool isUpdated = _deliveryManManager.Update(deliveryMan);
                if (isUpdated)
                {
                    return Ok();
                }
                return BadRequest("Could not update delivery man");
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }

        }

        [HttpGet]
        //[Authorize(Roles = "DeliveryMan,MasterDepotUser")]
        public IHttpActionResult ViewAssignedOrders(long id)
        {
            var orderIds = _assignOrderManager.GetByDeliveryManId(id).Select(o => o.Order.Id).ToArray();
            var orderDetails = _orderDetailManager.GetByOnProcessingOrderDetailsByOrderIds(orderIds);
            var orders = orderDetails.GroupBy(o => o.OrderId).Select(o => new ViewAssigedOrdersByDeliveryManDto()
            {
                OrderNo = o.First(p => p.OrderId == o.Key).Order.OrderNo,
                OrderId = o.First(p => p.OrderId == o.Key).OrderId,
                OrderDate = o.First(p => p.OrderId == o.Key).Order.OrderDate,
                TotalPrice = Convert.ToDouble(o.Sum(p => p.Price)),
                ModifiedDate = o.First(p => p.OrderId == o.Key)
                    .Order.OrderHistories.First(x => x.OrderStateId == o.First(p => p.OrderId == o.Key)
                                                         .Order.OrderStateId).OrderStateChangedOn,
                OrderStateId = (long)o.First(p => p.OrderId == o.Key).Order.OrderStateId,
                OrderStatus = o.First(p => p.OrderId == o.Key).Order.OrderState.Status
            }).ToList();

            return Ok(orders);
        }
        [HttpGet]
        public IHttpActionResult ViewPickedOrders(long id)
        {
            try
            {
                var orderIds = _assignOrderManager.GetByDeliveryManId(id).Select(o => o.Order.Id).ToArray();
                var orderDetails = _orderDetailManager.GetPickedOrderDetailsByOrderIds(orderIds);
                var orders = orderDetails.GroupBy(o => o.OrderId).Select(o => new ViewAssigedOrdersByDeliveryManDto()
                {
                    OrderNo = o.First(p => p.OrderId == o.Key).Order.OrderNo,
                    OrderId = o.First(p => p.OrderId == o.Key).OrderId,
                    OrderDate = o.First(p => p.OrderId == o.Key).Order.OrderDate,
                    ModifiedDate = o.First(p => p.OrderId == o.Key)
                        .Order.OrderHistories.First(x => x.OrderStateId == o.First(p => p.OrderId == o.Key)
                                                             .Order.OrderStateId).OrderStateChangedOn,
                    TotalPrice = Convert.ToDouble(o.Sum(p => p.Price)),
                    OrderStateId = (long)o.First(p => p.OrderId == o.Key).Order.OrderStateId,
                    OrderStatus = o.First(p => p.OrderId == o.Key).Order.OrderState.Status
                }).ToList();
                if (orders == null) NotFound();
                return Ok(orders);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        public IHttpActionResult ViewReceivedOrders(long id)
        {
            try
            {
                var orderIds = _assignOrderManager.GetByDeliveryManId(id).Select(o => o.Order.Id).ToArray();
                var orderDetails = _orderDetailManager.GetReceivedOrderDetailsByOrderIds(orderIds);
                var orders = orderDetails.GroupBy(o => o.OrderId).Select(o => new ViewAssigedOrdersByDeliveryManDto()
                {
                    OrderNo = o.First(p => p.OrderId == o.Key).Order.OrderNo,
                    OrderId = o.First(p => p.OrderId == o.Key).OrderId,
                    OrderDate = o.First(p => p.OrderId == o.Key).Order.OrderDate,
                    ModifiedDate = o.First(p => p.OrderId == o.Key)
                        .Order.OrderHistories.First(x => x.OrderStateId == o.First(p => p.OrderId == o.Key)
                                                             .Order.OrderStateId).OrderStateChangedOn,
                    TotalPrice = Convert.ToDouble(o.Sum(p => p.Price)),
                    OrderStateId = (long)o.First(p => p.OrderId == o.Key).Order.OrderStateId,
                    OrderStatus = o.First(p => p.OrderId == o.Key).Order.OrderState.Status
                }).ToList();
                if (orders == null) NotFound();
                return Ok(orders);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        private string GetImageUrlBySavingImage(byte[] imageBytes, string imageUrl)
        {
            string imageName = UtilityClass.GenerateImageNameFromTimestamp();
            if (imageBytes != null)
            {
                Image image = UtilityClass.ConvertByteToImage(imageBytes);
                string fileLocation = "Content/Img/DeliveryMan/";
                string path = Path.Combine(HttpContext.Current.Server.MapPath("~/" + fileLocation), imageName);
                image.Save(path, ImageFormat.Png);
                string fullPath = fileLocation + "/" + imageName;

                imageUrl = fullPath;
            }

            return imageUrl;
        }

    }
}
