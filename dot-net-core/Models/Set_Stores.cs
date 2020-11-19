using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EMenuApplication.Models
{
    public class Set_Stores
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public Guid StoreGuid { get; set; }
        public string StoreName { get; set; }
        public string IP { get; set; }
        public string URL { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public DateTime? DeletedOn { get; set; }
        public int? DeletedBy { get; set; }
        public bool IsDeleted { get; set; }
        public bool Status { get; set; }
        public int SquirrelID { get; set; }
        public string StoreCode { get; set; }
        public int CountryCode { get; set; }
        public int SquirrelCurrency { get; set; }
        public string CCEmailRecipient { get; set; }
        public int ListOrder { get; set; }
        public string CCSMSReceipient { get; set; }
        public bool CommentCardAutoFill { get; set; }
        public string TimeZone { get; set; }
        public int RegionId { get; set; }
        public int ConceptId { get; set; }
        public int ClientId { get; set; }
    }
}
