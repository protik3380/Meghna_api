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

    public partial class MasterDepot
    {
        public MasterDepot()
        {
            this.Distributors = new HashSet<Distributor>();
            this.MasterDepotDeliveryMen = new HashSet<MasterDepotDeliveryMan>();
            this.MasterDepotProductLines = new HashSet<MasterDepotProductLine>();
            this.Orders = new HashSet<Order>();
            this.ThanaWiseMasterDepots = new HashSet<ThanaWiseMasterDepot>();
        }
        public long Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string ContactPerson { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public Nullable<long> UserId { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public Nullable<long> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public Nullable<long> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedOn { get; set; }
        public virtual ICollection<Distributor> Distributors { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<MasterDepotDeliveryMan> MasterDepotDeliveryMen { get; set; }
        public virtual ICollection<MasterDepotProductLine> MasterDepotProductLines { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<ThanaWiseMasterDepot> ThanaWiseMasterDepots { get; set; }
    }
}