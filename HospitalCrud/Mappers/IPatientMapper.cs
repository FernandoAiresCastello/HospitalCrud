using HospitalCrud.DTO;
using HospitalCrud.Model;

namespace HospitalCrud.Mappers
{
	/// <summary>
	/// The interface for a mapper that maps Patient to PatientDTO and vice-versa
	/// </summary>
	public interface IPatientMapper
	{
		Patient MapToPatient(PatientDTO dto);
		PatientDTO MapToPatientDto(Patient patient);
		ICollection<PatientDTO> MapToPatientDtoCollection(ICollection<Patient> patients);
	}
}
