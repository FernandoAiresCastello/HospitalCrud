using HospitalCrud.JSONConverters;
using HospitalCrud.Model;
using HospitalCrud.Util;
using HospitalCrud.Validators;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace HospitalCrud.DTO
{
	/// <summary>
	/// Used by the JSON serializer during API calls to pass the data to update a <see cref="Patient"/>
	/// </summary>
	public record PatientUpdateDTO
	{
		[Required(ErrorMessage = ValidationMessages.RequiredId)]
		public int? Id { get; init; } = null;

		[StringLength(50)]
		public string? FirstName { get; init; }

		[StringLength(50)]
		public string? LastName { get; init; }

		[EmailAddress(ErrorMessage = ValidationMessages.InvalidEmail)]
		public string? Email { get; init; }

		[CpfNumber]
		public string? Cpf { get; init; }

		[BrazilianPhoneNumber]
		public string? Phone { get; init; }

		[DateOfBirth]
		[JsonConverter(typeof(DateTimeConverter))]
		public DateTime? DateOfBirth { get; init; }
	}
}
