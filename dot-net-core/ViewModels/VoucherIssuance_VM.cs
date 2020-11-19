using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMenuApplication.ViewModels
{
    public class VoucherIssuance_VM
    {
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
        public bool IsDeleted { get; set; }
        public string CustomerName { get; set; }
        public string VoucherName { get; set; }
        public string CategoryName { get; set; }
        public string SubCategoryName { get; set; }
        public bool? IsApproved { get; set; }
        public string ApprovedStatus { get; set; }
        public string ReasonDescription { get; set; }
        public bool HasVoucherApprovalPermission { get; set; }
        public bool HasVoucherIssuancePermission { get; set; }
        public List<int> StoreIds { get; set; }
        public VoucherIssuance_VM()
        {
            StoreIds = new List<int>();
        }
    }
}
