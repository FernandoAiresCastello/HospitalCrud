﻿namespace HospitalCrud.DTO
{
	/// <summary>
	/// DTO class used by the JSON serializer to hold the data to update for a patient
	/// </summary>
	public record PatientUpdateDTO
	{
		public required int Id { get; init; }
		public string? FirstName { get; init; }
		public string? LastName { get; init; }
		public string? Email { get; init; }
		public string? Phone { get; init; }
		public DateTime? DateOfBirth { get; init; }
		public string? Cpf { get; init; }
	}
}
