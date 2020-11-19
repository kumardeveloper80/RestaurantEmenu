using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMenuApplication.ViewModels
{
    public class Store_VM
    {
        public int Id { get; set; }
        public Guid StoreGuid { get; set; }
        public string StoreName { get; set; }
        public bool Status { get; set; } = true;
        public string StoreCode { get; set; }
        public int CountryCode { get; set; }
        public int RegionId { get; set; }
        public string RegionName { get; set; }
        public string ConceptName { get; set; }
        public int ClientId { get; set; }
        public string ClientName { get; set; }
        public string CountryName { get; set; }
        public List<int> ConceptIds { get; set; }
        public List<Concepts_VM> StoreConcepts { get; set; }
        public Store_VM()
        {
            ConceptIds = new List<int>();
            StoreConcepts = new List<Concepts_VM>();
        }
    }
}
