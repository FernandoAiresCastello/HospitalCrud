using HospitalCrud.JSONConverters;
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
		[Key]
		public int? Id { get; set; }

		[Required(ErrorMessage = "Nome é obrigatório")]
		[StringLength(50)]
		public string FirstName { get; set; } = "";

		[Required(ErrorMessage = "Sobrenome é obrigatório")]
		[StringLength(50)]
		public string LastName { get; set; } = "";

        [NotMapped]
		[JsonIgnore]
        public string FullName => $"{FirstName} {LastName}";

        [Required(ErrorMessage = "CPF é obrigatório")]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "Formato de CPF inválido. Necessário exatamente 11 dígitos.")]
        public string? Cpf { get; set; } = null;

		[EmailAddress(ErrorMessage = "Endereço de e-mail inválido")]
		[StringLength(50)]
		public string? Email { get; set; } = null;

		[RegularExpression(@"^(\+55\s?)?(\(?\d{2}\)?\s?)?(\d{4,5}-?\d{4})$", ErrorMessage = "Formato de telefone inválido")]
		[StringLength(20)]
		public string? Phone { get; set; } = null;

        [JsonConverter(typeof(DateTimeConverter))]
        public DateTime? DateOfBirth { get; set; } = null;
    }
}
