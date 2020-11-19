using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMenuApplication.ViewModels
{
    public class User_VM
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
        public string Password { get; set; }
        public IFormFile ProfilePicture { get; set; }
        public string FileName { get; set; }
        public bool Active { get; set; }
        public int ClientId { get; set; }
        public string ClientName { get; set; }
        public bool IsAdmin { get; set; }
        public int CreatedBy { get; set; }
        public List<int> StoreIds { get;set;}
        public bool IsAllowVoucherIssuancePermission { get; set; }
        public bool IsAllowVoucherApprovalPermission { get; set; }
        public User_VM()
        {
            StoreIds = new List<int>();
        }
    }
}
