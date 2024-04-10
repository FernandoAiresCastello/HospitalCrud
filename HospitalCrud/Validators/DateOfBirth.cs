using HospitalCrud.Util;
using System.ComponentModel.DataAnnotations;

namespace HospitalCrud.Validators
{
	/// <summary>
	/// Validator for a DateTime that should represent a date of birth
	/// </summary>
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
	public sealed class DateOfBirth : ValidationAttribute
	{
		public DateOfBirth() : base(ValidationMessages.InvalidDateOfBirth)
		{
		}

		public override bool IsValid(object value)
		{
			if (value == null)
				return true;

			if (value is DateTime dateOfBirth)
			{
				DateTime date = dateOfBirth.Date;
				return date != DateTime.MinValue.Date && date < DateTime.Now.Date;
			}

			return false;
		}
	}
}
