using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMenuApplication.ViewModels
{
    public class Concepts_VM
    {
        public int Id { get; set; }
        public string ConceptName { get; set; }
        public bool Active { get; set; } = true;
        public int ClientId { get; set; }
        public string ClientName { get; set; }
    }
}
