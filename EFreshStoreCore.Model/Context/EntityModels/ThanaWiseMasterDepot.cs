//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace EFreshStoreCore.Model.Context
{
    using System;
    using System.Collections.Generic;
    
    public partial class ThanaWiseMasterDepot
    {
        public long Id { get; set; }
        public Nullable<long> MasterDepotId { get; set; }
        public Nullable<long> ThanaId { get; set; }
    
        public virtual MasterDepot MasterDepot { get; set; }
        public virtual Thana Thana { get; set; }
    }
}