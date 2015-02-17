using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace RecursiveValidator.Validation {
   public class ValidateObjectAttribute : ValidationAttribute {
      protected override ValidationResult IsValid(object value, ValidationContext validationContext) {
         var results = new List<ValidationResult>();
         var context = new ValidationContext(value, null, null);

         Validator.TryValidateObject(value, context, results, true);

         if (results.Count != 0) {
            var compositeResults = new CompositeValidationResult(String.Format("Validation for {0} failed!", validationContext.DisplayName));
            results.ForEach(compositeResults.AddResult);

            return compositeResults;
         }

         return ValidationResult.Success;
      }
   }

   public class ValidateCollectionAttribute : ValidationAttribute {
      public Type ValidationType { get; set; }

      protected override ValidationResult IsValid(object value, ValidationContext validationContext) {
         var collectionResults = new CompositeValidationResult(String.Format("Validation for {0} failed!",
                        validationContext.DisplayName));
         var enumerable = value as IEnumerable;

         var validators = GetValidators().ToList();

         if (enumerable != null)
         {
            var index = 0;

            foreach (var val in enumerable)
            {
               var results = new List<ValidationResult>();
               var context = new ValidationContext(val, validationContext.ServiceContainer, null);

               if (ValidationType != null)
               {
                  Validator.TryValidateValue(val, context, results, validators);
               }
               else
               {
                  Validator.TryValidateObject(val, context, results, true);
               }

               if (results.Count != 0)
               {
                  var compositeResults =
                     new CompositeValidationResult(String.Format("Validation for {0}[{1}] failed!",
                        validationContext.DisplayName, index));

                  results.ForEach(compositeResults.AddResult);

                  collectionResults.AddResult(compositeResults);
               }

               index++;
            }
         }

         if (collectionResults.Results.Any())
         {
            return collectionResults;
         }

         return ValidationResult.Success;
      }

      private IEnumerable<ValidationAttribute> GetValidators()
      {
         if(ValidationType == null) yield break;

         yield return (ValidationAttribute) Activator.CreateInstance(ValidationType);
      }
   }

   public class CompositeValidationResult : ValidationResult {
      private readonly List<ValidationResult> _results = new List<ValidationResult>();

      public IEnumerable<ValidationResult> Results {
         get {
            return _results;
         }
      }

      public CompositeValidationResult(string errorMessage) : base(errorMessage) { }
      public CompositeValidationResult(string errorMessage, IEnumerable<string> memberNames) : base(errorMessage, memberNames) { }
      protected CompositeValidationResult(ValidationResult validationResult) : base(validationResult) { }

      public void AddResult(ValidationResult validationResult) {
         _results.Add(validationResult);
      }
   }
}
