using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EMenuApplication.Models
{
    public class Set_VoucherSurvey
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int VoucherId { get; set; }
        public int StoreId { get; set; }
        public int SurveyId { get; set; }
    }
}
