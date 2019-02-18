using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;

namespace Yarmoolka.Models
{
    public class YarmoolkaClass
    {
        public int ID { get; set; }
        [Required]
        [Display(Name = "Model Name")]
        public string Name { get; set; }
        [DataType(DataType.Date)]
        [Required]
        [Display(Name = "Model Date")]
        public DateTime ModelDate { get; set; }
        public string Style { get; set; }

        [Range(0, 59.99)]
        public decimal Price { get; set; }
        [Required]
        public string Company { get; set; }
        [Required]
        public int Size { get; set; }
 
        public string Color { get; set; }

        [Display(Name = "Supplier")]
        public int SupplierId { get; set; }

        
        public virtual Supplier YarmoolkaSupplier { get; set; }
    }
}