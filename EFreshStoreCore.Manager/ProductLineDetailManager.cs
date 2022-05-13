using System;
using System.Collections.Generic;
using System.Linq;
using EFreshStoreCore.Model.Context;
using EFreshStoreCore.Model.Interfaces.Managers;
using EFreshStoreCore.Repository;

namespace EFreshStoreCore.Manager
{
    public class ProductLineDetailManager : CommonManager<ProductLineDetail>, IProductLineDetailManager
    {
        public ProductLineDetailManager() : base(new ProductLineDetailRepository())
        {
        }

        public ICollection<ProductLineDetail> GetAll()
        {
            return GetAll(c => c.ProductLine, c => c.Product);
        }

        public ICollection<ProductLineDetail> GetProductsByLineId(long id)
        {
            return Get(c=> c.ProductLineId == id, c => c.ProductLine, c => c.Product);
        }

        public ProductLineDetail GetById(long id)
        {
            return GetFirstOrDefault(c => c.Id == id, c => c.Product, c => c.ProductLine);
        }

        public List<ProductLineDetail> GetByProduct(long productId)
        {
            return Get(c => c.ProductId == productId, c => c.ProductLine, c => c.ProductLine.MasterDepotProductLines).ToList();
        }

        public ProductLineDetail IsExistProductLine(ProductLineDetail productLineDetail)
        {
            return GetFirstOrDefault(c => c.ProductId == productLineDetail.ProductId && c.ProductLineId == productLineDetail.ProductLineId);
        }
    }
}