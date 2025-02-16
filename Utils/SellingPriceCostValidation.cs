using System;
using System.ComponentModel.DataAnnotations;

namespace InventiCloud.Utils;

public class SellingPriceCostValidation: ValidationAttribute
    {
        private string _sellingpriceProperty { get; set; }
        private string _costperitemProperty { get; set; }

        public SellingPriceCostValidation(string costperitemPropertyName, string sellingpricePropertyName)
        {
            _sellingpriceProperty = sellingpricePropertyName;
            _costperitemProperty = costperitemPropertyName;
        }

        protected override ValidationResult? IsValid(object value, ValidationContext validationContext)
        {
            var sellingpriceProperty = validationContext.ObjectType.GetProperty(_sellingpriceProperty);
            var costperitemProperty = validationContext.ObjectType.GetProperty(_costperitemProperty);

            if (sellingpriceProperty == null || costperitemProperty == null)
            {
                return new ValidationResult("Properties not found."); // Handle invalid property names
            }

            var amountA = (decimal)sellingpriceProperty.GetValue(validationContext.ObjectInstance);
            var amountB = (decimal)costperitemProperty.GetValue(validationContext.ObjectInstance);
            if (amountA <= amountB)
            {
                return new ValidationResult($"Selling Price must be greater than Cost.", new[] { validationContext.MemberName });
            }

            return ValidationResult.Success;
        }

    }