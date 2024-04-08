using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalCrud.Model
{
	/// <summary>
	/// Model for a patient in the hospital
	/// </summary>
	public class Patient
	{
		[Key]
		public int Id { get; set; }

		[Required]
		[StringLength(50)]
		public string FirstName { get; set; } = "";

		[Required]
		[StringLength(50)]
		public string LastName { get; set; } = "";

		[NotMapped]
		public string FullName => $"{FirstName} {LastName}";

		[EmailAddress]
		[StringLength(50)]
		public string? Email { get; set; } = null;

		[StringLength(20)]
		public string? Phone { get; set; } = null;

		public DateTime? DateOfBirth { get; set; } = null;

		[Required]
		[StringLength(15)]
		public string? Cpf { get; set; } = null;
	}
}
