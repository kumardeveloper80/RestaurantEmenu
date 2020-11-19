using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EMenuApplication.Models
{
    public class Emenu_Currency
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Currency { get; set; }
        public string Symbol { get; set; }
        public string SymbolAR { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime? DeletedOn { get; set; }
        public int DeletedBy { get; set; }
        public bool IsDeleted { get; set; }
        public int ClientId { get; set; }
    }
}
