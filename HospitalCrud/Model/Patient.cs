using HospitalCrud.JSONConverters;
using HospitalCrud.Util;
using HospitalCrud.Validators;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace HospitalCrud.Model
{
	/// <summary>
	/// Model for a patient in the hospital
	/// </summary>
	public class Patient
	{
		public const string UniqueCpfConstraint = "IX_Patient_Cpf";

		[Key]
		public int? Id { get; set; }

		[Required(ErrorMessage = ValidationMessages.RequiredFirstName)]
		[StringLength(50)]
		public string FirstName { get; set; } = "";

		[Required(ErrorMessage = ValidationMessages.RequiredLastName)]
		[StringLength(50)]
		public string LastName { get; set; } = "";

        [NotMapped]
		[JsonIgnore]
        public string FullName => $"{FirstName} {LastName}";

		[Required(ErrorMessage = ValidationMessages.RequiredCpf)]
		[CpfNumber]
		public string? Cpf { get; set; } = null;

		[EmailAddress(ErrorMessage = ValidationMessages.InvalidEmail)]
		[StringLength(50)]
		public string? Email { get; set; } = null;

		[BrazilianPhoneNumber]
		[StringLength(20)]
		public string? Phone { get; set; } = null;

		[DateOfBirth]
        [JsonConverter(typeof(DateTimeConverter))]
        public DateTime? DateOfBirth { get; set; } = null;
    }
}
