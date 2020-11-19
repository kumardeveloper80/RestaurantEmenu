using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMenuApplication.ViewModels
{
    public class ReasonSubCategoryMaster_VM
    {
        public int Id { get; set; }
        public int ReasonCategoryId { get; set; }
        public string Name { get; set; }
        public bool Status { get; set; }
        public string ReasonCategoryName { get; set; }
        public int ClientId { get; set; }
    }
}
