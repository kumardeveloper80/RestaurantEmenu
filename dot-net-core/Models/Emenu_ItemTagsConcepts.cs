using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EMenuApplication.Models
{
    public class Emenu_ItemTagsConcepts
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int ItemTagId { get; set; }
        public int ConceptId { get; set; }
    }
}
