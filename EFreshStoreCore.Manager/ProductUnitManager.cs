using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using EFreshStoreCore.Model.Context;
using EFreshStoreCore.Model.Interfaces.Managers;
using EFreshStoreCore.Repository;

namespace EFreshStoreCore.Manager
{
    public class ProductUnitManager : CommonManager<ProductUnit>, IProductUnitManager
    {
        public ProductUnitManager() : base(new ProductUnitRepository())
        {
        }

        public ICollection<ProductUnit> GetAll()
        {
            return GetAll(c => c.Product,
                c => c.Product.Category,
                c => c.Product.Category.ProductType,
                c => c.Product.Brand,
                c => c.ProductImages,
                c => c.ProductDiscounts);
        }

        public ProductUnit GetById(long id)
        {
            return GetFirstOrDefault(c => c.Id == id,
                c => c.Product.Category, 
                c => c.Product.Category.ProductType,
                c => c.Product.Brand,
                c => c.ProductImages,
                c => c.ProductDiscounts);
        }

        public ProductUnit Get(long id)
        {
            return GetFirstOrDefault(c => c.Id == id && c.IsDeleted.HasValue && !c.IsDeleted.Value
                && c.IsActive.HasValue && c.IsActive.Value 
                && c.Product.IsDeleted.HasValue 
                && !c.Product.IsDeleted.Value
                && c.Product.IsActive.HasValue && c.Product.IsActive.Value,
                c => c.Product.Category,
                c => c.Product.Brand,
                c => c.Product.Category.ProductType,
                c => c.ProductImages,
                c => c.ProductDiscounts);
        }

        public ICollection<ProductUnit> GetByBrand(long brandId)
        {
            IProductManager productManager = new ProductManager();
            List<Product> products = productManager.GetByBrand(brandId).ToList();
            List<ProductUnit> productUnits = new List<ProductUnit>();
            foreach (Product product in products)
            {
                productUnits.AddRange(product.ProductUnits);
            }

            return productUnits;
        }

        public ICollection<ProductUnit> GetByBrandIds(long[] brandIds)
        {
            return Get(c => c.IsActive.HasValue
                            && c.IsActive.Value
                            && c.IsDeleted.HasValue
                            && !c.IsDeleted.Value
                            && c.Product.IsDeleted.HasValue
                            && !c.Product.IsDeleted.Value
                            && c.Product.IsActive.HasValue
                            && c.Product.IsActive.Value
                            && brandIds.Contains((long)c.Product.BrandId)
                            && c.Product.Brand.IsActive
                            && c.Product.Category.IsActive
                , c => c.Product, 
                c => c.Product.Brand, 
                c => c.Product.Category,
                c => c.Product.Category.ProductType,
                c => c.ProductImages, 
                c => c.ProductUnitPrices, 
                c => c.ProductDiscounts);
        }

        public ICollection<ProductUnit> GetByCategoryIds(long[] categoryIds)
        {
            return Get(c => c.IsActive.HasValue
                            && c.IsActive.Value
                            && c.IsDeleted.HasValue
                            && !c.IsDeleted.Value
                            && c.Product.IsDeleted.HasValue
                            && !c.Product.IsDeleted.Value
                            && c.Product.IsActive.HasValue
                            && c.Product.IsActive.Value
                            && categoryIds.Contains((long)c.Product.CategoryId)
                            && c.Product.Brand.IsActive
                            && c.Product.Category.IsActive
                , c => c.Product, c => c.Product.Brand, 
                c => c.Product.Category,
                c => c.Product.Category.ProductType, 
                c => c.ProductImages, 
                c => c.ProductUnitPrices, 
                c => c.ProductDiscounts);
        }

        public ICollection<ProductUnit> GetByCategory(long categorydId)
        {
            return Get(c => c.Product.CategoryId == categorydId
             && c.IsActive.HasValue
             && c.IsActive.Value
             && c.IsDeleted.HasValue
             && !c.IsDeleted.Value
             && c.Product.Brand.IsActive
             && c.Product.Category.IsActive
            , c => c.Product, c => c.Product.Brand, 
            c => c.Product.Category,
            c => c.Product.Category.ProductType, 
            c => c.ProductImages,
            c => c.ProductUnitPrices, c => c.ProductDiscounts);
        }

        public ICollection<ProductUnit> GetByBrandIdsAndCategoryIds(long[] brandIds, long[] categoryIds)
        {
            return Get(c => c.IsActive.HasValue
                            && c.IsActive.Value
                            && c.IsDeleted.HasValue
                            && !c.IsDeleted.Value
                            && c.Product.IsDeleted.HasValue
                            && !c.Product.IsDeleted.Value
                            && c.Product.IsActive.HasValue
                            && c.Product.IsActive.Value
                            && categoryIds.Contains((long)c.Product.CategoryId)
                            && brandIds.Contains((long)c.Product.BrandId)
                            && c.Product.Brand.IsActive
                            && c.Product.Category.IsActive
                , c => c.Product, c => c.Product.Brand, 
                c => c.Product.Category,
                c => c.Product.Category.ProductType, 
                c => c.ProductImages, c => c.ProductUnitPrices, c => c.ProductDiscounts);
        }

        public ICollection<ProductUnit> GetByBrandId(long brandId)
        {
            return Get(c => c.Product.BrandId == brandId
                            && c.IsActive.HasValue
                            && c.IsActive.Value
                            && c.IsDeleted.HasValue
                            && !c.IsDeleted.Value
                            && c.Product.Brand.IsActive
                            && c.Product.Category.IsActive,
                c => c.Product, c => c.Product.Brand, c => c.Product.Category, c => c.Product.Category.ProductType,
                c => c.ProductImages, c => c.ProductUnitPrices, c => c.ProductDiscounts);
        }

        public ICollection<ProductUnit> GetByProductId(long productId)
        {
            return Get(c => c.ProductId == productId
                            && c.IsActive.HasValue
                            && c.IsActive.Value
                            && c.IsDeleted.HasValue
                            && !c.IsDeleted.Value
                            && c.Product.Brand.IsActive
                            && c.Product.Category.IsActive
                , c => c.Product);
        }

        public bool SaveProductDetails(ProductUnit productUnit)
        {
            IProductUnitManager _productUnitManager = new ProductUnitManager();
            IProductUnitPriceManager _productUnitPriceManager = new ProductUnitPriceManager();

            using (TransactionScope transactionScope = new TransactionScope())
            {
                try
                {
                    productUnit.CreatedOn = DateTime.Now;
                    productUnit.IsActive = true;

                    bool isSaved = _productUnitManager.Add(productUnit);


                    ProductUnitPrice productUnitPrice = new ProductUnitPrice();
                    productUnitPrice.CreatedOn = DateTime.Now;
                    productUnitPrice.IsActive = true;
                    productUnitPrice.MaximumRetailPrice = productUnit.MaximumRetailPrice;
                    productUnitPrice.TradePricePerCarton = productUnit.TradePricePerCarton;
                    productUnitPrice.DistributorPricePerCarton = productUnit.DistributorPricePerCarton;
                    productUnitPrice.ProductUnitId = productUnit.Id;
                    _productUnitPriceManager.Add(productUnitPrice);
                    //foreach (var item in productUnit.ProductImages)
                    //{
                    //    item.CreatedOn = DateTime.Now;
                    //    item.ProductUnit = null;
                    //    item.ProductUnitId = productUnit.Id;
                    //    _productImageManager.Add(item);
                    //}
                    transactionScope.Complete();
                    return true;
                    //transactionScope.Dispose();

                }
                catch (TransactionException ex)
                {
                    transactionScope.Dispose();

                }

            }
            return false;
        }

        public bool EditProductDetails(ProductUnit productUnit, bool hasNewImages)
        {
            IProductUnitManager _productUnitManager = new ProductUnitManager();
            IProductImageManager _productImageManager = new ProductImageManager();
            IProductUnitPriceManager _productUnitPriceManager = new ProductUnitPriceManager();

            using (TransactionScope transactionScope = new TransactionScope())
            {
                try
                {
                    productUnit.ModifiedOn = DateTime.Now;
                    var images = _productImageManager.GetAll().Where(c => c.ProductUnitId == productUnit.Id).ToList();
                    if (images != null && hasNewImages)
                    {
                        foreach (var item in images)
                        {
                            _productImageManager.Delete(item);
                        }
                    }
                    bool isUpdate = _productUnitManager.Update(productUnit);



                    ProductUnitPrice productUnitPrice = productUnitPrice = _productUnitPriceManager.GetFirstOrDefault(c => c.ProductUnitId == productUnit.Id);
                    if (productUnitPrice == null)
                    {
                        productUnitPrice = new ProductUnitPrice();
                    }
                    productUnitPrice.CreatedOn = DateTime.Now;
                    productUnitPrice.IsActive = true;
                    productUnitPrice.MaximumRetailPrice = productUnit.MaximumRetailPrice;
                    productUnitPrice.TradePricePerCarton = productUnit.TradePricePerCarton;
                    productUnitPrice.DistributorPricePerCarton = productUnit.DistributorPricePerCarton;
                    productUnitPrice.ProductUnitId = productUnit.Id;
                    _productUnitPriceManager.Update(productUnitPrice);
                    foreach (var item in productUnit.ProductImages)
                    {
                        item.CreatedOn = DateTime.Now;
                        item.ProductUnit = null;
                        item.ProductUnitId = productUnit.Id;
                        _productImageManager.Add(item);
                    }
                    transactionScope.Complete();
                    return true;
                    //transactionScope.Dispose();

                }
                catch (TransactionException)
                {
                    transactionScope.Dispose();

                }

            }
            return false;
        }

        public ICollection<ProductUnit> Get()
        {
            return Get(c => c.IsActive.HasValue
            && c.IsActive.Value
            && c.IsDeleted.HasValue
            && !c.IsDeleted.Value
            && c.Product.IsDeleted.HasValue
            && !c.Product.IsDeleted.Value
            && c.Product.IsActive.HasValue 
            && c.Product.IsActive.Value
            && c.Product.Brand.IsActive
            && c.Product.Category.IsActive
            , c => c.Product, c => c.Product.Brand, c => c.Product.Category,
            c => c.Product.Category.ProductType, c => c.ProductImages, c => c.ProductUnitPrices, c => c.ProductDiscounts);
        }

        public ICollection<ProductUnit> GetProductBySearch(string searchString)
        {
            ICollection<ProductUnit> productList = this.Get();
            return productList.Where(c => c.Product.Brand.Name.ToLower().Contains(searchString.ToLower())
                         || c.Product.Category.Name.ToLower().Contains(searchString.ToLower())
                         || c.Product.Name.ToLower().Contains(searchString.ToLower())).ToList();
        }
    }
}