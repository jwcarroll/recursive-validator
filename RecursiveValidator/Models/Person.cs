using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Xml;
using RecursiveValidator.Validation;

namespace RecursiveValidator.Models
{
   public class Person {
      [Required]
      public String Name { get; set; }

      [ValidateCollection(ValidationType = typeof(PhoneAttribute))]
      public List<String> Phone { get; set; }

      [Required, ValidateCollection]
      public List<Address> Addressess { get; set; }
   }
}