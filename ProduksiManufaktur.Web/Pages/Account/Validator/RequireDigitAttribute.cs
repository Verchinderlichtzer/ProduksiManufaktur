using System.ComponentModel.DataAnnotations;

namespace ProduksiManufaktur.Web.Pages.Account.Validator
{
    [AttributeUsage(AttributeTargets.All, AllowMultiple = false)]
    public class RequireDigitAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            string? x = value?.ToString();
            return x?.Any(char.IsDigit) == true && x?.Any(char.IsUpper) == true;
        }
    }
}