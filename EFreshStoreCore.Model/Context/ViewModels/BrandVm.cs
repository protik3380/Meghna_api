using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Web;
using EFreshStoreCore.Model.Context;

namespace EFreshStore.Models.ViewModels
{
    class BrandVm
    {
        [SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public BrandVm()
        {
            this.Products = new HashSet<Product>();
        }

        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<long> CreatedBy { get; set; }
        public Nullable<DateTime> CreatedOn { get; set; }
        public Nullable<long> ModifiedBy { get; set; }
        public Nullable<DateTime> ModifiedOn { get; set; }
        public string BrandImage { get; set; }

        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Product> Products { get; set; }

        public HttpPostedFileBase ImageLocation { get; set; }
        public byte[] ImageBytes { get; set; }
    }
}
