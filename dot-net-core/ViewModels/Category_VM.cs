using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMenuApplication.ViewModels
{
    public class Category_VM
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string LabelAR { get; set; }
        public string LabelEN { get; set; }
        public string DetailsAR { get; set; }
        public string DetailsEN { get; set; }
        public string ImageName { get; set; }
        public bool Status { get; set; } = true;
        public IFormFile Image { get; set; }
        public int CreatedBy { get; set; }
        public int CategorySequence { get; set; }
        public List<int> ConceptIds { get; set; }
        public string ConceptName { get; set; }
        public List<Concepts_VM> CategoryConcepts { get; set; }
        public Category_VM()
        {
            ConceptIds = new List<int>();
            CategoryConcepts = new List<Concepts_VM>();
        }
    }
}
