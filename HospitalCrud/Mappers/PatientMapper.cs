using HospitalCrud.DTO;
using HospitalCrud.Model;

namespace HospitalCrud.Mappers
{
	/// <summary>
	/// The mapper that maps Patient to PatientDTO and vice-versa
	/// </summary>
	public class PatientMapper : IPatientMapper
	{
		public Patient MapToPatient(PatientDTO dto)
		{
			return new Patient
			{
				FirstName = dto.FirstName,
				LastName = dto.LastName,
				DateOfBirth = dto.DateOfBirth,
				Cpf = dto.Cpf,
				Email = dto.Email,
				Phone = dto.Phone
			};
		}

		public PatientDTO MapToPatientDto(Patient patient)
		{
			return new PatientDTO
			{
				Id = patient.Id,
				FirstName = patient.FirstName,
				LastName = patient.LastName,
				DateOfBirth = patient.DateOfBirth,
				Cpf = patient.Cpf ?? "",
				Email = patient.Email,
				Phone = patient.Phone
			};
		}

		public ICollection<PatientDTO> MapToPatientDtoCollection(ICollection<Patient> patients)
		{
			return (ICollection<PatientDTO>)patients.Select(MapToPatientDto);
		}
	}
}
