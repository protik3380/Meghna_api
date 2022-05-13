using System.Collections.Generic;
using EFreshStoreCore.Model.Context;
using EFreshStoreCore.Model.Interfaces.Managers;
using EFreshStoreCore.Repository;

namespace EFreshStoreCore.Manager
{
    public class ProductLineManager : CommonManager<ProductLine>, IProductLineManager
    {
        public ProductLineManager() : base(new ProductLineRepository())
        {
        }

        public bool IsExistByName(string productLineName)
        {
            ProductLine productLine = GetFirstOrDefault(c => c.Name== productLineName);
            if(productLine!=null)
            {
                return true;
            }
            return false;
        }

        public ProductLine GetById(long id)
        {
            return GetFirstOrDefault(p => p.Id == id);
        }

        public List<ProductLine> GetByProduct(long productId)
        {
            IProductLineDetailManager productLineDetailManager = new ProductLineDetailManager();
            List<ProductLineDetail> productLineDetails = productLineDetailManager.GetByProduct(productId);

            var productLines = new List<ProductLine>();
            foreach (ProductLineDetail productLineDetail in productLineDetails)
            {
                productLines.Add(productLineDetail.ProductLine);
            }

            return productLines;
        }
    }
}