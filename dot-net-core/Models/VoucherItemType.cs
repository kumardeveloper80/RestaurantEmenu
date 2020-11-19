using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace EMenuApplication.Models
{
    public class VoucherItemType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int VoucherId { get; set; }
        public int MenuItemId { get; set; }
    }
}
