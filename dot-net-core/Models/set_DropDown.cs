using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EMenuApplication.Models
{
    public class set_DropDown
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public string DD_ID { get; set; }
        public string DD_Value { get; set; }
        public string DD_Display { get; set; }
        public string DD_ValueOrder { get; set; }
        public string createdby { get; set; }
        public int? status { get; set; }
    }
}
