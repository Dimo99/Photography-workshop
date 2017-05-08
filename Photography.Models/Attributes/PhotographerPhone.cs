using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Photography.Models.Attributes
{
    public class PhotographerPhone : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value == null)
            {
                return true;
            }
            string phone = (string) value;
            string pattern = @"^\+[\d]{1,3}\/[\d]{8,10}$";
            Regex regex = new Regex(pattern);
            if (regex.IsMatch(phone))
            {
                return true;
            }
            return false;
        }
    }
}