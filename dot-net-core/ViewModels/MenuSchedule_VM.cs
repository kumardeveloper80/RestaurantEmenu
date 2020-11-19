using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMenuApplication.ViewModels
{
    public class MenuSchedule_VM
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public int MenuId { get; set; }
        public string MenuName { get; set; }
        public bool Status { get; set; } = true;
        public int CreatedBy { get; set; }
        public string DateRange { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string UniqueCode { get; set; }
        public Byte[] QRCode { get; set; }
        public Menu_VM Menu { get; set; }
        public int ConceptId { get; set; }
        public List<Category_VM> Categories { get; set; }
        public string URL { get; set; }
        public int StoreId { get; set; }
        public Guid StoreGuid { get; set; }
        public DateTime dStartDate { get; set; }
        public DateTime dEndDate { get; set; }
        public string ConceptName { get; set; }
        public string StoreName { get; set; }
        public string ColorCode { get; set; }
        public string Logo { get; set; }
        public string FeedBackIcon { get; set; }
        public MenuSchedule_VM()
        {
            Categories = new List<Category_VM>();
        }
    }
}
