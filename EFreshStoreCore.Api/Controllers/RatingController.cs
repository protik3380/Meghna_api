using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AutoMapper;
using EFreshStoreCore.Manager;
using EFreshStoreCore.Model.Context;
using EFreshStoreCore.Model.Dtos;
using EFreshStoreCore.Model.Enums;
using EFreshStoreCore.Model.Interfaces.Managers;

namespace EFreshStoreCore.Api.Controllers
{
    public class RatingController : ApiController
    {
        private readonly IRatingManager _ratingManager;
        private readonly IMeghnaUserManager _meghnaUserManager;
        private readonly ICustomerManager _customerManager;
        private readonly ICorporateUserManager _corporateUserManager;
        public RatingController()
        {
            _ratingManager = new RatingManager();
            _meghnaUserManager = new MeghnaUserManager();
            _customerManager = new CustomerManager();
            ;_corporateUserManager = new CorporateUserManager();
        }

        [HttpPost]
        public IHttpActionResult Create([FromBody]Rating rating)
        {
            try
            {
                if (rating != null)
                {
                    rating.RatingTime = DateTime.Now;
                    var ratingToUpdate = _ratingManager.IsRatingExists(rating.UserId, rating.ProductUnitId);
                    if (ratingToUpdate != null)
                    {
                        rating.Id = ratingToUpdate.Id;
                        bool isUpdate = _ratingManager.Update(rating);
                        if (isUpdate)
                        {
                            return Ok();
                        }
                    }
                    else
                    {
                        bool isSaved = _ratingManager.Add(rating);
                        if (isSaved)
                        {
                            return Created(new Uri(Request.RequestUri.ToString()), rating);
                        }
                    }
                }
                return BadRequest();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet]
        public IHttpActionResult GetRatingByProductUnit(long? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            try
            {
                var ratings = _ratingManager.GetRatingByProductUnit((long)id);
                if (ratings != null)
                {
                    foreach (var rating in ratings)
                    {
                        if (rating.User.UserTypeId == (long)UserTypeEnum.MeghnaUser)
                        {
                            var meghnaUser = _meghnaUserManager.GetByUserId(rating.UserId);
                            rating.User.Username = meghnaUser.Name;
                        }
                        if (rating.User.UserTypeId == (long)UserTypeEnum.Corporate)
                        {
                            var corporateUser = _corporateUserManager.GetByUserId(rating.UserId);
                            rating.User.Username = corporateUser.Name;
                        }
                        if (rating.User.UserTypeId == (long)UserTypeEnum.Customer)
                        {
                            var customer = _customerManager.GetByUserId(rating.UserId);
                            rating.User.Username = customer.Name;
                        }
                    }

                    var config = new MapperConfiguration(cfg => {
                        cfg.CreateMap<Rating, RatingToReturnDto>()
                            .ForMember(d => d.UserName, opts => opts.MapFrom(s => s.User.Username))
                            .ForMember(d => d.ProductName, opts => opts.MapFrom(s => s.ProductUnit.Product.Name));
                    });

                    IMapper iMapper = config.CreateMapper();
                    var ratingsToReturn = iMapper.Map<IEnumerable<Rating>, IEnumerable<RatingToReturnDto>>(ratings);
                    ratingsToReturn = ratingsToReturn.OrderByDescending(r => r.RatingTime);
                    return Ok(ratingsToReturn);
                }

                return NotFound();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
