using System;
using System.ComponentModel.DataAnnotations;
using RecursiveValidator.Validation;

namespace RecursiveValidator.Models
{
   public class Address {
      [Required]
      public String Street1 { get; set; }

      public String Street2 { get; set; }

      [Required]
      public String City { get; set; }

      [Required]
      public String State { get; set; }

      [Required, ValidateObject]
      public ZipCode Zip { get; set; }
   }
}