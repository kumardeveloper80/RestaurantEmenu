using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EMenuApplication.Models
{
    public class Sec_Users
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
        public string ProfilePicture { get; set; }
        public bool? Lock { get; set; }
        public int? DefaultModule { get; set; }
        public int? DefaultPage { get; set; }
        public bool? LockProfile { get; set; }
        public bool? UnlockProfile { get; set; }
        public byte? Status { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public int? CreatedBy { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? LastLoggedIn { get; set; }
        public bool? Active { get; set; }
        public bool? CanExportProfiles { get; set; }
        public bool? IsAdmin { get; set; }
        public DateTime? DeletedOn { get; set; }
        public int? DeletedBy { get; set; }
        public bool? IsDeleted { get; set; }
        public int ClientId { get; set; }
        public bool IsAllowVoucherIssuancePermission { get; set; }
        public bool IsAllowVoucherApprovalPermission { get; set; }

    }
}
