using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecursiveValidator.Models {
   public class ZipCode {
      [Required]
      public String PrimaryCode { get; set; }

      public String SubCode { get; set; }
   }
}
