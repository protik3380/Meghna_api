//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace EFreshStoreCore.Model.Context
{
    using System;
    using System.Collections.Generic;
    
    public partial class LPGComboDiscount
    {
        public long Id { get; set; }
        public int DiscountType { get; set; }
        public Nullable<decimal> DiscountPercentage { get; set; }
        public System.DateTime ActiveDate { get; set; }
        public System.DateTime Validity { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public long CreatedBy { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public Nullable<long> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedOn { get; set; }
    }
}