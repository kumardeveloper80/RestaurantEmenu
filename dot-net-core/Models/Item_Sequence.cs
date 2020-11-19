using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EMenuApplication.Models
{
    public class Item_Sequence
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int MenuItemId { get; set; }
        public int Sequence { get; set; }
        public int MenuId { get; set; }
    }
}
