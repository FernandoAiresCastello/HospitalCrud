using HospitalCrud.Data;
using HospitalCrud.DTO;
using HospitalCrud.Exceptions;
using HospitalCrud.Mappers;
using HospitalCrud.Model;
using HospitalCrud.Repositories;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace HospitalCrud.Services
{
    public class PatientService : IPatientService
	{
		private readonly IPatientRepository patientRepository;
		private readonly IPatientMapper patientMapper;

		public PatientService(IPatientRepository patientRepository, IPatientMapper patientMapper)
		{
			this.patientRepository = patientRepository;
			this.patientMapper = patientMapper;
		}

		public async Task<Patient> AddNewPatient(PatientDTO dto)
		{
			try
			{
				var patient = patientMapper.MapToPatient(dto);
				await patientRepository.Add(patient);
				return patient;
			}
			catch (DbUpdateException ex)
			{
				if (ex.InnerException is PostgresException pgEx && pgEx.SqlState == "23505") // 23505: Unique violation error code
				{
					if (pgEx.ConstraintName == "IX_Patient_Cpf")
					{
						throw new DuplicateCpfException(dto.Cpf);
					}
				}

				throw;
			}
		}

		public async Task<ICollection<Patient>> GetAllPatients()
		{
			return await patientRepository.GetAll();
		}

		public async Task<ICollection<PatientDTO>> GetAllPatientsDto()
		{
			var patients = await GetAllPatients();
			return patientMapper.MapToPatientDtoCollection(patients);
		}

		public async Task<Patient> GetPatientById(int id)
		{
			return await patientRepository.GetById(id);
		}

		public async Task<PatientDTO> GetPatientDtoById(int id)
		{
			var patient = await GetPatientById(id);
			return patientMapper.MapToPatientDto(patient);
		}

		public async Task<PatientDTO> GetPatientDtoByCpf(string cpf)
		{
			var patient = await patientRepository.GetPatientByCpf(cpf);
			return patientMapper.MapToPatientDto(patient);
		}

		public async Task UpdatePatient(PatientUpdateDTO dto)
		{
			var patient = await GetPatientById(dto.Id);

			if (dto.FirstName != null)
				patient.FirstName = dto.FirstName;
			if (dto.LastName != null)
				patient.LastName = dto.LastName;
			if (dto.Phone != null)
				patient.Phone = dto.Phone;
			if (dto.Email != null)
				patient.Email = dto.Email;
			if (dto.DateOfBirth != null)
				patient.DateOfBirth = dto.DateOfBirth;
			if (dto.Cpf != null)
				patient.Cpf = dto.Cpf;

			await patientRepository.Update(patient);
		}

		public async Task DeletePatientById(int id)
		{
			await patientRepository.Delete(id);
		}
	}
}
