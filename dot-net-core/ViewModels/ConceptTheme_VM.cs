using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMenuApplication.ViewModels
{
    public class ConceptTheme_VM
    {
        public int Id { get; set; }
        public int ConceptId { get; set; }
        public string ConceptName { get; set; }
        public string ColorCode { get; set; }
        public string LogoName { get; set; }
        public string FeedBackIconName { get; set; }
        public bool Status { get; set; } = true;
        public IFormFile LogoImage { get; set; }
        public IFormFile FeedBackIcon { get; set; }
    }
}
