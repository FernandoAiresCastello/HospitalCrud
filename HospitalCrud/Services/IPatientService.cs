using HospitalCrud.DTO;
using HospitalCrud.Model;

namespace HospitalCrud.Services
{
	/// <summary>
	/// Interface for a service that handles patients
	/// </summary>
	public interface IPatientService
	{
		Task<Patient> AddNewPatient(PatientDTO dto);
		Task<ICollection<Patient>> GetAllPatients();
		Task<ICollection<PatientDTO>> GetAllPatientsDto();
		Task<Patient> GetPatientById(int id);
		Task<PatientDTO> GetPatientDtoById(int id);
		Task<PatientDTO> GetPatientDtoByCpf(string cpf);
		Task UpdatePatient(PatientUpdateDTO dto);
		Task DeletePatientById(int id);
	}
}
