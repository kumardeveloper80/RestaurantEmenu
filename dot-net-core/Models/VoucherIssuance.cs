using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EMenuApplication.Models
{
    public class VoucherIssuance
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int VoucherId { get; set; }
        public DateTime? RequestedDate { get; set; }
        public DateTime? ApprovedDate { get; set; }
        public DateTime? IssuedDate { get; set; }
        public int StoreId { get; set; }
        public int CustomerId { get; set; }
        public int ReasonCategoryId { get; set; }
        public int ReasonSubCategoryId { get; set; }
        public int Source { get; set; }
        public string UniqueId { get; set; }
        public string CampaignText { get; set; }
        public bool Status { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime? DeletedOn { get; set; }
        public int DeletedBy { get; set; }
        public bool IsDeleted { get; set; }
        public string ApprovedStatus { get; set; }
        public string ReasonDescription { get; set; }
    }
}
