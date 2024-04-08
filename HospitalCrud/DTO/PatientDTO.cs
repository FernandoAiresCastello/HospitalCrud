namespace HospitalCrud.DTO
{
	public record PatientDTO
	{
		public int? Id { get; init; } = null;
		public required string FirstName { get; init; }
		public required string LastName { get; init; }
		public required string Cpf { get; init; }
		public string? Email { get; init; } = null;
		public string? Phone { get; init; } = null;
		public DateTime? DateOfBirth { get; init; } = null;
	}
}
