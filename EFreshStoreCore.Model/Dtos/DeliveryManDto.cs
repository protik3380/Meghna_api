using System;
using System.ComponentModel.DataAnnotations;
using EFreshStoreCore.Model.Context;

namespace EFreshStoreCore.Model.Dtos
{
    public class DeliveryManDto
    {
        public long Id { get; set; }
        [Required(ErrorMessage = "Please enter name")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Please enter mobile no")]
        [RegularExpression(@"^01[1-9]\d{8}$", ErrorMessage = "Please enter a valid mobile no. Ex. 01xxxxxxxxx")]
        public string MobileNo { get; set; }
        [RegularExpression(@"[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?", ErrorMessage = "Please enter a valid email")]
        public string Email { get; set; }
        public string Address { get; set; }
        public Nullable<long> UserId { get; set; }
        [RegularExpression(@"^[0-9]{17}$", ErrorMessage = "Please enter a valid NID")]
        public string NID { get; set; }
        public long? CurrentUserId { get; set; }
        public long? ThanaId { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public long? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public long? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string ImageUrl { get; set; }
        public byte[] ImageByte { get; set; }
        public long[] MasterDepotIds { get; set; }
        public virtual Thana Thana { get; set; }
        public virtual User User { get; set; }
    }
}
