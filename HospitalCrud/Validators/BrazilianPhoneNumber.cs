using HospitalCrud.Util;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace HospitalCrud.Validators
{
	/// <summary>
	/// Validator for a Brazilian telephone number
	/// </summary>
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
	public sealed class BrazilianPhoneNumber : ValidationAttribute
	{
		public BrazilianPhoneNumber() : base(ValidationMessages.InvalidPhone)
		{
		}

		/// <summary>
		/// 
		/// Validates phone number formats commonly used in Brazil
		/// 
		/// Valid examples:
		///		988020412
		///		48988020412
		///		48 98802 0412
		///		(48)98802-0412
		///		+55 (48) 98802-0412
		///		
		/// </summary>
		public override bool IsValid(object value)
		{
			if (value == null)
				return true;

			if (value is string phone)
			{
				phone = phone.Trim();
				if (phone == "")
					return true;

				return Regex.IsMatch(phone, @"^(\+55\s?)?(\(?\d{2}\)?\s?)?(\d{4,5}-?\d{4})$");
			}

			return false;
		}
	}
}
