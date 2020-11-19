using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMenuApplication.ViewModels
{
    public class ItemTag_VM
    {
       
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Status { get; set; } = true;
        public int CreatedBy { get; set; }
        public string LabelAR { get; set; }
        public string LabelEN { get; set; }
        public string IconTagName { get; set; }
        public IFormFile IconTag { get; set; }
        public List<int> ConceptIds { get; set; }
        public string ConceptName { get; set; }
        public List<Concepts_VM> ItemTagsConcepts { get; set; }
        public ItemTag_VM()
        {
            ConceptIds = new List<int>();
            ItemTagsConcepts = new List<Concepts_VM>();
        }

       
    }
}
