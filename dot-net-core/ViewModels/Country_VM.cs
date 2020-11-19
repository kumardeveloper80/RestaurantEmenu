using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMenuApplication.ViewModels
{
    public class Country_VM
    {
        public int CountryID { get; set; }
        public string Name { get; set; }
        public string Country { get; set; }
        public string Code { get; set; }
        public int? Digits { get; set; }
        public int? minDigits { get; set; }
        public string timezone { get; set; }
        public bool Status { get; set; } = true;
    }
}
