using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RecursiveValidator.Models;
using RecursiveValidator.Validation;

namespace RecursiveValidator {
   class Program {
      static void Main(string[] args)
      {
         var person = new Person
                      {
                         Addressess = new List<Address>
                                      {
                                         new Address
                                         {
                                            Street1 = "Main Street 123",
                                            City = "Awesome Town",
                                            State = "TN",
                                            Zip = new ZipCode()
                                         }
                                         ,
                                         new Address
                                         {
                                            City = "Awesome Town",
                                            State = "TN",
                                            Zip = new ZipCode{PrimaryCode = "12345"}
                                         }
                                      },
                         Name = "Josh",
                         Phone = new List<string> {"blah", "blah"}
                      };

         var context = new ValidationContext(person, null, null);
         var results = new List<ValidationResult>();

         Validator.TryValidateObject(person, context, results, true);

         PrintResults(results, 0);

         Console.ReadKey();
      }

      private static void PrintResults(IEnumerable<ValidationResult> results, Int32 indentationLevel) {
         foreach (var validationResult in results) {
            SetIndentation(indentationLevel);

            Console.WriteLine(validationResult.ErrorMessage);
            Console.WriteLine();

            if (validationResult is CompositeValidationResult) {
               PrintResults(((CompositeValidationResult)validationResult).Results, indentationLevel + 1);
            }
         }
      }

      private static void SetIndentation(int indentationLevel) {
         Console.CursorLeft = indentationLevel * 2;
      }
   }
}
