using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Yarmoolka.Models
{
    public class Customer
    {
        public long ID { get; set; }
        public string CustomerId { get; set; }
        [Required]
        public string Name { get; set; }
        [Range(10, 120)]
        public int Age { get; set; }
        public string City { get; set; }
        [Phone]
        public string PhoneNumber { get; set; }
      
    }
}
