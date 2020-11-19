using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EMenuApplication.ViewModels
{
    public class Login_VM
    {
        [Required]
        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
        public bool IsAdmin { get; set; }
        public int UserId { get; set; }
        public string FullName { get; set; }
        public string ProfilePic { get; set; }
        public bool IsSuperAdmin { get; set; }
        public int ClientId { get; set; }
        public bool IsAllowVoucherIssuancePermission { get; set; }
        public bool IsAllowVoucherApprovalPermission { get; set; }
    }
}
