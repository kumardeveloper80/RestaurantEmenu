using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMenuApplication.ViewModels
{
    public class VoucherSetup_VM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Terms { get; set; }
        public string Limitations { get; set; }
        public int Type { get; set; }
        public decimal Value { get; set; }
        public bool IsMultipleTimeUsage { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public List<int> StoreIds { get; set; }
        public List<int> MenuItemIds { get; set; }
        public List<int> SurveyIds { get; set; }
        public bool Status { get; set; } = true;
        public bool IsDeleted { get; set; }
        public bool IsAmountType { get; set; }
        public bool IsPercentageType { get; set; }
        public bool IsHashOfPax { get; set; }
        public bool IsItemType { get; set; }
        public string Usage { get; set; }
        public VoucherSetup_VM()
        {
            StoreIds = new List<int>();
            MenuItemIds = new List<int>();
            SurveyIds = new List<int>();
        }
    }
}
