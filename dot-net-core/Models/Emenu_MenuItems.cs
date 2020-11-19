using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EMenuApplication.Models
{
    public class Emenu_MenuItems
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string PLU { get; set; }
        public string Name { get; set; }
        public int CategoryId { get; set; }
        public int CurrencyId { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }
        public bool Status { get; set; }
        public string DetailsEN { get; set; }
        public string DetailsAR { get; set; }
        public string ThumbnailImageName { get; set; }
        public string LargeImageName { get; set; }
        public string LargeDetailsEN { get; set; }
        public string LargeDetailsAR { get; set; }
        public string OverLayImageName { get; set; }
        public string OverlayDetailsEN { get; set; }
        public string OverlayDetailsAR { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime? DeletedOn { get; set; }
        public int DeletedBy { get; set; }
        public bool IsDeleted { get; set; }
        public string CommentsEN { get; set; }
        public string CommentsAR { get; set; }
        public string LabelEN { get; set; }
        public string LabelAR { get; set; }
    }
}
