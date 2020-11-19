using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EMenuApplication.Models
{
    public class CRM_CommentCardResultsHeader
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CCR_ID { get; set; }
        public int? cc_id { get; set; }
        public Guid? Customer_ID { get; set; }
        public Guid? AccountID { get; set; }
        public Guid? Store_ID { get; set; }
        public int? Storeno { get; set; }
        public int? Checkno { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? LastCall { get; set; }
        public int? NoCall { get; set; }
        public string Email { get; set; }
        public string company { get; set; }
        public string Mobile { get; set; }
        public string SecondaryMobile { get; set; }
        public DateTime? Birthday { get; set; }
        public string Gender { get; set; }
        public string Synchronized { get; set; }
        public string OperationalReporting { get; set; }
        public string ImmediateAction { get; set; }
        public string CallAgentNote { get; set; }
        public double? Score { get; set; }

        [Column(TypeName = "decimal(8,2)")]
        public decimal? Avgscore { get; set; }

        [Column(TypeName = "decimal(8,2)")]
        public decimal? TotalScore { get; set; }
        public string Waiter { get; set; }
        public string Tableno { get; set; }
        public string BadgeNo { get; set; }
        public string Fields { get; set; }
        public string Params { get; set; }
        public string ParamValues { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string ModifiedBy { get; set; }
        public bool? active { get; set; }
        public byte? status { get; set; }
        public bool? CaseCreated { get; set; }
        public int? ActNbreCall { get; set; }
        public string ActNote { get; set; }
        public bool? Actactive { get; set; }
        public string Act_lastStatus { get; set; }
        public DateTime? Act_lastcalled { get; set; }
        public string Act_lastUser { get; set; }
        public byte? Act_Status { get; set; }
        public int? salutation_id { get; set; }
        public int? CountryID { get; set; }
        public string Region { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string Fax { get; set; }
        public string BusinessCard { get; set; }
        public int? MaritalStatus_ID { get; set; }
        public int? Nationality { get; set; }
        public string Twitter { get; set; }
        public string Facebook { get; set; }
        public string Instagram { get; set; }
        public string LinkedIn { get; set; }
        public string Website { get; set; }
        public string OtherSM { get; set; }
        public DateTime? TimeCreatedOn { get; set; }
        public int? COR { get; set; }
        public float QSA { get; set; }
        public float Promo { get; set; }
        public float Amount { get; set; }
        public bool visible { get; set; }
        public string Concept { get; set; }
        public string StoreTimezone { get; set; }
        public string Language { get; set; }
        public float ServiceAVGscore { get; set; }
        public DateTime ImmediateUserDate { get; set; }
        public string ImmediateUser { get; set; }
    }
}
